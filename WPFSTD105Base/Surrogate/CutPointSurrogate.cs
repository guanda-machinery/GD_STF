using devDept.Serialization;
using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Model;

namespace WPFSTD105.Surrogate
{
    /// <summary>
    ///  定義 <see cref="CutPoint"/> 代理
    /// </summary>
    public class CutPointSurrogate : Surrogate<CutPoint>, IAxis2D
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CutPointSurrogate(CutPoint)' 的 XML 註解
        public CutPointSurrogate(CutPoint obj) : base(obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CutPointSurrogate(CutPoint)' 的 XML 註解
        {

        }
        /// <inheritdoc/>
        public double X { get; set; }
        /// <inheritdoc/>
        public double Y { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.ConvertToObject()' 的 XML 註解
        protected override CutPoint ConvertToObject()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.ConvertToObject()' 的 XML 註解
        {
            CutPoint result = new CutPoint();
            CopyDataToObject(result);
            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CopyDataFromObject(CutPoint)' 的 XML 註解
        protected override void CopyDataFromObject(CutPoint obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CopyDataFromObject(CutPoint)' 的 XML 註解
        {
            X = obj.X;
            Y = obj.Y;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CopyDataToObject(CutPoint)' 的 XML 註解
        protected override void CopyDataToObject(CutPoint obj)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPointSurrogate.CopyDataToObject(CutPoint)' 的 XML 註解
        {
            obj.X = X;
            obj.Y = Y;
        }
        /// <summary>
        /// 在反序列化過程中將代理轉換為相關對象。
        /// </summary>        
        public static implicit operator CutPoint(CutPointSurrogate surrogate)
        {
            return surrogate == null ? null : surrogate.ConvertToObject();
        }

        /// <summary>
        /// 在序列化過程中將對象轉換為相關的代理。
        /// </summary>
        public static implicit operator CutPointSurrogate(CutPoint source)
        {
            return source == null ? null : source.ConvertToSurrogate();
        }
    }
}
