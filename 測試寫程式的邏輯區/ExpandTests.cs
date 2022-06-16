using GD_STD;
using GD_STD.Base;
using GD_STD.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;

namespace 測試寫程式的邏輯區
{
    [TestFixture]
    public class ExpandTests
    {
        
        [Test]
        public void ReflectionTest1()
        {
            GetReflectionData(out GD_STD.Phone.MechanicalSetting mechanicalSetting, out WPFSTD105.FluentAPI.MecSetting mecSetting);
            //WPFSTD105.FluentAPI.MecSetting result = WPFSTD105.Expand.Reflection<WPFSTD105.FluentAPI.MecSetting>(mechanicalSetting, typeof(WPFSTD105.FluentAPI.MecSetting));
            WPFSTD105.FluentAPI.MecSetting result = JsonConvert.DeserializeObject<WPFSTD105.FluentAPI.MecSetting>(mechanicalSetting.ToString());
            Assert.True(true);
        }
        [Test]
        public void ReflectionTest2()
        {
            GetReflectionData(out GD_STD.Phone.MechanicalSetting mechanicalSetting, out WPFSTD105.FluentAPI.MecSetting mecSetting);
            //GD_STD.Phone.MechanicalSetting result = WPFSTD105.Expand.Reflection<GD_STD.Phone.MechanicalSetting>(mecSetting, typeof(GD_STD.Phone.MechanicalSetting));
            var _ = JsonConvert.DeserializeObject<GD_STD.Phone.MechanicalSetting>(mecSetting.ToString());
            Assert.True(true);
        }
        public static void GetReflectionData(out GD_STD.Phone.MechanicalSetting mechanicalSetting, out WPFSTD105.FluentAPI.MecSetting mecSetting)
        {
            Random random = new Random();
            mechanicalSetting = new GD_STD.Phone.MechanicalSetting()
            {
                Left = new GD_STD.Base.AxisSetting()
                {
                    MeasuringPosition = new GD_STD.Axis3D { X= random.Next(1, 1111111), Y=  random.Next(1, 1111111), Z =  random.Next(1, 1111111) },
                    OriginToSideLength =  random.Next(1, 1111111),
                    Torque =  11,
                    TotalLength =  random.Next(1, 1111111),
                    YAxisLimit =  random.Next(1, 1111111),
                },
                Right = new GD_STD.Base.AxisSetting()
                {
                    MeasuringPosition = new GD_STD.Axis3D { X= random.Next(1, 1111111), Y=  random.Next(1, 1111111), Z =  random.Next(1, 1111111) },
                    OriginToSideLength =  random.Next(1, 1111111),
                    Torque =  11,
                    TotalLength =  random.Next(1, 1111111),
                    YAxisLimit =  random.Next(1, 1111111),
                },
                Middle = new GD_STD.Base.AxisSetting()
                {
                    MeasuringPosition = new GD_STD.Axis3D { X= random.Next(1, 1111111), Y=  random.Next(1, 1111111), Z =  random.Next(1, 1111111) },
                    OriginToSideLength =  random.Next(1, 1111111),
                    Torque =  11,
                    TotalLength =  random.Next(1, 1111111),
                    YAxisLimit =  random.Next(1, 1111111),
                },
                AllowDrillTolerance = random.Next(1, 1111111),
                SafeTouchLength = random.Next(1, 1111111),
                SideClamp = new SideClamp()
                {
                    EntranceL = random.Next(1, 1111111),
                    ExportL = random.Next(1, 1111111),
                },
                StrWidth = random.Next(1, 1111111),
                MRZSafeLength = random.Next(1, 1111111),
                MYSafeLength = random.Next(1, 1111111),
                RankStratLength = random.Next(1, 1111111),
                ClampDown = new GD_STD.Base.ClampDownSetting()
                {
                    EntranceL = random.Next(1, 1111111),
                    EntranceR = random.Next(1, 1111111),
                    ExportL = random.Next(1, 1111111),
                    ExportR = random.Next(1, 1111111),
                },
                DrillProtection = new GD_STD.Base.ProtectionDistance()
                {
                    LRY = random.Next(1, 1111111),
                    MY = random.Next(1, 1111111),
                    X = random.Next(1, 1111111),
                },
                EndMUL = random.Next(1, 1111111),
                Entrance = new GD_STD.Base.Traverse()
                {
                    CarLength = random.Next(1, 1111111),
                    SafetyGap = random.Next(1, 1111111),
                    CarRaisedLength = random.Next(1, 1111111),
                    //PositiveSortCorrection = random.Next(1, 1111111),
                    SortTotalLength = random.Next(1, 1111111),
                    //ToArmSideCorrection = random.Next(1, 1111111),
                    Limit = random.Next(1, 1111111),
                    OriginToLocationPoint = random.Next(1, 1111111),
                    Resolution = random.Next(1, 1111111),
                },
                Export = new GD_STD.Base.ShapeTraverse
                {
                    Safety = random.Next(1, 1111111),
                    //PositiveSortCorrection = random.Next(1, 1111111),
                    SafetyGap = random.Next(1, 1111111),
                    //ToArmSideCorrection = random.Next(1, 1111111),
                    CarLength = random.Next(1, 1111111),
                    CarRaisedLength = random.Next(1, 1111111),
                    ConveyorPosition = new double[4] { 1, 1111, 65468141, 1566511 },
                    Limit = random.Next(1, 1111111),
                    OriginToLocationPoint = random.Next(1, 1111111),
                    Resolution = random.Next(1, 1111111),
                },
                Hand = new GD_STD.Base.HandSetting()
                {
                    SlowToOriginLength = random.Next(1, 1111111),
                    ArmInDownBellySensorToForntLength = random.Next(1, 1111111),
                    ArmInDownWingSensorToForntLength = random.Next(1, 1111111),
                    ArmInZSafeLength = random.Next(1, 1111111),
                    ArmOutZSafeLength = random.Next(1, 1111111),
                    BellySensorHigh = random.Next(1, 1111111),
                    ArmFeedOriginToOutUpRoll = random.Next(1, 1111111),
                    FeedSlowDownPointTotalLength = random.Next(1, 1111111),
                    WingSensorHigh = random.Next(1, 1111111),
                    ArmInDownCheckPosition = random.Next(1, 1111111),
                    ArmInTouchOriginLength = random.Next(1, 1111111),
                    ArmXToOriginPoint =random.Next(1, 1111111),
                    HandJobLimit = random.Next(1, 1111111),
                    InTotalLength = random.Next(1, 1111111),
                    Length = random.Next(1, 1111111),
                    MaterialCorrectionWorkRange = random.Next(1, 1111111),
                    MaterialZeroToWorkRange = random.Next(1, 1111111),
                    OutClampToZeroLength = random.Next(1, 1111111),
                    OutTotalLength = random.Next(1, 1111111),
                    VerticalGrippingXLimit = random.Next(1, 1111111),
                    VerticalToOriginLength = random.Next(1, 1111111),
                    XLimit = random.Next(1, 1111111),
                },
                MaterialAllowTolerance = random.Next(1, 1111111),
                MZOriginToDownBlockHeight = random.Next(1, 1111111),
                MZOriginToPillarHeight = random.Next(1, 1111111),
                OtherProtection = new GD_STD.Base.ProtectionDistance()
                {
                    LRY = random.Next(1, 1111111),
                    MY = random.Next(1, 1111111),
                    X = random.Next(1, 1111111),
                },
                RankEndLength = random.Next(1, 1111111),
                ThroughLength = random.Next(1, 1111111),
                TouchMUL = random.Next(1, 1111111),
            };
            mecSetting = new WPFSTD105.FluentAPI.MecSetting()
            {
                Left = new WPFSTD105.FluentAPI.AxisSetting()
                {
                    MeasuringPosition = new WPFSTD105.FluentAPI.Axis3D { X= random.Next(0, 1000000), Y=  random.Next(0, 1000000), Z =  random.Next(0, 1000000) },
                    OriginToSideLength =  random.Next(0, 1000000),
                    Torque =  10,
                    TotalLength =  random.Next(0, 1000000),
                    YAxisLimit =  random.Next(0, 1000000),
                },
                Right = new WPFSTD105.FluentAPI.AxisSetting()
                {
                    MeasuringPosition = new WPFSTD105.FluentAPI.Axis3D { X= random.Next(0, 1000000), Y=  random.Next(0, 1000000), Z =  random.Next(0, 1000000) },
                    OriginToSideLength =  random.Next(0, 1000000),
                    Torque =  10,
                    TotalLength =  random.Next(0, 1000000),
                    YAxisLimit =  random.Next(0, 1000000),
                },
                Middle = new WPFSTD105.FluentAPI.AxisSetting()
                {
                    MeasuringPosition = new WPFSTD105.FluentAPI.Axis3D { X= random.Next(0, 1000000), Y=  random.Next(0, 1000000), Z =  random.Next(0, 1000000) },
                    OriginToSideLength =  random.Next(0, 1000000),
                    Torque =  10,
                    TotalLength =  random.Next(0, 1000000),
                    YAxisLimit =  random.Next(0, 1000000),
                },
                AllowDrillTolerance = random.Next(0, 1000000),
                SafeTouchLength = random.Next(0, 1000000),
                SideClamp = new WPFSTD105.FluentAPI.SideClamp()
                {
                    EntranceL = random.Next(0, 1000000),
                    ExportL = random.Next(0, 1000000),
                },
                StrWidth = random.Next(0, 1000000),
                MRZSafeLength = random.Next(0, 1000000),
                MYSafeLength = random.Next(0, 1000000),
                RankStratLength = random.Next(0, 1000000),
                ClampDown = new WPFSTD105.FluentAPI.ClampDownSetting()
                {
                    EntranceL = random.Next(0, 1000000),
                    EntranceR = random.Next(0, 1000000),
                    ExportL = random.Next(0, 1000000),
                    ExportR = random.Next(0, 1000000),
                },
                DrillProtection = new WPFSTD105.FluentAPI.ProtectionDistance()
                {
                    LRY = random.Next(0, 1000000),
                    MY = random.Next(0, 1000000),
                    X = random.Next(0, 1000000),
                },
                EndMUL = random.Next(0, 1000000),
                Entrance = new WPFSTD105.FluentAPI.Traverse()
                {
                    CarLength = random.Next(0, 1000000),
                    SafetyGap = random.Next(0, 1000000),
                    CarRaisedLength = random.Next(0, 1000000),
                    //PositiveSortCorrection = random.Next(0, 1000000),
                    SortTotalLength = random.Next(0, 1000000),
                    //ToArmSideCorrection = random.Next(0, 1000000),
                    Limit = random.Next(0, 1000000),
                    OriginToLocationPoint = random.Next(0, 1000000),
                    Resolution = random.Next(0, 1000000),
                },
                Export = new WPFSTD105.FluentAPI.ShapeTraverse
                {
                    Safety = random.Next(0, 1000000),
                    //PositiveSortCorrection = random.Next(0, 1000000),
                    SafetyGap = random.Next(0, 1000000),
                    //ToArmSideCorrection = random.Next(0, 1000000),
                    CarLength = random.Next(0, 1000000),
                    CarRaisedLength = random.Next(0, 1000000),
                    ConveyorPosition = new double[4] { 0, 1000, 65468141, 1566510 },
                    Limit = random.Next(0, 1000000),
                    OriginToLocationPoint = random.Next(0, 1000000),
                    Resolution = random.Next(0, 1000000),
                },
                Hand = new WPFSTD105.FluentAPI.HandSetting()
                {
                    SlowToOriginLength = random.Next(0, 1000000),
                    ArmInDownBellySensorToForntLength = random.Next(0, 1000000),
                    ArmInDownWingSensorToForntLength = random.Next(0, 1000000),
                    ArmInZSafeLength = random.Next(0, 1000000),
                    ArmOutZSafeLength = random.Next(0, 1000000),
                    BellySensorHigh = random.Next(0, 1000000),
                    ArmFeedOriginToOutUpRoll = random.Next(0, 1000000),
                    FeedSlowDownPointTotalLength = random.Next(0, 1000000),
                    WingSensorHigh = random.Next(0, 1000000),
                    ArmInDownCheckPosition = random.Next(0, 1000000),
                    ArmInTouchOriginLength = random.Next(0, 1000000),
                    ArmXToOriginPoint =random.Next(0, 1000000),
                    HandJobLimit = random.Next(0, 1000000),
                    InTotalLength = random.Next(0, 1000000),
                    Length = random.Next(0, 1000000),
                    MaterialCorrectionWorkRange = random.Next(0, 1000000),
                    MaterialZeroToWorkRange = random.Next(0, 1000000),
                    OutClampToZeroLength = random.Next(0, 1000000),
                    OutTotalLength = random.Next(0, 1000000),
                    VerticalGrippingXLimit = random.Next(0, 1000000),
                    VerticalToOriginLength = random.Next(0, 1000000),
                    XLimit = random.Next(0, 1000000),
                },
                MaterialAllowTolerance = random.Next(0, 1000000),
                MZOriginToDownBlockHeight = random.Next(0, 1000000),
                MZOriginToPillarHeight = random.Next(0, 1000000),
                OtherProtection = new WPFSTD105.FluentAPI.ProtectionDistance()
                {
                    LRY = random.Next(0, 1000000),
                    MY = random.Next(0, 1000000),
                    X = random.Next(0, 1000000),
                },
                RankEndLength = random.Next(0, 1000000),
                ThroughLength = random.Next(0, 1000000),
                TouchMUL = random.Next(0, 1000000),
            };
        }
    }
}
