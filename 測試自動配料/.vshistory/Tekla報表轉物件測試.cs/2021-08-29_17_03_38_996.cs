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
    public class Tekla報表轉物件測試
    {
        private static IEnumerable<string> GetBom()
        {
            string[] info = Directory.GetDirectories(WPFSTD105.Properties.SofSetting.Default.LoadPath);
            for (int i = 0; i < info.Length; i++)
            {
                Directory.Delete(info[i]);
            }
            IoC.Setup();
            yield return @"C:\Users\User\Desktop\專案BOM表區\昇鋼金屬有限公司\GUANDA_Personal_Format.csv";
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
        public void 材質()
        {
            string _ = "SN400YB,SN400B,SN490B,A572-GR.50,A572,SS400,SM400A,A36,SN400A,SN400A,SM400B,A992,SN490BD,SN490BD,SN490YB,SM490B,SM490YB,SN490A,A709,G3101,G3106,G3136";

            ObservableCollection<string> SAVR = new ObservableCollection<string>();
            foreach (var item in _.Split(','))
            {
                SAVR.Add(item);
            }

            SerializationHelper.GZipSerializeBinary(SAVR,@"C:\Users\User\Desktop\Mater.lis");
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
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<BomProperty>(), ApplicationVM.BomProperty()); //模型屬性設定
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BH.inp", $@"{ApplicationVM.DirectoryPorfile()}\BH.inp");//複製 BH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\RH.inp", $@"{ApplicationVM.DirectoryPorfile()}\RH.inp");//複製 RH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\L.inp", $@"{ApplicationVM.DirectoryPorfile()}\L.inp");//複製 L 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\CH.inp", $@"{ApplicationVM.DirectoryPorfile()}\CH.inp");//複製 CH 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Profile\BOX.inp", $@"{ApplicationVM.DirectoryPorfile()}\BOX.inp");//複製 BOX 斷面規格到模型內
            File.Copy($@"C:\Users\User\source\repos\GD_STF\STD_105\bin\Debug\Mater.lis", $@"{ApplicationVM.DirectoryModel()}\Mater.lis");//複製材質到模型內


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
        }
    }
}