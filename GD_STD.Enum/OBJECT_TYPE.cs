using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 主零件類型
    /// </summary>
    
    public enum OBJECT_TYPE
    {
        //20220801 張燕華 若要移動集合列表順序，則inp檔必需重新建立，因為修改的東西會記錄在inp檔中
        //20220805 張燕華 有使用的斷面規格要排列在最上面，因為斷面規格下拉選項(ComboBox)才能對應到正確位置
        //20220830 呂宗霖 若有異動名稱或列舉名稱也要重跑
        /// <summary>
        /// TUBE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        [Description("TUBE")]
        TUBE,
        /// <summary>
        /// 方型管
        /// </summary>
        [Description("BOX")]
        BOX,
        /// <summary>
        /// BH型鋼
        /// </summary>
        [Description("BH")]
        BH,
        /// <summary>
        /// H 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        [Description("H")]
        H,
        /// <summary>
        /// RH型鋼
        /// </summary>
        [Description("RH")]
        RH,
        /// <summary>
        /// [ --> (longitudinal beam)LB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        [Description("[")]
        LB,
        /// <summary>
        /// 槽鐵, U or CH
        /// </summary>
        [Description("CH")]
        CH,
        /// <summary>
        /// 未知斷面
        /// </summary>
        //[Description("未知斷面")]
        Unknown,
        /// <summary>
        /// I 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("I型鋼")]
        I,
        /// <summary>
        /// L鐵
        /// </summary>
        //[Description("角鐵")]
        L,
        /// <summary>
        /// BT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("BT")]
        BT,
        /// <summary>
        /// CT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("CT")]
        CT,
        /// <summary>
        /// T 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("T")]
        T,
        /// <summary>
        /// C型鋼
        /// </summary>
        //[Description("C型鋼")]
        C,
        /// <summary>
        /// 圓管
        /// </summary>
        //[Description("圓管")]
        PIPE,
        /// <summary>
        /// TURN_BUCKLE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("TURN BUCKLE")]
        TURN_BUCKLE,
        /// <summary>
        /// WELD 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("WELD")]
        WELD,
        /// <summary>
        /// SA 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("SA")]
        SA,
        /// <summary>
        /// 格柵板踏階GRATING 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("格柵板踏階")]
        GRATING,
        /// <summary>
        /// 扁鐵
        /// </summary>
        //[Description("扁鐵")]
        FB,
        /// <summary>
        /// 圓棒
        /// </summary>
        //[Description("圓棒")]
        RB,
        /// <summary>
        /// HNUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("重型螺帽")]
        HNUT,
        /// <summary>
        /// NUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        //[Description("螺帽")]
        NUT,
        /// <summary>
        /// 多邊形鈑
        /// </summary>
        PLATE,
    }
}
