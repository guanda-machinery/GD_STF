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

namespace STD_105
{
    /// <summary>
    /// DataButtons_Office.xaml 的互動邏輯 20220822  蘇冠綸 版型更改
    /// </summary>
    public partial class DataButtons_Office : UserControl
    {
        public DataButtons_Office()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 刪除命令
        /// </summary>
        public ICommand Delete
        {
            get { return (ICommand)GetValue(DeleteProperty); }
            set { SetValue(DeleteProperty, value); }
        }
        /// <summary>
        /// <see cref="delete"/> 註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty DeleteProperty =
            DependencyProperty.Register(nameof(Delete), typeof(ICommand), typeof(DataButtons_Office), new PropertyMetadata(DeletePropertyChange));
        /// <summary>
        /// <see cref="delete"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DeletePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.delete.Command = (ICommand)e.NewValue;
        }


        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand Add
        {
            get { return (ICommand)GetValue(AddProperty); }
            set { SetValue(AddProperty, value); }
        }

        /// <summary>
        /// <see cref="Add"/>註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty AddProperty =
            DependencyProperty.Register(nameof(Add), typeof(ICommand), typeof(DataButtons_Office), new PropertyMetadata(AddPropertyChange));
        /// <summary>
        /// <see cref="Add"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void AddPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.add.Command = (ICommand)e.NewValue;
        }

        /// <summary>
        /// 修改命令
        /// </summary>
        public ICommand Modify
        {
            get { return (ICommand)GetValue(ModifyProperty); }
            set { SetValue(ModifyProperty, value); }
        }
        /// <summary>
        /// <see cref="Modify"/> 註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty ModifyProperty =
            DependencyProperty.Register(nameof(Modify), typeof(ICommand), typeof(DataButtons_Office), new PropertyMetadata(ModifyPropertyChange));
        /// <summary>
        /// <see cref="Modify"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ModifyPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.modify.Command = (ICommand)e.NewValue;
        }

        /// <summary>
        /// 刪除按鈕顯示狀態
        /// </summary>
        public Visibility DeleteVisibility
        {
            get { return (Visibility)GetValue(DeleteVisibilityProperty); }
            set { SetValue(DeleteVisibilityProperty, value); }
        }
        /// <summary>
        /// <see cref="DeleteVisibility"/> 註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty DeleteVisibilityProperty =
            DependencyProperty.Register(nameof(DeleteVisibility), typeof(Visibility), typeof(DataButtons_Office), new PropertyMetadata(DeleteVisibilityPropertyChange));
        /// <summary>
        /// <see cref="DeleteVisibility"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DeleteVisibilityPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.delete.Visibility = (Visibility)e.NewValue;
        }


        /// <summary>
        /// 讀取按鈕顯示狀態
        /// </summary>
        public Visibility ReadVisibility
        {
            get { return (Visibility)GetValue(ReadVisibilityProperty); }
            set { SetValue(ReadVisibilityProperty, value); }
        }

        /// <summary>
        /// <see cref="ReadVisibility"/> 註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty ReadVisibilityProperty =
            DependencyProperty.Register(nameof(ReadVisibility), typeof(Visibility), typeof(DataButtons_Office), new PropertyMetadata(ReadVisibilityPropertyChange));
        /// <summary>
        /// <see cref="ReadVisibility"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReadVisibilityPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.read.Visibility = (Visibility)e.NewValue;
        }

        /// <summary>
        /// 讀取按鈕命令
        /// </summary>
        public ICommand Read
        {
            get { return (ICommand)GetValue(ReadProperty); }
            set { SetValue(ReadProperty, value); }
        }

        /// <summary>
        /// <see cref="Read"/> 註冊相依屬性
        /// </summary>
        public static readonly DependencyProperty ReadProperty =
            DependencyProperty.Register(nameof(Read), typeof(ICommand), typeof(DataButtons_Office), new PropertyMetadata(ReadPropertyChange));
        /// <summary>
        /// <see cref="Read"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReadPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButtons_Office _ = (DataButtons_Office)d;
            _.read.Command = (ICommand)e.NewValue;
        }
    }
}
