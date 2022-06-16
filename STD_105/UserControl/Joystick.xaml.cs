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
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STD_105
{
    /// <summary>
    /// Joystick.xaml 的互動邏輯
    /// </summary>
    public partial class Joystick : UserControl
    {

        public Joystick()
        {
            InitializeComponent();
        }
        #region 搖桿路由
        /// <summary>
        /// 滑鼠指標在搖桿向上元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickUpPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此搖桿向上元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickUpPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於搖桿向上按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickUpMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在搖桿向下元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickDownPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此搖桿向下元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickDownPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於搖桿向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickDownMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在搖桿向左元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此搖桿向左元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於搖桿向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在搖桿向右元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickRightPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此搖桿向右元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickRightPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於搖桿向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickRightMouseDoubleClickEvent;
        #endregion

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

        #region 搖桿向上按鈕路由事件註冊
        /// <summary>
        /// <see cref="JoystickUpPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickUpMouseDoubleClick
        {
            add => btn_JoystickUp.AddHandler(MouseDoubleClickEvent, value);
            remove => btn_JoystickUp.RemoveHandler(MouseDoubleClickEvent, value);
        }
        /// <summary>
        /// <see cref="JoystickUpPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickUpPreviewMouseLeftButtonDown
        {
            add => btn_JoystickUp.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove => btn_JoystickUp.RemoveHandler(PreviewMouseLeftButtonDownEvent, value);
        }
        /// <summary>
        /// <see cref="JoystickUpPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickUpPreviewMouseLeftButtonUp
        {
            add => btn_JoystickUp.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_JoystickUp.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 搖桿向下按鈕路由事件註冊
        /// <summary>
        /// <see cref="JoystickDownPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickDownMouseDoubleClick
        {
            add => btn_JoystickDown.AddHandler(MouseDoubleClickEvent, value);
            remove => btn_JoystickDown.RemoveHandler(MouseDoubleClickEvent, value);
        }
        /// <summary>
        /// <see cref="JoystickDownPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickDownPreviewMouseLeftButtonDown
        {
            add => btn_JoystickDown.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_JoystickDown.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="JoystickDownPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickDownPreviewMouseLeftButtonUp
        {
            add => btn_JoystickDown.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_JoystickDown.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 搖桿向左按鈕路由事件註冊
        /// <summary>
        /// <see cref="JoystickLeftPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickLeftMouseDoubleClick
        {
            add => btn_JoystickLeft.AddHandler(MouseDoubleClickEvent, value);
            remove => btn_JoystickLeft.RemoveHandler(MouseDoubleClickEvent, value);
        }
        /// <summary>
        /// <see cref="JoystickLeftPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickLeftPreviewMouseLeftButtonDown
        {
            add => btn_JoystickLeft.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_JoystickLeft.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="JoystickLeftPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickLeftPreviewMouseLeftButtonUp
        {
            add => btn_JoystickLeft.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_JoystickLeft.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
        #endregion

        #region 搖桿向右路由事件註冊
        /// <summary>
        /// <see cref="JoystickRightPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickRightMouseDoubleClick
        {
            add => btn_JoystickRight.AddHandler(MouseDoubleClickEvent, value);
            remove => btn_JoystickRight.RemoveHandler(MouseDoubleClickEvent, value);
        }
        /// <summary>
        /// <see cref="JoystickRightPreviewMouseLeftButtonDownEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickRightPreviewMouseLeftButtonDown
        {
            add => btn_JoystickRight.AddHandler(PreviewMouseLeftButtonDownEvent, value);
            remove { btn_JoystickRight.RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }
        /// <summary>
        /// <see cref="JoystickRightPreviewMouseLeftButtonUpEvent"/> 註冊路由事件
        /// </summary>
        public event RoutedEventHandler JoystickRightPreviewMouseLeftButtonUp
        {
            add => btn_JoystickRight.AddHandler(PreviewMouseLeftButtonUpEvent, value);
            remove { btn_JoystickRight.RemoveHandler(PreviewMouseLeftButtonUpEvent, value); }
        }
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
    }
}
