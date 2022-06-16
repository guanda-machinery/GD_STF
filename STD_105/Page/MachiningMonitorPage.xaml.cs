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
using WPFWindowsBase;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using static WPFSTD105.CodesysIIS;
using devDept.Eyeshot;
using WPFSTD105.ViewModel;
using DevExpress.Xpf.Grid;

namespace STD_105
{
    /// <summary>
    /// MonitorPage.xaml 的互動邏輯
    /// </summary>
    public partial class MachiningMonitorPage : BasePage<ProcessingMonitorVM>
    {
        public MachiningMonitorPage()
        {
            InitializeComponent();
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            model.Renderer = rendererType.NativeExperimental; //使用 OpenGL渲染
            ((ProcessingMonitorVM)DataContext).UndoneGrid = Material;
            ((ProcessingMonitorVM)DataContext).FinishGrid = Finish;
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
            /*Task.Factory.StartNew(el => */
            ViewModel.SetModel(model)/*, model);*/;
            model.Loaded -= model_Loaded;
        }

        public void Run()
        {
            try
            {
                //CommonViewModel.AxisInfoListening.ChangeLevel(WPFSTD105.Listening.LEVEL.HIGH);
                //CommonViewModel.AxisInfoListening.Mode = true;
                AxisInfo Info = ReadCodesysMemor.GetAxisInfo();
                brep1.Translate(Info.Right.X, Info.Right.Z, Info.Right.Y);
                brep2.Translate(Info.Left.X, Info.Left.Z, Info.Left.Y); ;
                brep3.Translate(Info.Middle.X, Info.Middle.Y, Info.Middle.Z);
                while (true)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        AxisInfo axisInfo = ReadCodesysMemor.GetAxisInfo();
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
                        model.Refresh();
                    }));
                    Thread.Sleep(200);
                }
                ////CommonViewModel.AxisInfoListening.ChangeLevel(WPFSTD105.Listening.LEVEL.HIGH);
                ////CommonViewModel.AxisInfoListening.Mode = true;
                //AxisInfo Info = ReadCodesysMemor.GetAxisInfo();
                //Brep _brep1 = (Brep)brep1.Clone();
                //Brep _brep2 = (Brep)brep2.Clone();
                //Brep _brep3 = (Brep)brep3.Clone();

                //_brep1.Translate(Info.Right.X, Info.Right.Z, Info.Right.Y);
                //_brep2.Translate(Info.Left.X, Info.Left.Z, Info.Left.Y);
                //_brep3.Translate(Info.Middle.X, Info.Middle.Y, Info.Middle.Z);
                //this.Dispatcher.Invoke((Action)(() =>
                //{
                //    model.Entities.Add(_brep1);
                //    model.Entities.Add(_brep2);
                //    model.Entities.Add(_brep3);
                //    model.Refresh();
                //}));
                //Debug.WriteLine($"右軸 x = {Info.Right.X }, y = {Info.Right.Z }, z = {Info.Right.Y } ");
                //Debug.WriteLine($"左軸 x = {Info.Left.X}, y = {Info.Left.Z}, z = {Info.Left.Y} ");
                //Debug.WriteLine($"中軸 x = {Info.Middle.X}, y = {Info.Middle.Y }, z = {Info.Middle.Z} ");
                //while (true)
                //{
                //    //AxisInfo axisInfo = ReadCodesysMemor.GetAxisInfo();
                //    Info = ReadCodesysMemor.GetAxisInfo();
                //    this.Dispatcher.Invoke((Action)(() =>
                //    {
                //        //Point3D boxMin1, boxMax1, boxMin2, boxMax2, boxMin3, boxMax3;
                //        //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 3].Vertices, out boxMin1, out boxMax1);
                //        //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 2].Vertices, out boxMin2, out boxMax2);
                //        //Utility.ComputeBoundingBox(null, model.Entities[model.Entities.Count - 1].Vertices, out boxMin3, out boxMax3);
                //        //Point3D center1 = (boxMin1 + boxMax1) / 2; //鏡射中心點
                //        //Point3D center2 = (boxMin2 + boxMax2) / 2; //鏡射中心點
                //        //Point3D center3 = (boxMin3 + boxMax3) / 2; //鏡射中心點
                //        // brep1.Translate(axisInfo.Right.X - Info.Right.X, axisInfo.Right.Z - Info.Right.Z, axisInfo.Right.Y - Info.Right.Y);
                //        // brep2.Translate(axisInfo.Left.X - Info.Left.X, axisInfo.Left.Z - Info.Left.Z, axisInfo.Left.Y - Info.Left.Y);
                //        // brep3.Translate(axisInfo.Middle.X - Info.Middle.X, axisInfo.Middle.Y - Info.Middle.Y, axisInfo.Middle.Z - Info.Middle.Z);
                //        // if (axisInfo.Right.X - Info.Right.X != 0 || axisInfo.Right.Z - Info.Right.Z != 0 || axisInfo.Right.Y - Info.Right.Y != 0)
                //        // {
                //        //     Debug.WriteLine($"右軸 x = {axisInfo.Right.X - Info.Right.X}, y = {axisInfo.Right.Z - Info.Right.Z}, z = {axisInfo.Right.Y - Info.Right.Y} ");
                //        // }
                //        // if (axisInfo.Left.X - Info.Left.X != 0 || axisInfo.Left.Z - Info.Left.Z != 0 || axisInfo.Left.Y - Info.Left.Y != 0)
                //        // {
                //        //     Debug.WriteLine($"左軸 x = {axisInfo.Left.X - Info.Left.X}, y = {axisInfo.Left.Z - Info.Left.Z}, z = {axisInfo.Left.Y - Info.Left.Y} ");
                //        // }
                //        // if (axisInfo.Middle.X - Info.Middle.X != 0 || axisInfo.Middle.Y - Info.Middle.Y != 0 || axisInfo.Middle.Z - Info.Middle.Z != 0)
                //        // {
                //        //     Debug.WriteLine($"中軸 x = {axisInfo.Middle.X - Info.Middle.X}, y = {axisInfo.Middle.Y - Info.Middle.Y}, z = {axisInfo.Middle.Z - Info.Middle.Z} ");
                //        // }
                //        // Info = axisInfo;
                //        // model.Entities.Regen();
                //        for (int i = 0; i < model.Entities.Count; i++)
                //        {
                //            if (model.Entities[i].GetType() == typeof(Brep))
                //            {
                //                model.Entities.RemoveAt(i);
                //            }
                //        }
                //        _brep1 = (Brep)brep1.Clone();
                //        _brep2 = (Brep)brep2.Clone();
                //        _brep3 = (Brep)brep3.Clone();
                //        _brep1.Translate(Info.Right.X, Info.Right.Z, Info.Right.Y);
                //        _brep2.Translate(Info.Left.X, Info.Left.Z, Info.Left.Y);
                //        _brep3.Translate(Info.Middle.X, Info.Middle.Y, Info.Middle.Z);
                //        model.Entities.Add(_brep1);
                //        model.Entities.Add(_brep2);
                //        model.Entities.Add(_brep3);
                //        model.Refresh();
                //    }));
                //    Thread.Sleep(200);
                //}
            }
            catch (Exception)
            {

            }
        }
        Thread _;
        private void PowerButton1_ButtonClick(object sender, EventArgs e)
        {
            if (_ == null)
            {
                _ = new Thread(Run);
                _.Start();
            }
        }

        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                model.Dispose();
                disposable.Dispose();
            }
        }

        private void Material_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
            GridControl gridControl = sender as GridControl;    
            gridControl.Columns.Clear();
            GridColumn column = new GridColumn();
            column.FieldName = "MaterialNumber";
            column.Header = "素材編號";
            gridControl.Columns.Add(column);
            column = new GridColumn();
            column.FieldName = "Profile";
            column.Header = "斷面規格";
            gridControl.Columns.Add(column);
            column = new GridColumn();
            ConverterAssemblyNumber assemblyNumber = new ConverterAssemblyNumber();
            Binding binding = new Binding("Parts")
            {
                Converter = assemblyNumber
            };
            column.Binding = binding;   
            column.Header = "構件編號";            
            gridControl.Columns.Add(column);
            column = new GridColumn();
            ConverterToPartNumber partNumber = new ConverterToPartNumber();
            Binding binding1 = new Binding("__")
            {
                Converter = partNumber
            };
            column.Binding = binding1;
            column.Header = "零件編號";
            gridControl.Columns.Add(column);
            column = new GridColumn();
            column.FieldName = "Position";
            column.Header = "位置";
            gridControl.Columns.Add(column);
            column = new GridColumn();
            column.FieldName = "Schedule";
            column.Header = "進度(%)";
            gridControl.Columns.Add(column);
        }

        private void Finish_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            gridControl.Columns.Clear();

            GridColumn column1 = new GridColumn
            {
                FieldName = "MaterialNumber",
                Header = "素材編號"
            };
            gridControl.Columns.Add(column1);

            GridColumn column2 = new GridColumn
            {
                FieldName = "Profile",
                Header = "斷面規格"
            };
            gridControl.Columns.Add(column2);
                       
            ConverterAssemblyNumber assemblyNumber = new ConverterAssemblyNumber();
            Binding binding = new Binding("Parts")
            {
                Converter = assemblyNumber
            };
            GridColumn column3 = new GridColumn
            {
                Binding = binding,
                Header = "構件編號"
            };
            gridControl.Columns.Add(column3);
                        
            ConverterToPartNumber partNumber = new ConverterToPartNumber();
            Binding binding1 = new Binding("Parts")
            {
                Converter = partNumber
            };
            GridColumn column4 = new GridColumn
            {
                Binding = binding1,
                Header = "零件編號"
            };
            gridControl.Columns.Add(column4);

            GridColumn column5 = new GridColumn
            {
                FieldName = "Position",
                Header = "位置"
            };
            gridControl.Columns.Add(column5);
        }
    }
}
