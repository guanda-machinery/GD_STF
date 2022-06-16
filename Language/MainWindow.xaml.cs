using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Language
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool LanguageFlag = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
            ResourceDictionary langRd = null;
            try
            {
                langRd = Application.LoadComponent(
                new Uri(@"tw.xaml", UriKind.Relative)) as ResourceDictionary;
            }
            catch
            {
            }

            if (!LanguageFlag)
            {
                if (langRd != null)
                {
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@"ue.xaml", UriKind.Relative)
                    };
                    LanguageFlag = true;
                }
            }
            else
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                {
                    Source = new Uri(@"tw.xaml", UriKind.Relative)
                };
                LanguageFlag = false;
            }
        }
    }
}
