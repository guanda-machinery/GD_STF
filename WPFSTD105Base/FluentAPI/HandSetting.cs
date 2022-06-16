using GD_STD.Attribute;
using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 手臂設定
    /// </summary>
    [Serializable]
    public class HandSetting : GD_STD.Base.IHandSetting
    {
        /// <summary>
        /// 補正的值
        /// </summary>
        /// <remarks>
        /// 補正 <see cref="MaterialZeroToWorkRange"/> 的值
        /// <para>
        /// 客戶端調整。
        /// </para>
        /// </remarks>
        [MVVM("補正值", false)]
        public double MaterialCorrectionWorkRange { get; set; }
        /// <summary>
        /// 送料原點到加工第一點(X=300)的長度,預設500。
        /// </summary>
        /// <remarks>
        /// 初次送料到加工點，工件原點到加工點的值。
        /// <para>
        /// 工程模式調整。
        /// </para>
        /// </remarks>
        [MVVM("送料原點到加工第一點", false)]
        public double MaterialZeroToWorkRange { get; set; }
        /// <summary>
        /// 手臂工作範圍
        /// </summary>
        /// <remarks>
        /// 手臂移動行程
        /// </remarks>
        [MVVM("手臂工作範圍", false)]
        public double HandJobLimit { get; set; }
        /// <summary>
        /// 出口下壓夾料到加工0點的長度,下壓零件與主軸刀徑有錯開。
        /// 意思是加工若可以做300,送料須加上此段長度才能讓出口處下壓固定處夾持
        /// </summary>
        [MVVM(" 出口下壓夾料到加工0點", false)]
        public double OutClampToZeroLength { get; set; }
        /// <summary>
        /// 手臂水平夾爪垂直軸的總行程
        /// </summary>
        [MVVM("水平夾爪垂直軸總行程", false)]
        public double InTotalLength { get; set; }
        /// <summary>
        /// 手臂垂直夾爪垂直軸的總行程
        /// </summary>
        [MVVM("手臂垂直夾爪垂直軸的總行程", false)]
        public double OutTotalLength { get; set; }
        /// <summary>
        /// 手臂長度 ( 預設值2300 )
        /// </summary>
        [MVVM("手臂長度", false)]
        public double Length { get; set; }
        /// <summary>
        /// 垂直夾取時,X軸的最大移動行程_需預設，垂直夾取中_確保水平夾爪不會撞機的行程位置
        /// </summary>
        [MVVM("垂直夾取 X 軸的最大移動行程", false)]
        public double VerticalGrippingXLimit { get; set; }
        /// <summary>
        /// 垂直夾爪走到極限X時,垂直夾爪可以夾取的點 到 送料原點的長度。( 預設 2000 )
        /// </summary>
        [MVVM("垂直夾爪走到極限 X ", false)]
        public double VerticalToOriginLength { get; set; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (翼板向)。( 預設150 )
        /// </summary>
        [MVVM("翼板夾爪頭到 Sensor 的長度", false)]
        public double ArmInDownWingSensorToForntLength { get; set; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (腹板向)。( 預設150 )
        /// </summary>
        [MVVM("腹板爪頭到 Sensor 的長度", false)]
        public double ArmInDownBellySensorToForntLength { get; set; }
        /// <summary>
        /// 水平夾的檢測位置座標
        /// 配合向下照的SENSOR,固定一個位置,並將Sensor檢測距離設定完成
        /// </summary>
        [MVVM("水平夾的檢測位置座標", false)]
        public double ArmInDownCheckPosition { get; set; }
        /// <summary>
        /// 手臂移動極限範圍(到原點)，手臂原點到送料原點
        /// </summary>
        [MVVM("手臂移動極限範圍", false)]
        public double ArmXToOriginPoint { get; set; }
        /// <summary>
        /// 垂直夾爪最遠端與X原點交界的X軸長度位置__預設800
        /// </summary>
        [MVVM("垂直夾爪最遠端與原點交界", false)]
        public double ArmInTouchOriginLength { get; set; }
        /// <summary>
        /// 手臂送料原點到出口上浮滾輪的長度
        /// </summary>
        [MVVM("原點到上浮滾輪距離", false)]
        public double ArmFeedOriginToOutUpRoll { get; set; }
        /// <summary>
        /// 翼板測距Sensor總行程長度
        /// </summary>
        [MVVM("翼板測距 Sensor 總行程長度", false)]
        public double WingSensorHigh { get; set; }
        /// <summary>
        /// 腹板測距Sensor總行程長度
        /// </summary>
        [MVVM("腹板測距 Sensor 總行程長度", false)]
        public double BellySensorHigh { get; set; }
        /// <summary>
        /// 減速點測距Sensor總行程長度(抓最長的)
        /// </summary>
        [MVVM("減速點測距Sensor總行程長度", false)]
        public double FeedSlowDownPointTotalLength { get; set; }
        /// <summary>
        /// 手臂送料減速點到原點距離
        /// </summary>
        [MVVM("手臂送料減速點到原點距離", false)]
        public double SlowToOriginLength { get; set; }
        /// <summary>
        /// 手臂最大行程(夾腹板可到X最極限位置)_目前10655
        /// </summary>
        [MVVM("手臂夾腹板可到 X 最極限位(", false)]
        public double XLimit { get; set; }
        /// <summary>
        /// 夾腹板In_Z到底的安全長度_預設80到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        [MVVM("夾腹板In_Z到底的安全長度", false)]
        public double ArmInZSafeLength { get; set; }
        /// <summary>
        ///  夾翼板Out_Z到底的安全長度_預設60
        /// 到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        [MVVM("夾翼板Out_Z到底的安全長度", false)]
        public double ArmOutZSafeLength { get; set; }
        /// <inheritdoc/>
        [MVVM("配合實際 Sensor 設定的距離", false, description: "配合實際 Sensor 設定的距離")]
        public double DeceleratingOrigin { get; set; }
        /// <inheritdoc/>
        [MVVM("水平夾爪補正值", false, description: "水平夾爪夾取時，夾爪夾取翼板的位置補正")]
        public double HorizontalCompensate { get; set; }
    }
}
