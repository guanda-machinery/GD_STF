using GD_STD.Attribute;
using GD_STD.Base;
using GD_STD.Base.Additional;
using Newtonsoft.Json;
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
    /// 選配設定
    /// </summary>
    [Serializable]
    [MetadataType(typeof(MetadataFluentAPI))]
    public class OptionSettings
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public OptionSettings()
        {

        }
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        [MVVM("A軸刀庫", true)]
        public bool Middle { get; set; }

        /// <summary>
        /// 左軸出口刀庫
        /// </summary>
        [MVVM("B軸刀庫", true)]
        public bool LeftExport { get; set; }

        /// <summary>
        /// 右軸出口刀庫
        /// </summary>
        [MVVM("C軸刀庫", true)]
        public bool RightExport { get; set; }

        /// <summary>
        /// 左軸入口刀庫
        /// </summary>
        [MVVM("D軸刀庫", true)]
        public bool LeftEntrance { get; set; }
        /// <summary>
        /// 右軸入口刀庫
        /// </summary>
        [MVVM("E軸刀庫", true)]
        public bool RightEntrance { get; set; }

        /// <summary>
        /// 手臂自動
        /// </summary>
        [MVVM("手臂自動", true)]
        public bool HandAuto { get; set; }

        /// <summary>
        /// 入口橫移數量
        /// </summary>
        [MVVM("入口橫移數量", true)]
        public byte EntranceTraverseNumber { get; set; }
        /// <summary>
        /// 出口橫移數量
        /// </summary>
        [MVVM("出口橫移數量", true)]
        public byte ExportTraverseNumber { get; set; }
        /// <summary>
        /// 自動測刀長系統選配
        /// </summary>
        [MVVM("自動測刀長", true)]
        public bool AutoDrill { get; set; }
        /// <summary>
        /// 出口料架型態
        /// </summary>
        [MVVM("出口料架型態", true)]
        public GD_STD.Enum.RACK_ROUTE ExportTraverseRoute { get; set; }
        /// <summary>
        /// 出口橫移自動
        /// </summary>
        [MVVM("出口橫移自動", true)]
        public bool ExportTraverseAuto { get; set; }
        /// <summary>
        /// 手臂靠邊裝置數量
        /// </summary>
        /// <remarks>
        /// 最小2，最大6
        /// </remarks>
        [MVVM("手臂靠邊裝置數量", true)]
        public byte StepAsideCount { get; set; }
        /// <summary>
        /// 入口橫移自動
        /// </summary>
        [MVVM("入口橫移自動", true)]
        public bool EntranceTraverseAuto { get; set; }

        /// <summary>
        /// 劃線
        /// </summary>
        [MVVM("劃線", true)]
        public bool DrawLine { get; set; }

        /// <summary>
        /// 銑刀
        /// </summary>
        [MVVM("銑刀", true)]
        public bool MillingCutter { get; set; }

        /// <summary>
        /// 攻牙
        /// </summary>
        [MVVM("攻牙", true)]
        public bool Tapping { get; set; }

        /// <summary>
        /// 出口橫移感應器數量
        /// </summary>
        [MVVM("出口橫移感應器數量", true)]
        public byte ExportTraverseSnsorCount { get; set; }

        /// <summary>
        /// 出口橫移左側感應器數量
        /// </summary>
        [MVVM("出口橫移左側感應器數量", true)]
        public byte ExportTraverseLeftSnsorCount { get; set; }
        /// <summary>
        /// 出口橫移右側感應器數量
        /// </summary>
        [MVVM("出口橫移右側感應器數量", true)]
        public byte ExportTraverseRightSnsorCount { get; set; }

        /// <summary>
        /// 機器正反向
        /// </summary>
        [MVVM("機器正反向", true)]
        public bool Reverse { get; set; }
        ///// <summary>
        ///// 電壓
        ///// </summary>
        //[MVVM("電壓", true)]
        //public ushort Voltage { get; set; }
        ///<inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
        }
    }
}
