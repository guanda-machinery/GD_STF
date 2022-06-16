using System.ComponentModel;

namespace WPFSTD105
{
    /// <summary>
    /// 機器模式
    /// </summary>
    public enum MachineMode
    {
        /// <summary>
        /// 尚未選擇模式
        /// </summary>
        [Description("尚未選擇模式")]
        NULL,
        /// <summary>
        /// 物聯網模式
        /// </summary>
        [Description("物聯網模式")]
        IoT,
        /// <summary>
        /// 單機模式
        /// </summary>
        [Description("單機模式")]
        Single,
    }
}
