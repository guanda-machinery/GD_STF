namespace WPFWindowsBase
{
    /// <summary>
    /// 資料加密介面
    /// </summary>
    public interface IDES
    {
        /// <summary>
        /// 授權碼
        /// </summary>
        string Authorize { get; set; }
        /// <summary>
        /// 來源
        /// </summary>
        string Source { get; set; }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="source">加密內容</param>
        /// <param name="key">金鑰</param>
        /// <returns></returns>
        string DecryptByDES(string source, string key);
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="source">加密內容</param>
        /// <param name="key">金鑰</param>
        /// <returns></returns>
        string EncryptByDES(string source, string key);
        /// <summary>
        /// DES加密
        /// </summary>
        /// <returns></returns>
        string DecryptByDES();
        /// <summary>
        /// DES解密
        /// </summary>
        /// <returns></returns>
        string EncryptByDES();
    }
}