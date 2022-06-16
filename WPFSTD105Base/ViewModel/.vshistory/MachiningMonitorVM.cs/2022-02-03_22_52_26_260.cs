using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控
    /// </summary>
    public class MachiningMonitorVM : WPFWindowsBase.BaseViewModel
    {
       
        /// <summary>
        /// 配料清單
        /// </summary>
       public ObservableCollection<MaterialDataView> MaterialDataView { get; set; }

    }
}
