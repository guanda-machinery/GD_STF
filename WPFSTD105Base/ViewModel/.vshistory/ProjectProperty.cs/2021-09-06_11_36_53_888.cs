using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 專案屬性
    /// </summary>
    [Serializable]
    public class ProjectProperty : BaseViewModel
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ProjectProperty()
        {
            BomProperties = new ObservableCollection<BomProperty>();
        }
        /// <summary>
        /// 工程號碼
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 報表屬性設定
        /// </summary>
        public ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <summary>
        /// 有加載 nc 檔與報表
        /// </summary>
        public bool Load { get; set; }
        /// <summary>
        /// 專案建置日期
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 設計人員名稱
        /// </summary>
        public string Design { get; set; }
        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location { get; set; }
    }
}
