using devDept.Geometry;
using DevExpress.Utils.Extensions;
using GD_STD.Data;
using GD_STD.Enum;
using GD_STD.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using WPFSTD105.Model;
using WPFSTD105.Surrogate;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 主要鋼構物件自定義資訊
    /// </summary>
    [Serializable]
    public class SteelAttrInExcel : AbsAttrInExcel
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public SteelAttrInExcel()
        {

        }
        /// <inheritdoc/>
        public string section_name { get; set; }
        /// <inheritdoc/>
        public float h { get; set; }
        /// <inheritdoc/>
        public float b { get; set; }
        /// <inheritdoc/>
        public float t1 { get; set; }
        /// <inheritdoc/>
        public float t2 { get; set; }
        /// <inheritdoc/>
        public float r1 { get; set; }
        /// <inheritdoc/>
        public float r2 { get; set; }
        /// <inheritdoc/>
        public float surface_area { get; set; }
        /// <inheritdoc/>
        public float section_area { get; set; }
        /// <inheritdoc/>
        public float weight_per_unit { get; set; } = 1;
        /// <inheritdoc/>
        public int density { get; set; }
        //突出肢長度e(mm)
        public float e { get; set; }
        //直徑d(mm)
        public float diameter { get; set; }
    }
}
