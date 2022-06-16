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
        public BoltAttrSurrogate(BoltAttr obj) : base(obj)
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
        public Guid GUID { get; set; }
        /// <inheritdoc/>
        public OBJETC_TYPE Type { get; set; }

        protected override BoltAttr ConvertToObject()
        {
            BoltAttr result = new BoltAttr();
            CopyDataToObject(result);
            return result;
        }

        protected override void CopyDataFromObject(BoltAttr obj)
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
        }

        protected override void CopyDataToObject(BoltAttr obj)
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
