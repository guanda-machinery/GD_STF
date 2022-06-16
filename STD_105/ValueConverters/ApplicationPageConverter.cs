using System;
using System.Globalization;
using WPFBase = WPFWindowsBase;
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
                //case ApplicationPage.IoTLogin:
                //    basePage = new IoTLoginPage();
                //return basePage;
                //case ApplicationPage.StandAlone:
                //    basePage = new SingleLoginPage();
                //   return basePage;
                case ApplicationPage.Home:
                    using (WPFSTD105.Memor.WriteMemorClient write = new WPFSTD105.Memor.WriteMemorClient())
                    using (WPFSTD105.Memor.ReadMemorClient read = new WPFSTD105.Memor.ReadMemorClient())
                    {
                        GD_STD.PanelButton cPanel = read.GetPanel();//當前面板狀態
                        GD_STD.PanelButton panel = new GD_STD.PanelButton()
                        {
                            Key = cPanel.Key,
                            Oil = cPanel.Oil,
                            Origin = cPanel.Origin,
                            Alarm = cPanel.Alarm,
                            EMS = cPanel.EMS,
                            Run = cPanel.Run,
                            Stop = cPanel.Stop,
                        };
                        write.SetPanel(panel);
                    }
                    basePage = new Dashboard();
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
                case ApplicationPage.ExClampDown:
                    basePage = new ExClampDownPage();
                    break;
                case ApplicationPage.RackOperation:
                    return new RacksOperation();
                //case ApplicationPage.RackOperation:
                //    basePage = new RacksOperation();
                //    break;
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
                    basePage = new VolumePage();//暫用
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
            MainWindow_STD.ActivateLoading();
            basePage.CloseLoadingWindow += BasePage_LoadingEvent;
            if ((ApplicationPage)value != ApplicationPage.Home)
            {
                ApplicationViewModel.IsHome = false;
            }
            return basePage;
        }

        private static void BasePage_LoadingEvent(object sender, EventArgs e)
        {
            MainWindow_STD.DeactivateLoading();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轉換器 <see cref="ApplicationPage"/>頁面轉換成字串
    /// </summary>
    public class ApplicationPageNameConverter : WPFBase.BaseValueConverter<ApplicationPageNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] ZH = { "儀錶板", "參數設定", "製品設定", "左軸模式", "中軸模式", "右軸模式", "刀庫模式", "夾具下壓", "周邊料架", "手臂模式", "夾具側壓", "軟體設定", "捲屑機模式", "加工監控", "排版設定", "警報" };
            string[] EN = { "Dashboard", "Parameter Settings", "Product Settings", "Left Spindle", "Top Spindle", "Right Spindle", "Tool Magazine", "Fixture Down", "Racks", "Arm", "Fixture Push", "Software Settings", "Conveyor", "Processing Monitor", "Typesetting", "Alert" };
            string[] VN = { "bảng điều khiển", "Cài đặt tham số", "Cài đặt sản phẩm", "Trái Trục Chính", "Trục Chính", "Đúng Trục Chính", "Tạp chí công cụ", "Lịch thi đấu xuống", "Giá đỡ", "Cánh tay", "Đẩy lịch thi đấu", "Cài đặt phần mềm", "Băng tải", "Giám sát xử lý", "Sắp chữ", "báo động" };
            string[] TH = { "แผงควบคุม", "การตั้งค่าพารามิเตอร์", "การตั้งค่าผลิตภัณฑ์", "แกนหมุนซ้าย", "แกนหมุนด้านบน", "แกนหมุนขวา", "นิตยสารเครื่องมือ", "ติดตั้งลง", "ชั้นวาง", "แขน", "การแข่งขันผลักดัน", "การตั้งค่าซอฟต์แวร์", "สายพานลำเลียง", "การตรวจสอบการประมวลผล", "การเรียงพิมพ์", "เตือน" };
            string[] basePage = { };

            switch (WPFSTD105.Properties.SofSetting.Default.Language)
            {
                case 0:
                    basePage = (string[])ZH.Clone();
                    break;
                case 1:
                    basePage = (string[])EN.Clone();
                    break;
                case 2:
                    basePage = (string[])VN.Clone();
                    break;
                case 3:
                    basePage = (string[])TH.Clone();
                    break;
                default:
                    break;
            }
            //找到合適的頁面
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Lock:
                case ApplicationPage.ModeSelected:
                    return null;
                case ApplicationPage.Home:
                    return basePage[0];
                case ApplicationPage.SettingPar:
                    return basePage[1];
                case ApplicationPage.ObSetting:
                    return basePage[2];
                case ApplicationPage.LeftAxis:
                    return basePage[3];
                case ApplicationPage.MiddleAxis:
                    return basePage[4];
                case ApplicationPage.RightAxis:
                    return basePage[5];
                case ApplicationPage.Alarm:
                    return basePage[15];
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
                    return basePage[6];
                case ApplicationPage.EnClampDown:
                case ApplicationPage.ExClampDown:
                    return basePage[7];
                case ApplicationPage.RackOperation:
                    return basePage[8];
                //case ApplicationPage.RackOperation:
                //    basePage = new RacksOperation();
                //    break;
                case ApplicationPage.Hand:
                    return basePage[9];
                case ApplicationPage.SideClamp:
                    return basePage[10];
                case ApplicationPage.SofSettings:
                    return basePage[11];
                case ApplicationPage.Volume:
                    return basePage[12];
                case ApplicationPage.Monitor:
                    return basePage[13];
                case ApplicationPage.TypeSetting:
                    return basePage[14];
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
