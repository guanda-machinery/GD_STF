using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Enum;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WPFWindowsBase;

namespace STD_105
{
    public class ErrorCodeToAlarmConverter : BaseValueConverter<ErrorCodeToAlarmConverter>
    {
        /// <summary>
        /// 警報發送執行緒
        /// </summary>
        private static Thread WorkItem = new Thread(new ThreadStart(SendAlarm));
        /// <summary>
        /// 定時器
        /// </summary>
        //DispatcherTimer _blinkTimer = new DispatcherTimer();
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ERROR_CODE error = (ERROR_CODE)value;

            //_blinkTimer.Interval = TimeSpan.FromSeconds(0.1);
            //_blinkTimer.Tick += ImgOpacity;

            if (error == ERROR_CODE.Null)
            {
                WorkItem = new Thread(new ThreadStart(SendAlarm));
                //_blinkTimer.Stop();
                return Application.Current.Resources["NoAlarm"] as ImageSource;
            }
            else
            {
                if (WorkItem.ThreadState != ThreadState.Stopped && error != ERROR_CODE.Unknown)
                {
                    if (WorkItem.IsAlive)
                    {
                        WorkItem.Start();
                    }
                    //_blinkTimer.Start();
                }
                return Application.Current.Resources["HaveAlarm"] as ImageSource;
            }
        }
        /// <summary>
        /// 發送警報訊息
        /// </summary>
        private static void SendAlarm()
        {
            //MessageBox.Show("請查看警報", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            WinUIMessageBox.Show(null,
            $"請查看警報",
            "通知",
            MessageBoxButton.OK,
            MessageBoxImage.Exclamation,
            MessageBoxResult.None,
            MessageBoxOptions.None,
            FloatingMode.Window);
            WPFSTD105.ViewLocator.ApplicationViewModel.CurrentPage = WPFSTD105.ApplicationPage.Alarm;
            //WorkItem.Abort();
        }
        /*
        private void ImgOpacity(object sender, EventArgs e)
        {
            MainWindow_STD main = new MainWindow_STD();
            MainFunctionButton alertButton = main.FindName("alert") as MainFunctionButton;
            alertButton.Opacity = alertButton.Opacity == 1 ? 0.75 : 1;
        }*/
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
