using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 鋼構材質
    /// </summary>
    public enum MATERIAL : Int16
    {
        [Description("A36")]
        A36,
        [Description("SN400A")]
        SN400A,
        [Description("SN400B")]
        SN400B,
        [Description("SN400YB")]
        SN400YB,
        [Description("SN490B")]
        SN490B,
        [Description("SN490C")]
        SN490C,
        [Description("SM400A")]
        SM400A,
        [Description("SM400B")]
        SM400B,
        [Description("SM490A")]
        SM490A,
        [Description("SM490B")]
        SM490B,
        [Description("SS400")]
        SS400,
        [Description("A572-GR.50")]
        A572GR50
    }
}
