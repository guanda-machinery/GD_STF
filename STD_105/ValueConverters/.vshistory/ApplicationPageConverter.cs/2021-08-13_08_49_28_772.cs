using System;
using System.Globalization;
using WPFBase =  WPFWindowsBase;
using WPFSTD105;
using static WPFSTD105.ViewLocator;
namespace STD_105
{
    /// <summary>
    /// 轉換器 <see cref="ApplicationPage"/>到實際的視圖/頁面
    /// </summary>
    public class ApplicationPageConverter : WPFBase.BaseValueConverter<ApplicationPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WPFBase.BasePage basePage = null;
            //找到合適的頁面
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Lock:
                    basePage = new LockPage();
                    return basePage;
                case ApplicationPage.ModeSelected:
                    basePage = new MachineModePage();
                    return basePage;
                case ApplicationPage.IoTLogin:
                    basePage = new IoTLoginPage();
                    return basePage;
                case ApplicationPage.StandAlone:
                    basePage = new SingleLoginPage();
                    return basePage;
                case ApplicationPage.Home:
                    basePage = new HomePage();
                    break;
                case ApplicationPage.SettingPar:
                    basePage = new SettingParPage();
                    break;
                case ApplicationPage.ObSetting:
                    basePage = new ObSettingPage();
                    break;
                case ApplicationPage.LeftAxis:
                    basePage = new LeftAxisPage();
                    break;
                case ApplicationPage.MiddleAxis:
                    basePage = new MiddleAxisPage();
                    break;
                case ApplicationPage.RightAxis:
                    basePage = new RightAxisPage();
                    break;
                case ApplicationPage.Alarm:
                    basePage = new AlarmPage();
                    break;
                //case ApplicationPage.DrillEntrance_L:
                //    return new DrillEntranceLPage();
                //case ApplicationPage.DrillEntrance_R:
                //    return new DrillEntranceRPage();
                //case ApplicationPage.DrillExport_L:
                //    return new DrillExportLPage();
                //case ApplicationPage.DrillExport_R:
                //    return new DrillExportRPage();
                //case ApplicationPage.DrillMiddle:
                case ApplicationPage.DrillEntrance_L:
                case ApplicationPage.DrillEntrance_R:
                case ApplicationPage.DrillExport_L:
                case ApplicationPage.DrillExport_R:
                case ApplicationPage.DrillMiddle:
                    basePage = new ChangeDrillPage();
                    break;
                case ApplicationPage.EnClampDown:
                    basePage = new EnClampDownPage();
                    break;
                case ApplicationPage.ExClampDown:
                    basePage = new ExClampDownPage();
                    break;
                //case ApplicationPage.EntranceRack:
                //return new EntranceRackPage();
                case ApplicationPage.RackOperation:
                    basePage = new RacksOperation();
                    break;
                case ApplicationPage.Hand:
                    basePage = new HandPage();
                    break;
                case ApplicationPage.SideClamp:
                    basePage = new SideClampPage();
                    break;
                case ApplicationPage.SofSettings:
                    basePage = new SofSettingsPage();
                    break;
                case ApplicationPage.Volume:
                    basePage = new VolumePage();
                    break;
                case ApplicationPage.Monitor:
                    basePage = new MachiningMonitorPage();
                    break;
                case ApplicationPage.TypeSetting:
                    basePage = new TypeSettingPage();
                    break;
                default:
                    return null;
            }
            MainWindow.ActivateLoading();
            basePage.CloseLoadingWindow += BasePage_LoadingEvent;
            return basePage;
        }

        private static void BasePage_LoadingEvent(object sender, EventArgs e)
        {
            MainWindow.DeactivateLoading();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
