namespace GD_STD.Base
{
    /// <summary>
    /// 刀庫介面
    /// </summary>
    public interface IDrillWarehouse
    {
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向 A 刀庫</remarks>
        DrillSetting[] Middle { get; set; }
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向 B 刀庫</remarks>
        DrillSetting[] LeftExport { get; set; }
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向 C 刀庫</remarks>
        DrillSetting[] RightExport { get; set; }
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料左邊的軸向 D 刀庫</remarks>
        DrillSetting[] LeftEntrance { get; set; }
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料右邊的軸向 E 刀庫</remarks>
        DrillSetting[] RightEntrance { get; set; }
    }
}