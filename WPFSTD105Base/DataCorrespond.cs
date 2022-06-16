using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105
{
    /// <summary>
    /// 模型物件編號與序列化檔案名稱的相對關係
    /// </summary>
    [Serializable]
    public class DataCorrespond : IModelData
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 資料名稱
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 斷面規格名稱
        /// </summary>
        public string Profile { get; set; }
        /// <summary>
        /// 物件類型
        /// </summary>
        public OBJETC_TYPE Type { get; set; }
        /// <summary>
        /// NC 分析出來的佔存檔
        /// </summary>
        public bool TP { get; set; }
    }
}
