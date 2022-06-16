using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;
using DevExpress.Mvvm.DataAnnotations;
using System;

namespace WPFSTD105
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
            if (ResultPath != string.Empty)
            {
                Properties.SofSetting.Default.LoadPath = ResultPath;
                Properties.SofSetting.Default.Save();
                CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath));
            }
        }
    }
}
