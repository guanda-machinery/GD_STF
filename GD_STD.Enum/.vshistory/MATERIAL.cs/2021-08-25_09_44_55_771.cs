using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 鋼構材質
    /// </summary>
    public enum MATERIAL : Int16
    {
        [Description("SN400YB")]
        SN400YB,
        [Description("SN490B")]
        SN490B,
        [Description("A572-GR.50")]
        A572GR50,
        [Description("A572")]
        A572,
        [Description("SN400B")]
        SN400B,
        [Description("SS400")]
        SS400,
        [Description("SM400A")]
        SM400A,
        [Description("A36")]
        A36,
        [Description("SN400A")]
        SN400A,
        [Description("SM400B")]
        SM400B,
        [Description("A992")]
        A992,
        [Description("SN490BD")]
        SN490BD,
        [Description("SN490YB")]
        SN490YB,
        [Description("SM490B")]
        SM490B,
        [Description("SM490YB")]
        SM490YB,
        [Description("SN490A")]
        SN490A,
        [Description("A709")]
        A709,
        [Description("G3101")]
        G3101,
        [Description("G3106")]
        G3106,
        [Description("G3136")]
        G3136,
    }
}
