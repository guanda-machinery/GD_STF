
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.Properties.SofSetting;
using static WPFSTD105.Tekla.TeklaNcFactory;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 代表鋼材 3D 塊繪製函數
    /// </summary>
    public class Steel3DBlock : Block
    {

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Steel3DBlock.Radian' 的 XML 註解
        public const double Radian = 0.09;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Steel3DBlock.Radian' 的 XML 註解
        /// <summary>
        /// 標準函數
        /// </summary>
        /// <param name="mesh"></param>
        public Steel3DBlock(Mesh mesh) : base(((SteelAttr)mesh.EntityData).GUID.ToString())
        {
            try
            {
                //檢查自定義屬性檔是不是空值
                log4net.LogManager.GetLogger("檢查").Debug("屬性檔是不是空值");
                if (mesh.EntityData == null)
                    throw new Exception("mesh EntityData is null");

                //檢查屬性檔案是否正確
                log4net.LogManager.GetLogger("檢查").Debug("屬性檔案是否正確");
                if (mesh.EntityData.GetType() != typeof(SteelAttr))
                    throw new Exception($"mesh EntityData type is not {typeof(SteelAttr)}");

                this.Units = linearUnitsType.Millimeters;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

            this.Entities.Add(mesh);//加入物件到圖塊內
        }

        public Steel3DBlock() { }
        ///// <summary>
        ///// 鋼構設定檔
        ///// </summary>
        //public SteelAttr SteelAttr { get => this.Entities.Count == 0 ? null : (SteelAttr)this.Entities[0].EntityData; }
        /// <summary>
        /// 需要產生的面
        /// </summary>
        /// <param name="mesh">要切割的實體</param>
        /// <param name="face">要切割的面</param>
        private static void CutLine(ref Mesh mesh, FACE face)
        {
            try
            {
                SteelAttr steelAttr = (SteelAttr)mesh.EntityData;
                List<Mesh> cut = null;
                switch (face)
                {
                    case FACE.BACK:
                    case FACE.FRONT:
                        //如果用戶沒有設定切割線
                        if (steelAttr.Front == null)
                        {
#if DEBUG
                            log4net.LogManager.GetLogger("頂面切割線產生失敗").Debug($"{nameof(steelAttr)}.{nameof(steelAttr.Top)} is null ");
#endif
                            return;
                        }
                        cut = steelAttr.Front.CutLineToMeshs(new Vector3D(0, 0, steelAttr.H), Plane.XY);//目前傳入的是符合面的2d座標，完成後需要旋轉
                        //旋轉物件並移動到適合位置
                        for (int i = 0; i < cut.Count; i++)
                        {
                            if (cut[i] == null)
                                continue;

                            cut[i].Rotate(Math.PI / 2, Vector3D.AxisX);
                            cut[i].Translate(0, steelAttr.H, 0);
                        }
                        mesh = mesh.Difference(cut);
                        break;
                    case FACE.TOP:
                        //如果用戶沒有設定切割線
                        if (steelAttr.Top == null)
                        {
#if DEBUG
                            log4net.LogManager.GetLogger("頂面切割線產生失敗").Debug($"{nameof(steelAttr)}.{nameof(steelAttr.Front)} is null ");
#endif
                            return;
                        }
                        cut = steelAttr.Top.CutLineToMeshs(new Vector3D(0, 0, steelAttr.W), Plane.XY);
                        mesh = mesh.Difference(cut);
                        break;
                    default:
                        throw new Exception("找不到面");
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// 取得鋼構實體
        /// </summary>
        /// <param name="steelAttr">物件設定檔</param>
        /// <returns></returns>
        public static Mesh GetProfile(SteelAttr steelAttr)
        {

#if DEBUG
            log4net.LogManager.GetLogger("GetProfile").Debug("");
#endif
            List<ICurve> curves = new List<ICurve>() { BaseProfile(steelAttr.H, steelAttr.W, 0, 0) };
            try
            {
                //組合實際斷面規格
                switch (steelAttr.Type)
                {
                    case OBJECT_TYPE.H: //20220805 張燕華 新增斷面規格H型鋼
                    case OBJECT_TYPE.BH:
                    case OBJECT_TYPE.RH:
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), 0, steelAttr.t2));
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), (steelAttr.W * 0.5) + (steelAttr.t1 * 0.5), steelAttr.t2));
#if DEBUG 
                        log4net.LogManager.GetLogger("產生H型鋼").Debug($"H{steelAttr.H},W{steelAttr.W},t1{steelAttr.t1},t2{steelAttr.t2}");
#endif
                        break;
                    case OBJECT_TYPE.LB: //20220805 張燕華 新增斷面規格槽鐵[(程式內標記為LB, 即longitudinal beam)
                    case OBJECT_TYPE.CH:
                        double b = Math.Sin(Radian) * ((steelAttr.W / 2) / Math.Cos(Radian)); //計算斜率點到點
                        double c = Math.Sin(Radian) * ((steelAttr.W / 2 - steelAttr.t1) / Math.Cos(Radian)); //計算斜率點到點
                        Point3D[] point3Ds = new Point3D[]
                        {
                                    new Point3D(0,steelAttr.t2- b,0),
                                    new Point3D(0,steelAttr.H - steelAttr.t2 + b,0),
                                    new Point3D(0,steelAttr.H -steelAttr.t2 - c, steelAttr.W- steelAttr.t1),
                                    new Point3D(0,steelAttr.t2+ c,steelAttr.W-steelAttr.t1),
                                    new Point3D(0,steelAttr.t2- b,0)
                        };
                        LinearPath linearPath = new LinearPath(point3Ds);
                        curves.Add(linearPath);
#if DEBUG
                        log4net.LogManager.GetLogger("產生CH型鋼").Debug($"H{steelAttr.H},W{steelAttr.W},t1{steelAttr.t1},t2{steelAttr.t2}");
#endif
                        break;
                    case OBJECT_TYPE.L:
                        curves.Add(BaseProfile(steelAttr.H - steelAttr.t1, steelAttr.W - steelAttr.t1, steelAttr.t1, steelAttr.t1));
#if DEBUG
                        log4net.LogManager.GetLogger("產生L型鋼").Debug($"H {steelAttr.H},W {steelAttr.W},t1 {steelAttr.t1}");
#endif
                        break;
                    case OBJECT_TYPE.TUBE: //20220805 張燕華 新增斷面規格TUBE
                    case OBJECT_TYPE.BOX:
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), steelAttr.W - (steelAttr.t1 * 2), steelAttr.t1, steelAttr.t2));
#if DEBUG
                        log4net.LogManager.GetLogger("產生BOX型鋼").Debug($"H {steelAttr.H},W {steelAttr.W},t1 {steelAttr.t1},t2 {steelAttr.t2}");
