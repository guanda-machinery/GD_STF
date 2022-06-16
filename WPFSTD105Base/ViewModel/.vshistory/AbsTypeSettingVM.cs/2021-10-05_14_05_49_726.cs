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
using GD_STD.Enum;

namespace WPFSTD105
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
                Parallel.For(0, buffer.Count, i => buffer[i].SortCount = buffer[i].NoMatcthIndex().Count);//加入預排數量
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

            SteelParts = new ObservableCollection<SteelPart>(_SteelParts == null ? new ObservableCollection<SteelPart>() : _SteelParts);

            AllSelectedGridCommand = AllSelectedGrid();// 選擇報表全部物件命令
            ReverseSelectedGridCommand = ReverseSelectedGrid();//反向選取命令
            UnselectSelectedGridCommand = UnselectSelectedGrid();//取消選取命令
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
        public ICommand UnselectSelectedGridCommand { get; set; }
        /// <summary>
        /// 顯示全部列表命令 (清除過濾器)
        /// </summary>
        public ICommand ClearFilterCommand { get; set; }
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
                grid.SelectAll();//全部資料行
                grid.EndSelection();//强制立即更新
            });
        }
        /// <summary>
        /// 反向選取處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand ReverseSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                List<int> useRow = grid.GetSelectedRowHandles().ToList(); //用戶選擇的行數
                int row = grid.VisibleRowCount; //顯示的行數
                for (int i = 0; i < row; i++) //逐步選取資料行
                {
                    if (!useRow.Contains(i))//用戶沒有選擇
                    {
                        grid.SelectItem(i); //選擇資料行
                    }
                }
                grid.EndSelection();//强制立即更新
            });
        }
        /// <summary>
        /// 取消選取處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand UnselectSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                grid.UnselectAll(); //取消選取
                grid.EndSelection();//强制立即更新
            });
        }
        /// <summary>
        /// 顯示全部列表處理方法 (清除過濾器)
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayCommand DisplayAllList()
        {
            return new WPFBase.RelayCommand(() =>
            {
                SteelParts = new ObservableCollection<SteelPart>(_SteelParts);
            });
        }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 建立零件列表內的集合
        /// </summary>
        private ObservableCollection<SteelPart> _SteelParts { get; set; } = new ObservableCollection<SteelPart>();
        #endregion

        #region 公開屬性
        /// <summary>
        /// 過濾斷面規格選擇器
        /// </summary>
        public ObservableCollection<FilterProfileType> FilterProfileTypes { get; set; } = new ObservableCollection<FilterProfileType>();
        /// <summary>
        /// 顯示在建立零件列表內的集合
        /// </summary>
        public ObservableCollection<SteelPart> SteelParts { get; set; }
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

        #region 類型
        /// <summary>
        /// 過濾斷面規格 type 
        /// </summary>
        public class FilterProfileType
        {
            /// <summary>
            /// 顯示此斷面規格 true，不顯示則 false
            /// </summary>
            public bool Display { get; set; } = true;
            /// <summary>
            /// 斷面規格類型
            /// </summary>
            public OBJETC_TYPE Type { get; set; }
        }
        #endregion
    }
}
