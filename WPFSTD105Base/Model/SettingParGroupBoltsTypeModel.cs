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
        /// 客製孔群 - 孔群編號
        /// </summary>
        public string groupBoltsTypeName { get; set; }
        /// <summary>
        /// 客製孔群 - 孔群資料
        /// Dia, StartHole, Mode, dX, dY, Face, groupBoltsType
        /// </summary>
        public GroupBoltsAttr groupBoltsAttr { get; set; } = new GroupBoltsAttr();

        /// <summary>
        /// 客製孔群 - 建立日期
        /// </summary>
        public DateTime Creation { get; }
        /// <summary>
        /// 客製孔群 - 修改日期
        /// </summary>
        public DateTime Revise { get; set; }
        #endregion
    }
}
