using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;

namespace WPFSTD105
{
    /// <summary>
    /// 開啟資料夾的ViewModel
    /// </summary>
    [POCOViewModel]
    public class FolderBrowserDialogViewModel
    {
        /// <summary>
        /// 視窗上方說明
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 是否顯示新增資料夾按鈕
        /// </summary>
        public virtual bool ShowNewFolderButton { get; set; }
        /// <summary>
        /// 路徑選擇的結果
        /// </summary>
        public virtual string ResultPath { get; set; } = Properties.SofSetting.Default.LoadPath;
        /// <summary>
        /// 呼叫開啟視窗的服務
        /// </summary>
        protected virtual IFolderBrowserDialogService FolderBrowserDialogService { get { return this.GetService<IFolderBrowserDialogService>(); } }
        /// <summary>
        /// 視窗開啟命令
        /// </summary>
        public virtual void ShowFolderDialog()
        {
            FolderBrowserDialogService.StartPath = Properties.SofSetting.Default.LoadPath;
            if (FolderBrowserDialogService.ShowDialog())
                ResultPath = FolderBrowserDialogService.ResultPath;
        }
    }
}
