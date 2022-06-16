using GD_STD.Phone;
using NUnit.Framework;
using System.Collections.Generic;
using static GD_STD.Phone.MemoryHelper;
using GD_STD;
using GD_STD.Enum;
using System;

namespace 測試GD_STD_Phone專案
{
    [TestFixture]
    public class 測試APP_Struct寫入記憶體結構
    {
        //private static IEnumerable<APP_Struct> DataSources()
        //{
        //    List<APP_Struct> list = new List<APP_Struct>();

        //}
        [SetUp]
        public void Setup()
        {
            OpenSharedMemory();
            Operating operating = new Operating();
            operating.OpenApp = true;
            operating.Satus = PHONE_SATUS.MANUAL;

        }

        [TestCase("Yowko", 5)]
        public void 單一測試欄位(string writeFieldName, object fieldValue, string conditionFieldName , object conditionFieldValue)
        {
            object conditionread
            Assert.Pass();
        }
    }
}