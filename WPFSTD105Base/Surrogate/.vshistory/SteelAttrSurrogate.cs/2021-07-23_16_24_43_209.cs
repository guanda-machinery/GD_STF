using devDept.Serialization;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.Model;

namespace WPFSTD105.Surrogate
{
    /// <summary>
    /// 定義 <see cref="SteelAttr"/> 代理
    /// </summary>
    public class SteelAttrSurrogate : Surrogate<SteelAttr>, IAbsAttr
    {
        public SteelAttrSurrogate(SteelAttr obj) : base(obj)
        {
        }
        /// <inheritdoc/>
        public Guid GUID { get; set; }
        /// <inheritdoc/>
        public OBJETC_TYPE Type { get; set; }
        /// <inheritdoc/>
        public string AsseNumber { get; set; }
        /// <inheritdoc/>
        public float Kg { get; set; }
        /// <inheritdoc/>
        public int Number { get; set; }
        /// <inheritdoc/>
        public string PartNumber { get; set; }
        /// <inheritdoc/>
        public CutListSurrogate PointFront { get; set; }
        /// <inheritdoc/>
        public CutListSurrogate PointTop { get; set; }
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public MATERIAL Material { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public float W { get; set; }
        /// <inheritdoc/>
        public float t1 { get; set; }
        /// <inheritdoc/>
        public float t2 { get; set; }
        /// <inheritdoc/>
        public string TeklaPartID { get; set; }
        /// <inheritdoc/>
        public string TeklaAssemblyID { get; set; }
        protected override SteelAttr ConvertToObject()
        {
            SteelAttr result = new SteelAttr();
            CopyDataToObject(result);
            return result;
        }

        protected override void CopyDataFromObject(SteelAttr obj)
        {
            AsseNumber = obj.AsseNumber;
            H = obj.H;
            Kg = obj.Kg;
            W = obj.W;
            t2 = obj.t2;
            Length = obj.Length;
            GUID = obj.GUID;
            Material = obj.Material;
            Number = obj.Number;
            PartNumber = obj.PartNumber;
            PointFront = obj.PointFront.ConvertToSurrogate();
            PointTop = obj.PointTop.ConvertToSurrogate();
            Profile = obj.Profile;
            t1 = obj.t1;
            Type = obj.Type;
            TeklaAssemblyID = obj.TeklaAssemblyID;
            TeklaPartID = obj.TeklaPartID;
        }

        protected override void CopyDataToObject(SteelAttr obj)
        {
            obj.AsseNumber = AsseNumber;
            obj.H = H;
            obj.Kg = Kg;
            obj.W = W;
            obj.t2 = t2;
            obj.Length = Length;
            obj.GUID = GUID;
            obj.Material = Material;
            obj.Number = Number;
            obj.PartNumber = PartNumber;
            obj.PointFront = PointFront ?? new CutList();
            obj.PointTop = PointTop ?? new CutList();
            obj.Profile = Profile;
            obj.t1 = t1;
            obj.Type = Type;
            obj.TeklaAssemblyID = TeklaAssemblyID;
            obj.TeklaPartID = TeklaPartID;
        }
        /// <summary>
        /// 在反序列化過程中將代理轉換為相關對象。
        /// </summary>        
        public static implicit operator SteelAttr(SteelAttrSurrogate surrogate)
        {
            return surrogate == null ? null : surrogate.ConvertToObject();
        }
        /// <summary>
        /// 在序列化過程中將對象轉換為相關的代理。
        /// </summary>
        public static implicit operator SteelAttrSurrogate(SteelAttr source)
        {
            return source == null ? null : source.ConvertToSurrogate();
        }
    }
}
