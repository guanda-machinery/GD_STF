using GD_STD.Enum;
using System;
using System.Collections.Generic;

namespace GD_STD.Data
{
    /// <summary>
    /// 排版設定零件視圖介面
    /// </summary>
    public interface ITypeSettingPartView
    {
        /// <summary>
        /// 建立日期
        /// </summary>
        DateTime Creation { get; }
        /// <summary>
        /// 零件 ID
        /// </summary>
        List<int> ID { get; set; }
        /// <summary>
        /// 零件長度
        /// </summary>
        double Length { get; set; }
        /// <summary>
        /// 物件上鎖
        /// </summary>
        bool Lock { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        string Material { get; set; }
        /// <summary>
        /// 編號
        /// </summary>
        string Number { get; set; }
        /// <summary>
        /// 斷面規格
        /// </summary>
        string Profile { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        DateTime Revise { get; set; }
        /// <summary>
        /// 圖紙狀態
        /// </summary>
        DRAWING_STATE State { get; set; }
    }
}