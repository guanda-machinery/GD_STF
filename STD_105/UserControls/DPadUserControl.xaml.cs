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


        
        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BordeChild_1_Source
        {
            get { return (UIElement)GetValue(BorderChild_1_Property); }
            set { SetValue(BorderChild_1_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_1_Property =
            DependencyProperty.Register(nameof(Border1), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(BorderChild_1_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_1_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).Border1.Child = (UIElement)e.NewValue;
        }


        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BordeChild_2_Source
        {
            get { return (UIElement)GetValue(BorderChild_2_Property); }
            set { SetValue(BorderChild_2_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_2_Property =
            DependencyProperty.Register(nameof(Border2), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(BorderChild_2_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_2_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).Border2.Child = (UIElement)e.NewValue;
        }

        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BordeChild_3_Source
        {
            get { return (UIElement)GetValue(BorderChild_3_Property); }
            set { SetValue(BorderChild_3_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_3_Property =
            DependencyProperty.Register(nameof(Border3), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(BorderChild_3_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_3_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).Border3.Child = (UIElement)e.NewValue;
        }

        /// <summary>
        /// 圖片樣式
        /// </summary>
        public UIElement BordeChild_4_Source
        {
            get { return (UIElement)GetValue(BorderChild_4_Property); }
            set { SetValue(BorderChild_4_Property, value); }
        }
        /// <summary>
        /// <see cref=""/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty BorderChild_4_Property =
            DependencyProperty.Register(nameof(Border4), typeof(UIElement), typeof(DPadUserControl), new PropertyMetadata(BorderChild_4_PropertyChanged));
        /// <summary>
        /// <see cref=""/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BorderChild_4_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DPadUserControl).Border4.Child = (UIElement)e.NewValue;
        }























    }
}
