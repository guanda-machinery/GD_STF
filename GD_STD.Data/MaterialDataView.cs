using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class ExcelAttribute : Attribute
    {
        public ExcelAttribute(string columnName, int index)
        {
            ColumnName=columnName;
            Index=index;
        }

        public string ColumnName { get; }
        public int Index { get; }
    }
    /// <summary>
    /// 素材組合
    /// </summary>
    [Serializable]
    public class MaterialDataView : WPFWindowsBase.BaseViewModel, IProfile, IMatchSetting
    {
        private double lengthStr;

        /// <summary>
        /// 長度列表
        /// </summary>
        public ObservableCollection<double> LengthList { get; set; } = new ObservableCollection<double>();
        /// <summary>
        /// 來源列表
        /// </summary>
        public string Sources { get; set; } = string.Empty;
        /// <summary>
        /// 素材編號
        /// </summary>
        [Excel("素材編號", 0)]
        public string MaterialNumber { get; set; }
        /// <inheritdoc/>
        [Excel("斷面規格", 1)]
        public string Profile { get; set; }
        /// <summary>
        /// <see cref="LengthList"/> 索引值
        /// </summary>
        public int LengthIndex { get; set; }
        /// <summary>
        /// 購料長
        /// </summary>
        [Excel("購料長", 3)]
        public double LengthStr { get; set; }
        //{
        //    get => lengthStr;
        //    set
        //    {
        //        if (lengthStr != value)
        //        {
        //            int index = LengthList.IndexOf(lengthStr);
        //            if (index >=0)
        //            {
        //                LengthList[index] = value;
        //            }
        //        }
        //        lengthStr =value;
        //    }
        //}
        /// <summary>
        /// 材質
        /// </summary>
        [Excel("材質", 2)]
        public string Material { get; set; }
        /// <summary>
        /// <see cref="Sources"/> 索引值
        /// </summary>
        [Excel("廠商", 4)]
        public int SourceIndex { get; set; }
        /// <summary>
        /// 零件列表
        /// </summary>
        public ObservableCollection<TypeSettingDataView> Parts { get; set; } = new ObservableCollection<TypeSettingDataView>();
        /// <inheritdoc/>
        public double StartCut { get; set; }
        /// <inheritdoc/>
        public double EndCut { get; set; }
        /// <inheritdoc/>
        public double Cut { get; set; }
        /// <summary>
        /// 完成進度
        /// </summary>
        public double Schedule { get; set; }
        /// <summary>
        /// 完成
        /// </summary>
        public bool Finish { get; set; }
        /// <summary>
        /// 物件座標
        /// </summary>
        public float CurrentCoordinate { get; set; }
        private string _position;
        /// <summary>
        /// 目前位置
        /// </summary>        
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
        /// <summary>
        /// 使用長度
        /// </summary>
        [Excel("損耗", 5)]
        public double Loss { get => Parts.Select(el => el.Length).Aggregate((l1, l2) => l1 + l2)  + Cut * (Parts.Count -1d > 0d ? Parts.Count -1d : 0d); }
    }
}
