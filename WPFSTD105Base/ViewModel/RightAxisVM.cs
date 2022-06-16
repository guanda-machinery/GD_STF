using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 右軸VM
    /// </summary>
    public class RightAxisVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public RightAxisVM() : base()
        {
            CircleTopButtonDESC = "切換左軸";
            CircleMiddleButtonDESC = "切換中軸";
            JoystickUpDESC = "X 軸 –";
            JoystickDownDESC = "X 軸 ＋";
            JoystickLeftDESC = "Z 軸 ＋";
            JoystickRightDESC = "Z 軸 –";
            EllipseTopButtonDESC = "Y 軸 ＋";
            EllipseBottomButtonDESC = "Y 軸 –";
        }    
    }
}
