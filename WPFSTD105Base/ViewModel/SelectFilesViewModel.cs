using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Controls;
using DevExpress.Xpf.Core;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 選擇檔案的VM
    /// </summary>
    [POCOViewModel]
    public class SelectFilesViewModel : IChildSelector
    {
        #region Properties
        /// <summary>
        /// 根目錄
        /// </summary>
        public string Root { get; private set; }

        Stack<string> ForwardStack = new Stack<string>();
        Stack<string> BackStack = new Stack<string>();
        /// <summary>
        /// 顯示物件的大小類型
        /// </summary>
        public virtual SizeIcon SizeType { get; set; }
        /// <summary>
        /// 切換RadioButton的事件
        /// </summary>
        protected void OnSizeTypeChanged()
        {
            Resize();
            this.RaisePropertyChanged(x => x.Size);
        }
        /// <summary>
        /// 圖片像素大小
        /// </summary>
        public int Size
        {
            get
            {
                switch (SizeType)
                {
                    case SizeIcon.ExtraLarge:
                        return 256;
                    case SizeIcon.Large:
                        return 128;
                    case SizeIcon.Medium:
                        return 64;
                    default:
                        return 128;
                }
            }
        }

        string _path;
        /// <summary>
        /// 選擇物件的路徑
        /// </summary>
        public string Path
        {
            get { return _path; }
            set
            {
                if (value == null) return;
                if (value != Root && !value.EndsWith("\\"))
                    value += "\\";
                if (value != Root && !Directory.Exists(value))
                {
                    RaisePathChanged();
                    return;
                }
                if (_path != null)
                    ForwardStack.Push(_path);
                _path = value;
                OpenFolder(value);
                RaisePathChanged();
            }
        }
        /// <summary>
        /// 滑鼠讀取物件時的樣式
        /// </summary>
        public virtual bool IsLoading { get; set; }
        /// <summary>
        /// 搜尋列的文字
        /// </summary>
        public virtual string SearchText { get; set; }
        /// <summary>
        /// 資料來源
        /// </summary>
        public ObservableCollectionCore<SelectFilesFileSource> Source { get; private set; }
        /// <summary>
        /// 當前的物件
        /// </summary>
        public SelectFilesFileSource CurrentItem { get; set; }
        #endregion

        #region POCO commands      
        /// <summary>
        /// 上一頁
        /// </summary>
        public void Back()
        {
            BackStack.Push(Path);
            string tmp = ForwardStack.Pop();
            _path = tmp;
            RaisePathChanged();
            OpenFolder(tmp, false);
        }
        /// <summary>
        /// 儲存上一頁的堆疊物件
        /// </summary>
        /// <returns></returns>
        public bool CanBack()
        {
            return ForwardStack.Count > 0;
        }
        /// <summary>
        /// 下一頁
        /// </summary>
        public void Forward()
        {
            string tmp = BackStack.Pop();
            ForwardStack.Push(_path);
            _path = tmp;
            RaisePathChanged();
            OpenFolder(tmp, false);
        }
        /// <summary>
        /// 儲存下一頁的堆疊物件
        /// </summary>
        /// <returns></returns>
        public bool CanForward()
        {
            return BackStack.Count > 0;
        }
        /// <summary>
        /// 展開點選的物件
        /// </summary>
        public void Open()
        {
            SelectFilesFileSource element = CurrentItem;
            Path = element.FullPath;
        }
        /// <summary>
        /// 可以展開的物件
        /// </summary>
        /// <returns></returns>
        public bool CanOpen()
        {
            return CurrentItem != null && CurrentItem.Type != SelectFilesFileSource.TypeElement.File;
        }
        /// <summary>
        /// 回到上一層
        /// </summary>
        public void Up()
        {
            string path = Path.TrimEnd('\\');
            if (path.Length != 2)
                Path = Directory.GetParent(path).FullName;
            else
                Path = Root;
        }
        /// <summary>
        /// 儲存上一層的堆疊物件
        /// </summary>
        /// <returns></returns>
        public bool CanUp()
        {
            return Path != Root;
        }
        #endregion
        /// <summary>
        /// 開啟檔案的VM
        /// </summary>
        #region Members
        protected SelectFilesViewModel()
        {
            Root = "Root";
            Source = new ObservableCollectionCore<SelectFilesFileSource>();
            SizeType = SizeIcon.Medium;
            OpenRoot();
        }

        void OpenFolder(string path, bool clearNextStack = true)
        {
            Source.Clear();
            Source.BeginUpdate();
            try
            {
                IsLoading = true;
                if (path == Root)
                    OpenRoot();
                else
                {
                    SizeIcon sizeType = SizeType;
                    int size = Size;
                    var info = new DirectoryInfo(path);
                    if (info.Exists)
                    {
                        foreach (var item in info.EnumerateDirectories().Where(x => (x.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0))
                            Source.Add(SelectFilesFileSource.Create(item.FullName, SelectFilesFileSource.TypeElement.Folder, sizeType, size));
                        foreach (var item in info.EnumerateFiles())
                            Source.Add(SelectFilesFileSource.Create(item.FullName, SelectFilesFileSource.TypeElement.File, sizeType, size));
                    }
                }
                if (clearNextStack)
                    BackStack.Clear();
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Back();
            }
            finally
            {
                IsLoading = false;
                Source.EndUpdate();
            }
        }

        void Resize()
        {
            try
            {
                IsLoading = true;
                SelectFilesFileSource.ClearCache();
                foreach (SelectFilesFileSource item in Source)
                    item.Resize(SizeType, Size);
            }
            finally
            {
                IsLoading = false;
            }
        }

        void OpenRoot()
        {
            Source.Clear();
            foreach (var drive in DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Fixed))
                Source.Add(SelectFilesFileSource.Create(drive.RootDirectory.Name, SelectFilesFileSource.TypeElement.Drive, SizeType, Size));
            _path = Root;
            RaisePathChanged();
        }

        void RaisePathChanged()
        {
            this.RaisePropertyChanged(x => x.Path);
        }
        #endregion
        #region IChildSelector
        IEnumerable<object> IChildSelector.SelectChildren(object item)
        {
            var info = item as DirectoryInfo;
            if (info == null || !info.Exists) return null;
            try
            {
                var dirs = info.EnumerateDirectories().Where(x => (x.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0);
                return dirs;
            }
            catch
            {
                return Enumerable.Empty<FileSystemInfo>();
            }
        }
        /// <summary>
        /// 自訂的顯示文字
        /// </summary>
        /// <param name="arg"></param>
        public void CustomDisplayText(BreadcrumbCustomDisplayTextEventArgs arg)
        {
            var info = arg.Item as DirectoryInfo;
            if (info == null) return;
            arg.DisplayText = info.Name.TrimEnd('\\');
        }
        /// <summary>
        /// 路徑
        /// </summary>
        /// <param name="arg"></param>
        public void QueryPath(BreadcrumbQueryPathEventArgs arg)
        {
            if (arg.Path.Count() == 0) return;
            string argPath = arg.Path.Count() == 1 ? arg.Path.FirstOrDefault() + arg.PathSeparator : string.Join(arg.PathSeparator, arg.Path);
            var infoList = new List<DirectoryInfo>();
            var info = new DirectoryInfo(argPath);
            if (!info.Exists)
            {
                arg.Breadcrumbs = infoList;
                return;
            }
            do
            {
                infoList.Insert(0, info);
                info = info.Parent;
            } while (info != null && info.Exists);
            arg.Breadcrumbs = infoList;
        }
        #endregion
    }
    /// <summary>
    /// 檔案瀏覽的圖示大小
    /// </summary>
    public enum SizeIcon
    {
        /// <summary>
        /// 最大
        /// </summary>
        [Display(Name = "Extra large icons")]
        ExtraLarge,
        /// <summary>
        /// 大
        /// </summary>
        [Display(Name = "Large icons")]
        Large,
        /// <summary>
        /// 中等
        /// </summary>
        [Display(Name = "Medium icons")]
        Medium
    }
}
