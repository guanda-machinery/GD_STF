using GD_STD;
using GD_STD.Base;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFSTD105.Properties.MecSetting;
namespace WPFSTD105
{
    /// <summary>
    /// 設定檔轉換值方法
    /// </summary>
    public static class SettingHelper
    {
        /// <summary>
        /// 將文字訊息轉換 <see cref="Axis3D"/>
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Axis3D ToAxis3D(string position)
        {
            string[] vs = position.Split(',');
            double.TryParse(vs[0], out double xv);
            double.TryParse(vs[1], out double yv);
            double.TryParse(vs[2], out double zv);
            return new Axis3D(xv, yv, zv);
        }
        /// <summary>
        /// 將文字訊息轉換 <see cref="Axis4D"/>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<Axis4D> ToAxis4D(string str)
        {
            List<Axis4D> result = new List<Axis4D>();
            string[] vs = str.Split('(', ')').Where(el => el != "").ToArray();
            for (int i = 0; i < vs.Length; i++)
            {
                string[] value = vs[i].Split(',');
                if (value.Length == 4)
                {
                    double.TryParse(value[0], out double xv);
                    double.TryParse(value[1], out double yv);
                    double.TryParse(value[2], out double zv);
                    double.TryParse(value[3], out double masterPhase);
                    result.Add(new Axis4D()
                    {
                        X = xv,
                        Y = yv,
                        Z = zv,
                        MasterPhase = masterPhase
                    });
                }
                else
                {
                    throw new Exception("刀庫參數寫入失敗，缺少相位點");
                }
            }
            return result;
        }
        /// <summary>
        /// 產生位置並賦予換刀座標
        /// </summary>
        /// <param name="drillSettings"></param>
        /// <param name="position"></param>
        public static DrillSetting[] Location(DrillSetting[] drillSettings, List<Axis4D> position)
        {
            for (int i = 0; i < drillSettings.Length; i++)
            {
                drillSettings[i].Index = Convert.ToInt16(i + 1);
                drillSettings[i].Position = position[i];
            }
            return drillSettings;
        }
        /// <summary>
        /// 設置刀庫位置座標
        /// </summary>
        /// <returns></returns>
        public static DrillWarehouse SetPosition(DrillWarehouse drillSettings)
        {
            List<Axis4D> _LeftExport = ToAxis4D(Default.LeftExportDrillSetting);
            List<Axis4D> _LeftEntrance = ToAxis4D(Default.LeftEntranceDrillSetting);
            List<Axis4D> _Middle = ToAxis4D(Default.MiddleDrillSetting);
            List<Axis4D> _RightEntrance = ToAxis4D(Default.RightEntranceDrillSetting);
            List<Axis4D> _RightExport = ToAxis4D(Default.RightExportDrillSetting);

            drillSettings.Middle = Location(drillSettings.Middle, _Middle);
            drillSettings.LeftExport = Location(drillSettings.LeftExport, _LeftExport);
            drillSettings.RightExport = Location(drillSettings.RightExport, _RightExport);
            drillSettings.LeftEntrance = Location(drillSettings.LeftEntrance, _LeftEntrance);
            drillSettings.RightEntrance = Location(drillSettings.RightEntrance, _RightEntrance);

            return drillSettings;
        }
        /// <summary>
        /// 取得機械校正參數
        /// </summary>
        public static GD_STD.Phone.MechanicalSetting GetMechanicalSetting()
        {
            GD_STD.Phone.MechanicalSetting result = new GD_STD.Phone.MechanicalSetting();
            result.Left = new AxisSetting()
            {
                MeasuringPosition = ToAxis3D(Default.LeftMeasuringPosition),
                SensorZero = Default.LeftSensorZero,
                TotalLength = Default.LeftTotalLength,
            };
            result.Right = new AxisSetting()
            {
                MeasuringPosition = ToAxis3D(Default.RighMeasuringPosition),
                SensorZero = Default.RighSensorZero,
                TotalLength = Default.RighTotalLength,
            };
            result.Middle = new MiddleAxisSetting()
            {
                MeasuringPosition = ToAxis3D(Default.MiddleMeasuringPosition),
                SensorZero = Default.MiddleSensorZero,
                TotalLength = Default.MiddleTotalLength,
                HighSensorZ = Default.MiddleHighSensorZ,
            };
            result.SideClamp = new SideClamp()
            {
                EntranceL = Default.SideClampEntranceL,
                ExportL = Default.SideClampExportL,
            };
            result.ClampDown = new ClampDownSetting()
            {
                EntranceL = Default.ClampDownEntranceL,
                EntranceR = Default.ClampDownEntranceR,
                ExportL = Default.ClampDownExportL,
                ExportR = Default.ClampDownExportR,
                Thickness = Default.ClampDownThickness,
            };
            result.Hand = new HandSetting()
            {
                HandJobLimit = Default.HandJobLimit,
                MaterialCorrectionWorkRange = Default.HandMaterialCorrectionWorkRange,
                MaterialZeroToWorkRange = Default.HandMaterialZeroToWorkRange,
                OutClampToZeroLength = Default.HandOutClampToZeroLength,
            };
            return result;
        }
    }
}
