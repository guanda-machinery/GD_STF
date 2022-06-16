using System.Windows.Input;

namespace WPFSTD105
{
    public interface IChildWin
    {
        ICommand ChildClose { get; set; }
    }
}