using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace STD_105
{
    /// <summary>
    /// SaveReduction.xaml 的互動邏輯
    /// </summary>
    public partial class SaveReduction : UserControl
    {

        /// <summary>
        /// 存檔命令
        /// </summary>
        public ICommand Save
        {
            get { return (ICommand)GetValue(SaveProperty); }
            set { SetValue(SaveProperty, value); }
        }

        /// <summary>
        /// <see cref="Save"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty SaveProperty =
            DependencyProperty.Register(nameof(Save), typeof(ICommand), typeof(SaveReduction), new PropertyMetadata(SavePropertyChange));
        /// <summary>
        /// <see cref="Save"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SavePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SaveReduction saveReduction = (SaveReduction)d;
            saveReduction.save.Command = (ICommand)e.NewValue;
        }


        /// <summary>
        /// 存檔命令
        /// </summary>
        public ICommand Reduction
        {
            get { return (ICommand)GetValue(ReductionProperty); }
            set { SetValue(ReductionProperty, value); }
        }
        /// <summary>
        /// <see cref="Save"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ReductionProperty =
            DependencyProperty.Register(nameof(Reduction), typeof(ICommand), typeof(SaveReduction), new PropertyMetadata(ReductionPropertyChange));
        /// <summary>
        /// <see cref="Reduction"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReductionPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SaveReduction saveReduction = (SaveReduction)d;
            saveReduction.reduction.Command = (ICommand)e.NewValue;
        }

        public SaveReduction()
        {
            InitializeComponent();
        }
    }
}
