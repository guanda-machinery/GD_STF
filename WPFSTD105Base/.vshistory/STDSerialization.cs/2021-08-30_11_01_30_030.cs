using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.SerializationHelper;
namespace WPFSTD105
{
    /// <summary>
    /// <see cref="GD_STD.Data"/> 物件序列化處理器
    /// </summary>
    public class STDSerialization
    {

        /// <summary>
        /// 標準建構式
        /// </summary>
        public STDSerialization()
        {
        }
        /// <summary>
        /// 取得目前模型構件資訊列表 (壓縮)
        /// </summary>
        /// <returns>回傳目前模型 <see cref="ApplicationVM.FileSteelAssembly"/> 反序列化物件</returns>
        public ObservableCollection<SteelAssembly> GetGZipAssemblies()
        {
            string path = ApplicationVM.FileSteelAssembly(); //構件表資料路徑
            return GZipDeserialize<ObservableCollection<SteelAssembly>>(path);//解壓縮反序列化回傳資料
        }
        /// <summary>
        /// 存取目前模型構件資訊列表 (壓縮)
        /// </summary>
        /// <param name="steels">要序列化的物件</param>
        public void SetSteelAssemblies(ObservableCollection<SteelAssembly> steels)
        {
            string path = ApplicationVM.FileSteelAssembly(); //構件表資料路徑
            GZipSerializeBinary(steels, path);//壓縮序列化檔案
        }
        /// <summary>
        /// 取得目前模型所有零件列表 (壓縮)
        /// </summary>
        /// <returns>回傳目前模型 <see cref="ApplicationVM.DirectorySteelPart"/> 所有序列化檔案物件</returns>
        public Dictionary<string, ObservableCollection<SteelPart>> GetPart()
        {
            Dictionary<string, ObservableCollection<SteelPart>> result = new Dictionary<string, ObservableCollection<SteelPart>>();
            string dirPath = ApplicationVM.DirectorySteelPart(); //零件資料夾路徑
            DirectoryInfo info = new DirectoryInfo(dirPath);//零件資料夾
            foreach (var item in info.GetFiles("*.lis")) //逐步尋找有關 .lis 檔案
            {
                ObservableCollection<SteelPart> _ = GZipDeserialize<ObservableCollection<SteelPart>>(item.FullName);//解壓縮反序列化只定路徑物件
                result.Add(item.Name, _);
            }
            return result;
        }
        /// <summary>
        /// 取得目前模型指定零件列表 (壓縮)
        /// </summary>
        /// <returns>回傳目前模型 <see cref="ApplicationVM.DirectorySteelPart"/> 指定檔案物件</returns>
        public ObservableCollection<SteelPart> GetPart(string dataName)
        {
            string path = $@"{ApplicationVM.DirectorySteelPart()}\{dataName}.lis"; //零件路徑
            return GZipDeserialize<ObservableCollection<object>>(path).as;//解壓縮反序列化回傳資料
        }
        /// <summary>
        /// 存取目前模型零件列表 (壓縮)
        /// </summary>
        /// <param name="profile">斷面規格</param>
        /// <param name="part">零件清單</param>
        public void SetPart(string profile, ObservableCollection<object> part)
        {
            GZipSerializeBinary(part, $@"{ApplicationVM.DirectorySteelPart()}\{profile}.lis");
        }
        /// <summary>
        /// 取得目前模型螺栓列表 (壓縮)
        /// </summary>
        /// <returns>回傳目前模型 <see cref="ApplicationVM.DirectorySteelBolts"/> 所有序列化檔案物件</returns>
        public Dictionary<string, ObservableCollection<SteelBolts>> GeBolts()
        {
            Dictionary<string, ObservableCollection<SteelBolts>> result = new Dictionary<string, ObservableCollection<SteelBolts>>();
            string dirPath = ApplicationVM.DirectorySteelBolts(); //螺栓資料夾路徑
            DirectoryInfo info = new DirectoryInfo(dirPath);//螺栓資料夾
            foreach (var item in info.GetFiles("*.lis")) //逐步尋找有關 .lis 檔案
            {
                ObservableCollection<SteelBolts> _ = GZipDeserialize<ObservableCollection<SteelBolts>>(item.FullName);//解壓縮反序列化只定路徑物件
                result.Add(item.Name, _);
            }
            return result;
        }
        /// <summary>
        /// 存取目前模型零件列表 (壓縮)
        /// </summary>
        /// <param name="profile">斷面規格</param>
        /// <param name="bolts">螺栓清單</param>
        public void SetBolts(string profile, ObservableCollection<object> bolts)
        {
            GZipSerializeBinary(bolts, $@"{ApplicationVM.DirectorySteelBolts()}\{profile}.lis");
        }
        /// <summary>
        /// 取得目前模型訊息斷面規格列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<string> GetProfile()
        {
            string path = ApplicationVM.FileProfileList(); //使用過的斷面規格文件
            return GZipDeserialize<ObservableCollection<string>>(path);//解壓縮反序列化回傳資料 
        }
        /// <summary>
        /// 取得指定模型訊息斷面規格列表
        /// </summary>
        /// <param name="modelPath">模型路徑</param>
        /// <returns></returns>
        public ObservableCollection<string> GetProfile(string modelPath)
        {
            string path = $@"{modelPath}\{ModelPath.ProfileList}"; //使用過的斷面規格文件
            return GZipDeserialize<ObservableCollection<string>>(path);//解壓縮反序列化回傳資料 
        }
        /// <summary>
        /// 存取目前模型使用過的斷面規格 (壓縮)
        /// </summary>
        /// <param name="profileList"></param>
        public void SetProfileList(ObservableCollection<string> profileList)
        {
            GZipSerializeBinary(profileList, $@"{ApplicationVM.FileProfileList()}");
        }
    }
}
