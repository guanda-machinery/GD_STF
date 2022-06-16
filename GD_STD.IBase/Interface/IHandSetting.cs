namespace GD_STD.Base
{
    /// <summary>
    /// 手臂設定相關參數介面
    /// </summary>
    public interface IHandSetting
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
        double MaterialCorrectionWorkRange { get; set; }
        /// <summary>
        /// 送料原點到加工第一點(X=300)的長度,預設500。
        /// </summary>
        /// <remarks>
        /// 初次送料到加工點，工件原點到加工點的值。
        /// <para>
        /// 工程模式調整。
        /// </para>
        /// </remarks>
        double MaterialZeroToWorkRange { get; set; }
        /// <summary>
        /// 手臂工作範圍
        /// </summary>
        /// <remarks>
        /// 手臂移動行程
        /// </remarks>
        double HandJobLimit { get; set; }
        /// <summary>
        /// 出口下壓夾料到加工0點的長度,下壓零件與主軸刀徑有錯開。
        /// 意思是加工若可以做300,送料須加上此段長度才能讓出口處下壓固定處夾持
        /// </summary>
        double OutClampToZeroLength { get; set; }
        /// <summary>
        /// 手臂水平夾爪垂直軸的總行程
        /// </summary>
        double InTotalLength { get; set; }
        /// <summary>
        /// 手臂垂直夾爪垂直軸的總行程
        /// </summary>
        double OutTotalLength { get; set; }
        /// <summary>
        /// 手臂長度 ( 預設值2300 )
        /// </summary>
        double Length { get; set; }
        /// <summary>
        /// 垂直夾取時,X軸的最大移動行程_需預設，垂直夾取中_確保水平夾爪不會撞機的行程位置
        /// </summary>
        double VerticalGrippingXLimit { get; set; }
        /// <summary>
        /// 垂直夾爪走到極限X時,垂直夾爪可以夾取的點 到 送料原點的長度。( 預設 2000 )
        /// </summary>
        double VerticalToOriginLength { get; set; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (翼板向)。( 預設150 )
        /// </summary>
        double ArmInDownWingSensorToForntLength { get; set; }
        /// <summary>
        /// 夾爪頭到Sensor的長度,納入計算避免撞到手臂前端 (腹板向)。( 預設150 )
        /// </summary>
        double ArmInDownBellySensorToForntLength { get; set; }
        /// <summary>
        /// 水平夾的檢測位置座標
        /// 配合向下照的SENSOR,固定一個位置,並將Sensor檢測距離設定完成
        /// </summary>
        double ArmInDownCheckPosition { get; set; }
        /// <summary>
        /// 手臂移動極限範圍(到原點)，手臂原點到送料原點
        /// </summary>
        double ArmXToOriginPoint { get; set; }
        /// <summary>
        /// 垂直夾爪最遠端與X原點交界的X軸長度位置__預設800
        /// </summary>
        double ArmInTouchOriginLength { get; set; }
        /// <summary>
        /// 手臂送料原點到出口 上浮滾輪的長度
        /// </summary>
        double ArmFeedOriginToOutUpRoll { get; set; }
        /// <summary>
        /// 翼板測距Sensor總行程長度
        /// </summary>
        double WingSensorHigh { get; set; }
        /// <summary>
        /// 腹板測距Sensor總行程長度
        /// </summary>
        double BellySensorHigh { get; set; }
        /// <summary>
        /// 減速點測距Sensor總行程長度(抓最長的)
        /// </summary>
        double FeedSlowDownPointTotalLength { get; set; }
        /// <summary>
        /// 手臂送料減速點到原點距離
        /// </summary>
        double SlowToOriginLength { get; set; }
        /// <summary>
        /// 手臂最大行程(夾腹板可到X最極限位置)_目前10655
        /// </summary>
        double XLimit { get; set; }
        /// <summary>
        /// 夾腹板In_Z到底的安全長度_預設80到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        double ArmInZSafeLength { get; set; }
        /// <summary>
        ///  夾翼板Out_Z到底的安全長度_預設60
        /// 到底總行程需要扣掉此長度(工件安全長,才不會撞機)
        /// </summary>
        double ArmOutZSafeLength { get; set; }
        /// <summary>
        /// 減速原點的距離設定
        /// </summary>
        /// <remarks>
        /// 配合實際 Sensor 設定的距離
        /// </remarks>
        double DeceleratingOrigin { get; set; }
        /// <summary>
        /// 水平夾爪補正值
        /// </summary>
        /// <remarks>
        /// 水平夾爪夾取時，夾爪夾取翼板的位置補正
        /// </remarks>
        double HorizontalCompensate { get; set; }
    }
}