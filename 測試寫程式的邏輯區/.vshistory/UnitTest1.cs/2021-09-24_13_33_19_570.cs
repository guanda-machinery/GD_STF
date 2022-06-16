using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Management;
using System.IO;
using Ionic.Zip;
using System.Text;
using System.Collections.Generic;

namespace 測試寫程式的邏輯區
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        private static IEnumerable<int[]> 測試連續數字資料()
        {
            yield return new[] { 2, 5, 4, 7 };
            yield return new[] { 0, 6, 7, 8, 10 };
        }
        [Test, TestCaseSource("測試連續數字資料")]
        public void 連續數字(int[] vs)
        {
            int count = 10;
            List<int> result = new List<int>();
            for (int i = 0; i < vs.Length; i += 2)
            {
                result.Add(vs[i] + 1);
                result.Add(vs.Length < i + 1 ? vs[i + 1] - 1 : vs.Length - 1);
            }
        }
        [Test]
        public void TreeCopyFile()
        {
            CopyFolder(@"C:\Users\User\Desktop\cca");
        }
        [Test]
        public void TreeDeleteFile()
        {
            DeleteFolder(@"C:\Users\User\Desktop\cca");
        }
        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件   
                }
                else
                    DeleteFolder(d);//遞迴删除子文件夹   
            }
            if (@"C:\Users\User\Desktop\cca" != dir)
            {
                Directory.Delete(dir);//删除已空文件夹   
            }
        }
        /// <summary>
        /// 複製文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="childDir">子資料夾</param>
        public void CopyFolder(string dir, string childDir = "")
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
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
                    string create = $@"\{Path.GetFileName(d)}";
                    Directory.CreateDirectory($@"C:\Users\User\Desktop\Abc\{create}");
                    CopyFolder(d, create);//递归删除子文件夹   
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