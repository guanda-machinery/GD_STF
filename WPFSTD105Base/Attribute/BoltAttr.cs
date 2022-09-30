using GD_STD.Enum;
using System;
using WPFSTD105.Surrogate;
using GD_STD.Base;

namespace WPFSTD105.Attribute
{
    ///  <summary>
    /// 單個螺栓的自定義屬性檔案
    /// </summary>
    [Serializable]
    public class BoltAttr : AbsAttr, IAxis3D, IBoltAttr
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BoltAttr.BoltAttr()' 的 XML 註解
        public BoltAttr()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BoltAttr.BoltAttr()' 的 XML 註解
        {

        }

        /// <summary>
        /// 鑽孔類型
        /// </summary>
        public AXIS_MODE Mode { get; set; }
        /// <summary>
        /// 螺栓直徑
        /// </summary>
        public double Dia { get; set; } = 20;
        /// <summary>
        /// 絕對X座標
        /// </summary>
        public virtual double X { get; set; }
        /// <summary>
        /// 絕對Y座標
        /// </summary>
        public virtual double Y { get; set; }
        /// <summary>
        /// 絕對座標Z
        /// </summary>
        public virtual double Z { get; set; }
        /// <summary>
        /// 實體面
        /// </summary>
        public FACE Face { get; set; }
        /// <summary>
        /// 圓柱體高度
        /// </summary>
        public double t { get; set; }

        /// <summary>
        /// 圖塊名稱
        /// </summary>
        public string BlockName { get; set; }   

        /// <summary>
        /// <see cref="BoltAttr"/> 轉換 <see cref="BoltAttrSurrogate"/>
        /// </summary>
        /// <returns></returns>
        public virtual BoltAttrSurrogate ConvertToSurrogate()
        {
            return new BoltAttrSurrogate(this);
        }
    }
}
