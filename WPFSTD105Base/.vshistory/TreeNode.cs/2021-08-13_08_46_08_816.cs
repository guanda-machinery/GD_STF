using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// 樹狀節點
    /// </summary>
    public class TreeNode : IModelData
    {
        public int NodeID { get; set; }
        /// <summary>
        /// 節點名稱
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 節點的子節點
        /// </summary>
        public ObservableCollection<TreeNode> Children { get; set; }
        /// <inheritdoc/>
        public string DataName { get; set; }
    }
}
