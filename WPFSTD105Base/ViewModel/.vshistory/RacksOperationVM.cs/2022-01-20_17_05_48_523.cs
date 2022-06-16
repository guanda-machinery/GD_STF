using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 設備料架 
    /// </summary>
    public class RacksOperationVM : WPFWindowsBase.BaseViewModel
    {
        /// <summary>
        /// 移動料架上升或下降數量
        /// </summary>
        /// <remarks>
        /// 最值是 2，最大 8
        /// </remarks>
        public int RackRiseCount { get; set; }
        /// <summary>
        /// 移動料架上升或下降，控制開關。
        /// </summary>
        public bool RackRisePowr { get; set; }
        /// <summary>
        /// 橫移料架移動，控制開關
        /// </summary>
        public bool RackMovePowr { get; set; }
        /// <summary>
        /// 用戶選擇的料架
        /// </summary>
        public RACKS_SELECTED SelectedRacks { get; set; }
    }
    /// <summary>
    /// 料架
    /// </summary>
    public enum RACKS_SELECTED
    {
        /// <summary>
        /// 入口
        /// </summary>
        Entrance,
        /// <summary>
        /// 出口
        /// </summary>
        Export,
    }
}
