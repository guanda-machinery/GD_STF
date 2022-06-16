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
using System.Text.RegularExpressions;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;
namespace 測試自動配料
{
    [TestFixture]
    public class Tekla報表轉物件測試
    {
        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            //foreach (string d in Directory.GetFileSystemEntries(dir))
            //{
            //    if (System.IO.File.Exists(d))
            //    {
            //        FileInfo fi = new FileInfo(d);
            //        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
            //            fi.Attributes = FileAttributes.Normal;
            //        System.IO.File.Delete(d);//直接删除其中的文件   
            //    }
            //    else
            //        DeleteFolder(d);//递归删除子文件夹   
            //}
            //Directory.Delete(dir);//删除已空文件夹   
        }
        private static IEnumerable<string> GetBom()
        {
            string[] info = Directory.GetDirectories(WPFSTD105.Properties.SofSetting.Default.LoadPath);
            for (int i = 0; i < info.Length; i++)
            {
                DeleteFolder(info[i]);
            }
            IoC.Setup();
            yield return @"C:\Users\User\Desktop\專案BOM表區\昇鋼金屬有限公司\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\卜蜂\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\南亞科技\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\英發\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\高雄港\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\觀音運動中心\GUANDA_Personal_Format.csv";
            
            yield return @"C:\Users\User\Desktop\專案BOM表區\南亞RH200X100\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\卜蜂RH400X200\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\高雄港RH194X150\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\高雄港RH390X300\GUANDA_Personal_Format.csv";
            yield return @"C:\Users\User\Desktop\專案BOM表區\高雄港RH400X200\GUANDA_Personal_Format.csv";
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
        public void 測試材質高寬重複性()
        {
            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();

            ObservableCollection<SteelAttr> A = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BH.inp");//複製 BH 斷面規格到模型內
            ObservableCollection<SteelAttr> B = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\RH.inp");//複製 RH 斷面規格到模型內
            ObservableCollection<SteelAttr> C = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\L.inp");//複製 L 斷面規格到模型內
            ObservableCollection<SteelAttr> D = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\CH.inp");//複製 CH 斷面規格到模型內
            ObservableCollection<SteelAttr> E = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BOX.inp");//複製 BOX 斷面規格到模型內

            NewMethod(ref keyValuePairs, A);
            NewMethod(ref keyValuePairs, B);
            NewMethod(ref keyValuePairs, C);
            NewMethod(ref keyValuePairs, D);
            NewMethod(ref keyValuePairs, E);
            foreach (var item in keyValuePairs)
            {
                if (item.Value.Count > 1)
                {
                    Assert.True(false);
                }
            }
            Assert.True(true);
        }

        private static void NewMethod(ref Dictionary<string, List<string>> keyValuePairs, ObservableCollection<SteelAttr> A)
        {
            for (int i = 0; i < A.Count; i++)
            {
                MatchCollection matches = Regex.Matches(A[i].Profile, @"[0-9.]+");
                string key = string.Empty;
                for (int c = 0; c < matches.Count; c++)
                {
                    key += matches[c].Value + ',';
                }
                if (!keyValuePairs.ContainsKey(key))
                {
                    keyValuePairs.Add(key, new List<string> { A[i].Profile });
                }
                else
                {
                    keyValuePairs[key].Add(A[i].Profile);
                }
            }
        }

        [Test]
        public void 材質()
        {
            string _ = "SN400YB,SN400B,SN490B,A572-GR.50,A572,SS400,SM400A,A36,SN400A,SN400A,SM400B,A992,SN490BD,SN490BD,SN490YB,SM490B,SM490YB,SN490A,A709,G3101,G3106,G3136";

            ObservableCollection<SteelMaterial> SAVR = new ObservableCollection<SteelMaterial>();
            foreach (var item in _.Split(','))
            {
                SAVR.Add(new SteelMaterial() { Name = item, PlateDensity = 7850.00d, ProfileDensity = 7850.00d });
            }

            SerializationHelper.GZipSerializeBinary(SAVR, @"C:\Users\User\Desktop\Mater.lis");
        }
        [Test, TestCaseSource("GetBom")]
        public void 測試報表轉換(string path)
        {
            string dirTestPath = Path.GetDirectoryName(path); //取得資料夾
            string dirName = Path.GetFileName(dirTestPath);//取得資料夾名稱
            ApplicationViewModel.ProjectName = dirName;

            Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
            Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryPorfile());//存放螺栓資料序列化的資料夾
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileSteelAssembly()); //斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileProfileList());//斷面規格列表

            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BH.inp", $@"{ApplicationVM.DirectoryPorfile()}\BH.inp");//複製 BH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\RH.inp", $@"{ApplicationVM.DirectoryPorfile()}\RH.inp");//複製 RH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\L.inp", $@"{ApplicationVM.DirectoryPorfile()}\L.inp");//複製 L 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\CH.inp", $@"{ApplicationVM.DirectoryPorfile()}\CH.inp");//複製 CH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BOX.inp", $@"{ApplicationVM.DirectoryPorfile()}\BOX.inp");//複製 BOX 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Mater.lis", $@"{ApplicationVM.ModelFileMaterial()}");//複製材質到模型內


            STDSerialization serialization = new STDSerialization();//序列化處理器
            TeklaHtemlFactory teklaHtemlFactory = new TeklaHtemlFactory($@"{path}"); //報表讀取器
            teklaHtemlFactory.Load();//載入物件
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
            if (teklaHtemlFactory.LackMaterial())//如果報表導入到模型沒有找到符合的材質就序列化物件
            {
                SerializationHelper.GZipSerializeBinary(teklaHtemlFactory.Material, ApplicationVM.ModelFileMaterial());
            }
            if (teklaHtemlFactory.LackProfile())//如果報表導入到模型沒有找到符合的斷面規格就序列化物件
            {
                foreach (var item in teklaHtemlFactory.Profile)
                {
                    SerializationHelper.SerializeBinary(item.Value, $@"{ApplicationVM.DirectoryPorfile()}\{item.Key.ToString()}.inp");//此項不是壓縮物件
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
        }
    }
}