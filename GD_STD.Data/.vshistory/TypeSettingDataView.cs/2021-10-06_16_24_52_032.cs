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
    public class TypeSettingDataView : ITypeSettingPartView, ITypeSettingAssemblyView
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
            //ID = partView.ID;
            ShippingNumber = steelAssembly.ShippingNumber[index];
            Phase = steelAssembly.Phase[index];
            ShippingDescription = steelAssembly.ShippingDescription[index];
            Count = steelAssembly.ID.Intersect(steelPart.Father).Count();//取得相同物件數
           
        }
        /// <summary>
        /// 物件數量
        /// </summary>
        public int Count { get; set; }
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
        public string Phase { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        public string PartNumber { get => ((ITypeSettingPartView)this).Number; set => ((ITypeSettingPartView)this).Number = value; }
        /// <summary>
        /// 構件編號
        /// </summary>
        public string AssemblyNumber { get => ((ITypeSettingAssemblyView)this).Number; set => ((ITypeSettingAssemblyView)this).Number = value; }
        /// <inheritdoc/>
        public DateTime Creation { get; }
        /// <inheritdoc/>
        public List<int> Father { get; set; }
        /// <inheritdoc/>
        public List<int> ID { get; set; }
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
    }
}
