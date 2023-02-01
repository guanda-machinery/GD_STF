using DevExpress.Xpf.Ribbon.Customization.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105
{
    [Serializable]
    public class SettingParGroupBoltsTypeModel
    {
        #region GroupBoltsType 客製孔群設定

        /// <summary>
        /// 只會在讀取才用到的值 剛新增的孔群不會有值
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        /// 客製孔群 - 孔群編號
        /// </summary>
        public string groupBoltsTypeName { get; set; }
        /// <summary>
        /// 客製孔群 - 孔群資料
        /// Dia, StartHole, Mode, dX, dY, Face, groupBoltsType
        /// </summary>
        public GroupBoltsAttr groupBoltsAttr { get; set; }

        /// <summary>
        /// 客製孔群 - 建立日期
        /// </summary>
        public DateTime Creation { get; set; }
        /// <summary>
        /// 客製孔群 - 修改日期
        /// </summary>
        public DateTime Revise { get; set; }
        #endregion


        [NonSerialized]
        public static readonly SettingParGroupBoltsTypeModel NotSelectGroupBoltsTypeModel = new SettingParGroupBoltsTypeModel()
        {
            groupBoltsTypeName = NotSelectGroupBolts,
            groupBoltsAttr = null,
            OriginalFilePath = null,
        };

        [NonSerialized]
        public const string NotSelectGroupBolts = "未選擇孔群";
    }
}
