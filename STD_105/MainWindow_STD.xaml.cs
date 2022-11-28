using GD_STD;
using GD_STD.Enum;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.Listening;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using STD_105.Office;
using System.Globalization;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System.Windows.Threading;
using System.Windows.Media;

namespace STD_105
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow_STD : Window
    {
        /// <summary>
        /// 是滑鼠按下按鈕
        /// </summary>
        private bool IsDown { get; set; }
        /// <summary>
        /// 面板縮放狀態 true=放大 false=縮小
        /// </summary>
        private bool IsPanelZoomOut { get; set; } = true;
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;
        //private static SplashScreenManager manager = SplashScreenManager.Create(()=> new WaitIndicator(), new DXSplashScreenViewModel { });
        //private static SplashScreenManager manager = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
        private readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// 持續撈取斷電保持
        /// </summary>
        private System.Windows.Threading.DispatcherTimer BAVTTimer;
        /// <summary>
        /// 檢測Alert狀態
        /// </summary>
        private DispatcherTimer _blinkTimer = new DispatcherTimer();

        public MainWindow_STD()
        {
            InitializeComponent();
            DataContext = new WindowsVM(this);
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationViewModel.PanelListening.Mode = true;//啟用聆聽模式
            //ApplicationViewModel.HostListening.ChangeLevel(LEVEL.INFERIOR);//變更 Host 聆聽等級
            ApplicationViewModel.PhoneListening.ChangeLevel(1500); //變更聆聽秒數
            ApplicationViewModel.PhoneListening.Mode =  true;//啟用聆聽模式
            //ApplicationViewModel.AccountNumber = new AccountNumber().Deserialize(@"acc.det");
            Sub.PreviewMouseLeftButtonDown += Sub_PreviewMouseLeftButtonDown;
            Sub.PreviewMouseLeftButtonUp += Sub_PreviewMouseLeftButtonUp;
            Add.PreviewMouseLeftButtonDown += Add_PreviewMouseLeftButtonDown;
            Add.PreviewMouseLeftButtonUp += Add_PreviewMouseLeftButtonUp;

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            //if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果不是辦公室軟體
            //{
            //    ApplicationViewModel.BAVT = CodesysMemor.BAVT();
            //    //讀取斷電保持計時器，10分鐘讀一次
            //    BAVTTimer = new System.Windows.Threading.DispatcherTimer();
            //    BAVTTimer.Tick += new EventHandler(BAVT_Click);
            //    BAVTTimer.Interval = new TimeSpan(0, 10, 0);
            //    BAVTTimer.Start();
            //}
        }

        public void BAVT_Click(object sender, EventArgs e)
        {
            ApplicationViewModel.BAVT = CodesysMemor.BAVT();
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            //每秒更新時間文字
            DateTime d = DateTime.Now;
            ApplicationViewModel.GetNowDate = d.ToShortDateString();
            ApplicationViewModel.GetNowTime = d.ToString(@"HH:mm:ss");
            //每秒更新專案名稱顏色
            if (!string.IsNullOrWhiteSpace(ApplicationViewModel.ProjectName))
            {
                Brush brushOrigin = Application.Current.Resources["solidclr_Gray"] as Brush;
                Brush brushWhite = Application.Current.Resources["solidclr_SilverGray"] as Brush;
                if (projectName.Foreground == brushOrigin)
                {
                    //tbk_ProjectNow.Foreground = brushWhite;
                    projectName.Foreground = brushWhite;
                }
                else
                {
                    //tbk_ProjectNow.Foreground = brushOrigin;
                    projectName.Foreground = brushOrigin;
                }
            }
            //當警報響起時做動
            if (alert.ButtonImageSource == this.FindResource("HaveAlarm") as ImageSource)
            {
                if (alert.Opacity == 1)
                {
                    alert.Opacity = 0.5;
                }
                else
                {
                    alert.Opacity = 1;
                }
            }
        }
        /// <summary>
        /// 關閉視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppWindows_Closing(object sender, CancelEventArgs e)
        {
            //e.Cancel = true;
            //if (ReadCodesysMemor.GetHost().CodesysStatus == CODESYS_STATUS.RUN)
            //{
            //    e.Cancel = true;
            //    MessageBox.Show("加工流程執行中，請等待加工完畢，重新在操作一次。",
            //        "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            //    return;
            //}
            ApplicationViewModel.PanelListening.Mode= false;
            ApplicationViewModel.PhoneListening.Mode = false;
            Timer.Stop();
            
#if DEBUG
            //PCClose();
#endif
        }
        /// <summary>
        /// 減速按鈕按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sub_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDown = true;
            AdjustSpeed(-5);
        }
        /// <summary>
        /// 減速按鈕放開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sub_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDown = false;
        }

        /// <summary>
        /// 加速按鈕按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDown = true;
            AdjustSpeed(+5);
        }
        /// <summary>
        /// 加速按鈕放開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDown = false;
        }

        /// <summary>
        /// 調整送料手臂的速度
        /// </summary>
        /// <param name="i">疊加的速度</param>
        public void AdjustSpeed(Int16 i)
        {
            BackgroundWorker _ = new BackgroundWorker();

            _.DoWork += delegate (object sender, DoWorkEventArgs e)
            {
                Thread.Sleep(700);
                while (IsDown)
                {

                    PanelButton panelButton = ReadCodesysMemor.GetPanel();

                    if (panelButton.HandSpeed + i > 100 || panelButton.HandSpeed + i < 0)
                        return;

                    panelButton.HandSpeed += i;
                    WriteCodesysMemor.SetPanel(panelButton);
                    ApplicationViewModel.PanelButton = panelButton;

                    WPFSTD105.Properties.MecSetting.Default.HandSpeed = panelButton.HandSpeed;
                    WPFSTD105.Properties.MecSetting.Default.Save();

                    Thread.Sleep(500);
                }
            };
            _.RunWorkerAsync();
        }
        /*
        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            //if (sender != null)
            //{
            //    Slider slider = sender as Slider;
            //    PanelButton panelButton = ReadCodesysMemor.GetPanel();
            //    panelButton.HandSpeed = (Int16)slider.Value;
            //    WriteCodesysMemor.SetPanel(panelButton);
            //    ApplicationViewModel.PanelButton = panelButton;
            //}
        }

        private void slider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                Slider slider = sender as Slider;
                PanelButton panelButton = ReadCodesysMemor.GetPanel();
                panelButton.HandSpeed = (Int16)slider.Value;
                WriteCodesysMemor.SetPanel(panelButton);
                WPFSTD105.Properties.MecSetting.Default.HandSpeed = (byte)panelButton.HandSpeed;
                WPFSTD105.Properties.MecSetting.Default.Save();
                //ApplicationViewModel.PanelButton = panelButton;
            }
        }

        private void PanelZoom(object sender, RoutedEventArgs e)
        {
            if (IsPanelZoomOut)
            {
                IsPanelZoomOut = false;
                Storyboard stbShow = (Storyboard)FindResource("panelMin");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
            else
            {
                IsPanelZoomOut = true;
                Storyboard stbShow = (Storyboard)FindResource("panelMax");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
        }*/

        /*public static void ActivateLoading()
        {
            switch (WPFSTD105.Properties.SofSetting.Default.Language)
            {
                case 0:
                    manager.ViewModel.Status = "讀取中....";
                    break;
                case 1:
                    manager.ViewModel.Status = "Loading....";
                    break;
                case 2:
                    manager.ViewModel.Status = "Đang tải....";
                    break;
                case 3:
                    manager.ViewModel.Status = "กำลังโหลด....";
                    break;
                default:
                    manager.ViewModel.Status = "讀取中....";
                    break;
            }
            manager.Show();
        }

        public static void DeactivateLoading()
        {
            switch (WPFSTD105.Properties.SofSetting.Default.Language)
            {
                case 0:
                    manager.ViewModel.Status = "讀取完成";
                    break;
                case 1:
                    manager.ViewModel.Status = "Completed";
                    break;
                case 2:
                    manager.ViewModel.Status = "Đang tải xong";
                    break;
                case 3:
                    manager.ViewModel.Status = "โหลดเสร็จ";
                    break;
                default:
                    manager.ViewModel.Status = "讀取完成";
                    break;
            }
            Thread.Sleep(500);
            manager.Close();
        }
                                                             */
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
