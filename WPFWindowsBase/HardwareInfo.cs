using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    /// <summary>
    /// 硬體訊息
    /// </summary>
    public static class HardwareInfo
    {
        /// <summary>
        /// 取得主機板序號
        /// </summary>
        /// <returns></returns>
        public static string GetMotherBoardUID()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
                result = share["SerialNumber"].ToString();
            }
            return result;
        }
    }
}
