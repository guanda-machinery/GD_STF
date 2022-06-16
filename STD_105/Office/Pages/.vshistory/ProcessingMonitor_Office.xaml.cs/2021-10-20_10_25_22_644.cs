using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
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
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFWindowsBase;

namespace STD_105.Office
{
    /// <summary>
    /// ProcessingMonitor_Office.xaml 的互動邏輯
    /// </summary>
    public partial class ProcessingMonitor_Office : BasePage
    {
        public ProcessingMonitor_Office()
        {
            InitializeComponent();
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");


        }

        private void model_SelectionChanged(object sender, devDept.Eyeshot.Environment.SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// 在模型內按下右鍵時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void model_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void model_Loaded(object sender, RoutedEventArgs e)
        {
            ReadFile readFile = new ReadFile($@"31c18603-88cc-47ce-8654-2a2bf0400e7e.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
            readFile.DoWork();//開始工作
            readFile.AddToScene(model);//將讀取完的檔案放入到模型
            SteelAttr steel = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            SteelAttr steelAttr = (SteelAttr)steel.Clone();
            steelAttr.Length = 13;
            Mesh mesh1 = Steel3DBlock.GetProfile(steelAttr);
            mesh1.Translate(-13, 0);
            model.Entities.Add(mesh1, System.Drawing.Color.Orange);
            double le = 4534d - steel.Length + 13;
            SteelAttr attr = (SteelAttr)steelAttr.Clone();
            attr.Length = le;
            Mesh mesh2 = Steel3DBlock.GetProfile(attr);
            mesh2.Translate(steel.Length, 0);
            model.Entities.Add(mesh2, System.Drawing.Color.Orange);
            for (int i = 0; i < model.Entities.Count; i++)
            {
                model.Entities[i].Selectable = false;
            }
            Brep brep = DevEx.GetDrill();
            brep.Rotate(Math.PI / 2, Vector3D.AxisY);
            brep.Translate(-13, 0);
            model.Entities.Add(DevEx.GetDrill());
        }
    }
}
