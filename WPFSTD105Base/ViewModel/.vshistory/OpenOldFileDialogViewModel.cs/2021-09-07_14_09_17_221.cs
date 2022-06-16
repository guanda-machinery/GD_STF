using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;
using DevExpress.Mvvm.DataAnnotations;

namespace WPFSTD105
{
    /// <summary>
    /// 開啟舊檔資料夾的ViewModel
    /// </summary>
    [POCOViewModel]
    public class OpenOldFileDialogViewModel : FolderBrowserDialogViewModel
    {
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
