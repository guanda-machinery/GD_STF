using GD_STD;
using GD_STD.Data;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;
namespace 測試自動配料
{
    [TestFixture]
    public class 分析
    {
        private static IEnumerable<string> GetBom()
        {
            IoC.Setup();
            yield return @"C:\Users\User\Desktop\專案BOM表區\卜蜂\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\南亞科技\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\英發\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\高雄港\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\觀音運動中心\GUANDA_Personal_Format.csv";
        }
        /// <summary>
        /// 每一個測試單元都會進入這方法
        /// </summary>
        [SetUp]
        public void Setup()
        {
           
        }
        /// <summary>
        /// 單完測試結束
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
        }
        [Test]
        public void 轉換()
        {
            GD_STD.Enum.MATERIAL _;
            object value = System.Enum.Parse(typeof(GD_STD.Enum.MATERIAL), "AAWDAWDFVV");//反射出屬性 TYPE ，並轉換要賦予的值
        }
        [Test]
        public void ProjectName()
        {
            //Path.GetDirectoryName(@"C:\Users\User\Desktop\專案BOM表區\卜蜂\GUANDA_Personal_Format.csv");
            Path.GetFileName(Path.GetDirectoryName(@"C:\Users\User\Desktop\專案BOM表區\卜蜂\GUANDA_Personal_Format.csv"));
        }
        [Test, TestCaseSource("GetBom")]
        public void 報表(string path)
        {
            //計算程式秒數
            DateTime date = DateTime.Now;
            Debug.Print(string.Format("秒數計算").PadLeft(15, '-').PadRight(30, '-'));

            string dirTestPath = Path.GetDirectoryName(path); //取得資料夾
            string dirName = Path.GetFileName(dirTestPath);//取得資料夾名稱
            ApplicationViewModel.ProjectName = dirName;

            Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
            Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileSteelAssembly()); //斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileProfileList());//斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<BomProperty>(), ApplicationVM.BomProperty()); //斷面規格列表

            STDSerialization serialization = new STDSerialization();//序列化處理器
            TeklaHtemlFactory teklaHtemlFactory = new TeklaHtemlFactory($@"{path}"); //報表讀取器
            serialization.SetSteelAssemblies(teklaHtemlFactory.SteelAssemblies);//序列化構件資訊
            serialization.SetProfileList(teklaHtemlFactory.ProfileList);
            int part = 0, bolt = 0; //計算資料數量
            foreach (var el in teklaHtemlFactory.KeyValuePairs)//逐步存取序列化物件
            {
                //判斷 type 序列化
                if (el.Value[0].GetType() == typeof(SteelPart))
                {
                    //string dataNmae = el.Key;
                    serialization.SetPart(el.Key.GetHashCode().ToString(), el.Value);
                    part++;
                }
                else if (el.Value[0].GetType() == typeof(SteelBolts))
                {
                    serialization.SetBolts(el.Key.GetHashCode().ToString(), el.Value);
                    bolt++;
                }
            }
            //判斷資料數量與輸出數量有沒有相符
            FileInfo[] partFile = new DirectoryInfo(ApplicationVM.DirectorySteelPart()).GetFiles();
            FileInfo[] boltFile = new DirectoryInfo(ApplicationVM.DirectorySteelBolts()).GetFiles();
            Assert.AreEqual(part, partFile.Length);
            Assert.AreEqual(bolt, boltFile.Length);

            ObservableCollection<string> profileList = serialization.GetProfile();
            string dirPath = ApplicationVM.DirectorySteelPart();
            //驗證斷面規格轉資料名稱
            for (int i = 0; i < profileList.Count; i++)
            {
                string s = profileList[i].GetHashCode().ToString();
                if (partFile.Where(el => Path.GetFileNameWithoutExtension(el.FullName) == s).Count() == 1)
                {
                    Assert.True(true);
                }
                else
                {
                    Assert.True(false);
                }
            }
            Debug.Print(string.Format("花費 {0} 秒", (DateTime.Now - date).Seconds.ToString()).PadLeft(15, '-').PadRight(30, '-'));
            //TODO: 這邊要分析NC檔案
        }
        [Test]
        public void 檢查雜湊()
        {
            SteelAssembly steel1 = new SteelAssembly
            {
                DrawingName = "123",
                W = 100,
                H = 200,
                Length = 300,
                UnitWeight = 500,
                UnitArea = 600,
            };
            SteelAssembly steel2 = new SteelAssembly
            {
                DrawingName = "123",
                W = 100,
                H = 300,
                Length = 300,
                UnitWeight = 500,
                UnitArea = 600,
            };
            SteelAssembly steel3 = new SteelAssembly
            {
                DrawingName = "123",
                W = 100,
                H = 200,
                Length = 300,
                UnitWeight = 500,
                UnitArea = 600,
            };
            int a = steel1.GetHashCode(), b = steel2.GetHashCode(), c = steel3.GetHashCode();
            bool result1 = steel1.Equals(steel2);
            bool result2 = steel1.Equals(steel3);

        }
        [Test]
        public void 創建資料夾()
        {
            Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
            Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileSteelAssembly()); //斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileProfileList());//斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<BomProperty>(), ApplicationVM.BomProperty()); //斷面規格列表
        }
    }
}