using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Management;
using System.IO;
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
            ZipFile.CreateFromDirectory($@"C:\Users\User\Desktop\544", @"C:\Users\User\Desktop\rwdwdff.zip");//建立壓縮檔
            File.Copy(@"C:\Users\User\Desktop\新文字文件 (6).txt", @"C:\Users\User\Desktop\rwdwdff.zip");
        }
    }
}