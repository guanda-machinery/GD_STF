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
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using devDept.Geometry;
using devDept.Eyeshot.Entities;
using WPFSTD105.Model;
using WPFSTD105;
using WPFWindowsBase;
using WPFSTD105.Tekla;
using devDept.Eyeshot;
using WPFSTD105.Attribute;
using static WPFSTD105.Model.Expand;
using System.Collections.Generic;
using DevExpress.Data.Extensions;

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
            string path = @"NM11.nc1";
            using (StreamReader stream = new StreamReader(path))
            {
                vAK.t = 13;
                oAK.t = 24;
                uAK.t = 24;
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
            H = 350,
            W=  175,
            t1 = 7,
            t2 = 11,
            Length = 2005.71
        };
        public void AK()
        {
            //前視圖
            Mesh vMesh = ConvertNcPointToMesh(SteelAttr.vPoint, SteelAttr.t1);          // 將 nc 座標轉實體
            List<Mesh> vCut = GetCutMesh(SteelAttr.vPoint, SteelAttr.t1);               // 取得切割物件
            Mesh cut1 = Mesh.CreateBox(SteelAttr.Length, SteelAttr.t2, SteelAttr.t1);   // 切割前視圖翼板輪廓
            //cut1.Translate(-5, 0);
            Mesh otherCut1 = (Mesh)cut1.Clone();
            Mesh cut2 = (Mesh)cut1.Clone();
            cut2.Translate(0, SteelAttr.H - SteelAttr.t2);
            Mesh otherCut2 = (Mesh)cut2.Clone();
            List<Solid> solids = new List<Solid>();
            solids.AddRange(Solid.Difference(otherCut1.ConvertToSolid(), vMesh.ConvertToSolid()).Where(el => el != null));
            solids.AddRange(Solid.Difference(otherCut2.ConvertToSolid(), vMesh.ConvertToSolid()).Where(el => el != null));
            //solids.ForEach(el => el.Portions.Where(el => el.)
            var cutMeshs = solids.Select(el => el.ConvertToMesh()).ToList();
            cutMeshs.ForEach(mesh =>
            {
                mesh.Vertices.ForEach(el =>
                {
                    if (el.Z == SteelAttr.t1)
                    {
                        el.Z  = SteelAttr.W;
                    }
                });
                mesh.Regen(1E3);
            });
            vCut.Add(cut1);
            vCut.Add(cut2);
            Solid vSolid = vMesh.ConvertToSolid();
            vSolid = vSolid.Difference(vCut);
            vMesh = vSolid.ConvertToMesh();
            vMesh.Color = System.Drawing.Color.Gray;
            vMesh.ColorMethod = colorMethodType.byEntity;
            vMesh.Translate(0, 0, SteelAttr.W * 0.5 - SteelAttr.t1 * 0.5);

            //頂視圖
            Mesh oMesh = ConvertNcPointToMesh(SteelAttr.oPoint, SteelAttr.t2);
            List<Mesh> oCut = GetCutMesh(SteelAttr.oPoint, SteelAttr.t2);
            Solid oSolid = oMesh.ConvertToSolid();
            oSolid = oSolid.Difference(oCut);
            oSolid.Mirror(Vector3D.AxisY, new Point3D(-10, 0, SteelAttr.t2*0.5), new Point3D(10, 0, SteelAttr.t2*0.5));
            oSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
            oSolid.Translate(0, SteelAttr.H);
            //oSolid = oSolid.Difference(cutMeshs);
            oMesh = oSolid.ConvertToMesh();
            oMesh.Color = System.Drawing.Color.Gray;
            oMesh.ColorMethod = colorMethodType.byEntity;

            //底視圖
            Mesh uMesh = ConvertNcPointToMesh(SteelAttr.uPoint, SteelAttr.t2);
            List<Mesh> uCut = GetCutMesh(SteelAttr.uPoint, SteelAttr.t2);
            Solid uSolid = uMesh.ConvertToSolid();
            uSolid = uSolid.Difference(uCut);
            uSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
            uSolid.Translate(0, SteelAttr.t2);

            //uSolid = uSolid.Difference(cutMeshs);
            uMesh = uSolid.ConvertToMesh();
            uMesh.Color = System.Drawing.Color.Gray;
            uMesh.ColorMethod = colorMethodType.byEntity;
            vMesh.MergeWith(uMesh);
            vMesh.MergeWith(oMesh);
            model1.Entities.Add(vMesh);

           //model1.Entities.Add(new LinearPath(ds));
           //model1.Entities.Add(oMesh);
           //model1.Entities.Add(uMesh);
           //model1.Entities.Add(vMesh);
            model1.Refresh();
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
        /// 返回一個網格，其中刪除了未使用的頂點，並且頂點引用在網格m的“三角形”列表中重新排序。
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
        private bool Flip(List<Point3D> point3Ds, Func<double> func, SteelAttr steelAttr, double extendX, out double angleP1, out double angleP2)
        {
            double x = func.Invoke();
            var point = point3Ds.Where(el => el.X == x).OrderBy(el => el.Y).FirstOrDefault();
            int index = point3Ds.FindIndex(el => el == point);
            //int maxIndex = point3Ds.FindIndex(el => el == maxPoint);
            var center = point3Ds[index];
            var p1 = point3Ds[CycleIndex(point3Ds, index -1)];
            var p2 = point3Ds[CycleIndex(point3Ds, index +1)];
            angleP1 = WPFSTD105.Tekla.AK.Angle(center, p1, new Point3D(center.X + extendX, center.Y));
            angleP2 = WPFSTD105.Tekla.AK.Angle(center, p2, new Point3D(center.X + extendX, center.Y));
            if (angleP1 % 90 != 0 || angleP2 % 90 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int CycleIndex(List<Point3D> point3Ds, int index)
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
    }
}
