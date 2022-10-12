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
            SectionSpecMenuCommand = SectionSpecMenu();
            //LanguageSettingsCommand = LanguageSettings();
            SectionSpecExcel2InpCommand = SectionSpecExcel2Inp();

            SpiltLineSettingCommand = SpiltLineSetting();

            ShapedSteelMachiningAreaSettingCommand = ShapedSteelMachiningAreaSetting();
            ToolManagerSettingCommand = ToolManagerSetting();
            SoftwareVersionCommand = SoftwareVersion();
            ReportLogoCommand = ReportLogo();
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

        /// <summary>
        /// 20220725 張燕華 斷面規格目錄
        /// </summary>
        public ICommand SectionSpecMenuCommand { get; set; }
        public WPFBase.RelayCommand SectionSpecMenu()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationMenu;
            });
        }

        /// <summary>
        /// 20220727 張燕華 斷面規格Excel轉Inp
        /// </summary>
        public ICommand SectionSpecExcel2InpCommand { get; set; }
        public WPFBase.RelayCommand SectionSpecExcel2Inp()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationExcel2Inp;
            });
        }

        /// <summary>
        /// 20220801 蘇冠綸 顯示切割線畫面
        /// </summary>
        public ICommand SpiltLineSettingCommand { get; set; }

        public WPFBase.RelayCommand SpiltLineSetting()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SpiltLineSetting;
            });
        }

        /// <summary>
        /// 20220801 蘇冠綸 型鋼加工區域設定
        /// </summary>
        public ICommand ShapedSteelMachiningAreaSettingCommand { get; set; }

        public WPFBase.RelayCommand ShapedSteelMachiningAreaSetting()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.ShapedSteelMachiningAreaSetting;
            });
        }

        /// <summary>
        /// 20220801 蘇冠綸 刀具管理設定
        /// </summary>
        public ICommand ToolManagerSettingCommand { get; set; }
        public WPFBase.RelayCommand ToolManagerSetting()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.ToolManagerSetting;
            });
        }

        /// <summary>
        /// 20221006 張燕華 報表LOGO
        /// </summary>
        public ICommand ReportLogoCommand { get; set; }
        public WPFBase.RelayCommand ReportLogo()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.ReportLogo;
            });
        }

        /// <summary>
        /// 20220801 蘇冠綸 軟體版本
        /// </summary>
        public ICommand SoftwareVersionCommand { get; set; }
        public WPFBase.RelayCommand SoftwareVersion()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SoftwareVersion;
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
