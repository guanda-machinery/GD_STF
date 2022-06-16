using GD_STD.Enum;
using System;

namespace GD_STD.Data
{
    public interface IDrawing
    {
        DateTime Creation { get; }
        DateTime Revise { get; set; }
        DRAWING_STATE State { get; set; }
    }
}