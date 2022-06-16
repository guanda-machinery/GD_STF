using devDept.Eyeshot.Translators;
using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.Surrogate;
using WPFSTD105.ViewModel;
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
            return new ObservableCollection<SteelPart>(GZipDeserialize<ObservableCollection<object>>(path).Select(el => (SteelPart)el));//解壓縮反序列化回傳資料
        }
        /// <summary>
        /// 存取目前模型零件列表 (壓縮)
        /// </summary>
        /// <param name="profile">斷面規格 Hash Code</param>
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
        /// <param name="profile">斷面規格 Hash Code</param>
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
            GZipSerializeBinary(profileList, $@"{ApplicationVM.FileProfileList()}");//壓縮序列化檔案
        }
        /// <summary>
        /// 存取專案設定屬性內容
        /// </summary>
        /// <param name="project"></param>
        public void SetProjectProperty(ProjectProperty project)
        {
            GZipSerializeBinary(new ProjectPropertySurrogate(project), $@"{ApplicationVM.FileProjectProperty()}");//壓縮序列化檔案
        }
        /// <summary>
        /// 取得目前專案設定屬性內容
        /// </summary>
        public ProjectProperty GetProjectProperty()
        {
            ProjectPropertySurrogate project = GZipDeserialize<ProjectPropertySurrogate>($@"{ApplicationVM.FileProjectProperty()}");//解壓縮反序列化
            ProjectProperty result = new ProjectProperty(project);
            return GetProjectProperty($@"{ApplicationVM.FileProjectProperty()}");
        }

        /// <summary>
        /// 取得指定專案設定屬性內容
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public ProjectProperty GetProjectProperty(string path)
        {
            ProjectPropertySurrogate project = GZipDeserialize<ProjectPropertySurrogate>($@"{path}");//解壓縮反序列化
            ProjectProperty result = new ProjectProperty(project);
            return result;
        }
        /// <summary>
        /// 存取材質列表
        /// </summary>
        /// <param name="materials"></param>
        public void SetMaterial(ObservableCollection<SteelMaterial> materials)
        {
            SetMaterial(ApplicationVM.FileMaterial(), materials);//壓縮序列化檔案
        }
        /// <summary>
        /// 存取材質列表
        /// </summary>
        /// <param name="path">存取路徑</param>
        /// <param name="materials"></param>
        public void SetMaterial(string path, ObservableCollection<SteelMaterial> materials)
        {
            GZipSerializeBinary(materials, path);//壓縮序列化檔案
        }
        /// <summary>
        /// 取得材質列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SteelMaterial> GetMaterial()
        {
            return GetMaterial(ApplicationVM.FileMaterial());//解壓縮反序列化
        }
        /// <summary>
        /// 取得材質列表
        /// </summary>
        /// <param name="path">存取路徑</param>
        /// <returns></returns>
        public ObservableCollection<SteelMaterial> GetMaterial(string path)
        {
            return GZipDeserialize<ObservableCollection<SteelMaterial>>(path);//解壓縮反序列化
        }
        /// <summary>
        /// 取得鑽頭品牌
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DrillBrand> GetDrillBrands()
        {
            return GZipDeserialize<ObservableCollection<DrillBrand>>(ModelPath.DrillBrand);//解壓縮反序列化
        }
        /// <summary>
        /// 取得鑽頭品牌
        /// </summary>
        /// <returns></returns>
        public void SetDrillBrands(ObservableCollection<DrillBrand> value)
        {
            GZipSerializeBinary(value, ModelPath.DrillBrand);//壓縮序列畫檔案
        }
        /// <summary>
        /// 存取 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName">是 <see cref="AbsAttr.GUID"/></param>
        /// <param name="model"></param>
        public void SetPartModel(string dataName, devDept.Eyeshot.Model model)
        {
            WriteFile writeFile = new WriteFile(new WriteFileParams(model)  //產生序列化檔案
            {
                Content = devDept.Serialization.contentType.GeometryAndTessellation,
                SerializationMode = devDept.Serialization.serializationType.WithLengthPrefix,
                SelectedOnly = false,
                Purge = true
            }, $@"{ApplicationVM.DirectoryDevPart()}\{dataName}.dm", new FileSerializerExt());
            writeFile.DoWork();//存取檔案
        }
        /// <summary>
        /// 讀取 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName">是 <see cref="AbsAttr.GUID"/></param>
        /// <param name="model"></param>
        /// <remarks>
        /// 使用 AddToScene 加載場景
        /// </remarks>
        public ReadFile ReadModel(string dataName, devDept.Eyeshot.Model model)
        {
            ReadFile result = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{dataName}.dm", devDept.Serialization.contentType.GeometryAndTessellation);
            result.DoWork();
            return result;
        }
        /// <summary>
        ///  儲存序列化檔案資料
        /// </summary>
        /// <param name="dataCorresponds"></param>
        public void SetDataCorrespond(ObservableCollection<DataCorrespond> dataCorresponds)
        {
            GZipSerializeBinary(dataCorresponds, ApplicationVM.FilePartList());
        }
        /// <summary>
        ///  取得序列化檔案資料
        /// </summary>
        public ObservableCollection<DataCorrespond> GetDataCorrespond()
        {
            return GZipDeserialize<ObservableCollection<DataCorrespond>>(ApplicationVM.FilePartList());
        }
        /// <summary>
        /// 取得目前模型 nc 設定檔
        /// </summary>
        /// <returns></returns>
        public NcTempList GetNcTempList()
        {
            return GZipDeserialize<NcTempList>(ApplicationVM.FileNcTemp());
        }
        /// <summary>
        /// 存取目前模型 nc 設定檔
        /// </summary>
        /// <returns></returns>
        public void SetNcTempList(NcTempList value)
        {
            GZipSerializeBinary(value, ApplicationVM.FileNcTemp());
        }
        ///// <summary>
        ///// 取得排版設定
        ///// </summary>
        //public ObservableCollection<MaterialDataView> SetMaterialDataView()
        //{
        //    return GetMaterialDataView(ApplicationVM.FileMaterialDataView());
        //}
        /// <summary>
        /// 取得排版設定
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MaterialDataView> GetMaterialDataView() => GZipDeserialize<ObservableCollection<MaterialDataView>>(ApplicationVM.FileMaterialDataView());
        
        /// <summary>
        /// 取得排版設定
        /// </summary>
        /// <param name="ModelPath">模型路徑</param>
        public ObservableCollection<MaterialDataView> GetMaterialDataView(string ModelPath)
        {
            return GZipDeserialize<ObservableCollection<MaterialDataView>>(ModelPath);
        }
        /// <summary>
        /// 存取排版設定
        /// </summary>
        /// <param name="materialDataView"></param>
        public void SetMaterialDataView(ObservableCollection<MaterialDataView> materialDataView)
        {
            SetMaterialDataView(materialDataView, ApplicationVM.FileMaterialDataView());
        }
        /// <summary>
        /// 存取排版設定
        /// </summary>
        /// <param name="materialDataView"></param>
        /// <param name="ModelPath">模型路徑</param>
        public void SetMaterialDataView(ObservableCollection<MaterialDataView> materialDataView, string ModelPath)
        {
            GZipSerializeBinary(materialDataView, ModelPath);
        }
    }
}
