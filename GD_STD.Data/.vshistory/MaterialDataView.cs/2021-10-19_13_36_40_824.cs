using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 素材組合
    /// </summary>
    [Serializable]
    public class MaterialDataView : WPFWindowsBase.BaseViewModel, IProfile
    {
        /// <summary>
        /// 長度列表
        /// </summary>
        public ObservableCollection<double> LengthList { get; set; } = new ObservableCollection<double>();
        /// <summary>
        /// 來源列表
        /// </summary>
        public ObservableCollection<string> Sources { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// 素材編號
        /// </summary>
        public string MaterialNumber { get; set; }
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <summary>
        /// <see cref="LengthList"/> 索引值
        /// </summary>
        public double LengthIndex { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        public string Material { get; set; }
        /// <summary>
        /// <see cref="Sources"/> 索引值
        /// </summary>
        public int SourceIndex { get; set; }
        /// <summary>
        /// 零件列表
        /// </summary>
        public ObservableCollection<TypeSettingDataView> Parts { get; set; } = new ObservableCollection<TypeSettingDataView>();
    }
}
