using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.IBase
{
    /// <summary>
    /// 單支鋼材的相關資訊
    /// </summary>
    public interface IPart : IProfile
    {
        /// <summary>
        /// 長度
        /// </summary>
        double Length { get; set; }
        /// <summary>
        /// 物件材質
        /// </summary>
        MATERIAL Material { get; set; }
    }
}