using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD
{
    /// <summary>
    /// 鑽頭品牌
    /// </summary>
    [Serializable]
    public class DrillBrand : WPFWindowsBase.BaseViewModel, GD_STD.Base.IDrillParameter
    {
        private float drillLength;
        private float holderLength;
        private double _f;
        private double vc;
        private double _F;
        private double rpm;

        /// <summary>
        /// 品牌名稱
        /// </summary>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double Dia { get; set; }
        /// <inheritdoc/>
        public DRILL_TYPE DrillType { get; set; }
        ///// <inheritdoc/>
        //public float FeedQuantity { get; set; }
        /// <inheritdoc/>
        public double Rpm { get => rpm; set => rpm = value; }
        /// <summary>
        /// 設定檔名稱
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 切消速度
        /// </summary>
        public double Vc
        {
            get => vc;
            set
            {
                vc = value;
                rpm = (vc * 1000) / (Math.PI * Dia);
            }
        }
        /// <summary>
        /// 每一轉進己量
        /// </summary>
        public double f
        {
            get => _f;
            set
            {
                _f = value;
                F = rpm * _f;
            }
        }
        /// <summary>
        /// 進己速度
        /// </summary>
        public double F
        {
            get => _F;
            set
            {
                _F = value;
                _f = _F / rpm;
            }
        }
        /// <summary>
        /// 鑽頭長度
        /// </summary>
        public float DrillLength
        {
            get
            {
                return drillLength;
            }
            set
            {
                drillLength = value;
                SumLength = value + holderLength;
            }
        }
        /// <summary>
        /// 刀柄長度
        /// </summary>
        public float HolderLength
        {
            get
            {
                return holderLength;
            }
            set
            {
                holderLength = value;
                SumLength = value + drillLength;
            }
        }
        /// <summary>
        /// <see cref="HolderLength"/> + <see cref="DrillLength"/> 長度
        /// </summary>
        public float SumLength { get; set; }
    }
    /// <summary>
    /// <see cref="DrillBrand"/>動態集合列表
    /// </summary>
    public class DrillBrands : ObservableCollection<DrillBrand>
    {

    }
}
