using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    //API文件:
    /// <summary>
    /// 機台狀態代碼
    /// </summary>
    [Flags]
    public enum STATUS_CODE : ulong
    {
        [ErrorCodeAttribute("NULL", null)]
        NULL = 0,
        /// <summary>
        /// 入口橫移料架無呼叫需求_1
        /// </summary>
        [ErrorCodeAttribute("In_Traverse_NO_NEED", null)]
        In_Traverse_NO_NEED = 1,
        /// <summary>
        /// 入口橫移素材尺寸超過台車_2
        /// </summary>
        [ErrorCodeAttribute("入口橫移素材尺寸超過台車_2", null)]
        In_Traverse_Size_Over_Trolley = 2,
        /// <summary>
        /// 入口橫移空間不足_4
        /// </summary>
        [ErrorCodeAttribute("入口橫移空間不足_4", null)]
        In_Traverse_Not_Enough_Space = 4,
        /// <summary>
        /// 台車上素材H 尺寸不符_8
        /// </summary>
        [ErrorCodeAttribute("台車上素材H 尺寸不符_8", null)]
        In_Traverse_H_Size_NG = 8,
    }
}
