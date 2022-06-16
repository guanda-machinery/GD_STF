using System.Windows.Input;

namespace WPFSTD105
{
    public interface IWindowsVM
    {
        ICommand HomeCommand { get; set; }
        ICommand ImportFileCommand { get; set; }
        ICommand ModifyProjectCommand { get; set; }
        ICommand ModifyProjectInfoCommand { get; set; }
        ICommand OpenProjectCommand { get; set; }
        ICommand OutProjectNameCommand { get; set; }
        ICommand SaveAsProjectCommand { get; set; }
        bool SaveAsProjectControl { get; set; }
        ICommand SettingParCommand { get; set; }
        ICommand SoftwareSettingsCommand { get; set; }
        ICommand TypeSettingCommand { get; set; }
        ICommand WatchProjectCommand { get; set; }
    }
}