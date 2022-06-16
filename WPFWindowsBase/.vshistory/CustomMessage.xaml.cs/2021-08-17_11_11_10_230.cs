using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace WPFWindowsBase
{
    /// <summary>
    /// 自訂 MessageBox 
    /// </summary>
    public partial class CustomMessage : Window
    {
        /// <summary>
        /// 計時器
        /// </summary>
        public DispatcherTimer Timer { get; private set; } = null;
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="message"></param>
        public CustomMessage(MessageVM message)
        {
            InitializeComponent();
            DataContext = message;
            if (message.AutoClose)
            {

                Timer = new DispatcherTimer();
                Timer.Interval = TimeSpan.FromSeconds(1d);
                Loaded += Message_Loaded;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Close();
        }
        private void btn_Refuse_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageVM message = (MessageVM)this.DataContext;
            message.Result = MessageBoxResult.Yes;
            Close();
        }

        private void btn_Confirm_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageVM message = (MessageVM)this.DataContext;
            message.Result = MessageBoxResult.No;
            Close();
        }

        private void Message_Loaded(object sender, RoutedEventArgs e)
        {
            if (Timer != null)
            {
                Timer.Tick += (s, ex) =>
                {
                    if (((MessageVM)DataContext).CloseSecond > 0)
                    {
                        ((MessageVM)DataContext).CloseSecond -= 1;
                    }
                    else
                    {
                        Timer.Stop();
                        ((MessageVM)DataContext).Result = MessageBoxResult.No;
                        this.Close();
                    }
                };
                Timer.Start();
            }
        }
        /// <summary>
        ///  顯示訊息方塊具有訊息、 標題列標題、 按鈕和圖示。且接受預設的訊息方塊結果，符合指定的選項，並傳回結果。
        /// </summary>
        /// <param name="VM"><see cref="CustomMessage"/> 參數</param>
        /// <returns></returns>
        public static MessageBoxResult Show(MessageVM VM)
        {
            CustomMessage message = new CustomMessage(VM);
            message.ShowDialog();
            return ((MessageVM)message.DataContext).Result;
        }

        private void Message_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
            }
        }
    }
}
