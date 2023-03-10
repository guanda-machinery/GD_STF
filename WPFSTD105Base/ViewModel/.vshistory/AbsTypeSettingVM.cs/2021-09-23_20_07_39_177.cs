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
using static WPFSTD105.ViewLocator;
using DevExpress.Xpf.CodeView;
using DevExpress.Data.Extensions;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 抽象排版設定
    /// </summary>
    public abstract class AbsTypeSettingVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 抽象排版設定
        /// </summary>
        public AbsTypeSettingVM()
        {
            STDSerialization ser = new STDSerialization();
            ObservableCollection<BomProperty> bomProperties = CommonViewModel.ProjectProperty.BomProperties; //報表屬性設定檔
            foreach (var profile in ser.GetProfile()) //逐步展開斷面規格
            {
                ObservableCollection<SteelPart> buffer = ser.GetPart(profile.GetHashCode().ToString());
                Parallel.For(0, buffer.Count, i => MatchCount.Add(0));//加入匹配數量
                _SteelParts.AddRange(buffer); //加入到物件料表內
                int index = bomProperties.FindIndex(el => el.Type == buffer[0].Type); //找出 TYPE 陣列位置

                if (index == -1) // index = -1 引發例外
                    throw new Exception("index 不可以是 -1");

                BomProperty property = bomProperties[index];
                if (property.Working && property.Purchase) //加工 / 採購 
                {
                    WorkPurchaseCount += buffer.Count;
                }
                else if (property.Working) //加工
                {
                    WorkCount += buffer.Count;
                }
                else if (property.Purchase) //採購
                {
                    PurchaseCount += buffer.Count;
                }
            }


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

        #region 命令處理方法
        /// <summary>
        /// 手動排版處理方法
        /// </summary>
        /// <returns></returns>
        protected abstract WPFBase.RelayCommand Manual();
        /// <summary>
        /// 自動排版處理方法
        /// </summary>
        /// <returns></returns>
        protected abstract WPFBase.RelayCommand Auto();
        /// <summary>
        /// 選擇報表全部物件處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand AllSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                grid.SelectRange(0, SteelParts.Count);//選擇物件
            });
        }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 建立零件列表內的集合
        /// </summary>
        public ObservableCollection<SteelPart> _SteelParts { get; set; }
        #endregion


        #region 公開屬性
        /// <summary>
        /// 顯示在建立零件列表內的集合
        /// </summary>
        public ObservableCollection<SteelPart> SteelParts { get; set; }
        /// <summary>
        /// 在零件列表內需要配料的數量
        /// </summary>
        public ObservableCollection<int> MatchCount { get; set; }
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
        public int PartTotal { get => PurchaseCount + WorkCount + WorkPurchaseCount; set { } }
        #endregion
    }
}
