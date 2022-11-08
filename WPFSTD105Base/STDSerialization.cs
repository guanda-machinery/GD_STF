using devDept.Eyeshot.Translators;
using GD_STD;
using GD_STD.Base;
using GD_STD.Data;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.FluentAPI;
using WPFSTD105.Model;
using WPFSTD105.Surrogate;
using WPFSTD105.ViewModel;
using static GD_STD.SerializationHelper;
using SectionData;
using SplitLineSettingData;

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
        ///// <summary>
        ///// 取得刀庫設定檔
        ///// </summary>
        ///// <returns></returns>
        //public GD_STD.DrillWarehouse GetDrillWarehouse()
        //{
        //    string path = ApplicationVM.FileDrillWarehouse(); //刀庫資料路徑
        //    return GZipDeserialize<GD_STD.DrillWarehouse>(path);//解壓縮反序列化回傳資料
        //}
        ///// <summary>
        ///// 取得刀庫設定檔
        ///// </summary>
        ///// <returns></returns>
        //public void SetDrillWarehouse(GD_STD.DrillWarehouse value)
        //{
        //    string path = ApplicationVM.FileDrillWarehouse(); //刀庫資料路徑
        //    GZipSerializeBinary(value, path);//壓縮序列化檔案
        //}
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
        /// 取得所有斷面規格
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ObservableCollection<SteelAttr>> GetSteelAttr()
        {            
            Dictionary<string, ObservableCollection<SteelAttr>> result = new Dictionary<string, ObservableCollection<SteelAttr>>();
            string dirPath = ApplicationVM.DirectoryPorfile(); //斷面規格
            DirectoryInfo info = new DirectoryInfo(dirPath);//零件資料夾
            foreach (var item in info.GetFiles("*.inp")) //逐步尋找有關 .lis 檔案
            {
                ObservableCollection<SteelAttr> _ = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>(item.FullName);
                //ObservableCollection<object> _ = Deserialize<ObservableCollection<object>>(item.FullName);//解壓縮反序列化只定路徑物件
                ObservableCollection<SteelAttr> add = new ObservableCollection<SteelAttr>(_.Select(el => (SteelAttr)el));
                result.Add(item.Name.Replace(".inp",""), add);
            }
            return result;
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
                ObservableCollection<object> _ = GZipDeserialize<ObservableCollection<object>>(item.FullName);//解壓縮反序列化只定路徑物件
                ObservableCollection<SteelPart> add = new ObservableCollection<SteelPart>(_.Select(el => (SteelPart)el));
                result.Add(item.Name, add);
            }
            return result;
        }
        /// <summary>
        /// 取得目前模型指定零件列表 (壓縮)
        /// </summary>
        /// <returns>回傳目前模型 <see cref="ApplicationVM.DirectorySteelPart"/> 指定檔案物件</returns>
        public ObservableCollection<SteelPart> GetPart(string dataName)
        {
            try
            {
                string path = $@"{ApplicationVM.DirectorySteelPart()}\{dataName}.lis"; //零件路徑
                return new ObservableCollection<SteelPart>(GZipDeserialize<ObservableCollection<object>>(path).Select(el => (SteelPart)el));//解壓縮反序列化回傳資料
            }
            catch (Exception)
            {
                return null;
            }
           
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
        /// 存取工作陣列列表 (壓縮)
        /// </summary>
        /// <param name="values">加工列表</param>
        public void SetWorkMaterialBackup(WorkMaterial values)
        {
            string dataName = values.MaterialNumber
                                                            .Where(el => el != 0)
                                                            .Select(el => Convert.ToChar(el).ToString())
                                                            .Aggregate((str1, str2) => str1+ str2);

            GZipSerializeBinary(values, $@"{ApplicationVM.DirectoryWorkMaterialBackup()}\{dataName}.db");
        }
        /// <summary>
        /// 讀取工作陣列列表 (壓縮)
        /// </summary>
        public WorkMaterial? GetWorkMaterialBackup(string materialNumber)
        {
            string dataPath = $@"{ApplicationVM.DirectoryWorkMaterialBackup()}\{materialNumber}.db";
            if (File.Exists(dataPath))
            {
                return GZipDeserialize<WorkMaterial>(dataPath);//解壓縮反序列化回傳資料 
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 存取工作索引列表 (壓縮)
        /// </summary>
        /// <param name="values">陣列索引列表</param>
        public void SetWorkMaterialIndexBackup(short[] values)
        {
            GZipSerializeBinary(values, $@"{ApplicationVM.WorkMaterialIndexBackup()}");
        }
        /// <summary>
        /// 讀取工作索引列表 (壓縮)
        /// </summary>
        public short[] GetWorkMaterialIndexBackup()
        {
            return GZipDeserialize<short[]>($@"{ApplicationVM.WorkMaterialIndexBackup()}");//解壓縮反序列化回傳資料 
        }
        /// <summary>
        /// 存取工作其他資訊 (壓縮)
        /// </summary>
        /// <param name="values">其他參數</param>
        public void SetWorkMaterialOtherBackup(WorkOther values)
        {
            GZipSerializeBinary(values, $@"{ApplicationVM.WorkMaterialOtherBackup()}");
        }
        /// <summary>
        /// 讀取工作其他資訊 (壓縮)
        /// </summary>
        public WorkOther GetWorkMaterialOtherBackup()
        {
            return GZipDeserialize<WorkOther>($@"{ApplicationVM.WorkMaterialOtherBackup()}");//解壓縮反序列化回傳資料 
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
            {//return new ObservableCollection<SteelPart>(GZipDeserialize<ObservableCollection<object>>(path).Select(el => (SteelPart)el));//解壓縮反序列化回傳資料
                ObservableCollection<Object> _ = GZipDeserialize<ObservableCollection<Object>>(item.FullName);//解壓縮反序列化只定路徑物件

                result.Add(item.Name, new ObservableCollection<SteelBolts>(GZipDeserialize<ObservableCollection<object>>(item.FullName).Select(el => (SteelBolts)el)));

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
            // 2022.06.23 呂宗霖 避免所選路徑無專案
            if (project== null)
            {
                return null;
            }
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
        public DrillBrands GetDrillBrands()
        {
            return GZipDeserialize<DrillBrands>(ModelPath.DrillBrand);//解壓縮反序列化
        }
        /// <summary>
        /// 存取鑽頭品牌
        /// </summary>
        /// <returns></returns>
        public void SetDrillBrands(DrillBrands value)
        {
            GZipSerializeBinary(value, ModelPath.DrillBrand);//壓縮序列畫檔案
        }
        /// <summary>
        /// 存取 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataName">是 <see cref="AbsAttr.GUID"/></param>
        /// <param name="model"></param>
        public void SetModel(string path, string dataName, devDept.Eyeshot.Model model)
        {
            //if (File.Exists($@"{path}\{dataName}.dm"))
            //{
                //File.Delete($@"{path}\{dataName}.dm");//刪除檔案
            //}
            WriteFile writeFile = new WriteFile(new WriteFileParams(model)  //產生序列化檔案
            {
                Content = devDept.Serialization.contentType.GeometryAndTessellation,
                SerializationMode = devDept.Serialization.serializationType.WithLengthPrefix,
                SelectedOnly = false,
                Purge = true, Blocks = model.Blocks, Entities = model.Entities,
                }, $@"{path}\{dataName}.dm", new FileSerializerExt());    
            writeFile.DoWork();//存取檔案
        }
        /// <summary>
        /// 存取單零件<see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName">是 <see cref="AbsAttr.GUID"/></param>
        /// <param name="model"></param>
        public void SetPartModel(string dataName, devDept.Eyeshot.Model model) => SetModel(ApplicationVM.DirectoryDevPart(), dataName, model);
        /// <summary>
        /// 存取素材<see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="model"></param>
        public void SetMaterialModel(string dataName, devDept.Eyeshot.Model model) => SetModel(ApplicationVM.DirectoryMaterial(), dataName, model);

        /// <summary>
        /// 讀取 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName">是 <see cref="AbsAttr.GUID"/></param>
        /// <returns></returns>
        public ReadFile ReadPartModel(string dataName) => ReadModel(ApplicationVM.DirectoryDevPart(), dataName);
        /// <summary>
        /// 讀取素材<see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="dataName">是 <see cref="MaterialDataView.MaterialNumber"/></param>
        /// <returns></returns>
        public ReadFile ReadMaterialModel(string dataName) => ReadModel(ApplicationVM.DirectoryMaterial(), dataName);
        /// <summary>
        /// 讀取 <see cref="devDept.Eyeshot.Model"/>
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="dataName">檔案名稱</param>
        /// <remarks>
        /// 使用 AddToScene 加載場景
        /// </remarks>
        public ReadFile ReadModel(string path, string dataName)
        {
            try
            {
                ReadFile result = new ReadFile($@"{path}\{dataName}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation));
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        /// <summary>
        ///  加工區域 - 儲存序列化檔案資料 20220811 張燕華
        /// </summary>
        /// <param name="sectionTypeProcessingData"></param>
        public void SetSectionTypeProcessingData(ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData)
        {
            string SectionType_ProcessingBehavior = "";
            switch (sectionTypeProcessingData[0].ProcessingBehavior)
            {
                case (int)ProcessingBehavior.DRILLING:
                    SectionType_ProcessingBehavior = sectionTypeProcessingData[0].SectionCategoryType + "_DRILLING";
                    break;
                case (int)ProcessingBehavior.POINT:
                    SectionType_ProcessingBehavior = sectionTypeProcessingData[0].SectionCategoryType + "_POINT";
                    break;
            }
            GZipSerializeBinary(sectionTypeProcessingData, ApplicationVM.FileProcessingZone(SectionType_ProcessingBehavior));
        }
        /// <summary>
        ///  加工區域 - 讀出序列化檔案資料 20220811 張燕華
        /// </summary>
        public ObservableCollection<SectionTypeProcessingData> GetSectionTypeProcessingData(string SectionType, int intProcessingBehavior)
        {
            string SectionType_ProcessingBehavior = "";
            switch (intProcessingBehavior)
            {
                case (int)ProcessingBehavior.DRILLING:
                    SectionType_ProcessingBehavior = SectionType + "_DRILLING";
                    break;
                case (int)ProcessingBehavior.POINT:
                    SectionType_ProcessingBehavior = SectionType + "_POINT";
                    break;
            }
            
            return GZipDeserialize<ObservableCollection<SectionTypeProcessingData>>(ApplicationVM.FileProcessingZone(SectionType_ProcessingBehavior));
        }
        /// <summary>
        /// 型鋼加工區域設定 - 檢查設定值檔案是否存在 20220818 張燕華
        /// </summary>
        public bool[] CheckSectionTypeProcessingDataFile()
        {
            return ApplicationVM.CheckFileSectionTypeProcessingData();
        }
        /// <summary>
        /// 檢查參數設定資料夾是否存在, 若否則新增 20220819 張燕華 
        /// </summary>
        public bool CheckParameterSettingDirectory()
        {
            ApplicationVM appvm = new ApplicationVM();
            return appvm.CheckParameterSettingDirectoryPath();
        }
        /// <summary>
        ///  切割線設定 - 儲存序列化檔案資料 20220816 張燕華
        /// </summary>
        /// <param name="splitLineData"></param>
        public void SetSplitLineData(ObservableCollection<SplitLineSettingClass> splitLineData)
        {
            GZipSerializeBinary(splitLineData, ApplicationVM.FileSplitLine());
        }
        /// <summary>
        ///  切割線設定 - 讀出序列化檔案資料 20220816 張燕華
        /// </summary>
        public ObservableCollection<SplitLineSettingClass> GetSplitLineData()
        {
            return GZipDeserialize<ObservableCollection<SplitLineSettingClass>>(ApplicationVM.FileSplitLine());
        }
        /// <summary>
        ///  切割線設定 - 檢查設定值檔案是否存在 20220818 張燕華
        /// </summary>
        public bool CheckSplitLineDataFile()
        {
            return ApplicationVM.CheckFileSplitLine();
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
            ObservableCollection<DataCorrespond> temp_corr = GZipDeserialize<ObservableCollection<DataCorrespond>>(ApplicationVM.FilePartList());
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
        /// <summary>
        /// 存取機械參數設定檔案
        /// </summary>
        /// <param name="value"></param>
        public void SetMecSetting(MecSetting value)
        {
            //TODO:等待修正,暫時性
            string current = System.Environment.CurrentDirectory;
            GZipSerializeBinary(value, $@"{current}\{ModelPath.MecSetting}");

            string[] backup = GetBackupMecSetting();
            List<byte[]> stream = new List<byte[]>();
            //TODO: 等待修正
            //string current = System.Environment.CurrentDirectory;
            //string[] backup = GetBackupMecSetting();
            //List<byte[]> stream = new List<byte[]>();

            //if (backup.Length == 20) //如果有5個備份檔，先把第5個備份檔案刪除
            //{
            //    File.Delete(backup[19]);//刪除檔案
            //}
            ////5個備份檔，但只備份 4 個檔案。只備份 db1 ~ db4 並改變數字尾碼 +1，db5將刪除
            //for (int i = 0; i < backup.Length -1; i++)
            //{
            //    using (FileStream file = new FileStream(backup[i], FileMode.Open))
            //    {
            //        long length = file.Length;
            //        stream.Add(new byte[length]);
            //        file.Read(stream[i], 0, Convert.ToInt32(length));
            //    }
            //    File.Delete(backup[i]);//刪除檔案
            //}
            //for (int i = 0; i < stream.Count; i++)
            //{
            //    File.WriteAllBytes($@"{current}\{ModelPath.BackupMecSetting}\{ModelPath.MecSetting}{i+2}", stream[i]);
            //}
            //File.Copy($@"{current}\{ModelPath.MecSetting}", $@"{current}\{ModelPath.BackupMecSetting}\{ModelPath.MecSetting}1");//複製到備份區
            //GZipSerializeBinary(value, $@"{current}\{ModelPath.MecSetting}");
        }
        /// <summary>
        /// 取得機械參數設定檔案
        /// </summary>
        /// <param name="path"></param>
        public MecSetting GetMecSetting(string path)
        {
            string current = System.Environment.CurrentDirectory;
            return GZipDeserialize<MecSetting>($@"{current}\{path}");
        }
        /// <summary>
        /// 取得機械參數設定檔案
        /// </summary>
        public MecSetting GetMecSetting() => GetMecSetting(ModelPath.MecSetting);

        /// <summary>
        /// 取得機械參數設定檔的備份檔案完整路徑
        /// </summary>
        /// <returns></returns>
        public string[] GetBackupMecSetting()
        {
            string current = System.Environment.CurrentDirectory;
            DirectoryInfo directoryInfo = new DirectoryInfo(current);
            var result = directoryInfo.GetFiles($@"{ModelPath.MecSetting}.db*").Select(el => el.Name);
            return result.ToArray();
        }
        /// <summary>
        /// 存取選配設定檔案
        /// </summary>
        /// <param name="value"></param>
        public void SetOptionSettings(OptionSettings value)
        {
            //TODO:等待修正,暫時性
            string current = System.Environment.CurrentDirectory;
            GZipSerializeBinary(value, $@"{current}\{ModelPath.OptionSettings}");
        }

        /// <summary>
        /// 取得選配設定檔案
        /// </summary>
        /// <param name="path"></param>
        public OptionSettings GetOptionSettings(string path)
        {
            string current = System.Environment.CurrentDirectory;
            return GZipDeserialize<OptionSettings>($@"{current}\{path}");
        }
        /// <summary>
        /// 取得選配設定檔案
        /// </summary>
        public OptionSettings GetOptionSettings() => GetOptionSettings(ModelPath.OptionSettings);

        /// <summary>
        /// 取得選配設定檔的備份檔案完整路徑
        /// </summary>
        /// <returns></returns>
        public string[] GetBackupOptionSettings()
        {
            string current = System.Environment.CurrentDirectory;
            DirectoryInfo directoryInfo = new DirectoryInfo(current);
            var result = directoryInfo.GetFiles($@"{ModelPath.OptionSettings}.db*").Select(el => el.Name);
            return result.ToArray();
        }
    }
}
