using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// Tekla Assembly 訊息
    /// </summary>
    public class SteelAssembly : ITekla
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public SteelAssembly()
        {
            ID = new List<int>();
            Position = new List<string>();
        }
        /// <summary>
        /// 構件編號
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Tekla 圖名稱
        /// </summary>
        public string DrawingName { get; set; }
        /// <summary>
        /// Tekla Model ID
        /// </summary>
        public List<int> ID { get; set; }
        /// <summary>
        /// 構件位置
        /// </summary>
        public List<string> Position { get; set; }
        /// <summary>
        /// 構件高層
        /// </summary>
        public List<string> TopLevel { get; set; }
        /// <summary>
        /// 區域號碼
        /// </summary>
        public List<int> Phase { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        public List<int> ShippingNumber { get; set; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        public List<string> ShippingDescription { get; set; }
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public float W { get; set; }
        /// <inheritdoc/>
        public double UnitWeight { get; set; }
        /// <inheritdoc/>
        public double TotalWeight { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public int Count { get; set; }
        /// <inheritdoc/>
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        public double TotalArea { get; }
    }
}