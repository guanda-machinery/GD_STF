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
    public class ProductSettingsPageViewModel : BaseViewModel
    {
        public SteelAttr steelAttr { get; set; }
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
        public double PieceWeight { get; set; }
        /// <summary>
        /// 零件長
        /// </summary>
        public double PieceLength { get; set; }
        /// <summary>
        /// Phase
        /// </summary>
        public string Phase { get; set; }
        /// <summary>
        /// 拆運
        /// </summary>
        string ShippingDescription { get; set; }

        public ProductSettingsPageViewModel()
        {
            steelAttr = new SteelAttr();
        }

    }
}
