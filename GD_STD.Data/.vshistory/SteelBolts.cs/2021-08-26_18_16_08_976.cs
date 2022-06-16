using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 報表螺栓
    /// </summary>
    [Serializable]
    public class SteelBolts : Base.IWeight, IProfile, IDrawing, IMerge, ITekla
    {
        public SteelBolts()
        {
            Creation = DateTime.Now;
            IsTekla = true;
        }
        #region 公開屬性
        [TeklaBom(1)]
        public string Profile { get; set; }
        /// <summary>
        /// 螺栓類型
        /// </summary>
        [TeklaBom(2)]
        public BOLT_TYPE Type { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        [TeklaBom(3)]
        public int Count { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        [TeklaBom(4)]
        public string Material { get; set; }
        /// <inheritdoc/>
        [TeklaBom(5)]
        public double UnitWeight { get; set; }
        /// <inheritdoc/>
        public DateTime Creation { get; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
        /// <inheritdoc/>
        public DRAWING_STATE State { get; set; }
        /// <inheritdoc/>
        public bool IsTekla { get; private set; }
        #endregion

        #region 公開方法
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SteelBolts bolts &&
                   Profile == bolts.Profile &&
                   Type == bolts.Type &&
                   Material == bolts.Material &&
                   UnitWeight == bolts.UnitWeight &&
                   Count == bolts.Count &&
                   Creation == bolts.Creation &&
                   Revise == bolts.Revise &&
                   State == bolts.State;
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -665378563;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Profile);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Material);
            hashCode = hashCode * -1521134295 + UnitWeight.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + Creation.GetHashCode();
            hashCode = hashCode * -1521134295 + Revise.GetHashCode();
            hashCode = hashCode * -1521134295 + State.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public void Merge(object obj)
        {
            if (obj is SteelBolts steel)
            {
                if (steel.Profile == Profile)
                {
                    throw new Exception("合併物件失敗，因編號不相同。");
                }
                Count += steel.Count;
            }
        }
        /// <inheritdoc/>
        [Obsolete("不能使用此方法", true)]
        public void Reduce(int id)
        {

        }

        /// <inheritdoc/>
        public double TotalWeight() => UnitWeight * Count;

        public static bool operator ==(SteelBolts left, SteelBolts right)
        {
            return EqualityComparer<SteelBolts>.Default.Equals(left, right);
        }

        public static bool operator !=(SteelBolts left, SteelBolts right)
        {
            return !(left == right);
        }
        #endregion
    }
}
