using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Utils.Extensions;
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
using WPFSTD105.Attribute;
using WPFSTD105.Model;

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
        /// 加入外部輪廓
        /// </summary>
        /// <returns></returns>
        private void AK(ref SteelAttr steelAttr)
        {
            //TODO: 形狀分析尚未完成
        }
        /// <summary>
        /// 加入鋼構訊息
        /// </summary>
        /// <param name="steel"></param>
        /// <param name="str"></param>
        /// <param name="line"></param>
        /// <returns>繼續讀取文件回傳 true，不讀取文件回傳 false</returns>
        private bool Info(ref SteelAttr steel, string str, int line)
        {
            string value = str.Trim();//要加入的值
            switch ((NcLine)line)
            {
                case NcLine.PartNumber:
                    string dataName = steel.Profile.GetHashCode().ToString(); //檔案名稱
                    if (File.Exists($@"{ApplicationVM.DirectorySteelPart()}\{dataName}.lis")) //判斷是否有存在此檔案
                    {
                        STDSerialization ser = new STDSerialization();//序列化處理器
                        ObservableCollection<SteelPart> steelParts = ser.GetPart(dataName); //反列化物件]
                        int index = steelParts.FindIndex(el => el.Number == value);//列表位置
                        if (index != -1) //列表內有物件
                        {
                            steelParts[index].Nc = true;
                            steelParts[index].GUID = steel.GUID; //填入
                            steel.WriteProfile(steelParts[index]); //寫入斷面規格參數
                            steel.TeklaPartID = steelParts[index].ID.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將零件 ID 組起來
                            steel.TeklaAssemblyID = steelParts[index].Father.Select(el => el.ToString()).Aggregate((str1, str2) => $"{str1},{str2}"); //將構件 ID 組起來
                            steel.PartNumber = steelParts[index].Number;//寫入零件編號
                            ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies(); //構件列表
                            List<string> assNumber = new List<string>();//構件編號
                            steelParts[index].Father.ForEach(fatherID =>//取得指定零件的所有構件 ID
                            {
                                int indexAsse = assemblies.FindIndex(el => el.ID.Contains(fatherID)/*符合零件指定的構件ID*/);//構件索引位置
                                assNumber.Add(assemblies[indexAsse].Number);//加入到構件編號集合內
                            });
                            steel.AsseNumber = assNumber.Aggregate((str1, str2) => $"{str1},{str2}");//組合構件編號
                            ser.SetPart(dataName, new ObservableCollection<object>(steelParts));//存入模型零件列表
                        }
                        else //列表內沒有物件
                        {
                            return false;
                        }
                    }
                    return false;
                case NcLine.Material:
                    steel.Material = value;//加入材質
                    break;
                case NcLine.Profile:
                    steel.Profile = value;//加入斷面規格
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
                result = Convert.ToDouble(matches[0]);
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
            string[] par = data.Split(' ');//參數
            //面的處理方法順便賦予螺栓與Z軸位置
            {
                FACE face;//螺栓面在的面
                switch (par[0])
                {
                    case "o":
                        face = FACE.TOP;
                        //斷面規格類型
                        switch (steelAttr.Type)
                        {
                            case OBJETC_TYPE.BH:
                            case OBJETC_TYPE.RH:
                                result.Z = steelAttr.W * 0.5 - steelAttr.t1 * 0.5;
                                break;
                            case OBJETC_TYPE.BOX:
                            case OBJETC_TYPE.CH:
                                result.Z = steelAttr.W - steelAttr.t1;
                                break;
                            case OBJETC_TYPE.L:
                                result.Z = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "v":
                        face = FACE.FRONT;
                        result.t = steelAttr.t2;
                        result.Z = steelAttr.t2;
                        break;
                    case "h":
                        face = FACE.BACK;
                        result.t = steelAttr.t2;
                        result.Z = steelAttr.H;
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
            //螺栓座標的處理方法
            {
                /*value[0] = x座標
                  * value[1] = y座標
                  * value[2] = 直徑*/
                double[] value = new double[3];
                for (int i = 1; i < par.Length; i++)
                {
                    value[i - 1] = GetValue(par[i]);
                }
                result.X = value[0];
                result.Y = value[1];
                result.Dia = value[2];
                result.StartHole = START_HOLE.START;
                result.xCount = 1;
                result.yCount = 1;
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
        public bool Load(DXSplashScreenViewModel vm = null)
        {
            try
            {
                STDSerialization ser = new STDSerialization();
                ObservableCollection<DataCorrespond> DataCorrespond = ser.GetDataCorrespond();// 3d model 零件
                double number = GetAllNcPath(ApplicationVM.DirectoryNc()).Count(); //檔案數量
                if (vm != null)
                {
                    vm.Status = _Status;
                }
                foreach (var path in GetAllNcPath(ApplicationVM.DirectoryNc())) //逐步展開nc檔案
                {
                    string dataName = Path.GetFileName(path);//檔案名稱
                    string line; //資料行
                    int lineNumber = 0;//資料行
                    using (StreamReader stream = new StreamReader(path, Encoding.Default))
                    {
                        SteelAttr steelAttr = new SteelAttr();//定義鋼構屬性
                        string blockName = string.Empty; //資料行的標示區塊，例如AK or BO
                        bool save = true; //檔案是否要儲存，需要true，不需要則false
                        List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();//螺栓設定檔
                        while ((line = stream.ReadLine()) != null)
                        {

                            if (!System.Enum.IsDefined(typeof(NcLine), lineNumber))
                            {
                                if (!Info(ref steelAttr, line, lineNumber)) //如果不繼續讀取文件結束
                                {
                                    save = false;//檔案不需要儲存
                                    break;
                                }
                            }
                            else
                            {
                                if (blockName.Contains("AK", "BO", "SI", "IK", "PU", "KO", "KA")) //是標示區塊
                                {
                                    blockName = line.Trim();
                                }
                                else
                                {
                                    if (blockName == "BO")//如果是螺栓
                                        BO(line, steelAttr);

                                }
                            }
                            lineNumber++;
                            if (save) //檔案要序列化
                            {
                                devDept.Eyeshot.Model model = new devDept.Eyeshot.Model(); //產生模型
                                Steel3DBlock.AddSteel(steelAttr, model, out BlockReference steelReference); //產生型鋼形狀
                                for (int i = 0; i < groups.Count; i++)
                                {
                                    Bolts3DBlock.AddBolts(groups[i], model, out BlockReference block); //產生孔位
                                }
                                ser.SetModel(steelAttr.GUID.ToString(), model); //存取模型
                                DataCorrespond.Add(new DataCorrespond()
                                {
                                    DataName = steelAttr.GUID.ToString(),
                                    Number = steelAttr.PartNumber,
                                    Type = steelAttr.Type,
                                    Profile = steelAttr.Profile
                                });
                            }
                        }
                        ser.SetDataCorrespond(DataCorrespond);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"轉換失敗{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return false;
            }
        }
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
            PartNumber = 4,
            /// <summary>
            /// 材質
            /// </summary>
            Material = 6,
            /// <summary>
            /// 斷面規格
            /// </summary>
            Profile = 8,
        }
    }
}
