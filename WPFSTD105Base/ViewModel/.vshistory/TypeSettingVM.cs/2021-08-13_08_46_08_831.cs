using GD_STD;
using GD_STD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using WPFSTD105.Properties;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 排版設定
    /// </summary>
    public class TypeSettingVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 註冊命令
        /// </summary>
        public TypeSettingVM()
        {

        }
        
        /// <summary>
        /// 零件過濾器資料容器
        /// </summary>
        public static ObservableCollection<TreeNode> parts = new ObservableCollection<TreeNode>()
        {
            new TreeNode() {
                ItemName="H型鋼(SN490B)", NodeID=4, Children=new ObservableCollection<TreeNode>()
                {
                    new TreeNode() { ItemName="H300*150" },
                    new TreeNode() { ItemName="H350*175" },
                    new TreeNode() { ItemName="H300*300" },
                    new TreeNode() { ItemName="H390*300" }
                }
            },
            new TreeNode() { ItemName="H型鋼(SN400B)", NodeID=0 },
            new TreeNode() {
                ItemName="槽鋼CH", NodeID=2, Children=new ObservableCollection<TreeNode>()
                {
                    new TreeNode() { ItemName="CH115*75" },
                    new TreeNode() { ItemName="CH200*90" }
                }
            },
            new TreeNode() { ItemName="方管&扁管", NodeID=0 }
        };
        /// <summary>
        /// 零件過濾器資料容器
        /// </summary>
        public static Dictionary<string, ObservableCollection<TreeNode>> partsFilter = new Dictionary<string, ObservableCollection<TreeNode>>()
        {
            { "斷面規格", parts }
        };

        /// <summary>
        /// 組合件過濾器資料容器
        /// </summary>
        public static ObservableCollection<TreeNode> assemble = new ObservableCollection<TreeNode>()
        {
            new TreeNode() {
                ItemName="H型鋼(SN490B)", NodeID=4, Children=new ObservableCollection<TreeNode>()
                {
                    new TreeNode() { ItemName="H300*150" },
                    new TreeNode() { ItemName="H350*175" },
                    new TreeNode() { ItemName="H300*300" },
                    new TreeNode() { ItemName="H390*300" }
                }
            },
            new TreeNode() { ItemName="H型鋼(SN400B)", NodeID=0 },
            new TreeNode() {
                ItemName="槽鋼CH", NodeID=2, Children=new ObservableCollection<TreeNode>()
                {
                    new TreeNode() { ItemName="CH115*75" },
                    new TreeNode() { ItemName="CH200*90" }
                }
            },
            new TreeNode() { ItemName="方管&扁管", NodeID=0 }
        };
        /// <summary>
        /// 組合過濾器資料容器
        /// </summary>
        public static Dictionary<string, ObservableCollection<TreeNode>> AssembleFilter = new Dictionary<string, ObservableCollection<TreeNode>>()
        {
            { "斷面規格", assemble }
        };
    }
}
