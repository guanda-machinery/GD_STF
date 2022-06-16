using System.Windows.Input;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 機器行為模式選擇
    /// </summary>
    public class ModeSelectedVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 物聯網模式
        /// </summary>
        public ICommand IoTCommand { get; set; }

        /// <summary>
        /// 單機模式
        /// </summary>
        public ICommand StandAloneCommand { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ModeSelectedVM.ModeSelectedVM()' 的 XML 註解
        public ModeSelectedVM()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ModeSelectedVM.ModeSelectedVM()' 的 XML 註解
        {
            IoTCommand = new WPFWindowsBase.RelayCommand(() =>
            {
                ApplicationViewModel.CurrentPage = ApplicationPage.IoTLogin;
            });
            StandAloneCommand = new WPFWindowsBase.RelayCommand(() =>
            {
                ApplicationViewModel.CurrentPage = ApplicationPage.StandAlone;
            });
        }
    }
}