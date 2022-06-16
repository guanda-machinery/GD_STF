using GD_STD.Phone;
using NUnit.Framework;
using System;
using System.Diagnostics;
using static 測試CodesysIIS.Endpoint;
namespace 測試CodesysIIS
{
    public class 測試讀取端
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void 測試GetProjectName()
        {
            MonitorWork monitorWork = MonitorWork.Initialization();
            string testResult = "全端:這麼簡單就用C寫就好了";
            monitorWork.ProjectName = testResult.GetUInt16(typeof(MonitorWork).ArrayLength(nameof(MonitorWork.ProjectName)));
            //計算程式秒數
#if DEBUG
            DateTime date = DateTime.Now;
            Debug.Print(string.Format("秒數計算").PadLeft(15, '-').PadRight(30, '-'));
#endif
            long offset = SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.ProjectName));
            Client.SetMonitorWorkOffset(testResult.ToByteArray(), offset);

#if DEBUG
            Debug.Print(string.Format("花費 {0} 秒", (DateTime.Now - date).Seconds.ToString()).PadLeft(15, '-').PadRight(30, '-'));
#endif
            string result = Client.GetProjectName();
            Assert.AreEqual(testResult, result);
        }
    }
}