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
    public class SteelPart : ITekla, IProfile, IPart, ISecondary, IModelObjectBase, IMerge, IDrawing
    {
        public SteelPart()
        {
            ID = new List<int>();
            Father = new List<int>();
            Creation = DateTime.Now;
        }
        #region 公開屬性
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
        #endregion

        #region 公開方法
        /// <inheritdoc/>
        public double TotalWeight() => UnitWeight * Count;
        /// <inheritdoc/>
        public double TotalArea() => UnitWeight * Count;
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SteelPart part &&
                   Profile == part.Profile &&
                   Type == part.Type &&
                   Length == part.Length &&
                   H == part.H &&
                   W == part.W &&
                   t1 == part.t1 &&
                   t2 == part.t2 &&
                   Material == part.Material &&
                   UnitWeight == part.UnitWeight &&
                   UnitArea == part.UnitArea &&
                   DrawingName == part.DrawingName &&
                   Count == part.Count &&
                   EqualityComparer<List<int>>.Default.Equals(Father, part.Father) &&
                   GUID.Equals(part.GUID) &&
                   Creation == part.Creation &&
                   Revise == part.Revise &&
                   State == part.State;
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 899635981;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Profile);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + t1.GetHashCode();
            hashCode = hashCode * -1521134295 + t2.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + UnitWeight.GetHashCode();
            hashCode = hashCode * -1521134295 + UnitArea.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DrawingName);
            hashCode = hashCode * -1521134295 + GUID.GetHashCode();
            return hashCode;
        }

        public void Merge(object obj)
        {
            if (obj is SteelPart steel)
            {
                if (steel.Number != this.Number)
                {
                    throw new Exception("合併物件失敗，因構件編號不相同。");
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

        public static bool operator ==(SteelPart left, SteelPart right)
        {
            return EqualityComparer<SteelPart>.Default.Equals(left, right);
        }

        public static bool operator !=(SteelPart left, SteelPart right)
        {
            return !(left == right);
        }


        #endregion
    }
}