using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
//using WPFSTD105;
//using WPFSTD105.Attribute;
using GD_STD;
using DevExpress.Xpf.Grid;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GD_STD.Data
{
    /// <summary>
    /// 排版設定資料視圖
    /// </summary>
    [Serializable]
    public class TypeSettingDataView : WPFWindowsBase.BaseViewModel, ITypeSettingPartView, ITypeSettingAssemblyView, ITypeSettingDataView
    {
        public TypeSettingDataView()
        { }

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="steelPart">零件參數</param>
        /// <param name="steelAssembly">構件資訊</param>
        /// <param name="assemblyIndex">構件 ID 陣列位置</param>
        /// <param name="partIndex">零件 ID 陣列位置</param>
        public TypeSettingDataView(SteelPart steelPart, SteelAssembly steelAssembly, int assemblyIndex, int partIndex)
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
            ID = new List<int>() { steelPart.ID[partIndex] };
            Match = new List<bool>() { steelPart.Match[partIndex] };
            //1114 CYH
            //ShippingNumber = steelAssembly.ShippingNumber[assemblyIndex];
            //Phase = steelAssembly.Phase[assemblyIndex];
            //ShippingDescription = steelAssembly.ShippingDescription[assemblyIndex];
            SteelType = Convert.ToInt32(steelPart.Type); //型鋼型態 20220831 張燕華
            H = steelPart.H;
            W = steelPart.W;
            t1 = steelPart.t1;
            t2 = steelPart.t2;
            PartWeight = steelPart.UnitWeight * Length; //零件重 20220901 張燕華

            Count = ID.Count;
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

        private int _Count;
        ///// <summary>
        ///// 物件數量
        ///// </summary>
        public int Count { get => ID.Count; set=> _Count = value; }
        /// <summary>
        /// 型鋼型態 20220825 張燕華
        /// </summary>
        public int SteelType { get; set; }
        /// <summary>
        /// 高度 20220826 張燕華
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// 寬度 20220826 張燕華
        /// </summary>
        public float W { get; set; }
        /// <summary>
        /// s/t1 20220826 張燕華
        /// </summary>
        public float t1 { get; set; }
        /// <summary>
        /// t/t2 20220826 張燕華
        /// </summary>
        public float t2 { get; set; }
        /// <summary>
        /// 零件重量 20220901 張燕華
        /// </summary>
        public double PartWeight { get; set; }
        /// <summary>
        /// 標題一 20220901 張燕華
        /// </summary>
        public string Title1 { get; set; }
        /// <summary>
        /// 標題二 20220901 張燕華
        /// </summary>
        public string Title2 { get; set; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        public string ShippingDescription { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        [Excel("車次", 5)]
        public int ShippingNumber { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        [Excel("Phase", 4)]
        public int Phase { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        [Excel("零件編號", 2)]
        public string PartNumber { get => ((ITypeSettingPartView)this).Number; set => ((ITypeSettingPartView)this).Number = value; }
        /// <summary>
        /// 構件編號
        /// </summary>
        [Excel("構件編號", 1)]
        public string AssemblyNumber { get => ((ITypeSettingAssemblyView)this).Number; set => ((ITypeSettingAssemblyView)this).Number = value; }
        /// <inheritdoc/>
        public DateTime Creation { get; private set; }
        /// <inheritdoc/>
        public List<int> ID { get; set; }
        /// <summary>
        /// 可批配料單
        /// </summary>
        public List<bool> Match { get; set; }
        /// <inheritdoc/>
        [Excel("長度", 3)]
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

        private int _alreadyMatedMaterialCounts;
        public int AlreadyMatedMaterialCounts
        {
            get 
            {
                _alreadyMatedMaterialCounts = Match.FindAll(x => x is false).Count;
                return _alreadyMatedMaterialCounts;
            }
            set
            {
                _alreadyMatedMaterialCounts = value;
            }
        }


        /// <summary>
        /// 排版數量
        /// </summary>
        public int SortCount { get; set; }
        /// <inheritdoc/>
        string ITypeSettingAssemblyView.Number { get; set; }
        /// <inheritdoc/>
        string ITypeSettingPartView.Number { get; set; }
        public int GetSortCount()
        {
            return Match.Count() - Match.Where(e => e == false).Count();
        }

        /// <summary>
        /// 工件重量
        /// </summary>
        public double Weigth { get; set; }

        /// <summary>
        /// 工件類型
        /// </summary>
        public GD_STD.Enum.OBJECT_TYPE PartType { get; set; }

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

        //public static explicit operator TypeSettingDataView(SelectedItemChangedEventArgs v)
        //{
        //    throw new NotImplementedException();
        //}

        public object DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                //序列化物件格式
                IFormatter formatter = new BinaryFormatter();
                //將自己所有資料序列化
                formatter.Serialize(objectStream, this);
                //複寫資料流位置，返回最前端
                objectStream.Seek(0, SeekOrigin.Begin);
                //再將objectStream反序列化回去 
                return formatter.Deserialize(objectStream) as TypeSettingDataView;
            }
        }
    }
}
