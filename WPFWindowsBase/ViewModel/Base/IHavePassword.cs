using System.Security;

namespace WPFWindowsBase
{
    /// <summary>
    /// 可以提供安全密碼的類的接口
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// 安全密碼
        /// </summary>
        SecureString SecurePassword { get; }
    }
}
