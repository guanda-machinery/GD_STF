using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// DPadUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class JoystickUserControl : UserControl
    {
        public JoystickUserControl()
        {
            InitializeComponent();
        }
        #region 按鈕路由
        /// <summary>
        /// 滑鼠指標在上圓形按鈕元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        [Category("Behavior")]
        public static readonly System.Windows.Input.MouseButtonEventHandler CircleTopPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此上圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        [Category("Behavior")]
        public static readonly System.Windows.Input.MouseButtonEventHandler CircleTopPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 滑鼠指標在此中間圓形按鈕元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent CircleMiddlePreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此中間圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent CircleMiddlePreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 滑鼠指標在此下圓形按鈕元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent CircleBottomPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此下圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent CircleBottomPreviewMouseLeftButtonUpEvent;

        /// <summary>
        /// 滑鼠指標在此下橢圓形上按鈕元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent EllipseTopPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此下橢圓形上按鈕元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent EllipseTopPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 滑鼠指標在此下橢圓形下按鈕元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent EllipseBottomPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此下橢圓形下元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent EllipseBottomPreviewMouseLeftButtonUpEvent;
        #endregion

        #region 右上圓按鈕路由事件註冊
        /// <summary>
        /// <see cref="CircleTopPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleTopPreviewMouseLeftButtonDown
        {
            add => btn_CircleTop.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_CircleTop.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="CircleTopPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleTopPreviewMouseLeftButtonUp
        {
            add => btn_CircleTop.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_CircleTop.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 右中圓按鈕路由事件註冊
        /// <summary>
        /// <see cref="CircleMiddlePreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleMiddlePreviewMouseLeftButtonDown
        {
            add => btn_CircleMiddle.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_CircleMiddle.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="CircleMiddlePreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleMiddlePreviewMouseLeftButtonUp
        {
            add => btn_CircleMiddle.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_CircleMiddle.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 右下圓按鈕路由事件註冊
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleBottomPreviewMouseLeftButtonDown
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler CircleBottomPreviewMouseLeftButtonUp
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 左上椭圆按鈕路由事件註冊
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler EllipseTopPreviewMouseLeftButtonDown
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler EllipseTopPreviewMouseLeftButtonUp
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 左下椭圓按鈕路由事件註冊
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler EllipseBottomPreviewMouseLeftButtonDown
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="CircleBottomPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler EllipseBottomPreviewMouseLeftButtonUp
        {
            add => btn_CircleBottom.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_CircleBottom.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_1_Source
        {
            get { return (UIElement)GetValue(BorderChild_1_Property); }
            set { SetValue(BorderChild_1_Property, value); }
        }

        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_1_Property =
            DependencyProperty.Register(nameof(Joystick_Border1), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_1_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_1_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as JoystickUserControl).Joystick_Border1.Child = e.NewValue as UIElement;
        }


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_2_Source
        {
            get { return (UIElement)GetValue(BorderChild_2_Property); }
            set { SetValue(BorderChild_2_Property, value); }
        }



        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_2_Property =
            DependencyProperty.Register(nameof(Joystick_Border2), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_2_PropertyChanged));


        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_2_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is UIElement)
            {
                (d as JoystickUserControl).Joystick_Border2.Child = e.NewValue as UIElement;
            }
        }








        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_3_Source
        {
            get { return (UIElement)GetValue(BorderChild_3_Property); }
            set { SetValue(BorderChild_3_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_3_Property =
            DependencyProperty.Register(nameof(Joystick_Border3), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_3_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_3_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as JoystickUserControl).Joystick_Border3.Child = (UIElement)e.NewValue;
        }

        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_4_Source
        {
            get { return (UIElement)GetValue(BorderChild_4_Property); }
            set { SetValue(BorderChild_4_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_4_Property =
            DependencyProperty.Register(nameof(Joystick_Border4), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_4_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_4_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as JoystickUserControl).Joystick_Border4.Child = (UIElement)e.NewValue;
        }


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_5_Source
        {
            get { return (UIElement)GetValue(BorderChild_5_Property); }
            set { SetValue(BorderChild_5_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_5_Property =
            DependencyProperty.Register(nameof(Joystick_Border5), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_5_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_5_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as JoystickUserControl).Joystick_Border5.Child = (UIElement)e.NewValue;
        }











        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BorderChild_6_Source
        {
            get { return (UIElement)GetValue(BorderChild_6_Property); }
            set { SetValue(BorderChild_6_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_6_Property =
            DependencyProperty.Register(nameof(Joystick_Border6), typeof(UIElement), typeof(JoystickUserControl), new PropertyMetadata(BorderChild_6_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_6_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as JoystickUserControl).Joystick_Border6.Child = (UIElement)e.NewValue;
        }




    }
}
