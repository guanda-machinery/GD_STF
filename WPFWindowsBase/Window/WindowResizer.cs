using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFWindowsBase
{
    /// <summary>
    /// 窗口的停靠位置
    /// </summary>
    public enum WindowDockPosition
    {
        /// <summary>
        /// Not docked
        /// </summary>
        Undocked,
        /// <summary>
        /// Docked to the left of the screen
        /// </summary>
        Left,
        /// <summary>
        /// Docked to the right of the screen
        /// </summary>
        Right,
    }

    /// <summary>
    ///解決了樣式為Windows的問題，請參見涵蓋任務欄的<see cref =" WindowStyle.None" />
    /// </summary>
    public class WindowResizer
    {
        #region 私有方法

        /// <summary>
        /// 處理大小調整的窗口
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// 最後計算的可用屏幕尺寸
        /// </summary>
        private Rect mScreenSize = new Rect();

        /// <summary>
        /// 必須在屏幕邊緣附近檢測到窗口的邊緣距離
        /// </summary>
        private int mEdgeTolerance = 2;

        /// <summary>
        /// 用於將WPF大小轉換為屏幕像素的轉換矩陣
        /// </summary>
        private Matrix mTransformToDevice;

        /// <summary>
        /// The last screen the window was on
        /// </summary>
        private IntPtr mLastScreen;

        /// <summary>
        /// 最後已知的碼頭位置
        /// </summary>
        private WindowDockPosition mLastDock = WindowDockPosition.Undocked;

        #endregion

        #region Wind32 API 接口

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        #endregion

        #region 公用事件

        /// <summary>
        /// 停靠窗位置更改時調用
        /// </summary>
        public event Action<WindowDockPosition> WindowDockChanged = (dock) => { };

        #endregion

        #region 建構式


        /// <summary>
        /// 默認構造函數
        /// </summary>
        /// <param name="window">監視並正確最大化窗口</param>
        public WindowResizer(Window window)
        {
            mWindow = window;

            //創建轉換視覺效果（用於將WPF大小轉換為像素大小）
            GetTransform();

            //偵聽初始化設置的源
            mWindow.SourceInitialized += Window_SourceInitialized;

            // 監控邊緣對接
            mWindow.SizeChanged += Window_SizeChanged;
        }

        #endregion

        #region Initialize

        /// <summary>
        ///獲取用於將WPF大小轉換為屏幕像素的轉換對象
        /// </summary>
        private void GetTransform()
        {
            //獲取視覺資源
            var source = PresentationSource.FromVisual(mWindow);

            //將轉換重置為默認值
            mTransformToDevice = default(Matrix);

            //如果我們無法獲取來源，請忽略
            if (source == null)
                return;

            //否則，獲取新的變換對象
            mTransformToDevice = source.CompositionTarget.TransformToDevice;
        }

        /// <summary>
        /// 初始化並掛接到Windows消息泵中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            //獲取此窗口的句柄
            var handle = (new WindowInteropHelper(mWindow)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            //如果找不到 return
            if (handleSource == null)
                return;

            //了解它的Windows消息
            handleSource.AddHook(WindowProc);
        }

        #endregion

        #region 邊緣對接

        /// <summary>
        /// 監視大小變化並檢測窗口是否已停靠（Aero snap）到邊緣
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 在建立窗口變換之前我們無法找到位置
            if (mTransformToDevice == default(Matrix))
                return;

            // Get  WPF size
            var size = e.NewSize;

            // Get window 長方形
            var top = mWindow.Top;
            var left = mWindow.Left;
            var bottom = top + size.Height;
            var right = left + mWindow.Width;

            // Get window 位置/大小（以設備像素為單位）
            var windowTopLeft = mTransformToDevice.Transform(new Point(left, top));
            var windowBottomRight = mTransformToDevice.Transform(new Point(right, bottom));

            // 檢查邊緣是否對接
            var edgedTop = windowTopLeft.Y <= (mScreenSize.Top + mEdgeTolerance);
            var edgedLeft = windowTopLeft.X <= (mScreenSize.Left + mEdgeTolerance);
            var edgedBottom = windowBottomRight.Y >= (mScreenSize.Bottom - mEdgeTolerance);
            var edgedRight = windowBottomRight.X >= (mScreenSize.Right - mEdgeTolerance);

            // 獲取停靠位置
            var dock = WindowDockPosition.Undocked;

            // 左對接
            if (edgedTop && edgedBottom && edgedLeft)
                dock = WindowDockPosition.Left;
            else if (edgedTop && edgedBottom && edgedRight)
                dock = WindowDockPosition.Right;
            // None
            else
                dock = WindowDockPosition.Undocked;

            //如果基座已更改
            if (dock != mLastDock)
                //通知監聽器
                WindowDockChanged(dock);

            // 保存上一個最後位置
            mLastDock = dock;
        }

        #endregion

        #region Windows 程序

        /// <summary>
        ///偵聽此窗口的所有Windows消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                //處理窗口的GetMinMaxInfo
                case 0x0024:/* 獲取windows最小最大信息*/
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }
        #endregion

        /// <summary>
        /// 獲取此windows的最小/最大窗口大小
        /// 正確考慮windows的大小和位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // 獲取點位置以確定我們在哪個屏幕上
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            // 在光標位置0,0處獲取主監視器
            var lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            // 嘗試獲取主屏幕信息
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
                return;

            // 獲取現在當前屏幕
            var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            //如果與上一個相比有所更改，請更新轉換
            if (lCurrentScreen != mLastScreen || mTransformToDevice == default(Matrix))
                GetTransform();

            //儲存最後知道的畫面
            mLastScreen = lCurrentScreen;

            // 獲取最小/最大結構以填充信息
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            //如果是主屏幕，請使用rcWork變數
            if (lPrimaryScreen.Equals(lCurrentScreen) == true)
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
            }
            //否則就是rcMonitor值
            else
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
            }

            // Set min size
            var minSize = mTransformToDevice.Transform(new Point(mWindow.MinWidth, mWindow.MinHeight));

            lMmi.ptMinTrackSize.X = (int)minSize.X;
            lMmi.ptMinTrackSize.Y = (int)minSize.Y;

            // 儲存 new size
            mScreenSize = new Rect(lMmi.ptMaxPosition.X, lMmi.ptMaxPosition.Y, lMmi.ptMaxSize.X, lMmi.ptMaxSize.Y);

            // 最大大小，允許主機根據需要進行調整
            Marshal.StructureToPtr(lMmi, lParam, true);
        }
    }

    #region Dll Helper Structures

    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MONITORINFO' 的 XML 註解
    public class MONITORINFO
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MONITORINFO' 的 XML 註解
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.cbSize' 的 XML 註解
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.cbSize' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.rcMonitor' 的 XML 註解
        public Rectangle rcMonitor = new Rectangle();
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.rcMonitor' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.rcWork' 的 XML 註解
        public Rectangle rcWork = new Rectangle();
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.rcWork' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.dwFlags' 的 XML 註解
        public int dwFlags = 0;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MONITORINFO.dwFlags' 的 XML 註解
    }


    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle' 的 XML 註解
    public struct Rectangle
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle' 的 XML 註解
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle.Left' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle.Bottom' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle.Top' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle.Right' 的 XML 註解
        public int Left, Top, Right, Bottom;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle.Right' 的 XML 註解
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle.Top' 的 XML 註解
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle.Bottom' 的 XML 註解
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle.Left' 的 XML 註解

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Rectangle.Rectangle(int, int, int, int)' 的 XML 註解
        public Rectangle(int left, int top, int right, int bottom)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Rectangle.Rectangle(int, int, int, int)' 的 XML 註解
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO' 的 XML 註解
    public struct MINMAXINFO
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO' 的 XML 註解
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptReserved' 的 XML 註解
        public POINT ptReserved;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptReserved' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxSize' 的 XML 註解
        public POINT ptMaxSize;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxSize' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxPosition' 的 XML 註解
        public POINT ptMaxPosition;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxPosition' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMinTrackSize' 的 XML 註解
        public POINT ptMinTrackSize;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMinTrackSize' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxTrackSize' 的 XML 註解
        public POINT ptMaxTrackSize;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MINMAXINFO.ptMaxTrackSize' 的 XML 註解
    };

    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'POINT' 的 XML 註解
    public struct POINT
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'POINT' 的 XML 註解
    {
        /// <summary>
        /// x點的坐標。
        /// </summary>
        public int X;
        /// <summary>
        ///y點的坐標。
        /// </summary>
        public int Y;

        /// <summary>
        /// 構造一個坐標點（x，y）。
        /// </summary>
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    #endregion
}