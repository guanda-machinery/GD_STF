//#define Debug
//using devDept.Eyeshot;
//using devDept.Eyeshot.Entities;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using WPFWindowsBase;
//using static devDept.Eyeshot.Environment;
//using Environment = devDept.Eyeshot.Environment;
//namespace WPFSTD105
//{
//    /// <summary>
//    /// 3D 視圖 ViewModel
//    /// </summary>]
//    public class ModelVM : BaseViewModel
//    {
//        #region 命令
//        /// <summary>
//        /// 平移
//        /// </summary>
//        /// <returns></returns>
//        public ICommand Pan { get; set; }
//        /// <summary>
//        /// 旋轉
//        /// </summary>
//        public ICommand Rotate { get; set; }
//        /// <summary>
//        /// 放大到框選舉型視窗
//        /// </summary>
//        public ICommand ZoomWindow { get; set; }
//        /// <summary>
//        /// 放大縮小 
//        /// </summary>
//        public ICommand Zoom { get; set; }
//        /// <summary>
//        /// 編輯物件
//        /// </summary>
//        public ICommand EditObject { get; set; }
//        /// <summary>
//        /// 刪除物件
//        /// </summary>
//        public ICommand Delete { get; set; }
//        /// <summary>
//        /// 取消所有功能
//        /// </summary>
//        public ICommand Esc { get; set; }
//        /// <summary>
//        /// 恢復上一個動作
//        /// </summary>
//        public ICommand Recovery { get; set; }
//        #endregion

//        #region 公開屬性
//        /// <summary>
//        /// 實體列表
//        /// </summary>
//        public EntityList EntityList { get; set; } = new EntityList();
//        /// <summary>
//        /// 圖層列表
//        /// </summary>
//        public LayerList LayerList { get; set; } = new LayerList();
//        /// <summary>
//        /// 取得或設置坐標系圖標。
//        /// </summary>
//        public CoordinateSystemIcon CoordinateSystemIcon { get; set; }
//        /// <summary>
//        /// 取得或設置原點符號列表。
//        /// </summary>
//        public ObservableCollection<OriginSymbol> OriginSymbols { get; set; }
//        /// <summary>
//        /// 獲取或設置活動視口動作。
//        /// </summary>
//        public actionType ActionMode { get; set; }
//        /// <summary>
//        /// 取得原點符號
//        /// </summary>
//        public OriginSymbol OriginSymbol { get => OriginSymbols[0]; }
//        /// <summary>
//        /// 用戶設定檔案
//        /// </summary>
//        public ModelAttr Setting { get; set; } = new ModelAttr();

//        /// <summary>
//        /// 以選擇的參考圖塊列表
//        /// </summary>
//        public List<SelectedItem> SelectItem { get; set; } = new List<SelectedItem>();
//        /// <summary>
//        /// 被選中的圖塊內物件
//        /// </summary>
//        public List<Entity> temRecycle { get; set; } = new List<Entity>();
//        /// <summary>
//        /// 被刪除過的物件列表
//        /// </summary>
//        public List<List<Entity>> Recycle { get; set; } = new List<List<Entity>>();
//        /// <summary>
//        /// 更動過的圖塊名稱
//        /// </summary>
//        public List<BlockReference> BlockReferences { get; set; } = new List<BlockReference>();
//        #endregion



//        #region 私有屬性


//        #endregion
//        /// <summary>
//        /// 初始化
//        /// </summary>
//        public ModelVM()
//        {
//            ////TODO:可以設定 X Y Z 軸向顏色
//            //初始化數據綁定的坐標系圖標。
//            CoordinateSystemIcon = new CoordinateSystemIcon();

//            //初始化數據綁定的原始符號。
//            OriginSymbols = new ObservableCollection<OriginSymbol>(new List<OriginSymbol>() { OriginSymbol.GetDefaultOriginSymbol() });

//            LoadAttribute();
//        }
//        #region 私有方法
//        /// <summary>
//        /// 載入用戶屬性
//        /// </summary>
//        public void LoadAttribute()
//        {
//#if DEBUG
//            OriginSymbol.Visible = false;
//#else
//            //TODO : 讀取用戶設定檔
//#endif
//        }
//        #endregion
//    }
//}
