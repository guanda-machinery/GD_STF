using NUnit.Framework;
using GD_STD.Phone;
using static GD_STD.Phone.MemoryHelper;
using System;

namespace 測試保護鎖
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            OpenSharedMemory();
        }
        /// <summary>
        /// 單完測試結束
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            Dispose();
        }
        [Test]
        public void 測試上鎖APP結構()
        {
            APP_Struct aPP_Struct = APP_Struct.Initialization();
            SharedMemory.SetValue(aPP_Struct);
            aPP_Struct = SharedMemory.GetValue<APP_Struct>();
        }
        [Test]
        public void sssssssss()
        {
            byte[] c =  BitConverter.GetBytes(1d);
        }
    }
}