using GD_STD.Data;
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
    /// 加工監控VM
    /// </summary>
    public class ProcessingMonitorVM : BaseViewModel
    {
        /// <summary>
        /// 配料清單
        /// </summary>
        public ObservableCollection<MaterialDataView> DataView { get; set; }
        /// <summary>
        /// 建構式
        /// </summary>
        public ProcessingMonitorVM()
        {
            STDSerialization ser = new STDSerialization();
            DataView = ser.GetMaterialDataView();
        }
    }
}
