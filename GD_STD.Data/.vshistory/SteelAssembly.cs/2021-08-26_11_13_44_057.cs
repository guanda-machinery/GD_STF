using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// 報表 Assembly 訊息
    /// </summary>
    [Serializable]
    public class SteelAssembly : ITekla
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public SteelAssembly()
        {
            ID = new List<int>();
            Position = new List<string>();
            TopLevel = new List<string>();
            Phase = new List<int>();
            ShippingNumber = new List<int>();
            ShippingDescription = new List<string>();
        }
        /// <summary>
        /// Tekla Model ID
        /// </summary>
        [TeklaBom(1)]
        public List<int> ID { get; set; }
        /// <summary>
        /// 構件編號
        /// </summary>
        [TeklaBom(2)]
        public string Number { get; set; }
        /// <summary>
        /// 構件位置
        /// </summary>
        [TeklaBom(3)]
        public List<string> Position { get; set; }
        /// <summary>
        /// 構件高層
        /// </summary>
        [TeklaBom(4)]
        public List<string> TopLevel { get; set; }
        /// <summary>
        /// 區域號碼
        /// </summary>
        [TeklaBom(5)]
        public List<int> Phase { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        [TeklaBom(6)]
        public List<int> ShippingNumber { get; set; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        [TeklaBom(7)]
        public List<string> ShippingDescription { get; set; }
        /// <summary>
        /// Tekla 圖名稱
        /// </summary>
        [TeklaBom(8)]
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        [TeklaBom(9)]
        public float W { get; set; }
        /// <inheritdoc/>
        [TeklaBom(10)]
        public float H { get; set; }
        /// <inheritdoc/>
        [TeklaBom(11)]
        public double Length { get; set; }
        /// <inheritdoc/>
        [TeklaBom(12)]
        public double UnitWeight { get; set; }
        /// <inheritdoc/>
        [TeklaBom(13)]
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        public double TotalWeight { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public double TotalArea { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public int Count { get; set; }
    }
}