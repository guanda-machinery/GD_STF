using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFSTD105
{
    /// <summary>
    /// InsertPartsWin.xaml 的互動邏輯
    /// </summary>
    public partial class DMFileGeneratorScreenWindow: WPFWindowsBase.BasePage
    {
        private List<string> _materialListGridControl { get; }

        public DMFileGeneratorScreenWindow(List<string> MaterialList)
        {
            InitializeComponent();
           // _materialListGridControl = MaterialList;
        }



        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ExitWin();
        }

        private void Check_Button_Click(object sender, RoutedEventArgs e)
        {



            ExitWin();
        }

        private void ExitWin()
        {
            //關閉視窗
            Window.GetWindow(this).Close();
        }




    }
}
