using System;
using System.Runtime.InteropServices;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 選擇檔案視窗的圖片列表
    /// </summary>
    public class SelectFilesImageList
    {
        /// <summary>
        /// 四邊形的結構
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// 代表四個方向
            /// </summary>
            public int left, top, right, bottom;
        }
        /// <summary>
        /// 物件的座標
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            int x;
            int y;
        }
        /// <summary>
        /// 圖像列表繪製參數
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGELISTDRAWPARAMS
        {
            /// <summary>
            /// 大小
            /// </summary>
            public int cbSize;
            /// <summary>
            /// himl
            /// </summary>
            public IntPtr himl;
            /// <summary>
            /// i
            /// </summary>
            public int i;
            /// <summary>
            /// hdcDst
            /// </summary>
            public IntPtr hdcDst;
            /// <summary>
            /// x
            /// </summary>
            public int x;
            /// <summary>
            /// y
            /// </summary>
            public int y;
            /// <summary>
            /// cx
            /// </summary>
            public int cx;
            /// <summary>
            /// cy
            /// </summary>
            public int cy;
            /// <summary>
            /// xBitmap
            /// </summary>
            public int xBitmap;
            /// <summary>
            /// yBitmap
            /// </summary>
            public int yBitmap;
            /// <summary>
            /// rgbBk
            /// </summary>
            public int rgbBk;
            /// <summary>
            /// rgbFg
            /// </summary>
            public int rgbFg;
            /// <summary>
            /// fStyle
            /// </summary>
            public int fStyle;
            /// <summary>
            /// dwRop
            /// </summary>
            public int dwRop;
            /// <summary>
            /// fState
            /// </summary>
            public int fState;
            /// <summary>
            /// Frame
            /// </summary>
            public int Frame;
            /// <summary>
            /// crEffect
            /// </summary>
            public int crEffect;
        }
        /// <summary>
        /// 圖片資訊
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGEINFO
        {
            /// <summary>
            /// hbmImage
            /// </summary>
            public IntPtr hbmImage;
            /// <summary>
            /// hbmMask
            /// </summary>
            public IntPtr hbmMask;
            /// <summary>
            /// Unused1
            /// </summary>
            public int Unused1;
            /// <summary>
            /// Unused2
            /// </summary>
            public int Unused2;
            /// <summary>
            /// rcImage
            /// </summary>
            public RECT rcImage;
        }
        /// <summary>
        /// 圖片列表
        /// </summary>
        [ComImport()]
        [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IImageList
        {
            /// <summary>
            /// Add
            /// </summary>
            /// <param name="hbmImage"></param>
            /// <param name="hbmMask"></param>
            /// <param name="pi"></param>
            /// <returns></returns>
            [PreserveSig]
            int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);
            /// <summary>
            /// ReplaceIcon
            /// </summary>
            /// <param name="i"></param>
            /// <param name="hicon"></param>
            /// <param name="pi"></param>
            /// <returns></returns>
            [PreserveSig]
            int ReplaceIcon(int i, IntPtr hicon, ref int pi);
            /// <summary>
            /// SetOverlayImage
            /// </summary>
            /// <param name="iImage"></param>
            /// <param name="iOverlay"></param>
            /// <returns></returns>
            [PreserveSig]
            int SetOverlayImage(int iImage, int iOverlay);
            /// <summary>
            /// Replace
            /// </summary>
            /// <param name="i"></param>
            /// <param name="hbmImage"></param>
            /// <param name="hbmMask"></param>
            /// <returns></returns>
            [PreserveSig]
            int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);
            /// <summary>
            /// AddMasked
            /// </summary>
            /// <param name="hbmImage"></param>
            /// <param name="crMask"></param>
            /// <param name="pi"></param>
            /// <returns></returns>
            [PreserveSig]
            int AddMasked(IntPtr hbmImage, int crMask, ref int pi);
            /// <summary>
            /// Draw
            /// </summary>
            /// <param name="pimldp"></param>
            /// <returns></returns>
            [PreserveSig]
            int Draw(ref IMAGELISTDRAWPARAMS pimldp);
            /// <summary>
            /// Remove
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            [PreserveSig]
            int Remove(int i);
            /// <summary>
            /// GetIcon
            /// </summary>
            /// <param name="i"></param>
            /// <param name="flags"></param>
            /// <param name="picon"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetIcon(int i, int flags, ref IntPtr picon);
            /// <summary>
            /// GetImageInfo
            /// </summary>
            /// <param name="i"></param>
            /// <param name="pImageInfo"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetImageInfo(int i, ref IMAGEINFO pImageInfo);
            /// <summary>
            /// Copy
            /// </summary>
            /// <param name="iDst"></param>
            /// <param name="punkSrc"></param>
            /// <param name="iSrc"></param>
            /// <param name="uFlags"></param>
            /// <returns></returns>
            [PreserveSig]
            int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);
            /// <summary>
            /// Merge
            /// </summary>
            /// <param name="i1"></param>
            /// <param name="punk2"></param>
            /// <param name="i2"></param>
            /// <param name="dx"></param>
            /// <param name="dy"></param>
            /// <param name="riid"></param>
            /// <param name="ppv"></param>
            /// <returns></returns>
            [PreserveSig]
            int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);
            /// <summary>
            /// Clone
            /// </summary>
            /// <param name="riid"></param>
            /// <param name="ppv"></param>
            /// <returns></returns>
            [PreserveSig]
            int Clone(ref Guid riid, ref IntPtr ppv);
            /// <summary>
            /// GetImageRect
            /// </summary>
            /// <param name="i"></param>
            /// <param name="prc"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetImageRect(int i, ref RECT prc);
            /// <summary>
            /// GetIconSize
            /// </summary>
            /// <param name="cx"></param>
            /// <param name="cy"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetIconSize(ref int cx, ref int cy);
            /// <summary>
            /// SetIconSize
            /// </summary>
            /// <param name="cx"></param>
            /// <param name="cy"></param>
            /// <returns></returns>
            [PreserveSig]
            int SetIconSize(int cx, int cy);
            /// <summary>
            /// GetImageCount
            /// </summary>
            /// <param name="pi"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetImageCount(ref int pi);
            /// <summary>
            /// SetImageCount
            /// </summary>
            /// <param name="uNewCount"></param>
            /// <returns></returns>
            [PreserveSig]
            int SetImageCount(int uNewCount);
            /// <summary>
            /// SetBkColor
            /// </summary>
            /// <param name="clrBk"></param>
            /// <param name="pclr"></param>
            /// <returns></returns>
            [PreserveSig]
            int SetBkColor(int clrBk, ref int pclr);
            /// <summary>
            /// GetBkColor
            /// </summary>
            /// <param name="pclr"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetBkColor(ref int pclr);
            /// <summary>
            /// BeginDrag
            /// </summary>
            /// <param name="iTrack"></param>
            /// <param name="dxHotspot"></param>
            /// <param name="dyHotspot"></param>
            /// <returns></returns>
            [PreserveSig]
            int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);
            /// <summary>
            /// EndDrag
            /// </summary>
            /// <returns></returns>
            [PreserveSig]
            int EndDrag();
            /// <summary>
            /// DragEnter
            /// </summary>
            /// <param name="hwndLock"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            [PreserveSig]
            int DragEnter(IntPtr hwndLock, int x, int y);
            /// <summary>
            /// DragLeave
            /// </summary>
            /// <param name="hwndLock"></param>
            /// <returns></returns>
            [PreserveSig]
            int DragLeave(IntPtr hwndLock);
            /// <summary>
            /// DragMove
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            [PreserveSig]
            int DragMove(int x, int y);
            /// <summary>
            /// SetDragCursorImage
            /// </summary>
            /// <param name="punk"></param>
            /// <param name="iDrag"></param>
            /// <param name="dxHotspot"></param>
            /// <param name="dyHotspot"></param>
            /// <returns></returns>
            [PreserveSig]
            int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);
            /// <summary>
            /// DragShowNolock
            /// </summary>
            /// <param name="fShow"></param>
            /// <returns></returns>
            [PreserveSig]
            int DragShowNolock(int fShow);
            /// <summary>
            /// GetDragImage
            /// </summary>
            /// <param name="ppt"></param>
            /// <param name="pptHotspot"></param>
            /// <param name="riid"></param>
            /// <param name="ppv"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetDragImage(ref POINT ppt, ref POINT pptHotspot, ref Guid riid, ref IntPtr ppv);
            /// <summary>
            /// GetItemFlags
            /// </summary>
            /// <param name="i"></param>
            /// <param name="dwFlags"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetItemFlags(int i, ref int dwFlags);
            /// <summary>
            /// GetOverlayImage
            /// </summary>
            /// <param name="iOverlay"></param>
            /// <param name="piIndex"></param>
            /// <returns></returns>
            [PreserveSig]
            int GetOverlayImage(int iOverlay, ref int piIndex);
        };
    }
}
