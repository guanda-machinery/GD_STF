using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace SplitLineSettingData
{
    [Serializable]
    public class SplitLineSettingClass
    {
        /// <summary>
        /// 打點位置(A值)
        /// </summary>
        public string A { get; set; }
        /// <summary>
        /// 打點位置(B值)
        /// </summary>
        public string B { get; set; }
        /// <summary>
        /// 打點位置(C值)
        /// </summary>
        public string C { get; set; }
        /// <summary>
        /// 打點位置(D值)
        /// </summary>
        public string D { get; set; }
        /// <summary>
        /// 切割厚度
        /// </summary>
        public int Thickness { get; set; }
        /// <summary>
        /// 最小餘料長度
        /// </summary>
        public double RemainingLength { get; set; }
        /// <summary>
        /// 取得combobox item source
        /// </summary>
        public List<string> GetSplitLineItemSource(object current_cbb)
        {
            List<string> SL_ItemSource = new List<string>();
            switch (current_cbb)
            {
                case SplitLineCombobox.cbb_A:
                    SL_ItemSource.Add("1/5");
                    SL_ItemSource.Add("2/5");
                    //SL_ItemSource.Add("3/5");
                    //SL_ItemSource.Add("4/5");
                    return SL_ItemSource;
                case SplitLineCombobox.cbb_B:
                    //SL_ItemSource.Add("1/5");
                    //SL_ItemSource.Add("2/5");
                    SL_ItemSource.Add("3/5");
                    SL_ItemSource.Add("4/5");
                    return SL_ItemSource;
                case SplitLineCombobox.cbb_C:
                    SL_ItemSource.Add("1/5");
                    SL_ItemSource.Add("2/5");
                    //SL_ItemSource.Add("3/5");
                    //SL_ItemSource.Add("4/5");
                    return SL_ItemSource;
                case SplitLineCombobox.cbb_D:
                    //SL_ItemSource.Add("1/5");
                    //SL_ItemSource.Add("2/5");
                    SL_ItemSource.Add("3/5");
                    SL_ItemSource.Add("4/5");
                    return SL_ItemSource;
                default: 
                    return SL_ItemSource;
            }
        }
    }

    public enum SplitLineCombobox
    {
        /// <summary>
        /// Combobox A值
        /// </summary>
        cbb_A,
        /// <summary>
        /// Combobox B值
        /// </summary>
        cbb_B,
        /// <summary>
        /// Combobox C值
        /// </summary>
        cbb_C,
        /// <summary>
        /// Combobox D值
        /// </summary>
        cbb_D,
    }
}