using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace GD_STD.Data
{
    /// <summary>
    /// 專案屬性
    /// </summary>
    [Serializable]
    public class ProjectProperty : BaseViewModel
    {
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
    }
}
