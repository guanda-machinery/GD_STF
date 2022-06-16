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
    public class SteelBolts : Base.IWeight, IProfile, IDrawing, IMerge, ITekla, IChange, ISecondary
    {
        public SteelBolts()
        {
            Creation = DateTime.Now;
            IsTekla = true;
            State = DRAWING_STATE.NEW;
            Father = new List<int>();
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
        public int Count { get; private set; }
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
        /// <summary>
        /// 主件
        /// </summary>
        public List<int> Father { get; set; }
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
            int hashCode = -1795420834;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Profile);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Material);
            return hashCode;
        }

        /// <inheritdoc/>
        public void Merge(object obj)
        {
            if (obj is SteelBolts bolts)
            {
                if (bolts.Profile != Profile) //以斷面規格判斷合併物件
                {
                    throw new Exception("合併物件失敗，因斷面不相同。");
                }
                Count += bolts.Count;
                if (UnitWeight < bolts.UnitWeight)//只取最重的重量
                {
                    UnitWeight = bolts.UnitWeight;
                }
                Father.AddRange(bolts.Father);
            }
            else
            {
                throw new Exception("合併物件失敗，物件 Type 不相同。");
            }
        }
        /// <summary>
        /// 相減
        /// </summary>
        /// <param name="count">要減取的數量</param>
        public void Reduce(int count)
        {
            Count -= count; //減掉目前數量
            
            if (Count < 0)//如果數量小於0
            {
                Count = 0; //不可以賦予它負數
                State = DRAWING_STATE.DELETE; //如果數量變 0 狀態就改變變成刪除物件
            }
            else //如果非 0 
            {
                State = DRAWING_STATE.REDUCE_COUNT; //就數量減少
            }
        }

        /// <inheritdoc/>
        public double TotalWeight() => UnitWeight * Count;
        /// <inheritdoc/>
        public void Update(object obj)
        {
            if (obj is SteelBolts steel)
            {
                if (Count > steel.Count) //如果當前數量大於更新數量
                {
                    State = DRAWING_STATE.INCREASE_COUNT; //就改變狀態為新增數量
                }
                else //如果當前數量小於更新數量
                {
                    State = DRAWING_STATE.INCREASE_COUNT;//就改變狀態為減少數量
                }
                Count = steel.Count; //賦予跟新後的數量
            }
            else
            {
                throw new Exception("更新物件失敗，物件 Type 不相同。");
            }
        }

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
