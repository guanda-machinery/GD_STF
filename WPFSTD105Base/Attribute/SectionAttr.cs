using System.ComponentModel;
using System;

namespace SectionData
{
    [Serializable]
    public class SectionTypeProcessingData
    {
        /// <summary>
        /// 斷面類別
        /// </summary>
        public string SectionCategoryType { get; set; }
        /// <summary>
        /// 加工行為
        /// </summary>
        public int ProcessingBehavior { get; set; }
        /// <summary>
        /// 斷面加工區域參數A
        /// </summary>
        public int A { get; set; }
        /// <summary>
        /// 斷面加工區域參數B
        /// </summary>
        public int B { get; set; }
        /// <summary>
        /// 斷面加工區域參數C
        /// </summary>
        public int C { get; set; }
    }

    public enum SectionProcessing_SteelType
    {
        /// <summary>
        /// H型鋼
        /// </summary>
        H,
        /// <summary>
        /// 方管
        /// </summary>
        BOX,
        /// <summary>
        /// 槽管
        /// </summary>
        CH,
    }

    public enum ProcessingBehavior
    {
        /// <summary>
        /// 鑽孔
        /// </summary>
        [Description("鑽孔")]
        DRILLING,
        /// <summary>
        /// 打點
        /// </summary>
        //[Description("打點")] //20220815 張燕華 關閉打點功能
        POINT,
    }

    public class ProcessingBehavior_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<ProcessingBehavior>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}