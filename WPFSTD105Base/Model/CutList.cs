using System;
using WPFSTD105.Surrogate;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 切割點位列表
    /// </summary>
    [Serializable]
    public class CutList
    {
        /// <summary>
        /// 預設
        /// </summary>
        public CutList()
        {

        }
        /// <summary>
        /// 切割點位列表
        /// </summary>
        /// <param name="ur">右邊上緣</param>
        /// <param name="dr">右邊下緣</param>
        /// <param name="ul">左邊上緣</param>
        /// <param name="dl">左邊下緣</param>
        public CutList(CutPoint ur, CutPoint dr, CutPoint ul, CutPoint dl)
        {
            UR = ur;
            DR = dr;
            UL = ul;
            DL = dl;
        }
        /// <summary>
        /// 右邊上緣
        /// </summary>
        public CutPoint UR { get; set; } = new CutPoint();
        /// <summary>
        /// 右邊下緣
        /// </summary>
        public CutPoint DR { get; set; } = new CutPoint();
        /// <summary>
        /// 左邊上緣
        /// </summary>
        public CutPoint UL { get; set; } = new CutPoint();
        /// <summary>
        /// 左邊下緣
        /// </summary>
        public CutPoint DL { get; set; } = new CutPoint();

        /// <summary>
        /// <see cref="CutList"/> 轉換 <see cref="CutListSurrogate"/>
        /// </summary>
        /// <returns></returns>
        public virtual CutListSurrogate ConvertToSurrogate()
        {
            return new CutListSurrogate(this);
        }
    }
}
