using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 夾具側壓
    /// </summary>
    public class SideClampVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public SideClampVM() : base()
        {
            JoystickLeftDESC = "側壓復歸";
            JoystickRightDESC = "側壓夾取";
        }   
    }
}
