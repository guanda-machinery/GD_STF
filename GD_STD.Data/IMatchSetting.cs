using System.Collections.Generic;

namespace GD_STD.Data
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IMatchSetting' 的 XML 註解
    public interface IMatchSetting
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IMatchSetting' 的 XML 註解
    {
        /// <summary>
        /// 素材起始切除長度
        /// </summary>
        double StartCut { get; }
        /// <summary>
        /// 素材尾部切除長度
        /// </summary>
        double EndCut { get; }
        /// <summary>
        /// 切割損耗
        /// </summary>
        double Cut { get; }
    }
}