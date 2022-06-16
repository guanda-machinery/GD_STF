using GD_STD;
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
    public class Tests
    {
        //private IEnumerable<List<object>> SteelAttrs()
        //{
        //    yield return new TeklaHtemlFactory("GUANDA_Personal_Format.xls").ToObject(typeof(SteelAttr), null, null);
        //}
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void 創建資料夾 ()
        {
            IoC.Setup();
            ApplicationViewModel.ProjectName = "測試";
            //TODO: 這裡是一開始新建專案後建置資料夾的地方
            Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
            Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelAssembly());//存放構件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
            Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
            SerializationHelper.SerializeBinary(new ObservableCollection<DataCorrespond>(), ApplicationVM.PartList());
            SerializationHelper.SerializeBinary(new ObservableCollection<string>(), ApplicationVM.PartList());
            Assert.Pass();
        }

    }
}