using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Management;
using System.IO;
using Ionic.Zip;
using System.Text;

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
            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                zip.AddDirectory($@"C:\Users\User\Desktop\544");
                zip.AddFile(@"C:\Users\User\Desktop\新文字文件 (6).txt");
                zip.Save($@"C:\Users\User\Desktop\rwdwdff.zip");
            }
        }
    }
}