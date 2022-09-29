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
    /// 還原或存檔控制向
    /// </summary>
    public partial class SaveReductionControl : UserControl
    {
        public SaveReductionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 存檔命令
        /// </summary>
        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        /// <summary>
        /// <see cref="SaveCommand"/>  註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register(nameof(SaveCommand), typeof(ICommand), typeof(SaveReductionControl), new PropertyMetadata(SaveCommandPropertyChange));

        /// <summary>
        /// <see cref="SaveCommand"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SaveCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SaveReductionControl _ = (SaveReductionControl)d;
            _.save.ButtonCommand = (ICommand)e.NewValue;
        }


        /// <summary>
        /// 還原命令
        /// </summary>
        public ICommand ReductionCommand
        {
            get { return (ICommand)GetValue(ReductionCommandProperty); }
            set { SetValue(ReductionCommandProperty, value); }
        }

        /// <summary>
        /// <see cref="ReductionCommand"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ReductionCommandProperty =
            DependencyProperty.Register(nameof(ReductionCommand), typeof(ICommand), typeof(SaveReductionControl), new PropertyMetadata(ReductionCommandPropertyChange));
        /// <summary>
        /// <see cref="ReductionCommand"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReductionCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SaveReductionControl _ = (SaveReductionControl)d;
            _.reduction.ButtonCommand = (ICommand)e.NewValue;
        }
    }
}
