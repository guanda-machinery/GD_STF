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
            String strQuery = "select * from meta_class";
            //2.引用ManagementObjectSearcher
            ManagementObjectSearcher Query = new ManagementObjectSearcher(strQuery);
            //3.擷取各種WMI集合
            ManagementObjectCollection EnumWMI = Query.Get();
            //4.列舉WMI類別
            foreach (ManagementObject searcher in EnumWMI)
            {
                Debug.WriteLine(searcher.ToString());
            }
        }
    }
}