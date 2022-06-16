using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;

namespace WPFWindowsBase
{
    /// <summary>
    /// 開啟舊檔資料夾的ViewModel
    /// </summary>
    [POCOViewModel]
    public class OpenOldFileDialogViewModel : FolderBrowserDialogViewModel
    {
        /// <summary>
        /// 資料夾列表
        /// </summary>
        public Action FolderList { get; set; } = null;
        /// <summary>
        /// 視窗開啟命令
        /// </summary>
        public override void ShowFolderDialog()
        {
            base.ShowFolderDialog();
            ApplicationViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(ResultPath));
        }
    }
}
