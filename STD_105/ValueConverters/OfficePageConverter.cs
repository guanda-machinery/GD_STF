using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.ViewLocator;

namespace STD_105.Office
{
    /// <summary>
    /// Office頁面切換轉換器
    /// </summary>
    public class OfficePageConverter : WPFBase.BaseValueConverter<OfficePageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WPFBase.BasePage basePage = null;

            switch ((OfficePage)value)
            {
                // 空白遮蔽頁面
                case OfficePage.Home:
                    basePage = new FirstPage();
                    break; 
                //20220824 張燕華 舊排版
                // 製品設定
                case OfficePage.old_ObSettings:
                    basePage = new ObSettingsPage_Office();//舊版頁面
                    break;
                //// 2022/08/24 張彥華 開啟舊排版
                //case OfficePage.old_ObSettings:
                //    basePage = new ObSettingsPage_Office();//舊版頁面
                //    break;
                //20220816 蘇冠綸 新排版
                // 製品設定
                case OfficePage.ObSettings:
                    //basePage = new ObSettingsPage_Office();//舊版頁面
                    basePage = new ProductSettingsPage_Office();//新版頁面
                    break;
                // 參數設定
                case OfficePage.ParameterSettings:
                    basePage = new ParametersSettings_Office();
                    break;
                // 20220711 張燕華 參數設定 - 選單功能
                case OfficePage.ParameterSettings_FuncList:
                    basePage = new ParametersSettings_Office_FuncList();
                    break;
                //排版設定
                //20220816 蘇冠綸 排版設定
                case OfficePage.AutoTypeSettings:
                    //   basePage = new PartsList_Office();//舊版頁面
                    basePage = new TypesettingsSetting();//新版頁面
                    break;
                //舊排版設定
                //20220816 蘇冠綸 排版設定
                case OfficePage.old_PartsList:
                    basePage = new PartsList_Office();//舊版頁面

                    break;

                // 加工監控
                case OfficePage.ProcessingMonitor:
                    basePage = new ProcessingMonitor_Office();
                    break;
                // 廠區監控
                case OfficePage.WorkingAreaMonitor:
                    basePage = new WorkingAreaMonitor_Office();
                    break;
                default:
                    break;
            }
            OfficeBaseWindow.ActivateLoading();
            OfficeViewModel.ProjectManagerControl = true;
            basePage.CloseLoadingWindow += BasePage_LoadingEvent;
            return basePage;
        }

        private static void BasePage_LoadingEvent(object sender, EventArgs e)
        {
            OfficeBaseWindow.DeactivateLoading();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 20220711 張燕華 參數設定 - 功能頁面切換轉換器
    /// </summary>
    public class ParaSettingFuncPageConverter : WPFBase.BaseValueConverter<ParaSettingFuncPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WPFBase.BasePage basePage = null;

            switch ((ParameterSettingPage)value)
            {
                // 空白遮蔽頁面
                case ParameterSettingPage.Home:
                    basePage = new FirstPage();
                    break;
                // 原斷面規格頁面
                case ParameterSettingPage.SectionSpecificationSetting:
                    basePage = new ParametersSettings_Office();
                    break;
                // 斷面規格目錄
                case ParameterSettingPage.SectionSpecificationMenu:
                    basePage = new SectionSpecificationMenu();
                    break;
                // 語言設定
                case ParameterSettingPage.LanguageSetting:
                    //20220711 張燕華 從切換畫面改為顯示DecExpress的Dialog Service
                    //basePage = new ParametersSettings_Office_Language();
                    break;
                // 斷面規格Excel轉Inp
                case ParameterSettingPage.SectionSpecificationExcel2Inp:
                    //20220711 張燕華 從切換畫面改為顯示DecExpress的Dialog Service
                    basePage = new SectionSpecExcel2Inp();
                    break;
                case ParameterSettingPage.SpiltLineSetting:
                    //20220801 蘇冠綸 顯示切割線畫面
                    basePage = new SpiltLineSetting();
                    break;
                case ParameterSettingPage.ShapedSteelMachiningAreaSetting:
                    //20220801 蘇冠綸 型鋼加工區域設定
                    basePage = new ShapedSteelMachiningAreaSetting();
                    break;
                //20220802  蘇冠綸 刀具管理設定
                case ParameterSettingPage.ToolManagerSetting:
                    basePage = new ToolManagerSetting();
                    break;
                //20220802 蘇冠綸 軟體版本
                case ParameterSettingPage.SoftwareVersion:
                    basePage = new SoftwareVersion();
                    break;
                default:
                    break;
            }
            OfficeBaseWindow.ActivateLoading();
            OfficeViewModel.ProjectManagerControl = true;
            basePage.CloseLoadingWindow += BasePage_LoadingEvent;
            return basePage;
        }

        private static void BasePage_LoadingEvent(object sender, EventArgs e)
        {
            OfficeBaseWindow.DeactivateLoading();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
