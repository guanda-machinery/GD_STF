#define Debug
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using WPFSTD105.ViewModel;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using WPFWindowsBase;
using Region = devDept.Eyeshot.Entities.Region;
using devDept.Graphics;
using devDept.Serialization;
using GD_STD.Enum;
using System.Windows.Media;
using static DevExpress.Utils.Menu.DXMenuItemPainter;
using DevExpress.Mvvm;
//using TriangleNet;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 擴展
    /// </summary>
    public static class Expand
    {
        /// <summary>
        /// 組合零件變成素材(3D)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="materialNumber">素材編號</param>
        public static void AssemblyPart(this devDept.Eyeshot.Model model, string materialNumber)
        {
            ObSettingVM obvm = new ObSettingVM();
            model.Clear();
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            int index = materialDataViews.FindIndex(el => el.MaterialNumber == materialNumber);//序列化的列表索引
            MaterialDataView material = materialDataViews[index];
            ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            var guid = (from el in ncTemps
                        where _.ToList().Contains(el.SteelAttr.PartNumber)
                        select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            //產生nc檔案圖檔
            for (int i = 0; i < guid.Count; i++)
            {
                model.LoadNcToModel(guid[i], ObSettingVM.allowType);
            }


            var place = new List<(double Start, double End, bool IsCut, string Number)>();//放置位置參數
            place.Add((Start: 0, End: material.StartCut, IsCut: true, Number: "")); //素材起始切割物件
            Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            for (int i = 0; i < material.Parts.Count; i++)
            {
                int partIndex = parts.FindIndex(el => el.Number == material.Parts[i].PartNumber); //回傳要使用的陣列位置
                if (partIndex == -1)
                {
                    // 未找到對應零件編號
                    string tmp=material.Parts[i].PartNumber;
                    WinUIMessageBox.Show(null,
                    $"未找到對應零件編號"+ tmp,
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                    return;

                    //throw new Exception($"在 ObservableCollection<SteelPart> 找不到 {material.Parts[i].PartNumber}");
                }
                else
                {
                    double startCurrent = place[place.Count - 1].End,//當前物件放置起始點的座標
                                  endCurrent = startCurrent + parts[partIndex].Length;//當前物件放置結束點的座標
                    place.Add((Start: startCurrent, End: endCurrent, IsCut: false, Number: parts[partIndex].Number));
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                    //計算切割物件
                    double startCut = place[place.Count - 1].End, //當前切割物件放置起始點的座標
                                  endCut;//當前切割物件放置結束點的座標
                    if (i + 1 >= material.Parts.Count) //下一次迴圈結束
                    {
                        endCut = material.LengthStr + material.StartCut + material.EndCut;//當前切割物件放置結束點的座標
                    }
                    else //下一次迴圈尚未結束
                    {
                        endCut = startCut + material.Cut;//當前切割物件放置結束點的座標
                    }
                    place.Add((Start: startCut, End: endCut, IsCut: true, Number: "")); //素材零件位置
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                }
            }
            EntityList entities = new EntityList();
            for (int i = 0; i < place.Count; i++)
            {
                if (place[i].IsCut) //如果是切割物件
                {
                    //Entity cut = DrawCutMesh(parts[0], model, place[i].Start, place[i].End, "Cut");
                    //if (cut != null)
                    //{
                    //    entities.Add(cut);
                    //}

                    continue;
                }
                int placeIndex = place.FindIndex(el => el.Number == place[i].Number); //如果有重複的編號只會回傳第一個，以這個下去做比較。
                if (placeIndex != i) //如果 i != 第一次出現的 index 代表需要使用複製
                {
                    EntityList ent = new EntityList();
                    entities.
                        Where(el => el.GroupIndex == placeIndex).
                        ForEach(el =>
                        {
                            Entity copy = (Entity)el.Clone(); //複製物件
                            copy.GroupIndex = i;
                            copy.Translate(place[i].Start - place[placeIndex].Start, 0);
                            ent.Add(copy);
                        });
                    entities.AddRange(ent);
                }
                else
                {
                    int partIndex = parts.FindIndex(el => el.Number == place[i].Number);
                    if (parts[partIndex].GUID.ToString() != "") //如果圖面檔案
                    {
                        ReadFile file = ser.ReadPartModel(parts[partIndex].GUID.ToString()); //讀取檔案內容
                        if (file == null)
                        {
                            WinUIMessageBox.Show(null,
                                $"專案Dev_Part資料夾讀取失敗",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                FloatingMode.Popup);
                            return;
                        }
                        file.DoWork();
                        file.AddToScene(model);
                        Entity _entity = null;
                        SteelAttr _steelAttr = null;
                        model.Entities.ForEach(el =>
                        {
                            if (el.GetType() != typeof(LinearDim))
                            {
                                model.Blocks[((BlockReference)el).BlockName].Entities.ForEach(entity =>
                                {
                                    if (entity.EntityData is SteelAttr steelAttr)
                                    {
                                        _entity = entity;
                                        _steelAttr = steelAttr;
                                    }
                                });
                                el.GroupIndex = i;
                                el.Translate(place[i].Start, 0);
                                el.Selectable = false;
                                entities.Add(el);//加入到暫存列表
                            }
                        });
                        Func<List<Point3D>, double> minFunc = (point3d) => point3d.Min(e => e.X);
                        Func<List<Point3D>, double> maxFunc = (point3d) => point3d.Max(e => e.X);
                        Transformation transformation = new Transformation(new Point3D(0, _steelAttr.H / 2, _steelAttr.W), Vector3D.AxisX, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0));

                        var minTopFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minTopAngle1, out double minTopAngle2, out double topMinX);
                        var maxTopFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxTopAngle1, out double maxTopAngle2, out double topMaxX);
                        var minBackFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minBackAngle1, out double minBackAngle2, out double backMinX, transformation);
                        var maxBackFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxBackAngle1, out double maxBackAngle2, out double backMaxX, transformation);
                        var minFrontFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minFrontAngle1, out double minFrontAngle2, out double frontMinX, transformation);
                        var maxFrontFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxFrontAngle1, out double maxFrontAngle2, out double frontMaxX, transformation);
                        if ((minBackFlip && minFrontFlip) || (maxBackFlip && maxFrontFlip))
                        {
                            if (minTopFlip || maxTopFlip)
                            {
                                _steelAttr.StartAngle = minTopAngle1 == 0 ? minTopAngle2 : minTopAngle1;
                                _steelAttr.EndAngle = maxTopAngle1 == 0 ? maxTopAngle2 : maxTopAngle1;
                                if (_steelAttr.StartAngle > 90 || _steelAttr.EndAngle > 90)
                                {
                                    Rotate(entities.Where(el => el.GroupIndex == i), (_entity.BoxMin + _entity.BoxMax) / 2);
                                }
                            }
                            else if (backMinX < frontMinX)
                                _steelAttr.StartAngle = minBackAngle1 == 0 ? minBackAngle2 : minBackAngle1;
                            else
                                _steelAttr.StartAngle = minFrontAngle1 == 0 ? minFrontAngle2 : minFrontAngle1;

                            if (backMaxX < frontMaxX)
                            {
                                _steelAttr.EndAngle = maxBackAngle1 == 0 ? maxBackAngle2 : maxBackAngle1;
                            }
                            else
                            {
                                _steelAttr.EndAngle = maxFrontAngle1 == 0 ? maxFrontAngle2 : maxFrontAngle1;
                            }
                        }
                        else if (minBackFlip || maxBackFlip)
                        {
                            _steelAttr.StartAngle = minBackAngle1 == 0 ? minBackAngle2 : minBackAngle1;
                            _steelAttr.EndAngle = maxBackAngle1 == 0 ? maxBackAngle2 : maxBackAngle1;
                        }
                        else if (minFrontFlip || maxFrontFlip)
                        {
                            _steelAttr.StartAngle = minFrontAngle1 == 0 ? minFrontAngle2 : minFrontAngle1;
                            _steelAttr.EndAngle = maxFrontAngle1 == 0 ? maxFrontAngle2 : maxFrontAngle1;
                        }
                        else if (minTopFlip || maxTopFlip)
                        {
                            _steelAttr.StartAngle = minTopAngle1 == 0 ? minTopAngle2 : minTopAngle1;
                            _steelAttr.EndAngle = maxTopAngle1 == 0 ? maxTopAngle2 : maxTopAngle1;
                            if (_steelAttr.StartAngle > 90 || _steelAttr.EndAngle > 90)
                            {
                                Rotate(entities.Where(el => el.GroupIndex == i), (_entity.BoxMin + _entity.BoxMax) / 2);
                            }
                        }
                    }
                    else //如果沒有 NC 檔案
                    {
                        SteelAttr steelAttr = new SteelAttr(parts[partIndex]); //產生物件設定檔
                        steelAttr.GUID = parts[partIndex].GUID = Guid.NewGuid(); //賦予新的guid
                        Steel3DBlock steel = Steel3DBlock.AddSteel(steelAttr, model, out BlockReference blockReference); //加入鋼構物件到 Model
                        blockReference.Translate(place[i].Start, 0);//移動目標
                        blockReference.Selectable = true;
                        entities.Add(blockReference); //加入到暫存列表
                        ser.SetPart(material.Profile.GetHashCode().ToString(), new ObservableCollection<object>(parts));//存取
                        ser.SetPartModel(steelAttr.GUID.ToString(), model);//儲存模型
                        ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond();//零件清單
                        dataCorrespond.Add(new DataCorrespond()
                        {
                            Number = steelAttr.PartNumber,
                            DataName = steelAttr.GUID.ToString(),
                            Profile = steelAttr.Profile,
                            Type = steelAttr.Type,
                            TP = false,

                            // 2022/09/08 彥谷
                            oPoint = steelAttr.oPoint.ToArray(),
                            vPoint = steelAttr.vPoint.ToArray(),
                            uPoint = steelAttr.uPoint.ToArray(),
                        });
                        ser.SetDataCorrespond(dataCorrespond);//加入到製品零件清單
                    }
                }
            }
            //model.AssemblySelectionMode = devDept.Eyeshot.Environment.assemblySelectionType.Leaf;
            model.Refresh();
            model.Entities.Clear();
            SteelAttr attr = new SteelAttr(parts[0]);
            attr.Length = material.LengthStr;
            Mesh cut = Steel3DBlock.GetProfile(attr);
            Solid solid = cut.ConvertToSolid();
            List<Solid> resultSolid = new List<Solid>();
            //foreach (BlockReference item in entities)
            //{

            //    if (item.Attributes.ContainsKey("Steel"))
            //    {
            //        Mesh main = ((Mesh)model.Blocks[item.BlockName].Entities[0]);
            //        main.Weld();
            //        Solid[] solids = Solid.Difference(solid, ((Mesh)model.Blocks[item.BlockName].Entities[0]).ConvertToSolid());
            //        if (solids.Length > 0)
            //        {
            //            resultSolid.Add(solids[0]);
            //            if (solids.Length >= 2)
            //            {
            //                solid = solids[1];
            //            }
            //        }
            //    }


            //}
            //EntityList result =  entities.Where(el => !((BlockReference)el).Attributes.ContainsKey("Cut")).ToList();
            model.Entities.AddRange(entities);
            model.Entities.AddRange(resultSolid);
            ser.SetMaterialModel(materialNumber, model); //儲存素材
        }

        private static void Rotate(IEnumerable<Entity> entities, Point3D origin)
        {
            entities.ForEach(el =>
            {
                el.Rotate(Math.PI, Vector3D.AxisX, origin);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point3Ds"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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
        public static bool Flip(Entity entity, Func<List<Point3D>, double> func, SteelAttr steelAttr, double extendX, out double angleP1, out double angleP2, out double x, Transformation transformation = null)
        {
            List<Point3D> point3Ds = entity.Vertices.ToList();
            if (transformation != null)
                point3Ds.ForEach(p => p.TransformBy(transformation));
            List<Point2D> point2Ds = point3Ds.Select(e => new Point2D(e.X, e.Y)).ToList();
            point3Ds =  UtilityEx.ConvexHull2D(point2Ds, true).Select(e => new Point3D(e.X, e.Y)).ToList();
            double _x = func.Invoke(point3Ds);
            var point = point3Ds.Where(el => el.X == _x).OrderBy(el => el.Y).FirstOrDefault();
            int index = point3Ds.FindIndex(el => el == point);
            //int maxIndex = point3Ds.FindIndex(el => el == maxPoint);
            var origin = point3Ds[index];
            var p1 = point3Ds[CycleIndex(point3Ds, index -1)];
            var p2 = point3Ds[CycleIndex(point3Ds, index +1)];
            x= origin.X;
            angleP1 =AK.Angle(origin, p1, new Point3D(origin.X + extendX, origin.Y));
            angleP2 = AK.Angle(origin, p2, new Point3D(origin.X + extendX, origin.Y));
            angleP1 =Math.Abs(AK.SideLengthC(p1.Y - origin.Y, angleP1)) > 50 ? angleP1 : 0;
            angleP2 =Math.Abs(AK.SideLengthC(p2.Y - origin.Y, angleP1)) > 50 ? angleP2 : 0;
            if (angleP1 % 90 != 0 || angleP2 % 90 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 將 AK v 輪廓轉實體
        /// 2022/09/05 呂宗霖 因GetNcPoint要新增參數 且 此Function無參考 故註解
        /// </summary>
        /// <param name="aK"></param>
        /// <returns></returns>
        //public static Mesh ConvertAKvToMesh(AK aK)
        //{
        //    return ConvertNcPointToMesh(aK.GetNcPoint(), aK.t);
        //}

        /// <summary>
        /// 取得切割物件
        /// </summary>
        /// <param name="points">Nc 點位</param>
        /// <param name="t">物件厚度</param>
        /// <returns></returns>
        public static List<Mesh> GetCutMesh(List<NcPoint3D> points, double t)
        {
            List<Mesh> result = new List<Mesh>();
            for (int i = 1; i < points.Count; i++)
            {
                NcPoint3D p1 = (NcPoint3D)points[i -1].DeepClone();
                NcPoint3D p2 = (NcPoint3D)points[i].DeepClone();
                if (p1.Angle1 != 0 || p1.Angle2 != 0)
                {
                    if (p1.Angle1 != 0 && p1.Angle2 != 0)
                    {
                        Mesh mesh1 = AKCalculateCutMesh(t, points, (NcPoint3D)p1.DeepClone(), (NcPoint3D)p2.DeepClone(), p1.StartAngle1, p1.Angle1);
                        Mesh mesh2 = AKCalculateCutMesh(t, points, p1, p2, p1.StartAngle2, p1.Angle2);
                        result.Add(mesh1);
                        result.Add(mesh2);
                    }
                    else
                    {
                        //List<Line> lines = new List<Line>();
                        //double start = p1.StartAngle1 != 0 ? p1.StartAngle1 : p1.StartAngle2;
                        //Line line1 = new Line(p1, p2);
                        //double radian = p1.Angle1 != 0 ? AK.ConvertToRadian(p1.Angle1) : AK.ConvertToRadian(p1.Angle2); //弧度
                        //double a = t- start; //邊長 A
                        //double b = AK.SideLengthB(a, radian); //邊長 B
                        //Line line2 = (Line)line1.Offset(b, Vector3D.AxisZ); //偏移線段
                        //Point3D[] ds = points.Select(el => (Point3D)el).ToArray();
                        //if (!AK.InsideShape(ds, line2.StartPoint) && !AK.InsideShape(ds, line2.StartPoint)) //判斷偏移線段是否在形狀內
                        //{
                        //    line2 = (Line)line1.Offset(-b, Vector3D.AxisZ);
                        //}
                        //Line line3 = (Line)line1.Clone();
                        //line1.Translate(0, 0, start);
                        //line2.Translate(0, 0, t); //移動到正確位置
                        //line3.Translate(0, 0, t); //移動到正確位置
                        //List<Point3D> cutList = new List<Point3D>();
                        //cutList.AddRange(line1.Vertices);
                        //cutList.AddRange(line2.Vertices);
                        //cutList.AddRange(line3.Vertices);
                        Mesh mesh = AKCalculateCutMesh(t, points, p1, p2,
                        p1.StartAngle1 != 0 ? p1.StartAngle1 : p1.StartAngle2,
                        p1.Angle1 != 0 ? p1.Angle1 : p1.Angle2);
                        result.Add(mesh);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 計算 <see cref="NcPoint3D"/> 切割物件
        /// </summary>
        /// <param name="t">厚度</param>
        /// <param name="points">Nc切割點位</param>
        /// <param name="p1">第一點</param>
        /// <param name="p2">第二點</param>
        /// <param name="startAngle">起始位置</param>
        /// <param name="angle">角度</param>
        private static Mesh AKCalculateCutMesh(double t, List<NcPoint3D> points, NcPoint3D p1, NcPoint3D p2, double startAngle, double angle)
        {
            List<Line> lines = new List<Line>();
            //double start = p1.StartAngle1 != 0 ? p1.StartAngle1 : p1.StartAngle2;
            Line line1 = new Line(p1, p2);
            double radian = AK.ConvertToRadian(angle); //角度轉弧度
            double a = angle >= 0 ? t- startAngle : startAngle; //邊長 A
            double b = AK.SideLengthB(a, radian); //邊長 B
            double lineZ = angle >= 0 ? t : 0; //line2 and line3 的 Z 軸位置
            Line line2 = (Line)line1.Offset(b, Vector3D.AxisZ); //偏移線段
            Point3D[] ds = points.Select(el => (Point3D)el).ToArray();
            if (!AK.InsideShape(ds, line2.StartPoint) && !AK.InsideShape(ds, line2.EndPoint)) //判斷偏移線段是否在形狀內
            {
                line2 = (Line)line1.Offset(-b, Vector3D.AxisZ);
            }
            Line line3 = (Line)line1.Clone();
            line1.Translate(0, 0, startAngle);
            line2.Translate(0, 0, lineZ); //移動到正確位置
            line3.Translate(0, 0, lineZ); //移動到正確位置
            List<Point3D> cutList = new List<Point3D>();
            cutList.AddRange(line1.Vertices);
            cutList.AddRange(line2.Vertices);
            cutList.AddRange(line3.Vertices);
            return UtilityEx.ConvexHull(cutList);
        }

        /// <summary>
        /// 將 nc 座標轉實體
        /// </summary>
        /// <param name="points"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Mesh ConvertNcPointToMesh(List<NcPoint3D> points, double t)
        {
            Point3D[] point3Ds = points.Select(el => new Point3D(el.X, el.Y)).ToArray();
            LinearPath profile = new LinearPath(point3Ds);
            devDept.Eyeshot.Entities.Region region1 = new devDept.Eyeshot.Entities.Region(profile, Plane.XY, false);
            Mesh result = region1.ExtrudeAsMesh<Mesh>(new Vector3D(0, 0, t), 0.25, Mesh.natureType.Plain);// 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
            result.Color = System.Drawing.Color.Gray;
            return result;
        }

        /// <summary>
        /// 繪製切割型狀
        /// </summary>
        /// <param name="part"></param>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <param name="start"></param>
        /// <param name="dic"></param>
        private static Entity DrawCutMesh(SteelPart part, devDept.Eyeshot.Model model, double end, double start, string dic)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;
            if (steelAttr.Length ==0)
            {
                return null;
            }
            Steel3DBlock.AddSteel(steelAttr, model, out BlockReference result, dic);
            model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient2);
            result.GroupIndex = int.MaxValue;
            result.Translate(start, 0);
            return result;
        }
        /// <summary>
        /// 繪製切割型狀
        /// </summary>
        /// <param name="part"></param>
        /// <param name="model"></param>
        /// <param name="end"></param>
        /// <param name="start"></param>
        /// <param name="ent"></param>
        /// <param name="dic"></param>
        private static void DrawCutMesh(SteelPart part, devDept.Eyeshot.Model model, double end, double start, EntityList ent, string dic)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;
            Steel3DBlock.AddSteel(steelAttr, model, out BlockReference reference, dic);
            model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient2);
            reference.GroupIndex = int.MaxValue;
            reference.Translate(start, 0);
            ent.Add(reference);
        }

        public static void LoadNoNCToModel(this devDept.Eyeshot.Model model,SteelAttr sa)
        {
            STDSerialization ser = new STDSerialization(); //序列化處理器
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var nc = ncTemps.GetData(sa.GUID.Value.ToString()); //取得nc資訊
            NcTemp reduceNC = new NcTemp() { GroupBoltsAttrs = nc.GroupBoltsAttrs, SteelAttr = nc.SteelAttr };
            model.InitializeViewports();
            //model.Blocks.Add(new Steel3DBlock(Steel3DBlock.GetProfile(sa)));//加入鋼構圖塊到模型
            model.Clear(); //清除目前模型
            Steel3DBlock steel = Steel3DBlock.AddSteel(nc.SteelAttr, model, out BlockReference blockReference);
            string dataName = nc.SteelAttr.GUID.Value.ToString();
            DataCorrespond data = new DataCorrespond()
            {
                DataName = sa.GUID.ToString(),
                Number = sa.PartNumber,
                Type = sa.Type,
                Profile = sa.Profile,
                TP = false,
            };
            ser.SetPartModel(model.Blocks[1].Name, model);

            #region 檢測是否成功，失敗則將NC檔寫回
            ReadFile readFile = ser.ReadPartModel(dataName); //讀取檔案內容
            readFile.DoWork();//開始工作
            try
            {
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                if (model.Blocks.Count() <= 1)
                {
                    ncTemps.Add(reduceNC);
                }
            }
            catch (Exception ex)
            {
                ncTemps.Add(reduceNC);
            }
            #endregion

            ser.SetNcTempList(ncTemps);//儲存檔案
        }




        /// <summary>
        /// 載入 <see cref="NcTemp"/> 到 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataName"></param>
        /// <param name="allowType"></param>
        public static void LoadNcToModel(this devDept.Eyeshot.Model model, string dataName, List<OBJECT_TYPE> allowType, DXSplashScreenViewModel vm = null)
        {
            STDSerialization ser = new STDSerialization(); //序列化處理器
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            NcTemp nc = ncTemps.GetData(dataName); //取得nc資訊
            NcTemp reduceNC = new NcTemp() { GroupBoltsAttrs = nc.GroupBoltsAttrs, SteelAttr = nc.SteelAttr };

            //ObSettingVM obVM = new ObSettingVM();
            if (nc == null)
            {
                if (!allowType.Contains( nc.SteelAttr.Type))
                {
                    return;
                }
                return;
            }
            model.Clear();//清除模型內物件
            Steel3DBlock.AddSteel(nc.SteelAttr, model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
            if (nc.SteelAttr.oPoint.Count != 0)
            {
                //model.Clear();//清除模型內物件
                model.Blocks[steelBlock.BlockName].Entities.Clear();//清除圖塊
                                                                    //前視圖
                Mesh vMesh = ConvertNcPointToMesh(nc.SteelAttr.vPoint, nc.SteelAttr.t1);
                List<Mesh> vCut = GetCutMesh(nc.SteelAttr.vPoint, nc.SteelAttr.t1);
                Mesh cut1 = Mesh.CreateBox(nc.SteelAttr.Length +10, nc.SteelAttr.t2, nc.SteelAttr.t1);//切割前視圖翼板輪廓
                cut1.Translate(-5, 0);
                Mesh otherCut1 = (Mesh)cut1.Clone();
                Mesh cut2 = (Mesh)cut1.Clone();
                cut2.Translate(0, nc.SteelAttr.H - nc.SteelAttr.t2);
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
                    mesh.Vertices.Where(x=>x.Z==nc.SteelAttr.t1).ForEach(el =>
                    {
                        el.Z = nc.SteelAttr.W;
                        //if (el.Z == nc.SteelAttr.t1)
                        //{
                        //    el.Z  = nc.SteelAttr.W;
                        //}
                    });
                    mesh.Regen(1E3);
                });
                vCut.Add(cut1);
                vCut.Add(cut2);
                Solid vSolid = vMesh.ConvertToSolid();
                vSolid = vSolid.Difference(vCut);
                vMesh = vSolid.ConvertToMesh();
                vMesh.Color =ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                vMesh.ColorMethod = colorMethodType.byEntity;
                vMesh.Translate(0, 0, nc.SteelAttr.W * 0.5 - nc.SteelAttr.t1 * 0.5);

                //頂視圖
                Mesh oMesh = ConvertNcPointToMesh(nc.SteelAttr.oPoint, nc.SteelAttr.t2);
                List<Mesh> oCut = GetCutMesh(nc.SteelAttr.oPoint, nc.SteelAttr.t2);
                Solid oSolid = oMesh.ConvertToSolid();
                oSolid = oSolid.Difference(oCut);
                oSolid.Mirror(Vector3D.AxisY, new Point3D(-10, 0, nc.SteelAttr.t2*0.5), new Point3D(10, 0, nc.SteelAttr.t2*0.5));
                oSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
                oSolid.Translate(0, nc.SteelAttr.H);
                //oSolid = oSolid.Difference(cutMeshs);
                oMesh = oSolid.ConvertToMesh();
                oMesh.Color = ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                oMesh.ColorMethod = colorMethodType.byEntity;

                //底視圖
                Mesh uMesh = ConvertNcPointToMesh(nc.SteelAttr.uPoint, nc.SteelAttr.t2);
                List<Mesh> uCut = GetCutMesh(nc.SteelAttr.uPoint, nc.SteelAttr.t2);
                Solid uSolid = uMesh.ConvertToSolid();
                uSolid = uSolid.Difference(uCut);
                uSolid.Rotate(Math.PI / 2, Vector3D.AxisX);
                uSolid.Translate(0, nc.SteelAttr.t2);
                //uSolid = uSolid.Difference(cutMeshs);
                uMesh = uSolid.ConvertToMesh();
                uMesh.Color = ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                uMesh.ColorMethod = colorMethodType.byEntity;

                vMesh.MergeWith(oMesh);
                
                vMesh.MergeWith(uMesh);
                vMesh.EntityData = nc.SteelAttr;
                model.Blocks[steelBlock.BlockName].Entities.Add(vMesh);
                model.Refresh();
                //////前視圖
                ////Mesh vMesh = ConvertNcPointToMesh(nc.SteelAttr.vPoint, nc.SteelAttr.t1);
                ////List<Mesh> vCut = GetCutMesh(nc.SteelAttr.vPoint, nc.SteelAttr.t1);
                ////Mesh cut1 = Mesh.CreateBox(nc.SteelAttr.Length +10, nc.SteelAttr.t2, nc.SteelAttr.t2 +10);//切割前視圖翼板輪廓
                ////cut1.Translate(-5, 0);
                ////Mesh cut2 = (Mesh)cut1.Clone();
                ////cut2.Translate(0, nc.SteelAttr.H - nc.SteelAttr.t2);
                ////vCut.Add(cut1);
                ////vCut.Add(cut2);
                ////Solid vSolid = vMesh.ConvertToSolid();
                ////vSolid = vSolid.Difference(vCut);
                ////vMesh = vSolid.ConvertToMesh();
                ////vMesh.Color = ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                ////vMesh.ColorMethod = colorMethodType.byEntity;
                ////vMesh.Translate(0, 0, nc.SteelAttr.W * 0.5 - nc.SteelAttr.t1 * 0.5);

                //////頂視圖
                ////Mesh oMesh = ConvertNcPointToMesh(nc.SteelAttr.oPoint, nc.SteelAttr.t2);
                ////List<Mesh> oCut = GetCutMesh(nc.SteelAttr.oPoint, nc.SteelAttr.t2);
                ////Solid oSolid = oMesh.ConvertToSolid();
                ////oSolid = oSolid.Difference(oCut);
                ////oMesh = oSolid.ConvertToMesh();
                ////oMesh.Mirror(Vector3D.AxisY, new Point3D(-10, 0, nc.SteelAttr.t2*0.5), new Point3D(10, 0, nc.SteelAttr.t2*0.5));
                ////oMesh.Rotate(Math.PI / 2, Vector3D.AxisX);
                ////oMesh.Translate(0, nc.SteelAttr.H);
                ////oMesh.Color = ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                ////oMesh.ColorMethod = colorMethodType.byEntity;

                //////底視圖
                ////Mesh uMesh = ConvertNcPointToMesh(nc.SteelAttr.uPoint, nc.SteelAttr.t2);
                ////List<Mesh> uCut = GetCutMesh(nc.SteelAttr.uPoint, nc.SteelAttr.t2);
                ////Solid uSolid = uMesh.ConvertToSolid();
                ////uSolid = uSolid.Difference(uCut);
                ////uMesh = uSolid.ConvertToMesh();
                ////uMesh.Rotate(Math.PI / 2, Vector3D.AxisX);
                ////uMesh.Translate(0, nc.SteelAttr.t2);
                ////uMesh.Color = ColorTranslator.FromHtml(Properties.SofSetting.Default.Part);
                ////uMesh.ColorMethod = colorMethodType.byEntity;

                ////vMesh.MergeWith(oMesh);
                ////vMesh.MergeWith(uMesh);
                ////vMesh.EntityData = nc.SteelAttr;
                ////model.Blocks[steelBlock.BlockName].Entities.Add(vMesh);
                ////model.Refresh();
            }

            if (nc.GroupBoltsAttrs != null)
            {
                nc.GroupBoltsAttrs.ForEach(bolt =>
                           {
                               GroupBoltsAttr temp = new GroupBoltsAttr()
                               {
                                   BlockName = bolt.BlockName,
                                   Dia = bolt.Dia,
                                   dX = "0",
                                   dY = "0",
                                   Face = bolt.Face,
                                   GUID = bolt.GUID,
                                   Mode = AXIS_MODE.PIERCE,
                                   StartHole = bolt.StartHole,
                                   t = bolt.t,
                                   Type = bolt.Type,
                                   xCount = 1,
                                   yCount = 1,
                                   X = bolt.X,
                                   Y = bolt.Y,
                                   Z = bolt.Z,
                               };
                               Bolts3DBlock.AddBolts(temp, model, out BlockReference botsBlock, out bool check); //加入到 3d 視圖
                           });
            }
           

            // 寫入oPoint,vPoint,uPoint
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = nc.SteelAttr.oPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = nc.SteelAttr.vPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = nc.SteelAttr.uPoint;


            ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).oPoint = nc.SteelAttr.oPoint;
            ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).vPoint = nc.SteelAttr.vPoint;
            ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).uPoint = nc.SteelAttr.uPoint;


            ser.SetPartModel(dataName, model);//儲存 3d 視圖

            #region 檢測是否成功，失敗則將NC檔寫回
            ReadFile readFile = ser.ReadPartModel(dataName); //讀取檔案內容
            readFile.DoWork();//開始工作
            try
            {
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                if (model.Blocks.Count() <= 1)
                {
                    ncTemps.Add(reduceNC);
                }
            }
            catch (Exception ex)
            {
                ncTemps.Add(reduceNC);
            } 
            #endregion

            ser.SetNcTempList(ncTemps);//儲存檔案
        }
        /// <summary>
        /// 切割線轉換差集實體列表
        /// </summary>
        /// <param name="cutLine">切割線列表</param>
        /// <param name="vector3D">擠出量</param>
        /// <param name="plane">輪廓平面</param>
        /// <param name="secondary">修剪輪廓</param>
        /// <returns></returns>
        public static List<Mesh> CutLineToMeshs(this CutContour cutLine, Vector3D vector3D, Plane plane, List<ICurve> secondary = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("產生多個切割物件").Debug($"開始");
#endif
            return new List<Mesh>()
            {
                cutLine.GetDLCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetDRCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetULCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetURCutPoint().PointToMesh(vector3D, plane),
            };
        }

        /// <summary>
        /// 差集多個物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="meshes">次要物件</param>
        public static Mesh Difference(this Mesh mesh, List<Mesh> meshes)
        {
            //object attr = mesh.EntityData; //暫時存取原本物件的的設定檔
            //foreach (var el in meshes)
            //{
            //    var me = mesh.Difference(el);
            //    mesh = me == null ? mesh : me;
            //}
            //mesh.EntityData = attr;
            //return mesh;
            return mesh.Difference(meshes.ToArray());
        }
        /// <summary>
        /// 差集多個物件
        /// </summary>
        /// <param name="solid">主要物件</param>
        /// <param name="meshes">次要物件</param>
        public static Solid Difference(this Solid solid, List<Mesh> meshes)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                try
                {
                    Solid[] solids = Solid.Difference(solid, meshes[i].ConvertToSolid());
                    if (solids != null)
                    {
                        solid = solids[0];
                    }
                }
                catch (Exception ex)
                {
                    return solid;
                }
               
            }
            return solid;
        }
        /// <summary>
        /// 差集多個物件
        /// </summary>
        /// <param name="solid">主要物件</param>
        /// <param name="meshes">次要物件</param>
        public static Solid Difference(this Solid solid, List<Solid> solids)
        {
            for (int i = 0; i < solids.Count; i++)
            {
                Solid[] _solids = Solid.Difference(solid, solids[i]);
                if (solids != null)
                {
                    solid =  solids[0];
                }
            }
            return solid;
        }
        /// <summary>
        /// 差集多個物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="meshes">次要物件</param>
        public static Mesh Difference(this Mesh mesh, Mesh[] meshes)
        {
            object attr = mesh.EntityData; //暫時存取原本物件的的設定檔
            foreach (var el in meshes)
            {
                var me = mesh.Difference(el);
                mesh = me == null ? mesh : me;
            }
            mesh.EntityData = attr;
            return mesh;
        }
        /// <summary>
        /// 差集物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="secondary">次要物件</param>
        public static Mesh Difference(this Mesh mesh, Mesh secondary)
        {
            try
            {
                if (secondary == null) //如果次件是空值
                    return null;

                Solid[] solids = Solid.Difference(mesh.ConvertToSolid(), secondary.ConvertToSolid()); //差集物件
                if (solids == null)
                    return mesh;

                mesh = solids[0].ConvertToMesh(); //轉換實體
                return mesh;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 點位轉換實體
        /// </summary>
        /// <param name="point3Ds">點位列表</param>
        /// <param name="vector3D">擠出量</param>
        /// <param name="plane">輪廓平面</param>
        /// <param name="secondary">修剪輪廓</param>
        /// <returns></returns>
        public static Mesh PointToMesh(this List<Point3D> point3Ds, Vector3D vector3D, Plane plane, List<ICurve> secondary = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("產生單個切割物件").Debug($"繪製輪廓");
#endif

            if (point3Ds.Count <= 0) //如果切割點為空
            {
                return null;
            }
            List<ICurve> curves = new List<ICurve>() { point3Ds.PointToLinearPath() }; //主要輪廓
                                                                                       //差集物件不等於空值
            if (secondary != null)
                for (int i = 0; i < secondary.Count; i++)
                    curves.Add(secondary[i]);
#if DEBUG
            else
                log4net.LogManager.GetLogger("產生切割物件失敗").Debug($"value is null");
#endif
            // 區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
            Region region = new Region(curves, plane, true);
            // 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
            Mesh result = region.ExtrudeAsMesh(vector3D, 1e-6, Mesh.natureType.Plain);
            return result;
        }
        /// <summary>
        /// 點位轉換路徑
        /// </summary>
        /// <param name="point3Ds"></param>
        /// <returns></returns>
        public static LinearPath PointToLinearPath(this List<Point3D> point3Ds)
        {
            point3Ds.Add(point3Ds[0]);
            return new LinearPath(point3Ds);
        }


    }
}
