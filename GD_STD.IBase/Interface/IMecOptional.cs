using GD_STD.Enum;

namespace GD_STD.Base
{
    public interface IMecOptional
    {
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料中間的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
       
         bool Middle { get; set; }
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
       
         bool LeftExport { get; set; }
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
       
         bool RightExport { get; set; }
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
       
         bool LeftEntrance { get; set; }
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
         bool RightEntrance { get; set; }
        /// <summary>
        /// 入口橫移料架選配
        /// </summary>
        /// <remarks>如果有選配橫移料架則回傳 0。</remarks>
         byte EntranceTraverseNumber { get; set; }
        /// <summary>
        /// 出口橫移料架
        /// </summary>
        /// <remarks>如果有選配橫移料架則回傳 0。</remarks>
         byte ExportTraverseNumber { get; set; }
        /// <summary>
        /// 手臂自動夾料
        /// </summary>
        /// <remarks>如果有選配 手臂自動夾料。回傳 ture，沒有則回傳 false</remarks>
         bool HandAuto { get; set; }
        /// <summary>
        /// 手臂靠邊裝置數量
        /// </summary>
        /// <remarks>
        /// 最小2，最大6
        /// </remarks>
         byte StepAsideCount { get; set; }
        /// <summary>
        /// 自動測刀長系統選配
        /// </summary>
         bool AutoDrill { get; set; }
        /// <summary>
        /// 橫移料架形狀
        /// </summary>
         RACK_ROUTE ExportTraverseRoute { get; set; }
        /// <summary>
        /// 出口橫移自動選配
        /// </summary>
         bool ExportTraverseAuto { get; set; }
        /// <summary>
        /// 入口橫移自動選配
        /// </summary>
         bool EntranceTraverseAuto { get; set; }
        /// <summary>
        /// 畫線功能
        /// </summary>
        /// <remarks>
        /// 切割線、畫板線、刻字
        /// </remarks>
         bool DrawLine { get; set; }
        /// <summary>
        /// 銑刀功能
        /// </summary>
         bool MillingCutter { get; set; }
        /// <summary>
        /// 攻牙功能
        /// </summary>
         bool Tapping { get; set; }
        /// <summary>
        /// 出口有動力料架 Snsor 數量
        /// </summary>
         byte ExportTraverseSnsorCount { get; set; }
        /// <summary>
        /// 出口左邊無動力料架 Snsor 數量
        /// </summary>
        /// <remarks>
        /// 背對機器出口處的左邊
        /// </remarks>
         byte ExportTraverseLeftSnsorCount { get; set; }
        /// <summary>
        /// 出口右邊無動力料架 Snsor 數量
        /// </summary>
        /// <remarks>
        /// 背對機器出口處的右邊邊
        /// </remarks>
         byte ExportTraverseRightSnsorCount { get; set; }
        /// <summary>
        /// 機器入料端反向
        /// </summary>
         bool Reverse { get; set; }
    }
}