﻿using System;
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
                // 製品設定
                case OfficePage.ObSettings:
                    basePage = new ObSettingsPage_Office();
                    break;
                // 參數設定
                case OfficePage.ParameterSettings:
                    basePage = new ParametersSettings_Office();
                    break;
                //排版設定
                case OfficePage.AutoTypeSettings:
                    basePage = new PartsList_Office();
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
}
