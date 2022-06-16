using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// 
    /// </summary>
    public class OfficePageVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 辦公室當前頁面
        /// </summary>
        public OfficePage CurrentPage { get; set; } = OfficePage.Home;
    }
}
