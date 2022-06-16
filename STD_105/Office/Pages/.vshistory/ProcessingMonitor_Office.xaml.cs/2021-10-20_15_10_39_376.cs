using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static WPFSTD105.ViewLocator;
using static WPFSTD105.CodesysIIS;
using devDept.Eyeshot;

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

            model.Renderer = rendererType.NativeExperimental; //使用 OpenGL渲染
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
        Brep brep1;
        Brep brep2;
        Brep brep3;
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
            brep1 = DevEx.GetDrill();
            brep2 = (Brep)brep1.Clone();
            brep3 = (Brep)brep1.Clone();
            brep1.Rotate(Math.PI / 2, Vector3D.AxisZ); //右軸
            brep2.Rotate(-Math.PI / 2, Vector3D.AxisZ); //左軸
            brep3.Rotate(Math.PI / 2, Vector3D.AxisY); //中軸

            model.Entities.Add(brep1);
            model.Entities.Add(brep2);
            model.Entities.Add(brep3);
            Thread _ = new Thread(Run);
            _.Start();
        }
        public void Run()
        {
            Thread.Sleep(2000);
            //CommonViewModel.AxisInfoListening.ChangeLevel(WPFSTD105.Listening.LEVEL.HIGH);
            //CommonViewModel.AxisInfoListening.Mode = true;
            AxisInfo Info = ReadCodesysMemor.GetAxisInfo();
            brep1.Translate(Info.Right.X - 13, Info.Right.Z, Info.Right.Y);
            brep2.Translate(Info.Left.X - 13, Info.Left.Z, Info.Left.Y);
            brep3.Translate(Info.Middle.X - 13, Info.Middle.Y, -Info.Middle.Z);
            while (true)
            {
                AxisInfo axisInfo = ReadCodesysMemor.GetAxisInfo();
              
                this.Dispatcher.Invoke((Action)(() =>
                {

                    //Point3D boxMin1, boxMax1, boxMin2, boxMax2, boxMin3, boxMax3;
                    //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 3].Vertices, out boxMin1, out boxMax1);
                    //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 2].Vertices, out boxMin2, out boxMax2);
                    //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 1].Vertices, out boxMin3, out boxMax3);
                    //Point3D center1 = (boxMin1 + boxMax1) / 2; //鏡射中心點
                    //Point3D center2 = (boxMin2 + boxMax2) / 2; //鏡射中心點
                    //Point3D center3 = (boxMin3 + boxMax3) / 2; //鏡射中心點

                    brep1.Translate(axisInfo.Right.X - Info.Right.X, axisInfo.Right.Z - Info.Right.Z, axisInfo.Right.Y - Info.Right.Y);
                    brep2.Translate(axisInfo.Left.X - Info.Left.X, axisInfo.Left.Z - Info.Left.Z, axisInfo.Left.Y - Info.Left.Y); ;
                    brep3.Translate(axisInfo.Middle.X - Info.Middle.X, axisInfo.Middle.Y - Info.Middle.Y, axisInfo.Middle.Z - Info.Middle.Z);
                    Info = axisInfo;
                    model.Entities.Regen();
                    model.Invalidate();
                }));
                Thread.Sleep(200);
            }

        }
    }
}
