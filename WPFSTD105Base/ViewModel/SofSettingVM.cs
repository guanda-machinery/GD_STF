using GD_STD;
using GD_STD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.CodesysIIS;
using WPFSTD105;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using System.Drawing.Text;
using System.Windows.Media;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 軟體設定
    /// </summary>
    public class SofSettingVM : BaseViewModel
    {

        /// <inheritdoc/>
        public SofSettingVM()
        {
            //工程模式才可註冊的命令
            //if (ViewLocator.ApplicationViewModel.EngineeringMode)
            //{
            SaveOptionalCommand = SaveOptional();
            STDSerialization ser = new STDSerialization();
            MecSetting = ser.GetMecSetting();
            OptionSettings = ser.GetOptionSettings();
            SaveMecSettingCommand = SaveMecSetting();
            SaveOptionSettingCommand = SaveOprionSetting();
            OpenColorPickerCommand = OpenColorPicker();
            GetCustomColorCommand = GetCustomColor();
        }
        #region 命令
        /// <summary>
        /// 取得自訂顏色代碼
        /// </summary>
        public ICommand GetCustomColorCommand { get; set; }

        private ICommand GetCustomColor()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                if (ColorPickerVisibility)
                {
                    switch (SelectedItemValue)
                    {
                        //3D顏色
                        case "鑽孔顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.Hole = el.ToString();
                            break;
                        case "反白顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.HoleFinish = el.ToString();
                            break;
                        case "X軸顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.AxisX = el.ToString();
                            break;
                        case "Y軸顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.AxisY = el.ToString();
                            break;
                        case "Z軸顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.AxisZ = el.ToString();
                            break;
                        case "預覽圖示顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.ViewCubeIcon = el.ToString();
                            break;
                        case "零件顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.Part = el.ToString();
                            break;
                        case "組件A顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.Ingredient1 = el.ToString();
                            break;
                        case "組件B顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.Ingredient2 = el.ToString();
                            break;
                        case "廢料顏色設定":
                            WPFSTD105.Properties.SofSetting.Default.Null = el.ToString();
                            break;
                        case "打點":
                            WPFSTD105.Properties.SofSetting.Default.Point = el.ToString();
                            break;
                        //報表顏色
                        case "完工":
                            WPFSTD105.Properties.SofSetting.Default.Report_Finish = el.ToString();
                            break;
                        case "加工中":
                            WPFSTD105.Properties.SofSetting.Default.Report_Processing = el.ToString();
                            break;
                        case "等待搬運":
                            WPFSTD105.Properties.SofSetting.Default.Report_Waiting = el.ToString();
                            break;
                        case "搬運中":
                            WPFSTD105.Properties.SofSetting.Default.Report_Moving = el.ToString();
                            break;
                        case "字體變色":
                            WPFSTD105.Properties.SofSetting.Default.Report_Foreground = el.ToString();
                            break;
                    }
                    ColorPickerVisibility = false;
                }
            });
        }
        /*
        /// <summary>
        /// 字體Index數值
        /// </summary>
        public ICommand GetFontIndex { get; set; }
        private ICommand ConvertStringToIndex()
        {
            return new WPFBase.RelayCommand(() =>
            {
                string FontName = WPFSTD105.Properties.SofSetting.Default.FontFamily;

                FontFamily[] fonts = Fonts.SystemFontFamilies.ToArray();
                
                if (string.IsNullOrEmpty(FontName) || fonts == null)
                    return;

                for(int i =0; i< fonts.Length; i++)
                {
                    FontFamily font = fonts[i];
                    if (FontName == font.Source)
                    {
                        FontIndex = i;
                        break;
                    }
                }
            });
        }*/

        /// <summary>
        /// 打開調色盤
        /// </summary>
        public ICommand OpenColorPickerCommand { get; set; }
        private WPFBase.RelayParameterizedCommand OpenColorPicker()
        {
            return new WPFBase.RelayParameterizedCommand((object el) =>
            {
                if (el != null)
                {                    
                    SelectedItemValue = el.ToString();
                    switch (SelectedItemValue)
                    {
                        case "鑽孔顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Hole.ToString();
                            break;
                        case "反白顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Selection.ToString();
                            break;
                        case "X軸顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.AxisX.ToString();
                            break;
                        case "Y軸顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.AxisY.ToString();
                            break;
                        case "Z軸顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.AxisZ.ToString();
                            break;
                        case "預覽圖示顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.ViewCubeIcon.ToString();
                            break;
                        case "零件顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Part.ToString();
                            break;
                        case "組件A顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Ingredient1.ToString();
                            break;
                        case "組件B顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Ingredient2.ToString();
                            break;
                        case "廢料顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Null.ToString();
                            break;
                        case "餘料顏色設定":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Surplus.ToString();
                            break;
                        case "打點":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Point.ToString();
                            break;
                        //報表顏色
                        case "完工":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Report_Finish.ToString();
                            break;
                        case "加工中":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Report_Processing.ToString();
                            break;
                        case "等待搬運":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Report_Waiting.ToString();
                            break;
                        case "搬運中":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Report_Moving.ToString();
                            break;
                        case "字體變色":
                            DefaultColor = WPFSTD105.Properties.SofSetting.Default.Report_Foreground.ToString();
                            break;
                    }
                }

                if (ColorPickerVisibility)
                    ColorPickerVisibility = false;
                else
                    ColorPickerVisibility = true;
            });
        }
        /// <summary>
        /// 觸發選配存取命令
        /// </summary>
        public ICommand SaveOptionalCommand { get; set; }
        private WPFBase.RelayCommand SaveOptional()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //存取選配設定檔
                Optional.Default.Save();

                GD_STD.Phone.MecOptional mec = new GD_STD.Phone.MecOptional();//讀取記憶體
                //修改參數
                mec.HandAuto = Optional.Default.HandAuto;
                mec.LeftEntrance = Optional.Default.LeftEntrance;
                mec.LeftExport = Optional.Default.LeftExport;
                mec.Middle = Optional.Default.Middle;
                mec.RightEntrance = Optional.Default.RightEntrance;
                mec.RightExport = Optional.Default.RightExport;
                WriteCodesysMemor.SetMecOptional(mec);//寫入記憶體
            });
        }
        /// <summary>
        /// 觸發選配還原命令
        /// </summary>
        public ICommand ReductionOptionalCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            Optional.Default.Reset();
        });
        /// <summary>
        /// 觸發機械參數存取命令
        /// </summary>
        public ICommand SaveMecCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            //存取機械參數設定檔
            WPFSTD105.Properties.MecSetting.Default.Save();
            //取得 Codesys 本機狀態
            Host host = ReadCodesysMemor.GetHost();
            //修改機械參數
            //host.DrillJobLimit = Properties.MecSetting.Default.LeftMeasuringPosition;
            //host.HandJobLimit = Properties.MecSetting.Default.HandJobLimit;
            //host.SlowDownPoint = Properties.MecSetting.Default.SlowDownPoint;
            //host.MiddleMeasuringPosition = new Axis3D(Properties.MecSetting.Default.MiddleMeasuringPositionX, Properties.MecSetting.Default.MiddleMeasuringPositionY, Properties.MecSetting.Default.MiddleMeasuringPositionZ);
            //host.LeftMeasuringPosition = new Axis3D(Properties.MecSetting.Default.LeftMeasuringPositionX, Properties.MecSetting.Default.LeftMeasuringPositionY, Properties.MecSetting.Default.LeftMeasuringPositionZ);
            //host.RightMeasuringPosition = new Axis3D(Properties.MecSetting.Default.RightMeasuringPositionX, Properties.MecSetting.Default.RightMeasuringPositionY, Properties.MecSetting.Default.RightMeasuringPositionZ);
            //WPFSTD105.Properties.MecSetting.Default.DrillWarehouse = SettingHelper.SetPosition(WPFSTD105.Properties.MecSetting.Default.DrillWarehouse);
            //WriteCodesysMemor.SetDrillWarehouse(WPFSTD105.Properties.MecSetting.Default.DrillWarehouse);
            WPFSTD105.Properties.MecSetting.Default.Save();
            CodesysMemor.Create(host, WPFSTD105.Properties.MecSetting.Default.Company);
        });
        /// <summary>
        /// 觸發機械參數還原命令
        /// </summary>
        public ICommand ReductionMecCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            Properties.MecSetting.Default.Reset();
        });
        /// <summary>
        /// 觸發軟體存取設定
        /// </summary>
        public ICommand SaveSofCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            WPFSTD105.Properties.SofSetting.Default.Save();
        });
        /// <summary>
        /// 觸發軟體還原命令
        /// </summary>
        public ICommand ReductionSofCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            WPFSTD105.Properties.SofSetting.Default.Reset();
        });

        /// <summary>
        /// 儲存校機命令
        /// </summary>
        public WPFWindowsBase.RelayCommand SaveMecSettingCommand { get; set; }
        /// <summary>
        /// 儲存校機
        /// </summary>
        /// <returns></returns>
        public WPFWindowsBase.RelayCommand SaveMecSetting()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization();
                ser.SetMecSetting(MecSetting);
            });
        }

        /// <summary>
        /// 儲存選配設定命令
        /// </summary>
        public WPFWindowsBase.RelayCommand SaveOptionSettingCommand { get; set; }
        /// <summary>
        /// 儲存校機
        /// </summary>
        /// <returns></returns>
        public WPFWindowsBase.RelayCommand SaveOprionSetting()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization();
                ser.SetOptionSettings(OptionSettings);
            });
        }
        #endregion 

        #region 私有方法
        #endregion

        #region 私有屬性
        #endregion

        #region 公開屬性
        /// <summary>
        /// 控制調色盤是否顯示
        /// </summary>
        public bool ColorPickerVisibility { get; set; } 
        /// <summary>
        /// 給予調色盤初始值
        /// </summary>
        public string DefaultColor { get; set; }   
        /// <summary>
        /// 欲修改的參數暫存容器
        /// </summary>
        public string SelectedItemValue { get; set; }
        /// <summary>
        /// 校機參數
        /// </summary>
        public WPFSTD105.FluentAPI.MecSetting MecSetting { get; set; }
        /// <summary>
        /// 選配設定
        /// </summary>
        public WPFSTD105.FluentAPI.OptionSettings OptionSettings { get; set; } //= new WPFSTD105.FluentAPI.OptionSettings();
        #endregion
    }
}