#endif
                        break;
                    default:
                        log4net.LogManager.GetLogger("steelAttr").Debug("失敗");
                        throw new Exception("找不到不符合類型");
                }
                // 區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
                devDept.Eyeshot.Entities.Region region = new devDept.Eyeshot.Entities.Region(curves, Plane.XY, false);
                Mesh result = region.ExtrudeAsMesh<Mesh>(new Vector3D(steelAttr.Length, 0, 0), 1e-6, Mesh.natureType.Plain);// 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
                result.EntityData = steelAttr;

                //切割物件
                CutLine(ref result, FACE.TOP);
                //CutLine(ref result, FACE.BACK);
                CutLine(ref result, FACE.FRONT);
#if DEBUG
                log4net.LogManager.GetLogger("完成").Debug(steelAttr.GUID.ToString());
#endif
                result.EntityData = steelAttr.DeepClone();
                result.Color = ColorTranslator.FromHtml(Default.Part);
                result.ColorMethod = colorMethodType.byEntity;
                return result;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("steelAttr").Debug(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 螺栓文字列表
        /// </summary>
        private List<string> Bolts { get; set; } = new List<string>();
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK vAK { get; set; }
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK oAK { get; set; }
        /// <summary>
        /// 上視圖形狀
        /// </summary>
        private AK uAK { get; set; }
        public void ReadNcFile(string path, Dictionary<string, ObservableCollection<SteelAttr>> profile, SteelAttr steelAttr, ref SteelAttr steelAttr1,ref List<GroupBoltsAttr> groups)
        {
            string line = "";
            int lineNumber = 0;//資料行
            string face = null; //形狀輪廓的面
                                //SteelAttr steelAttr = new SteelAttr();//定義鋼構屬性
            string blockName = string.Empty; //資料行的標示區塊，例如AK or BO
            bool save = true; //檔案是否要儲存，需要true，不需要則false
             groups = new List<GroupBoltsAttr>();//螺栓設定檔
            Bolts = new List<string>();
            oAK = new AK();
            uAK = new AK();
            vAK = new AK();
            using (StreamReader stream = new StreamReader(path, Encoding.Default))
            {
                STDSerialization ser = new STDSerialization();
                #region 讀NC檔內容

                while ((line = stream.ReadLine().Trim()) != null)
                {
                    if (System.Enum.IsDefined(typeof(NcLine), lineNumber))
                    {
                        if (!Info(ref steelAttr, line, lineNumber, profile)) //如果不繼續讀取文件結束
                        {
                            save = false;//檔案不需要儲存
                            break;
                        }
                    }
                    else
                    {
                        if (line.Contains("AK", "BO", "IK", "PU", "KO", "KA")) //輪廓螺栓標示區號
                        {
                            blockName = line.Trim();
                            face = null;//清除目前的視圖標記
                        }
                        else if (line.Contains("SI")) //鋼印標示區號
                        {
                            line = stream.ReadLine().Trim();
                            if (!Info(ref steelAttr, line, int.MaxValue, profile)) //如果不繼續讀取文件結束
                            {
                                save = false;//檔案不需要儲存
                                break;
                            }
                        }
                        else
                        {
                            if (line == "EN") //nc1 結束符號
                            {
                                break;
                            }
                            if (blockName == "AK")
                            {
                                if (face == null)
                                {
                                    string[] str = line.Split(' ').Where(el => el != string.Empty).ToArray();
                                    if (str[0] == "v")
                                    {
                                        face = "v";
                                        vAK.Parameter.Add(line.Split('v')[1]);
                                    }
                                    else if (str[0] == "o")
                                    {
                                        face = "o";
                                        oAK.Parameter.Add(line);
                                    }
                                    else if (str[0] == "u")
                                    {
                                        face = "u";
                                        uAK.Parameter.Add(line);
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
                            else if (blockName == "BO")//如果是螺栓
                            {
                                Bolts.Add(line);
                            }
                        }
                    }
                    lineNumber++;
                }
                #endregion
            }
            SteelAttr tempSA = new SteelAttr();
            List<GroupBoltsAttr> tempGroups = new List<GroupBoltsAttr>();
            tempSA = steelAttr;
            Bolts.ForEach(el => tempGroups.Add(BO(el, tempSA)));
            oAK.t = uAK.t = steelAttr.t2 == 0 ? steelAttr.t1 : steelAttr.t2;
            vAK.t = steelAttr.t1;
            steelAttr.oPoint = oAK.GetNcPoint(steelAttr.Type);
            steelAttr.vPoint = vAK.GetNcPoint(steelAttr.Type);
            steelAttr.uPoint = uAK.GetNcPoint(steelAttr.Type);
            steelAttr1 = (SteelAttr)steelAttr.DeepClone();
            groups = tempGroups;
            //return steelAttr;

        }

        private bool Info(ref SteelAttr steel, string str, int line, Dictionary<string, ObservableCollection<SteelAttr>> steelAttr)
        {
            string value = str.Trim();//要加入的值
            switch ((NcLine)line)
            {
                case NcLine.Length:
                    steel.Length = Convert.ToDouble(value);
                    break;
                case NcLine.PartNumber:
                    steel.PartNumber = value;
                    break;
                case NcLine.Material:
                    steel.Material = value;//加入材質
                    break;
                case NcLine.Profile:
                    steel.Profile = value;//加入斷面規格
                    foreach (KeyValuePair<string, ObservableCollection<SteelAttr>> item in steelAttr)
                    {
                        foreach (var sa in item.Value.Where(x => x.Profile == value))
                        {
                            steel.Type = sa.Type;
                            break;
                        }
                    }
                    break;
                case NcLine.SI:
                    string[] array = str.Split(' ');
                    steel.PartNumber = array[array.Length - 1].Split('#')[0];
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// nc 轉螺栓設定檔
        /// </summary>
        /// <param name="data"></param>
        /// <param name="steelAttr"></param>
        /// <returns></returns>
        private GroupBoltsAttr BO(string data, SteelAttr steelAttr)
        {
            GroupBoltsAttr result = new GroupBoltsAttr();
            /*par[0] = 參考哪個面
             * par[1] = x座標
             * par[2] = y座標
             * par[3] = 直徑*/
            string[] par = data.Split(' ').Where(el => el != string.Empty).ToArray();//參數
            FACE face = FACE.TOP;//螺栓面在的面
            //面的處理方法順便賦予螺栓與Z軸位置
            {
                switch (par[0])
                {
                    case "v":
                        face = FACE.TOP;
                        result.t = steelAttr.t1;//孔位高度
                        //斷面規格類型
                        switch (steelAttr.Type)
                        {
                            case OBJECT_TYPE.BH:
                            case OBJECT_TYPE.RH:
                                result.Z = steelAttr.W * 0.5 - steelAttr.t1 * 0.5;
                                break;
                            case OBJECT_TYPE.BOX:
                            case OBJECT_TYPE.CH:
                                result.Z = steelAttr.W - steelAttr.t1;
                                break;
                            case OBJECT_TYPE.L:
                                result.Z = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "o":
                        face = FACE.BACK;
                        result.t = steelAttr.t2;
                        result.Z = steelAttr.H;
                        break;
                    case "u":
                        face = FACE.FRONT;
                        result.t = steelAttr.t2;
                        result.Z = steelAttr.t2;
                        break;
                    default:
                        //Debugger.Break();
                        break;
                }
            }
            //螺栓座標的處理方法
            {
                /*value[0] = x座標
                  * value[1] = y座標
                  * value[2] = 直徑*/
                //double[] value = new double[3];
                //for (int i = 1; i < par.Length; i++)
                //{
                //    value[i - 1] = GetValue(par[i]);
                //}
                double[] value = new double[3];
                for (int i = 0; i < value.Length; i++)
                {
                    value[i] = GetValue(par[i + 1]);
                }
                result.X = value[0];
                result.Y = value[1];
                result.Dia = value[2];
                result.StartHole = START_HOLE.START;
                result.xCount = 1;
                result.yCount = 1;
                result.Face = face;
                result.Mode = AXIS_MODE.PIERCE;
            }
            return result;
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
#if DEBUG
            for (int i = 0; i < point3Ds.Length; i++)
            {
                Console.WriteLine($"new Point3D(0, {point3Ds[i].Y}, {point3Ds[i].Z})");
                log4net.LogManager.GetLogger($"產生Base[{i}]").Debug($@"{point3Ds[i]}");
            }

#endif
            return result;
        }
        /// <summary>
        /// 加入鋼構圖塊到模型
        /// </summary>
        /// <param name="steelAttr">鋼構設定檔</param>
        /// <param name="model">要被加入的模型</param>
        /// <param name="blockReference">加入到模型的參考圖塊</param>
        /// <param name="dic">要寫入到 <see cref="BlockReference.Attributes"/> 的文字</param>
        /// <returns></returns>
        public static Steel3DBlock AddSteel(SteelAttr steelAttr, devDept.Eyeshot.Model model, out BlockReference blockReference, string dic = "Steel")
        {

#if DEBUG
            log4net.LogManager.GetLogger("AddSteel").Debug("");
#endif
            Steel3DBlock result;
            // 尚未連動 重新代值進Steel
            STDSerialization ser = new STDSerialization();
            if (steelAttr.H == 0 || steelAttr.W == 0 || steelAttr.t1 == 0 || steelAttr.t2 == 0)
            {
                ObservableCollection<SteelAttr> Type_inp = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{steelAttr.Type}.inp");

                foreach (var item in Type_inp)
                {
                    if (item.Profile == steelAttr.Profile)
                    {
                        steelAttr.H = item.H;
                        steelAttr.W = item.W;
                        steelAttr.t1 = item.t1;
                        steelAttr.t2 = item.t2;
                        break;
                    }
                }
            }

            result = new Steel3DBlock(GetProfile(steelAttr));
            model.Blocks.Add(result);//加入鋼構圖塊到模型
            blockReference = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);
            blockReference.EntityData = steelAttr;
            blockReference.Selectable = false;//關閉用戶選擇
            blockReference.Attributes.Add(dic, new AttributeReference(0, 0, 0));
            model.Entities.Insert(0,blockReference);//加入參考圖塊到模型
            return result;
        }

        //private static Mesh a()
        //{
        //    Point3D[] point3Ds1 = new Point3D[]
        //    {
        //      new Point3D(0, 0, 0),
        //      new Point3D(0, 900, 0),
        //      new Point3D(0, 900, 300),
        //      new Point3D(0, 0, 300),
        //      new Point3D(0, 0, 0),
        //    };

        //    Point3D[] point3Ds2 = new Point3D[]
        //    {
        //        new Point3D(0, 28, 0),
        //        new Point3D(0, 872, 0),
        //        new Point3D(0, 872, 142),
        //        new Point3D(0, 28, 142),
        //        new Point3D(0, 28, 0),
        //    };
        //    Point3D[] point3Ds3 = new Point3D[]
        //    {
        //        new Point3D(0, 28, 158),
        //        new Point3D(0, 872, 158),
        //        new Point3D(0, 872, 300),
        //        new Point3D(0, 28, 300),
        //        new Point3D(0, 28, 158)
        //    };

        //    LinearPath linearPath1 = new LinearPath(point3Ds1);
        //    LinearPath linearPath2 = new LinearPath(point3Ds2);
        //    LinearPath linearPath3 = new LinearPath(point3Ds3);
        //    List<ICurve> curves = new List<ICurve>()
        //    {
        //        linearPath1,
        //        linearPath2,
        //        linearPath3
        //    };
        //    devDept.Eyeshot.Entities.Region region1 = new devDept.Eyeshot.Entities.Region(curves, Plane.XY, false);
        //    Mesh mesh = region1.ExtrudeAsMesh<Mesh>(new Vector3D(500, 0, 0), 0.01, Mesh.natureType.Plain);


        //    Point3D[] point3Ds4 = new Point3D[]
        //    {
        //        new Point3D(0, -300, 0),
        //        new Point3D(0, 300, 0),
        //        new Point3D(0, 1300, 0),
        //        new Point3D(0, -300, 0),
        //    };
        //    LinearPath cut = new LinearPath(point3Ds4);

        //    devDept.Eyeshot.Entities.Region region2 = new devDept.Eyeshot.Entities.Region(cut, Plane.XY, true);
        //    Mesh secondary = region2.ExtrudeAsMesh(new Vector3D(0, 0, 900), 1e-5, Mesh.natureType.Plain);
        //    secondary.Rotate(Math.PI / 2, Vector3D.AxisX);
        //    secondary.Translate(0,900, 0);

        //    Solid[] solids = Solid.Difference(mesh.ConvertToSolid(), secondary.ConvertToSolid()); 

        //    mesh = solids[0].ConvertToMesh();

        //    return mesh;
        //}
    }
}
