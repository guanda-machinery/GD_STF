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
    /// 報表 Part 訊息
    /// </summary>
    [Serializable]
    public class SteelPart : ITeklaObject, IProfile, ISecondary, IModelObjectBase, IMerge, IDrawing, ITekla, IChange, ISteelProfile, ITypeSettingPartView
    {
        /// <summary>
        /// Tekla Part 訊息
        /// </summary>
        public SteelPart()
        {
            ID = new List<int>();
            Father = new List<int>();
            Creation = DateTime.Now;
            IsTekla = true;
            Count++;
            State = DRAWING_STATE.NEW;
        }

        /// <summary>
        /// 手動操作訊息
        /// </summary>
        /// <param name="profile">鋼構輪廓資訊</param>
        /// <param name="teklaName">圖名稱</param>
        /// <param name="number">零件編號</param>
        /// <param name="length">長度</param>
        /// <param name="count">數量</param>
        /// <param name="guid">GUID</param>
        /// <param name="phase">Phase</param>
        /// <param name="shippingNumber">拆運</param>
        /// <param name="title1">標題1</param>
        /// <param name="title2">標題2</param>
        /// <param name="lockMark">鎖</param>
        /// <exception cref="ArgumentException"></exception>
        public SteelPart(ISteelProfile profile, string teklaName,
            string partnumber,double length, int count, Guid guid,int? phase,int? shippingNumber,
            string title1,string title2 ,bool lockMark)
        {
            //if (string.IsNullOrWhiteSpace(number))
            //{
            //    throw new ArgumentException($"'{nameof(number)}' 不得為 Null 或空白字元。", nameof(number));
            //}
            if (count <= 0)
            {
                throw new ArgumentException($"'{nameof(count)}' 不得小於等於 0", nameof(count));
            }
            H = profile.H;
            W = profile.W;
            t1 = profile.t1;
            t2 = profile.t2;
            Profile = profile.Profile;
            Type = profile.Type;
            IsTekla = false;
            Number = partnumber;
            DrawingName = teklaName;
            Count = count;
            GUID = guid;
            Phase = phase;
            ShippingNumber = shippingNumber;
            Title2 = title2;
            Title1 = title1;
            Length= length;
            Lock = lockMark; 
            Creation = DateTime.Now;
            Revise = DateTime.Now;
        }
        #region 公開屬性
        /// <inheritdoc/>
        [TeklaBom(1)]
        public List<int> ID { get; set; }
        /// <inheritdoc/>
        [TeklaBom(2)]
        public string Number { get; set; }
        /// <inheritdoc/>
        [TeklaBom(3)]
        public string Profile { get; set; }
        /// <summary>
        /// 驚嘆號
        /// </summary>
        public bool? ExclamationMark { get; set; }
        /// <summary>
        /// 斷面規格類型
        /// </summary>
        [TeklaBom(4)]
        public OBJECT_TYPE Type { get; set; }
        /// <inheritdoc/>
        [TeklaBom(5)]
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
        public string Material { get; set; }
        /// <inheritdoc/>
        [TeklaBom(11)]
        public double UnitWeight { get; set; }
        /// <inheritdoc/>
        [TeklaBom(12)]
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        [TeklaBom(13)]
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public int Count { get; private set; }
        /// <inheritdoc/>
        public List<int> Father { get; set; }
        /// <summary>
        /// 圖檔 GUID 
        /// </summary>
        public Guid? GUID { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime Creation { get; private set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime Revise { get; set; }
        /// <summary>
        /// Phase
        /// </summary>
        public int? Phase { get; set; }
        /// <summary>
        /// 車次、拆運
        /// </summary>
        public int? ShippingNumber { get; set; }
        /// <summary>
        /// 標題1
        /// </summary>
        public String Title1 { get; set; }
        /// <summary>
        /// 標題2
        /// </summary>
        public String Title2 { get; set; }
        /// <summary>
        /// 圖面狀態
        /// </summary>
        public DRAWING_STATE State { get; set; }
        /// <summary>
        /// 是 Tekla 物件
        /// </summary>
        public bool IsTekla { get; private set; }
        /// <summary>
        /// 有 NC 文件
        /// </summary>
        public bool Nc { get; set; }
        /// <summary>
        /// 可批配料單
        /// </summary>
        /// <remarks>
        /// 可以匹配料單回傳 true，料單不行匹配則回傳 false。
        /// </remarks>
        public List<bool> Match { get; set; } = new List<bool>() { true };
        /// <summary>
        /// 物件上鎖
        /// </summary>
        public bool Lock { get; set; }
        ///// <summary>
        ///// 排序數量
        ///// </summary>
        //public int SortCount { get; set; }
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
        public void Merge(object obj)
        {
            if (obj is SteelPart steel)
            {
                if (steel.Number != this.Number) //編號判斷合併物件
                {
                    throw new Exception("合併物件失敗，因編號不相同。");
                }
                ID.AddRange(steel.ID);
                Father.AddRange(steel.Father);

                if (GUID != null) //如果有圖面 GUID 
                {
                    //如果圖紙狀態是新建或數量減少
                    if (State == DRAWING_STATE.NEW || State == DRAWING_STATE.REDUCE_COUNT)
                    {
                        State = DRAWING_STATE.NULL;
                    }
                    State = DRAWING_STATE.INCREASE_COUNT; //狀態數量新增改變
                    Revise = DateTime.Now;
                }
                else
                {

                }
                if (UnitWeight < steel.UnitWeight)//只取最重的重量
                {
                    UnitWeight = steel.UnitWeight;
                }
                Count++;
                Match.AddRange(steel.Match);
            }
            else
            {
                throw new Exception("合併物件失敗，物件 Type 不相同。");
            }
        }
        /// <summary>
        /// 變更可匹配料件數量
        /// </summary>
        /// <param name="count"></param>
        public void ChangeMatchCount(int count)
        {
            List<int> idnex = NoMatcthIndex();//沒有配料過的料單
            for (int i = 0; i < count; i++)
            {
                Match[i] = false; //改變狀態變成不可匹配
            }
        }
        /// <summary>
        /// 還未匹配料件陣列位置
        /// </summary>
        /// <returns><see cref="Match"/>[i] = false 的 i</returns>
        public List<int> NoMatcthIndex()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < Match.Count; i++) //逐步查看可以匹配的物件
            {
                if (Match[i]) //可以配料的物件
                {
                    result.Add(i); //加入到集合
                }
            }
            return result;
        }
    
        /// <inheritdoc/>
        public void Reduce(int id)
        {
            int index = ID.Find(el => el == id);//取出物件 ID 位置
            Father.Remove(index);//刪除父物件
            ID.Remove(index);//刪除物件
            Match.RemoveAt(index); //刪除匹配料件
            Count--;//物件數量 -1 
            State = DRAWING_STATE.REDUCE_COUNT; //改變狀態為減少數量
        }
        /// <inheritdoc/>
        public void Update(object obj)
        {
            if (obj is SteelPart steel)
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
                Father = steel.Father;//賦予跟新後的父 ID
                ID = steel.ID;//賦予跟新後的 ID
                Length = steel.Length;//賦予跟新後的長度
                UnitWeight = steel.UnitWeight;//賦予跟新後的單件重
                UnitArea = steel.UnitArea;//賦予跟新後的單件面積
            }
            else
            {
                throw new Exception("更新物件失敗，物件 Type 不相同。");
            }
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1258444329;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Profile);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + t1.GetHashCode();
            hashCode = hashCode * -1521134295 + t2.GetHashCode();
            return hashCode;
        }
        /// <summary>
        /// 已構件 ID 找出 子物件的 ID 
        /// </summary>
        /// <param name="father">構件 ID 集合</param>
        /// <returns></returns>
        public List<int> PartID(List<int> father)
        {
            List<int> result = new List<int>();
            Parallel.For(0, father.Count, i => 
            {
                int index = this.ID.IndexOf(father[i]);
                if (index != -1) //找到構件的零件
                {
                    result.Add(ID[index]);
                }
                else //找不到夠件的零件
                {
                    throw new Exception("找不到物件索引");
                }
            });
            return result;
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