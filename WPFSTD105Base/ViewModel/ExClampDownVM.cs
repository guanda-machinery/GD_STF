using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 側壓
    /// </summary>
    public class ExClampDownVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ExClampDownVM() : base()
        {
            JoystickLeftDESC = "往原點移動。兩下直接回原點。";
            JoystickRightDESC = "啟動夾料。兩下直接夾到底。";
        }
     
    }
}
