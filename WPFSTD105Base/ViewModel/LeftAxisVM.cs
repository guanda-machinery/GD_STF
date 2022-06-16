using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 左軸頁面VM
    /// </summary>
    public class LeftAxisVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public LeftAxisVM():base()
        {
            CircleMiddleButtonDESC = "切換中軸";
            CircleBottomButtonDESC = "切換右軸";
            JoystickUpDESC = "X 軸 –";
            JoystickDownDESC = "X 軸 ＋";
            JoystickLeftDESC = "Z 軸 –";
            JoystickRightDESC = "Z 軸 ＋";
            EllipseTopButtonDESC = "Y 軸 ＋";
            EllipseBottomButtonDESC = "Y 軸 –";
        }
    }
}
