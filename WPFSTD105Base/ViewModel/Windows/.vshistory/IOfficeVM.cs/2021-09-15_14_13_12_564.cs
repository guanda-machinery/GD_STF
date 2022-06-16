using System.Windows.Input;

namespace WPFSTD105
{
    public interface IOfficeVM
    {
        ICommand ChildClose { get; set; }
    }
}