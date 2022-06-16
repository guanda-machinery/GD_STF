using GD_STD;
using GD_STD.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;
namespace 測試自動配料
{
    [TestFixture]
    public class 分析
    {
        [SetUp]
        public void Setup()
        {
            IoC.Setup();
            ApplicationViewModel.ProjectName = "測試";
        }
        [Test]
        public void 轉換()
        {
            GD_STD.Enum.MATERIAL _;
            object value = System.Enum.Parse(typeof(GD_STD.Enum.MATERIAL), "AAWDAWDFVV");//反射出屬性 TYPE ，並轉換要賦予的值
        }
        [Test]
        public void 報表()
        {
            TeklaHtemlFactory teklaHtemlFactory = new TeklaHtemlFactory(@"C:\TeklaStructuresModels\昇鋼金屬有限公司-接36M\Reports\GUANDA_Personal_Format.csv");
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
            //TODO: 這裡是一開始新建專案後建置資料夾的地方
            Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
            Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelAssembly());//存放構件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
            SerializationHelper.SerializeBinary(new ObservableCollection<DataCorrespond>(), ApplicationVM.PartList());
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.ProfileList()); //斷面規格列表
            SerializationHelper.GZipSerializeBinary(new ObservableCollection<BomProperty>(), ApplicationVM.BomProperty()); //斷面規格列表
        }
    }
}