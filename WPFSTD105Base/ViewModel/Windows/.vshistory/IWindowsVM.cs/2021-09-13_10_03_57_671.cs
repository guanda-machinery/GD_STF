using System.Windows.Input;

namespace WPFSTD105
{
    public interface IWindowsVM
    {
        /// <summary>
        /// 返回首頁命令
        /// </summary>
        ICommand HomeCommand { get; set; }
        /// <summary>
        /// 匯入檔案命令 
        /// </summary>
        ICommand ImportFileCommand { get; set; }
        /// <summary>
        /// 專案修改命令 
        /// </summary>
        ICommand ModifyProjectCommand { get; set; }
        /// <summary>
        /// 修改專案屬性
        /// </summary>
        ICommand ModifyProjectInfoCommand { get; set; }
        /// <summary>
        /// 開啟專案命令
        /// </summary>
        ICommand OpenProjectCommand { get; set; }
        /// <summary>
        /// 新建專案命令
        /// </summary>
        ICommand OutProjectNameCommand { get; set; }
        /// <summary>
        /// 開啟另存專案視窗命令
        /// </summary>
        ICommand SaveAsProjectCommand { get; set; }
        /// <summary>
        /// 另存專案
        /// </summary>
        bool SaveAsProjectControl { get; set; }
        /// <summary>
        /// 參數設定命令
        /// </summary>
        ICommand SettingParCommand { get; set; }
        /// <summary>
        /// 軟體設定命令
        /// </summary>
        ICommand SoftwareSettingsCommand { get; set; }
        /// <summary>
        /// 排版設定命令
        /// </summary>
        ICommand TypeSettingCommand { get; set; }
        /// <summary>
        /// 專案瀏覽命令 
        /// </summary>
        ICommand WatchProjectCommand { get; set; }
    }
}