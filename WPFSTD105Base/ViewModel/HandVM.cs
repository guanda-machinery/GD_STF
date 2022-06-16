using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 自動手臂VM
    /// </summary>
    public class HandVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public HandVM() : base()
        {
            CircleTopButtonDESC = "X 軸";
            CircleMiddleButtonDESC = "夾爪";
            CircleBottomButtonDESC = "翼板夾爪";
            JoystickLeftDESC = "手臂移動正向";
            JoystickUpDESC = "手臂往上";
            JoystickDownDESC = "手臂往下";
            EllipseTopButtonDESC = "取消手臂夾取";
            EllipseBottomButtonDESC = "手臂夾取";
        }
    }
}
