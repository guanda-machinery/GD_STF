using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace WPFWindowsBase
{
    /// <summary>
    /// 自定義平面窗口的視圖模型
    /// </summary>
    public class WindowsViewModel : BaseViewModel
    {
        #region 命令

        /// <summary>
        /// 最小化窗口的命令
        /// </summary>
        public ICommand MinimizeCommand { get; set; }
        /// <summary>
        /// 最大化窗口的命令
        /// </summary>
        public ICommand MaximizeCommand { get; set; }
        /// <summary>
        /// 關閉窗口命令
        /// </summary>
        public ICommand CloseCommand { get; set; }
        /// <summary>
        /// 菜單窗口命令
        /// </summary>
        public ICommand MenuCommand { get; set; }
        #endregion

        #region 私用方法
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'WindowsViewModel._Window' 的 XML 註解
        protected Window _Window;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'WindowsViewModel._Window' 的 XML 註解
        /// <summary>
        /// 窗口周圍的邊距允許有陰影
        /// </summary>
        private int _OuterMarginSize = 10;
        /// <summary>
        /// 窗口邊緣的半徑
        /// </summary>
        private int _WindowRadius = 10;
        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition _DockPosition = WindowDockPosition.Undocked;
        #endregion

        #region 公用方法
        /// <summary>
        /// 最小窗口寬度
        /// </summary>
        public double WindowMinimmumWidth { get; set; } = 800;
        /// <summary>
        /// 最小窗口高度
        /// </summary>
        public double WindowMinimmumHeight { get; set; } = 800;

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'WindowsViewModel.Borderless' 的 XML 註解
        public bool Borderless { get { return (_Window.WindowState == WindowState.Maximized || _DockPosition != WindowDockPosition.Undocked); } }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'WindowsViewModel.Borderless' 的 XML 註解
        /// <summary>
        /// 窗口周圍的調整邊框大小
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }
        /// <summary>
        /// 考慮到外部邊緣，窗口周圍的調整邊框大小
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }
        /// <summary>
        /// 主窗口內部內容的填充
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);
        /// <summary>
        /// 窗口周圍的邊距允許有陰影
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }
        ///// <summary>
        ///// 目前頁面
        ///// </summary>
        //public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Menu;
        /// <summary>
        /// 窗口周圍的邊距允許有陰影
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return _Window.WindowState == WindowState.Maximized ? 0 : _OuterMarginSize;
            }
            set
            {
                _OuterMarginSize = value;
            }
        }
        /// <summary>
        /// 標題高度
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        /// <summary>
        /// 標題欄/窗口標題的高度
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }
        /// <summary>
        /// 倒圓角
        /// </summary>
        public CornerRadius WindCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// 考慮到外部邊緣，窗口周圍的調整邊框大小
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return _Window.WindowState == WindowState.Maximized ? 0 : _WindowRadius;
            }
            set
            {
                _WindowRadius = value;
            }
        }
        #endregion

        #region 建設者
        /// <summary>
        /// 默認構造函數
        /// </summary>
        public WindowsViewModel(Window window)
        {
            this._Window = window;

            _Window.StateChanged += (sender, e) =>
            {
                //為受調整大小影響的所有屬性觸發事件
                OnPropertyChanged(nameof(Borderless));
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindCornerRadius));
            };
            //創建命令
            MinimizeCommand = new RelayCommand(() => _Window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _Window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() =>
            {
                _Window.Close();
                });
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_Window, GetMousePosition()));

            var resizer = new WindowResizer(_Window);
        }
        #endregion

        #region Win32API wpf獲取鼠標的屏幕位置
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        /// <summary>
        /// 獲取滑鼠的屏幕位置
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            var positiin = Mouse.GetPosition(_Window);
            return new Point(positiin.X + _Window.Left, positiin.Y + _Window.Top);
        }
        #endregion
    }
}
