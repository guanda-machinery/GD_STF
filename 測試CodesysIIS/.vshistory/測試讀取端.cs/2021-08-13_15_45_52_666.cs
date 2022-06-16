using GD_STD.Phone;
using NUnit.Framework;
using System;
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
            Client.SetAPPMonitorWork(monitorWork);
            string result = Client.GetProjectName();
            Assert.AreEqual(testResult, result);
        }
    }
}