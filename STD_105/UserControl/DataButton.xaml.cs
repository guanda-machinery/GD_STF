using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace STD_105
{
    /// <summary>
    /// 新增,修改,刪除
    /// </summary>
    public partial class DataButton : UserControl
    {
        public DataButton()
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
            DependencyProperty.Register(nameof(Delete), typeof(ICommand), typeof(DataButton), new PropertyMetadata(DeletePropertyChange));
        /// <summary>
        /// <see cref="delete"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DeletePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
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
            DependencyProperty.Register(nameof(Add), typeof(ICommand), typeof(DataButton), new PropertyMetadata(AddPropertyChange));
        /// <summary>
        /// <see cref="Add"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void AddPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
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
            DependencyProperty.Register(nameof(Modify), typeof(ICommand), typeof(DataButton), new PropertyMetadata(ModifyPropertyChange));
        /// <summary>
        /// <see cref="Modify"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ModifyPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
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
            DependencyProperty.Register(nameof(DeleteVisibility), typeof(Visibility), typeof(DataButton), new PropertyMetadata(DeleteVisibilityPropertyChange));
        /// <summary>
        /// <see cref="DeleteVisibility"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DeleteVisibilityPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
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
            DependencyProperty.Register(nameof(ReadVisibility), typeof(Visibility), typeof(DataButton), new PropertyMetadata(ReadVisibilityPropertyChange));
        /// <summary>
        /// <see cref="ReadVisibility"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReadVisibilityPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
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
            DependencyProperty.Register(nameof(Read), typeof(ICommand), typeof(DataButton), new PropertyMetadata(ReadPropertyChange));
        /// <summary>
        /// <see cref="Read"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReadPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataButton _ = (DataButton)d;
            _.read.Command = (ICommand)e.NewValue;
        }
    }
}
