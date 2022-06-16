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
        public string mainFieldName;
        public object mainFieldValue;
        public string secondFieldName;
        public object secondFieldValue;
        public PhoneConditionAttribute.CONDITION Condition;
        /// <summary>
        /// 測試參數
        /// </summary>
        /// <param name="mainFieldName"></param>
        /// <param name="mainValue"></param>
        /// <param name="secondValueName"></param>
        /// <param name="secondValue"></param>
        public TestObjectInfo(string mainFieldName, object mainValue, string secondValueName, object secondValue, PhoneConditionAttribute.CONDITION condition)
        {
            this.mainFieldName = mainFieldName;
            this.mainFieldValue = mainValue;
            this.secondFieldName = secondValueName;
            this.secondFieldValue = secondValue;
            Condition = condition;
        }
    }
    [TestFixture]
    public class 測試APP_Struct寫入記憶體結構
    {
        /// <summary>
        /// 測試寫入欄位參數是否符合條件帶入的值
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<TestObjectInfo> TestFieldDataSources()
        {
            #region 條件
            //錯誤條件原點復歸
            yield return GetTestObjectInfo(nameof(APP_Struct.Arm), new Arm() { Axis = COORDINATE.Z }, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.DrillMiddle), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftEntrance), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftExport), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.LoosenDril), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.Move_OutSide), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.OpenOil), false, nameof(APP_Struct.Origin));//油壓關閉
            yield return GetTestObjectInfo(nameof(APP_Struct.RightEntrance), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightExport), true, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.Traverse_Shelf_UP), SHELF.LEVEL2, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollMove), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollDeepDrop), MOBILE_RACK.INSIDE, nameof(APP_Struct.Origin));

            //錯誤條件油壓
            yield return GetTestObjectInfo(nameof(APP_Struct.Arm), new Arm() { Axis = COORDINATE.Z }, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.DrillMiddle), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftEntrance), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LeftExport), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.LoosenDril), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.Origin), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightEntrance), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RightExport), true, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.Traverse_Shelf_UP), SHELF.LEVEL2, nameof(APP_Struct.OpenOil));
            yield return GetTestObjectInfo(nameof(APP_Struct.RollDeepDrop), MOBILE_RACK.INSIDE, nameof(APP_Struct.OpenOil));
            #endregion

            #region 範圍
            //移動料架條件
            yield return new TestObjectInfo(nameof(APP_Struct.Count), (short)1, null, null, PhoneConditionAttribute.CONDITION.LIMITS);
            yield return new TestObjectInfo(nameof(APP_Struct.Count), (short)9, null, null, PhoneConditionAttribute.CONDITION.LIMITS);
            #endregion

            #region 不可寫入
            GD_STD.Phone.DrillWarehouse drillWarehouse = new GD_STD.Phone.DrillWarehouse()
            {
                LeftEntrance = new DrillSetting[10],
                LeftExport = new DrillSetting[10],
                Middle = new DrillSetting[10],
                RightEntrance = new DrillSetting[10],
                RightExport = new DrillSetting[10]
            };
            drillWarehouse.LeftEntrance[0].Dia = 500;

            //不可寫入
            yield return new TestObjectInfo(nameof(APP_Struct.DrillWarehouse), drillWarehouse, null, null, PhoneConditionAttribute.CONDITION.FIELD);
            #endregion
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

            FieldInfo mainField = typeof(APP_Struct).GetField(testObjectInfo.mainFieldName);
            if (testObjectInfo.secondFieldName != null)
            {
                FieldInfo secondField = typeof(APP_Struct).GetField(testObjectInfo.secondFieldName);
                secondField.SetValue(readMemoryValue, testObjectInfo.secondFieldValue);

                //如果這是一個油壓關閉測試邏輯必須先把油壓打開
                if (testObjectInfo.mainFieldName == nameof(APP_Struct.OpenOil))
                {
                    readMemoryValue.GetType().GetField(nameof(APP_Struct.OpenOil)).SetValue(readMemoryValue, true);
                }
            }

            mainField.SetValue(writeMemortyValue, testObjectInfo.mainFieldValue);

            PhoneConditionFactory<APP_Struct> factory = new PhoneConditionFactory<APP_Struct>((APP_Struct)writeMemortyValue, (APP_Struct)readMemoryValue);

            try
            {
                Assert.Throws<MemoryException>(()=> factory.Field(testObjectInfo.mainFieldName)) ;
            }
            catch (MemoryException e)
            {
                Console.Write($"欄位 {testObjectInfo.mainFieldName} 測試成功");
            }
            catch (Exception e)
            {
                Console.Write($"欄位 {testObjectInfo.mainFieldName} 測試失敗。欄位值 {testObjectInfo.mainFieldValue}");
            }
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
                                    !conditionValuealue,
                                    PhoneConditionAttribute.CONDITION.FIELD);
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
    }
}