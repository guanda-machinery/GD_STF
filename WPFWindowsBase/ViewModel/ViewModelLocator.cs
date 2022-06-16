namespace WPFWindowsBase
{
    /// <summary>
    /// 從IoC定位視圖模型以用於Xaml文件的綁定
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// 定位器的單例實例
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// 應用程序視圖模型
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoCApplication.Get<ApplicationViewModel>();
    }
}
