using GD_STD.Enum;
using System.Collections.Generic;

namespace GD_STD.Data
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart' 的 XML 註解
    public interface ISteelPart
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart' 的 XML 註解
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.DrawingName' 的 XML 註解
        string DrawingName { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.DrawingName' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.H' 的 XML 註解
        float H { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.H' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.ID' 的 XML 註解
        List<int> ID { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.ID' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.IsTekla' 的 XML 註解
        bool IsTekla { get; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.IsTekla' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Material' 的 XML 註解
        string Material { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Material' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Number' 的 XML 註解
        string Number { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Number' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Profile' 的 XML 註解
        string Profile { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.Profile' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.t1' 的 XML 註解
        float t1 { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.t1' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.t2' 的 XML 註解
        float t2 { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.t2' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ISteelPart.W' 的 XML 註解
        float W { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ISteelPart.W' 的 XML 註解
    }
}