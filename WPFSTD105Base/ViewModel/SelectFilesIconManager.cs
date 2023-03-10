using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static WPFSTD105.ViewModel.SelectFilesImageList;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 檔案圖標管理
    /// </summary>
    public class SelectFilesIconManager
    {
        #region CommonWinApi

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("user32")]
        static extern int DestroyIcon(IntPtr hIcon);

        const int MAX_PATH = 260;
        const int MAX_TYPE = 80;
        /// <summary>
        /// SHFILEINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            /// <summary>
            /// SHFILEINFO
            /// </summary>
            /// <param name="b"></param>
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero;
                iIcon = 0;
                dwAttributes = 0;
                szDisplayName = "";
                szTypeName = "";
            }
            /// <summary>
            /// hIcon
            /// </summary>
            public IntPtr hIcon;
            /// <summary>
            /// iIcon
            /// </summary>
            public int iIcon;
            /// <summary>
            /// dwAttributes
            /// </summary>
            public uint dwAttributes;
            /// <summary>
            /// szDisplayName
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            /// <summary>
            /// szTypeName
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_TYPE)]
            public string szTypeName;
        }
        /// <summary>
        /// SHGFI
        /// </summary>
        [Flags]
        public enum SHGFI : int
        {
            /// <summary>
            /// Icon
            /// </summary>
            Icon = 0x000000100,
            /// <summary>
            /// DisplayName
            /// </summary>
            DisplayName = 0x000000200,
            /// <summary>
            /// TypeName
            /// </summary>
            TypeName = 0x000000400,
            /// <summary>
            /// Attributes
            /// </summary>
            Attributes = 0x000000800,
            /// <summary>
            /// IconLocation
            /// </summary>
            IconLocation = 0x000001000,
            /// <summary>
            /// ExeType
            /// </summary>
            ExeType = 0x000002000,
            /// <summary>
            /// SysIconIndex
            /// </summary>
            SysIconIndex = 0x000004000,
            /// <summary>
            /// LinkOverlay
            /// </summary>
            LinkOverlay = 0x000008000,
            /// <summary>
            /// Selected
            /// </summary>
            Selected = 0x000010000,
            /// <summary>
            /// Attr_Specified
            /// </summary>
            Attr_Specified = 0x000020000,
            /// <summary>
            /// LargeIcon
            /// </summary>
            LargeIcon = 0x000000000,
            /// <summary>
            /// SmallIcon
            /// </summary>
            SmallIcon = 0x000000001,
            /// <summary>
            /// OpenIcon
            /// </summary>
            OpenIcon = 0x000000002,
            /// <summary>
            /// ShellIconSize
            /// </summary>
            ShellIconSize = 0x000000004,
            /// <summary>
            /// PIDL
            /// </summary>
            PIDL = 0x000000008,
            /// <summary>
            /// UseFileAttributes
            /// </summary>
            UseFileAttributes = 0x000000010,
            /// <summary>
            /// AddOverlays
            /// </summary>
            AddOverlays = 0x000000020,
            /// <summary>
            /// OverlayIndex
            /// </summary>
            OverlayIndex = 0x000000040,
        }

        #endregion

        static int dpi = (int)(typeof(SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null, null));
        static double dpiCoef = (double)dpi / 76;
        static double dpiCoef2 = (double)dpi / 96;

        static BitmapSource IconSource(Icon ic, SizeIcon size, bool maybeMinIcon)
        {
            Bitmap src = GetBitmap(ic, size, maybeMinIcon);
            BitmapSource ic2 = Imaging.CreateBitmapSourceFromHBitmap(src.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ic2.Freeze();
            return ic2;
        }

        static byte[] IconByte(Icon ic, SizeIcon size, bool maybeMinIcon)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(GetBitmap(ic, size, maybeMinIcon), typeof(byte[]));
        }

        static Bitmap GetBitmap(Icon ic, SizeIcon size, bool maybeMinIcon)
        {
            Bitmap src = ic.ToBitmap();
            if (maybeMinIcon)
            {
                int sizeReal = CalcRealSize(src);
                if (sizeReal < 200)
                {
                    src = src.Clone(new Rectangle(0, 0, (int)(sizeReal * dpiCoef2), (int)(sizeReal * dpiCoef2)), src.PixelFormat);
                    switch (size)
                    {
                        case SizeIcon.ExtraLarge:
                            src = MakeBorder(new System.Drawing.Size((int)(256 * dpiCoef), (int)(256 * dpiCoef)), src, (int)(sizeReal * dpiCoef2));
                            break;
                        case SizeIcon.Large:
                            src = MakeBorder(new System.Drawing.Size((int)(128 * dpiCoef), (int)(128 * dpiCoef)), src, (int)(sizeReal * dpiCoef2));
                            break;
                        case SizeIcon.Medium:
                            src = MakeBorder(new System.Drawing.Size((int)(65 * dpiCoef), (int)(65 * dpiCoef)), src, (int)(sizeReal * dpiCoef2));
                            break;
                    }
                }
            }
            return src;
        }

        static Bitmap MakeBorder(System.Drawing.Size itemSize, Bitmap bmpSource, int sizeReal)
        {
            using (Bitmap bmpSmall = bmpSource.Clone(new Rectangle(0, 0, sizeReal, sizeReal), bmpSource.PixelFormat))
            {
                Bitmap result = new Bitmap(itemSize.Width, itemSize.Height);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result))
                {
                    g.DrawRectangle(Pens.Transparent, 0, 0, itemSize.Width - 1, itemSize.Height - 1);
                    System.Drawing.Point pt = new System.Drawing.Point(result.Width / 2 - bmpSmall.Width / 2, result.Width / 2 - bmpSmall.Width / 2);
                    g.DrawImage(bmpSmall, pt);
                }
                return result;
            }
        }

        static int CalcRealSize(Bitmap bmp)
        {
            int halfW = bmp.Width / 2;
            int halfH = bmp.Height / 2;
            int result = 0;
            for (int i = bmp.Width - 1; i > 0; i--)
                if (bmp.GetPixel(i, halfH).A != 0)
                {
                    result = i;
                    break;
                }
            if (result != 0)
            {
                for (int i = bmp.Height - 1; i > halfH; i--)
                    if (bmp.GetPixel(halfW, i).A != 0)
                    {
                        result = Math.Max(i, result);
                        break;
                    }
            }
            else
            {
                int count = Math.Min(bmp.Width, bmp.Height);
                for (int i = 48; i < bmp.Width; i++)
                    if (bmp.GetPixel(i, i).A == 0)
                    {
                        int newI = i - 1;
                        if (bmp.GetPixel(newI, i).A != 0)
                            continue;
                        if (bmp.GetPixel(i, newI).A != 0)
                            continue;
                        return i;
                    }
            }
            return result != 0 ? result : Math.Max(bmp.Width, bmp.Height);
        }

        static SHFILEINFO info = new SHFILEINFO(true);
        static uint cbFileInfo = (uint)Marshal.SizeOf(info);
        static SHGFI flags = SHGFI.Icon | SHGFI.LargeIcon;
        static Guid guil = new Guid("192B9D83-50FC-457B-90A0-2B82A8B5DAE1");
        static int flagListImage = 0x00000001 | 0x00000020;


        static Icon GetFileIcon(string path, out IntPtr hIcon)
        {
            SHGetFileInfo(path, 256, out info, cbFileInfo, flags);
            hIcon = GetJumboIcon(info.iIcon);
            Icon icon = Icon.FromHandle(hIcon);
            return icon;
        }

        static IntPtr GetJumboIcon(int iImage)
        {
            IImageList spiml = null;
            SHGetImageList(0x4, ref guil, ref spiml);
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(iImage, flagListImage, ref hIcon);
            return hIcon;
        }
        /// <summary>
        /// GetLargeIcon
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="maybeSmall"></param>
        /// <param name="sizeIcon"></param>
        /// <returns></returns>
        public static BitmapSource GetLargeIcon(string fileName, bool maybeSmall, SizeIcon sizeIcon = SizeIcon.ExtraLarge)
        {
            IntPtr hIcon;
            Icon icon = GetFileIcon(fileName, out hIcon);
            BitmapSource bs = IconSource(icon, sizeIcon, maybeSmall);
            icon.Dispose();
            DestroyIcon(hIcon);
            return bs;
        }
        /// <summary>
        /// GetLargeIconByte
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="maybeSmall"></param>
        /// <param name="sizeIcon"></param>
        /// <returns></returns>
        public static byte[] GetLargeIconByte(string fileName, bool maybeSmall, SizeIcon sizeIcon = SizeIcon.ExtraLarge)
        {
            try
            {
                IntPtr hIcon;
                Icon icon = GetFileIcon(fileName, out hIcon);
                byte[] byteIcon = IconByte(icon, sizeIcon, maybeSmall);
                icon.Dispose();
                DestroyIcon(hIcon);
                return byteIcon;
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
