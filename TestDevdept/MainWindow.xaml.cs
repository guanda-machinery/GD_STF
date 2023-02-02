using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DevExpress.Xpf.CodeView;
using DevExpress.XtraRichEdit.Model;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TriangleNet;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.Tekla;
using WPFWindowsBase;
using static WPFSTD105.Model.Expand;
using Mesh = devDept.Eyeshot.Entities.Mesh;

namespace TestDevdept
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            model1.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
        }

        private void button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string path = @"M166.nc1";
            using (StreamReader stream = new StreamReader(path))
            {

                SteelAttr.vPoint = null;
                SteelAttr.oPoint = null;
                SteelAttr.uPoint = null;

                string line; //資料行
                string blockName = string.Empty; //資料行的標示區塊，例如AK or BO
                string face = null; //形狀輪廓的面
                while ((line = stream.ReadLine()) != null)
                {
                    if (line.Contains("AK", "BO", "IK", "PU", "KO", "KA")) //輪廓螺栓標示區號
                    {
                        blockName = line.Trim();
                        face = null;//清除目前的視圖標記
                    }
                    else
                    {
                        if (line == "EN") //nc1 結束符號
                        {
                            break;
                        }
                        if (blockName == "AK" && line != string.Empty)
                        {
                            if (face == null)
                            {
                                string[] str = line.Split(' ').Where(el => el != string.Empty).ToArray();
                                if (str.Length != 0)
                                {
                                    if (str[0] == "v")
                                    {
                                        face = "v";
                                        vAK.Parameter.Add(line.Split('v')[1]);
                                    }
                                    else if (str[0] == "o")
                                    {
                                        face = "o";
                                        oAK.Parameter.Add(line.Split('o')[1]);
                                    }
                                    else if (str[0] == "u")
                                    {
                                        face = "u";
                                        uAK.Parameter.Add(line.Split('u')[1]);
                                    }
                                }
                            }
                            else if (face == "v")
                            {
                                vAK.Parameter.Add(line);
                            }
                            else if (face == "o")
                            {
                                oAK.Parameter.Add(line);
                            }
                            else if (face == "u")
                            {
                                uAK.Parameter.Add(line);
                            }
                        }
                    }
                }

               
                SteelAttr.vPoint = vAK.GetNcPoint();
                SteelAttr.uPoint = uAK.GetNcPoint();
                SteelAttr.oPoint = oAK.GetNcPoint();
                AK();
            }
        }
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK vAK { get; set; } = new AK();
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK oAK { get; set; } = new AK();
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK uAK { get; set; } = new AK();
        private SteelAttr SteelAttr = new SteelAttr()
        {
            H = 250,                /// 高
            W = 125,                /// 寬
            t1 = (float)9  ,        /// 腹板厚度
            t2 = 6,                 /// 翼板厚度
            Length = 3000.05        ///長
        };

        List<NcPoint3D> VKGetPoint = new List<NcPoint3D>();

        public void TestAK(List<string> VKGetPoint)
        {


            List<NcPoint3D> result = new List<NcPoint3D>();
            foreach (var item in VKGetPoint)
            {
                //將 string 轉換 double
                List<double> values = GetValues(item);
                NcPoint3D point3D = new NcPoint3D(values[0],
                                                    values[1],
                                                    r: values[2],
                                                    angle1: values[3],
                                                    startAngle1: values[4],
                                                    angle2: values[5],
                                                    startAngle2: values[6]);

            }


                // List<NcPoint3D> VKGetPoint = new List<NcPoint3D>();




            }


        public List<double> GetValues(string para)
        {
            string[] str = para.Split(' ');
            List<double> result = new List<double>();
            for (int i = 0; i < str.Length; i++)
            {
                double? _ = GetValue(str[i]);
                if (_ != null)
                {
                    result.Add(_.Value);
                }
            }
            return result;
        }








        public void AK()
        {



            //model.Clear();//清除模型內物件
            //  model1.Blocks[steelBlock.BlockName].Entities.Clear();//清除圖塊
            //vMesh 頂視圖
            Mesh vMesh = ConvertNcPointToMesh(SteelAttr.vPoint, SteelAttr.t1); //將 nc 座標轉實體
            List<Mesh> vCut = GetCutMesh(SteelAttr.vPoint, SteelAttr.t1); //取得切割物件
            Mesh cut1 = Mesh.CreateBox(SteelAttr.Length + 10, SteelAttr.t2, SteelAttr.t1);//建一立方體  切割前視圖翼板輪廓
            cut1.Translate(-5, 0);
            Mesh otherCut1 = (Mesh)cut1.Clone();
            Mesh cut2 = (Mesh)cut1.Clone();
            cut2.Translate(0, SteelAttr.H - SteelAttr.t2);
            Mesh otherCut2 = (Mesh)cut2.Clone();
            List<Solid> solids = new List<Solid>();
            Solid[] s1 = Solid.Difference(otherCut1.ConvertToSolid(), vMesh.ConvertToSolid());
            Solid[] s2 = Solid.Difference(otherCut2.ConvertToSolid(), vMesh.ConvertToSolid());
            Solid[] emptySolid = new Solid[0];
            solids.AddRange(s1 == null ? emptySolid : s1.Where(el => el != null));
            solids.AddRange(s2 == null ? emptySolid : s2.Where(el => el != null));
            //solids.ForEach(el => el.Portions.Where(el => el.)
            var cutMeshs = solids.Select(el => el.ConvertToMesh()).ToList();
            cutMeshs.ForEach(mesh =>
            {
                mesh.Vertices.Where(x => x.Z == SteelAttr.t1).ForEach(el =>
                {
                    el.Z = SteelAttr.W;
                    //if (el.Z == SteelAttr.t1)
                    //{
                    //    el.Z  = SteelAttr.W;
                    //}
                });
                mesh.Regen(1E3);
            });
            vCut.Add(cut1);
            vCut.Add(cut2);
            Solid vSolid = vMesh.ConvertToSolid();
            vSolid = vSolid.Difference(vCut);
            vMesh = vSolid.ConvertToMesh();
            vMesh.Color = System.Drawing.ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Part);
            vMesh.ColorMethod = colorMethodType.byEntity;
            vMesh.Translate(0, 0, SteelAttr.W * 0.5 - SteelAttr.t1 * 0.5);

            //oMesh 後視圖
            Mesh oMesh = ConvertNcPointToMesh(SteelAttr.oPoint, SteelAttr.t2);
            List<Mesh> oCut = GetCutMesh(SteelAttr.oPoint, SteelAttr.t2);
            Solid oSolid = oMesh.ConvertToSolid();
            oSolid = oSolid.Difference(oCut);
            oSolid.Mirror(Vector3D.AxisY, new Point3D(-10, 0, SteelAttr.t2 * 0.5), new Point3D(10, 0, SteelAttr.t2 * 0.5));
            oSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
            oSolid.Translate(0, SteelAttr.H);
            //oSolid = oSolid.Difference(cutMeshs);
            oMesh = oSolid.ConvertToMesh();
            oMesh.Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Part);
            oMesh.ColorMethod = colorMethodType.byEntity;

            //uMesh 前視圖
            Mesh uMesh = ConvertNcPointToMesh(SteelAttr.uPoint, SteelAttr.t2);
            List<Mesh> uCut = GetCutMesh(SteelAttr.uPoint, SteelAttr.t2);
            Solid uSolid = uMesh.ConvertToSolid();
            uSolid = uSolid.Difference(uCut);
            uSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
            uSolid.Translate(0, SteelAttr.t2);
            //uSolid = uSolid.Difference(cutMeshs);
            uMesh = uSolid.ConvertToMesh();
            uMesh.Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Part);
            uMesh.ColorMethod = colorMethodType.byEntity;



            vMesh.EntityData = SteelAttr;

            vMesh.MergeWith(oMesh);
            vMesh.MergeWith(uMesh);



            SteelAttr.Type = GD_STD.Enum.OBJECT_TYPE.BH;
            model1.Entities.Add(vMesh);//加入物件到圖塊內

          //  Mesh modifym = cutasm(SteelAttr);
         //   Solid modifys = modifym.ConvertToSolid();
        //    modifys = modifys.Difference(solids);

         //   modifys.Translate(0, 500, 0);
           // model1.Entities.Add(modifys);

            model1.ZoomFit();
            model1.Refresh();

         




        }



        public Mesh cutasm(SteelAttr steelAttr)
        {


            List<ICurve> curves = new List<ICurve>() { BaseProfile(steelAttr.H, steelAttr.W, 0, 0) };
    
                //組合實際斷面規格
                switch (steelAttr.Type)
                {
                    case OBJECT_TYPE.H: //20220805 張燕華 新增斷面規格H型鋼
                    case OBJECT_TYPE.BH:
                    case OBJECT_TYPE.RH:
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), 0, steelAttr.t2));
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), (steelAttr.W * 0.5) + (steelAttr.t1 * 0.5), steelAttr.t2));

                        break;

                }
                // 區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
                devDept.Eyeshot.Entities.Region region = new devDept.Eyeshot.Entities.Region(curves, Plane.XY, false);
                Mesh result = region.ExtrudeAsMesh<Mesh>(new Vector3D(steelAttr.Length, 0, 0), 1e-6, Mesh.natureType.Plain);// 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
                result.EntityData = steelAttr;

                ////切割物件
                //CutLine(ref result, FACE.TOP);
                ////CutLine(ref result, FACE.BACK);
                //CutLine(ref result, FACE.FRONT);

                //result.EntityData = steelAttr.DeepClone();
                // 零件顏色 #調整顏色
                result.Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Part);
                result.ColorMethod = colorMethodType.byEntity;


            return result;

        }



        /// <summary>
        /// 繪製方形輪廓
        /// </summary>
        /// <param name="h">高度</param>
        /// <param name="w">寬度</param>
        /// <param name="startZ">起始點Z</param>
        /// <param name="startY">起始點Y</param>
        /// <returns></returns>
        private static ICurve BaseProfile(double h, double w, double startZ, double startY)
        {
            //2D繪製矩形
            Point3D[] point3Ds = new Point3D[]
            {
                new Point3D(0,startY,startZ),
                new Point3D(0,h +startY,startZ),
                new Point3D(0,h+startY,w+startZ),
                new Point3D(0,startY,w+startZ),
                new Point3D(0,startY,startZ),
            };

            LinearPath result = new LinearPath(point3Ds);

            for (int i = 0; i < point3Ds.Length; i++)
            {
                Console.WriteLine($"new Point3D(0, {point3Ds[i].Y}, {point3Ds[i].Z})");
                log4net.LogManager.GetLogger($"產生Base[{i}]").Debug($@"{point3Ds[i]}");
            }


            return result;
        }



        /// <summary>
        /// 刪除未使用的三角面
        /// </summary>
        /// <param name="top">上視圖</param>
        /// <param name="front">前視圖</param>
        /// <param name="back">後視圖</param>
        /// <param name="triangleCount">上視圖三角面數量</param>
        /// <param name="fromTriangleCount">前視圖角面數量</param>
        /// <param name="backTriangleCount">後視圖角面數量</param>
        void DeleteUnusedTriangle(ref Mesh result, int triangleCount)
        {
            int destCount = 0;

            IndexTriangle[] triangles = new IndexTriangle[triangleCount];

            for (int i = 0; i < result.Triangles.Count(); i++)
            {
                if (result.Triangles[i] != null)
                {
                    triangles[destCount] = (IndexTriangle)result.Triangles[i].Clone();
                    destCount++;
                }

            }
            result.Triangles = triangles;

        }
        /// <summary>
        /// EG4YAup2hySgcimq8MkLuB6e34rNB2SF9h94LL1ryzzq
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected Mesh DeleteUnusedVertices(Mesh m)
        {
            int[] newV = new int[m.Vertices.Length];
            int count = 1;

            foreach (IndexTriangle it in m.Triangles)
            {
                if (newV[it.V1] == 0)
                {
                    newV[it.V1] = count;
                    count++;
                }
                it.V1 = newV[it.V1] - 1;

                if (newV[it.V2] == 0)
                {
                    newV[it.V2] = count;
                    count++;
                }
                it.V2 = newV[it.V2] - 1;

                if (newV[it.V3] == 0)
                {
                    newV[it.V3] = count;
                    count++;
                }
                it.V3 = newV[it.V3] - 1;
            }

            Point3D[] finalV = new Point3D[count - 1];
            for (int i = 0; i < m.Vertices.Length; i++)
            {
                if (newV[i] != 0)
                {
                    finalV[newV[i] - 1] = m.Vertices[i];
                    //finalV[newV[i] - 1].Z = 0;
                }
            }
            m.Vertices = finalV;
            return m;
        }
        /// <summary>
        /// 關聯面角角度
        /// </summary>
        readonly double _angularTol = Utility.DegToRad(7);
        /// <summary>
        /// 按法線分割三角形。將三角形和頂點分成，頂面。
        /// </summary>
        /// <param name="section">要拆解成部分面的圖形</param>
        /// <param name="count">保留的三角數量</param>
        /// <param name="vector3D"></param>
        private Mesh SplitTrianglesByNormal(Mesh mesh, out int count, Vector3D vector3D, Func<double, bool> func)
        {
            count = 0;
            Mesh result = (Mesh)mesh.Clone();
            for (int i = 0; i < mesh.Triangles.Length; i++)
            {
                IndexTriangle indexTriangle = mesh.Triangles[i];//使用頂點索引定義三角形。
                Triangle triangle = new Triangle(mesh.Vertices[indexTriangle.V1], mesh.Vertices[indexTriangle.V2], mesh.Vertices[indexTriangle.V3]);//三角形
                triangle.Regen(0.1);//計算曲線或曲面細分。
                double angle = Vector3D.AngleBetween(vector3D, triangle.Normal);//計算兩個向量之間的角度。
                if (func.Invoke(angle))
                {
                    count++;
                }
                else
                {
                    result.Triangles[i] = null;
                }
            }
            return result;
        }




        public static bool Flip(Entity entity, Func<List<Point3D>, double> func, SteelAttr steelAttr, double extendX, out double angleP1, out double angleP2, out double x, Transformation transformation = null)
        {

            //var minTopFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minTopAngle1, out double minTopAngle2, out double topMinX);

            List<Point3D> point3Ds = entity.Vertices.ToList();
            if (transformation != null)
                point3Ds.ForEach(p => p.TransformBy(transformation));
            List<Point2D> point2Ds = point3Ds.Select(e => new Point2D(e.X, e.Y)).ToList();
            point3Ds = UtilityEx.ConvexHull2D(point2Ds, true).Select(e => new Point3D(e.X, e.Y)).ToList();
            double _x = func.Invoke(point3Ds);
            var point = point3Ds.Where(el => el.X == _x).OrderBy(el => el.Y).FirstOrDefault();
            int index = point3Ds.FindIndex(el => el == point);
            //int maxIndex = point3Ds.FindIndex(el => el == maxPoint);
            var origin = point3Ds[index];
            var p1 = point3Ds[CycleIndex(point3Ds, index - 1)];
            var p2 = point3Ds[CycleIndex(point3Ds, index + 1)];
            x = origin.X;
            angleP1 = WPFSTD105.Tekla.AK.Angle(origin, p1, new Point3D(origin.X + extendX, origin.Y));
            angleP2 = WPFSTD105.Tekla.AK.Angle(origin, p2, new Point3D(origin.X + extendX, origin.Y));
            angleP1 = Math.Abs(WPFSTD105.Tekla.AK.SideLengthC(p1.Y - origin.Y, angleP1)) > 50 ? angleP1 : 0;
            angleP2 = Math.Abs(WPFSTD105.Tekla.AK.SideLengthC(p2.Y - origin.Y, angleP1)) > 50 ? angleP2 : 0;
            if (angleP1 % 90 != 0 || angleP2 % 90 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        //private bool Flip(List<Point3D> point3Ds, Func<double> func, SteelAttr steelAttr, double extendX, out double angleP1, out double angleP2)
        //{
        //    double x = func.Invoke();
        //    var point = point3Ds.Where(el => el.X == x).OrderBy(el => el.Y).FirstOrDefault();
        //    int index = point3Ds.FindIndex(el => el == point);
        //    //int maxIndex = point3Ds.FindIndex(el => el == maxPoint);
        //    var center = point3Ds[index];
        //    var p1 = point3Ds[CycleIndex(point3Ds, index - 1)];
        //    var p2 = point3Ds[CycleIndex(point3Ds, index + 1)];
        //    angleP1 = WPFSTD105.Tekla.AK.Angle(center, p1, new Point3D(center.X + extendX, center.Y));
        //    angleP2 = WPFSTD105.Tekla.AK.Angle(center, p2, new Point3D(center.X + extendX, center.Y));
        //    if (angleP1 % 90 != 0 || angleP2 % 90 != 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}



        public static int CycleIndex(List<Point3D> point3Ds, int index)
        {
            if (index >= point3Ds.Count)
            {
                return 1;
            }
            else if (index < 0)
            {
                return point3Ds.Count - 2;
            }
            else
            {
                return index;
            }
        }


        //public int CycleIndex(List<Point3D> point3Ds, int index)
        //{
        //    if (index >= point3Ds.Count)
        //    {
        //        return 1;
        //    }
        //    else if (index < 0)
        //    {
        //        return point3Ds.Count - 2;
        //    }
        //    else
        //    {
        //        return index;
        //    }
        //}
        /// <summary>
        /// 取得數值
        /// </summary>
        /// <param name="str"></param>
        private double GetValue(string str)
        {
            MatchCollection matches = Regex.Matches(str, @"[0-9.]+");//找出數值包含小數點
            double result = 0d;
            if (matches.Count > 0)
            {
                result = Convert.ToDouble(matches[0].Value);
            }
            else
            {
                Debugger.Break();   
            }
            return result;
        }

        private void button_Copy_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_Click()
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
