using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface IMatchSetting
    {
        /// <summary>
        /// 素材起始切除長度
        /// </summary>
        double StartCut { get; set; }
        /// <summary>
        /// 素材尾部切除長度
        /// </summary>
        double StartEnd { get; set; }
        /// <summary>
        /// 切割損耗
        /// </summary>
        double Cut { get; set; }
    }
}