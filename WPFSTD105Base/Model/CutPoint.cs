using GD_STD.Base;
using GD_STD.IBase;
using System;
using WPFSTD105.Surrogate;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 切割點
    /// </summary>
    [Serializable]
    public class CutPoint : IAxis2D
    {
        /// <summary>
        ///  預設
        /// </summary>
        public CutPoint()
        {
            X= 0;
            Y= 0;
        }

        /// <summary>
        /// 反序列化結構
        /// </summary>
        public CutPoint(CutPointSurrogate cut)
        {
            X = cut == null ? 0 : cut.X;
            Y = cut == null ? 0 : cut.Y;
        }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPoint.X' 的 XML 註解
        public double X { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPoint.X' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CutPoint.Y' 的 XML 註解
        public double Y { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CutPoint.Y' 的 XML 註解
        /// <summary>
        /// <see cref="CutPoint"/> 轉換 <see cref="CutPointSurrogate"/>
        /// </summary>
        /// <returns></returns>
        public virtual CutPointSurrogate ConvertToSurrogate()
        {
            return new CutPointSurrogate(this);
        }
    }
}
