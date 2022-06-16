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
using WPFSTD105.Properties;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using WPFSTD105.Attribute;
using WPFWindowsBase;

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
            if (ViewLocator.ApplicationViewModel.EngineeringMode)
            {
                SaveOptionalCommand = SaveOptional();
            }
            OpenColorPickerCommand = OpenColorPicker();
            CloseColorPickerCommand = CloseColorPicker();
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
                        case "背景底色設定":
                            SofSetting.Default.BaseBackColor = el.ToString();
                            break;
                        case "物件底色設定":
                            SofSetting.Default.ParameterBackColor = el.ToString();
                            break;
                        case "字體顏色設定":
                            SofSetting.Default.ParameterFontColor = el.ToString();
                            break;
                        case "框線顏色設定":
                            SofSetting.Default.BorderBrushColor = el.ToString();
                            break;
                    }
                    ColorPickerVisibility = false;
                }
            });
        }

        /// <summary>
        /// 關閉調色盤
        /// </summary>
        public ICommand CloseColorPickerCommand { get; set; }
        private WPFBase.RelayCommand CloseColorPicker()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ColorPickerVisibility = false;
            });
        }
        /// <summary>
        /// 打開調色盤
        /// </summary>
        public ICommand OpenColorPickerCommand { get; set; }
        private WPFBase.RelayParameterizedCommand OpenColorPicker()
        {
            return new WPFBase.RelayParameterizedCommand((object el) =>
            {
                if (el != null)
                    SelectedItemValue = el.ToString();

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

                Host host = ReadCodesysMemor.GetHost();//讀取記憶體
                //修改參數
                host.HandAuto = Optional.Default.HandAuto;
                host.LeftEntrance = Optional.Default.LeftEntrance;
                host.LeftExport = Optional.Default.LeftExport;
                host.Middle = Optional.Default.Middle;
                host.RightEntrance = Optional.Default.RightEntrance;
                host.RightExport = Optional.Default.RightExport;
                host.Traverse = Optional.Default.Traverse;
                WriteCodesysMemor.SetHost(host);//寫入記憶體
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
            MecSetting.Default.Save();
            //取得 Codesys 本機狀態
            Host host = ReadCodesysMemor.GetHost();
            //修改機械參數
            //host.DrillJobLimit = Properties.MecSetting.Default.LeftMeasuringPosition;
            //host.HandJobLimit = Properties.MecSetting.Default.HandJobLimit;
            //host.SlowDownPoint = Properties.MecSetting.Default.SlowDownPoint;
            //host.MiddleMeasuringPosition = new Axis3D(Properties.MecSetting.Default.MiddleMeasuringPositionX, Properties.MecSetting.Default.MiddleMeasuringPositionY, Properties.MecSetting.Default.MiddleMeasuringPositionZ);
            //host.LeftMeasuringPosition = new Axis3D(Properties.MecSetting.Default.LeftMeasuringPositionX, Properties.MecSetting.Default.LeftMeasuringPositionY, Properties.MecSetting.Default.LeftMeasuringPositionZ);
            //host.RightMeasuringPosition = new Axis3D(Properties.MecSetting.Default.RightMeasuringPositionX, Properties.MecSetting.Default.RightMeasuringPositionY, Properties.MecSetting.Default.RightMeasuringPositionZ);
            MecSetting.Default.DrillWarehouse = SettingHelper.SetPosition(MecSetting.Default.DrillWarehouse);
            WriteCodesysMemor.SetDrillWarehouse(MecSetting.Default.DrillWarehouse);
            MecSetting.Default.Save();
            CodesysMemor.Create(host, MecSetting.Default.Company);
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
            SofSetting.Default.Save();
        });
        /// <summary>
        /// 觸發軟體還原命令
        /// </summary>
        public ICommand ReductionSofCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            SofSetting.Default.Reset();
        });
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
        /// 欲修改的參數暫存容器
        /// </summary>
        public string SelectedItemValue { get; set; }
        #endregion
    }
}
