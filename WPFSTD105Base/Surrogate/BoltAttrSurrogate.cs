using devDept.Serialization;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using GD_STD.Base;
namespace WPFSTD105.Surrogate
{
    /// <summary>
    /// 定義 <see cref="BoltAttr"/> 代理
    /// </summary>
    public class BoltAttrSurrogate : Surrogate<BoltAttr> , IBoltAttr, IModelObjectBase
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.BoltAttrSurrogate(BoltAttr)' 的 XML 註解
        public BoltAttrSurrogate(BoltAttr obj) : base(obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.BoltAttrSurrogate(BoltAttr)' 的 XML 註解
        {
        }
        /// <inheritdoc/>
        public double Dia { get; set; }
        /// <inheritdoc/>
        public FACE Face { get; set; }
        /// <inheritdoc/>
        public AXIS_MODE Mode { get; set; }
        /// <inheritdoc/>
        public double t { get; set; }
        /// <inheritdoc/>
        public double X { get; set; }
        /// <inheritdoc/>
        public double Y { get; set; }
        /// <inheritdoc/>
        public double Z { get; set; }
        /// <inheritdoc/>
        public Guid? GUID { get; set; }
        /// <inheritdoc/>
        public OBJECT_TYPE Type { get; set; }
        /// <inheritdoc/>
        public string BlackName { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.ConvertToObject()' 的 XML 註解
        protected override BoltAttr ConvertToObject()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.ConvertToObject()' 的 XML 註解
        {
            BoltAttr result = new BoltAttr();
            CopyDataToObject(result);
            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.CopyDataFromObject(BoltAttr)' 的 XML 註解
        protected override void CopyDataFromObject(BoltAttr obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.CopyDataFromObject(BoltAttr)' 的 XML 註解
        {
            Dia = obj.Dia;
            Face = obj.Face;
            GUID = obj.GUID;
            Mode = obj.Mode;
            t = obj.t;
            Type = obj.Type;
            X = obj.X;
            Y = obj.Y;
            Z = obj.Z;
            BlackName= obj.BlockName;   
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.CopyDataToObject(BoltAttr)' 的 XML 註解
        protected override void CopyDataToObject(BoltAttr obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BoltAttrSurrogate.CopyDataToObject(BoltAttr)' 的 XML 註解
        {
            obj.Dia = Dia;
            obj.Face = Face;
            obj.GUID = GUID;
            obj.Mode = Mode;
            obj.t = t;
            obj.Type = Type;
            obj.X = X;
            obj.Y = Y;
            obj.Z = Z;
            obj.BlockName = BlackName;
        }
        /// <summary>
        /// 在反序列化過程中將代理轉換為相關對象。
        /// </summary>        
        public static implicit operator BoltAttr(BoltAttrSurrogate surrogate)
        {
            return surrogate == null ? null : surrogate.ConvertToObject();
        }
        /// <summary>
        /// 在序列化過程中將對象轉換為相關的代理。
        /// </summary>
        public static implicit operator BoltAttrSurrogate(BoltAttr source)
        {
            return source == null ? null : source.ConvertToSurrogate();
        }
    }
}
