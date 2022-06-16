using GD_STD;
using System;
namespace WPFSTD105
{
    /// <summary>
    /// 登入帳號資訊
    /// </summary>
    [Serializable()]
    public class AccountNumber : SerializationHelper<AccountNumber>
    {
        /// <summary>
        /// 公司帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 個人代號
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string PasswordText { get; set; }
    }
}
