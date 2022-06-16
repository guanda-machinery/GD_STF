using System.Windows;
using System.Windows.Input;

namespace WPFSTD105
{
    /// <summary>
    /// 子視窗
    /// </summary>
    public interface IChildWin
    {
        /// <summary>
        /// 關閉子窗體，關閉後會將 <see cref="ChildWin"/> 釋放掉。
        /// </summary>
        ICommand Close { get; set; }
        /// <summary>
        /// 子視窗
        /// </summary>
        Window ChildWin { get; set; }
    }
}