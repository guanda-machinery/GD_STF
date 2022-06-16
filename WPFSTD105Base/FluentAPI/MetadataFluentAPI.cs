using DevExpress.Mvvm.DataAnnotations;
using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 關聯資料列別轉換器
    /// </summary>
    public static class MetadataFluentAPI
    {
        /// <summary>
        /// 建構 <see cref="MecSetting"/> 數據
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildMetadata(MetadataBuilder<MecSetting> builder)
        {
            builder.DataFormLayout()
                    /*********************************************/
                    .GroupBox("左軸", System.Windows.Controls.Orientation.Vertical)
                        /*********************************************/
                        .Group(null, System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("測刀量長座標", System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.LeftMeasuringPositionX)
                                .ContainsProperty(el => el.LeftMeasuringPositionY)
                                .ContainsProperty(el => el.LeftMeasuringPositionZ)
                            .EndGroup()
                            .Group(null, System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.LeftTotalLength)
                                .ContainsProperty(el => el.LeftTorque)
                                .ContainsProperty(el => el.LeftYAxisLimit)
                                .ContainsProperty(el => el.LeftOriginToSideLength)
                                .ContainsProperty(el => el.LeftElectricalCurrent)
                            .EndGroup()
                        /*********************************************/
                        .EndGroup()
                        /*********************************************/
                        .GroupBox("入口刀庫", System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("第 1 座")
                                .ContainsProperty(el => el.LeftEntranceDrillPosition1X)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition1Y)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition1Z)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition1MasterPhase)
                            .EndGroup()
                            .GroupBox("第 2 座")
                                .ContainsProperty(el => el.LeftEntranceDrillPosition2X)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition2Y)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition2Z)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition2MasterPhase)
                            .EndGroup()
                            .GroupBox("第 3 座")
                                .ContainsProperty(el => el.LeftEntranceDrillPosition3X)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition3Y)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition3Z)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition3MasterPhase)
                            .EndGroup()
                            .GroupBox("第 4 座")
                                .ContainsProperty(el => el.LeftEntranceDrillPosition4X)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition4Y)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition4Z)
                                .ContainsProperty(el => el.LeftEntranceDrillPosition4MasterPhase)
                            .EndGroup()
                        .EndGroup()
                        /*********************************************/
                        .GroupBox("出口刀庫", System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("第 1 座")
                                .ContainsProperty(el => el.LeftExportDrillPosition1X)
                                .ContainsProperty(el => el.LeftExportDrillPosition1Y)
                                .ContainsProperty(el => el.LeftExportDrillPosition1Z)
                                .ContainsProperty(el => el.LeftExportDrillPosition1MasterPhase)
                            .EndGroup()
                            .GroupBox("第 2 座")
                                .ContainsProperty(el => el.LeftExportDrillPosition2X)
                                .ContainsProperty(el => el.LeftExportDrillPosition2Y)
                                .ContainsProperty(el => el.LeftExportDrillPosition2Z)
                                .ContainsProperty(el => el.LeftExportDrillPosition2MasterPhase)
                            .EndGroup()
                            .GroupBox("第 3 座")
                                .ContainsProperty(el => el.LeftExportDrillPosition3X)
                                .ContainsProperty(el => el.LeftExportDrillPosition3Y)
                                .ContainsProperty(el => el.LeftExportDrillPosition3Z)
                                .ContainsProperty(el => el.LeftExportDrillPosition3MasterPhase)
                            .EndGroup()
                            .GroupBox("第 4 座")
                                .ContainsProperty(el => el.LeftExportDrillPosition4X)
                                .ContainsProperty(el => el.LeftExportDrillPosition4Y)
                                .ContainsProperty(el => el.LeftExportDrillPosition4Z)
                                .ContainsProperty(el => el.LeftExportDrillPosition4MasterPhase)
                            .EndGroup()
                        /*********************************************/
                        .EndGroup()
                    /*********************************************/
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("中軸", System.Windows.Controls.Orientation.Vertical)
                        /*********************************************/
                        .Group(null, System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("測刀量長座標", System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.MiddleMeasuringPositionX)
                                .ContainsProperty(el => el.MiddleMeasuringPositionY)
                                .ContainsProperty(el => el.MiddleMeasuringPositionZ)
                            .EndGroup()
                            .Group(null, System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.MiddleTotalLength)
                                .ContainsProperty(el => el.MiddleTorque)
                                .ContainsProperty(el => el.MiddleYAxisLimit)
                                .ContainsProperty(el => el.MiddleOriginToSideLength)
                                .ContainsProperty(el => el.MiddleElectricalCurrent)
                            .EndGroup()
                        .EndGroup()
                        /*********************************************/
                        .GroupBox("刀庫", System.Windows.Controls.Orientation.Vertical)
                            .Group(null, System.Windows.Controls.Orientation.Horizontal)
                                .GroupBox("第 1 座")
                                    .ContainsProperty(el => el.MiddleDrillPosition1X)
                                    .ContainsProperty(el => el.MiddleDrillPosition1Y)
                                    .ContainsProperty(el => el.MiddleDrillPosition1Z)
                                    .ContainsProperty(el => el.MiddleDrillPosition1MasterPhase)
                                .EndGroup()
                                .GroupBox("第 2 座")
                                    .ContainsProperty(el => el.MiddleDrillPosition2X)
                                    .ContainsProperty(el => el.MiddleDrillPosition2Y)
                                    .ContainsProperty(el => el.MiddleDrillPosition2Z)
                                    .ContainsProperty(el => el.MiddleDrillPosition2MasterPhase)
                                .EndGroup()
                                .GroupBox("第 3 座")
                                    .ContainsProperty(el => el.MiddleDrillPosition3X)
                                    .ContainsProperty(el => el.MiddleDrillPosition3Y)
                                    .ContainsProperty(el => el.MiddleDrillPosition3Z)
                                    .ContainsProperty(el => el.MiddleDrillPosition3MasterPhase)
                                .EndGroup()

                            .EndGroup()
                            .Group("123", System.Windows.Controls.Orientation.Horizontal)
                                .GroupBox("第 4 座")
                                    .ContainsProperty(el => el.MiddleDrillPosition4X)
                                    .ContainsProperty(el => el.MiddleDrillPosition4Y)
                                    .ContainsProperty(el => el.MiddleDrillPosition4Z)
                                    .ContainsProperty(el => el.MiddleDrillPosition4MasterPhase)
                                .EndGroup()
                                .GroupBox("第 5 座")
                                    .ContainsProperty(el => el.MiddleDrillPosition5X)
                                    .ContainsProperty(el => el.MiddleDrillPosition5Y)
                                    .ContainsProperty(el => el.MiddleDrillPosition5Z)
                                    .ContainsProperty(el => el.MiddleDrillPosition5MasterPhase)
                                .EndGroup()
                            .EndGroup()
                        .EndGroup()
                    /*********************************************/
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("右軸", System.Windows.Controls.Orientation.Vertical)
                        /*********************************************/
                        .Group(null, System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("測刀量長座標", System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.RightMeasuringPositionX)
                                .ContainsProperty(el => el.RightMeasuringPositionY)
                                .ContainsProperty(el => el.RightMeasuringPositionZ)
                            .EndGroup()
                            .Group(null, System.Windows.Controls.Orientation.Vertical)
                                .ContainsProperty(el => el.RightTotalLength)
                                .ContainsProperty(el => el.RightTorque)
                                .ContainsProperty(el => el.RightYAxisLimit)
                                .ContainsProperty(el => el.RightOriginToSideLength)
                                .ContainsProperty(el => el.RightElectricalCurrent)
                            .EndGroup()
                        .EndGroup()
                        /*********************************************/
                        .GroupBox("入口刀庫", System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("第 1 座")
                                .ContainsProperty(el => el.RightEntranceDrillPosition1X)
                                .ContainsProperty(el => el.RightEntranceDrillPosition1Y)
                                .ContainsProperty(el => el.RightEntranceDrillPosition1Z)
                                .ContainsProperty(el => el.RightEntranceDrillPosition1MasterPhase)
                            .EndGroup()
                            .GroupBox("第 2 座")
                                .ContainsProperty(el => el.RightEntranceDrillPosition2X)
                                .ContainsProperty(el => el.RightEntranceDrillPosition2Y)
                                .ContainsProperty(el => el.RightEntranceDrillPosition2Z)
                                .ContainsProperty(el => el.RightEntranceDrillPosition2MasterPhase)
                            .EndGroup()
                            .GroupBox("第 3 座")
                                .ContainsProperty(el => el.RightEntranceDrillPosition3X)
                                .ContainsProperty(el => el.RightEntranceDrillPosition3Y)
                                .ContainsProperty(el => el.RightEntranceDrillPosition3Z)
                                .ContainsProperty(el => el.RightEntranceDrillPosition3MasterPhase)
                            .EndGroup()
                            .GroupBox("第 4 座")
                                .ContainsProperty(el => el.RightEntranceDrillPosition4X)
                                .ContainsProperty(el => el.RightEntranceDrillPosition4Y)
                                .ContainsProperty(el => el.RightEntranceDrillPosition4Z)
                                .ContainsProperty(el => el.RightEntranceDrillPosition4MasterPhase)
                            .EndGroup()
                        .EndGroup()
                        /*********************************************/
                        .GroupBox("出口刀庫", System.Windows.Controls.Orientation.Horizontal)
                            .GroupBox("第 1 座")
                                .ContainsProperty(el => el.RightExportDrillPosition1X)
                                .ContainsProperty(el => el.RightExportDrillPosition1Y)
                                .ContainsProperty(el => el.RightExportDrillPosition1Z)
                                .ContainsProperty(el => el.RightExportDrillPosition1MasterPhase)
                            .EndGroup()
                            .GroupBox("第 2 座")
                                .ContainsProperty(el => el.RightExportDrillPosition2X)
                                .ContainsProperty(el => el.RightExportDrillPosition2Y)
                                .ContainsProperty(el => el.RightExportDrillPosition2Z)
                                .ContainsProperty(el => el.RightExportDrillPosition2MasterPhase)
                            .EndGroup()
                            .GroupBox("第 3 座")
                                .ContainsProperty(el => el.RightExportDrillPosition3X)
                                .ContainsProperty(el => el.RightExportDrillPosition3Y)
                                .ContainsProperty(el => el.RightExportDrillPosition3Z)
                                .ContainsProperty(el => el.RightExportDrillPosition3MasterPhase)
                            .EndGroup()
                            .GroupBox("第 4 座")
                                .ContainsProperty(el => el.RightExportDrillPosition4X)
                                .ContainsProperty(el => el.RightExportDrillPosition4Y)
                                .ContainsProperty(el => el.RightExportDrillPosition4Z)
                                .ContainsProperty(el => el.RightExportDrillPosition4MasterPhase)
                            .EndGroup()
                        /*********************************************/
                        .EndGroup()
                    /*********************************************/
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("送料手臂", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.MaterialCorrectionWorkRange)
                        .ContainsProperty(el => el.MaterialZeroToWorkRange)
                        .ContainsProperty(el => el.HandJobLimit)
                        .ContainsProperty(el => el.OutClampToZeroLength)
                        .ContainsProperty(el => el.InTotalLength)
                        .ContainsProperty(el => el.OutTotalLength)
                        .ContainsProperty(el => el.Length)
                        .ContainsProperty(el => el.VerticalGrippingXLimit)
                        .ContainsProperty(el => el.VerticalToOriginLength)
                        .ContainsProperty(el => el.ArmInDownWingSensorToForntLength)
                        .ContainsProperty(el => el.ArmInDownBellySensorToForntLength)
                        .ContainsProperty(el => el.ArmInDownCheckPosition)
                        .ContainsProperty(el => el.ArmXToOriginPoint)
                        .ContainsProperty(el => el.ArmInTouchOriginLength)
                        .ContainsProperty(el => el.ArmFeedOriginToOutUpRoll)
                        .ContainsProperty(el => el.WingSensorHigh)
                        .ContainsProperty(el => el.BellySensorHigh)
                        .ContainsProperty(el => el.FeedSlowDownPointTotalLength)
                        .ContainsProperty(el => el.SlowToOriginLength)
                        .ContainsProperty(el => el.XLimit)
                        .ContainsProperty(el => el.ArmInZSafeLength)
                        .ContainsProperty(el => el.ArmOutZSafeLength)
                        .ContainsProperty(el => el.DeceleratingOrigin)
                        .ContainsProperty(el => el.HorizontalCompensate)
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("下壓夾具", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.ClampDownEntranceL)
                        .ContainsProperty(el => el.ClampDownExportL)
                        .ContainsProperty(el => el.ClampDownEntranceR)
                        .ContainsProperty(el => el.ClampDownExportR)
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("側壓夾具", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.SideClampEntranceL)
                        .ContainsProperty(el => el.SideClampExportL)
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("入口料架", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.EntranceCarLength)
                        .ContainsProperty(el => el.EntranceCarRaisedLength)
                        .ContainsProperty(el => el.EntranceOriginToLocationPoint)
                        .ContainsProperty(el => el.EntranceSortTotalLength)
                        .ContainsProperty(el => el.EntranceLimit)
                        .ContainsProperty(el => el.EntranceResolution)
                        .ContainsProperty(el => el.EntranceSafetyGap)
                        .ContainsProperty(el => el.EntranceRideRange)
                        .ContainsProperty(el => el.EntranceSpeed1Correct)
                        .ContainsProperty(el => el.EntranceSpeed2Correct)
                    .EndGroup()
                    .GroupBox("出口料架", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.ExportCarLength)
                        .ContainsProperty(el => el.ExportCarRaisedLength)
                        .ContainsProperty(el => el.ExportOriginToLocationPoint)
                        .ContainsProperty(el => el.ExportSortTotalLength)
                        .ContainsProperty(el => el.ExportLimit)
                        //.ContainsProperty(el => el.ExportSortCorrection)
                        //.ContainsProperty(el => el.ExportReverseSortCorrection)
                        //.ContainsProperty(el => el.ExportToArmSideCorrection)
                        .ContainsProperty(el => el.ExportResolution)
                        .ContainsProperty(el => el.ExportSafetyGap)
                        .ContainsProperty(el => el.ExportConveyorPosition1)
                        .ContainsProperty(el => el.ExportConveyorPosition2)
                        .ContainsProperty(el => el.ExportConveyorPosition3)
                        .ContainsProperty(el => el.ExportConveyorPosition4)
                        .ContainsProperty(el => el.ExportSafety)
                        .ContainsProperty(el => el.ExportRideRange)
                        .ContainsProperty(el => el.ExportSpeed1Correct)
                        .ContainsProperty(el => el.ExportSpeed2Correct)
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("RH 鑽孔加工安全間隙", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.DrillProtectionX)
                        .ContainsProperty(el => el.DrillProtectionLRY)
                        .ContainsProperty(el => el.DrillProtectionMY)
                        .ContainsProperty(el => el.DrillProtectionU_And_BOX_Y_Protection_Length)
                    .EndGroup()
                    /*********************************************/
                    .GroupBox("方管 & 槽鐵鑽孔加工安全間隙", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.OtherProtectionX)
                        .ContainsProperty(el => el.OtherProtectionLRY)
                        .ContainsProperty(el => el.OtherProtectionMY)
                    .EndGroup()
                    .GroupBox("其他", System.Windows.Controls.Orientation.Vertical)
                        .ContainsProperty(el => el.MaterialAllowTolerance)
                        .ContainsProperty(el => el.AllowDrillTolerance)
                        .ContainsProperty(el => el.SafeTouchLength)
                        .ContainsProperty(el => el.ThroughLength)
                        .ContainsProperty(el => el.RankStratLength)
                        .ContainsProperty(el => el.RankEndLength)
                        .ContainsProperty(el => el.TouchMUL)
                        .ContainsProperty(el => el.EndMUL)
                        .ContainsProperty(el => el.StrWidth)
                        .ContainsProperty(el => el.MZOriginToDownBlockHeight)
                        .ContainsProperty(el => el.MZOriginToPillarHeight)
                        .ContainsProperty(el => el.MYSafeLength)
                        .ContainsProperty(el => el.MRZSafeLength)
                        .ContainsProperty(el => el.ProtectDrillLength)
                        .ContainsProperty(el => el.PrepWorkLength)
                        .ContainsProperty(el => el.LZMYSafeLength)
                    .EndGroup();
            //唯獨區塊
            //builder.Property(el => el.LeftMeasuringPositionX).ReadOnly();

            var infos = typeof(MecSetting).GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                if (!infos[i].PropertyType.IsClass)
                {
                    InitializesBuilder(builder, infos[i]);
                }
                else
                {
                    builder.Property<string>(infos[i].Name).NotAutoGenerated();
                }
            }
        }

        /// <summary>
        /// 建構 <see cref="OptionSettings"/> 數據
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildMetadata(MetadataBuilder<OptionSettings> builder)
        {
            builder.DataFormLayout()
                .GroupBox("刀庫", System.Windows.Controls.Orientation.Vertical)
                    .Group("中軸", System.Windows.Controls.Orientation.Horizontal)
                    .ContainsProperty(el => el.Middle)
                    .EndGroup()
                    .Group("左軸", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.LeftEntrance)
                        .ContainsProperty(el => el.LeftExport)
                    .EndGroup()
                    .Group("右軸", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.RightExport)
                        .ContainsProperty(el => el.RightEntrance)
                    .EndGroup()
                .EndGroup()
                .GroupBox("橫移料架", System.Windows.Controls.Orientation.Vertical)
                    .Group("自動有無", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.EntranceTraverseAuto)
                        .ContainsProperty(el => el.ExportTraverseAuto)
                    .EndGroup()
                    .Group("數量", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.EntranceTraverseNumber)
                        .ContainsProperty(el => el.ExportTraverseNumber)
                        .ContainsProperty(el => el.StepAsideCount)
                    .EndGroup()
                .EndGroup()
                .GroupBox("鑽刀功能", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.DrawLine)
                        .ContainsProperty(el => el.MillingCutter)
                        .ContainsProperty(el => el.Tapping)
               .EndGroup()
               .GroupBox("感測器", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.ExportTraverseSnsorCount)
                        .ContainsProperty(el => el.ExportTraverseLeftSnsorCount)
                        .ContainsProperty(el => el.ExportTraverseRightSnsorCount)
                .EndGroup()
                .GroupBox("手自動功能", System.Windows.Controls.Orientation.Horizontal)
                        .ContainsProperty(el => el.AutoDrill)
                        .ContainsProperty(el => el.HandAuto)
                .EndGroup()
                .GroupBox("設備輸入輸出方式", System.Windows.Controls.Orientation.Horizontal)
                    .ContainsProperty(el => el.Reverse)
                    .ContainsProperty(el => el.ExportTraverseRoute)
                .EndGroup();

            var infos = typeof(OptionSettings).GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                if (!infos[i].PropertyType.IsClass)
                {
                    InitializesBuilder(builder, infos[i]);
                }
                else
                {
                    builder.Property<string>(infos[i].Name).NotAutoGenerated();
                }
            }
        }

        /// <summary>
        /// 初始化屬性數據構建器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="Info"></param>
        private static void InitializesBuilder<T>(MetadataBuilder<T> builder, PropertyInfo Info)
        {
            MVVMAttribute attribute = Info.GetCustomAttribute<MVVMAttribute>();
            if (attribute != null)
            {
                var property = builder.Property<string>(Info.Name);
                property.DisplayName(attribute.Title);
                if (attribute.Description != null)
                {
                    property.Description(attribute.Description);
                }
                if (attribute.ReadOnly)
                {

                }
                return;
            }
            throw new Exception($"找不到 {Info.Name} MVVMAttribute 附加屬性");
        }
        ///// <summary>
        ///// 建構 <see cref="MecSetting"/> 數據
        ///// </summary>
        ///// <param name="builder"></param>
        //public static void BuildMetadata(MetadataBuilder<AxisSetting> builder)
        //{
        //    builder.DataFormLayout()
        //        .GroupBox("軸向")
        //            .ContainsProperty(el => el.MeasuringPosition)
        //            .ContainsProperty(el => el.OriginToSideLength)
        //            .ContainsProperty(el=>el.Torque)
        //            .ContainsProperty(el=>el.TotalLength)
        //            .ContainsProperty(el => el.YAxisLimit)
        //        .EndGroup();
        //}
        ///// <summary>
        ///// 建構 <see cref="MecSetting"/> 數據
        ///// </summary>
        ///// <param name="builder"></param>
        //public static void BuildMetadata(MetadataBuilder<Axis3D> builder)
        //{
        //    builder.DataFormLayout()
        //            .ContainsProperty(el => el.X)
        //            .ContainsProperty(el => el.Y)
        //            .ContainsProperty(el => el.Z);
        //}
    }
}
