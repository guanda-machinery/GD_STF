using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Pdf;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFSTD105.Attribute;

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
                        STDSerialization ser = new STDSerialization();
                        ObservableCollection<SteelPart> steelParts = ser.GetPart(dataName);
                        int index = steelParts.FindIndex(el => el.Number == value);//列表位置
                        if (index != -1) //列表內有物件
                        {
                            steelParts[index].Nc = true;
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
        private GroupBoltsAttr[] CoverterGroupBoltsAttr()
        {
            //TODO: 螺栓分析尚未完成
            return null;
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
                        SteelAttr steel = new SteelAttr();//定義鋼構屬性
                        string type = string.Empty; //資料行的型別，例如AK or BO
                        while ((line = stream.ReadLine()) != null)
                        {
                            
                            if (!System.Enum.IsDefined(typeof(NcLine), lineNumber))
                            {
                                if (!Info(ref steel, line, lineNumber)) //如果不繼續讀取文件結束
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (type.Contains("AK", "BO","SI","IK","PU", "KO","KA")) //判斷
                                {

                                }
                            }
                            lineNumber++;
                        }
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
