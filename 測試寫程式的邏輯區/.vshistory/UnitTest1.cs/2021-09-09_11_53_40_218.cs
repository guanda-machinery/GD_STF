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
        public void TreeFile()
        {
            CopyFolder(@"C:\Users\User\Desktop\cca");
        }
        /// <summary>
        /// 複製文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="childDir">子資料夾</param>
        public void CopyFolder(string dir, string childDir = "")
        {
            foreach (string d in Directory.GetFiles(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string c = Path.GetExtension(d);//副檔名
                    if (c == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        File.Copy(d, $@"C:\Users\User\Desktop\Abc\{childDir}\{dataName}");//複製文件
                    }

                }
                else
                {

                    childDir += $@"\{Path.GetFileName(d)}";
                    Directory.CreateDirectory($@"C:\Users\User\Desktop\Abc\{childDir}");
                    CopyFolder(d, childDir);//递归删除子文件夹   
                }
            }
        }
        [Test]
        public void Test1()
        {
            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                zip.AddDirectory($@"C:\Users\User\Desktop\544");
                zip.AddFile(@"C:\Users\User\Desktop\新文字文件 (6).txt", "");
                zip.Save($@"C:\Users\User\Desktop\rwdwdff.zip");
            }
        }
    }
}