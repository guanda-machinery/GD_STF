using GD_STD.Attribute;
using GD_STD.Base;
using GD_STD.Base.Additional;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 機械參數設定
    /// </summary>
    [Serializable]
    [MetadataType(typeof(MetadataFluentAPI))]
    public class MecSetting : IMecSetting
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public MecSetting()
        {
            MiddleDrillPosition = new Axis4D[5] { new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D() };
            LeftEntranceDrillPosition = new Axis4D[4] { new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D() };
            LeftExportDrillPosition = new Axis4D[4] { new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D() };
            RightEntranceDrillPosition = new Axis4D[4] { new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D() };
            RightExportDrillPosition = new Axis4D[4] { new Axis4D(), new Axis4D(), new Axis4D(), new Axis4D() };
        }
        #region 左軸
        /// <summary>
        /// 左軸設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public AxisSetting Left { get; set; } = new AxisSetting();
        /// <summary>
        /// 左軸測刀量長的 X 座標  
        /// </summary>
        [MVVM("X", false, reflection: false)]
        public double LeftMeasuringPositionX { get => Left.MeasuringPosition.X; set => Left.MeasuringPosition.X=value; }
        /// <summary>
        /// 左軸測刀量長的 Y 座標 
        /// </summary>
        [MVVM("Y", false, reflection: false)]
        public double LeftMeasuringPositionY { get => Left.MeasuringPosition.Y; set => Left.MeasuringPosition.Y=value; }
        /// <summary>
        /// 左軸測刀量長的 Z 座標 
        /// </summary>
        [MVVM("Z", false, reflection: false)]
        public double LeftMeasuringPositionZ { get => Left.MeasuringPosition.Z; set => Left.MeasuringPosition.Z=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("Z 軸行程總長", false, reflection: false)]
        public double LeftTotalLength { get => Left.TotalLength; set => Left.TotalLength=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("扭力停止點", false, "打點時，接觸到物件的扭力。", reflection: false)]
        public short LeftTorque { get => Left.Torque; set => Left.Torque=value; }
        /// <summary>
        /// 左軸 Y軸極限
        /// </summary>
        [MVVM("Y 軸極限", false, reflection: false)]
        public double LeftYAxisLimit { get => Left.YAxisLimit; set => Left.YAxisLimit=value; }
        /// <summary>
        /// 左軸主軸額定電流
        /// </summary>
        [MVVM("主軸額定電流", false, reflection: false)]
        public double LeftElectricalCurrent { get => Left.ElectricalCurrent; set => Left.ElectricalCurrent=value; }
        /// <summary>
        /// 左軸 Z 軸原點到活動端距離
        /// </summary>
        [MVVM("Z 軸原點到活動端距離", false, "Z 軸 0 點 到活動端和固定端接觸的距離，小於則有撞機可能")]
        public double LeftOriginToSideLength { get => Left.OriginToSideLength; set => Left.OriginToSideLength=value; }
        /// <summary>
        /// 左軸入口刀庫位置
        /// </summary>
        /// <summary>
        /// 左軸 Y軸極限
        /// </summary>
        [MVVM(false, reflection: false)]
        public Axis4D[] LeftEntranceDrillPosition { get; set; }
        /// <summary>
        /// 左軸出口刀庫位置
        /// </summary>
        [MVVM(false, reflection: false)]
        public Axis4D[] LeftExportDrillPosition { get; set; }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        [MVVM("X", false, reflection: false)]
        public double LeftEntranceDrillPosition1X { get => LeftEntranceDrillPosition[0].X; set => LeftEntranceDrillPosition[0].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftEntranceDrillPosition2X { get => LeftEntranceDrillPosition[1].X; set => LeftEntranceDrillPosition[1].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftEntranceDrillPosition3X { get => LeftEntranceDrillPosition[2].X; set => LeftEntranceDrillPosition[2].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftEntranceDrillPosition4X { get => LeftEntranceDrillPosition[3].X; set => LeftEntranceDrillPosition[3].X =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftEntranceDrillPosition1Y { get => LeftEntranceDrillPosition[0].Y; set => LeftEntranceDrillPosition[0].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftEntranceDrillPosition2Y { get => LeftEntranceDrillPosition[1].Y; set => LeftEntranceDrillPosition[1].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftEntranceDrillPosition3Y { get => LeftEntranceDrillPosition[2].Y; set => LeftEntranceDrillPosition[2].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftEntranceDrillPosition4Y { get => LeftEntranceDrillPosition[3].Y; set => LeftEntranceDrillPosition[3].Y =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftEntranceDrillPosition1Z { get => LeftEntranceDrillPosition[0].Z; set => LeftEntranceDrillPosition[0].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftEntranceDrillPosition2Z { get => LeftEntranceDrillPosition[1].Z; set => LeftEntranceDrillPosition[1].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftEntranceDrillPosition3Z { get => LeftEntranceDrillPosition[2].Z; set => LeftEntranceDrillPosition[2].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftEntranceDrillPosition4Z { get => LeftEntranceDrillPosition[3].Z; set => LeftEntranceDrillPosition[3].Z =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftExportDrillPosition1X { get => LeftExportDrillPosition[0].X; set => LeftExportDrillPosition[0].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftExportDrillPosition2X { get => LeftExportDrillPosition[1].X; set => LeftExportDrillPosition[1].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftExportDrillPosition3X { get => LeftExportDrillPosition[2].X; set => LeftExportDrillPosition[2].X =value; }
        [MVVM("X", false, reflection: false)]
        public double LeftExportDrillPosition4X { get => LeftExportDrillPosition[3].X; set => LeftExportDrillPosition[3].X =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftExportDrillPosition1Y { get => LeftExportDrillPosition[0].Y; set => LeftExportDrillPosition[0].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftExportDrillPosition2Y { get => LeftExportDrillPosition[1].Y; set => LeftExportDrillPosition[1].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftExportDrillPosition3Y { get => LeftExportDrillPosition[2].Y; set => LeftExportDrillPosition[2].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double LeftExportDrillPosition4Y { get => LeftExportDrillPosition[3].Y; set => LeftExportDrillPosition[3].Y =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftExportDrillPosition1Z { get => LeftExportDrillPosition[0].Z; set => LeftExportDrillPosition[0].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftExportDrillPosition2Z { get => LeftExportDrillPosition[1].Z; set => LeftExportDrillPosition[1].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftExportDrillPosition3Z { get => LeftExportDrillPosition[2].Z; set => LeftExportDrillPosition[2].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double LeftExportDrillPosition4Z { get => LeftExportDrillPosition[3].Z; set => LeftExportDrillPosition[3].Z =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftEntranceDrillPosition1MasterPhase { get => LeftEntranceDrillPosition[0].MasterPhase; set => LeftEntranceDrillPosition[0].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftEntranceDrillPosition2MasterPhase { get => LeftEntranceDrillPosition[1].MasterPhase; set => LeftEntranceDrillPosition[1].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftEntranceDrillPosition3MasterPhase { get => LeftEntranceDrillPosition[2].MasterPhase; set => LeftEntranceDrillPosition[2].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftEntranceDrillPosition4MasterPhase { get => LeftEntranceDrillPosition[3].MasterPhase; set => LeftEntranceDrillPosition[3].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftExportDrillPosition1MasterPhase { get => LeftExportDrillPosition[0].MasterPhase; set => LeftExportDrillPosition[0].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftExportDrillPosition2MasterPhase { get => LeftExportDrillPosition[1].MasterPhase; set => LeftExportDrillPosition[1].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftExportDrillPosition3MasterPhase { get => LeftExportDrillPosition[2].MasterPhase; set => LeftExportDrillPosition[2].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double LeftExportDrillPosition4MasterPhase { get => LeftExportDrillPosition[3].MasterPhase; set => LeftExportDrillPosition[3].MasterPhase =value; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員的 XML 註解
        #endregion

        #region 中軸
        /// <summary>
        /// 中軸主軸額定電流
        /// </summary>
        [MVVM("主軸額定電流", false, reflection: false)]
        public double MiddleElectricalCurrent { get => Middle.ElectricalCurrent; set => Middle.ElectricalCurrent=value; }
        /// <summary>
        /// 中軸設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public AxisSetting Middle { get; set; } = new AxisSetting();
        /// <summary>
        /// 左軸測刀量長的 X 座標  
        /// </summary>
        [MVVM("X 座標", false, reflection: false)]
        public double MiddleMeasuringPositionX { get => Middle.MeasuringPosition.X; set => Middle.MeasuringPosition.X=value; }
        /// <summary>
        /// 左軸測刀量長的 Y 座標 
        /// </summary>
        [MVVM("Y", false, reflection: false)]
        public double MiddleMeasuringPositionY { get => Middle.MeasuringPosition.Y; set => Middle.MeasuringPosition.Y=value; }
        /// <summary>
        /// 左軸測刀量長的 Z 座標 
        /// </summary>
        [MVVM("Z", false, reflection: false)]
        public double MiddleMeasuringPositionZ { get => Middle.MeasuringPosition.Z; set => Middle.MeasuringPosition.Z=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("Z 軸行程總長", false, reflection: false)]
        public double MiddleTotalLength { get => Middle.TotalLength; set => Middle.TotalLength=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("扭力停止點", false, "打點時，接觸到物件的扭力。", reflection: false)]
        public short MiddleTorque { get => Middle.Torque; set => Middle.Torque=value; }
        /// <summary>
        /// 左軸 Y軸極限
        /// </summary>
        [MVVM("Y 軸極限", false, reflection: false)]
        public double MiddleYAxisLimit { get => Middle.YAxisLimit; set => Middle.YAxisLimit=value; }
        /// <summary>
        /// 左軸 Z 軸原點到活動端距離
        /// </summary>
        [MVVM("Z 軸原點到活動端距離", false, "Z 軸 0 點 到活動端和固定端接觸的距離，小於則有撞機可能", reflection: false)]
        public double MiddleOriginToSideLength { get => Middle.OriginToSideLength; set => Middle.OriginToSideLength=value; }
        /// <summary>
        /// 左軸入口刀庫位置
        /// </summary>
        [MVVM(false, reflection: false)]
        public Axis4D[] MiddleDrillPosition { get; set; }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        [MVVM("X", false, reflection: false)]
        public double MiddleDrillPosition1X { get => MiddleDrillPosition[0].X; set => MiddleDrillPosition[0].X =value; }
        [MVVM("X", false, reflection: false)]
        public double MiddleDrillPosition2X { get => MiddleDrillPosition[1].X; set => MiddleDrillPosition[1].X =value; }
        [MVVM("X", false, reflection: false)]
        public double MiddleDrillPosition3X { get => MiddleDrillPosition[2].X; set => MiddleDrillPosition[2].X =value; }
        [MVVM("X", false, reflection: false)]
        public double MiddleDrillPosition4X { get => MiddleDrillPosition[3].X; set => MiddleDrillPosition[3].X =value; }
        [MVVM("X", false, reflection: false)]
        public double MiddleDrillPosition5X { get => MiddleDrillPosition[4].X; set => MiddleDrillPosition[4].X =value; }
        [MVVM("Y", false, reflection: false)]
        public double MiddleDrillPosition1Y { get => MiddleDrillPosition[0].Y; set => MiddleDrillPosition[0].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double MiddleDrillPosition2Y { get => MiddleDrillPosition[1].Y; set => MiddleDrillPosition[1].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double MiddleDrillPosition3Y { get => MiddleDrillPosition[2].Y; set => MiddleDrillPosition[2].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double MiddleDrillPosition4Y { get => MiddleDrillPosition[3].Y; set => MiddleDrillPosition[3].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double MiddleDrillPosition5Y { get => MiddleDrillPosition[4].Y; set => MiddleDrillPosition[4].Y =value; }
        [MVVM("Z", false, reflection: false)]
        public double MiddleDrillPosition1Z { get => MiddleDrillPosition[0].Z; set => MiddleDrillPosition[0].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double MiddleDrillPosition2Z { get => MiddleDrillPosition[1].Z; set => MiddleDrillPosition[1].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double MiddleDrillPosition3Z { get => MiddleDrillPosition[2].Z; set => MiddleDrillPosition[2].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double MiddleDrillPosition4Z { get => MiddleDrillPosition[3].Z; set => MiddleDrillPosition[3].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double MiddleDrillPosition5Z { get => MiddleDrillPosition[4].Z; set => MiddleDrillPosition[4].Z =value; }
        [MVVM("相位", false, reflection: false)]
        public double MiddleDrillPosition1MasterPhase { get => MiddleDrillPosition[0].MasterPhase; set => MiddleDrillPosition[0].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double MiddleDrillPosition2MasterPhase { get => MiddleDrillPosition[1].MasterPhase; set => MiddleDrillPosition[1].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double MiddleDrillPosition3MasterPhase { get => MiddleDrillPosition[2].MasterPhase; set => MiddleDrillPosition[2].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double MiddleDrillPosition4MasterPhase { get => MiddleDrillPosition[3].MasterPhase; set => MiddleDrillPosition[3].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double MiddleDrillPosition5MasterPhase { get => MiddleDrillPosition[4].MasterPhase; set => MiddleDrillPosition[4].MasterPhase =value; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員的 XML 註解
        #endregion

        #region 右軸
        /// <summary>
        /// 右軸設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public AxisSetting Right { get; set; } = new AxisSetting();
        /// <summary>
        /// 左軸測刀量長的 X 座標  
        /// </summary>
        [MVVM("X", false, reflection: false)]
        public double RightMeasuringPositionX { get => Right.MeasuringPosition.X; set => Right.MeasuringPosition.X=value; }
        /// <summary>
        /// 左軸測刀量長的 Y 座標 
        /// </summary>
        [MVVM("Y", false, reflection: false)]
        public double RightMeasuringPositionY { get => Right.MeasuringPosition.Y; set => Right.MeasuringPosition.Y=value; }
        /// <summary>
        /// 左軸測刀量長的 Z 座標 
        /// </summary>
        [MVVM("Z", false, reflection: false)]
        public double RightMeasuringPositionZ { get => Right.MeasuringPosition.Z; set => Right.MeasuringPosition.Z=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("Z 軸行程總長", false, reflection: false)]
        public double RightTotalLength { get => Right.TotalLength; set => Right.TotalLength=value; }
        /// <summary>
        /// 左軸 Z 軸行程總長
        /// </summary>
        [MVVM("扭力停止點", false, "打點時，接觸到物件的扭力。", reflection: false)]
        public short RightTorque { get => Right.Torque; set => Right.Torque=value; }
        /// <summary>
        /// 左軸 Y軸極限
        /// </summary>
        [MVVM("Y 軸極限", false, reflection: false)]
        public double RightYAxisLimit { get => Right.YAxisLimit; set => Right.YAxisLimit=value; }
        /// <summary>
        /// 左軸 Z 軸原點到活動端距離
        /// </summary>
        [MVVM("Z 軸原點到活動端距離", false, "Z 軸 0 點 到活動端和固定端接觸的距離，小於則有撞機可能", reflection: false)]
        public double RightOriginToSideLength { get => Right.OriginToSideLength; set => Right.OriginToSideLength=value; }
        /// <summary>
        /// 左軸入口刀庫位置
        /// </summary>
        [MVVM(false, reflection: false)]
        public Axis4D[] RightEntranceDrillPosition { get; set; }
        /// <summary>
        /// 左軸出口刀庫位置
        /// </summary>
        public Axis4D[] RightExportDrillPosition { get; set; }
        /// <summary>
        /// 右軸主軸額定電流
        /// </summary>
        [MVVM("主軸額定電流", false, reflection: false)]
        public double RightElectricalCurrent { get => Right.ElectricalCurrent; set => Right.ElectricalCurrent=value; }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        [MVVM("X", false, reflection: false)]
        public double RightEntranceDrillPosition1X { get => RightEntranceDrillPosition[0].X; set => RightEntranceDrillPosition[0].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightEntranceDrillPosition2X { get => RightEntranceDrillPosition[1].X; set => RightEntranceDrillPosition[1].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightEntranceDrillPosition3X { get => RightEntranceDrillPosition[2].X; set => RightEntranceDrillPosition[2].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightEntranceDrillPosition4X { get => RightEntranceDrillPosition[3].X; set => RightEntranceDrillPosition[3].X =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightEntranceDrillPosition1Y { get => RightEntranceDrillPosition[0].Y; set => RightEntranceDrillPosition[0].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightEntranceDrillPosition2Y { get => RightEntranceDrillPosition[1].Y; set => RightEntranceDrillPosition[1].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightEntranceDrillPosition3Y { get => RightEntranceDrillPosition[2].Y; set => RightEntranceDrillPosition[2].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightEntranceDrillPosition4Y { get => RightEntranceDrillPosition[3].Y; set => RightEntranceDrillPosition[3].Y =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightEntranceDrillPosition1Z { get => RightEntranceDrillPosition[0].Z; set => RightEntranceDrillPosition[0].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightEntranceDrillPosition2Z { get => RightEntranceDrillPosition[1].Z; set => RightEntranceDrillPosition[1].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightEntranceDrillPosition3Z { get => RightEntranceDrillPosition[2].Z; set => RightEntranceDrillPosition[2].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightEntranceDrillPosition4Z { get => RightEntranceDrillPosition[3].Z; set => RightEntranceDrillPosition[3].Z =value; }

        [MVVM("X", false, reflection: false)]
        public double RightExportDrillPosition1X { get => RightExportDrillPosition[0].X; set => RightExportDrillPosition[0].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightExportDrillPosition2X { get => RightExportDrillPosition[1].X; set => RightExportDrillPosition[1].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightExportDrillPosition3X { get => RightExportDrillPosition[2].X; set => RightExportDrillPosition[2].X =value; }
        [MVVM("X", false, reflection: false)]
        public double RightExportDrillPosition4X { get => RightExportDrillPosition[3].X; set => RightExportDrillPosition[3].X =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightExportDrillPosition1Y { get => RightExportDrillPosition[0].Y; set => RightExportDrillPosition[0].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightExportDrillPosition2Y { get => RightExportDrillPosition[1].Y; set => RightExportDrillPosition[1].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightExportDrillPosition3Y { get => RightExportDrillPosition[2].Y; set => RightExportDrillPosition[2].Y =value; }
        [MVVM("Y", false, reflection: false)]
        public double RightExportDrillPosition4Y { get => RightExportDrillPosition[3].Y; set => RightExportDrillPosition[3].Y =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightExportDrillPosition1Z { get => RightExportDrillPosition[0].Z; set => RightExportDrillPosition[0].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightExportDrillPosition2Z { get => RightExportDrillPosition[1].Z; set => RightExportDrillPosition[1].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightExportDrillPosition3Z { get => RightExportDrillPosition[2].Z; set => RightExportDrillPosition[2].Z =value; }
        [MVVM("Z", false, reflection: false)]
        public double RightExportDrillPosition4Z { get => RightExportDrillPosition[3].Z; set => RightExportDrillPosition[3].Z =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightEntranceDrillPosition1MasterPhase { get => RightEntranceDrillPosition[0].MasterPhase; set => RightEntranceDrillPosition[0].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightEntranceDrillPosition2MasterPhase { get => RightEntranceDrillPosition[1].MasterPhase; set => RightEntranceDrillPosition[1].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightEntranceDrillPosition3MasterPhase { get => RightEntranceDrillPosition[2].MasterPhase; set => RightEntranceDrillPosition[2].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightEntranceDrillPosition4MasterPhase { get => RightEntranceDrillPosition[3].MasterPhase; set => RightEntranceDrillPosition[3].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightExportDrillPosition1MasterPhase { get => RightExportDrillPosition[0].MasterPhase; set => RightExportDrillPosition[0].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightExportDrillPosition2MasterPhase { get => RightExportDrillPosition[1].MasterPhase; set => RightExportDrillPosition[1].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightExportDrillPosition3MasterPhase { get => RightExportDrillPosition[2].MasterPhase; set => RightExportDrillPosition[2].MasterPhase =value; }
        [MVVM("相位", false, reflection: false)]
        public double RightExportDrillPosition4MasterPhase { get => RightExportDrillPosition[3].MasterPhase; set => RightExportDrillPosition[3].MasterPhase =value; }
        #endregion

        #region 手臂
        /// <summary>
        /// 送料手臂設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public HandSetting Hand { get; set; } = new HandSetting();
        /// <summary>
        /// 手臂補正的值
        /// </summary>
        /// <remarks>
        /// 補正 <see cref="MaterialZeroToWorkRange"/> 的值
        /// <para>
        /// 客戶端調整。
        /// </para>
        /// </remarks>
        [MVVM("補正值", false, reflection: false, readOnly: false)]
        public double MaterialCorrectionWorkRange { get => Hand.MaterialCorrectionWorkRange; set => Hand.MaterialCorrectionWorkRange = value; }
        /// <summary>
        /// 送料原點到加工第一點(X=300)的長度,預設500。
        /// </summary>
        /// <remarks>
        /// 初次送料到加工點，工件原點到加工點的值。
        /// <para>
        /// 工程模式調整。
        /// </para>
        /// </remarks>
        [MVVM("送料原點到第一點", false, "初次送料到加工點，工件原點到加工點的值。", reflection: false)]
        public double MaterialZeroToWorkRange { get => Hand.MaterialZeroToWorkRange; set => Hand.MaterialZeroToWorkRange = value; }
        /// <summary>
        /// 手臂工作範圍
        /// </summary>
        /// <remarks>
        /// 手臂移動行程
        /// </remarks>
        [MVVM("手臂工作範圍", false, reflection: false)]
        public double HandJobLimit { get => Hand.HandJobLimit; set => Hand.HandJobLimit = value; }
        /// <summary>
        /// 出口下壓夾料到加工0點的長度,下壓零件與主軸刀徑有錯開。
        /// 意思是加工若可以做300,送料須加上此段長度才能讓出口處下壓固定處夾持
        /// </summary>
        [MVVM("出口下壓夾料到加工 0 點", false, "是加工若可以做300,送料須加上此段長度才能讓出口處下壓固定處夾持", reflection: false)]
        public double OutClampToZeroLength { get => Hand.OutClampToZeroLength; set => Hand.OutClampToZeroLength = value; }
        /// <summary>
        /// 手臂水平夾爪垂直軸的總行程
        /// </summary>
        [MVVM("水平夾爪垂直軸總行程", false, reflection: false)]
        public double InTotalLength { get => Hand.InTotalLength; set => Hand.InTotalLength = value; }
        /// <summary>
        /// 手臂垂直夾爪垂直軸的總行程
        /// </summary>
        [MVVM("手臂垂直夾爪垂直軸的總行程", false, reflection: false)]
        public double OutTotalLength { get => Hand.OutTotalLength; set => Hand.OutTotalLength = value; }
        /// <summary>
        /// 手臂長度 ( 預設值2300 )
        /// </summary>
        [MVVM("手臂長度", false, reflection: false)]
        public double Length { get => Hand.Length; set => Hand.Length = value; }
        /// <summary>
        /// 垂直夾取時,X軸的最大移動行程_需預設，垂直夾取中_確保水平夾爪不會撞機的行程位置
        /// </summary>
        [MVVM("垂直夾爪 X 軸的移動行程", false, "垂直夾取時，X軸的最大移動行程_需預設，垂直夾取中_確保水平夾爪不會撞機的行程位置", reflection: false)]
        public double VerticalGrippingXLimit { get => Hand.VerticalGrippingXLimit; set => Hand.VerticalGrippingXLimit = value; }
        /// <summary>
        /// 垂直夾爪走到極限X時,垂直夾爪可以夾取的點到送料原點的長度。( 預設 2000 )
        /// </summary>
        [MVVM("垂直夾爪 X 極限", false, "垂直夾爪走到極限X時,垂直夾爪可以夾取的點到送料原點的長度", reflection: false)]
        public double VerticalToOriginLength { get => Hand.VerticalToOriginLength; set => Hand.VerticalToOriginLength = value; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (翼板向)。( 預設150 )
        /// </summary>
        [MVVM("翼板夾爪頭到 Sensor 的長度", false, "夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (翼板向)", reflection: false)]
        public double ArmInDownWingSensorToForntLength { get => Hand.ArmInDownWingSensorToForntLength; set => Hand.ArmInDownWingSensorToForntLength = value; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (腹板向)。( 預設150 )
        /// </summary>
        [MVVM("腹板爪頭到 Sensor 的長度", false, "夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (腹板向)", reflection: false)]
        public double ArmInDownBellySensorToForntLength { get => Hand.ArmInDownBellySensorToForntLength; set => Hand.ArmInDownBellySensorToForntLength = value; }
        /// <summary>
        /// 水平夾的檢測位置座標
        /// 配合向下照的SENSOR,固定一個位置,並將Sensor檢測距離設定完成
        /// </summary>
        [MVVM("水平夾檢測位置座標", false, "配合向下照的SENSOR,固定一個位置,並將Sensor檢測距離設定完成", reflection: false)]
        public double ArmInDownCheckPosition { get => Hand.ArmInDownCheckPosition; set => Hand.ArmInDownCheckPosition = value; }
        /// <summary>
        /// 手臂移動極限範圍(到原點)，手臂原點到送料原點
        /// </summary>
        [MVVM("手臂移動極限範圍", false, "手臂原點到送料原點", reflection: false)]
        public double ArmXToOriginPoint { get => Hand.ArmXToOriginPoint; set => Hand.ArmXToOriginPoint = value; }
        /// <summary>
        /// 垂直夾爪最遠端與X原點交界的X軸長度位置__預設800
        /// </summary>
        [MVVM("垂直夾爪最遠端與原點交界", false, "垂直夾爪最遠端與X原點交界的X軸長度位置", reflection: false)]
        public double ArmInTouchOriginLength { get => Hand.ArmInTouchOriginLength; set => Hand.ArmInTouchOriginLength = value; }
        /// <summary>
        /// 手臂送料原點到出口上浮滾輪的長度
        /// </summary>
        [MVVM("原點到上浮滾輪距離", false, reflection: false)]
        public double ArmFeedOriginToOutUpRoll { get => Hand.ArmFeedOriginToOutUpRoll; set => Hand.ArmFeedOriginToOutUpRoll = value; }
        /// <summary>
        /// 翼板測距Sensor總行程長度
        /// </summary>
        [MVVM("翼板測距 Sensor 總行程長度", false, reflection: false)]
        public double WingSensorHigh { get => Hand.WingSensorHigh; set => Hand.WingSensorHigh = value; }
        /// <summary>
        /// 腹板測距Sensor總行程長度
        /// </summary>
        [MVVM("腹板測距 Sensor 總行程長度", false, reflection: false)]
        public double BellySensorHigh { get => Hand.BellySensorHigh; set => Hand.BellySensorHigh = value; }
        /// <summary>
        /// 減速點測距Sensor總行程長度(抓最長的)
        /// </summary>
        [MVVM("減速點測距Sensor總行程長度", false, reflection: false)]
        public double FeedSlowDownPointTotalLength { get => Hand.FeedSlowDownPointTotalLength; set => Hand.FeedSlowDownPointTotalLength = value; }
        /// <summary>
        /// 手臂送料減速點到原點距離
        /// </summary>
        [MVVM("手臂送料減速點到原點距離", false, reflection: false)]
        public double SlowToOriginLength { get => Hand.SlowToOriginLength; set => Hand.SlowToOriginLength = value; }
        /// <summary>
        /// 手臂最大行程(夾腹板可到X最極限位置)_目前10655
        /// </summary>
        [MVVM("手臂夾腹板 X 最極限位", false, reflection: false)]
        public double XLimit { get => Hand.XLimit; set => Hand.XLimit = value; }
        /// <summary>
        /// 夾腹板In_Z到底的安全長度_預設80到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        [MVVM("夾腹板In_Z到底的安全長度", false, reflection: false)]
        public double ArmInZSafeLength { get => Hand.ArmInZSafeLength; set => Hand.ArmInZSafeLength = value; }
        /// <summary>
        ///  夾翼板Out_Z到底的安全長度_預設60
        /// 到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        [MVVM("夾翼板Out_Z到底的安全長度", false, reflection: false)]
        public double ArmOutZSafeLength { get => Hand.ArmOutZSafeLength; set => Hand.ArmOutZSafeLength = value; }
        /// <summary>
        /// 減速原點的距離設定
        /// </summary>
        [MVVM("減速原點的距離設定", false, description: "配合實際 Sensor 設定的距離")]
        public double DeceleratingOrigin { get => Hand.DeceleratingOrigin; set => Hand.DeceleratingOrigin = value; }
        /// <inheritdoc/>
        [MVVM("水平夾爪補正值", false, description: "水平夾爪夾取時，夾爪夾取翼板的位置補正")]
        public double HorizontalCompensate { get => Hand.HorizontalCompensate; set => Hand.HorizontalCompensate = value; }
        #endregion

        #region 下壓夾具
        /// <summary>
        /// 下壓夾具設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public ClampDownSetting ClampDown { get; set; } = new ClampDownSetting();
        /// <inheritdoc/>
        [MVVM("入口左側", false, reflection: false)]
        public double ClampDownEntranceL { get => ClampDown.EntranceL; set => ClampDown.EntranceL = value; }
        /// <inheritdoc/>
        [MVVM("出口左側", false, reflection: false)]
        public double ClampDownExportL { get => ClampDown.ExportL; set => ClampDown.ExportL = value; }
        /// <inheritdoc/>
        [MVVM("入口右側", false, reflection: false)]
        public double ClampDownEntranceR { get => ClampDown.EntranceR; set => ClampDown.EntranceR = value; }
        /// <inheritdoc/>
        [MVVM("出口右側", false, reflection: false)]
        public double ClampDownExportR { get => ClampDown.ExportR; set => ClampDown.ExportR = value; }
        #endregion

        #region 側壓夾具
        /// <summary>
        /// 側壓夾具設定參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public SideClamp SideClamp { get; set; } = new SideClamp();
        /// <inheritdoc/>
        [MVVM("入口", false, reflection: false)]
        public double SideClampEntranceL { get => SideClamp.EntranceL; set => SideClamp.EntranceL = value; }
        /// <inheritdoc/>
        [MVVM("出口", false, reflection: false)]
        public double SideClampExportL { get => SideClamp.ExportL; set => SideClamp.ExportL = value; }
        #endregion

        #region 入口料架
        /// <summary>
        /// 入口料架參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public Traverse Entrance { get; set; } = new Traverse();
        /// <summary>
        /// 入口台車長度
        /// </summary>
        [MVVM("臺車長度", false, reflection: false)]
        public double EntranceCarLength { get => Entrance.CarLength; set => Entrance.CarLength = value; }
        /// <summary>
        /// 入口台車凸起區域厚度
        /// </summary>
        [MVVM("台車凸起區域厚度", false, reflection: false)]
        public double EntranceCarRaisedLength { get => Entrance.CarRaisedLength; set => Entrance.CarRaisedLength = value; }
        /// <summary>
        /// 入口台車原點到定位點
        /// </summary>
        [MVVM("台車原點到定位點", false, reflection: false)]
        public double EntranceOriginToLocationPoint { get => Entrance.OriginToLocationPoint; set => Entrance.OriginToLocationPoint = value; }
        /// <summary>
        /// 入口台車排序總空間
        /// </summary>
        [MVVM("台車排序總空間", false, reflection: false)]
        public double EntranceSortTotalLength { get => Entrance.SortTotalLength; set => Entrance.SortTotalLength  = value; }
        /// <summary>
        /// 入口台車極限總行程
        /// </summary>
        [MVVM("台車極限總行程", false, reflection: false)]
        public double EntranceLimit { get => Entrance.Limit; set => Entrance.Limit  = value; }
        ///// <summary>
        ///// 入口移動到料與料中間的尻料位置抓中心點的補正 (正轉)
        ///// </summary>
        //[MVVM("排序校正 (正轉)", false, "移動到料與料中間的尻料位置抓中心點的補正(正轉)", reflection: false)]
        //public double EntranceSortCorrection { get => Entrance.PositiveSortCorrection; set => Entrance.PositiveSortCorrection  = value; }
        ///// <summary>
        ///// 入口台車凸點在料與料的中心補正
        ///// </summary>
        //[MVVM("台車凸點在料與料的中心補正", false, reflection: false)]
        //public double EntranceToArmSideCorrection { get => Entrance.ToArmSideCorrection; set => Entrance.ToArmSideCorrection  = value; }
        /// <summary>
        /// 入口橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [MVVM("橫移料架電阻尺解析度", false, "橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse", reflection: false)]
        public double EntranceResolution { get => Entrance.Resolution; set => Entrance.Resolution   = value; }
        /// <summary>
        /// 入口安全間隙
        /// </summary>
        [MVVM("排序安全間隙", false, "物件與物件之間的安全間隙", reflection: false)]
        public double EntranceSafetyGap { get => Entrance.SafetyGap; set => Entrance.SafetyGap   = value; }
        /// <summary>
        /// 乘載上升的範圍
        /// </summary>
        [MVVM("乘載上升的範圍", false, description: "車邊緣跟素材切齊後要再多支撐面的長度")]
        public double EntranceRideRange { get => Entrance.RideRange; set => Entrance.RideRange   = value; }
        ///// <summary>
        ///// 排序校正 (逆轉)
        ///// </summary>
        //[MVVM("排序校正 (逆轉)", false, "移動到料與料中間的尻料位置抓中心點的補正(逆轉)", reflection: false)]
        //public double EntranceReverseSortCorrection { get => Entrance.ReverseSortCorrection; set => Entrance.ReverseSortCorrection   = value; }
        /// <summary>
        /// 出口速度 1 補正
        /// </summary>
        /// <remarks>
        /// 補正停止慣性
        /// </remarks>
        [MVVM("速度 1 補正", false, description: "補正停止慣性")]
        public double EntranceSpeed1Correct { get => Entrance.Speed1Correct; set => Entrance.Speed1Correct   = value; }
        /// <summary>
        /// 出口速度 2 補正
        /// </summary>
        [MVVM("速度 2 補正", false, description: "補正停止慣性")]
        public double EntranceSpeed2Correct { get => Entrance.Speed2Correct; set => Entrance.Speed2Correct   = value; }
        #endregion

        #region 出口料架
        /// <summary>
        /// 出口料架參數
        /// </summary>
        [MVVM("左軸設定參數", true)]
        public ShapeTraverse Export { get; set; } = new ShapeTraverse();
        /// <summary>
        /// 出口台車長度
        /// </summary>
        [MVVM("臺車長度", false, reflection: false)]
        public double ExportCarLength { get => Export.CarLength; set => Export.CarLength = value; }
        /// <summary>
        /// 出口台車凸起區域厚度
        /// </summary>
        [MVVM("台車凸起區域厚度", false, reflection: false)]
        public double ExportCarRaisedLength { get => Export.CarRaisedLength; set => Export.CarRaisedLength = value; }
        /// <summary>
        /// 出口台車原點到定位點
        /// </summary>
        [MVVM("台車原點到定位點", false, reflection: false)]
        public double ExportOriginToLocationPoint { get => Export.OriginToLocationPoint; set => Export.OriginToLocationPoint = value; }
        /// <summary>
        /// 出口台車排序總空間
        /// </summary>
        [MVVM("台車排序總空間", false, reflection: false)]
        public double ExportSortTotalLength { get => Export.SortTotalLength; set => Export.SortTotalLength  = value; }
        /// <summary>
        /// 出口台車極限總行程
        /// </summary>
        [MVVM("台車極限總行程", false, reflection: false)]
        public double ExportLimit { get => Export.Limit; set => Export.Limit  = value; }
        ///// <summary>
        ///// 出口移動到料與料中間的尻料位置抓中心點的補正
        ///// </summary>
        //[MVVM("排序校正", false, "移動到料與料中間的尻料位置抓中心點的補正", reflection: false)]
        //public double ExportSortCorrection { get => Export.PositiveSortCorrection; set => Export.PositiveSortCorrection  = value; }
        ///// <summary>
        ///// 出口台車凸點在料與料的中心補正
        ///// </summary>
        //[MVVM("台車凸點在料與料的中心補正", false, reflection: false)]
        //public double ExportToArmSideCorrection { get => Export.ToArmSideCorrection; set => Export.ToArmSideCorrection  = value; }
        /// <summary>
        /// 出口橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [MVVM("橫移料架電阻尺解析度", false, "橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse", reflection: false)]
        public double ExportResolution { get => Export.Resolution; set => Export.Resolution   = value; }
        /// <summary>
        /// 出口安全間隙
        /// </summary>
        [MVVM("排序安全間隙", false, "物件與物件之間的安全間隙", reflection: false)]
        public double ExportSafetyGap { get => Export.SafetyGap; set => Export.SafetyGap   = value; }
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。第 1 個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM("可排放物件的座標 1", false, reflection: false)]
        public double ExportConveyorPosition1 { get => Export.ConveyorPosition[0]; set => Export.ConveyorPosition[0]   = value; }
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。第 2 個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM("可排放物件的座標 2", false, reflection: false)]
        public double ExportConveyorPosition2 { get => Export.ConveyorPosition[1]; set => Export.ConveyorPosition[1]   = value; }
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。第 3 個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM("可排放物件的座標 3", false, reflection: false)]
        public double ExportConveyorPosition3 { get => Export.ConveyorPosition[2]; set => Export.ConveyorPosition[2]   = value; }
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。第 4 個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM("可排放物件的座標 4", false, reflection: false)]
        public double ExportConveyorPosition4 { get => Export.ConveyorPosition[3]; set => Export.ConveyorPosition[3]   = value; }
        /// <summary>
        /// 安全距離
        /// </summary>
        [MVVM("安全距離", false, reflection: false)]
        public double ExportSafety { get => Export.Safety; set => Export.Safety  = value; }
        [MVVM("乘載上升的範圍", false, description: "車邊緣跟素材切齊後要再多支撐面的長度")]
        public double ExportRideRange { get => Export.RideRange; set => Export.RideRange   = value; }
        ///// <summary>
        ///// 出口移動到料與料中間的尻料位置抓中心點的補正 (逆轉)
        ///// </summary>
        //[MVVM("排序校正 (逆轉)", false, "移動到料與料中間的尻料位置抓中心點的補正(逆轉)", reflection: false)]
        //public double ExportReverseSortCorrection { get => Export.ReverseSortCorrection; set => Export.ReverseSortCorrection   = value; }
        /// <summary>
        /// 出口速度 1 補正
        /// </summary>
        /// <remarks>
        /// 補正停止慣性
        /// </remarks>
        [MVVM("速度 1 補正", false, description: "補正停止慣性")]
        public double ExportSpeed1Correct { get => Export.Speed1Correct; set => Export.Speed1Correct = value; }
        /// <summary>
        /// 出口速度 2 補正
        /// </summary>
        [MVVM("速度 2 補正", false, description: "補正停止慣性")]
        public double ExportSpeed2Correct { get => Export.Speed2Correct; set => Export.Speed2Correct = value; }
        #endregion

        #region 鑽孔加工時不可執行安全間隙
        /// <summary>
        ///  RH 鑽孔加工安全間隙
        /// </summary>
        [MVVM("鑽孔加工時不可執行安全間隙", true)]
        public ProtectionDistance DrillProtection { get; set; } = new ProtectionDistance();
#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        [MVVM("X 向", false)]
        public double DrillProtectionX { get => DrillProtection.X; set => DrillProtection.X = value; }
        [MVVM("左右軸 Y 向", false)]
        public double DrillProtectionLRY { get => DrillProtection.LRY; set => DrillProtection.LRY = value; }
        [MVVM("中軸 Y 向", false)]
        public double DrillProtectionMY { get => DrillProtection.MY; set => DrillProtection.MY = value; }
        [MVVM("槽鐵與方管的 Y 軸保護", false)]
        public double DrillProtectionU_And_BOX_Y_Protection_Length { get => DrillProtection.U_And_BOX_Y_Protection_Length; set => DrillProtection.U_And_BOX_Y_Protection_Length = value; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員的 XML 註解
        #endregion

        #region 其他加工時不可執行安全間隙
        /// <summary>
        ///  其他加工時不可執行安全間隙
        /// </summary>
        [MVVM("其他加工時不可執行安全間隙", true)]
        public ProtectionDistance OtherProtection { get; set; } = new ProtectionDistance();
        [MVVM("X 向", false, reflection: false)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        public double OtherProtectionX { get => OtherProtection.X; set => OtherProtection.X = value; }
        [MVVM("左右軸 Y 向", false, reflection: false)]
        public double OtherProtectionLRY { get => OtherProtection.LRY; set => OtherProtection.LRY = value; }
        [MVVM("中軸 Y 向", false, reflection: false)]
        public double OtherProtectionMY { get => OtherProtection.MY; set => OtherProtection.MY = value; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員的 XML 註解

        #endregion

        /// <inheritdoc/>
        [MVVM("素材容許誤差", false, reflection: false)]
        public double MaterialAllowTolerance { get; set; }
        /// <inheritdoc/>
        [MVVM("刀長容許誤差", false, "用戶輸入的刀具長度與實際測量結果容許範圍", reflection: false)]
        public double AllowDrillTolerance { get; set; }
        /// <inheritdoc/>
        [MVVM("刀具安全距離", false, reflection: false)]
        public double SafeTouchLength { get; set; }
        /// <inheritdoc/>
        [MVVM("鑽破長度", false, reflection: false)]
        public double ThroughLength { get; set; }
        /// <inheritdoc/>
        [MVVM("第一段減速", false, "第一段減速結束的長度(接觸到便正常速度的長度)", reflection: false)]
        public double RankStratLength { get; set; }
        /// <inheritdoc/>
        [MVVM("出尾降速的長度", false, reflection: false)]
        public double RankEndLength { get; set; }
        /// <inheritdoc/>
        [MVVM("鑽孔接觸進給倍率", false, reflection: false)]
        public double TouchMUL { get; set; }
        /// <inheritdoc/>
        [MVVM("鑽孔出尾速度倍率", false, reflection: false)]
        public double EndMUL { get; set; }
        /// <inheritdoc/>
        [MVVM("文字大小", false, "刻字的文字大小", reflection: false)]
        public double StrWidth { get; set; }
        /// <inheritdoc/>
        [MVVM("中 Z 軸 0 點 到接觸下壓固定塊原點", false, "中Z位置 >= 下壓總行程 - 下壓現在位置 (抓最高) + 此設定距離  ,有可能撞機", reflection: false)]
        public double MZOriginToDownBlockHeight { get; set; }
        /// <inheritdoc/>
        [MVVM("上 Z 軸 0 點到固定端", false, reflection: false)]
        public double MZOriginToPillarHeight { get; set; }
        /// <inheritdoc/>
        [MVVM("上軸 Y 軸離開撞擊固定側柱子的距離", false, "上Y 若超過此距離 則Z軸不會撞擊 固定側的柱子 ,只要考慮下壓高度即可", reflection: false)]
        public double MYSafeLength { get; set; }
        /// <inheritdoc/>
        [MVVM("中軸與右軸兩個重力軸保護距離", false, "校機方式 固定R_Y在 +向極限位置, MZ在 0 ,量測中軸滑軌零件組(若下降)與右軸會碰撞的距離中軸Z軸 + (RY總行程 - RY位置 ) 若 >= 此保護 則限制行動", reflection: false)]
        public double MRZSafeLength { get; set; }
        /// <summary>
        /// 允許加工保護刀長
        /// </summary>
        [MVVM("允許加工保護刀長", false, "素材W若過低需要與刀具Check是否長度足夠", reflection: false)]
        public double ProtectDrillLength { get; set; }
        /// <summary>
        /// 刀具加工預備位置
        /// </summary>
        [MVVM("刀具加工預備位置", false,  reflection: false)]
        public double PrepWorkLength { get; set; }
        /// <summary>
        /// L與MY的碰撞保護，若LZ+MY>=此值則停止運動
        /// </summary>
        [MVVM("L與MY的碰撞保護", false, reflection: false)]
        public double LZMYSafeLength { get; set; }
        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
        }
    }
}
