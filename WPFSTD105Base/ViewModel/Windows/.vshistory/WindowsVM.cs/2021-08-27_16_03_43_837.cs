using GD_STD;
using GD_STD.Enum;
using OpenGL.Delegates;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.PanelListening;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
using System.Threading.Tasks;
using DevExpress.Xpf.Core;

namespace WPFSTD105
{
    /// <summary>
    /// 機械型號 STD-105 平面窗口的視圖模型
    /// </summary>
    public class WindowsVM : WPFBase.WindowsViewModel
    {
        /// <summary>
        /// 判斷是否切換語言
        /// </summary>
        public bool LanguageFlag = false;
        #region 命令
        /// <summary>
        /// 返回首頁命令
        /// </summary>
        public ICommand HomeCommand { get; set; }
        /// <summary>
        /// 首頁功能
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand Home()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (ApplicationViewModel.CurrentPage >= ApplicationPage.Home)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
            });
        }

        /// <summary>
        /// 油壓開關命令
        /// </summary>
        public ICommand OillCommand { get; set; }
        /// <summary>
        /// 油壓功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Oill()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                _.Oil = !_.Oil;
                
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 參數設定命令
        /// </summary>
        public ICommand SettingParCommand { get; set; }
        private WPFBase.RelayCommand SettingPar()
        {
            return new WPFBase.RelayCommand(() =>
            {
                PanelButton panelButton = new PanelButton();

                panelButton = StateClear(ApplicationViewModel.PanelButton);
                WriteCodesysMemor.SetPanel(panelButton);
                ApplicationViewModel.CurrentPage = ApplicationPage.SettingPar;
            });
        }

        /// <summary>
        /// 警報命令
        /// </summary>
        public ICommand AlarmCommand { get; set; }
        /// <summary>
        /// 警報功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Alarm()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (ApplicationViewModel.PanelButton.Alarm > ERROR_CODE.Null) //發生警報
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Alarm;
                }
            });
        }

        /// <summary>
        /// 語言命令
        /// </summary>
        public ICommand LanguageCommand { get; set; }
        /// <summary>
        /// 語言功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Language()
        {
            return new WPFBase.RelayCommand(() =>
            {
                // Application.Current.Resources.MergedDictionaries[0] 0為App.xaml裡面TW.xaml的序列位置
                int language = Properties.SofSetting.Default.Language;
                string languageName = "/LanguagePackages/EN.xaml";
                CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
                ResourceDictionary langRd = null;
                switch (language)
                {
                    case 0:
                        languageName = "/LanguagePackages/EN.xaml";
                        break;
                    case 1:
                        languageName = "/LanguagePackages/TH.xaml";
                        break;
                    case 2:
                        languageName = "/LanguagePackages/VN.xaml";
                        break;
                }
                try
                {
                    langRd = Application.LoadComponent(
                    new Uri(@languageName, UriKind.Relative)) as ResourceDictionary;
                }
                catch
                {
                }

                if (!LanguageFlag)
                {
                    if (langRd != null)
                    {

                    }
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@languageName, UriKind.Relative)
                    };
                    LanguageFlag = true;
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@"/LanguagePackages/TW.xaml", UriKind.Relative)
                    };
                    LanguageFlag = false;
                }
            });
        }

        /// <summary>
        /// 原點復歸命令
        /// </summary>
        public ICommand OriginCommand { get; set; }
        /// <summary>
        /// 原點復歸功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Origin()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                _.Origin = true;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }
        /*
        /// <summary>
        /// 出口料架命令
        /// </summary>
        public ICommand ExportRackCommand { get; set; }
        /// <summary>
        /// 出口料架功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand ExportRack()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;

                if (!ApplicationViewModel.PanelButton.ExportRack)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.ExportRack = !_.ExportRack;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }
        */
        /*
        /// <summary>
        /// 入口料架命令
        /// </summary>
        public ICommand EntranceRackCommand { get; set; }
        /// <summary>
        /// 入口料架功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand EntranceRack()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.EntranceRack)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.EntranceRack = !_.EntranceRack;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }
        */
        /// <summary>
        /// 周邊料架命令
        /// </summary>
        public ICommand RackOperationCommand { get; set; }
        /// <summary>
        /// 周邊料架功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand RackOperation()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.RackOperation)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.RackOperation = !_.RackOperation;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 主軸模式命令 
        /// </summary>
        public ICommand MainAxisModeCommand { get; set; }
        /// <summary>
        /// 主軸模式功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand MainAxisMode()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                //AxisAction axisAction = new AxisAction();
                if (!ApplicationViewModel.PanelButton.MainAxisMode)
                {
                    _ = StateClear(ApplicationViewModel.PanelButton);
                    _.MainAxisMode = true;
                    _.AxisStop = true;
                }
                else
                {
                    _ = StateClear(ApplicationViewModel.PanelButton);
                }
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 主軸旋轉命令 
        /// </summary>
        public ICommand MainAxisRotationCommand { get; set; }
        /// <summary>
        /// 主軸旋轉功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand MainAxisRotation()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                if (!_.AxisRotation)
                {
                    _.AxisStop = false;
                    _.AxisLooseKnife = false;
                    _.AxisRotation = true;
                }
                else
                {
                    _.AxisStop = true;
                    _.AxisRotation = false;
                }
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }
        /// <summary>
        /// 主軸停止命令 
        /// </summary>
        public ICommand MainAxisStopCommand { get; set; }
        /// <summary>
        /// 主軸停止功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand MainAxisStop()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                if (!_.AxisStop)
                {
                    _.AxisRotation = false;
                }
                else
                    _ = ApplicationViewModel.PanelButton;

                _.AxisStop = true;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 中心出水命令 
        /// </summary>
        public ICommand EffluentCommand { get; set; }
        /// <summary>
        /// 中心出水功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Effluent()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _ = ApplicationViewModel.PanelButton;
                if (!_.AxisEffluent)
                {
                    _.AxisLooseKnife = false;
                }
                _.AxisEffluent = !_.AxisEffluent;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 鬆拉刀命令 
        /// </summary>
        public ICommand LooseKnifeCommand { get; set; }
        /// <summary>
        /// 鬆拉刀功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand LooseKnife()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;


                PanelButton _ = ApplicationViewModel.PanelButton;
                if (!_.AxisLooseKnife)
                {
                    _.AxisEffluent = false;
                    _.AxisRotation = false;
                    _.AxisStop = true;
                }
                else
                    _ = ApplicationViewModel.PanelButton;

                _.AxisLooseKnife = !_.AxisLooseKnife;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO

            });
        }

        /// <summary>
        /// 刀庫設定命令 
        /// </summary>
        public ICommand DrillWarehouseCommand { get; set; }
        /// <summary>
        /// 刀庫設定功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand DrillWarehouse()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;

                if (!ApplicationViewModel.PanelButton.DrillWarehouse)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.DrillWarehouse = !_.DrillWarehouse;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 送料手臂命令 
        /// </summary>
        public ICommand FeederCommand { get; set; }
        /// <summary>
        /// 送料手臂功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Feeder()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.Hand)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.Hand = !_.Hand;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO
            });
        }

        /// <summary>
        /// 夾具下壓命令 
        /// </summary>
        public ICommand ClampDownCommand { get; set; }
        /// <summary>
        /// 夾具下壓功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand ClampDown()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.ClampDown)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.ClampDown = !_.ClampDown;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO

            });
        }

        /// <summary>
        /// 夾具側壓命令 
        /// </summary>
        public ICommand SideClampCommand { get; set; }
        /// <summary>
        /// 夾具側壓功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand SideClamp()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.SideClamp)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.SideClamp = !_.SideClamp;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO

            });
        }
        /// <summary>
        /// 開啟專案命令
        /// </summary>
        public ICommand OpenProjectCommand { get; set; }
        public WPFBase.RelayParameterizedCommand OpenProject()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                if (e == null)
                    return;

                string _ = e.ToString();
                ApplicationViewModel.ProjectName = _;
                OpenProjectControl = true;
            });
        }

        /// <summary>
        /// 新建專案命令
        /// </summary>
        public ICommand OutProjectNameCommand { get; set; }
        private WPFBase.RelayParameterizedCommand OutProjectName()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {

                string _ = e.ToString();
                string path = $"{Properties.SofSetting.Default.LoadPath}\\{_}";
                //查詢是否有相同專案名稱
                if (Directory.Exists(path)) //如果有相同專案名稱請彈出通知
                {
                    MessageBox.Show("已經有此專案名稱了, 請重新輸入", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                }
                else
                {
                    ApplicationViewModel.ProjectName = _;
                    //TODO: 這裡是一開始新建專案後建置資料夾的地方
                    Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
                    Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
                    Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
                    Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
                    Directory.CreateDirectory(ApplicationVM.FileSteelAssembly());//存放構件資料序列化的資料夾
                    Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
                    Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
                    SerializationHelper.SerializeBinary(new ObservableCollection<DataCorrespond>(), ApplicationVM.PartList());
                    SerializationHelper.SerializeBinary(new ObservableCollection<string>(), ApplicationVM.ProfileList());//斷面規格列表
                    this.NewProjectControl = true; //隱藏新建專案控制項
                }
            });
        }

        /// <summary>
        /// 隱藏新建專案、開啟專案的控制項
        /// </summary>
        public ICommand CollapsedCommand { get; set; }
        private WPFBase.RelayCommand Collapsed()
        {
            return new WPFBase.RelayCommand(() =>
            {
                this.NewProjectControl = true;
                this.OpenProjectControl = true;
                this.ImportFileControl = true;
            });
        }

        /// <summary>
        /// 其他物件顯示命令 
        /// </summary>
        public ICommand NewProjectShowCommand { get; set; }
        /// <summary>
        /// 顯示其他物件
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand NewProjectShow()
        {
            return new WPFBase.RelayCommand(() =>
            {
                this.NewProjectControl = false;
            });
        }

        /// <summary>
        /// 專案瀏覽命令 
        /// </summary>
        public ICommand WatchProjectCommand { get; set; }
        /// <summary>
        /// 專案瀏覽功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand WatchProject()
        {
            return new WPFBase.RelayCommand(() =>
            {

                ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory());
                //判斷是軟體用的模型資料夾
                //foreach (var model in Directory.GetDirectories(ModelPath.Models))
                //{
                //    if (Directory.Exists($"{model}\\{ModelPath.Nc}"))
                //        if (Directory.Exists($"{model}\\{ModelPath.Material}"))
                //            if (Directory.Exists($"{model}\\{ModelPath.Dev_Part}"))
                //                ProjectList.Add(Path.GetFileNameWithoutExtension(model));
                //}
                this.OpenProjectControl = false;
            });
        }
        /// <summary>
        /// 匯入檔案命令 
        /// </summary>
        public ICommand ImportFileCommand { get; set; }
        /// <summary>
        /// 匯入檔案功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand ImportFile()
        {
            return new WPFBase.RelayCommand(()=>
            {
                ImportFileControl = false;
            });
        }

        /// <summary>
        /// 製品設定命令
        /// </summary>
        public ICommand ProductSettingCommand { get; set; }
        /// <summary>
        /// 製品設定功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand ProductSetting()
        {
            return new WPFBase.RelayCommand(() =>
            {
                GC.Collect();
                PanelButton panelButton = new PanelButton();
                panelButton = StateClear(ApplicationViewModel.PanelButton);
                WriteCodesysMemor.SetPanel(panelButton);
                ApplicationViewModel.CurrentPage = ApplicationPage.ObSetting;
            });
        }

        /// <summary>
        /// 排版設定命令
        /// </summary>
        public ICommand TypeSettingCommand { get; set; }
        /// <summary>
        /// 排版設定功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand TypeSetting()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ApplicationViewModel.CurrentPage = ApplicationPage.TypeSetting;
            });
        }

        /// <summary>
        /// 加工監控命令
        /// </summary>
        public ICommand MonitorCommand { get; set; }
        /// <summary>
        /// 加工監控功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Monitor()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ApplicationViewModel.CurrentPage = ApplicationPage.Monitor;
            });
        }

        /// <summary>
        /// 軟體設定命令
        /// </summary>
        public ICommand SoftwareSettingsCommand { get; set; }
        /// <summary>
        /// 軟體設定功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand SoftwareSettings()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ApplicationViewModel.CurrentPage = ApplicationPage.SofSettings;
            });
        }

        /// <summary>
        /// 捲削機命令
        /// </summary>
        public ICommand RollingCommand { get; set; }
        /// <summary>
        /// 捲削機功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Rolling()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (SLPEMS() || SLPAlarm())
                    return;

                PanelButton _;
                if (!ApplicationViewModel.PanelButton.Volume)
                    _ = StateClear(ApplicationViewModel.PanelButton);
                else
                    _ = ApplicationViewModel.PanelButton;

                _.Volume = !_.Volume;
                WriteCodesysMemor.SetPanel(_);//寫入操控面板 IO

            });
        }

        #endregion
        /// <summary>
        /// 最小窗口寬度
        /// </summary>
        public new double WindowMinimmumWidth { get; set; } = 1024;
        /// <summary>
        /// 最小窗口高度
        /// </summary>
        public new double WindowMinimmumHeight { get; set; } = 1280;
        /// <summary>
        /// 專案列表
        /// </summary>
        /// <remarks>
        /// 在 <see cref="ApplicationVM.DirectoryModel"/> 內所有的專案
        /// </remarks>
        public ObservableCollection<string> ProjectList { get; set; } = new ObservableCollection<string>();
        /// <summary>
        ///  隱藏新建專案控制項
        /// </summary>
        /// <remarks>
        /// 要隱藏控制項傳入true,
        /// 不隱藏控制項傳入false
        /// </remarks>
        public bool NewProjectControl { get; set; } = true;
        /// <summary>
        ///  隱藏開啟專案控制項
        /// </summary>
        /// <remarks>
        /// 要隱藏控制項傳入true,
        /// 不隱藏控制項傳入false
        /// </remarks>
        public bool OpenProjectControl { get; set; } = true;
        /// <summary>
        /// 匯入檔案
        /// </summary>
        public bool ImportFileControl { get; set; } = true;
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="window"></param>
        public WindowsVM(Window window) : base(window)
        {
            OillCommand = Oill();
            SettingParCommand = SettingPar();
            AlarmCommand = Alarm();
            LanguageCommand = Language();
            OriginCommand = Origin();

            /*手動面板*/
            //ExportRackCommand = ExportRack();
            //EntranceRackCommand = EntranceRack();
            RackOperationCommand = RackOperation();
            MainAxisModeCommand = MainAxisMode();
            MainAxisRotationCommand = MainAxisRotation();
            MainAxisStopCommand = MainAxisStop();
            EffluentCommand = Effluent();
            LooseKnifeCommand = LooseKnife();
            DrillWarehouseCommand = DrillWarehouse();
            FeederCommand = Feeder();
            ClampDownCommand = ClampDown();
            SideClampCommand = SideClamp();
            RollingCommand = Rolling();

            /*軟體*/
            NewProjectShowCommand = NewProjectShow();
            WatchProjectCommand = WatchProject();
            ImportFileCommand = ImportFile();
            ProductSettingCommand = ProductSetting();
            TypeSettingCommand = TypeSetting();
            MonitorCommand = Monitor();
            SoftwareSettingsCommand = SoftwareSettings();
            OutProjectNameCommand = OutProjectName();
            CollapsedCommand = Collapsed();
            OpenProjectCommand = OpenProject();
            /*返回首頁*/
            HomeCommand = Home();
        }
        #region 私有方法
        /// <summary>
        /// 清除Codesys原有的按鈕狀態
        /// </summary>
        /// <param name="panelButton"></param>
        /// <returns></returns>
        private PanelButton StateClear(PanelButton panelButton)
        {
            //panelButton.ExportRack = false;
            //panelButton.EntranceRack = false;
            //panelButton.MainAxisMode = false;
            //panelButton.Middle = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            //panelButton.Righ = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            //panelButton.Left = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            panelButton.AxisEffluent = false;
            panelButton.AxisLooseKnife = false;
            panelButton.AxisRotation = false;
            panelButton.AxisStop = false;
            panelButton.MainAxisMode = false;
            panelButton.Hand = false;
            panelButton.ClampDown = false;
            panelButton.SideClamp = false;
            panelButton.Volume = false;
            panelButton.DrillWarehouse = false;
            panelButton.Hand = false;
            return panelButton;
        }
        #endregion
    }
    /// <summary>
    /// Loading視窗的ViewModel
    /// </summary>
    public class SplashScreenMainViewModel
    {
        /// <summary>
        /// Loading視窗的類型，分別為Fluent、Themed、WaitIndicator
        /// </summary>
        public virtual PredefinedSplashScreenType SplashScreenType { get; set; }
        /// <summary>
        /// 視窗的起始位置
        /// </summary>
        public virtual WindowStartupLocation StartupLocation { get; set; }
        /// <summary>
        /// 是否追隨主畫面
        /// </summary>
        public virtual bool TrackOwnerPosition { get; set; }
        /// <summary>
        /// 綁定背景視窗的方式
        /// </summary>
        public virtual InputBlockMode InputBlock { get; set; }
    }
}
