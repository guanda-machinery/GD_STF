#define Debug
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using DevExpress.Data.Extensions;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using Region = devDept.Eyeshot.Entities.Region;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 擴展
    /// </summary>
    public static class Expand
    {

        /// <summary>
        /// 組合零件變成素材
        /// </summary>
        /// <param name="model"></param>
        /// <param name="materialNumber">素材編號</param>
        public static void AssemblyPart(this devDept.Eyeshot.Model model, string materialNumber)
        {
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
                model.LoadNcToModel(guid[i]);
            }

            List<int> viewIndex = new List<int>();
            material.Parts.
                ForEach(data => viewIndex.Add(parts.FindIndex(el => el.Number == data.PartNumber)));

            //Dictionary<int, EntityList> keyValuePairs = new Dictionary<int, EntityList>();
            List<int> finish = new List<int>(); //使用過的陣列位置
            List<double> position = new List<double>(); //第一次移動的座標(只記錄 start)，匹配 finish 位置
            double end = material.StartCut; //結束位置
            double start = 0; //起始位置
            EntityList ent = new EntityList();
            //產生3D物件
            for (int i = 0; i < viewIndex.Count; i++)
            {
                model.Entities.Clear();

                //產生代表餘料的 3D 實體
                DrawCutMesh(parts[0], model, end, start, ent);

                if (finish.Contains(viewIndex[i]))//如果有反序列化過
                {
                    EntityList list = new EntityList();
                    ent.Where(el => el.GroupIndex == finish.IndexOf(viewIndex[i])).ForEach(el =>
                    {
                        int posIndex = finish.IndexOf(viewIndex[i]);
                        Entity entity = (Entity)el.Clone();
                        entity.GroupIndex = i;
                        double x = start - position[posIndex];
                        entity.Translate(end - position[posIndex], 0);
                        list.Add(entity);
                    });
                    ent.AddRange(list);
                }
                else
                {
                    if (parts[viewIndex[i]].GUID.ToString() != "") //有 NC 檔
                    {
                        ReadFile file = ser.ReadPartModel(parts[viewIndex[i]].GUID.ToString()); //讀取檔案內容
                        file.DoWork();
                        file.AddToScene(model);
                        model.Entities.ForEach(el =>
                        {
                            model.Blocks[((BlockReference)el).BlockName].Entities.ForEach(entity =>
                            {
                                if (entity.EntityData is SteelAttr)
                                {
                                    entity.Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient1);
                                }
                            });
                            el.GroupIndex = i;
                            el.Translate(end, 0);
                            ent.Add(el);
                        });
                        position.Add(start);
                        finish.Add(viewIndex[i]);
                    }
                    else //如果沒有 NC 檔案就幫用戶產生一隻物件
                    {
                        SteelAttr steelAttr = new SteelAttr(parts[viewIndex[i]]);
                        steelAttr.GUID = parts[viewIndex[i]].GUID = Guid.NewGuid();
                        Steel3DBlock.AddSteel(steelAttr, model, out BlockReference blockReference);
                        blockReference.Translate(end, 0);
                        ent.Add(blockReference);
                        ser.SetPart(material.Profile.GetHashCode().ToString(), new ObservableCollection<object>(parts));
                        ser.SetPartModel(steelAttr.GUID.ToString(), model);
                    }
                }
                start = end + parts[viewIndex[i]].Length;
                end = start + material.Cut;
            }
            DrawCutMesh(parts[0], model, end, material.LengthList[material.LengthIndex] + material.StartCut + material.EndCut, ent);

            model.Entities.Clear();
            model.Entities.AddRange(ent);
            //model.Entities.Regen();
            //model.Refresh();
            //model.Invalidate();
        }
        private static void DrawCutMesh(SteelPart part, devDept.Eyeshot.Model model, double end, double start, EntityList ent)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;
            Steel3DBlock.AddSteel(steelAttr, model, out BlockReference reference, "Cut");
            model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient2);
            reference.GroupIndex = int.MaxValue;
            reference.Translate(start, 0);
            ent.Add(reference);
        }
        /// <summary>
        /// 載入 <see cref="NcTemp"/> 到 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataName"></param>
        public static void LoadNcToModel(this devDept.Eyeshot.Model model, string dataName)
        {
            STDSerialization ser = new STDSerialization(); //序列化處理器
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            NcTemp nc = ncTemps.GetData(dataName); //取得nc資訊
            model.Clear();//清除模型內物件
            Steel3DBlock.AddSteel(nc.SteelAttr, model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
            nc.GroupBoltsAttrs.ForEach(bolt =>
            {
                Bolts3DBlock.AddBolts(bolt, model, out BlockReference botsBlock); //加入到 3d 視圖
            });
            ser.SetPartModel(dataName, model);//儲存 3d 視圖
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
            object attr = mesh.EntityData; //暫時存取原本物件的的設定檔
            foreach (var el in meshes)
            {
                var me = mesh.Difference(el);
                mesh = me == null ? mesh : me;
            }
            mesh.EntityData = attr;
            return mesh;
        }

#pragma warning disable CS1572 // XML 註解中的 'meshes' 有 param 標籤，但沒有該名稱的參數
        /// <summary>
        /// 差集物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="secondary">次要物件</param>
        /// <param name="meshes">旋轉弧度</param>
        public static Mesh Difference(this Mesh mesh, Mesh secondary)
#pragma warning restore CS1572 // XML 註解中的 'meshes' 有 param 標籤，但沒有該名稱的參數
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
