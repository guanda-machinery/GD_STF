using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    /// <summary>
    /// MD5 加密
    /// </summary>
    public class MD5DES : IDES
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="source"></param>
        public MD5DES(string source)
        {
            Authorize = new Random().Next(100, 9999).ToString().PadLeft(4, '0');
            Source=source;
        }

        /// <summary>
        /// 產生對應金鑰
        /// </summary>
        /// <param name="source">加密內容</param>
        /// <param name="authorize">金鑰</param>
        public MD5DES(string source, string authorize)
        {
            Source=source;
            Authorize=authorize;
        }

        /// <summary>
        /// 來源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 授權碼
        /// </summary>
        public string Authorize { get; set; }
        /// <summary>
        /// MD5 Hash
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public byte[] HashByMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="source">來源</param>
        /// <param name="key">金鑰</param>
        /// <returns></returns>
        public string EncryptByDES(string source, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。
            // 使用 MD5 來 Hash 出 Rfc2898 需要的 Salt。
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, this.HashByMD5(key));

            // 8 bits = 1 byte，將 KeySize 及 BlockSize 個別除以 8，取得 Rfc2898 產生衍生金鑰的長度。
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);

            var dateByteArray = Encoding.UTF8.GetBytes(source);

            // 加密
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dateByteArray, 0, dateByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DecryptByDES(string encrypted, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。
            // 使用 MD5 來 Hash 出 Rfc2898 需要的 Salt。
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, this.HashByMD5(key));

            // 8 bits = 1 byte，將 KeySize 及 BlockSize 個別除以 8，取得 Rfc2898 產生衍生金鑰的長度。
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);

            var dateByteArray = Convert.FromBase64String(encrypted);

            // 解密
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dateByteArray, 0, dateByteArray.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        /// <inheritdoc/>
        public string DecryptByDES()
        {
            if (Source == null)
            {
                throw new Exception($" {nameof(Source)} 不可以是 null");
            }
            if (Authorize == null)
            {
                throw new Exception($" {nameof(Authorize)} 不可以是 null");
            }
            return DecryptByDES(Source, Authorize);
        }
        /// <inheritdoc/>
        public string EncryptByDES()
        {
            if (Source == null)
            {
                throw new Exception($" {nameof(Source)} 不可以是 null");
            }
            if (Authorize == null)
            {
                throw new Exception($" {nameof(Authorize)} 不可以是 null");
            }
            return EncryptByDES(Source, Authorize);
        }

    }
}
