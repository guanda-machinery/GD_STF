using System.Windows.Input;

namespace WPFSTD105
{
    /// <summary>
    /// 共用命令
    /// </summary>
    public interface IWindowsVM 
    {
        /// <summary>
        /// 語言命令
        /// </summary>
        ICommand LanguageCommand { get; set; }
        /// <summary>
        /// 返回首頁命令
        /// </summary>
        ICommand HomeCommand { get; set; }
        /// <summary>
        /// 匯入檔案命令 
        /// </summary>
        ICommand ImportFileCommand { get; set; }
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
        /// 新建專案存路徑命令
        /// </summary>
        ICommand OutProjectPathCommand { get; set; }
        /// <summary>
        /// 開啟專案存路徑命令
        /// </summary>
        ICommand OpenProjectPathCommand { get; set; }
        /// <summary>
        /// 另存專案命令
        /// </summary>
        ICommand SaveAsCommand { get; set; }
        /// <summary>
        /// 其他物件顯示命令 
        /// </summary>
        ICommand NewProjectShowCommand { get; set; }
        ///// <summary>
        ///// 參數設定命令
        ///// </summary>
        //ICommand SettingParCommand { get; set; }
        ///// <summary>
        ///// 軟體設定命令
        ///// </summary>
        //ICommand SoftwareSettingsCommand { get; set; }
        ///// <summary>
        ///// 排版設定命令
        ///// </summary>
        //ICommand TypeSettingCommand { get; set; }
        /// <summary>
        /// 專案瀏覽命令 
        /// </summary>
        ICommand WatchProjectCommand { get; set; }
    }
}