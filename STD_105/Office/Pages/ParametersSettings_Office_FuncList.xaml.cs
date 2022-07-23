using System;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.ViewLocator;
using System.Windows;
using System.Windows.Media;
using STD_105.Office;

namespace STD_105.Office
{
    public class OfficeVM_ParaSettingFuncList
    {
        public OfficeVM_ParaSettingFuncList()
        {
            ParameterSettingsCommand = ParameterSettings();
            //LanguageSettingsCommand = LanguageSettings();
        }

        /// <summary>
        /// 20220711 張燕華 開啟斷面設定
        /// </summary>
        public ICommand ParameterSettingsCommand { get; set; }
        public WPFBase.RelayCommand ParameterSettings()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationSetting;
            });
        }
    }

    /// <summary>
    /// 20220711 張燕華 ParametersSettings_Office_FuncList.xaml 的互動邏輯
    /// </summary>
    public partial class ParametersSettings_Office_FuncList : BasePage
    {
        
        public ParametersSettings_Office_FuncList()
        {
            InitializeComponent();
            
            DataContext = new OfficeVM_ParaSettingFuncList();
            int a=0;
        }
    }
}
