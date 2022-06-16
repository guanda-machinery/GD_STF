//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WPFWindowsBase;

//namespace WPFSTD105.ViewModel
//{
//    /// <summary>
//    /// 加工監控VM
//    /// </summary>
//    public class ProcessingMonitorVM : BaseViewModel
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public ObservableCollection<FakeData> fakeData { get; set; } = new ObservableCollection<FakeData>() 
//        { 
//            new FakeData() { PartNumber="B-CS44", ProcessingNumber="B-C44"} 
//        };
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public class FakeData
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string ProcessingNumber { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public string PartNumber { get; set; } 
//    }
//}
