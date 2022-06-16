using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 中軸ＶＭ
    /// </summary>
    public class MiddleAxisVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public MiddleAxisVM() : base()
        {
            CircleTopButtonDESC = "切換左軸";
            CircleBottomButtonDESC = "切換右軸";
            JoystickUpDESC = "X 軸 –";
            JoystickDownDESC = "X 軸 ＋";
            JoystickLeftDESC = "Y 軸 ＋";
            JoystickRightDESC = "Y 軸 –";
            EllipseTopButtonDESC = "Z 軸 –";
            EllipseBottomButtonDESC = "Z 軸 ＋";
        }
    }
}
