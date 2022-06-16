using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace StringToPathGeometry
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string GetTextPath(string word, string fontFamily, int fontSize)
        {
            Typeface typeface = new Typeface(new FontFamily(fontFamily), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            return GetTextPath(word, typeface, fontSize);
        }

        public string GetTextPath(string word, Typeface typeface, int fontSize)
        {
            FormattedText text = new FormattedText(word,
            new System.Globalization.CultureInfo("en-tw"),
            FlowDirection.LeftToRight, typeface, fontSize,
            Brushes.Black);

            Geometry geo = text.BuildGeometry(new Point(0, 0));
            PathGeometry path = geo.GetFlattenedPathGeometry();

            return path.ToString();
        }
        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                path.Data = Geometry.Parse(GetTextPath(textBox.Text, textBox.FontFamily.ToString(), (int)(96 / 2.54)*3));
            }
        }
    }
}
