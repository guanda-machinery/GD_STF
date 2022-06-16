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
    /// Synchronization.xaml 的互動邏輯
    /// </summary>
    public partial class Synchronization : UserControl
    {
        public Synchronization()
        {
            InitializeComponent();
        }

        public ICommand SubmitButtonCommand
        {
            get { return (ICommand)GetValue(SubmitButtonCommandProperty); }
            set { SetValue(SubmitButtonCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubmitButtonCommandProperty =
            DependencyProperty.Register(nameof(SubmitButtonCommand), typeof(ICommand), typeof(Synchronization), new PropertyMetadata(DoSubmitButtonCommand));

        private static void DoSubmitButtonCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Synchronization parameter = d as Synchronization;
            Button tbk = parameter.btn_Confirm;
            tbk.Command = (ICommand)e.NewValue;
        }
        public ICommand CancelButtonCommand
        {
            get { return (ICommand)GetValue(CancelButtonCommandProperty); }
            set { SetValue(CancelButtonCommandProperty, value); }
        }
        public static readonly DependencyProperty CancelButtonCommandProperty =
            DependencyProperty.Register(nameof(CancelButtonCommand), typeof(ICommand), typeof(Synchronization), new PropertyMetadata(DoCancelButtonCommand));

        private static void DoCancelButtonCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Synchronization parameter = d as Synchronization;
            Button tbk = parameter.btn_Cancel;
            tbk.Command = (ICommand)e.NewValue;
        }
    }
}
