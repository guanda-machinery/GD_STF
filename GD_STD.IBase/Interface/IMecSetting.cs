using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 機械設定介面
    /// </summary>
    public interface IMecSetting
    {
        /// <summary>
        /// 素材長度容許誤差
        /// </summary>
        double MaterialAllowTolerance { get; set; }
        /// <summary>
        /// 刀長設定與實際測量誤差/mm
        /// </summary>
        double AllowDrillTolerance { get; set; }
        /// <summary>
        /// 刀具安全距離
        /// </summary>
        /// <remarks> 
        /// 設定最大值 30mm，最小值 8mm。
        /// </remarks>
        double SafeTouchLength { get; set; }
        /// <summary>
        /// 鑽破厚的長度
        /// </summary>
        double ThroughLength { get; set; }
        /// <summary>
        /// 第一段減速結束的長度(接觸到便正常速度的長度)
        /// </summary>
        double RankStratLength { get; set; }
        /// <summary>
        /// 出尾降速的長度
        /// </summary>
        double RankEndLength { get; set; }
        /// <summary>
        /// 鑽孔接觸進給倍率
        /// </summary>
        double TouchMUL { get; set; }
        /// <summary>
        /// 鑽孔出尾速度倍率
        /// </summary>
        double EndMUL { get; set; }
        /// <summary>
        /// 文字寬度
        /// </summary>
        double StrWidth { get; set; }
        /// <summary>
        ///中Z軸 0點 到接觸下壓固定塊原點(抓最高的) 的距離
        ///中Z位置 >= 下壓總行程 - 下壓現在位置 (抓最高) + 此設定距離  ,有可能撞機
        /// </summary>
        double MZOriginToDownBlockHeight { get; set; }
        /// <summary>
        /// 上Z軸 0點 到 固定端 柱子接觸的距離_700
        /// </summary>
        double MZOriginToPillarHeight { get; set; }
        /// <summary>
        ///上軸Y軸 離開撞擊 固定側柱子的距離
        ///上Y 若超過此距離 則Z軸不會撞擊 固定側的柱子 ,只要考慮下壓高度即可
        /// </summary>
        double MYSafeLength { get; set; }
        /// <summary>
        /// 中軸與右軸兩個重力軸保護距離,_505
        ///校機方式 固定R_Y在 +向極限位置, MZ在 0 ,量測中軸滑軌零件組(若下降)與右軸會碰撞的距離
        ///中軸Z軸 + (RY總行程 - RY位置 ) 若 >= 此保護 則限制行動
        /// </summary>
        double MRZSafeLength { get; set; }
    }
}
