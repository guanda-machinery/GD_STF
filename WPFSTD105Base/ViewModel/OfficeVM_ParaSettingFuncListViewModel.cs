using System;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using WPFBase = WPFWindowsBase;
using System.Windows;
using System.Windows.Media;
using static WPFSTD105.ViewLocator;


namespace WPFSTD105.ViewModel
{
    public class OfficeVM_ParaSettingFuncListViewModel : WPFBase.BaseViewModel
    {
        public OfficeVM_ParaSettingFuncListViewModel()
        {
        }
        /// <summary>
        /// 20220711 張燕華 開啟斷面設定
        /// </summary>
        /*public ICommand ParameterSettingsCommand {
            get
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationSetting;
            });
        }*/
        /// <summary>
        /// 20220725 張燕華 斷面規格目錄
        /// </summary>
        public ICommand SectionSpecMenuCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationMenu;
                });
            }
        }

        /// <summary>
        /// 20220727 張燕華 斷面規格Excel轉Inp
        /// </summary>
        public ICommand SectionSpecExcel2InpCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SectionSpecificationExcel2Inp;
                });
            }
        }

        /// <summary>
        /// 20220801 蘇冠綸 顯示切割線畫面
        /// </summary>
        public ICommand SpiltLineSettingCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SpiltLineSetting;
                });
            }
        }
        /// <summary>
        /// 20220801 蘇冠綸 型鋼加工區域設定
        /// </summary>
        public ICommand ShapedSteelMachiningAreaSettingCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.ShapedSteelMachiningAreaSetting;
                });
            }
        }
        /// <summary>
        /// 20220801 蘇冠綸 刀具管理設定
        /// </summary>
        public ICommand ToolManagerSettingCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.ToolManagerSetting;
                });
            }
        }
        /// <summary>
        /// 20220801 蘇冠綸 軟體版本
        /// </summary>
        public ICommand SoftwareVersionCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    OfficeViewModel.ParaSettingCurrentPage = ParameterSettingPage.SoftwareVersion;
                });
            }
        }

    }
}
