using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// 系統管理員驗證
    /// </summary>
    public class SystemVerification
    {
        private IDES _IDES;
        /// <summary>
        /// 主機板序號
        /// </summary>
        public string MotherBoard { get => _IDES.Source; }
        /// <summary>
        /// 驗證碼
        /// </summary>
        public string Authorize { get => _IDES.Authorize; }
        /// <summary>
        /// 系統管理員
        /// </summary>
        /// <param name="dES">加密方式</param>
        public SystemVerification(IDES dES)
        {
            _IDES = dES;
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            //foreach (ManagementObject share in searcher.Get())
            //{
            //    Source = share["SerialNumber"].ToString();
            //}
            ////Authorize = new Random().Next(999, 9999).ToString().PadLeft(4, '0');
        }
        ///// <summary>
        ///// 產生對應金鑰
        ///// </summary>
        ///// <param name="motherBoard"></param>
        ///// <param name="authorize"></param>
        //public SystemVerification(string motherBoard, string authorize)
        //{
        //}
        /// <summary>
        /// 是系統管理員
        /// </summary>
        /// <returns>
        /// 如果是系統管理員回傳 true，不是則 false。
        /// </returns>
        public bool IsAdmin(string key)
        {
            string encryptDES = _IDES.EncryptByDES();
            return encryptDES == key;
        }
        /// <summary>
        /// 取得金鑰
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            string result = _IDES.EncryptByDES();
            return result;
        }

    }
}
