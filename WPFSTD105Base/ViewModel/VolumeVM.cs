using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 捲屑機
    /// </summary>
    public class VolumeVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public VolumeVM() : base()
        {
            JoystickLeftDESC = "逆轉";
            JoystickRightDESC = "正轉";
        }
    }
}
