using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 排版設定資料視圖
    /// </summary>
    public class TypeSettingDataView : ITypeSettingPartView, ITypeSettingAssemblyView, ITypeSettingDataView
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="steelPart">零件參數</param>
        /// <param name="steelAssembly">構件資訊</param>
        /// <param name="index">陣列位置</param>
        public TypeSettingDataView(SteelPart steelPart, SteelAssembly steelAssembly, int index)
        {
            ((ITypeSettingPartView)this).Number = steelPart.Number;
            ((ITypeSettingAssemblyView)this).Number = steelAssembly.Number;
            Creation = steelPart.Creation;
            Length = steelPart.Length;
            Lock = steelPart.Lock;
            Material = steelPart.Material;
            Profile = steelPart.Profile;
            Revise = steelPart.Revise;
            State = steelPart.State;
            //ID = steelPart.ID;
            ShippingNumber = steelAssembly.ShippingNumber[index];
            Phase = steelAssembly.Phase[index];
            ShippingDescription = steelAssembly.ShippingDescription[index];
            //Count = ID.Count;
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is TypeSettingDataView value)
            {
                return ShippingNumber == value.ShippingNumber &&
                           Phase == value.Phase &&
                           PartNumber == value.PartNumber &&
                           AssemblyNumber == value.AssemblyNumber &&
                           Material == value.Material &&
                           Profile == value.Profile;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 加入物件
        /// </summary>
        /// <param name="part"></param>
        /// <param name="index"><see cref="SteelPart.ID"/> 陣列位置</param>
        public void Add(SteelPart part, int index)
        {
            ID.Add(part.ID[index]); //加入零件 id
            Match.Add(part.Match[index]); //加入批配料單參數
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -80396427;
            hashCode = hashCode * -1521134295 + ShippingNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Phase.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PartNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AssemblyNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Material);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Profile);
            return hashCode;
        }
        //private int _Count;
        ///// <summary>
        ///// 物件數量
        ///// </summary>
        //public int Count { get => ID.Count; set=> _Count = value; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        public string ShippingDescription { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        public int ShippingNumber { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        public int Phase { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        public string PartNumber { get => ((ITypeSettingPartView)this).Number; set => ((ITypeSettingPartView)this).Number = value; }
        /// <summary>
        /// 構件編號
        /// </summary>
        public string AssemblyNumber { get => ((ITypeSettingAssemblyView)this).Number; set => ((ITypeSettingAssemblyView)this).Number = value; }
        /// <inheritdoc/>
        public DateTime Creation { get; private set; }
        /// <inheritdoc/>
        public List<int> ID { get; set; } = new List<int>();
        /// <summary>
        /// 可批配料單
        /// </summary>
        public List<bool> Match { get; set; } = new List<bool>();
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public bool Lock { get; set; }
        /// <inheritdoc/>
        public string Material { get; set; }
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
        /// <inheritdoc/>
        public DRAWING_STATE State { get; set; }
        /// <inheritdoc/>
        string ITypeSettingAssemblyView.Number { get; set; }
        /// <inheritdoc/>
        string ITypeSettingPartView.Number { get; set; }
        /// <summary>
        ///  物件相同
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TypeSettingDataView left, TypeSettingDataView right)
        {
            return EqualityComparer<TypeSettingDataView>.Default.Equals(left, right);
        }
        /// <summary>
        /// 物件不相同
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TypeSettingDataView left, TypeSettingDataView right)
        {
            return !(left == right);
        }
    }
}
