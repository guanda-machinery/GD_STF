using devDept.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.Model;

namespace WPFSTD105.Surrogate
{
    /// <summary>
    /// 定義 <see cref="CutList"/> 代理
    /// </summary>
    public class CutListSurrogate : Surrogate<CutList>
    {
        public CutListSurrogate(CutList obj) : base(obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CutListSurrogate(CutList)' 的 XML 註解
        {
        }
        /// <summary>
        /// <see cref="CutList.UR"/>
        /// </summary>
        public CutPointSurrogate UR = new CutPoint().ConvertToSurrogate();
        /// <summary>
        /// <see cref="CutList.DR"/>
        /// </summary>
        public CutPointSurrogate DR = new CutPoint().ConvertToSurrogate();
        /// <summary>
        /// <see cref="CutList.UL"/>
        /// </summary>
        public CutPointSurrogate UL = new CutPoint().ConvertToSurrogate();
        /// <summary>             
        /// <see cref="CutList.DL"/>                   
        /// </summary>            
        public CutPointSurrogate DL = new CutPoint().ConvertToSurrogate();

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CutListSurrogate(CutList)' 的 XML 註解

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.ConvertToObject()' 的 XML 註解
        protected override CutList ConvertToObject()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.ConvertToObject()' 的 XML 註解
        {
            CutList result = new CutList();
            CopyDataToObject(result);
            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CopyDataFromObject(CutList)' 的 XML 註解
        protected override void CopyDataFromObject(CutList obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CopyDataFromObject(CutList)' 的 XML 註解
        {
            UR = obj.UR ?? new CutPoint().ConvertToSurrogate() ;
            DR = obj.DR ?? new CutPoint().ConvertToSurrogate();
            UL = obj.UL ?? new CutPoint().ConvertToSurrogate();
            DL = obj.DL ?? new CutPoint().ConvertToSurrogate();
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CopyDataToObject(CutList)' 的 XML 註解
        protected override void CopyDataToObject(CutList obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutListSurrogate.CopyDataToObject(CutList)' 的 XML 註解
        {
            obj.UR = UR;
            obj.DR = DR;
            obj.UL = UL;
            obj.DL = DL;
        }
       
        /// <summary>
        /// 在反序列化過程中將代理轉換為相關對象。
        /// </summary>        
        public static implicit operator CutList(CutListSurrogate surrogate)
        {
            return surrogate == null ? null : surrogate.ConvertToObject();
        }

        /// <summary>
        /// 在序列化過程中將對象轉換為相關的代理。
        /// </summary>
        public static implicit operator CutListSurrogate(CutList source)
        {
            return source == null ? null : source.ConvertToSurrogate();
        }
    }
}
