using GD_STD.Phone;
using NUnit.Framework;
using System.Collections.Generic;
using static GD_STD.Phone.MemoryHelper;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Reflection;
using GD_STD.Base;
using System.Linq;

namespace 測試GD_STD_Phone專案
{
    public class TestObjectInfo
    {
        public string writeFieldName;
        public object writefieldValue;
        public string secondFieldName;
        public object secondFieldValue;
        /// <summary>
        /// 測試參數
        /// </summary>
        /// <param name="mainFieldName"></param>
        /// <param name="writefieldValue"></param>
        /// <param name="conditionFieldName"></param>
        /// <param name="conditionFieldValue"></param>
        public TestObjectInfo(string mainFieldName, object writefieldValue, string conditionFieldName, object conditionFieldValue)
        {
            this.writeFieldName = mainFieldName;
            this.writefieldValue = writefieldValue;
            this.secondFieldName = conditionFieldName;
            this.secondFieldValue = conditionFieldValue;
        }
    }
    [TestFixture]
    public class 測試APP_Struct寫入記憶體結構
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<TestObjectInfo> TestFieldDataSources()
        {
            //原點復歸條件
            yield return GetTestObjectInfo(nameof(APP_Struct.Arm), new Arm() { Axis = COORDINATE.Z }, nameof(APP_Struct.Origin));//手臂
            yield return GetTestObjectInfo(nameof(APP_Struct.DrillMiddle), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftEntrance), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftExport), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LoosenDril), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.Move_OutSide), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.OpenOil), false, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightEntrance), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightExport), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.Traverse_Shelf_UP), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollMove), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollDeepDrop), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));

            //油壓條件
            yield return GetTestObjectInfo(nameof(APP_Struct.Arm), new Arm() { Axis = COORDINATE.Z }, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.DrillMiddle), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftEntrance), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftExport), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LoosenDril), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.Origin), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightEntrance), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightExport), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.Traverse_Shelf_UP), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollDeepDrop), MOBILE_RACK.INSIDE, nameof(APP_Struct.OpenOil));

            //移動料架條件
            //yield return GetTestObjectInfo(nameof(APP_Struct.Count), 1, nameof(APP_Struct.SelectMove));

            GD_STD.Phone.DrillWarehouse drillWarehouse = new GD_STD.Phone.DrillWarehouse()
            {
                LeftEntrance = new DrillSetting[10],
                LeftExport = new DrillSetting[10],
                Middle = new DrillSetting[10],
                RightEntrance = new DrillSetting[10],
                RightExport = new DrillSetting[10]
            };
            //不可寫入
            //yield return GetTestObjectInfo(nameof(APP_Struct.DrillWarehouse), drillWarehouse, nameof(APP_Struct.OpenOil));//中軸刀庫
        }

        public IEnumerable<TestObjectInfo> TestScopeDataSources()
        {
            yield return GetTestObjectInfo(nameof(APP_Struct.Count), 1, nameof(APP_Struct.SelectMove));
        }

        [Test, TestCaseSource("TestFieldDataSources")]
        public void 寫入欄位否有符合最大最小值(TestObjectInfo testObjectInfo)
        {

        }


        private static TestObjectInfo GetTestObjectInfo(string writeFieldName, object writefieldValue, string conditionFieldName)
        {
            bool conditionValuealue = Convert.ToBoolean(typeof(APP_Struct).
                                                                                    GetField(writeFieldName).
                                                                                        GetCustomAttributes<PhoneConditionAttribute>().
                                                                                            First(el => el.FieldName == conditionFieldName).Value);
            return new TestObjectInfo(
                                    writeFieldName,
                                    writefieldValue,
                                    conditionFieldName,
                                    !conditionValuealue);
        }
        /// <summary>
        /// 每一個測試單元都會進入這方法
        /// </summary>
        [SetUp]
        public void Setup()
        {
            OpenSharedMemory();
            Operating operating = new Operating();
            operating.OpenApp = true;
            operating.Satus = PHONE_SATUS.MANUAL;
            PCSharedMemory.SetValue<Operating>(operating);
        }
        /// <summary>
        /// 單完測試結束
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            Dispose();
        }



        /// <summary>
        /// 測試內容必須是不符合條件的狀況下，並請拋出例外狀況
        /// </summary>
        /// <param name="testObjectInfo"></param>
        [Test, TestCaseSource("TestFieldDataSources")]
        public void 測試欄位參數是否符合條件(TestObjectInfo testObjectInfo)
        {
            object writeMemortyValue = APP_Struct.Initialization();
            object readMemoryValue = APP_Struct.Initialization();

            typeof(APP_Struct).GetField(testObjectInfo.writeFieldName).SetValue(writeMemortyValue, testObjectInfo.writefieldValue);
            typeof(APP_Struct).GetField(testObjectInfo.secondFieldName).SetValue(readMemoryValue, testObjectInfo.secondFieldValue);

            //如果這是一個油壓測試邏輯必須先把油壓打開
            if (testObjectInfo.writeFieldName == nameof(APP_Struct.OpenOil))
            {
                typeof(APP_Struct).GetField(nameof(APP_Struct.OpenOil)).SetValue(readMemoryValue, true);
            }

            PhoneConditionFactory<APP_Struct> factory = new PhoneConditionFactory<APP_Struct>((APP_Struct)writeMemortyValue, (APP_Struct)readMemoryValue);
            try
            {
                Assert.Throws<MemoryException>(() => factory.Field(testObjectInfo.writeFieldName));
            }
            catch (Exception e)
            {
                Console.Write($"欄位 {testObjectInfo.writeFieldName} 測試失敗");
            }
            Dispose();
            //APP_Struct
        }
    }
}