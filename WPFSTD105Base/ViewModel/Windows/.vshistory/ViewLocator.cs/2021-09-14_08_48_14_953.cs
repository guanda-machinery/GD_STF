namespace WPFSTD105
{
    /// <summary>
    /// 從IoC定位視圖模型以用於Xaml文件的綁定
    /// </summary>
    public class ViewLocator
    {
        /// <summary>
        /// 定位器的單例實例
        /// </summary>
        public static ViewLocator Instance { get; private set; } = new ViewLocator();
        /// <summary>
        /// 應用程序視圖模型
        /// </summary>
        public static ApplicationVM ApplicationViewModel => IoC.Get<ApplicationVM>();
        /// <summary>
        /// 應用程序視圖模型 for Office
        /// </summary>
        public static OfficePageVM OfficeViewModel => IoC.Get<OfficePageVM>();
        /// <summary>
        /// 共通 VM
        /// </summary>
        public static IApplicationVM CommonViewModel
        {
            get
            {
                if (Properties.SofSetting.Default.OfficeMode == false)
                {
                    return ApplicationViewModel;
                }
                else
                {
                    return OfficeViewModel;
                }
            }
        }

        ///// <summary>
        ///// 應用程序視圖模型 for Popup
        ///// </summary>
        //public static PopupWindowsVM PopupViewModel => IoC.Get<PopupWindowsVM>();
    }
}
