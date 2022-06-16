using GD_STD.Base;
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
    public class SteelAssembly : IMerge, ITeklaObject, IDrawing, IModelObjectBase, ITekla
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
            Creation = DateTime.Now;
            IsTekla = true;
            Count++;
        }
        #region 公開屬性
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
        public int Count { get; private set; }
        /// <inheritdoc/>
        public DateTime Creation { get; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
        /// <inheritdoc/>
        public DRAWING_STATE State { get; set; }
        /// <inheritdoc/>
        public Guid? GUID { get; set; }
        public bool IsTekla { get; private set; }
        #endregion

        #region 公開方法
        /// <inheritdoc/>
        public void Merge(object obj)
        {
            if (obj is SteelAssembly steel)
            {
                if (steel.Number != this.Number)
                {
                    throw new Exception("合併物件失敗，因編號不相同。");
                }
                ID.AddRange(steel.ID);
                Position.AddRange(steel.Position);
                TopLevel.AddRange(steel.TopLevel);
                Phase.AddRange(steel.Phase);
                ShippingNumber.AddRange(steel.ShippingNumber);
                ShippingDescription.AddRange(steel.ShippingDescription);
                Count++;
            }
            else
            {
                throw new Exception("合併物件失敗，物件不相同。");
            }
        }
        /// <inheritdoc/>
        public double TotalWeight() => UnitWeight * Count;
        /// <inheritdoc/>
        public double TotalArea() => UnitWeight * Count;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SteelAssembly assembly &&
                   Number == assembly.Number &&
                   DrawingName == assembly.DrawingName &&
                   W == assembly.W &&
                   H == assembly.H &&
                   Length == assembly.Length &&
                   UnitWeight == assembly.UnitWeight &&
                   UnitArea == assembly.UnitArea &&
                   Count == assembly.Count;
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -640012255;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DrawingName);
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + UnitWeight.GetHashCode();
            hashCode = hashCode * -1521134295 + UnitArea.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }
        /// <inheritdoc/>
        public void Reduce(int id)
        {
            int index = ID.Find(el => el == id);
            ID.Remove(index);
            Count--;
        }
        /// <inheritdoc/>
        public static bool operator ==(SteelAssembly left, SteelAssembly right)
        {
            return EqualityComparer<SteelAssembly>.Default.Equals(left, right);
        }
        /// <inheritdoc/>
        public static bool operator !=(SteelAssembly left, SteelAssembly right)
        {
            return !(left == right);
        }
        #endregion
    }
}