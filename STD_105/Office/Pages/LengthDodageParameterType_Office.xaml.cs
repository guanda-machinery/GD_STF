using GD_STD.Data;
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

namespace STD_105.Office
{
    /// <summary>
    /// LengthDodageParameterType_Office.xaml 的互動邏輯
    /// </summary>
    public partial class LengthDodageParameterType_Office : BasePage
    {
        public LengthDodageParameterType_Office()
        {
            InitializeComponent();
            //MatchSetting match = (MatchSetting)DataContext;
            //MainLength.Text = match.MainLengths.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1} {str2}");
            //SecLength.Text = match.SecondaryLengths.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1} {str2}");
        }
    
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            MatchSetting match = (MatchSetting)DataContext;

            match.MainLengths = MainLength.Text.Split(' ').Select(el => Convert.ToDouble(el)).ToList();
            match.SecondaryLengths = SecLength.Text.Split(' ').Select(el => Convert.ToDouble(el)).ToList();
        }
    }
}
