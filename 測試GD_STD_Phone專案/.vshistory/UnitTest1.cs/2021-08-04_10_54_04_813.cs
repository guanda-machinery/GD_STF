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
        public PhoneConditionAttribute.CONDITION Condition;
        public string writeFieldName;
        public object writefieldValue;
        public string conditionFieldName;
        public object conditionFieldValue;
        /// <summary>
        /// 測試參數
        /// </summary>
        /// <param name="writeFieldName"></param>
        /// <param name="writefieldValue"></param>
        /// <param name="conditionFieldName"></param>
        /// <param name="conditionFieldValue"></param>
        public TestObjectInfo(string writeFieldName, object writefieldValue, string conditionFieldName, object conditionFieldValue)
        {
            this.writeFieldName = writeFieldName;
            this.writefieldValue = writefieldValue;
            this.conditionFieldName = conditionFieldName;
            this.conditionFieldValue = conditionFieldValue;
        }
    }
    [TestFixture]
    public class 測試APP_Struct寫入記憶體結構
    {
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

            ////移動料架條件
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
        [SetUp]
        public void Setup()
        {

        }

        [Test, TestCaseSource("TestFieldDataSources")]
        public void 單一測試欄位(TestObjectInfo testObjectInfo)
        {
            OpenSharedMemory();
            Operating operating = new Operating();
            operating.OpenApp = true;
            operating.Satus = PHONE_SATUS.MANUAL;
            PCSharedMemory.SetValue<Operating>(operating);

            object writeMemortyValue = APP_Struct.Initialization();
            object readMemoryValue = APP_Struct.Initialization();

            typeof(APP_Struct).GetField(testObjectInfo.writeFieldName).SetValue(writeMemortyValue, testObjectInfo.writefieldValue);
            typeof(APP_Struct).GetField(testObjectInfo.conditionFieldName).SetValue(readMemoryValue, testObjectInfo.conditionFieldValue);

            
            PhoneConditionFactory<APP_Struct> factory = new PhoneConditionFactory<APP_Struct>((APP_Struct)writeMemortyValue, (APP_Struct)readMemoryValue);
            try
            {
                Assert.Throws<MemoryException>(() => factory.Field(testObjectInfo.writeFieldName));
            }
            catch (Exception e)
            {
                Console.Write($"欄位 {nameof(testObjectInfo.writeFieldName)} 測試失敗");
            }
            Dispose();
            //APP_Struct


        }
    }
}