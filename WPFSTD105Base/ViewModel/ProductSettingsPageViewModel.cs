using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductSettingsPageViewModel : BaseViewModel
    {
        /// <summary>
        /// 驚嘆號
        /// </summary>
        public bool? ExclamationMark { 
            get
            {
                return steelAttr.ExclamationMark;

            } set
            {

                steelAttr.ExclamationMark = value;
            }
        } 
        public SteelAttr steelAttr { get; set; }
        /// <summary>
        /// DataName dm檔
        /// </summary>
        public String DataName { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime Creation { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? Revise { get; set; }
        public String TeklaAssemblyID { get; set; }
        //public String PartNumber { get; set; }
        //public String TeklaPartID { get; set; }
        public String Profile { get; set; }
        public String Material { get; set; }

        public String TeklaName { get; set; }
        /// <summary>
        /// 構件編號
        /// </summary>
        public String AssemblyNumber { get; set; }
        /// <summary>
        /// 鋼構類型
        /// </summary>
        public OBJECT_TYPE Type { get; set; }
        public int SteelType { get; set; }
        /// <summary>
        /// 鋼構類型名稱
        /// </summary>
        public String TypeDesc { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public double Count { get; set; }
        /// <summary>
        /// 零件重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 零件長
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Phase
        /// </summary>
        public int? Phase { get; set; } = null;
        /// <summary>
        /// 拆運
        /// </summary>
        public int? ShippingNumber { get; set; } = null;
        /// <summary>
        /// 標題1
        /// </summary>
        public string Title1 { get; set; }
        /// <summary>
        /// 標題2
        /// </summary>
        public string Title2 { get; set; }
        /// <summary>
        /// 腹板厚度
        /// </summary>
        public double t1 { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        public double t2 { get; set; }

        public ProductSettingsPageViewModel()
        {
            steelAttr = new SteelAttr();
        }


    }
}
