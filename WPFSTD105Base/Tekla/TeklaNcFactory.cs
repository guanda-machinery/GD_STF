using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Utils.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;

namespace WPFSTD105.Tekla
{
    /// <summary>
    /// 這是一個 nc1 檔案轉 STD 3D Model 的處理器
    /// </summary>
    public class TeklaNcFactory
    {
        /// <summary>
        /// nc 轉 STD 3D Model 
        /// </summary>
        public TeklaNcFactory()
        {                                                                                                                               

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
        /// <summary>
        /// NC檔讀取資訊
        /// </summary>
        public ObservableCollection<DataCorrespond> DataCorrespond { get; set; }
        /// <summary>
        /// NC檔讀取資訊
        /// </summary>
        public NcTempList ncTemps { get; set; }
        /// <summary>
        /// Group後的新零件
        /// </summary>
        public Dictionary<string, ObservableCollection<object>> newPart = new Dictionary<string, ObservableCollection<object>>();
        /// <summary>
        /// 加入鋼構訊息
        /// </summary>
        /// <param name="steel"></param>
        /// <param name="str"></param>
        /// <param name="line"></param>
        /// <returns>繼續讀取文件回傳 true，不讀取文件回傳 false</returns>
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
                    string p = value.Replace("*","X");
                    steel.Profile = p;//加入斷面規格
                    foreach (KeyValuePair<string,ObservableCollection<SteelAttr>> item in steelAttr)
                    {
                        foreach (var sa in item.Value.Where(x=>x.Profile== p))
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
                    value[i]= GetValue(par[i +1]);
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
        /// 取得模型資料夾所有的 nc1 檔案
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>目前模型內的所有 .nc1 檔案</returns>
        private IEnumerable<string> GetAllNcPath(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string ext = Path.GetExtension(d);//副檔名
                    if (ext == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        yield return d;
                    }
                    else
                    {
                        GetAllNcPath(d);
                    }
                }
            }
        }
        private string _Status = "轉換 nc 物件 ..."; 
        /// <summary>
        /// 載入報表物件
        /// </summary>
        /// <param name="vm">為 SplashScreenManager 指定數據和選項的視圖模型。</param>
        /// <returns>載入成功回傳true，失敗則 false。</returns>
        public bool Load(DXSplashScreenViewModel vm = null, string ncPath = null)
        {
            try
            {
                STDSerialization ser = new STDSerialization();
                var profile = ser.GetSteelAttr();
                //ObSettingVM obvm = new ObSettingVM();
                ncPath = ncPath ?? ApplicationVM.DirectoryNc();
                this.DataCorrespond = ser.GetDataCorrespond();// 3d model 零件
                if (this.DataCorrespond ==null)
                {
                    this.DataCorrespond = new ObservableCollection<DataCorrespond>();
                }
                this.ncTemps = ser.GetNcTempList();//NC設定檔
                if (this.ncTemps ==null)
                {
                    this.ncTemps = new NcTempList();
                }
                this.newPart = new Dictionary<string, ObservableCollection<object>>();
                this.DataCorrespond = new ObservableCollection<DataCorrespond>();
                double number = GetAllNcPath(ncPath).Count(); //檔案數量
                if (vm != null)
                {
                    vm.Status = _Status;
                }
                foreach (var path in GetAllNcPath(ncPath)) //逐步展開nc檔案
                {
                    ReadOneNc(ObSettingVM.allowType, profile, path, vm);
                    #region 讀每個NC檔
                    //string dataName = Path.GetFileName(path);//檔案名稱
                    //string line = ""; //資料行
                    //try
                    //{
                    //    using (StreamReader stream = new StreamReader(path, Encoding.Default))
                    //    {
                    //        #region 讀NC檔內容
                    //        line = "";
                    //        int lineNumber = 0;//資料行
                    //        string face = null; //形狀輪廓的面
                    //        SteelAttr steelAttr = new SteelAttr();//定義鋼構屬性
                    //        string blockName = string.Empty; //資料行的標示區塊，例如AK or BO
                    //        bool save = true; //檔案是否要儲存，需要true，不需要則false
                    //        List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();//螺栓設定檔
                    //        Bolts = new List<string>();
                    //        oAK = new AK();
                    //        uAK = new AK();
                    //        vAK = new AK();
                    //        if (vm != null)
                    //        {
                    //            vm.Status = $"{_Status} {dataName}";
                    //        }
                    //        while ((line = stream.ReadLine().Trim()) != null)
                    //        {
                    //            if (System.Enum.IsDefined(typeof(NcLine), lineNumber))
                    //            {
                    //                if (!Info(ref steelAttr, line, lineNumber, profile)) //如果不繼續讀取文件結束
                    //                {
                    //                    save = false;//檔案不需要儲存
                    //                    break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (line.Contains("AK", "BO", "IK", "PU", "KO", "KA")) //輪廓螺栓標示區號
                    //                {
                    //                    blockName = line.Trim();
                    //                    face = null;//清除目前的視圖標記
                    //                }
                    //                else if (line.Contains("SI")) //鋼印標示區號
                    //                {
                    //                    line = stream.ReadLine().Trim();
                    //                    if (!Info(ref steelAttr, line, int.MaxValue, profile)) //如果不繼續讀取文件結束
                    //                    {
                    //                        save = false;//檔案不需要儲存
                    //                        break;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    if (line == "EN") //nc1 結束符號
                    //                    {
                    //                        break;
                    //                    }
                    //                    if (blockName == "AK")
                    //                    {
                    //                        if (face == null)
                    //                        {
                    //                            string[] str = line.Split(' ').Where(el => el != string.Empty).ToArray();
                    //                            if (str[0] == "v")
                    //                            {
                    //                                face = "v";
                    //                                vAK.Parameter.Add(line.Split('v')[1]);
                    //                            }
                    //                            else if (str[0] == "o")
                    //                            {
                    //                                face = "o";
                    //                                oAK.Parameter.Add(line);
                    //                            }
                    //                            else if (str[0] == "u")
                    //                            {
                    //                                face = "u";
                    //                                uAK.Parameter.Add(line);
                    //                            }
                    //                        }
                    //                        else if (face == "v")
                    //                        {
                    //                            vAK.Parameter.Add(line);
                    //                        }
                    //                        else if (face == "o")
                    //                        {
                    //                            oAK.Parameter.Add(line);
                    //                        }
                    //                        else if (face == "u")
                    //                        {
                    //                            uAK.Parameter.Add(line);
                    //                        }
                    //                    }
                    //                    else if (blockName == "BO")//如果是螺栓
                    //                    {
                    //                        Bolts.Add(line);
                    //                    }
                    //                }
                    //            }
                    //            lineNumber++;
                    //        }
                    //        #endregion

                    //        if (save && obvm.allowType.Contains(steelAttr.Type)) //檔案要序列化
                    //        {
                    //            //AK(ref steelAttr);
                    //            string fileName = steelAttr.Profile.GetHashCode().ToString(); //檔案名稱
                    //            if (File.Exists($@"{ApplicationVM.DirectorySteelPart()}\{fileName}.lis")) //判斷是否有存在此檔案
                    //            {
                    //                ObservableCollection<SteelPart> steelPartsFile = ser.GetPart(fileName); //反列化物件
                    //                                                                                        //string partNumber = steelAttr.PartNumber;
                    //                                                                                        //var steelPartsFileFilter = (from a in steelPartsFile where obvm.allowType.Contains(a.Type) select a).ToList();
                    //                                                                                        // 2022/10/21 呂宗霖 調整零件清單列表條件
                    //                                                                                        //int index = steelParts.FindIndex(el => el.Number == steelAttr.PartNumber);//列表位置

                    //                // 若限制長度 只抓的到與NC檔相同零件編號相同長度的BOM表零件 所以比對零件編號
                    //                var suitList = (from single in steelPartsFile
                    //                                .Where(el =>
                    //                                el.Number == steelAttr.PartNumber &&
                    //                                el.Profile == steelAttr.Profile
                    //                                //&& el.Length == steelAttr.Length
                    //                                )
                    //                                select single).ToList();

                    //                #region 依照長度分組
                    //                var steelParts = (from aa in steelPartsFile
                    //                                                              .Where(el =>
                    //                                                              el.Number == steelAttr.PartNumber &&
                    //                                                              el.Profile == steelAttr.Profile
                    //                                                              //&& el.Length == steelAttr.Length
                    //                                                              )
                    //                                  group aa by new { aa.Number, aa.Profile, aa.Length, aa.Type } into g
                    //                                  select new SteelPart
                    //                                  {
                    //                                      Length = g.Key.Length,
                    //                                      Number = g.Key.Number,
                    //                                      Profile = g.Key.Profile,
                    //                                      Type = g.Key.Type,

                    //                                      Count = g.Sum(x => x.Count),

                    //                                      GUID = Guid.NewGuid(),
                    //                                      Nc = true,

                    //                                      Father = new List<int>(),
                    //                                      ID = new List<int>(),
                    //                                      Match = new List<bool>(),

                    //                                      DrawingName = g.FirstOrDefault().DrawingName,
                    //                                      ExclamationMark = g.FirstOrDefault().ExclamationMark,
                    //                                      Lock = g.FirstOrDefault().Lock,
                    //                                      Material = g.FirstOrDefault().Material,
                    //                                      Phase = g.FirstOrDefault().Phase,
                    //                                      ShippingNumber = g.FirstOrDefault().ShippingNumber,
                    //                                      State = g.FirstOrDefault().State,
                    //                                      H = g.FirstOrDefault().H,
                    //                                      t1 = g.FirstOrDefault().t1,
                    //                                      t2 = g.FirstOrDefault().t2,
                    //                                      Title1 = g.FirstOrDefault().Title1,
                    //                                      Title2 = g.FirstOrDefault().Title2,
                    //                                      UnitArea = g.FirstOrDefault().UnitArea,

                    //                                      UnitWeight = g.FirstOrDefault().UnitWeight,
                    //                                      W = g.FirstOrDefault().W
                    //                                  }).ToList();
                    //                foreach (var item in steelParts)
                    //                {
                    //                    var idList = (from a in suitList where a.Length == item.Length select a.ID).ToList();
                    //                    foreach (int id in idList.SelectMany(x => x))
                    //                    {
                    //                        item.ID.Add(id);
                    //                    }
                    //                    var fatherList = (from a in suitList where a.Length == item.Length select a.Father).ToList();
                    //                    foreach (int father in fatherList.SelectMany(x => x))
                    //                    {
                    //                        item.Father.Add(father);
                    //                    }
                    //                    var matchList = (from a in suitList where a.Length == item.Length select a.Match).ToList();
                    //                    foreach (bool match in matchList.SelectMany(x => x))
                    //                    {
                    //                        item.Match.Add(match);
                    //                    }
                    //                }
                    //                #endregion

                    //                int index = steelParts.FindIndex(el =>
                    //               el.Number == steelAttr.PartNumber &&
                    //               //el.Type == steelAttr.Type &&
                    //               el.Profile == steelAttr.Profile &&
                    //               el.Length == steelAttr.Length);//列表位置

                    //                //newPart = new Dictionary<string, ObservableCollection<object>>();

                    //                if (steelParts.Any(el => el.Number == steelAttr.PartNumber && el.Profile == steelAttr.Profile))
                    //                {
                    //                    foreach (var item in steelParts)
                    //                    {
                    //                        item.Nc = true;
                    //                        //item.GUID = steelAttr.GUID; //填入
                    //                        steelAttr.WriteProfile(item); //寫入斷面規格參數
                    //                        steelAttr.GUID = item.GUID;                           //if (steelAttr.Length != item.Length)
                    //                                                      //{
                    //                        steelAttr.Length = item.Length;
                    //                        //    ser.SetPart(fileName, new ObservableCollection<object>(steelParts));
                    //                        //}
                    //                        steelAttr.TeklaPartID = item.ID.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將零件 ID 組起來
                    //                        steelAttr.TeklaAssemblyID = item.Father.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將構件 ID 組起來
                    //                        steelAttr.PartNumber = item.Number;//寫入零件編號

                    //                        // 2022/10/21 呂宗霖 新增 + steelAttr.Number
                    //                        steelAttr.Number = item.Count + steelAttr.Number;//零件數量
                    //                                                                         //steelAttr.Length = item.Length;//寫入長度
                    //                        ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies(); //構件列表
                    //                                                                                                  // 2022/08/26 呂宗霖 避免構件重複顯示，故不存在List中的構件才加入
                    //                                                                                                  // 2022/08/26 架構師說其實這是正常的，雖然構件編號一樣，但其實ID是不同的
                    //                        List<string> assNumber = new List<string>();//構件編號

                    //                        List<string> assNumberList = new List<string>();
                    //                        foreach (var father in item.Father)
                    //                        {
                    //                            assNumberList.AddRange(assemblies.Where(x => x.ID.Contains(father)).Select(x => x.Number).ToList());
                    //                            //steelAttr.AsseNumber = assStr;//組合構件編號
                    //                        }
                    //                        steelAttr.AsseNumber = assNumberList.Distinct().ToList().Aggregate((number1, number2) => $"{number1},{number2}");

                    //                        //var PartInAss = assemblies.Where(x => item.Father.Intersect(x.ID).Count()>0).Select(x=> item.Father.Intersect(x.ID)).ToList();
                    //                        //string partInAssStr = PartInAss.Aggregate((str1, str2) => $"{str1},{str2}");


                    //                        //steelAttr.AsseNumber = assNumber.Aggregate((str1, str2) => $"{str1},{str2}");//組合構件編號
                    //                        if (obvm.allowType.Contains(item.Type))
                    //                        {
                    //                            //item.Father.ForEach(fatherID =>//取得指定零件的所有構件 ID
                    //                            //{
                    //                            //    //int indexAsse = assemblies.FindIndex(el => el.ID.Contains(fatherID)/*符合零件指定的構件ID*/);//構件索引位置
                    //                            //    //assNumber.Add(assemblies[indexAsse].Number);//加入到構件編號集合內                                                
                    //                            //});
                    //                            try
                    //                            {
                    //                                newPart[fileName].Add(item);
                    //                            }
                    //                            catch (Exception ex)
                    //                            {
                    //                                newPart.Add(fileName, new ObservableCollection<object> { item });
                    //                            }
                                                
                    //                            Bolts.ForEach(el => groups.Add(BO(el, steelAttr)));
                    //                            NcTemp nc = new NcTemp { SteelAttr = steelAttr, GroupBoltsAttrs = groups };
                    //                            oAK.t = uAK.t = steelAttr.t2 == 0 ? steelAttr.t1 : steelAttr.t2;
                    //                            vAK.t = steelAttr.t1;
                    //                            steelAttr.oPoint = oAK.GetNcPoint(steelAttr.Type);
                    //                            steelAttr.vPoint = vAK.GetNcPoint(steelAttr.Type);
                    //                            steelAttr.uPoint = uAK.GetNcPoint(steelAttr.Type);
                    //                            //加入未實體化NC檔
                    //                            ncTemps.Add(nc);
                    //                            DataCorrespond data = new DataCorrespond()
                    //                            {
                    //                                DataName = steelAttr.GUID.ToString(),
                    //                                Number = steelAttr.PartNumber,
                    //                                Type = steelAttr.Type,
                    //                                Profile = steelAttr.Profile,
                    //                                TP = true,
                    //                                // 2022/09/08 彥谷
                    //                                oPoint = steelAttr.oPoint == null ? null : steelAttr.oPoint.ToArray(),
                    //                                vPoint = steelAttr.vPoint == null ? null : steelAttr.vPoint.ToArray(),
                    //                                uPoint = steelAttr.uPoint == null ? null : steelAttr.uPoint.ToArray(),
                    //                            };
                    //                            if (data.DataName == nc.SteelAttr.GUID.ToString()) { DataCorrespond.Add(data); }
                    //                        }
                    //                    }
                    //                }



                    //                //if (index != -1) //列表內有物件
                    //                //{
                    //                //    steelParts[index].Nc = true;
                    //                //    steelParts[index].GUID = steelAttr.GUID; //填入
                    //                //    steelAttr.WriteProfile(steelParts[index]); //寫入斷面規格參數
                    //                //                                               //if (steelAttr.Length != steelParts[index].Length)
                    //                //                                               //{
                    //                //    steelAttr.Length = steelParts[index].Length;
                    //                //    //    ser.SetPart(fileName, new ObservableCollection<object>(steelParts));
                    //                //    //}
                    //                //    steelAttr.TeklaPartID = steelParts[index].ID.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將零件 ID 組起來
                    //                //    steelAttr.TeklaAssemblyID = steelParts[index].Father.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將構件 ID 組起來
                    //                //    steelAttr.PartNumber = steelParts[index].Number;//寫入零件編號

                    //                //    // 2022/10/21 呂宗霖 新增 + steelAttr.Number
                    //                //    steelAttr.Number = steelParts[index].Count + steelAttr.Number;//零件數量
                    //                //    //steelAttr.Length = steelParts[index].Length;//寫入長度
                    //                //    ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies(); //構件列表
                    //                //                                                                              // 2022/08/26 呂宗霖 避免構件重複顯示，故不存在List中的構件才加入
                    //                //                                                                              // 2022/08/26 架構師說其實這是正常的，雖然構件編號一樣，但其實ID是不同的
                    //                //    List<string> assNumber = new List<string>();//構件編號

                    //                //    List<string> assNumberList = new List<string>();
                    //                //    foreach (var father in steelParts[index].Father)
                    //                //    {
                    //                //        assNumberList.AddRange(assemblies.Where(x => x.ID.Contains(father)).Select(x => x.Number).ToList());
                    //                //        //steelAttr.AsseNumber = assStr;//組合構件編號
                    //                //    }
                    //                //    steelAttr.AsseNumber = assNumberList.Distinct().ToList().Aggregate((number1, number2) => $"{number1},{number2}");

                    //                //    //var PartInAss = assemblies.Where(x => steelParts[index].Father.Intersect(x.ID).Count()>0).Select(x=> steelParts[index].Father.Intersect(x.ID)).ToList();
                    //                //    //string partInAssStr = PartInAss.Aggregate((str1, str2) => $"{str1},{str2}");


                    //                //    //steelAttr.AsseNumber = assNumber.Aggregate((str1, str2) => $"{str1},{str2}");//組合構件編號
                    //                //    if (obvm.allowType.Contains(steelParts[index].Type))
                    //                //    {
                    //                //        //steelParts[index].Father.ForEach(fatherID =>//取得指定零件的所有構件 ID
                    //                //        //{
                    //                //        //    //int indexAsse = assemblies.FindIndex(el => el.ID.Contains(fatherID)/*符合零件指定的構件ID*/);//構件索引位置
                    //                //        //    //assNumber.Add(assemblies[indexAsse].Number);//加入到構件編號集合內                                                
                    //                //        //});
                    //                //        ser.SetPart(fileName, new ObservableCollection<object>(steelParts));//存入模型零件列表
                    //                //        Bolts.ForEach(el => groups.Add(BO(el, steelAttr)));
                    //                //        NcTemp nc = new NcTemp { SteelAttr = steelAttr, GroupBoltsAttrs = groups };
                    //                //        oAK.t = uAK.t = steelAttr.t2 == 0 ? steelAttr.t1 : steelAttr.t2;
                    //                //        vAK.t = steelAttr.t1;
                    //                //        steelAttr.oPoint = oAK.GetNcPoint(steelAttr.Type);
                    //                //        steelAttr.vPoint = vAK.GetNcPoint(steelAttr.Type);
                    //                //        steelAttr.uPoint = uAK.GetNcPoint(steelAttr.Type);
                    //                //        //加入未實體化NC檔
                    //                //        ncTemps.Add(nc);
                    //                //        DataCorrespond data = new DataCorrespond()
                    //                //        {
                    //                //            DataName = steelAttr.GUID.ToString(),
                    //                //            Number = steelAttr.PartNumber,
                    //                //            Type = steelAttr.Type,
                    //                //            Profile = steelAttr.Profile,
                    //                //            TP = true,
                    //                //            // 2022/09/08 彥谷
                    //                //            oPoint = steelAttr.oPoint == null ? null : steelAttr.oPoint.ToArray(),
                    //                //            vPoint = steelAttr.vPoint == null ? null : steelAttr.vPoint.ToArray(),
                    //                //            uPoint = steelAttr.uPoint == null ? null : steelAttr.uPoint.ToArray(),
                    //                //        };
                    //                //        if (data.DataName == nc.SteelAttr.GUID.ToString()) { DataCorrespond.Add(data); }
                    //                //    }
                    //                //}
                    //                else //列表內沒有物件
                    //                {
                    //                    continue;
                    //                }

                    //            }
                    //        }
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    WinUIMessageBox.Show(null,
                    // $"轉換失敗{dataName}\n{ex.Message}\n{ex.InnerException}\n{line}",
                    // "錯誤",
                    // MessageBoxButton.OK,
                    // MessageBoxImage.Error,
                    // MessageBoxResult.None,
                    // MessageBoxOptions.None,
                    // FloatingMode.Popup);
                    //} 
                    #endregion
                }

                foreach (var item in newPart)
                {
                    ser.SetPart(item.Key, item.Value);//存入模型零件列表
                }
                ser.SetDataCorrespond(this.DataCorrespond);
                ser.SetNcTempList(this.ncTemps);                
                //this.DataCorrespond = DataCorrespond;
                //this.ncTemps = ncTemps;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"轉換失敗{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"轉換失敗{ex.Message}",
                    "錯誤",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return false;
            }
        }

        

            /// <summary>
            /// 讀取單一NC檔
            /// </summary>
            /// <param name="allowType"></param>
            /// <param name="profile"></param>
            /// <param name="path"></param>
            /// <param name="vm"></param>
            public void ReadOneNc(
            List<OBJECT_TYPE> allowType,
            Dictionary<string, ObservableCollection<SteelAttr>> profile,
            string path, 
            DXSplashScreenViewModel vm = null) 
        {
            string dataName = Path.GetFileName(path);//檔案名稱
            string line = ""; //資料行
            #region 讀NC檔
            try
            {
                using (StreamReader stream = new StreamReader(path, Encoding.Default))
                {
                    STDSerialization ser = new STDSerialization();
                    #region 讀NC檔內容
                    line = "";
                    int lineNumber = 0;//資料行
                    string face = null; //形狀輪廓的面
                    SteelAttr steelAttr = new SteelAttr();//定義鋼構屬性
                    string blockName = string.Empty; //資料行的標示區塊，例如AK or BO
                    bool save = true; //檔案是否要儲存，需要true，不需要則false
                    List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();//螺栓設定檔
                    Bolts = new List<string>();
                    oAK = new AK();
                    uAK = new AK();
                    vAK = new AK();
                    if (vm != null)
                    {
                        vm.Status = $"{_Status} {dataName}";
                    }
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

                    if (save && allowType.Contains(steelAttr.Type)) //檔案要序列化
                    {
                        //AK(ref steelAttr);
                        string fileName = steelAttr.Profile.GetHashCode().ToString(); //檔案名稱
                        if (File.Exists($@"{ApplicationVM.DirectorySteelPart()}\{fileName}.lis")) //判斷是否有存在此檔案
                        {
                            ObservableCollection<SteelPart> steelPartsFile = ser.GetPart(fileName); //反列化物件
                                                                                                    //string partNumber = steelAttr.PartNumber;
                                                                                                    //var steelPartsFileFilter = (from a in steelPartsFile where obvm.allowType.Contains(a.Type) select a).ToList();
                                                                                                    // 2022/10/21 呂宗霖 調整零件清單列表條件
                                                                                                    //int index = steelParts.FindIndex(el => el.Number == steelAttr.PartNumber);//列表位置

                            // 若限制長度 只抓的到與NC檔相同零件編號相同長度的BOM表零件 所以比對零件編號
                            var suitList = (from single in steelPartsFile
                                            .Where(el =>
                                            el.Number == steelAttr.PartNumber &&
                                            el.Profile == steelAttr.Profile
                                            //&& el.Length == steelAttr.Length
                                            )
                                            select single).ToList();

                            #region 依照長度分組
                            var steelParts = (from aa in steelPartsFile
                                                                          .Where(el =>
                                                                          el.Number == steelAttr.PartNumber &&
                                                                          el.Profile == steelAttr.Profile
                                                                          //&& el.Length == steelAttr.Length
                                                                          )
                                              group aa by new { aa.Number, aa.Profile, aa.Length, aa.Type } into g
                                              select new SteelPart
                                              {
                                                  Length = g.Key.Length,
                                                  Number = g.Key.Number,
                                                  Profile = g.Key.Profile,
                                                  Type = g.Key.Type,

                                                  Count = g.Sum(x => x.Count),

                                                  GUID = Guid.NewGuid(),
                                                  Nc = true,

                                                  Father = new List<int>(),
                                                  ID = new List<int>(),
                                                  Match = new List<bool>(),

                                                  DrawingName = g.FirstOrDefault().DrawingName,
                                                  ExclamationMark = g.FirstOrDefault().ExclamationMark,
                                                  Lock = g.FirstOrDefault().Lock,
                                                  Material = g.FirstOrDefault().Material,
                                                  Phase = g.FirstOrDefault().Phase,
                                                  ShippingNumber = g.FirstOrDefault().ShippingNumber,
                                                  State = g.FirstOrDefault().State,
                                                  H = g.FirstOrDefault().H,
                                                  t1 = g.FirstOrDefault().t1,
                                                  t2 = g.FirstOrDefault().t2,
                                                  Title1 = g.FirstOrDefault().Title1,
                                                  Title2 = g.FirstOrDefault().Title2,
                                                  UnitArea = g.FirstOrDefault().UnitArea,

                                                  UnitWeight = g.FirstOrDefault().UnitWeight,
                                                  W = g.FirstOrDefault().W
                                              }).ToList();
                            foreach (var item in steelParts)
                            {
                                var idList = (from a in suitList where a.Length == item.Length select a.ID).ToList();
                                foreach (int id in idList.SelectMany(x => x))
                                {
                                    item.ID.Add(id);
                                }
                                var fatherList = (from a in suitList where a.Length == item.Length select a.Father).ToList();
                                foreach (int father in fatherList.SelectMany(x => x))
                                {
                                    item.Father.Add(father);
                                }
                                var matchList = (from a in suitList where a.Length == item.Length select a.Match).ToList();
                                foreach (bool match in matchList.SelectMany(x => x))
                                {
                                    item.Match.Add(match);
                                }
                            }
                            #endregion

                            int index = steelParts.FindIndex(el =>
                           el.Number == steelAttr.PartNumber &&
                           //el.Type == steelAttr.Type &&
                           el.Profile == steelAttr.Profile &&
                           el.Length == steelAttr.Length);//列表位置

                            //newPart = new Dictionary<string, ObservableCollection<object>>();

                            if (steelParts.Any(el => el.Number == steelAttr.PartNumber && el.Profile == steelAttr.Profile))
                            {
                                foreach (var item in steelParts)
                                {
                                    item.Nc = true;
                                    //item.GUID = steelAttr.GUID; //填入
                                    steelAttr.WriteProfile(item); //寫入斷面規格參數
                                    steelAttr.GUID = item.GUID;                           //if (steelAttr.Length != item.Length)
                                                                                          //{
                                    steelAttr.Length = item.Length;
                                    //    ser.SetPart(fileName, new ObservableCollection<object>(steelParts));
                                    //}
                                    steelAttr.TeklaPartID = item.ID.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將零件 ID 組起來
                                    steelAttr.TeklaAssemblyID = item.Father.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將構件 ID 組起來
                                    steelAttr.PartNumber = item.Number;//寫入零件編號

                                    // 2022/10/21 呂宗霖 新增 + steelAttr.Number
                                    steelAttr.Number = item.Count;// + steelAttr.Number零件數量
                                                                                     //steelAttr.Length = item.Length;//寫入長度
                                    ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies(); //構件列表
                                                                                                              // 2022/08/26 呂宗霖 避免構件重複顯示，故不存在List中的構件才加入
                                                                                                              // 2022/08/26 架構師說其實這是正常的，雖然構件編號一樣，但其實ID是不同的
                                    List<string> assNumber = new List<string>();//構件編號

                                    List<string> assNumberList = new List<string>();
                                    foreach (var father in item.Father)
                                    {
                                        assNumberList.AddRange(assemblies.Where(x => x.ID.Contains(father)).Select(x => x.Number).ToList());
                                        //steelAttr.AsseNumber = assStr;//組合構件編號
                                    }
                                    steelAttr.AsseNumber = assNumberList.Distinct().ToList().Aggregate((number1, number2) => $"{number1},{number2}");

                                    //var PartInAss = assemblies.Where(x => item.Father.Intersect(x.ID).Count()>0).Select(x=> item.Father.Intersect(x.ID)).ToList();
                                    //string partInAssStr = PartInAss.Aggregate((str1, str2) => $"{str1},{str2}");


                                    //steelAttr.AsseNumber = assNumber.Aggregate((str1, str2) => $"{str1},{str2}");//組合構件編號
                                    if (allowType.Contains(item.Type))
                                    {
                                        //item.Father.ForEach(fatherID =>//取得指定零件的所有構件 ID
                                        //{
                                        //    //int indexAsse = assemblies.FindIndex(el => el.ID.Contains(fatherID)/*符合零件指定的構件ID*/);//構件索引位置
                                        //    //assNumber.Add(assemblies[indexAsse].Number);//加入到構件編號集合內                                                
                                        //});
                                        try
                                        {
                                            newPart[fileName].Add(item);
                                        }
                                        catch (Exception ex)
                                        {
                                            newPart.Add(fileName, new ObservableCollection<object> { item });
                                        }

                                        Bolts.ForEach(el => groups.Add(BO(el, steelAttr)));
                                        NcTemp nc = new NcTemp { SteelAttr = steelAttr, GroupBoltsAttrs = groups };
                                        oAK.t = uAK.t = steelAttr.t2 == 0 ? steelAttr.t1 : steelAttr.t2;
                                        vAK.t = steelAttr.t1;
                                        steelAttr.oPoint = oAK.GetNcPoint(steelAttr.Type);
                                        steelAttr.vPoint = vAK.GetNcPoint(steelAttr.Type);
                                        steelAttr.uPoint = uAK.GetNcPoint(steelAttr.Type);
                                        //加入未實體化NC檔
                                        this.ncTemps.Add(nc);
                                        DataCorrespond data = new DataCorrespond()
                                        {
                                            DataName = steelAttr.GUID.ToString(),
                                            Number = steelAttr.PartNumber,
                                            Type = steelAttr.Type,
                                            Profile = steelAttr.Profile,
                                            TP = true,
                                            // 2022/09/08 彥谷
                                            oPoint = steelAttr.oPoint == null ? null : steelAttr.oPoint.ToArray(),
                                            vPoint = steelAttr.vPoint == null ? null : steelAttr.vPoint.ToArray(),
                                            uPoint = steelAttr.uPoint == null ? null : steelAttr.uPoint.ToArray(),
                                        };
                                        if (data.DataName == nc.SteelAttr.GUID.ToString()) { this.DataCorrespond.Add(data); }
                                    }
                                }
                            }
                            else //列表內沒有物件
                            {
                                //continue;
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WinUIMessageBox.Show(null,
             $"轉換失敗\nNC檔名:{dataName}\n錯誤訊息:{ex.Message}\n{ex.InnerException}\n讀取行內容:{line}",
             "錯誤",
             MessageBoxButton.OK,
             MessageBoxImage.Error,
             MessageBoxResult.None,
             MessageBoxOptions.None,
             FloatingMode.Popup);
            }
            #endregion
        }



        //public SaveNCFile(bool save) 
        //{
        //        
        //}

        /// <summary>
        /// nc1 標示區塊
        /// </summary>
        public string[] Block = new[] { "AK", "BO", "SI", "IK", "PU", "KO", "KA" };
        /// <summary>
        /// nc 文字行表示
        /// </summary>
        public enum NcLine
        {
            /// <summary>
            /// 零件編號
            /// </summary>
            PartNumber = 5,
            /// <summary>
            /// 材質
            /// </summary>
            Material = 6,
            /// <summary>
            /// 斷面規格
            /// </summary>
            Profile = 8,
            /// <summary>
            /// 長度
            /// </summary>
            Length = 10,
            /// <summary>
            /// 鋼印
            /// </summary>
            /// <remarks>
            /// nc文件:
            /// SI
            ///   u 5723.28s  270.00    0.00 005  零件編號#構件編號
            /// </remarks>
            SI = int.MaxValue
        }
    }
}
