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
using System.Runtime.InteropServices;

namespace 測試GD_STD_Phone專案
{
    [TestFixture]
    public class 計算記憶體偏移量
    {
        /// <summary>
        /// 測試寫入欄位參數是否符合條件帶入的值
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> TestOffsetDataSources()
        {
            //yield return $"{nameof(MonitorWork.WorkMaterial)}[1]";
            yield return $"{nameof(MonitorWork.WorkMaterial)}[1].{nameof(WorkMaterial.Finish)}";
        }
        /// <summary>
        /// 測試內容必須是不符合條件的狀況下，並請拋出例外狀況
        /// </summary>
        /// <param name="testObjectInfo"></param>
        [Test, TestCaseSource("TestOffsetDataSources")]
        public void 測試欄位參數是否符合條件(string fieldName)
        {
            MonitorWork monitorWork = MonitorWork.Initialization();
            long i = SharedMemory.GetMemoryOffset<MonitorWork>(monitorWork, fieldName);
            long size = Marshal.SizeOf(typeof(WorkMaterial));
            Assert.AreEqual(i, size);
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

    }
}
