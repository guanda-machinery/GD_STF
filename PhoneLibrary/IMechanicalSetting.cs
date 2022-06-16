using GD_STD.Base;

namespace GD_STD.Phone
{
    public interface IMechanicalSetting
    {
        double AllowDrillTolerance { get; set; }
        ClampDownSetting ClampDown { get; set; }
        ProtectionDistance DrillProtection { get; set; }
        double EndMUL { get; set; }
        Traverse Entrance { get; set; }
        ShapeTraverse Export { get; set; }
        HandSetting Hand { get; set; }
        AxisSetting Left { get; set; }
        double MaterialAllowTolerance { get; set; }
        AxisSetting Middle { get; set; }
        double MRZSafeLength { get; set; }
        double MYSafeLength { get; set; }
        double MZOriginToDownBlockHeight { get; set; }
        double MZOriginToPillarHeight { get; set; }
        ProtectionDistance OtherProtection { get; set; }
        double RankEndLength { get; set; }
        double RankStratLength { get; set; }
        AxisSetting Right { get; set; }
        double SafeTouchLength { get; set; }
        SideClamp SideClamp { get; set; }
        double StrWidth { get; set; }
        double ThroughLength { get; set; }
        double TouchMUL { get; set; }
    }
}