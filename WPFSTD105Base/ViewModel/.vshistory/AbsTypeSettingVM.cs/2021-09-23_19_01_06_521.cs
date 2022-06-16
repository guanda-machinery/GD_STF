using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using GD_STD.Data;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 抽象排版設定
    /// </summary>
    public abstract class AbsTypeSettingVM : WPFBase.BaseViewModel
    {

        public AbsTypeSettingVM()
        {

        }
        #region 命令
        /// <summary>
        /// 手動排版命令
        /// </summary>
        public ICommand ManualCommand { get; set; }

        /// <summary>
        /// 自動排版命令
        /// </summary>
        public ICommand AutoCommand { get; set; }
        /// <summary>
        /// 選擇報表全部物件命令
        /// </summary>
        public ICommand AllSelectedGridCommand { get; set; }
        /// <summary>
        /// 反向選取命令
        /// </summary>
        public ICommand ReverseSelectedGridCommand { get; set; }
        /// <summary>
        /// 取消選取命令
        /// </summary>
        public ICommand CloseSelectedGridCommand { get; set; }
        /// <summary>
        /// 顯示全部列表命令 (清除過濾器)
        /// </summary>
        public ICommand DisplayAllListCommand { get; set; }
        /// <summary>
        /// 顯示未完成物件的列表命令 (只顯示未完成的過濾器)
        /// </summary>
        public ICommand DisplayUndoneCommand { get; set; }
        /// <summary>
        /// 顯示已完成的物件列表命令 (只顯示完成的過濾器)
        /// </summary>
        public ICommand DisplayFinishCommand { get; set; }
        /// <summary>
        /// 顯示斷面 type 命令 (只顯示指定的斷面規格 type 的過濾器)
        /// </summary>
        public ICommand DisplayProfileTypeCommand { get; set; }
        #endregion
        #region 公開屬性
        /// <summary>
        /// 顯示在建立零件列表內的集合
        /// </summary>
        ObservableCollection<SteelPart> SteelParts { get; set; }
        /// <summary>
        /// 在零件列表內需要配料的數量
        /// </summary>
        ObservableCollection<int> MatchCount { get; set; }
        /// <summary>
        /// 加工完成進度百分比
        /// </summary>
        public double FinishPercentage { get; set; }
        /// <summary>
        /// 採購未排版數量
        /// </summary>
        public int PurchaseCount { get; set; }
        /// <summary>
        /// 加工未排版數量
        /// </summary>
        public int WorkCount { get; set; }
        /// <summary>
        /// 加工 / 採購未排版數量
        /// </summary>
        public int WorkPurchaseCount { get; set; }
        /// <summary>
        /// 零件總數量
        /// </summary>
        public int PartTotal
        {
            get => PurchaseCount + WorkCount + WorkPurchaseCount;
            set
            {
            }
        }
        #endregion
    }
}
