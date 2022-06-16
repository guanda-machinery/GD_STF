namespace WPFWindowsBase
{
    /// <summary>
    /// 應用程序狀態作為視圖模型
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// 應用程序目前頁面
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;
        /// <summary>
        /// 隱藏次要物件
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;
        /// <summary>
        /// 軟體說明內容
        /// </summary>
        public string ExplanationContent { get; set; } = "說明內容";
        /// <summary>
        /// 可見軟體開始按鈕
        /// </summary>
        public bool StartButtonVisible { get; set; } = false;
        /// <summary>
        /// 可見更新按鈕
        /// </summary>
        public bool UpdatedButtonVisible { get; set; } = false;
        /// <summary>
        /// 說明標題
        /// </summary>
        public string ExplanationTitle { get; set; } = "說明標題";
    }
}
