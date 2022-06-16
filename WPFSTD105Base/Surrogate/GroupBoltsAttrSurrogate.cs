using devDept.Serialization;
using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.Surrogate
{
    /// <summary>
    /// 定義 <see cref="GroupBoltsAttr"/> 代理
    /// </summary>
    public class GroupBoltsAttrSurrogate : Surrogate<GroupBoltsAttr>, IBoltAttr, IModelObjectBase, IGroupBoltsAttr
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.GroupBoltsAttrSurrogate(GroupBoltsAttr)' 的 XML 註解
        public GroupBoltsAttrSurrogate(GroupBoltsAttr obj) : base(obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.GroupBoltsAttrSurrogate(GroupBoltsAttr)' 的 XML 註解
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
        public OBJETC_TYPE Type { get; set; }
        /// <inheritdoc/>
        public string dX { get; set; }
        /// <inheritdoc/>
        public string dY { get; set; }
        /// <inheritdoc/>
        public START_HOLE StartHole { get; set; }
        /// <inheritdoc/>
        public int xCount { get; set; }
        /// <inheritdoc/>
        public int yCount { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.ConvertToObject()' 的 XML 註解
        protected override GroupBoltsAttr ConvertToObject()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.ConvertToObject()' 的 XML 註解
        {
            GroupBoltsAttr result = new GroupBoltsAttr();
            CopyDataToObject(result);

            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.CopyDataFromObject(GroupBoltsAttr)' 的 XML 註解
        protected override void CopyDataFromObject(GroupBoltsAttr obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.CopyDataFromObject(GroupBoltsAttr)' 的 XML 註解
        {
            Dia = obj.Dia;
            dX = obj.dX;
            dY = obj.dY;
            Face = obj.Face;
            GUID = obj.GUID;
            Mode = obj.Mode;
            StartHole = obj.StartHole;
            t = obj.t;
            Type = obj.Type;
            xCount = obj.xCount;
            X = obj.X;
            Y = obj.Y;
            Z = obj.Z;
            yCount = obj.yCount;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.CopyDataToObject(GroupBoltsAttr)' 的 XML 註解
        protected override void CopyDataToObject(GroupBoltsAttr obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttrSurrogate.CopyDataToObject(GroupBoltsAttr)' 的 XML 註解
        {
            obj.Dia = Dia;
            obj.dX = dX;
            obj.dY = dY;
            obj.Face = Face;
            obj.GUID = GUID;
            obj.Mode = Mode;
            obj.StartHole = StartHole;
            obj.t = t;
            obj.Type = Type;
            obj.X = X;
            obj.xCount = xCount;
            obj.Y = Y;
            obj.yCount = yCount;
            obj.Z = Z;
        }
        /// <summary>
        /// 在反序列化過程中將代理轉換為相關對象。
        /// </summary>        
        public static implicit operator GroupBoltsAttr(GroupBoltsAttrSurrogate surrogate)
        {
            return surrogate == null ? null : surrogate.ConvertToObject();
        }
        /// <summary>
        /// 在序列化過程中將對象轉換為相關的代理。
        /// </summary>
        public static implicit operator GroupBoltsAttrSurrogate(GroupBoltsAttr source)
        {
            return source == null ? null : source.ConvertToSurrogate();
        }
    }
}
