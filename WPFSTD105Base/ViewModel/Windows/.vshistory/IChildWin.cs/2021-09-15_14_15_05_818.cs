using System.Windows.Input;

namespace WPFSTD105
{
    /// <summary>
    /// 子視窗
    /// </summary>
    public interface IChildWin
    {
        /// <summary>
        /// 關閉自身窗體
        /// </summary>
        ICommand ChildClose { get; set; }
    }
}