using System;

namespace GD_STD.Enum
{
    //API文件: 新增enum
    /// <summary>
    /// 推播通知
    /// </summary>
    [Flags]
    public enum PUSH : ushort
    {
        /// <summary>
        /// 沒有任何通知
        /// </summary>
        NULL = 0b_0000_0000,
        /// <summary>
        ///  通知用戶去入口料架補上材料
        /// </summary>
        PLACE_MATERIAL = 0b_0000_0001,
        /// <summary>
        /// 通知用戶去出口料架移動加工完成料件
        /// </summary>
        EXPORT_BUSY = 0b_0000_0010,
    }
}