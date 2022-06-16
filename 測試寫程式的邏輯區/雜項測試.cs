using GD_STD.Base;
using GD_STD.Phone;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WPFSTD105;
using WPFSTD105.Tekla;
using WPFWindowsBase;

namespace 測試寫程式的邏輯區
{
    [TestFixture]
    public class 雜項測試
    {
        [Test]
        public void 測試字元組合()
        {
            List<string> strings = new List<string> { "123" };
            string str = strings.Aggregate((str1, str2) => $"{str1},{str2}");
            Assert.True(true);
        }
        [Test]
        public void 取得主機版序號()
        {
            ManagementObjectSearcher my = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in my.Get())
            {
                string a = "主機板製造商：" + share["Manufacturer"].ToString();
                string b = "產品：" + share["Product"].ToString();
                string c = "主機板序號：" + share["SerialNumber"].ToString();
                Debug.WriteLine($"{a}, {b}, {c}");
            }
            Assert.True(true);
        }
        private static IEnumerable<List<double>> 測試圓心座標()
        {
            //P1.X, P1.Y, P2.X, P2.Y, 正確圓心X, 正確圓心Y 
            yield return new List<double> { 0, 49, 35, 14, 0, 14 };
            yield return new List<double> { 965, 14, 1000, 49, 1000, 14 };
            yield return new List<double> { 1000, 201, 965, 236, 1000, 236 };
            yield return new List<double> { 35, 236, 0, 201, 0, 236 };
        }
        [Test, TestCaseSource("測試圓心座標")]
        public void 測試圓心計算(List<double> points)
        {
            double x1 = points[0], y1 = points[1], x2 = points[2], y2 = points[3];
            AK.CircleCenter(x1, y1, x2, y2, 35, out double resultX1, out double resultY1);
            if (resultX1 == points[4]&& resultY1 == points[5])
            {
                Console.WriteLine("正確");
            }
            else
            {
                Assert.Fail("運算錯誤");
            }
            Assert.True(true, "正確");
            //Assert.AreEqual(0, resultX);
            //Assert.AreEqual(14, resultY);
        }
        [Test]
        public void LoginMemory()
        {
            GD_STD.Phone.MemoryHelper.OpenSharedMemory();
            Login login1 = new Login();
            string uid = "123".PadRight(20, ' ');
            string code = "456".PadRight(20, ' '); ;
            string pas = "5555".PadRight(20, ' '); ;
            string tokne = "456465".PadRight(20, ' ');
            login1.UID = uid.ToCharArray();
            login1.Code = code.ToCharArray();
            login1.Passwpord = pas.ToCharArray();
            login1.Token = tokne.ToCharArray();
            GD_STD.Phone.SharedMemory.SetValue(login1);
            Login login2 = new Login();
            login2 = GD_STD.Phone.SharedMemory.GetValue<Login>();
        }

        [Test]
        public void 字串()
        {
            MatchCollection matches = Regex.Matches("RH0000001", @"[0-9]+");
           string result = string.Empty;
            for (int i = 0; i < matches.Count; i++)
            {
                result = matches[i].Value;
            }
            Assert.AreEqual("0000001", result);
        }
        [Test]
        public void 驗證系統管理員登入()
        {
            MD5DES mD51 = new MD5DES(HardwareInfo.GetMotherBoardUID());
            MD5DES mD52 = new MD5DES(mD51.Source, mD51.Authorize);
            SystemVerification client = new SystemVerification(mD51);
            SystemVerification server = new SystemVerification(mD52);
            Assert.True(client.IsAdmin(server.GetKey()));
            //user.MotherBoard
            //user.IsAdmin();
        }
    }
}
