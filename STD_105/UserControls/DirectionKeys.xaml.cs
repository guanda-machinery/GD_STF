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
    /// DirectionKeys.xaml 的互動邏輯
    /// </summary>
    public partial class DirectionKeys : UserControl
    {
        public DirectionKeys()
        {
            InitializeComponent();
        }
        ///// <summary>
        ///// 向上鈕命令
        ///// </summary>
        //public ICommand UpInvokeCommandAction
        //{
        //    get { return (ICommand)GetValue(UpButtonCommandProperty); }
        //    set { SetValue(UpButtonCommandProperty, value); }
        //}
        ///// <summary>
        ///// <see cref="UpInvokeCommandAction"/> 註冊為依賴屬性
        ///// </summary>
        //public static readonly DependencyProperty UpButtonCommandProperty =
        //    DependencyProperty.Register(nameof(UpInvokeCommandAction), typeof(ICommand), typeof(DirectionKeys), new PropertyMetadata(ButtonCommandPropertyChanged));
        ///// <summary>
        ///// <see cref="UpInvokeCommandAction"/> 變更時觸發
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="e"></param>
        //private static void ButtonCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DirectionKeys menuControl = (d as DirectionKeys);
        //    menuControl.UpButton.Command = (ICommand)e.NewValue;
        //    //menuControl.UpButton.Triggers.Add();
        //}
        ///// <summary>
        ///// 向下鈕命令
        ///// </summary>
        //public ICommand DownCommand
        //{
        //    get { return (ICommand)GetValue(DownButtonCommandProperty); }
        //    set { SetValue(DownButtonCommandProperty, value); }
        //}
        ///// <summary>
        ///// <see cref="DownCommand"/> 註冊為依賴屬性
        ///// </summary>
        //public static readonly DependencyProperty DownButtonCommandProperty =
        //    DependencyProperty.Register(nameof(DownCommand), typeof(ICommand), typeof(DirectionKeys), new PropertyMetadata(ButtonCommandPropertyChanged));
        ///// <summary>
        ///// <see cref="DownCommand"/> 變更時觸發
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="e"></param>
        //private static void ButtonCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DirectionKeys menuControl = (d as DirectionKeys);
        //    menuControl.DownButton.Command = (ICommand)e.NewValue;
        //}
    }
}
