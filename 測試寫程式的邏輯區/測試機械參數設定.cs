using GD_STD;
using GD_STD.Phone;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.FluentAPI;
using WPFWindowsBase;
using WPFSTD105;
namespace 測試寫程式的邏輯區
{
    public class 測試機械參數設定
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void 測試機械參數序列化()
        {
            Directory.CreateDirectory($@"{System.Environment.CurrentDirectory}\MecSetting\");
            WPFSTD105.STDSerialization ser = new WPFSTD105.STDSerialization();
            MecSetting mecSetting = new MecSetting();
            SerializationHelper.GZipSerializeBinary(mecSetting, $@"{System.Environment.CurrentDirectory}\MecSetting.db");
            ser.SetMecSetting(mecSetting);
            Assert.True(true);
        }
    }
}
