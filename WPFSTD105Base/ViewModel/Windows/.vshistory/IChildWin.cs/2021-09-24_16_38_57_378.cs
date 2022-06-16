using System.Windows.Input;

namespace WPFSTD105
{
    /// <summary>
    /// 子視窗
    /// </summary>
    public interface IChildWin
    {
        /// <summary>
        /// 關閉子窗體
        /// </summary>
        ICommand Close { get; set; }
    }
}