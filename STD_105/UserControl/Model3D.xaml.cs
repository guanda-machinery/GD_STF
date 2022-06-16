using devDept.Eyeshot;
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
using Space;
using Space.Attribute;
using Space.Model;

namespace STD_105
{
    /// <summary>
    /// Model3D.xaml 的互動邏輯
    /// </summary>
    public partial class Model3D : BaseUserControl<ModelVM>
    {
        public Model3D()
        {
            InitializeComponent();
            model.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            model.Renderer = rendererType.NativeExperimental; //使用 WPF本機Direct3D渲染器，無需偵聽
       
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SteelAttr steelAttr = new SteelAttr()
            {
                H = 588,
                W = 300,
                t1 = 12,
                t2 = 20,
            };

            Steel steel = new Steel();
            this.ViewModel.EntityList.Add(Steel.GetSteel(steelAttr));
        }
    }
}
