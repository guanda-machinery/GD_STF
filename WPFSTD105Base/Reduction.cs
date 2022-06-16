using devDept.Eyeshot.Entities;
using System.Collections.Generic;

namespace WPFSTD105
{
    /// <summary>
    /// 還原物件
    /// </summary>
    public class Reduction
    {
        /// <summary>
        /// 選擇的參考塊
        /// </summary>
        public BlockReference SelectReference { get; set; }
        /// <summary>
        /// 編輯過的物件列表
        /// </summary>
        public List<List<Entity>> Recycle { get; set; } = new List<List<Entity>>();
        /// <summary>
        /// 動作列表
        /// </summary>
        public List<ACTION_USER> User { get; set; } = new List<ACTION_USER>();

    }
}
