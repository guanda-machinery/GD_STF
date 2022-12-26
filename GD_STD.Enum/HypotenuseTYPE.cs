using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 斜邊類型(使用於BlockName)
    /// </summary>
    
    public enum HypotenuseTYPE
    {
        //
        /// <summary>
        /// 切割線
        /// </summary>
        [Description("Manual Hypotenuse")]
        HypotenuseMan,
        /// <summary>
        /// 自動打點(依據NC檔之輪廓)
        /// </summary>
        [Description("Auto Hypotenuse")]
        HypotenuseAuto,
        /// <summary>
        /// 使用者自定義斜邊打點
        /// </summary>
        [Description("Customer Hypotenuse")]
        HypotenuseCustomer,       
    }
}
