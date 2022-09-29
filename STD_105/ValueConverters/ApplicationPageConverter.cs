using System;
using System.Globalization;
using WPFBase = WPFWindowsBase;
using WPFSTD105;
using static WPFSTD105.ViewLocator;
using System.Collections.Generic;

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

                case ApplicationPage.MachineTypeSetting:
                    basePage = new TypeSettingsSettingPage_Machine();
                    break;
                case ApplicationPage.MachineProductSetting:
                    basePage = new ProductSettingsPage_Machine();
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
        private enum LangEnum
        {
            ZH, EN, VN, TH
        }

        private Dictionary<ApplicationPage, Dictionary<LangEnum, string>> Page_LanguageDict
        {
            get
            {
                var LangDict = new Dictionary<ApplicationPage, Dictionary<LangEnum, string>>();
                LangDict[ApplicationPage.Lock] = null;
                LangDict[ApplicationPage.ModeSelected] = null;
                LangDict[ApplicationPage.Home] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "儀錶板" },
                    { LangEnum.EN, "Dashboard" },
                    { LangEnum.VN, "bảng điều khiển" },
                    { LangEnum.TH, "แผงควบคุม" }
                };
                LangDict[ApplicationPage.SettingPar] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "參數設定" },{ LangEnum.EN, "Parameter Settings" },{ LangEnum.VN, "Cài đặt tham số" },{ LangEnum.TH, "การตั้งค่าพารามิเตอร์" }
                };
                LangDict[ApplicationPage.MachineProductSetting] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "製品設定" },{ LangEnum.EN, "Product Settings" },{ LangEnum.VN, "Cài đặt sản phẩm" },{ LangEnum.TH, "การตั้งค่าผลิตภัณฑ์" }
                };
                LangDict[ApplicationPage.ObSetting] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "製品設定" },{ LangEnum.EN, "Product Settings" },{ LangEnum.VN, "Cài đặt sản phẩm" },{ LangEnum.TH, "การตั้งค่าผลิตภัณฑ์" }
                };
                LangDict[ApplicationPage.LeftAxis] = new Dictionary<LangEnum, string>
                {
                { LangEnum.ZH, "左軸模式" },{ LangEnum.EN, "Left Spindle" },{ LangEnum.VN, "Trái Trục Chính" },{ LangEnum.TH, "แกนหมุนซ้าย" }
                };
                LangDict[ApplicationPage.MiddleAxis] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "中軸模式" },{ LangEnum.EN, "Top Spindle" },{ LangEnum.VN, "Trục Chính" },{ LangEnum.TH, "แกนหมุนด้านบน" }
                };
                LangDict[ApplicationPage.RightAxis] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "右軸模式" },{ LangEnum.EN, "Right Spindle" },{ LangEnum.VN, "Đúng Trục Chính" },{ LangEnum.TH, "แกนหมุนขวา" }
                };
                var ToolMagazineDict = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "刀庫模式" },{ LangEnum.EN, "Tool Magazine" },{ LangEnum.VN, "Tạp chí công cụ" },{ LangEnum.TH, "นิตยสารเครื่องมือ" }
                };
                LangDict[ApplicationPage.DrillEntrance_L] = ToolMagazineDict;
                LangDict[ApplicationPage.DrillEntrance_R] = ToolMagazineDict;
                LangDict[ApplicationPage.DrillExport_R] = ToolMagazineDict;
                LangDict[ApplicationPage.DrillMiddle] = ToolMagazineDict;
                LangDict[ApplicationPage.EnClampDown] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "夾具下壓" },{ LangEnum.EN, "Fixture Down" },{ LangEnum.VN, "Lịch thi đấu xuống" },{ LangEnum.TH, "ติดตั้งลง" }
                };
                LangDict[ApplicationPage.RackOperation] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "周邊料架" },{ LangEnum.EN, "Racks" },{ LangEnum.VN, "Giá đỡ" },{ LangEnum.TH, "ชั้นวาง" }
                };
                LangDict[ApplicationPage.Hand] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "手臂模式" },{ LangEnum.EN, "Arm" },{ LangEnum.VN, "Cánh tay" },{ LangEnum.TH, "แขน" }
                };
                LangDict[ApplicationPage.SideClamp] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "夾具側壓" },{ LangEnum.EN, "Fixture Push" },{ LangEnum.VN, "Đẩy lịch thi đấu" },{ LangEnum.TH, "การแข่งขันผลักดัน" }
                };
                LangDict[ApplicationPage.SofSettings] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "軟體設定" },{ LangEnum.EN, "Software Settings" },{ LangEnum.VN, "Cài đặt phần mềm" },{ LangEnum.TH, "การตั้งค่าซอฟต์แวร์" }
                };
                LangDict[ApplicationPage.Volume] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "捲屑機模式" },{ LangEnum.EN, "Conveyor" },{ LangEnum.VN, "Băng tải" },{ LangEnum.TH, "สายพานลำเลียง" }
                };
                LangDict[ApplicationPage.Monitor] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "加工監控" },{ LangEnum.EN, "Processing Monitor" },{ LangEnum.VN, "Giám sát xử lý" },{ LangEnum.TH, "การตรวจสอบการประมวลผล" }
                };
                LangDict[ApplicationPage.TypeSetting] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "排版設定" },{ LangEnum.EN, "Typesetting" },{ LangEnum.VN, "Sắp chữ" },{ LangEnum.TH, "การเรียงพิมพ์" }
                };
                LangDict[ApplicationPage.MachineTypeSetting] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "排版設定" },{ LangEnum.EN, "Typesetting" },{ LangEnum.VN, "Sắp chữ" },{ LangEnum.TH, "การเรียงพิมพ์" }
                };
                LangDict[ApplicationPage.Alarm] = new Dictionary<LangEnum, string>
                {
                    { LangEnum.ZH, "警報" },{ LangEnum.EN, "Alert" },{ LangEnum.VN, "báo động" },{ LangEnum.TH, "เตือน" }
                };
                return LangDict;
            }
        }


        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //找到合適的頁面
            if (value is ApplicationPage)
            {
                var pageL = Page_LanguageDict[(ApplicationPage)value];
                //找語言
                LangEnum _Lang = LangEnum.ZH;
                switch (WPFSTD105.Properties.SofSetting.Default.Language)
                {
                    case 0:
                        _Lang = LangEnum.ZH;
                        break;
                    case 1:
                        _Lang = LangEnum.EN;
                        break;
                    case 2:
                        _Lang = LangEnum.VN;
                        break;
                    case 3:
                        _Lang = LangEnum.TH;
                        break;
                    default:
                        break;
                }

                if (pageL != null)
                    return pageL[_Lang];
                else
                    return null;
            }
            return null;
        }


        #region 舊版寫法
        /*
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
                    case ApplicationPage.MachineTypeSetting:
                    return basePage[14];
                case ApplicationPage.MachineProductSetting:
                    return basePage[2];
                default:
                    return null;
            }
        }
        */
        #endregion

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
