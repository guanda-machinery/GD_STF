using GD_STD.Enum;
using System;
using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface ITypeSettingPartView
    {
        int Count { get; }
        DateTime Creation { get; }
        string DrawingName { get; set; }
        List<int> Father { get; set; }
        float H { get; set; }
        List<int> ID { get; set; }
        double Length { get; set; }
        bool Lock { get; set; }
        string Material { get; set; }
        string Number { get; set; }
        string Profile { get; set; }
        DateTime Revise { get; set; }
        int SortCount { get; set; }
        DRAWING_STATE State { get; set; }

        bool Equals(object obj);
    }
}