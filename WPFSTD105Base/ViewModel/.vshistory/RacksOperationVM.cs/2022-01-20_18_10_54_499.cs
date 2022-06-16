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
        /// 入口移動料架上升或下降數量
        /// </summary>
        private short _EntranceRiseCount = 2;
        /// <summary>
        /// 出口移動料架上升或下降數量
        /// </summary>
        private short _ExportRiseCount = 2;
        /// <summary>
        /// 出口移動料架上升或下降，控制開關。 
        /// </summary>
        private bool _EntranceRisePowr = false;
        /// <summary>
        /// 出口移動料架上升或下降，控制開關。 
        /// </summary>
        private bool _ExportRisePowr = false;

        /// <summary>
        /// 移動料架上升或下降數量
        /// </summary>
        /// <remarks>
        /// 最值是 2，最大 8
        /// </remarks>
        public short RackRiseCount
        {
            get => SelectedRacks == RACKS_SELECTED.Entrance ? _EntranceRiseCount : _ExportRiseCount;
            set
            {
                if (SelectedRacks == RACKS_SELECTED.Entrance)
                {
                    _EntranceRiseCount = value;
                }
                else
                {
                    _ExportRiseCount = value;
                }
            }
        }
        /// <summary>
        /// 移動料架上升或下降，控制開關。 (綁定VM)
        /// </summary>
        public bool RisePowr
        {
            get => SelectedRacks == RACKS_SELECTED.Entrance ? _EntranceRisePowr : _ExportRisePowr;
            set
            {
                if (SelectedRacks == RACKS_SELECTED.Entrance)
                {
                    _EntranceRisePowr = value;
                }
                else
                {
                    _ExportRisePowr = value;
                }
            }
        }
        /// <summary>
        /// 橫移料架移動，控制開關
        /// </summary>
        public bool MovePowr { get; set; }
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
