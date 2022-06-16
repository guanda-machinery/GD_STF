using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// 報表 Part 訊息
    /// </summary>
    [Serializable]
    public class SteelPart : ITekla, IProfile, IPart, ISecondary, IModelObjectBase
    {
        public SteelPart()
        {
            ID = new List<int>();
            Father = new List<int>();
            Creation = DateTime.Now;
        }
        /// <inheritdoc/>
        [TeklaBom(1)]
        public List<int> ID { get; set; }
        [TeklaBom(2)]
        public string Number { get; set; }
        /// <inheritdoc/>
        [TeklaBom(3)]
        public string Profile { get; set; }
        [TeklaBom(4)]
        /// <inheritdoc/>
        public OBJETC_TYPE Type { get; set; }
        [TeklaBom(5)]
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        [TeklaBom(6)]
        public float H { get; set; }
        /// <inheritdoc/>
        [TeklaBom(7)]
        public float W { get; set; }
        /// <inheritdoc/>
        [TeklaBom(8)]
        public float t1 { get; set; }
        /// <inheritdoc/>
        [TeklaBom(9)]
        public float t2 { get; set; }
        /// <inheritdoc/>
        [TeklaBom(10)]
        public MATERIAL Material { get; set; }
        /// <inheritdoc/>
        [TeklaBom(11)]
        public double UnitWeight { get; set; }
        [TeklaBom(12)]
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        [TeklaBom(13)]
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public int Count { get; set; }
        /// <inheritdoc/>
        public List<int> Father { get; set; }
        /// <summary>
        /// 圖檔 GUID 
        /// </summary>
        public Guid GUID { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime Creation { get; private set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime Revise { get; set; }
        /// <summary>
        /// 圖面狀態
        /// </summary>
        public DRAWING_STATE State { get; set; }

        #region 公開方法
        /// <inheritdoc/>
        public double TotalWeight() => UnitWeight * Count;
        /// <inheritdoc/>
        public double TotalArea() => UnitWeight * Count;

        #endregion
    }
}