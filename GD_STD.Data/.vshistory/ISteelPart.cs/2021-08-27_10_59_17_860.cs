using GD_STD.Enum;
using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface ISteelPart
    {
        string DrawingName { get; set; }
        float H { get; set; }
        List<int> ID { get; set; }
        bool IsTekla { get; }
        MATERIAL Material { get; set; }
        string Number { get; set; }
        string Profile { get; set; }
        float t1 { get; set; }
        float t2 { get; set; }
        float W { get; set; }
    }
}