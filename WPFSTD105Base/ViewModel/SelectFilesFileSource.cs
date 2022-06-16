using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 資料來源
    /// </summary>
    public class SelectFilesFileSource
    {
        static readonly Func<string, TypeElement, SizeIcon, int, SelectFilesFileSource> factory
          = ViewModelSource.Factory((string fullPath, TypeElement type, SizeIcon sizeType, int size) => new SelectFilesFileSource(fullPath, type, sizeType, size));
        /// <summary>
        /// 創造資料夾?
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="type"></param>
        /// <param name="sizeType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SelectFilesFileSource Create(string fullPath, TypeElement type, SizeIcon sizeType, int size)
        {
            return factory(fullPath, type, sizeType, size);
        }

        static byte[] folder;
        static Dictionary<string, byte[]> cache = new Dictionary<string, byte[]>();
        /// <summary>
        /// 以檔名排序
        /// </summary>
        [DependsOnProperties("FileName")]
        public char FileNameFirst
        {
            get { return char.ToUpper(FileName[0]); }
        }
        /// <summary>
        /// 檔名
        /// </summary>
        public virtual string FileName { get; set; }
        /// <summary>
        /// 圖標
        /// </summary>
        public virtual byte[] Icon { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public virtual int Size { get; set; }
        /// <summary>
        /// 完整路徑
        /// </summary>
        public string FullPath { get; private set; }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SelectFilesFileSource.Type' 的 XML 註解
        public TypeElement Type { get; private set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SelectFilesFileSource.Type' 的 XML 註解
        /// <summary>
        /// 檔案類型
        /// </summary>
        public enum TypeElement
        {
            /// <summary>
            /// 資料夾
            /// </summary>
            Folder,
            /// <summary>
            /// 檔案
            /// </summary>
            File,
            /// <summary>
            /// 驅動
            /// </summary>
            Drive
        }
        /// <summary>
        /// 選擇檔案來源
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="type"></param>
        /// <param name="sizeType"></param>
        /// <param name="size"></param>
        protected SelectFilesFileSource(string fullPath, TypeElement type, SizeIcon sizeType, int size)
        {
            Type = type;
            FullPath = fullPath;
            SetIcon(sizeType, size);
        }
        /// <summary>
        /// 重置大小
        /// </summary>
        /// <param name="sizeType"></param>
        /// <param name="sizeInt"></param>
        public void Resize(SizeIcon sizeType, int sizeInt)
        {
            SetIcon(sizeType, sizeInt);
        }

        void SetIcon(SizeIcon sizeType, int sizeInt)
        {
            Size = sizeInt;
            Size size = new Size(sizeInt, sizeInt);
            switch (Type)
            {
                case TypeElement.Folder:
                    FileName = Path.GetFileName(FullPath);
                    if (folder == null)
                        folder = SelectFilesIconManager.GetLargeIconByte(FullPath, false);
                    Icon = folder;
                    break;
                case TypeElement.File:
                    FileName = Path.GetFileName(FullPath);
                    string ext = System.IO.Path.GetExtension(FullPath);
                    if (ext == ".exe" || ext == ".lnk")
                    {
                        Icon = SelectFilesIconManager.GetLargeIconByte(FullPath, true, sizeType);
                    }
                    else if (!cache.ContainsKey(ext))
                    {
                        byte[] bi = SelectFilesIconManager.GetLargeIconByte(FullPath, true, sizeType);
                        cache.Add(ext, bi);
                        Icon = bi;
                    }
                    else
                        Icon = cache[ext];
                    break;
                case TypeElement.Drive:
                    FileName = FullPath;
                    Icon = SelectFilesIconManager.GetLargeIconByte(FullPath, false);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 清除快取
        /// </summary>
        public static void ClearCache()
        {
            cache.Clear();
            folder = null;
        }
    }
}
