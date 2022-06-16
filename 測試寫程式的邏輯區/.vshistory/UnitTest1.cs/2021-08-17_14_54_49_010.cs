using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Management;

namespace 測試寫程式的邏輯區
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            foreach (ManagementBaseObject core in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                Debug.WriteLine(core["CurrentVoltage"].ToString());
            }
        }
    }
}