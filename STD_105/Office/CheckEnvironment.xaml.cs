using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace STD_105.Office
{
    /// <summary>
    /// CheckEnvironment.xaml 的互動邏輯
    /// </summary>
    public partial class CheckEnvironment : Window
    {
        private StatusDataContext status = new StatusDataContext();
        public CheckEnvironment()
        {
            InitializeComponent();
            this.DataContext = status;
            Page_Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Page_Load()
        {
            /*
            progress.Value = 0;
            status.CurrentStatus = EnvironmentStatus.CheckingUpdate;
            while (progress.Value < 100)
            {
                await Task.Run(Method);
            }
            progress.Value = 0;
            status.CurrentStatus = EnvironmentStatus.Downloading;
            while (progress.Value < 100)
            {
                await Task.Run(Method);
            }
            status.CurrentStatus = EnvironmentStatus.Installing;
            progress.Value = 0;
            while (progress.Value < 100)
            {
                await Task.Run(Method);
            }*/
            //status.CurrentStatus = EnvironmentStatus.CheckingCodesys;
            //progress.Value = 0;
            //while (progress.Value < 100)
            //{
            //    await Task.Run(Method);
            //}
            //status.CurrentStatus = EnvironmentStatus.CheckingServer;
            //progress.Value = 0;
            //while (progress.Value < 100)
            //{
            //    await Task.Run(Method);
            //}
            //if (status.CurrentStatus == EnvironmentStatus.CheckingServer && progress.Value == 100)
            //{
            //    lab_Message.Visibility = Visibility.Visible;
            //    EnvironmentVM.IsServerOK = true;
            //}

            //if (EnvironmentVM.IsServerOK)
            //{
            //    LoginPage loginPage = new LoginPage();
            //    loginPage.Show();
            //    Close();
            //}
        }

        private void Method()
        {
            Thread.Sleep(10);
            progress.Dispatcher.Invoke(() => progress.Value += 1);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
