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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// DPadUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class DPadUserControl :  UserControl
    {
        public DPadUserControl()
        {
            InitializeComponent();
        }


        #region 十字鍵路由
        /// <summary>
        /// 滑鼠指標在十字鍵向上元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickUpPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此十字鍵向上元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickUpPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於十字鍵向上按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickUpMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在十字鍵向下元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickDownPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此十字鍵向下元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickDownPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於十字鍵向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickDownMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在十字鍵向左元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此十字鍵向左元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於十字鍵向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickLeftMouseDoubleClickEvent;
        /// <summary>
        /// 滑鼠指標在十字鍵向右元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickRightPreviewMouseLeftButtonDownEvent;
        /// <summary>
        /// 滑鼠指標在此十字鍵向右元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public static readonly RoutedEvent JoystickRightPreviewMouseLeftButtonUpEvent;
        /// <summary>
        /// 發生於十字鍵向下按兩下或更多下滑鼠按鈕時。
        /// </summary>
        public static readonly RoutedEvent JoystickRightMouseDoubleClickEvent;
        #endregion

        #region 十字鍵向上按鈕路由事件註冊
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

        #region 十字鍵向下按鈕路由事件註冊
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

        #region 十字鍵向左按鈕路由事件註冊
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

        #region 十字鍵向右路由事件註冊
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



        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement ButtonContent_1_Source
        {
            get { return (UIElement)GetValue(ButtonContent_1_Property); }
            set { SetValue(ButtonContent_1_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonContent_1_Property =
            DependencyProperty.Register(nameof(DPad_Button1), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(ButtonContent_1_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonContent_1_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).DPad_Button1.Content = (UIElement)e.NewValue;
        }


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement ButtonContent_2_Source
        {
            get { return (UIElement)GetValue(ButtonContent_2_Property); }
            set { SetValue(ButtonContent_2_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonContent_2_Property =
            DependencyProperty.Register("DPad_Button2Content", typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(ButtonContent_2_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonContent_2_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).DPad_Button2.Content = (UIElement)e.NewValue;
        }


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement ButtonContent_3_Source
        {
            get { return (UIElement)GetValue(ButtonContent_3_Property); }
            set { SetValue(ButtonContent_3_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonContent_3_Property =
            DependencyProperty.Register(nameof(DPad_Button3), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(ButtonContent_3_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonContent_3_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).DPad_Button3.Content = (UIElement)e.NewValue;
        }

        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement ButtonContent_4_Source
        {
            get { return (UIElement)GetValue(ButtonContent_4_Property); }
            set { SetValue(ButtonContent_4_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonContent_4_Property =
            DependencyProperty.Register(nameof(DPad_Button4), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(ButtonContent_4_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonContent_4_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).DPad_Button1.Content = (UIElement)e.NewValue;
        }























    }
}
