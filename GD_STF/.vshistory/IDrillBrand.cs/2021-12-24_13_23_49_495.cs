using GD_STD.Enum;

namespace GD_STD
{
    public interface IDrillBrand
    {
        string DataName { get; set; }
        double Dia { get; set; }
        float DrillLength { get; set; }
        DRILL_TYPE DrillType { get; set; }
        float FeedQuantity { get; set; }
        float HolderLength { get; set; }
        string Name { get; set; }
        double Rpm { get; set; }
        double Vc { get; set; }
    }
}