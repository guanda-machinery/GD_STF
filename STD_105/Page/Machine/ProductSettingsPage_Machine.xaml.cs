using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using GD_STD;
using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using static WPFSTD105.Model.Expand;
using BlockReference = devDept.Eyeshot.Entities.BlockReference;
using MouseButton = devDept.Eyeshot.MouseButton;

namespace STD_105
{
    /// <summary>
    /// Product.xaml 的互動邏輯
    /// </summary>
    public partial class ProductSettingsPage_Machine : BasePage<ObSettingVM>
    {
        #region 變數宣告
        ApplicationVM appVM = new ApplicationVM();
        //ObSettingVM sr = new ObSettingVM();

        /// <summary>
        /// 是否產生新零件
        /// 新增.修改為true
        /// 加入切割線為false
        /// </summary>
        public bool isNewPart = false;
        /// <summary>
        /// 啟動畫面管理器
        /// </summary>
        //public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.CreateWaitIndicator();
        private SplashScreenManager ProcessingScreenWin = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
        /// <summary>
        /// Grid Reload前的Index
        /// </summary>
        public int PreIndex { get; set; }

        /// <summary>
        /// 是否為首次匯入NC&BOM的falg
        /// </summary>
        private bool fAfterFirstImportTeklaData { get; set; } = true;

        private devDept.Eyeshot.Model _BufferModel { get; set; }
        #endregion


        /// <summary>
        /// 20220823 蘇冠綸 製品設定
        /// </summary>                                                                                                                                          
        public ProductSettingsPage_Machine()
        {
            InitializeComponent();

            /// <summary>
            /// 鑽孔radio button測試 20220906 張燕華
            /// </summary>
            ViewModel.CmdShowMessage = new RelayCommand(() =>
            {
                System.Windows.MessageBox.Show("You Select " + ViewModel.rbtn_DrillingFace.ToString());
            });

            #region 3D
            model.DataContext = ViewModel;
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.InitializeViewports();
            //model.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            model.Secondary = drawing;
            #endregion

            #region 2D
            drawing.DataContext = ViewModel;
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.InitializeViewports();
            //drawing.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            drawing.Secondary = model;
            //drawing.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
            #endregion

            tabControl.SelectedIndex = 1;

            #region 定義 MenuItem 綁定的命令
            //放大縮小
            ViewModel.Zoom = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("放大縮小");
#endif
                //使用放大縮小
                model.ActionMode = actionType.Zoom;
            });
            //放大到框選舉型視窗
            ViewModel.ZoomWindow = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("放大到框選舉型視窗");
#endif
                //使用放大到框選舉型
                model.ActionMode = actionType.ZoomWindow;
            });
            //旋轉
            ViewModel.Rotate = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("旋轉");
#endif
                //開啟旋轉
                model.ActionMode = actionType.Rotate;
                //開啟取消功能
                esc.Visibility = Visibility.Visible;
            });
            //平移
            ViewModel.Pan = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("平移");
#endif
                model.ActionMode = actionType.Pan;
            });
            //取消
            ViewModel.Esc = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("取消");
#endif
                Esc();
                //刷新模型
                model.Refresh();
                //更新模型
                drawing.Refresh();
            });
            //編輯物件
            ViewModel.EditObject = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("編輯");
                log4net.LogManager.GetLogger("EditObject").Debug("");
#endif
                try
                {
                    //層級 To 要編輯的BlockReference
                    model.SetCurrent((BlockReference)ViewModel.Select3DItem[0].Item);
                    drawing.SetCurrent((BlockReference)drawing.Entities.Find(el => ((BlockReference)el).BlockName == model.CurrentBlockReference.BlockName));
                    model.Refresh();//更新模型
                    drawing.Refresh();
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                    WinUIMessageBox.Show(null,
                    $"目前已在編輯模式內，如要離開請按下Esc",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
            });
            //還原上一個動作
            ViewModel.Recovery = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("恢復上一個動作");
#endif
                ViewModel.Reductions.Previous();//回到上一個動作
                model.Refresh();//更新模型
                drawing.Refresh();//更新模型
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("完成上一個動作");
#endif
            });
            //還原下一個動作
            ViewModel.Next = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("恢復下一個動作");
#endif
                ViewModel.Reductions.Next();//回到上一個動作
                model.Refresh();//更新模型
                drawing.Refresh();//更新模型
#if DEBUG
                log4net.LogManager.GetLogger("按下選單命令").Debug("完成下一個動作");
#endif
            });
            //刪除物件(右鍵)
            ViewModel.Delete = new RelayCommand(() =>
            {
                SimulationDelete();
                Esc();
                SaveModel(false);
            });
            //清除標註
            ViewModel.ClearDim = new RelayCommand(() =>
            {
                try
                {
                    ModelExt modelExt = null;
                    if (tabControl.SelectedIndex == 0)
                    {
                        modelExt = model;
                    }
                    else
                    {
                        // 2022.06.24 呂宗霖 還原註解
                        modelExt = drawing;
                    }
                    List<Entity> dimensions = new List<Entity>();//標註物件
                    modelExt.Entities.ForEach(el =>
                    {
#if DEBUG
                        log4net.LogManager.GetLogger("清除標註").Debug("開始");
#endif
                        if (el is Dimension dim)
                        {
                            dimensions.Add(dim);
                        }
#if DEBUG
                        log4net.LogManager.GetLogger("清除標註").Debug("結束");
#endif
                    });
                    modelExt.Entities.Remove(dimensions);
                    modelExt.Invalidate();//刷新模型
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                    Debugger.Break();
                }
            });
            #endregion

            #region VM Command

            //加入主零件
            ViewModel.AddPart = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("AddPart").Debug("");
#endif
                #region 3D 
                if (!DataCheck("add"))
                {
                    ViewModel.fclickOK = false;
                    return;
                }

                //檢測用戶輸入的參數是否有完整
                if (!CheckPart())
                {
                    ViewModel.fclickOK = false;
                    return;
                }

                // 第一次按新增
                if (ViewModel.fFirstAdd.Value)
                {
                    ViewModel.fFirstAdd = false;
                    ViewModel.fNewPart = true;
                    ViewModel.fGrid = false;
                    ViewModel.fclickOK = false;

                    ViewModel.StateParaSetting(false, true, false);
                    //ViewModel.StateParaSetting(false, true, false);
                }

                model.Entities.Clear();//清除模型物件
                model.Blocks.Clear(); //清除模型圖塊

                STDSerialization ser = new STDSerialization();
                SteelAttr sa = new SteelAttr();
                ViewModel.SteelAttr = new SteelAttr();
                GetViewToViewModel(true);
                sa = GetViewToSteelAttr(sa, true);
                sa.Creation = DateTime.Now;
                sa.Revise = DateTime.Now;

#if DEBUG
                log4net.LogManager.GetLogger("加入主件").Debug("產生圖塊");
#endif

                // 規則.
                //如果零件編號與Grid所選零件之零件編號不同
                //1.長度 / 型鋼類型 / 斷面規格與所選項目之長度 / 型鋼類型 / 斷面規格[完全相同]
                //　→則[複製所選項目之細項(含鑽孔 挖槽 切割線)] 給新零件編號
                //　→a.有NC檔→複製為此新零件之NC檔→讀dm，取型鋼及孔位資訊→畫2D/3D模型
                //　→b.無NC檔→讀dm，取型鋼及孔位資訊→畫2D/3D模型
                //2.長度 / 型鋼類型 / 斷面規格與所選項目之長度 / 型鋼類型 / 斷面規格[其一不相同]
                //　→則[產生全新型鋼] 給新零件編號


                // 已選項目之細項
                ProductSettingsPageViewModel gridItem = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                if (gridItem != null)
                {
                    Guid? g_GUID = gridItem.steelAttr.GUID;
                    OBJECT_TYPE g_Type = gridItem.steelAttr.Type;
                    string g_partNumber = gridItem.steelAttr.PartNumber;
                    double g_length = gridItem.steelAttr.Length;
                    string g_profile = gridItem.steelAttr.Profile;

                    string path = ApplicationVM.DirectoryNc();
                    string allPath = path + $"\\{g_partNumber}.nc1";

                    //如果零件編號與Grid所選零件之零件編號不同
                    //1.長度 / 型鋼類型 / 斷面規格與所選項目之長度 / 型鋼類型 / 斷面規格[完全相同] 則[複製所選項目之細項(含鑽孔 挖槽 切割線)] 給新零件編號
                    //2.長度 / 型鋼類型 / 斷面規格與所選項目之長度 / 型鋼類型 / 斷面規格[其一不相同] 則[產生全新型鋼] 給新零件編號
                    ModelExt gridModel = new ModelExt();
                    #region 零件編號不同 長度 / 型鋼類型 / 斷面規格一樣
                    if (g_partNumber != ViewModel.PartNumberProperty && g_length == ViewModel.ProductLengthProperty &&
                    (int)g_Type == ViewModel.SteelTypeProperty_int && g_profile == ViewModel.SteelSectionProperty)
                    {
                        #region 有NC檔
                        if (File.Exists($@"{allPath}"))
                        {
                            #region 複製NC檔
                            ViewModel.CopyNCFile(allPath, g_partNumber, ViewModel.PartNumberProperty);
                            #endregion

                            #region 讀取dm檔
                            ReadFile readFile = ser.ReadPartModel(g_GUID.ToString()); //讀取檔案內容
                            //if (readFile == null)
                            //{
                            //    WinUIMessageBox.Show(null,
                            //        $"專案Dev_Part資料夾讀取失敗",
                            //        "通知",
                            //        MessageBoxButton.OK,
                            //        MessageBoxImage.Exclamation,
                            //        MessageBoxResult.None,
                            //        MessageBoxOptions.None,
                            //         FloatingMode.Window);
                            //    return;
                            //}
                            readFile.DoWork();//開始工作
                            readFile.AddToScene(model);//將讀取完的檔案放入到模型 
                            #endregion

                            #region 新型鋼資訊
                            sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                            // 若原零件有多組構件，sa.AsseNumber會呈現字串串接的狀態，故調整回單一個構件
                            sa.GUID = Guid.NewGuid();
                            ViewModel.GuidProperty = sa.GUID;
                            sa.AsseNumber = ViewModel.AssemblyNumberProperty;
                            sa.PartNumber = ViewModel.PartNumberProperty;
                            // 原零件檔中的數量欄位為總數量，故調整為畫面上之數量
                            sa.Number = ViewModel.ProductCountProperty;
                            // 來源不為Tekla
                            sa.TeklaAssemblyID = "";
                            // 來源不為Tekla
                            sa.TeklaPartID = "";
                            sa.Length = g_length;
                            sa.Name = gridItem.steelAttr.Name;
                            sa.Material = gridItem.steelAttr.Material;
                            sa.Phase = null;
                            sa.ShippingNumber = null;
                            sa.Title1 = "";
                            sa.Title2 = "";
                            sa.Creation = DateTime.Now;
                            #endregion

                            #region 讀NC檔
                            List<GroupBoltsAttr> groupBolts = new List<GroupBoltsAttr>();
                            SteelAttr saDeepClone = (SteelAttr)sa.DeepClone();
                            sa = ViewModel.ReadNCInfo(saDeepClone, ref groupBolts);
                            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
                            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.vPoint;
                            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.uPoint;
                            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;
                            #endregion

                            #region 孔位資訊
                            List<Block> block = model.GetBoltFromBlock(groupBolts);
                            #endregion
                            //var profile = ser.GetSteelAttr();
                            //TeklaNcFactory t = new TeklaNcFactory();
                            //Steel3DBlock s3Db = new Steel3DBlock();
                            //SteelAttr steelAttrNC = new SteelAttr();
                            //List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();

                            //s3Db.ReadNcFile($@"{ApplicationVM.DirectoryNc()}\{ViewModel.PartNumberProperty}.nc1", profile, sa, ref steelAttrNC, ref groups);
                            //sa.GUID = Guid.NewGuid();
                            //sa.oPoint = steelAttrNC.oPoint;
                            //sa.vPoint = steelAttrNC.vPoint;
                            //sa.uPoint = steelAttrNC.uPoint;
                            //sa.CutList = steelAttrNC.CutList;

                            #region 建模(以舊有形鋼為底新增)
                            // 建立型鋼
                            Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);// 修改型鋼資訊
                            model.Blocks[1].Name = result.Name;
                            model.Entities[model.Entities.Count - 1] = model.Blocks[1].Entities[0];
                            //Steel3DBlock.AddSteel(sa, model, out BlockReference blockReference, "Steel");
                            //model.AddModelSteelAttr(sa, result);
                            // 建立2D模型
                            BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                            drawing.Entities[drawing.Entities.Count - 1].EntityData = steel2D;
                            // 建立2D/3D孔
                            ViewModel.AddBolts(model, drawing, out bool hasOutSteel, block);
                            #endregion

                            ScrollViewbox.IsEnabled = true;
                            if (model.RunHypotenuseEnable()) { /*ScrollViewbox.IsEnabled = false;*/ } else { ScrollViewbox.IsEnabled = true; }
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);
                            SaveModel(true, true);

                            ViewModel.fAddSteelPart = false; // hank 新設 新增零件旗號,暫不儲存
                            ViewModel.fclickOK = false;
                            ViewModel.StateParaSetting(false, false, false);
                            model.ZoomFit();//設置道適合的視口
                            model.Refresh();//刷新模型
                            drawing.ZoomFit();
                            drawing.Refresh();

                            //ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                            //this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                            //int PreIndex = source.FindIndex(x => x.DataName == sa.GUID.Value.ToString());
                            //PieceListGridControl.View.FocusedRowHandle = PreIndex;
                            //PieceListGridControl.SelectItem(PreIndex);
                            ////ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
                            //cbx_SectionTypeComboBox.Text = sa.Profile;
                            //this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                            return;
                        }
                        #endregion
                        #region 無NC檔
                        else
                        {
                            #region DM新增
                            #region 讀取dm檔
                            ReadFile readFile = ser.ReadPartModel(g_GUID.ToString()); //讀取檔案內容
                            if (readFile == null)
                            {
                                WinUIMessageBox.Show(null,
                                    $"專案Dev_Part資料夾讀取失敗",
                                    "通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                     FloatingMode.Window);
                                return;
                            }
                            readFile.DoWork();//開始工作
                            readFile.AddToScene(model);//將讀取完的檔案放入到模型 
                            #endregion

                            #region 孔位資訊
                            List<Block> block = model.GetBoltFromBlock();
                            #endregion

                            #region 塞值
                            sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                            sa = GetViewToSteelAttr(sa, true);
                            sa.GUID = Guid.NewGuid();
                            sa.Length = g_length;
                            sa.Name = gridItem.steelAttr.Name;
                            sa.Material = gridItem.steelAttr.Material;
                            sa.Phase = 0;
                            sa.ShippingNumber = 0;
                            sa.Title1 = "";
                            sa.Title2 = "";
                            sa.PartNumber = ViewModel.PartNumberProperty;
                            sa.Creation = DateTime.Now;
                            #endregion


                            #region 建模(以舊有形鋼為底新增)
                            // 建立型鋼
                            Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);// 修改型鋼資訊
                            model.Blocks[1].Name = result.Name;
                            model.Entities[model.Entities.Count - 1] = model.Blocks[1].Entities[0];
                            //Steel3DBlock.AddSteel(sa, model, out BlockReference blockReference, "Steel");
                            //model.AddModelSteelAttr(sa, result);
                            // 建立2D模型
                            BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                            drawing.Entities[drawing.Entities.Count - 1].EntityData = steel2D;
                            // 建立2D/3D孔
                            ViewModel.AddBolts(model, drawing, out bool hasOutSteel, block);
                            #endregion

                            ScrollViewbox.IsEnabled = true;
                            if (model.RunHypotenuseEnable()) { /*ScrollViewbox.IsEnabled = false;*/ } else { ScrollViewbox.IsEnabled = true; }
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
                            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);

                            SaveModel(true, true);
                            #endregion
                            ViewModel.fAddSteelPart = true; // hank 新設 新增零件旗號,暫不儲存
                            ViewModel.fclickOK = false;
                            ViewModel.StateParaSetting(false, false, false);
                            model.ZoomFit();//設置道適合的視口
                            model.Refresh();//刷新模型
                            drawing.ZoomFit();
                            drawing.Refresh();


                            ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                            this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                            int PreIndex = source.FindIndex(x => x.DataName == sa.GUID.Value.ToString());
                            PieceListGridControl.View.FocusedRowHandle = PreIndex;
                            PieceListGridControl.SelectItem(PreIndex);
                            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
                            cbx_SectionTypeComboBox.Text = sa.Profile;
                            this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                            return;
                        }
                        #endregion
                    }
                    #endregion
                    #region 零件編號不同 長度 / 型鋼類型 / 斷面規格其一不同
                    else
                    {
                        #region 一般新增
                        sa = GetViewToSteelAttr(sa, true);
                        sa.Creation = DateTime.Now;
                        ViewModel.WriteSteelAttr(sa);
                        sa = ViewModel.GetSteelAttr();
                        if (string.IsNullOrEmpty(sa.PartNumber))
                        {
                            // 若ViewModel.SteelAttr.PartNumber代表取值又失敗了，只好強制給值囉~
                            GetViewToViewModel(true);
                        }

                        #region 建模(全新型鋼)
                        // 建立型鋼
                        Steel3DBlock.AddSteel(sa, model, out BlockReference blockReference, "Steel");
                        // 建立2D模型
                        BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                        // 建立2D/3D孔
                        ViewModel.AddBolts(model, drawing, out bool hasOutSteel, null);
                        #endregion

                        #region 回復動作
                        ViewModel.Reductions.Add(new Reduction()
                        {
                            Recycle = new List<List<Entity>>() { new List<Entity>() { blockReference } },
                            SelectReference = null,
                            User = new List<ACTION_USER>() { ACTION_USER.Add }
                        }, new Reduction()
                        {
                            // 2022.06.24 呂宗霖 還原註解
                            Recycle = new List<List<Entity>>() { new List<Entity>() { steel2D } },
                            SelectReference = null,
                            User = new List<ACTION_USER>() { ACTION_USER.Add }
                        });
                        #endregion

                        ScrollViewbox.IsEnabled = true;
                        #endregion

                        ViewModel.fAddSteelPart = true; // hank 新設 新增零件旗號,暫不儲存
                        ViewModel.fclickOK = false;
                        ViewModel.StateParaSetting(true, true, false);
                        model.ZoomFit();//設置道適合的視口
                        model.Refresh();//刷新模型
                        drawing.ZoomFit();
                        drawing.Refresh();

                        ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                        this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        int PreIndex = source.FindIndex(x => x.DataName == sa.GUID.Value.ToString());
                        PieceListGridControl.View.FocusedRowHandle = PreIndex;
                        PieceListGridControl.SelectItem(PreIndex);
                        cbx_SectionTypeComboBox.Text = sa.Profile;
                        PieceListGridControl.View.FocusedRowHandle = PieceListGridControl.VisibleRowCount;
                        this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    }
                    #endregion
                }
                else
                {
                    #region 一般新增
                    sa = GetViewToSteelAttr(sa, true);
                    sa.Creation = DateTime.Now;
                    ViewModel.WriteSteelAttr(sa);
                    sa = ViewModel.GetSteelAttr();

                    var profile = ser.GetProfile();

                    if (string.IsNullOrEmpty(sa.PartNumber))
                    {
                        // 若ViewModel.SteelAttr.PartNumber代表取值又失敗了，只好強制給值囉~
                        Thread.Sleep(1000);
                        sa = GetViewToSteelAttr(sa, true);
                        sa.Creation = DateTime.Now;
                        ViewModel.WriteSteelAttr(sa);
                        sa = ViewModel.GetSteelAttr();
                    }

                    #region 建模(全新型鋼)
                    // 建立型鋼
                    Steel3DBlock.AddSteel(sa, model, out BlockReference blockReference, "Steel");
                    // 建立2D模型
                    BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                    // 建立2D/3D孔
                    ViewModel.AddBolts(model, drawing, out bool hasOutSteel, null);
                    #endregion

                    #region 回復動作
                    ViewModel.Reductions.Add(new Reduction()
                    {
                        Recycle = new List<List<Entity>>() { new List<Entity>() { blockReference } },
                        SelectReference = null,
                        User = new List<ACTION_USER>() { ACTION_USER.Add }
                    }, new Reduction()
                    {
                        // 2022.06.24 呂宗霖 還原註解
                        Recycle = new List<List<Entity>>() { new List<Entity>() { steel2D } },
                        SelectReference = null,
                        User = new List<ACTION_USER>() { ACTION_USER.Add }
                    });
                    #endregion
                    #endregion

                    ScrollViewbox.IsEnabled = true;

                    ViewModel.fAddSteelPart = true; // hank 新設 新增零件旗號,暫不儲存
                    ViewModel.fclickOK = false;
                    ViewModel.StateParaSetting(true, true, false);
                    model.ZoomFit();//設置道適合的視口
                    model.Refresh();//刷新模型
                    drawing.ZoomFit();
                    drawing.Refresh();
                }
                // SaveModel(true);
                // Reload
                //GridReload();
                //SteelAttr sat = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;

                // 鋼材類別
                var aa = sa.Type.GetType().GetMember(sa.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>();
                string type = aa == null ? "" : aa.Description;

                ViewModel.ProfileType = (int)sa.Type;

                ProductSettingsPageViewModel tempSteelAttr = new ProductSettingsPageViewModel()
                {
                    Length = sa.H,
                    Weight = sa.Weight,
                    Profile = sa.Profile,
                    Material = sa.Material,
                    Count = sa.Number,
                    Phase = sa.Phase,
                    ShippingNumber = sa.ShippingNumber,
                    Title1 = sa.Title1,
                    Title2 = sa.Title2,
                    Type = sa.Type,
                    TypeDesc = type,
                    SteelType = Convert.ToInt32(sa.Type),
                    TeklaName = sa.Name,
                    DataName = sa.GUID.ToString(),
                    Creation = sa.Creation,
                    Revise = sa.Revise,
                    steelAttr = sa,
                };
                tempSteelAttr.steelAttr.Creation = sa.Creation;
                tempSteelAttr.steelAttr.Revise = sa.Revise;
                tempSteelAttr.steelAttr.PartNumber = sa.PartNumber;
                tempSteelAttr.steelAttr.AsseNumber = sa.AsseNumber;
                tempSteelAttr.steelAttr.Length = sa.Length;
                tempSteelAttr.Length = sa.Length;
                tempSteelAttr.steelAttr.Weight = sa.Weight;
                tempSteelAttr.steelAttr.Name = sa.Name;
                tempSteelAttr.steelAttr.Phase = sa.Phase;
                tempSteelAttr.steelAttr.ShippingNumber = sa.ShippingNumber;
                tempSteelAttr.steelAttr.Title1 = sa.Title1;
                tempSteelAttr.steelAttr.Title2 = sa.Title2;
                tempSteelAttr.steelAttr.ExclamationMark = sa.ExclamationMark;
                tempSteelAttr.steelAttr.Material = sa.Material;
                tempSteelAttr.steelAttr.Number = sa.Number;
                tempSteelAttr.steelAttr.GUID = sa.GUID;
                tempSteelAttr.steelAttr.Profile = sa.Profile;
                //ViewModel.ProfileList= SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
                //cbx_SectionTypeComboBox.ItemsSource = ViewModel.ProfileList;
                //cbx_SectionTypeComboBox.Text = sa.Profile;
                tempSteelAttr.steelAttr.t1 = sa.t1;
                tempSteelAttr.steelAttr.t2 = sa.t2;
                tempSteelAttr.steelAttr.Type = sa.Type;
                //cbx_SteelTypeComboBox.Text = sa.Type.ToString();

                var tmpSource = PieceListGridControl.ItemsSource;

                #region tempNewSource = ItemSource + New Data
                ObservableCollection<ProductSettingsPageViewModel> tempNewSource = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                tempNewSource.Add(tempSteelAttr);
                #endregion
                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                PieceListGridControl.ItemsSource = tempNewSource;
                PieceListGridControl.View.MoveLastRow();
                //PieceListGridControl.SelectItem(tempNewSource.Count - 1);
                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                ViewModel.ProfileType = tempSteelAttr.SteelType;
                //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(tempSteelAttr.steelAttr.Type).ToString()}.inp");
                cbx_SectionTypeComboBox.Text = tempSteelAttr.steelAttr.Profile;
                //ConfirmCurrentSteelSection(((ProductSettingsPageViewModel)PieceListGridControl.SelectedItem));
                ConfirmCurrentSteelSection(((ProductSettingsPageViewModel)tempSteelAttr));
#if DEBUG
                log4net.LogManager.GetLogger("AddPart").Debug("");
                log4net.LogManager.GetLogger("加入主件").Debug("結束");
#endif
                #endregion

                ViewModel.fclickOK = false;

            });
            //修改主零件
            ViewModel.ModifyPart = new RelayCommand(() =>
            {
                // 修改 = 新增
                //ViewModel.AddPart.Execute(null);
                //ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                //ConfirmCurrentSteelSection(row);
                //GetViewToViewModel(false, ViewModel.GuidProperty);
                //規格不對 不可修改?


                if (!DataCheck("edit"))
                {
                    ViewModel.fclickOK = true;
                    return;
                }

                if (!CheckPart()) //檢測用戶輸入的參數是否有完整
                { ViewModel.fclickOK = true; return; }
                if (model.CurrentBlockReference != null)
                {
                    WinUIMessageBox.Show(null,
                    $"退出編輯模式，才可修改主件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                    ViewModel.fclickOK = true;
                    return;
                }

                // 新增 及 修改 都是新增零件
                //if (this.PieceListGridControl.VisibleRowCount > 0)
                //{
                ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                Guid? oldPartGuid = row.steelAttr.GUID;
                row.steelAttr.GUID = Guid.Parse(row.DataName);
                ProductSettingsPageViewModel temp = RowToEntity(row);
                if (File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{temp.steelAttr.GUID}.dm"))
                {
                    // 非新零件
                    ViewModel.fNewPart = false;
                }


#if DEBUG
                log4net.LogManager.GetLogger("ModifyPart").Debug("");
                log4net.LogManager.GetLogger("修改主件").Debug("開始");
#endif

                try
                {
                    //SelectedItem sele3D = new SelectedItem(model.Entities.Where(x=>x.EntityData.GetType().Name == "SteelAttr").LastOrDefault());
                    //SelectedItem sele2D = new SelectedItem(drawing.Entities.Where(x=>((BlockReference)x).BlockName == ViewModel.SteelAttr.GUID.ToString()).LastOrDefault());
                    SelectedItem sele3D = new SelectedItem(model.Entities[model.Entities.Count - 1]);
                    SelectedItem sele2D = new SelectedItem(drawing.Entities[drawing.Entities.Count - 1]);
                    //SelectedItem sele2D = new SelectedItem(drawing.Entities[drawing.Entities.Count - 1]);

                    BlockReference reference3D = (BlockReference)sele3D.Item;
                    BlockReference reference2D = (BlockReference)sele2D.Item;

                    //模擬用戶實際選擇編輯
                    ViewModel.Select3DItem.Add(sele3D);
                    ViewModel.Select2DItem.Add(sele2D);
                }
                catch (Exception)
                {
                    WinUIMessageBox.Show(null,
                    $"請再次點擊該零件列",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                    ViewModel.fclickOK = true;
                    return;
                }

                //層級 To 要編輯的BlockReference
                //model.SetCurrent((BlockReference)model.Entities.Where(x=>x.EntityData.GetType().Name == "SteelAttr").LastOrDefault());
                model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);

                //drawing.SetCurrent((BlockReference)drawing.Entities.Where(x=>((BlockReference)x).BlockName == ViewModel.SteelAttr.GUID.ToString()).LastOrDefault());
                drawing.SetCurrent((BlockReference)drawing.Entities[drawing.Entities.Count - 1]);
                ////0→     drawing.Entities.Count - 1
                //drawing.SetCurrent((BlockReference)drawing.Entities[drawing.Entities.Count - 1]);

                //SteelAttr steelAttr = ViewModel.GetSteelAttr();
                //if (string.IsNullOrEmpty(steelAttr.PartNumber))
                //{
                //    // 若ViewModel.SteelAttr.PartNumber代表取值又失敗了，只好強制給值囉~
                //    GetViewToViewModel();
                //}

                STDSerialization ser = new STDSerialization();
                ReadFile readFile = ser.ReadPartModel(temp.steelAttr.GUID.ToString()); //讀取檔案內容
                if (readFile == null)
                {
                    WinUIMessageBox.Show(null,
                        $"專案Dev_Part資料夾讀取失敗",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
                    return;
                }
                readFile.DoWork();//開始工作
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                //}





                //SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                SteelAttr steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                // 所有一般孔位
                List<GroupBoltsAttr> modelAllBoltList = model.Blocks.SelectMany(x => x.Entities).Where(y => y.GetType() == typeof(BlockReference) && y.EntityData.GetType() == typeof(GroupBoltsAttr) && ((GroupBoltsAttr)y.EntityData).Mode == AXIS_MODE.PIERCE).Select(y => (GroupBoltsAttr)y.EntityData).ToList();

                //// 2022/10/13 呂宗霖 編輯:零件編號須一致才可按編輯
                //if (steelAttr.PartNumber != this.partNumber.Text)
                //{
                //    WinUIMessageBox.Show(null,
                //   $"零件編號相同，才可修改主件",
                //   "通知",
                //   MessageBoxButton.OK,
                //   MessageBoxImage.Exclamation,
                //   MessageBoxResult.None,
                //   MessageBoxOptions.None,
                //    FloatingMode.Window);
                //    return;
                //}

                if (ViewModel.fNewPart.Value)
                {
                    GetViewToViewModel(true);
                    steelAttr = GetViewToSteelAttr(steelAttr, true);
                    steelAttr.Creation = DateTime.Now;
                    steelAttr.Revise = DateTime.Now;
                    model.Blocks[1].Entities[0].EntityData = steelAttr;
                }
                else
                {
                    GetViewToViewModel(false, (steelAttr).GUID);
                    steelAttr = GetViewToSteelAttr(steelAttr, false, (steelAttr).GUID);
                    steelAttr.Revise = DateTime.Now;
                }

                List<Entity> steel2D = new List<Entity>();
                //BlockReference modify2 = (BlockReference)model.Entities[model.Entities.Count() - 1];
                ProductSettingsPageViewModel gridItem = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                gridItem.steelAttr.GUID = Guid.Parse(gridItem.DataName);
                string path = ApplicationVM.DirectoryNc();
                string allPath = path + $"\\{gridItem.steelAttr.PartNumber}.nc1";
                if (
                (gridItem.Profile != steelAttr.Profile || gridItem.Type != steelAttr.Type) && !File.Exists($@"{allPath}") ||
                (gridItem.Profile == steelAttr.Profile && gridItem.Type == steelAttr.Type) && !File.Exists($@"{allPath}")
                )
                {
                    #region 無NC檔
                    //Mesh modify = Steel3DBlock.GetProfile(steelAttr); //修改的形狀
                    // modify->model.Blocks[0].Entities[1]
                    ViewModel.tem3DRecycle.Add(model.Entities[model.Entities.Count - 1]);//加入垃圾桶準備刪除
                    ViewModel.tem2DRecycle.AddRange(drawing.Entities);//加入垃圾桶準備刪除

                    //model.Entities[0].Selected = true;//選擇物件
                    drawing.Entities.ForEach(el => el.Selected = true);

                    //Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[reference2D.BlockName];//drawing.CurrentBlockReference.BlockName→1
                    //steel2DBlock.ChangeMesh(modify);//SteelAttr
                    //加入復原動作至LIST
                    ViewModel.Reductions.Add(new Reduction()
                    {
                        SelectReference = model.CurrentBlockReference,
                        Recycle = new List<List<Entity>>() { ViewModel.tem3DRecycle.ToList() },//Entities.toList()
                        User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                    }, new Reduction() //加入到垃圾桶內
                    {
                        SelectReference = drawing.CurrentBlockReference,
                        Recycle = new List<List<Entity>>() { ViewModel.tem2DRecycle.ToList() },
                        User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                    });

                    #region 孔位資訊
                    List<Block> block = model.GetBoltFromBlock();
                    #endregion

                    #region 建模
                    // 建立型鋼
                    //Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);// 修改型鋼資訊
                    ViewModel.RemoveHypotenusePoint(model, HypotenuseTYPE.HypotenuseMan.ToString());
                    Steel3DBlock result = new Steel3DBlock(Steel3DBlock.GetProfile((SteelAttr)model.Blocks[1].Entities[0].EntityData));// 修改型鋼資訊                    
                    model.AddModelSteelAttr(steelAttr, result);
                    //Steel3DBlock.AddSteel(steelAttr, model, out BlockReference blockReference, "Steel");
                    // 建立2D模型
                    //steel2D = new Steel2DBlock((Mesh)result.Entities[0], "123").Entities.ToList();//EntityData
                    ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                    //drawing.Entities.Clear();
                    //drawing.Entities.AddRange(steel2D);
                    //drawing.Entities[drawing.Entities.Count - 1].EntityData=(BlockReference)steel2D;
                    //ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);
                    // 建立2D/3D孔
                    ViewModel.AddBolts(model, drawing, out bool hasOutSteel, block);

                    // 切割線打點
                    //ScrollViewbox.IsEnabled = !model.RunHypotenuseEnable();
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);
                    #endregion


                    //Steel3DBlock result = new Steel3DBlock(Steel3DBlock.GetProfile(steelAttr));
                    //if (model.Blocks.Count > 1)
                    //{
                    //    model.Blocks.Remove(model.Blocks[1]);
                    //}
                    //model.Blocks.Insert(1, result);//加入鋼構圖塊到模型
                    //BlockReference blockReference = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);
                    //blockReference.EntityData = steelAttr;
                    //blockReference.Selectable = false;//關閉用戶選擇
                    //blockReference.Attributes.Add("steel", new AttributeReference(0, 0, 0));
                    //if (model.Entities.Count > 0)
                    //{
                    //    model.Entities.RemoveAt(model.Entities.Count - 1);
                    //}
                    //model.Entities.Insert(model.Entities.Count, blockReference);//加入參考圖塊到模型
                    //List<Block> blocks = model.Blocks.ToList();
                    //ViewModel.AddBolts(model, drawing,  out bool hasOutSteel, blocks);

                    //steel2D = new Steel2DBlock((Mesh)result.Entities[0], "123").Entities.ToList();//EntityData


                    //刪除指定物件
                    //model.Blocks[1].Entities[0] = modify;
                    //model.Entities.RemoveAt(model.Entities.Count() - 1);
                    //drawing.Entities.Clear();
                    //清空選擇物件
                    ViewModel.Select2DItem.Clear();
                    ViewModel.Select3DItem.Clear();
                    //清空圖塊內物件
                    ViewModel.tem3DRecycle.Clear();
                    ViewModel.tem2DRecycle.Clear();

                    //ViewModel.Reductions.AddContinuous(new List<Entity>() { ViewModel.tem3DRecycle[0], drawing.Entities[drawing.Entities.Count - 1] });
                    //model.Entities.Add(modify);//SteelAttr上一層
                    //drawing.Entities.AddRange(steel2D);
                    #endregion
                }
                else
                {
                    #region 有NC檔
                    ViewModel.tem3DRecycle.Add(model.Entities[model.Entities.Count - 1]);//加入垃圾桶準備刪除
                    ViewModel.tem2DRecycle.AddRange(drawing.Entities);//加入垃圾桶準備刪除
                    //加入復原動作至LIST
                    ViewModel.Reductions.Add(new Reduction()
                    {
                        SelectReference = model.CurrentBlockReference,
                        Recycle = new List<List<Entity>>() { ViewModel.tem3DRecycle.ToList() },
                        User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                    }, new Reduction() //加入到垃圾桶內
                    {
                        SelectReference = drawing.CurrentBlockReference,
                        Recycle = new List<List<Entity>>() { ViewModel.tem2DRecycle.ToList() },
                        User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                    });

                    #region 讀NC檔
                    List<GroupBoltsAttr> groupBolts = new List<GroupBoltsAttr>();
                    SteelAttr saDeepClone = (SteelAttr)steelAttr.DeepClone();
                    steelAttr = ViewModel.ReadNCInfo(saDeepClone, ref groupBolts, false);
                    #endregion

                    #region 孔位資訊
                    List<Block> blocks = model.GetBoltFromBlock(groupBolts);
                    #endregion

                    #region 塞值
                    steelAttr.Creation = DateTime.Now;
                    steelAttr.Revise = DateTime.Now;
                    steelAttr.Length = ViewModel.ProductLengthProperty;

                    double maxX = steelAttr.oPoint.Union(steelAttr.uPoint).Union(steelAttr.vPoint).Select(x => x.X).Max();
                    double minX = steelAttr.oPoint.Union(steelAttr.uPoint).Union(steelAttr.vPoint).Select(x => x.X).Min();
                    double midX = (minX + maxX) / 2;
                    // 長度差
                    double diffLength = maxX - ViewModel.ProductLengthProperty;
                    #endregion

                    #region 產生模型(有凹槽)
                    model.LoadNcToModel(gridItem.DataName, ObSettingVM.allowType, diffLength, null, steelAttr, modelAllBoltList, blocks);
                    ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊 
                    #endregion

                    #region 孔
                    // 取出斜邊打點
                    ViewModel.GetHypotenusePoint(model, modelAllBoltList);
                    ViewModel.AddBolts(model, drawing, out bool checkRef, blocks);

                    // 切割線打點
                    //ScrollViewbox.IsEnabled = !model.RunHypotenuseEnable();
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);

                    if (!Bolts3DBlock.CheckBolts(model))
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                        WinUIMessageBox.Show(null,
                                     $"孔群落入非加工區域，請再確認",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                      FloatingMode.Window);
                    }
                    else
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                    }

                    #endregion


                    if (!ViewModel.fNewPart.Value)
                    {
                        SaveModel(true, true, oldPartGuid);//存取檔案
                    }

                    // // 讀NC檔
                    // var profile = ser.GetSteelAttr();
                    // Steel3DBlock s3Db = new Steel3DBlock();
                    // SteelAttr steelAttrNC = new SteelAttr();
                    // List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();

                    // SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                    // sa.Revise = DateTime.Now;
                    // //List<GroupBoltsAttr> modelAllBoltList = model.Blocks.SelectMany(x => x.Entities).Where(y => y.GetType() == typeof(BlockReference) && y.EntityData.GetType() == typeof(GroupBoltsAttr) && ((GroupBoltsAttr)y.EntityData).Mode == AXIS_MODE.PIERCE).Select(y => (GroupBoltsAttr)y.EntityData).ToList();
                    // // 使用dm上的孔做還原，不使用NC檔中的孔
                    // s3Db.ReadNcFile(allPath, profile, steelAttr, ref steelAttrNC, ref groups);
                    // //steelAttr.GUID = Guid.Parse(gridItem.DataName);
                    // GetViewToSteelAttr(steelAttr, false, Guid.Parse(gridItem.DataName));
                    // steelAttr.oPoint = steelAttrNC.oPoint;
                    // steelAttr.vPoint = steelAttrNC.vPoint;
                    // steelAttr.uPoint = steelAttrNC.uPoint;
                    // steelAttr.CutList = steelAttrNC.CutList;
                    // steelAttr.Creation = DateTime.Now;
                    // steelAttr.Revise = DateTime.Now;
                    // //steelAttr.Type = (OBJECT_TYPE)ViewModel.SteelTypeProperty_int;
                    // steelAttr.Length = ViewModel.ProductLengthProperty;
                    // //steelAttr.Profile = ViewModel.SteelSectionProperty;
                    // //steelAttr.Length = ViewModel.ProductLengthProperty;//1117 張燕華 加入使用者輸入長度賦值
                    // //steelAttr.Name = ViewModel.ProductNameProperty;
                    // //steelAttr.Phase = ViewModel.PhaseProperty;
                    // //steelAttr.ShippingNumber = ViewModel.ShippingNumberProperty;
                    // //steelAttr.Title1 = ViewModel.Title1Property;
                    // //steelAttr.Title2 = ViewModel.Title2Property;
                    // //steelAttr.Number = (int)ViewModel.ProductCountProperty;
                    // //steelAttr.Material = ViewModel.ProductMaterialProperty;
                    // //steelAttr.PointBack = ViewModel.SteelAttr.PointBack;
                    // //steelAttr.PointTop = ViewModel.SteelAttr.PointTop;
                    // //steelAttr.PointFront = ViewModel.SteelAttr.PointFront;



                    ////double maxX = steelAttr.oPoint.Union(steelAttr.uPoint).Union(steelAttr.vPoint).Select(x => x.X).Max();
                    ////double minX = steelAttr.oPoint.Union(steelAttr.uPoint).Union(steelAttr.vPoint).Select(x => x.X).Min();
                    ////double midX = (minX + maxX) / 2;
                    ////// 長度差
                    ////double diffLength = maxX - ViewModel.ProductLengthProperty;


                    // #region 產生模型方式一(有凹槽)
                    // model.LoadNcToModel(gridItem.DataName, ObSettingVM.allowType, diffLength, null, sa, modelAllBoltList, blocks) ;
                    // SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊 
                    // #endregion

                    // #region 產生模型方式二(無凹槽)
                    // //var block1 = new Steel3DBlock(Steel3DBlock.GetProfile(steelAttr));//改變讀取到的圖塊變成自訂義格式
                    // //model.SetCurrent(null);
                    // //if (model.Blocks.Count > 1)
                    // //{
                    // //    model.Blocks.RemoveAt(1);
                    // //}
                    // //model.Blocks.Insert(1, block1);//加入鋼構圖塊到模型
                    // //BlockReference blockReference = new BlockReference(0, 0, 0, block1.Name, 1, 1, 1, 0);
                    // //blockReference.EntityData = steelAttr;
                    // //blockReference.Selectable = false;//關閉用戶選擇
                    // //blockReference.Attributes.Add("Steel", new AttributeReference(0, 0, 0));
                    // //model.Entities.Add(blockReference);//加入參考圖塊到模型
                    // //
                    // //drawing.Entities.ForEach(el => el.Selected = true);
                    // //steel2D = new Steel2DBlock((Mesh)block1.Entities[0], "123").Entities.ToList();
                    // //Steel2DBlock steel2DBlock2 = (Steel2DBlock)drawing.Blocks[reference2D.BlockName];//drawing.CurrentBlockReference.BlockName→1
                    // //steel2DBlock2.ChangeMesh((Mesh)block1.Entities[0]); 
                    // //drawing.Entities.Clear();
                    // //drawing.Entities.AddRange(steel2D);
                    // #endregion

                    // //bool hasOutSteel = false;
                    // List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();

                    // // 取出斜邊打點
                    // ViewModel.GetHypotenusePoint(model, modelAllBoltList);
                    // //var hb = model.Blocks.SelectMany(x => x.Entities.Select(y => y.EntityData));
                    // //foreach (GroupBoltsAttr item in hb.Where(x => x.GetType() == typeof(GroupBoltsAttr) && ((GroupBoltsAttr)x).Mode == AXIS_MODE.HypotenusePOINT))
                    // //{
                    // //    modelAllBoltList.Add((GroupBoltsAttr)item);
                    // //}
                    // ViewModel.AddBolts(model, drawing, out bool hasOutSteel, blocks);

                    // if (!Bolts3DBlock.CheckBolts(model))
                    // {
                    //     ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                    //     ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                    //     WinUIMessageBox.Show(null,
                    //                  $"孔群落入非加工區域，請再確認",
                    //                  "通知",
                    //                  MessageBoxButton.OK,
                    //                  MessageBoxImage.Exclamation,
                    //                  MessageBoxResult.None,
                    //                  MessageBoxOptions.None,
                    //                   FloatingMode.Window);
                    // }
                    // else
                    // {
                    //     ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                    //     ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                    // }


                    //foreach (GroupBoltsAttr bolt in modelAllBoltList)
                    //{
                    //    Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts(bolt, model, out BlockReference blockRef, out bool checkRef);
                    //    if (bolts3DBlock.hasOutSteel)
                    //    {
                    //        hasOutSteel = true;
                    //    }
                    //    Add2DHole(bolts3DBlock);//加入孔位到2D
                    //}
                    //foreach (Bolts3DBlock bolt in B3DB)
                    //{
                    //    BlockReference referenceBolts = Add2DHole(bolt);//加入孔位到2D
                    //}
                    //刪除指定物件
                    //model.Blocks[1].Entities[0] = block1.Entities[0];
                    //model.Entities.RemoveAt(model.Entities.Count() - 1);
                    //清空選擇物件
                    ViewModel.Select2DItem.Clear();
                    ViewModel.Select3DItem.Clear();
                    //清空圖塊內物件
                    ViewModel.tem3DRecycle.Clear();
                    ViewModel.tem2DRecycle.Clear();

                    ViewModel.Reductions.AddContinuous(new List<Entity>() { (Mesh)model.Blocks[1].Entities[0] }, steel2D);
                    //model.Entities.Insert(model.Entities.Count() - 1, (Mesh)block1.Entities[0]); 
                    #endregion
                }

                //if (!Bolts3DBlock.CheckBolts(model))
                //{
                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                //}
                //else {
                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                //}


                //ManHypotenusePoint(FACE.TOP);
                //ManHypotenusePoint(FACE.FRONT);
                //ManHypotenusePoint(FACE.BACK);

                //if (!ViewModel.fNewPart.Value)
                //{
                //    //model.Blocks.Regen();
                //    //model.Entities.Regen();
                //    SaveModel(true, true);//存取檔案
                //    //await SaveModelAsync(true);
                //}

                #region tempNewSource = ItemSource + New Data
                // 斜邊打點已存
                //ObservableCollection<ProductSettingsPageViewModel> tempNewSource = new ObservableCollection<ProductSettingsPageViewModel>(ViewModel.GetData());
                //int oriIndex = tempNewSource.FindIndex(x => x.DataName == row.DataName);
                //tempNewSource.Remove(row);
                //tempNewSource.Insert(oriIndex,tempSteelAttr);
                #endregion
                ObservableCollection<ProductSettingsPageViewModel> tempNewSource = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                PieceListGridControl.ItemsSource = tempNewSource;
                PieceListGridControl.RefreshData();
                // 取得該GUID資料
                PreIndex = tempNewSource.FindIndex(x => x.DataName == (steelAttr).GUID.ToString() && x.AssemblyNumber == steelAttr.AsseNumber);
                PieceListGridControl.View.FocusedRowHandle = PreIndex;
                PieceListGridControl.SelectItem(PreIndex);
                //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{((steelAttr).Type).ToString()}.inp");
                cbx_SectionTypeComboBox.Text = (steelAttr).Profile;
                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                ViewModel.fclickOK = false;

                // 斜邊打點已存
                ////if (!ViewModel.fAddSteelPart)
                if (!ViewModel.fNewPart.Value)
                    SaveModel(false, true, oldPartGuid);//存取檔案

                //刷新模型
                model.Invalidate();
                drawing.Invalidate();
                model.SetCurrent(null);
                drawing.SetCurrent(null);
                model.ZoomFit();
                drawing.ZoomFit();
                model.Refresh();
                drawing.Refresh();

                WinUIMessageBox.Show(null,
                        $"零件已修改",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);

                ViewModel.fclickOK = false;
#if DEBUG
                log4net.LogManager.GetLogger("ModifyPart").Debug("");
                log4net.LogManager.GetLogger("修改主件").Debug("結束");
                //Thread.Sleep(1000);
                //ViewModel.FileOverView.Execute(null);
#endif
            });
            //讀取主零件
            ViewModel.ReadPart = new RelayCommand(() =>
            {
                //如果是在編輯模式
                if (model.CurrentBlockReference != null)
                {
                    WinUIMessageBox.Show(null,
                    $"退出編輯模式，才可讀取主件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
                //如果模型裡面有物件
                else if (model.Entities.Count > 0)
                {
                    model.SetCurrent((BlockReference)model.Blocks[model.CurrentBlock.Name].Entities.Where(x => x.EntityData.GetType().Name == "SteelAttr").LastOrDefault());
                    ViewModel.WriteSteelAttr((SteelAttr)(model.Blocks[model.CurrentBlock.Name].Entities.Where(x => x.EntityData.GetType().Name == "SteelAttr")).LastOrDefault().EntityData);
                    if (string.IsNullOrEmpty(ViewModel.SteelAttr.PartNumber))
                    {
                        // 若ViewModel.SteelAttr.PartNumber代表取值又失敗了，只好強制給值囉~
                        GetViewToViewModel(false, Guid.Parse(model.CurrentBlock.Name));
                    }
                    //model.SetCurrent((BlockReference)model.Entities.Where(x => x.GetType().Name == "BlockReference" && x.EntityData.GetType().Name == "SteelAttr").LastOrDefault());
                    //ViewModel.WriteSteelAttr((SteelAttr)(model.Entities.Where(x => x.GetType().Name == "BlockReference" && x.EntityData.GetType().Name == "SteelAttr").LastOrDefault()).EntityData);
                    // 寫入主件設定檔 To VM
                    //model.SetCurrent((BlockReference)   model.Entities[model.Entities.Count - 1]);  // 取得主件資訊
                    //ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData); // 寫入主件設定檔 To VM
                    model.SetCurrent(null);  // 返回最上層
                }
                else
                {
                    WinUIMessageBox.Show(null,
                    $"模型內找不到物件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
            });
            //另存加入零件 20220902 張燕華
            ViewModel.AddNewOne = new RelayCommand(() =>
            {
                //在此撰寫程式碼..
            });
            //檔案總覽
            ViewModel.FileOverView = new RelayCommand(() =>
            {
                //  List<SteelAttr> AllRH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE.RH).ToString()}.inp").ToList();
                //  var DELListRH = AllRH
                // .Where(x => (x.Profile.Substring(0, 2) != "RH" && x.Type == OBJECT_TYPE.RH)).ToList();

                //  List<SteelAttr> AllBH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE.BH).ToString()}.inp").ToList();
                //  var DELListBH = AllBH
                //.Where(x => (x.Profile.Substring(0, 2) != "BH" && x.Type == OBJECT_TYPE.RH)).ToList();








                ProcessingScreenWin.Show(inputBlock: InputBlockMode.None, timeout: 100);
                ExcelBuyService execl = new ExcelBuyService();
                //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls", MaterialDataViews);

                var stringFilePath = $@"{ApplicationVM.DirectoryModel()}\檔案概況{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls";


                //ObSettingVM obVM = new ObSettingVM();
                List<string> dmList = appVM.GetAllDevPart();
                List<object> modelBlockList = new List<object>();
                List<object> modelEntityList = new List<object>();
                bool success = true;
                string guid = "";
                List<string> wrongGUIDList = new List<string>();
                Thread.Sleep(1000);
                int i = 1;

                _BufferModel = new devDept.Eyeshot.Model();
                _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
                _BufferModel.InitializeViewports();
                _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());

                foreach (var dataName in dmList)
                {
                    try
                    {
                        ProcessingScreenWin.ViewModel.Status = $"讀取{dataName}中 ..{i}/{dmList.Count()}";
                        ProcessingScreenWin.ViewModel.Progress = (i * 100) / dmList.Count;
                        //Thread.Sleep(500); //暫停兩秒為了要顯示 ScreenManager
                        //guid = dataName;
                        ModelExt modelExt = new ModelExt();
                        modelExt = GetFinalModel(dataName);
                        Entity[] entities = new Entity[model.Entities.Count];
                        Block[] blocks = new Block[model.Blocks.Count];
                        model.Entities.CopyTo(entities, 0);
                        model.Blocks.CopyTo(blocks, 0);
                        //_BufferModel.Clear();
                        //ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{dataName}.dm", devDept.Serialization.contentType.GeometryAndTessellation);
                        //readFile.DoWork();
                        //readFile.AddToScene(_BufferModel);
                        //_BufferModel.Invalidate();

                        //Entity[] entities = new Entity[_BufferModel.Entities.Count];
                        //Block[] blocks = new Block[_BufferModel.Blocks.Count];
                        //_BufferModel.Entities.CopyTo(entities, 0);
                        //_BufferModel.Blocks.CopyTo(blocks, 0);


                        modelBlockList.Add(blocks);
                        modelEntityList.Add(entities);
                        //modelBlockList.Add(model.Blocks);
                        //modelEntityList.Add(model.Entities);
                    }
                    catch (Exception)
                    {
                        success = false;
                        wrongGUIDList.Add(guid);
                    }
                    i++;
                }

                execl.CreateFileOverView(stringFilePath, modelBlockList, modelEntityList, wrongGUIDList);

                ProcessingScreenWin.Close();

                WinUIMessageBox.Show(null,
                    $"檔案已下載",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);

            });
            //OK鈕 20220902 張燕華
            ViewModel.OKtoConfirmChanges = new RelayCommand(() =>
            {
                //if (fAddSteelPart)
                //{
                //    WinUIMessageBox.Show(null,
                //    $"新增零件已存檔",
                //    "通知",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Exclamation,
                //    MessageBoxResult.None,
                //    MessageBoxOptions.None,
                //     FloatingMode.Window);

                //    SaveModel(true);//存取檔案OKtoConfirmChanges
                //    fAddSteelPart=false;
                //}

                // 直接按OK
                if (ViewModel.fclickOK.Value)
                {
                    var ResultRtn = WinUIMessageBox.Show(null,
                         $"請選擇執行動作[新增] / [修改]",
                         "通知",
                         MessageBoxButton.OK,
                         MessageBoxImage.Exclamation,
                         MessageBoxResult.None,
                         MessageBoxOptions.None,
                          FloatingMode.Window);
                    return;
                }




                if (PieceListGridControl.SelectedItem == null)
                {
                    WinUIMessageBox.Show(null,
                        $"請選擇清單中欲執行之項目",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
                    return;
                }



                #region 取得目前選取列的GUID
                // 取得目前選取列的GUID
                string selectGUID = "";
                if (PieceListGridControl.VisibleRowCount > 0)
                {
                    selectGUID = ((ProductSettingsPageViewModel)PieceListGridControl.SelectedItem).DataName;
                    // 選取列是否有dm檔，有的話為修改，否則為新增
                    if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{selectGUID}.dm"))
                    {
                        if (!DataCheck("add")) { ViewModel.fclickOK = true; return; }
                    }
                    else
                    {
                        if (!DataCheck("edit")) { ViewModel.fclickOK = true; return; }
                    }
                }
                else
                {
                    // 若Grid無資料，則新建一筆暫存資料
                    ViewModel.AddPart.Execute(null);
                }
                #endregion

                // 最後一筆是否為新零件
                if (this.PieceListGridControl.VisibleRowCount > 0)
                {
                    ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.GetRow(PieceListGridControl.VisibleRowCount - 1);
                    row.steelAttr.GUID = Guid.Parse(row.DataName);
                    ProductSettingsPageViewModel temp = RowToEntity(row);
                    //新零件
                    if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{temp.steelAttr.GUID}.dm"))
                    {
                        ViewModel.fNewPart = true;
                    }
                }

                if (
                ViewModel.fNewPart.Value ||          // 新零件
                (ViewModel.fFirstAdd.Value && !ViewModel.fNewPart.Value)  // 尚未按新增 & 非新零件(新增零件OK後或Grid切換再按OK)
                )
                {
                    var ResultRtn = WinUIMessageBox.Show(null,
                    $"零件是否存檔 ?",
                    "通知",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                    if (ResultRtn == MessageBoxResult.Yes)
                    {
                        if (this.PieceListGridControl.VisibleRowCount > 0)
                        {
                            // 取得最後一筆dm檔
                            ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.GetRow(PieceListGridControl.VisibleRowCount - 1);
                            row.steelAttr.GUID = Guid.Parse(row.DataName);
                            ProductSettingsPageViewModel temp = RowToEntity(row);
                            if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{temp.steelAttr.GUID}.dm"))
                            {
                                //if (!Bolts3DBlock.CheckBolts(model))
                                //{
                                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                                //}
                                //else
                                //{
                                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                                //}
                                // Grid有資料且無dm檔，直接存，表示已按新增
                                SaveModel(true, true);
                            }
                            else
                            {
                                // 若最後一行GUID有值，代表可能為舊零件 或 直接按OK的新零件
                                // 若選取的GUID
                                if (string.IsNullOrEmpty(selectGUID))
                                {

                                }
                                //if (!Bolts3DBlock.CheckBolts(model))
                                //{
                                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                                //}
                                //else
                                //{
                                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                                //    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                                //}
                                // 已存在dm檔，表示要新增的零件尚未按新增按紐，先直接儲存
                                //ViewModel.AddPart.Execute(null);
                                SaveModel(true, true);
                                // 選取的零件有dm檔，表示為舊零件
                                // 比對GUID是否存在，不存在  代表新零件
                                STDSerialization ser = new STDSerialization();
                                ObservableCollection<DataCorrespond> DataCorrespond = ser.GetDataCorrespond();
                                if (string.IsNullOrEmpty(selectGUID) && File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{selectGUID}.dm"))
                                {
                                    if (DataCorrespond.Any(x => x.DataName == selectGUID))
                                    {

                                    }
                                    ViewModel.fNewPart = true;
                                }
                            }
                        }
                        else
                        {
                            // Grid無資料，直接存
                            //ViewModel.AddPart.Execute(null);
                            SaveModel(true, true);
                        }

                        WinUIMessageBox.Show(null,
                        $"零件已存檔",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
                        //fFirstAdd = true;
                        //fNewPart = false;
                        //fGrid = false;
                        //ViewModel.StateParaSetting(null, null, null);
                        GridReload();

                        ViewModel.fclickOK = true;
                        // 取得最新資料集
                        ViewModel.DataViews = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                        // 取得該GUID資料
                        PreIndex = ViewModel.DataViews.FindIndex(x => x.DataName == selectGUID);
                        // Grid 指標指於該 Guid
                        PieceListGridControl.View.FocusedRowHandle = PreIndex;
                        PieceListGridControl.SelectItem(PreIndex);
                        //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(ViewModel.DataViews[PreIndex].Type).ToString()}.inp");
                        //cbx_SectionTypeComboBox.Text = ViewModel.DataViews[PreIndex].Profile;
                        ConfirmCurrentSteelSection(((ProductSettingsPageViewModel)PieceListGridControl.SelectedItem));
                    }
                    else
                    {
                        ViewModel.fclickOK = true;
                        GridReload();
                        // 放棄，清空屬性，零件清單重載
                        #region 清空零件屬性
                        // 清空零件屬性
                        ViewModel.AssemblyNumberProperty = String.Empty;
                        ViewModel.PartNumberProperty = String.Empty;
                        ViewModel.ProductCountProperty = 0;
                        ViewModel.ProductLengthProperty = 0;
                        ViewModel.ProductWeightProperty = 0;
                        ViewModel.ProductNameProperty = String.Empty;
                        ViewModel.PhaseProperty = null;
                        ViewModel.ShippingNumberProperty = null;
                        ViewModel.Title1Property = String.Empty;
                        ViewModel.Title2Property = String.Empty;
                        ViewModel.ProductMaterialProperty = "";
                        this.asseNumber.Clear();
                        this.partNumber.Clear();
                        this.PartCount.Clear();
                        this.Length.Clear();
                        this.Weight.Text = "";
                        this.PartCount.Clear();
                        this.teklaName.Clear();
                        this.phase.Clear();
                        this.shippingNumber.Clear();
                        this.Title1.Clear();
                        this.Title2.Clear();
                        this.cbx_SteelTypeComboBox.SelectedIndex = 0;
                        this.cbx_SectionTypeComboBox.SelectedIndex = 0;
                        #endregion
                        this.PieceListGridControl.SelectItem(0);
                    }
                    ViewModel.StateParaSetting(null, null, null);
                }

                ViewModel.fclickOK = true;
#if DEBUG
                //ViewModel.FileOverView.Execute(null);
#endif
            });
            //加入孔
            ViewModel.AddHole = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("加入孔位").Debug("產生圖塊");
#endif

                if (!ViewModel.CheckData_AddHole(ViewModel.PartNumberProperty, model))
                {
                    return;
                }




                //if (ComparisonBolts())  // 欲新增孔位重複比對
                //{
                //    WinUIMessageBox.Show(null,
                //    $"新增孔位重複，請重新輸入",
                //    "通知",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Exclamation,
                //    MessageBoxResult.None,
                //    MessageBoxOptions.None,
                //     FloatingMode.Window);
                //   ViewModel.fclickOK = true;
                //    return;
                //}


                SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                sa = GetViewToSteelAttr(sa, false, sa.GUID);
                sa.Revise = DateTime.Now;
                GetViewToViewModel(false, ViewModel.GuidProperty);
                ViewModel.SteelAttr = sa;

                /*3D螺栓*/
                ViewModel.GroupBoltsAttr.GUID = Guid.NewGuid();
                ViewModel.SteelAttr.Name = ViewModel.ProductNameProperty;
                ViewModel.SteelAttr.Phase = ViewModel.PhaseProperty;
                ViewModel.SteelAttr.ShippingNumber = ViewModel.ShippingNumberProperty;
                ViewModel.SteelAttr.Title1 = ViewModel.Title1Property;
                ViewModel.SteelAttr.Title2 = ViewModel.Title2Property;
                ViewModel.GetSteelAttr();

                Steel3DBlock.FillCutSetting(sa);

                Bolts3DBlock bolts = Bolts3DBlock.AddBolts(ViewModel.GetGroupBoltsAttr(), model, out BlockReference blockReference, out bool check);

                if (ViewModel.fromModifyHole)
                {
                    if (!check && ViewModel.showMessage)
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                        WinUIMessageBox.Show(null,
                                     $"孔群落入非加工區域，請再確認",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                      FloatingMode.Window);
                        ViewModel.fclickOK = true;
                        //return;
                    }
                    else
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;

                        ViewModel.fclickOK = false;
                    }
                }
                else
                {
                    if (!check)
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                        WinUIMessageBox.Show(null,
                                     $"孔群落入非加工區域，請再確認",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                      FloatingMode.Window);
                        ViewModel.fclickOK = true;
                        //return;
                    }
                    else
                    {
                        ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                        ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;

                        ViewModel.fclickOK = false;
                    }
                }
                BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                                                                 //if (!ViewModel.fAddSteelPart)

                //if (!ViewModel.fNewPart.Value)
                SaveModel(false, true);//存取檔案

                //不是修改孔位狀態
                if (!ViewModel.modifyHole)
                {
                    ViewModel.Reductions.Add(new Reduction()
                    {
                        Recycle = new List<List<Entity>>() { new List<Entity>() { blockReference } },
                        SelectReference = null,
                        User = new List<ACTION_USER>() { ACTION_USER.Add }
                    }, new Reduction()
                    {
                        Recycle = new List<List<Entity>>() { new List<Entity>() { referenceBolts } },
                        SelectReference = null,
                        User = new List<ACTION_USER>() { ACTION_USER.Add }
                    });
                }

                //// 符合孔位加入件
                //if (check)
                //{

                //    ////刷新模型
                //    //model.Refresh();
                //    //drawing.Refresh();
                //    //SaveModel(false);//存取檔案
                //}
                //else
                //{

                //    return;
                //}
                //刷新模型
                model.Refresh();
                drawing.Refresh();

                //if (!ViewModel.fAddSteelPart)
                //    SaveModel(false);//存取檔案

            });
            //修改孔
            ViewModel.ModifyHole = new RelayCommand(() =>
            {
                //查看用戶是否有選擇圖塊
                if (ViewModel.Select3DItem.Count > 0)
                {
                    if (!ViewModel.CheckData_ModifyHole(ViewModel.PartNumberProperty, model))
                    {
                        return;
                    }

                    ViewModel.showMessage = false;
                    ViewModel.fromModifyHole = true;
                    List<SelectedItem> selectItem = ViewModel.Select3DItem.ToList();//暫存容器
                    GroupBoltsAttr original = (GroupBoltsAttr)ViewModel.GroupBoltsAttr.DeepClone(); //原有設定檔
                    selectItem.ForEach(el => el.Item.Selected = false);//取消選取
                    for (int i = 0; i < selectItem.Count; i++)
                    {
                        ViewModel.fromModifyHole = true;
                        if (i == selectItem.Count - 1)
                        {
                            ViewModel.showMessage = true;
                        }
                        BlockReference blockReference = (BlockReference)selectItem[i].Item;
                        //如果在編輯模式
                        if (model.CurrentBlockReference != null && ViewModel.showMessage)
                        {
                            WinUIMessageBox.Show(null,
                                $"出編輯模式，才可修改孔",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                 FloatingMode.Window);
                            return;
                        }
                        //如果選擇的物件不是孔位
                        else if ((blockReference == null || blockReference.EntityData.Equals(typeof(GroupBoltsAttr))) && ViewModel.showMessage)
                        {
                            WinUIMessageBox.Show(null,
                                $"選擇類型必須是孔，才可修改",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                 FloatingMode.Window);
                            return;

                        }
#if DEBUG
                        log4net.LogManager.GetLogger("修改孔位").Debug("開始");

#endif
                        SelectedItem sele = new SelectedItem(blockReference);
                        sele.Item.Selected = true;
                        GroupBoltsAttr groupBoltsAttr = (GroupBoltsAttr)blockReference.EntityData;//存取用戶設定檔                                                                                         
                                                                                                  //開啟Model焦點
                        bool mFocus = model.Focus();
                        if (!mFocus)
                        {
                            drawing.Focus();
                        }
                        ViewModel.GroupBoltsAttr = ViewModel.GetGroupBoltsAttr(groupBoltsAttr);
                        ViewModel.Select3DItem.Add(selectItem[i]);//模擬選擇
                        SimulationDelete();//模擬按下 delete 鍵
                        ViewModel.modifyHole = true;
                        model.Refresh();
                        //ViewModel.ignoreMessage = true;
                        ViewModel.AddHole.Execute(null);
                        //ViewModel.ignoreMessage = false;
                    }
                    Esc();
                    ViewModel.modifyHole = false;
                    ViewModel.GroupBoltsAttr = original;
                    ViewModel.fromModifyHole = false;
                    ViewModel.showMessage = true;

                    model.Invalidate();//刷新模型
                    //if (!ViewModel.fAddSteelPart)
                    if (!ViewModel.fNewPart.Value)
                        SaveModel(false);//存取檔案
#if DEBUG
                    log4net.LogManager.GetLogger("修改孔位").Debug("結束");
#endif
                }
                else
                {
                    WinUIMessageBox.Show(null,
                    $"請選擇孔，才可修改",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
            });
            //讀取孔位
            ViewModel.ReadHole = new RelayCommand(() =>
            {
                BlockReference blockReference;
                //查看用戶是否有選擇圖塊
                if (ViewModel.Select3DItem.Count > 0)
                {
                    blockReference = (BlockReference)ViewModel.Select3DItem[0].Item;
                    //如果在編輯模式
                    if (model.CurrentBlockReference != null)
                    {
                        WinUIMessageBox.Show(null,
                            $"退出編輯模式，才可讀取",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                        return;
                    }
                    //如果選擇的物件不是孔位
                    else if (blockReference == null || blockReference.EntityData.Equals(typeof(GroupBoltsAttr)))
                    {
                        WinUIMessageBox.Show(null,
                            $"選擇類型必須是孔，才可讀取",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                    }
                    ViewModel.WriteGroupBoltsAttr((GroupBoltsAttr)blockReference.EntityData);
                }
                else
                {
                    WinUIMessageBox.Show(null,
                        $"請選擇孔，才可修讀取",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
                }
            });
            //加入或修改切割線
            ViewModel.AddCut = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("加入切割線").Debug("開始");
#endif
                if (!ViewModel.CheckData_AddCutLine(ViewModel.PartNumberProperty, model))
                {
                    return;
                }

                ViewModel.GetSteelAttr();
                ViewModel.ReadPart.Execute(null);
                // 移除斜邊打點
                ViewModel.RemoveHypotenusePoint(model, HypotenuseTYPE.HypotenuseMan.ToString());
                //List<GroupBoltsAttr> delList = model.Blocks
                //.SelectMany(x => x.Entities)
                //.Where(y =>
                //    y.GetType() == typeof(BlockReference) &&
                //    y.EntityData.GetType() == typeof(GroupBoltsAttr) &&
                //    ((GroupBoltsAttr)y.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
                //.Select(y => (GroupBoltsAttr)y.EntityData).ToList();
                //foreach (GroupBoltsAttr del in delList)
                //{
                //    model.Remove(del.GUID.Value.ToString());
                //    model.Blocks.Remove(model.Blocks[del.GUID.Value.ToString()]);
                //}
                // 因為ModifyPart會再讀檔，故儲存
                ViewModel.SaveCut();
                SaveModel(false, false);
                ViewModel.fNewPart = false;//fasle不產生新GUID
                ViewLocator.OfficeViewModel.isPressAddCutKey = true;
                ViewModel.WriteCutAttr((SteelAttr)model.Blocks[1].Entities[0].EntityData);
                ViewModel.ModifyPart.Execute(null);
                ViewModel.fNewPart = true;//還原零件狀態
                ViewLocator.OfficeViewModel.isPressAddCutKey = false;
                //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D三視圖
                //bool hasOutSteel = false;
                //List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();
                //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                //{
                //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                //    {
                //        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                //        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
                //        Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);
                //        //Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
                //        if (bolts3DBlock.hasOutSteel)
                //        {
                //            hasOutSteel = true;
                //        }
                //        B3DB.Add(bolts3DBlock);
                //        //Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                //    }
                //}
                //if (hasOutSteel)
                //{
                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                //}
                //else
                //{
                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                //}
                //foreach (Bolts3DBlock item in B3DB)
                //{
                //    BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
                //}
                //RunHypotenusePoint();
                //ManHypotenusePoint(FACE.FRONT);
                //ManHypotenusePoint(FACE.BACK);
                //ManHypotenusePoint(FACE.TOP);
#if DEBUG
                log4net.LogManager.GetLogger("加入切割線").Debug("結束");
#endif
            });
            //修改切割線設定
            ViewModel.ModifyCut = new RelayCommand(() =>
            {
                //在這裡撰寫程式碼..
            });
            //刪除切割線設定
            ViewModel.DeleteCut = new RelayCommand(() =>
            {
                //在這裡撰寫程式碼..
            });
            //讀取切割線設定
            ViewModel.ReadCut = new RelayCommand(() =>
            {
                //如果是在編輯模式
                if (model.CurrentBlockReference != null)
                {
                    WinUIMessageBox.Show(null,
                    $"退出編輯模式，才可讀取切割線",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
                //如果模型裡面有物件
                else if (model.Entities.Count > 0)
                {
                    model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);
                    ViewModel.WriteCutAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);
                    model.SetCurrent(null);
                }
                else
                {
                    WinUIMessageBox.Show(null,
                    $"模型內找不到物件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                }
            });
            //鏡射 X 軸
            ViewModel.MirrorX = new RelayCommand(() =>
            {
                try
                {
                    //查看用戶是否有選擇圖塊
                    if (ViewModel.Select3DItem.Count > 0)
                    {
                        BlockReference reference3D;
                        BlockReference reference2D;

                        List<SelectedItem> select3D = ViewModel.Select3DItem.ToList();//暫存容器
                        List<SelectedItem> select2D = ViewModel.Select2DItem.ToList();//暫存容器
                        for (int i = 0; i < select3D.Count; i++)
                        {
                            reference3D = select3D.Count != 0 ? (BlockReference)select3D[i].Item : null;
                            reference2D = select2D.Count != 0 ? (BlockReference)select2D[i].Item : null;
                            //如果在編輯模式
                            if (model.CurrentBlockReference != null)
                            {
                                WinUIMessageBox.Show(null,
                                    $"退出編輯模式，才可鏡射",
                                    "通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                     FloatingMode.Window);
                                return;
                            }
                            //如果選擇的物件不是孔位
                            else if (reference3D == null || model.Blocks[reference3D.BlockName].Equals(typeof(Bolts3DBlock)))
                            {
                                WinUIMessageBox.Show(null,
                                    $"選擇類型必須是孔，才可鏡射",
                                    "通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                     FloatingMode.Window);
                            }
#if DEBUG
                            log4net.LogManager.GetLogger("鏡射孔位").Debug("開始");




#endif

                            SelectedItem sele3D = new SelectedItem(reference3D);
                            SelectedItem sele2D = new SelectedItem(reference2D);

                            //模擬用戶實際選擇編輯
                            ViewModel.Select3DItem.Add(sele3D);
                            ViewModel.Select2DItem.Add(sele2D);

                            model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);//先取得主件資訊
                            SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;

                            //產生物件物件頂點(整支素材的最起點和最末端)
                            //var a = model.Entities[model.Entities.Count - 1].Vertices;


                            //TODO:中心座標
                            List<Point3D> points = new List<Point3D>();
                            List<Point3D> finalPoint = new List<Point3D>();
                            Point3D boxMin = new Point3D(), boxMax = new Point3D();
                            //model.Entities.ForEach(el => points.AddRange(el.Vertices));

                            var modelSelected = model.Blocks[reference3D.BlockName].Entities.Select(x => x).ToList();
                            var drawingSelected = drawing.Blocks[reference2D.BlockName].Entities.Where(x => x.Selectable && x.GetType().Name == "Circle").ToList();

                            foreach (var item in modelSelected)
                            {
                                BoltAttr ba = (BoltAttr)item.EntityData;
                                Point3D p = new Point3D();
                                p = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };

                                switch (ba.Face)
                                {
                                    case FACE.TOP:
                                        break;
                                    case FACE.FRONT:
                                    case FACE.BACK:
                                        double y = 0, z = 0;
                                        y = p.Z;
                                        z = p.Y;
                                        p.Y = y;
                                        p.Z = z;
                                        break;
                                    default:
                                        break;
                                }
                                finalPoint.Add(p);
                            }

                            //modelSelected.ForEach(x =>
                            //{
                            //    BoltAttr ba = (BoltAttr)x.EntityData;
                            //    Point3D p = new Point3D();
                            //    p = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                            //    finalPoint.Add(p);
                            //});
                            //drawingSelected.ForEach(x =>
                            //{
                            //    BoltAttr ba = (BoltAttr)x.EntityData;
                            //    Point3D p = new Point3D(ba.X, ba.Y, ba.Z);
                            //    points.Add(p);
                            //});
                            //points.ForEach(p =>
                            //{
                            //    finalPoint.Add(new Point3D { X = p.X, Y = p.Y, Z = p.Z });
                            //});
                            //finalPoint = finalPoint.Distinct().ToList();                     

                            Utility.ComputeBoundingBox(null, finalPoint, out boxMin, out boxMax);
                            Point3D center = (boxMin + boxMax) / 2; //鏡射中心點

                            //通過最大點、最小點及中間值之平面
                            Point3D p31 = new Point3D { X = boxMax.X, Y = boxMax.Y, Z = boxMax.Z };
                            Point3D p32 = new Point3D { X = boxMin.X, Y = boxMin.Y, Z = boxMin.Z };
                            Point3D p33 = new Point3D { X = (boxMax.X + boxMin.X) / 2, Y = (boxMax.Y + boxMin.Y) / 2, Z = (boxMax.Z + boxMin.Z) / 2 };
                            Point3D p21 = new Point3D { X = boxMax.X, Y = boxMax.Y, Z = boxMax.Z };
                            Point3D p22 = new Point3D { X = boxMin.X, Y = boxMin.Y, Z = boxMin.Z };

                            model.SetCurrent(null);
                            model.SetCurrent(reference3D);
                            drawing.SetCurrent(reference2D);
                            //存放孔位資訊
                            Entity[] buffer3D = new Entity[model.Entities.Count]; //3D 鏡射物件緩衝區
                            Entity[] buffer2D = new Entity[drawing.Entities.Count]; //2D 鏡射物件緩衝區
                            model.Entities.CopyTo(buffer3D, 0);
                            drawing.Entities.CopyTo(buffer2D, 0);

                            //模擬選取
                            ViewModel.Reductions.Add(new Reduction()
                            {
                                Recycle = new List<List<Entity>>() { buffer3D.ToList() },
                                SelectReference = reference3D,
                                User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                            }, new Reduction()
                            {
                                Recycle = new List<List<Entity>>() { buffer2D.ToList() },
                                SelectReference = reference2D,
                                User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                            });

                            //if (model.Entities[model.Entities.Count - 1].EntityData is GroupBoltsAttr boltsAttr)
                            //{
                            //    BlockReference blockReference = (BlockReference)model.Entities[model.Entities.Count - 1]; //取得參考圖塊
                            //    Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                            //    Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊

                            //}
                            //Bolts3DBlock groupBolts = (Bolts3DBlock)model.Blocks[reference3D.BlockName];//轉換型別
                            BoltAttr boltAttr = (BoltAttr)model.Entities[model.Entities.Count - 1].EntityData;
                            FACE face = boltAttr.Face;














                            #region 原始寫法
                            ////3D鏡射參數
                            //Vector3D axis3DX = new Vector3D();
                            //Plane mirror3DPlane = new Plane();
                            //Point3D p3D1 = new Point3D(), p3D2 = new Point3D();//鏡射座標
                            //Vector3D axis3D = new Vector3D();//鏡射軸

                            ////2D鏡射
                            //Vector3D axis2DX = new Vector3D();
                            //Plane mirror2DPlane = new Plane();
                            //Point3D p2D1 = new Point3D(0, 0), p2D2 = new Point3D(10, 0);//鏡射座標
                            //Vector3D axis2D = Vector3D.AxisZ;//鏡射軸
                            //Bolts2DBlock bolts2DBlock = (Bolts2DBlock)drawing.Blocks[reference2D.BlockName];

                            //switch (face)
                            //{
                            //    case GD_STD.Enum.FACE.TOP:

                            //        p3D1 = new Point3D(0, center.Y, 0); //鏡射第一點
                            //        p3D2 = new Point3D(10, center.Y, 0);//鏡射第二點
                            //        axis3D = Vector3D.AxisZ;

                            //        p2D1.Y = p2D2.Y = steelAttr.H / 2;
                            //        break;
                            //    case GD_STD.Enum.FACE.BACK:
                            //    case GD_STD.Enum.FACE.FRONT:
                            //        p3D1 = new Point3D(0, 0, center.Z); //鏡射第一點
                            //        p3D2 = new Point3D(10, 0, center.Z);//鏡射第二點
                            //        axis3D = Vector3D.AxisY;
                            //        switch (face)
                            //        {
                            //            case FACE.FRONT:
                            //                p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.W / 2;
                            //                break;
                            //            case FACE.BACK:
                            //                p2D1.Y = p2D2.Y = bolts2DBlock.MoveBack - steelAttr.W / 2;
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        break;
                            //    default:
                            //        break;
                            //}

                            ////清除要鏡射的物件
                            //model.Entities.Clear();
                            //drawing.Entities.Clear();

                            ////修改 3D 參數
                            //axis3DX = new Vector3D(p3D1, p3D2);

                            ////修改 2D 參數
                            //axis2DX = new Vector3D(p2D1, p2D2);

                            //mirror3DPlane = new Plane(p3D1, axis3DX, axis3D);
                            //mirror2DPlane = new Plane(p2D1, axis2DX, axis2D);

                            ////鏡像轉換。
                            //Mirror mirror3D = new Mirror(mirror3DPlane);
                            //Mirror mirror2D = new Mirror(mirror2DPlane); 
                            #endregion



                            //FACE face = groupBolts.Info.Face; //孔位的面

                            //3D鏡射參數
                            Point3D p3D1 = new Point3D(), p3D2 = new Point3D(), p3D3 = new Point3D();//鏡射座標

                            //2D鏡射
                            Point3D p2D1 = p21, p2D2 = p22;//鏡射座標
                            Vector3D axis2D = Vector3D.AxisZ;//鏡射軸
                            Vector3D axis3D = new Vector3D();//鏡射軸

                            Bolts2DBlock bolts2DBlock = (Bolts2DBlock)drawing.Blocks[reference2D.BlockName];
                            // 鏡射 X 軸 : 需要三個點
                            // 俯視圖:XY平面.Y軸.中心點
                            // 前後視圖:XZ平面.Z軸.中心點
                            switch (face)
                            {
                                case GD_STD.Enum.FACE.TOP:
                                    p3D1 = new Point3D(0, center.Y, 0); //鏡射第一點
                                    p3D2 = new Point3D(10, center.Y, 0);//鏡射第二點
                                    axis3D = Vector3D.AxisZ;

                                    p2D1.Y = p2D2.Y = steelAttr.H / 2;
                                    break;
                                //p3D1 = p31;
                                //p3D2 = p32;
                                //p3D3 = p33;
                                //axis3D = Vector3D.AxisY;
                                //
                                ////axis3D = new Vector3D(1, 1, 0);
                                ////p2D1.Y = p2D2.Y = steelAttr.H / 2;
                                //p2D1 = new Point3D(center.X, center.Y);
                                //axis2D = Vector3D.AxisY;
                                //break;
                                case GD_STD.Enum.FACE.BACK:
                                case GD_STD.Enum.FACE.FRONT:
                                    p3D1 = new Point3D(0, 0, center.Z); //鏡射第一點
                                    p3D2 = new Point3D(10, 0, center.Z);//鏡射第二點
                                    axis3D = Vector3D.AxisY;
                                    switch (face)
                                    {
                                        case FACE.FRONT:
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.W / 2;
                                            break;
                                        case FACE.BACK:
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveBack - steelAttr.W / 2;
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            //    case GD_STD.Enum.FACE.BACK:
                            //    case GD_STD.Enum.FACE.FRONT:
                            //        //p3D1 = new Point3D(center.X, center.Y, steelAttr.W / 2);//+ steelAttr.W / 2
                            //        //p3D2 = new Point3D(center.X, 0, steelAttr.W / 2);//+ steelAttr.W / 2
                            //        //p3D3 = new Point3D(center.X, center.Y, center.Z+ steelAttr.W / 2);//+ steelAttr.W / 2
                            //        //以Back及FRONT來說，Z=Y,Y=Z
                            //        p3D1 = new Point3D { X = p31.X, Y = p31.Y, Z = p31.Z + steelAttr.W / 2 };//+ steelAttr.W / 2
                            //        p3D2 = new Point3D { X = p32.X, Y = p32.Y, Z = p32.Z + steelAttr.W / 2 };//+ steelAttr.W / 2
                            //        p3D3 = new Point3D { X = p33.X, Y = p33.Y, Z = p33.Z + steelAttr.W / 2 };// + steelAttr.W / 2
                            //        axis3D = Vector3D.AxisX;

                            //        axis2D = Vector3D.AxisX;
                            //        switch (face)
                            //        {
                            //            case FACE.FRONT:
                            //                //axis3D = new Vector3D(0, 0, 0);
                            //                p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.W / 2;
                            //                //p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + (steelAttr.W / 2 - steelAttr.t1 / 2);
                            //                break;
                            //            case FACE.BACK:
                            //                p2D1.Y = p2D2.Y = bolts2DBlock.MoveBack - steelAttr.W / 2;
                            //                //p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront - (steelAttr.W / 2 - steelAttr.t1 / 2);
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        break;
                            //    default:
                            //        break;
                            //}
                            //清除要鏡射的物件
                            model.Entities.Clear();
                            drawing.Entities.Clear();

                            //修改 3D 參數
                            Vector3D axis3DX = new Vector3D(p3D1, p3D2);

                            //修改 2D 參數
                            Vector3D axis2DX = new Vector3D(p2D1, p2D2);

                            Plane mirror3DPlane = new Plane(p3D1, axis3DX, axis3D);
                            Plane mirror2DPlane = new Plane(p2D1, axis2DX, axis2D);

                            //鏡像轉換。
                            Mirror mirror3D = new Mirror(mirror3DPlane);
                            Mirror mirror2D = new Mirror(mirror2DPlane);
                            ////清除要鏡射的物件
                            //model.Entities.Clear();
                            //drawing.Entities.Clear();

                            ////Vector3D axis3DX = new Vector3D();
                            //Vector3D axis2DX = new Vector3D();

                            //修改 3D 參數
                            //axis3DX = new Vector3D(new Point3D(p3D1.X, p3D1.Y, p3D1.Z), new Point3D(p3D2.X, p3D2.Y, p3D2.Z));
                            //修改 2D 參數
                            //axis2DX = new Vector3D(new Point3D(p2D1.X, p2D1.Y, p2D1.Z), new Point3D(p2D2.X, p2D2.Y, p2D2.Z));

                            //mirror3DPlane = new Plane(p3D1, axis3DX, axis3D);
                            //Plane mirror3DPlane = new Plane(p3D1, axisX, axis3D);
                            //Plane mirror2DPlane = new Plane(p2D1, axis2DX, axis2D);


                            //Vector3D axisX = new Vector3D(p3D2, p3D1);
                            //Vector3D axis1X = new Vector3D(p3D3, p3D1);
                            //Plane mirror3DPlane = new Plane(p3D1, axisX, axis1X);
                            //Plane mirror3DPlane = new Plane(p1, p2, p3);
                            //Plane mirror2DPlane = new Plane(p1, axis2D);
                            //p31 = XYPlane(face, p31);
                            //p32 = XYPlane(face, p32);
                            //p33 = XYPlane(face, p33);
                            //p21 = XYPlane(face, p21);
                            //p22 = XYPlane(face, p22);

                            //Plane mirror3DPlane = new Plane(p31, p32, p33);

                            //Plane mirror3DPlane = new Plane(center, axis3D);


                            //Plane mirror3DPlane = new Plane(center, axis3D);
                            //switch (face)
                            //{
                            //    case FACE.TOP:
                            //        mirror3DPlane = new Plane(center, axis3D);
                            //        break;
                            //    case FACE.FRONT:
                            //        double a = center.X, b = center.Y, c = center.Z;
                            //        Vector3D d = Vector3D.AxisZ;
                            //        mirror3DPlane = new Plane(new Point3D(a, b, c), axis3D);
                            //        break;
                            //    case FACE.BACK:
                            //        mirror3DPlane = new Plane(new Point3D(0, center.Y, 0), axis3D);
                            //        break;
                            //    default:
                            //        break;
                            //}


                            //Point3D PPP = new Point3D(0, 0, 50);
                            //Point3D PPPP = new Point3D(1, 0, 50);
                            //Vector3D axis3DX111 = new Vector3D(PPP, PPPP);
                            //mirror3DPlane = new Plane(PPP, axis3DX111);
                            //switch (face)
                            //{
                            //    case FACE.TOP:
                            //        break;
                            //    case FACE.FRONT:
                            //    case FACE.BACK:
                            //        double y = 0, z = 0;
                            //        y = center.Z;
                            //        z = center.Y;
                            //        center.Y = y;
                            //        center.Z = z;
                            //        break;
                            //    default:
                            //        break;
                            //}
                            //Plane mirror2DPlane = new Plane(center, axis2D);
                            //
                            ////鏡像轉換。
                            //Mirror mirror3D = new Mirror(mirror3DPlane);
                            //Mirror mirror2D = new Mirror(mirror2DPlane);

                            List<Entity> modify3D = new List<Entity>(), modify2D = new List<Entity>();
                            foreach (Entity item in buffer3D)
                            {
                                Entity entity = (Entity)item.Clone();
                                entity.TransformBy(mirror3D);
                                modify3D.Add(entity);
                            }
                            foreach (var item in buffer2D)
                            {
                                Entity entity = (Entity)item.Clone();
                                if (entity.Selectable)
                                {
                                    entity.TransformBy(mirror2D);
                                }
                                modify2D.Add(entity);
                            }

                            drawing.Entities.AddRange(modify2D);
                            model.Entities.AddRange(modify3D);

                            #region 若無此段去更改EntityData的XYZ的話，切換零件時，2D顯示會打回異常

                            var baList = new List<BoltAttr>();
                            foreach (var et in model.Blocks[reference3D.BlockName].Entities)
                            {
                                BoltAttr ba = (BoltAttr)et.EntityData;
                                double y = 0, z = 0;
                                Point3D ori = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                                ori.TransformBy(mirror3D);
                                ba.X = ori.X;
                                ba.Y = ori.Y;
                                ba.Z = ori.Z;
                                //y = ba.Z;
                                //z = ba.Y;
                                //ba.Y = y;
                                //ba.Z = z;
                                baList.Add(ba);
                                et.EntityData = ba;
                            }

                            baList.Clear();
                            foreach (var et in drawing.Entities)
                            {
                                BoltAttr ba = (BoltAttr)et.EntityData;
                                Point3D ori = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                                ori.TransformBy(mirror2D);
                                ba.X = ori.X;
                                ba.Y = ori.Y;
                                ba.Z = ori.Z;
                                baList.Add(ba);
                                et.EntityData = ba;
                            }



                            #endregion

                            ViewModel.Reductions.AddContinuous(modify3D, modify2D);
                            model.SetCurrent(null);
                            drawing.SetCurrent(null);
                        }
                        Esc();
                        model.Refresh();//刷新模型
                        drawing.Refresh();

                        SaveModel(true);//存取檔案
#if DEBUG

                        log4net.LogManager.GetLogger("鏡射孔位").Debug("結束");
#endif
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                            $"請選擇孔，才可鏡射",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                    }
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                    throw;
                }
            });
            //鏡射 Y 軸
            ViewModel.MirrorY = new RelayCommand(() =>
            {
                try
                {
                    //查看用戶是否有選擇圖塊
                    if (ViewModel.Select3DItem.Count > 0)
                    {
                        BlockReference reference3D;
                        BlockReference reference2D;

                        List<SelectedItem> select3D = ViewModel.Select3DItem.ToList();//暫存容器
                        List<SelectedItem> select2D = ViewModel.Select2DItem.ToList();//暫存容器

                        for (int i = 0; i < select3D.Count; i++)
                        {
                            reference3D = select3D.Count != 0 ? (BlockReference)select3D[i].Item : null;
                            reference2D = select2D.Count != 0 ? (BlockReference)select2D[i].Item : null;
                            //如果在編輯模式
                            if (model.CurrentBlockReference != null)
                            {
                                WinUIMessageBox.Show(null,
                                    $"退出編輯模式，才可鏡射",
                                    "通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                     FloatingMode.Window);
                                return;
                            }
                            //如果選擇的物件不是孔位
                            else if (reference3D == null || model.Blocks[reference3D.BlockName].Equals(typeof(Bolts3DBlock)))
                            {
                                WinUIMessageBox.Show(null,
                                    $"選擇類型必須是孔，才可鏡射",
                                    "通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                     FloatingMode.Window);
                                return;
                            }
#if DEBUG
                            log4net.LogManager.GetLogger("鏡射孔位").Debug("開始");
#endif
                            SelectedItem sele3D = new SelectedItem(reference3D);
                            SelectedItem sele2D = new SelectedItem(reference2D);

                            //模擬用戶實際選擇編輯
                            ViewModel.Select3DItem.Add(sele3D);
                            ViewModel.Select2DItem.Add(sele2D);

                            model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);//先取得主件資訊
                            SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;

                            model.SetCurrent(null);
                            model.SetCurrent(reference3D);
                            drawing.SetCurrent(reference2D);

                            List<Entity> buffer3D = model.Entities.ToList(), buffer2D = drawing.Entities.ToList();

                            //模擬選取
                            ViewModel.Reductions.Add(new Reduction()
                            {
                                Recycle = new List<List<Entity>>() { buffer3D.ToList() },
                                SelectReference = reference3D,
                                User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                            }, new Reduction()
                            {
                                Recycle = new List<List<Entity>>() { buffer2D.ToList() },
                                SelectReference = reference2D,
                                User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                            });

                            //Bolts3DBlock groupBolts = (Bolts3DBlock)model.Blocks[reference3D.BlockName];//轉換型別
                            BoltAttr BoltAttr = (BoltAttr)model.Entities[model.Entities.Count - 1].EntityData;
                            log4net.LogManager.GetLogger("取得孔位方位").Debug($"{BoltAttr.Face}");
                            FACE face = BoltAttr.Face; //孔位的面

                            //TODO:中心座標
                            List<Point3D> points = new List<Point3D>();
                            Point3D boxMin, boxMax;

                            model.Entities.ForEach(e => points.AddRange(e.Vertices));

                            var modelSelected = model.Blocks[reference3D.BlockName].Entities.Where(x => x.Selectable).ToList();
                            var drawingSelected = drawing.Blocks[reference2D.BlockName].Entities.Where(x => x.Selectable).ToList();


                            //取得所有孔位在該區域中的最邊端 及 求出中心點
                            //model.Entities.ForEach(el => points.AddRange(el.Vertices));
                            Utility.ComputeBoundingBox(null, points, out boxMin, out boxMax);
                            Point3D center = (boxMin + boxMax) / 2; //鏡射中心點
                            log4net.LogManager.GetLogger("取得中心點").Debug($"({center.X},{center.Y},{center.Z})");

                            //鏡射參數
                            Point3D p3D1 = new Point3D(), p3D2 = new Point3D(), p3D3 = new Point3D();

                            Point3D p2D1 = new Point3D(center.X, 0), p2D2 = new Point3D();
                            Vector3D axis2D = new Vector3D();
                            Vector3D axis3D = Vector3D.AxisY;
                            Bolts2DBlock bolts2DBlock = (Bolts2DBlock)drawing.Blocks[reference2D.BlockName];

                            // 鏡射 Y 軸 : 需要三個點
                            // 俯視圖:XY平面.X軸.中心點
                            // 前後視圖:XZ平面.X軸.中心點
                            switch (face)
                            {
                                case FACE.TOP:
                                    //俯視圖 Y軸鏡射 以X座標為中心 Z軸不變 移動Y座標(Y軸鏡射)
                                    p3D1 = new Point3D(center.X, center.Y, 1);
                                    p3D2 = new Point3D(center.X, 0, 0);
                                    p3D3 = new Point3D(center.X, center.Y, center.Z);
#if DEBUG
                                    log4net.LogManager
                                    .GetLogger($"TOP 3D 依據 以下三點之構面\n")
                                    .Debug($"({p3D1.X},{p3D1.Y},{p3D1.Z})\n({p3D2.X},{p3D2.Y},{p3D2.Z})\n({p3D3.X},{p3D3.Y},{p3D3.Z})\n");
#endif
                                    //p2D1.X = p2D2.X = steelAttr.W / 2;
                                    p2D1 = new Point3D(center.X, 0, 0);
                                    axis2D = Vector3D.AxisX;
#if DEBUG
                                    log4net.LogManager
                                    .GetLogger($"BACK 2D 依據 以下之構面\n")
                                    .Debug($"({p2D1.X},{p2D1.Y},{p2D1.Z})\nX軸({axis2D.X},{axis2D.Y},{axis2D.Z})n");
#endif
                                    break;
                                case FACE.FRONT:
                                case FACE.BACK:
                                    //前後視圖(XZ) Y軸鏡射 以Z座標為中心 Y軸不變 移動X座標(Y軸鏡射)
                                    //p3D1 = new Point3D(center.X, 0, 0); //鏡射第一點
                                    //p3D2 = new Point3D(center.X, 0, 10);//鏡射第二點
                                    //axis3D = new Vector3D() { X = 0, Y = center.Y, Z = 0 };

                                    p3D1 = new Point3D(center.X, 0, center.Z);
                                    p3D2 = new Point3D(center.X, 0, 0);
                                    p3D3 = new Point3D(center.X, center.Y, center.Z);
#if DEBUG
                                    log4net.LogManager
                                    .GetLogger($"FRONT.BACK 3D 依據 以下三點之構面\n")
                                    .Debug($"({p3D1.X},{p3D1.Y},{p3D1.Z})\n({p3D2.X},{p3D2.Y},{p3D2.Z})\n({p3D3.X},{p3D3.Y},{p3D3.Z})\n");
#endif
                                    // 以構件中心為基礎
                                    //axis3D = Vector3D.AxisY;//(0,1,0)
                                    // 以孔群中心為基礎
                                    //axis3D = new Vector3D(0, center.Y, 0);

                                    axis2D = Vector3D.AxisX;
                                    switch (face)
                                    {
                                        case FACE.TOP:
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.H / 2;
#if Debug
                                            log4net.LogManager
                                            .GetLogger($"FRONT.BACK 3D TOP 依據 以下之構面\n")
                                            .Debug($"Y=移動前視圖距離({bolts2DBlock.MoveFront})+高度{steelAttr.H}/2");
#endif
                                            break;
                                        case FACE.FRONT:
#if Debug
                                            log4net.LogManager
                                            .GetLogger($"FRONT.BACK 3D FRONT 依據 以下之構面\n")
                                            .Debug($"Y=移動前視圖距離({bolts2DBlock.MoveFront})+寬度{steelAttr.W}/2");
#endif
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.W / 2;
                                            break;
                                        case FACE.BACK:
#if Debug
                                            log4net.LogManager.GetLogger($"FRONT.BACK 3D BACK 依據 以下之構面\n").Debug($"Y=移動背視圖距離({bolts2DBlock.MoveFront})-寬度{steelAttr.H}/2");
#endif
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveBack - steelAttr.H / 2;
                                            break;
                                        default:
                                            break;
                                    }
#if Debug
                                    log4net.LogManager
                                    .GetLogger($"FRONT.BACK 2D 依據 以下之構面\n")
                                    .Debug($"({p2D1.X},{p2D1.Y},{p2D1.Z})\n({p2D2.X},{p2D2.Y},{p2D2.Z})\n({axis2D.X},{axis2D.Y},{axis2D.Z})\n");
#endif

                                    break;
                                default:
                                    break;
                            }

                            Plane mirror3DPlane = new Plane(p3D1, p3D2, p3D3);
                            Plane mirror2DPlane = new Plane(p2D1, axis2D);
#if Debug
                            log4net.LogManager
                            .GetLogger($"鏡射平面\n")
                            .Debug(
                                $"3D:({mirror3DPlane.AxisX},{mirror3DPlane.AxisY},{mirror3DPlane.AxisZ})\n)" +
                                $"2D:({mirror2DPlane.AxisX},{mirror2DPlane.AxisY},{mirror2DPlane.AxisZ})\n");
#endif

                            List<Entity> modify3D = new List<Entity>(), modify2D = new List<Entity>();
                            Mirror mirror3D = new Mirror(mirror3DPlane);
                            Mirror mirror2D = new Mirror(mirror2DPlane);

                            //清除要鏡射的物件
                            model.Entities.Clear();
                            drawing.Entities.Clear();

                            buffer3D.ForEach(el =>
                            {
                                //oSolid.Mirror(Vector3D.AxisY, new Point3D(-10, 0, SteelAttr.t2*0.5), new Point3D(10, 0, SteelAttr.t2*0.5));
                                Entity entity = (Entity)el.Clone();
                                entity.Mirror(Vector3D.AxisY, center, new Point3D() { X = center.X, Y = center.Y, Z = 0 });
                                //entity.TransformBy(mirror3D);
                                modify3D.Add(entity);
                            });
                            buffer2D.ForEach(el =>
                            {
                                Entity entity = (Entity)el.Clone();
                                entity.Mirror(Vector3D.AxisY, center, new Point3D() { X = center.X, Y = center.Y, Z = 0 });
                                //entity.TransformBy(mirror2D);
                                modify2D.Add(entity);
                            });

                            model.Entities.AddRange(modify3D);
                            drawing.Entities.AddRange(modify2D);


                            List<BoltAttr> baList = new List<BoltAttr>();
                            foreach (var item in model.Entities)
                            {
                                BoltAttr ba = (BoltAttr)item.EntityData;
                                Point3D ori = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                                ori.TransformBy(mirror3D);
                                ba.X = ori.X;
                                ba.Y = ori.Y;
                                ba.Z = ori.Z;
                                baList.Add(ba);
                                item.EntityData = ba;
                            }
                            baList.Clear();
                            foreach (var item in drawing.Entities)
                            {
                                BoltAttr ba = (BoltAttr)item.EntityData;
                                Point3D ori = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                                ori.TransformBy(mirror2D);
                                ba.X = ori.X;
                                ba.Y = ori.Y;
                                ba.Z = ori.Z;
                                baList.Add(ba);
                                item.EntityData = ba;
                            }
                            model.SetCurrent(null);
                            drawing.SetCurrent(null);
                            ViewModel.Reductions.AddContinuous(modify3D, modify2D);
                        }
                        Esc();
                        model.Refresh();//刷新模型
                        drawing.Refresh();
                        SaveModel(true);//存取檔案
#if DEBUG
                        log4net.LogManager.GetLogger("鏡射孔位").Debug("結束");

#endif
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                            $"請選擇孔，才可鏡射",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                    }
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                    throw;
                }
            });
            //刪除孔位(孔群)
            ViewModel.DeleteHole = new RelayCommand(() =>
            {
                if (!ViewModel.CheckData_DelHole(ViewModel.PartNumberProperty, model))
                {
                    return;
                }
                //開啟Model焦點
                bool mFocus = model.Focus();

                if (!mFocus)
                {
                    drawing.Focus();
                }
                SimulationDelete();
                Esc();
                //if (!ViewModel.fAddSteelPart)
                SaveModel(false);
            });
            #endregion

            model.Invalidate();
            drawing.Invalidate();


            GridReload();
        }

        #region 塞值
        /// <summary>
        ///列至實體
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public ProductSettingsPageViewModel RowToEntity(ProductSettingsPageViewModel row)
        {
            ProductSettingsPageViewModel temp = new ProductSettingsPageViewModel();
            temp.steelAttr.GUID = row.steelAttr.GUID;
            temp.DataName = row.steelAttr.GUID.ToString();
            temp.steelAttr.AsseNumber = row.steelAttr.AsseNumber;
            temp.AssemblyNumber = row.steelAttr.AsseNumber;
            temp.steelAttr.PartNumber = row.steelAttr.PartNumber;
            temp.steelAttr.Length = double.Parse(row.steelAttr.Length.ToString());
            temp.Length = double.Parse(row.steelAttr.Length.ToString());
            temp.steelAttr.Kg = float.Parse(row.steelAttr.Kg.ToString());
            temp.steelAttr.Weight = double.Parse(row.steelAttr.Weight.ToString());
            temp.Weight = double.Parse(row.steelAttr.Weight.ToString());
            temp.Count = row.Count;
            temp.steelAttr.Name = row.steelAttr.Name;
            temp.TeklaName = row.steelAttr.Name;
            temp.steelAttr.Material = row.steelAttr.Material;
            temp.Material = row.steelAttr.Material;
            temp.steelAttr.Phase = row.steelAttr.Phase;
            temp.Phase = row.steelAttr.Phase;
            temp.steelAttr.ShippingNumber = row.steelAttr.ShippingNumber;
            temp.ShippingNumber = row.steelAttr.ShippingNumber;
            temp.steelAttr.Title1 = row.steelAttr.Title1;
            temp.Title1 = row.steelAttr.Title1;
            temp.steelAttr.Title2 = row.steelAttr.Title2;
            temp.Title2 = row.steelAttr.Title2;
            temp.steelAttr.Profile = row.steelAttr.Profile;
            temp.Profile = row.steelAttr.Profile;
            temp.SteelType = (int)row.steelAttr.Type;
            temp.steelAttr.Type = row.steelAttr.Type;
            temp.Type = row.steelAttr.Type;
            temp.TypeDesc = row.TypeDesc;
            temp.SteelType = row.SteelType;
            temp.steelAttr.H = float.Parse(row.steelAttr.H.ToString());
            temp.steelAttr.W = float.Parse(row.steelAttr.W.ToString());
            temp.steelAttr.t1 = float.Parse(row.steelAttr.t1.ToString());
            temp.steelAttr.t2 = float.Parse(row.steelAttr.t2.ToString());
            temp.oPoint = row.oPoint;
            temp.uPoint = row.uPoint;
            temp.vPoint = row.vPoint;
            temp.CutList = row.CutList;
            temp.steelAttr.oPoint = row.oPoint;
            temp.steelAttr.uPoint = row.uPoint;
            temp.steelAttr.vPoint = row.vPoint;
            temp.steelAttr.CutList = row.CutList;

            //temp.steelAttr.GUID = ViewModel.SteelAttr.GUID;
            //temp.DataName = ViewModel.SteelAttr.GUID.ToString();
            //temp.steelAttr.AsseNumber = ViewModel.SteelAttr.AsseNumber;
            //temp.AssemblyNumber = ViewModel.SteelAttr.AsseNumber;
            //temp.steelAttr.PartNumber = ViewModel.SteelAttr.PartNumber;
            //temp.steelAttr.Length = double.Parse(ViewModel.SteelAttr.Length.ToString());
            //temp.Length = double.Parse(ViewModel.SteelAttr.Length.ToString());
            //temp.steelAttr.Weight = double.Parse(ViewModel.SteelAttr.Weight.ToString());
            //temp.Weight = double.Parse(ViewModel.SteelAttr.Weight.ToString());
            //temp.Count = double.Parse(ViewModel.SteelAttr.Number.ToString());
            //temp.steelAttr.Name = ViewModel.SteelAttr.Name;
            //temp.TeklaName = ViewModel.SteelAttr.Name;
            //temp.steelAttr.Material = ViewModel.SteelAttr.Material;
            //temp.Material = ViewModel.SteelAttr.Material;
            //temp.steelAttr.Phase = ViewModel.SteelAttr.Phase;
            //temp.Phase = ViewModel.SteelAttr.Phase;
            //temp.steelAttr.ShippingNumber = ViewModel.SteelAttr.ShippingNumber;
            //temp.ShippingNumber = ViewModel.SteelAttr.ShippingNumber;
            //temp.steelAttr.Title1 = ViewModel.SteelAttr.Title1;
            //temp.Title1 = ViewModel.SteelAttr.Title1;
            //temp.steelAttr.Title2 = ViewModel.SteelAttr.Title2;
            //temp.Title2 = ViewModel.SteelAttr.Title2;
            //temp.steelAttr.Profile = ViewModel.SteelAttr.Profile;
            //temp.Profile = ViewModel.SteelAttr.Profile;
            //temp.SteelType = (int)ViewModel.SteelAttr.Type;
            //temp.Type = ViewModel.SteelAttr.Type;
            //temp.TypeDesc = ViewModel.TypeDesc;
            //temp.SteelType = (int)ViewModel.SteelAttr.Type;
            //temp.steelAttr.H = float.Parse(ViewModel.SteelAttr.H.ToString());
            //temp.steelAttr.W = float.Parse(ViewModel.SteelAttr.W.ToString());
            //temp.steelAttr.t1 = float.Parse(ViewModel.SteelAttr.t1.ToString());
            //temp.steelAttr.t2 = float.Parse(ViewModel.SteelAttr.t2.ToString());
            //temp.oPoint = ViewModel.SteelAttr.oPoint;
            //temp.uPoint = ViewModel.SteelAttr.uPoint;
            //temp.vPoint = ViewModel.SteelAttr.vPoint;
            //temp.CutList = ViewModel.SteelAttr.CutList;
            //temp.steelAttr.oPoint = ViewModel.SteelAttr.oPoint;
            //temp.steelAttr.uPoint = ViewModel.SteelAttr.uPoint;
            //temp.steelAttr.vPoint = ViewModel.SteelAttr.vPoint;
            //temp.steelAttr.CutList = ViewModel.SteelAttr.CutList;

            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(row.steelAttr.Type).ToString()}.inp");
            //cbx_SectionTypeComboBox.Text = row.steelAttr.Profile;
            return temp;
        }
        public void RowToView(ProductSettingsPageViewModel row)
        {
            // 還原元件資訊
            this.asseNumber.Text = row.steelAttr.AsseNumber;
            this.partNumber.Text = row.steelAttr.PartNumber;
            this.Length.Text = $"{row.steelAttr.Length}";
            this.Weight.Text = $"{row.steelAttr.Weight}";
            this.PartCount.Text = $"{row.steelAttr.Number}";
            this.teklaName.Text = row.steelAttr.Name;
            this.material.Text = row.steelAttr.Material;
            this.phase.Text = $"{row.steelAttr.Phase}";
            this.shippingNumber.Text = $"{row.steelAttr.ShippingNumber}";
            this.Title1.Text = row.steelAttr.Title1;
            this.Title2.Text = row.steelAttr.Title2;
            //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            string tempProfile = row.steelAttr.Profile;
            this.cbx_SteelTypeComboBox.SelectedIndex = (int)row.SteelType;
            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{temp.Type}.inp");
            ViewModel.SteelSectionProperty = tempProfile;
            this.cbx_SectionTypeComboBox.Text = tempProfile;
            //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            this.H.Text = $"{row.steelAttr.H}";
            this.W.Text = $"{row.steelAttr.W}";
            this.t1.Text = $"{row.steelAttr.t1}";
            this.t2.Text = $"{row.steelAttr.t2}";
        }
        /// <summary>
        /// 畫面元件塞值至ViewModel.SteelAttr
        /// </summary>
        /// <param name="newGUID">是否重新產生GUID</param>
        /// <param name="guid">若newGUID = flase, guid填入舊有GUID</param>
        public void GetViewToViewModel(bool newGUID, Guid? guid = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("GetViewToVM").Debug("");
#endif
            //if (newGUID)
            //{
            //    ViewModel.SteelAttr.GUID = Guid.NewGuid();//產生新的 id
            //}
            //else
            //{
            //    ViewModel.SteelAttr.GUID = guid;
            //}
            //// 2022/07/14 呂宗霖 guid2區分2d或3d
            ////ViewModel.SteelAttr.GUID2 = ViewModel.SteelAttr.GUID;
            ////ViewModel.SteelAttr.PointFront = new CutList();//清除切割線
            ////ViewModel.SteelAttr.PointTop = new CutList();//清除切割線
            //ViewModel.SteelAttr.AsseNumber = this.asseNumber.Text;
            //ViewModel.SteelAttr.PartNumber = this.partNumber.Text;
            //ViewModel.SteelAttr.Length = string.IsNullOrEmpty(this.Length.Text) ? 0 : Double.Parse(this.Length.Text);
            //ViewModel.SteelAttr.Weight = string.IsNullOrEmpty(this.Weight.Text) ? 0 : double.Parse(this.Weight.Text);
            //ViewModel.SteelAttr.Name = this.teklaName.Text;
            //ViewModel.SteelAttr.Material = this.material.Text;
            //ViewModel.SteelAttr.Number = string.IsNullOrEmpty(this.PartCount.Text) ? 0 : int.Parse(this.PartCount.Text);
            //int.TryParse(this.phase.Text, out int temp);
            //ViewModel.SteelAttr.Phase = temp;
            //int.TryParse(this.shippingNumber.Text, out temp);
            //ViewModel.SteelAttr.ShippingNumber = temp;
            //ViewModel.SteelAttr.Title1 = this.Title1.Text;
            //ViewModel.SteelAttr.Title2 = this.Title2.Text;
            //ViewModel.SteelAttr.Type = (OBJECT_TYPE)this.cbx_SteelTypeComboBox.SelectedIndex;
            ////ViewModel.SteelAttr.Profile = profileStr;
            ////ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(ViewModel.SteelAttr.Type).ToString()}.inp");

            //ViewModel.SteelAttr.H = string.IsNullOrEmpty(this.H.Text) ? 0 : float.Parse(this.H.Text);
            //ViewModel.SteelAttr.W = string.IsNullOrEmpty(this.W.Text) ? 0 : float.Parse(this.W.Text);
            //ViewModel.SteelAttr.t1 = string.IsNullOrEmpty(this.t1.Text) ? 0 : float.Parse(this.t1.Text);
            //ViewModel.SteelAttr.t2 = string.IsNullOrEmpty(this.t2.Text) ? 0 : float.Parse(this.t2.Text);
            //ViewModel.SteelAttr.ExclamationMark = false;
            //ViewModel.SteelAttr.Lock = false;



            ViewModel.ProfileType = this.cbx_SteelTypeComboBox.SelectedIndex;
            string profileStr = ViewModel.SteelSectionProperty.Replace("*", "X").Replace(" ", "");
            this.cbx_SectionTypeComboBox.Text = profileStr;
        }
        /// <summary>
        /// 畫面元件塞值至SteelAttr
        /// </summary>
        /// <param name="newGUID">是否重新產生GUID</param>
        /// <param name="guid">若newGUID = flase, guid填入舊有GUID</param>
        /// <returns></returns>
        public SteelAttr GetViewToSteelAttr(SteelAttr steelAttr, bool newGUID, Guid? guid = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("GetViewToSteelAttr").Debug("");
#endif
            //SteelAttr steelAttr = new SteelAttr();
            //steelAttr.PointFront = new CutList();//清除切割線
            //steelAttr.PointTop = new CutList();//清除切割線
            steelAttr.AsseNumber = ViewModel.AssemblyNumberProperty;
            steelAttr.PartNumber = ViewModel.PartNumberProperty;
            steelAttr.Length = ViewModel.ProductLengthProperty;
            steelAttr.Kg = ViewModel.CurrentPartSteelAttr.Kg;
            steelAttr.Weight = ViewModel.ProductWeightProperty;
            steelAttr.Name = ViewModel.ProductNameProperty;
            steelAttr.Material = ViewModel.ProductMaterialProperty;
            steelAttr.Phase = ViewModel.PhaseProperty;
            steelAttr.ShippingNumber = ViewModel.ShippingNumberProperty;
            steelAttr.Title1 = ViewModel.Title1Property;
            steelAttr.Title2 = ViewModel.Title2Property;
            //steelAttr.Type = (OBJECT_TYPE)this.cbx_SteelTypeComboBox.SelectedIndex;
            steelAttr.Type = (OBJECT_TYPE)ViewModel.SteelTypeProperty_int;
            string profileStr = this.cbx_SectionTypeComboBox.Text;
            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(steelAttr.Type).ToString()}.inp");
            //this.cbx_SectionTypeComboBox.Text = profileStr;
            this.cbx_SectionTypeComboBox.Text = ViewModel.SteelSectionProperty;
            //steelAttr.Profile = profileStr;
            steelAttr.Profile = ViewModel.SteelSectionProperty;
            steelAttr.H = ViewModel.CurrentPartSteelAttr.H;
            //steelAttr.H = (string.IsNullOrEmpty(this.H.Text) || float.Parse(this.H.Text) == 0) ? 0 : float.Parse(this.H.Text);
            steelAttr.W = ViewModel.CurrentPartSteelAttr.W;
            //steelAttr.W = (string.IsNullOrEmpty(this.W.Text) || float.Parse(this.W.Text) == 0) ? 0 : float.Parse(this.W.Text);
            steelAttr.Number = ViewModel.ProductCountProperty;
            //steelAttr.Number = string.IsNullOrEmpty(this.PartCount.Text) ? 0 : int.Parse(this.PartCount.Text);
            steelAttr.t1 = ViewModel.CurrentPartSteelAttr.t1;
            //steelAttr.t1 = (string.IsNullOrEmpty(this.t1.Text) || float.Parse(this.t1.Text) == 0) ? 0 : float.Parse(this.t1.Text);
            steelAttr.t2 = ViewModel.CurrentPartSteelAttr.t2;
            //steelAttr.t2 = (string.IsNullOrEmpty(this.t2.Text) || float.Parse(this.t2.Text) == 0) ? 0 : float.Parse(this.t2.Text);
            steelAttr.ExclamationMark = ViewModel.ExclamationMarkProperty;
            //steelAttr.ExclamationMark = false;

            //steelAttr.PointBack = ViewModel.SteelAttr.PointBack;
            //steelAttr.PointTop = ViewModel.SteelAttr.PointTop;
            //steelAttr.PointFront = ViewModel.SteelAttr.PointFront;

            Steel3DBlock.FillCutSetting(steelAttr);

            // 2022/10/05 呂宗霖 與暐淇討論後 其實編輯也就等於新增(翻桌~)所以重新給定GUID
            //steelAttr.GUID = ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).GUID;
            if (newGUID)
            {
                steelAttr.GUID = Guid.NewGuid();//產生新的 id
                ViewModel.GuidProperty = steelAttr.GUID;
            }
            else
            {
                steelAttr.GUID = guid;
                ViewModel.GuidProperty = steelAttr.GUID;
            }
            // 調整修改日期
            return steelAttr;
        }

        /// <summary>
        /// 清空零件屬性區塊
        /// </summary>
        public void ClearPartAttribute()
        {
            ViewModel.AssemblyNumberProperty = String.Empty;
            ViewModel.PartNumberProperty = String.Empty;
            ViewModel.ProductCountProperty = 0;
            ViewModel.ProductLengthProperty = 0;
            ViewModel.ProductWeightProperty = 0;
            ViewModel.ProductMaterialProperty = String.Empty;
            ViewModel.ProductNameProperty = String.Empty;
            ViewModel.PhaseProperty = null;
            ViewModel.ShippingNumberProperty = null;
            ViewModel.Title1Property = String.Empty;
            ViewModel.Title2Property = String.Empty;
            ViewModel.ProfileIndex = 0;
            ViewModel.ProfileType = 0;
            //this.asseNumber.Clear();
            //this.partNumber.Clear();
            //this.PartCount.Clear();
            //this.Length.Clear();
            //this.Weight.Text = "";
            //this.PartCount.Clear();
            //this.teklaName.Clear();
            //this.phase.Clear();
            //this.shippingNumber.Clear();
            //this.Title1.Clear();
            //this.Title2.Clear();
            ////this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            //this.cbx_SteelTypeComboBox.SelectedIndex = 0;
            ////ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)0}.inp");
            //this.cbx_SectionTypeComboBox.SelectedIndex = 0;
        }
        #endregion

        /// <summary>
        /// 新增/編輯 零件/構件邏輯
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool DataCheck(string action)
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();
            switch (action)
            {
                case "add":
                    // 2022/10/13 呂宗霖 新增:零件是否已存在專案中，存在的話不允許新增
                    if (part.Any(x => x.Value.Any(y => y.Number == ViewModel.PartNumberProperty)))
                    {
                        WinUIMessageBox.Show(null,
                       $"零件編號已存在，不可新增",
                       "通知",
                       MessageBoxButton.OK,
                       MessageBoxImage.Exclamation,
                       MessageBoxResult.None,
                       MessageBoxOptions.None,
                        FloatingMode.Window);
                        return false;
                    }
                    break;
                case "edit":
                    ObservableCollection<DataCorrespond> DataCorrespond = ser.GetDataCorrespond();

                    if (!part.Values.SelectMany(x => x).Any(x => x.GUID == ViewModel.GuidProperty))
                    {
                        WinUIMessageBox.Show(null,
                      $"零件編號不存在，不可編輯",
                      "通知",
                      MessageBoxButton.OK,
                      MessageBoxImage.Exclamation,
                      MessageBoxResult.None,
                      MessageBoxOptions.None,
                       FloatingMode.Window);
                        return false;
                    }

                    //SteelAttr steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;

                    // 2022/10/13 呂宗霖 編輯:零件編號須一致才可按編輯
                    ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                    //if (ViewModel.PartNumberProperty != this.partNumber.Text)
                    if (ViewModel.PartNumberProperty != row.steelAttr.PartNumber)
                    {
                        WinUIMessageBox.Show(null,
                       $"零件編號相同，才可修改",
                       "通知",
                       MessageBoxButton.OK,
                       MessageBoxImage.Exclamation,
                       MessageBoxResult.None,
                       MessageBoxOptions.None,
                        FloatingMode.Window);
                        return false;
                    }
                    ProductSettingsPageViewModel temp = RowToEntity(row);
                    if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{Guid.Parse(row.DataName)}.dm"))
                    {
                        // 無零件檔
                        WinUIMessageBox.Show(null,
                           $"無此零件檔，請先新增零件再行修改",
                           "通知",
                           MessageBoxButton.OK,
                           MessageBoxImage.Exclamation,
                           MessageBoxResult.None,
                           MessageBoxOptions.None,
                            FloatingMode.Window);
                        return false;
                    }

                    ProductSettingsPageViewModel current_item = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                    if (current_item.Profile != ViewModel.SteelSectionProperty)
                    {
                        // 修改到斷面規格時
                        WinUIMessageBox.Show(null,
                           $"若需修改斷面規格時，請用新增按鈕來建立新零件",
                           "通知",
                           MessageBoxButton.OK,
                           MessageBoxImage.Exclamation,
                           MessageBoxResult.None,
                           MessageBoxOptions.None,
                            FloatingMode.Window);
                        return false;
                    }

                    break;
            }
            return true;
        }

        /// <summary>
        /// 3D Model 載入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model3D_Loaded(object sender, RoutedEventArgs e)
        {
            #region Model 初始化
            //model.InitialView = viewType.Top;
            /*旋轉軸中心設定當前的鼠標光標位置。 如果模型全部位於相機視錐內部，
             * 它圍繞其邊界框中心旋轉。 否則它繞著下點旋轉鼠標。 如果在鼠標下方沒有深度，則旋轉發生在
             * 視口中心位於當前可見場景的平均深度處。*/
            model.Rotate.RotationCenter = rotationCenterType.CursorLocation;
            //旋轉視圖 滑鼠中鍵 + Ctrl
            model.Rotate.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.Ctrl);
            //平移滑鼠中鍵
            model.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
            model.ActionMode = actionType.SelectByBox;
            if (ViewModel.Reductions == null)
            {
                ViewModel.Reductions = new ReductionList(model, drawing); //紀錄使用找操作
            }
            model.Refresh();
            model.ZoomFit();
            #endregion
        }
        /// <summary>
        /// 模擬按下 delete 鍵
        /// </summary>
        private void SimulationDelete()
        {
            //模擬鍵盤按下Delete
            var c = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Delete)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            };
            InputManager.Current.ProcessInput(c);



        }

        /// <summary>
        /// 在模型視圖按下鍵盤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void model_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) //取消所有功能
            {
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Esc");
                Esc();
                esc.Visibility = Visibility.Collapsed;//關閉取消功能
            }
            //            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.P)) //俯視圖
            //            {
            //#if DEBUG
            //                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + P");
            //#endif
            //                model.InitialView = viewType.Top;
            //                model.ZoomFit();//在視口控件的工作區中適合整個模型。
            //            }
            //            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.Z)) //退回
            //            {
            //#if DEBUG
            //                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Z");
            //#endif
            //                ViewModel.Reductions.Previous();
            //#if DEBUG
            //                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Z 完成");
            //#endif
            //            }
            //            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.Y)) //退回
            //            {
            //#if DEBUG
            //                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Y");
            //#endif
            //                ViewModel.Reductions.Next();//回到上一個動作
            //#if DEBUG
            //                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Y 完成");
            //#endif
            //            }
            // 2020/08/04 呂宗霖 因按Delete會造成無窮迴圈 跳不出去系統造成當掉 故先停用直接按Delete的動作
            else if (Keyboard.IsKeyDown(Key.Delete))
            {
                //    ViewModel.EditObject.Execute(null);
                //    drawing.SetCurrent(drawing.CurrentBlockReference);
                //    //model.Blocks.Remove(model.CurrentBlock);
                //    //SimulationDelete();
                //    //if (!ViewModel.fAddSteelPart)
                SaveModel(false);
                Esc();
                //    //drawing.SetCurrent(null);
                //    //model.SetCurrent(null);
            }
            model.Invalidate();
            drawing.Invalidate();
        }

        /// <summary>
        /// 檢測用戶輸入的零件參數是否有完整
        /// </summary>
        /// <returns>
        /// 有輸入完整回傳 true 。輸入不完整回傳 false
        /// </returns>
        public bool CheckPart()
        {
#if DEBUG
            log4net.LogManager.GetLogger("加入物件").Debug("檢測用戶輸入");
#endif

#if DEBUG
            log4net.LogManager.GetLogger("CheckPart").Debug("");
#endif
            //if (ViewModel.SteelAttr.PartNumber == null && ViewModel.SteelAttr.AsseNumber == null)
            //{
            //    // 錯誤狀況;無此判斷及SLEEP會造讀到的ViewModel.SteelAttr是new SteelAttr()
            //    // 20220922 呂宗霖 測試後 覺得是延遲造成程式把null寫回ViewModel.SteelAttr, 所以先用Sleep解決
            //    Thread.Sleep(1000);
            //    ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內1000
            //    ViewModel.GetSteelAttr();
            //}
            //ViewModel.SteelAttr.PartNumber = ViewModel.PartNumberProperty;
            //ViewModel.SteelAttr.AsseNumber = ViewModel.AssemblyNumberProperty;
            //if(ViewModel.SteelAttr.PartNumber==null || ViewModel.SteelAttr.AsseNumber == null) 
            //{
            //    ViewModel.SteelAttr.PartNumber = this.partNumber.Text;
            //    ViewModel.SteelAttr.AsseNumber = this.asseNumber.Text;
            //}


            // DataViews最後一筆GUID是否在Dev_Part中，無則代表尚未正式存檔按OK，有則代表可按新增鈕
            if (PieceListGridControl.VisibleRowCount > 0)
            {
                ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)PieceListGridControl.GetRow(PieceListGridControl.VisibleRowCount - 1);
                row.steelAttr.GUID = Guid.Parse(row.DataName);
                ProductSettingsPageViewModel temp = RowToEntity(row);

                if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{temp.steelAttr.GUID}.dm"))
                {
                    var ResultRtn = WinUIMessageBox.Show(null,
                        $"列表最後一筆 構件編號{temp.steelAttr.AsseNumber}. 零件編號{temp.steelAttr.PartNumber} 尚未點擊OK，是否放棄",
                        "通知",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
                    if (ResultRtn == MessageBoxResult.Yes)
                    {
                        // 放棄，刪除Grid Guid相同的資料列 並 return false
                        #region 還原零件清單
                        //刪除最後一列明細
                        ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                        source.Remove(source.Where(x => x.DataName == temp.steelAttr.GUID.ToString()).FirstOrDefault());
                        //this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        PieceListGridControl.ItemsSource = source;
                        this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        #endregion
                        return false;
                    }
                    else
                    {
                        // 不放棄 將舊零件屬性寫回零件屬性元件中 並 儲存該筆資訊 及 return false
                        #region 還原零件屬性
                        ConfirmCurrentSteelSection(temp);
                        //this.asseNumber.Text = temp.steelAttr.AsseNumber;
                        //this.partNumber.Text = temp.steelAttr.PartNumber;
                        //this.teklaName.Text = temp.steelAttr.Name;
                        //this.material.SelectedValue = temp.steelAttr.Material;
                        //this.phase.Text = temp.steelAttr.Phase.ToString();
                        //this.shippingNumber.Text = temp.steelAttr.ShippingNumber.ToString();
                        //this.Title1.Text = temp.steelAttr.Title1;
                        //this.Title2.Text = temp.steelAttr.Title2;
                        //this.H.Text = temp.steelAttr.H.ToString();
                        //this.W.Text = temp.steelAttr.W.ToString();
                        //this.t1.Text = temp.steelAttr.t1.ToString();
                        //this.t2.Text = temp.steelAttr.t2.ToString();
                        //this.Length.Text = temp.steelAttr.Length.ToString();
                        //this.Weight.Text = temp.steelAttr.Weight.ToString();
                        //this.PartCount.Text = temp.Count.ToString();
                        //this.cbx_SectionTypeComboBox.SelectedValue = temp.steelAttr.Profile;
                        //this.cbx_SteelTypeComboBox.SelectedValue = temp.SteelType;
                        //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(temp.steelAttr.Type).ToString()}.inp");
                        #endregion
                        SaveModel(true, true);

                        #region 指向新增零件
                        ObservableCollection<ProductSettingsPageViewModel> tempNewSource = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                        this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        PieceListGridControl.ItemsSource = tempNewSource;
                        // 取得該GUID資料
                        PreIndex = tempNewSource.FindIndex(x => x.DataName == temp.steelAttr.GUID.ToString());
                        var tns = tempNewSource.FirstOrDefault(x => x.DataName == temp.steelAttr.GUID.ToString());
                        ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(tns.steelAttr.Type).ToString()}.inp");
                        PieceListGridControl.View.FocusedRowHandle = PreIndex;
                        PieceListGridControl.SelectItem(PreIndex);
                        this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        #endregion
                        return false;
                    }
                }
            }
            //if (ViewModel.SteelAttr.PartNumber == "" || ViewModel.SteelAttr.PartNumber == null)//檢測用戶是否有輸入零件編號
            if (string.IsNullOrEmpty(ViewModel.PartNumberProperty))//檢測用戶是否有輸入零件編號
            {
                WinUIMessageBox.Show(null,
                $"請輸入零件編號",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Window);
                return false;
            }
            if (ViewModel.ProductLengthProperty <= 0)//檢測用戶長度是否有大於0
            {
                WinUIMessageBox.Show(null,
                    $"長度不可以小於等於 0",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return false;
            }
            if (ViewModel.ProductCountProperty <= 0) //檢測用戶是否零件數量大於0
            {
                WinUIMessageBox.Show(null,
                    $"數量不可以小於等於 0",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return false;
            }

            //if (ViewModel.DataCorrespond.FindIndex(el => el.Number == ViewModel.SteelAttr.PartNumber) != -1)
            //{
            //    WinUIMessageBox.Show(null,
            //        $"重複編號",
            //        "通知",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Exclamation,
            //        MessageBoxResult.None,
            //        MessageBoxOptions.None,
            //         FloatingMode.Window);
            //    return false;
            //}
#if DEBUG
            log4net.LogManager.GetLogger("加入物件").Debug("完成");
#endif
            return true;
        }



        /// <summary>
        /// 在模型內按下右鍵時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void model_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

#if DEBUG
            log4net.LogManager.GetLogger("在Model按下了右鍵").Debug("查看可用功能");
#endif
            //開啟刪除功能
            if (ViewModel.Select3DItem.Count >= 1 && ViewModel.Select3DItem[0].Item is BlockReference)
            {
#if DEBUG
                log4net.LogManager.GetLogger("啟用").Debug("刪除功能");
#endif
                //開啟取消功能
                delete.Visibility = Visibility.Visible;
                esc.Visibility = Visibility.Visible;
                delete2D.Visibility = Visibility.Visible;
                esc2D.Visibility = Visibility.Visible;
            }
            //開啟編輯功能
            if (ViewModel.Select3DItem.Count == 1 && ViewModel.Select3DItem[0].Item is BlockReference)
            {
#if DEBUG
                log4net.LogManager.GetLogger("啟用").Debug("編輯功能");
#endif
                edit.Visibility = Visibility.Visible;
                edit2D.Visibility = Visibility.Visible;
            }
            //關閉刪除功能與編輯功能
            if (ViewModel.Select3DItem.Count == 0)
            {
#if DEBUG
                log4net.LogManager.GetLogger("關閉").Debug("編輯功能、刪除功能、取消功能");
#endif
                edit.Visibility = Visibility.Collapsed;
                delete.Visibility = Visibility.Collapsed;
                edit2D.Visibility = Visibility.Collapsed;
                delete2D.Visibility = Visibility.Collapsed;
            }
#if DEBUG
            log4net.LogManager.GetLogger("在Model按下了右鍵").Debug("查看完畢");
#endif
        }

        /// <summary>
        /// 新增孔位比對
        /// </summary>
        public bool ComparisonBolts()
        {
            GroupBoltsAttr TmpBoltsArr = new GroupBoltsAttr();
            TmpBoltsArr = ViewModel.GetGroupBoltsAttr();
            double valueX = 0d;
            double valueY = 0d;
            double TmpXPos = 0d;
            double TmpYPos = 0d;
            bool bFindSamePos = false;
            List<(double, double)> AddBoltsList = new List<(double, double)>();

            TmpXPos = TmpBoltsArr.X;
            TmpYPos = TmpBoltsArr.Y;

            // 分解與儲存預建立之孔群各孔座標於LIST
            for (int i = 1; i <= TmpBoltsArr.xCount; i++)
            {
                AddBoltsList.Add((TmpXPos, TmpYPos));

                for (int j = 1; j < TmpBoltsArr.yCount; j++)
                {
                    if (j < TmpBoltsArr.dYs.Count) //判斷孔位Y向矩陣列表是否有超出長度,超過都取最後一位偏移量
                        valueY = TmpBoltsArr.dYs[j - 1];
                    else
                        valueY = TmpBoltsArr.dYs[TmpBoltsArr.dYs.Count - 1];

                    TmpYPos = TmpYPos + valueY;

                    AddBoltsList.Add((TmpXPos, TmpYPos));
                }

                if (i < TmpBoltsArr.dXs.Count) //判斷孔位X向矩陣列表是否有超出長度,超過都取最後一位偏移量
                    valueX = TmpBoltsArr.dXs[i - 1];
                else
                    valueX = TmpBoltsArr.dXs[TmpBoltsArr.dXs.Count - 1];

                TmpXPos = TmpXPos + valueX;

                TmpYPos = TmpBoltsArr.Y;
            }
            TmpXPos = 0d;
            TmpYPos = 0d;

            // 原3D模組各孔位座標存於各LIST
            List<(double, double)> AllBoltsAddList = new List<(double, double)>();
            List<(double, double)> TopBoltsAddList = new List<(double, double)>();
            List<(double, double)> FRONTBoltsAddList = new List<(double, double)>();
            List<(double, double)> BACKBoltsAddList = new List<(double, double)>();

            for (int i = 0; i < model.Entities.Count; i++)//逐步展開孔群資訊
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //判斷孔群
                {
                    BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                    Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生孔群圖塊

                    for (int j = 0; j < bolts3DBlock.Entities.Count; j++)
                    {
                        TmpXPos = ((BoltAttr)bolts3DBlock.Entities[j].EntityData).X;
                        TmpYPos = ((BoltAttr)bolts3DBlock.Entities[j].EntityData).Y;

                        switch (boltsAttr.Face)
                        {
                            case GD_STD.Enum.FACE.TOP:
                                TopBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            case GD_STD.Enum.FACE.FRONT:
                                FRONTBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            case GD_STD.Enum.FACE.BACK:
                                BACKBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // 將原3D各孔群座標存於共用LIST
            switch (TmpBoltsArr.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    AllBoltsAddList = TopBoltsAddList;
                    break;
                case GD_STD.Enum.FACE.FRONT:
                    AllBoltsAddList = FRONTBoltsAddList;
                    break;
                case GD_STD.Enum.FACE.BACK:
                    AllBoltsAddList = BACKBoltsAddList;
                    break;
                default:
                    break;
            }

            // 指定LIST比對是否有相同座標
            foreach (var x in AddBoltsList)
            {
                if (AllBoltsAddList.Contains(x))
                {
                    bFindSamePos = true;
                    break;
                }
                else
                    bFindSamePos = false;
            }
            return bFindSamePos;
        }

        /// <summary>
        /// 自動斜邊打點(與model.RunHypotenuseEnable一樣，差在一個回傳bool 一個有做元件控制)
        /// </summary>
        public async void RunHypotenuseEnable()
        {
            ScrollViewbox.IsEnabled = true;

            if (model.Entities[model.Entities.Count - 1].EntityData is null)
                return;

            SteelAttr TmpSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;

            bool isHypotenuse = false;

            if (TmpSteelAttr.vPoint.Count != 0)         //  頂面斜邊
            {
                isHypotenuse = isHypotenuse || AutoHypotenuseEnable(FACE.TOP);
            }
            if (TmpSteelAttr.uPoint.Count != 0)     //  前面斜邊
            {
                isHypotenuse = isHypotenuse || AutoHypotenuseEnable(FACE.FRONT);
            }
            if (TmpSteelAttr.oPoint.Count != 0)    //  後面斜邊
            {
                isHypotenuse = isHypotenuse || AutoHypotenuseEnable(FACE.BACK);
            }

            // 有斜邊，切割線不可用
            //if (isHypotenuse) ScrollViewbox.IsEnabled = false;

            model.ZoomFit();
            drawing.ZoomFit();
            //刷新模型
            model.Refresh();
            drawing.Refresh();

        }

        /// <summary>
        /// 自動斜邊判斷(切割線區塊)
        /// 可使用切割線，代表其頂視角為矩形，非特殊形狀
        /// </summary>
        /// <param name="face"></param>
        /// <returns>true = 有斜邊 false = 無斜邊</returns>
        public bool AutoHypotenuseEnable(FACE face)
        {
            MyCs myCs = new MyCs();

            STDSerialization ser = new STDSerialization();

            Point3D TmpDL = new Point3D();
            Point3D TmpDR = new Point3D();
            Point3D TmpUL = new Point3D();
            Point3D TmpUR = new Point3D();

            // 是否有斜邊
            bool isHypotenuse = false;

            //if (model.Entities[model.Entities.Count - 1].EntityData is null)
            //{
            //    //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
            //    return isHypotenuse;
            //}

            //SteelAttr CSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            //SteelAttr TmpSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            SteelAttr TmpSteelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
            //var a = GetverticesFromFile(CSteelAttr.PartNumber,ref TmpSteelAttr);

            switch (face)
            {
                #region BACK
                case FACE.BACK:

                    if (TmpSteelAttr.oPoint.Count == 0) return isHypotenuse;

                    if (TmpSteelAttr.oPoint.Select(x => x.X).Distinct().Count() > 2)
                    {
                        //ScrollViewbox.IsEnabled = false;
                        //ViewLocator.OfficeViewModel.HypotenuseEnable = ViewLocator.OfficeViewModel.HypotenuseEnable || false;
                        return !isHypotenuse;
                    }

                    //var tmp1 = TmpSteelAttr.oPoint.GroupBy(uu => uu.Y).Select(q => new
                    //{
                    //    key = q.Key,
                    //    max = q.Max(x => x.X),
                    //    min = q.Min(f => f.X)
                    //}).ToList();

                    //if (tmp1[0].key > tmp1[1].key)
                    //{
                    //    var swap = tmp1[0];
                    //    tmp1[0] = tmp1[1];
                    //    tmp1[1] = swap;
                    //}

                    //TmpDL = new Point3D(tmp1[0].min, tmp1[0].key);
                    //TmpDR = new Point3D(tmp1[0].max, tmp1[0].key);
                    //TmpUL = new Point3D(tmp1[1].min, tmp1[1].key);
                    //TmpUR = new Point3D(tmp1[1].max, tmp1[1].key);

                    //if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X))
                    //    return isHypotenuse;

                    //ViewLocator.OfficeViewModel.HypotenuseEnable = ViewLocator.OfficeViewModel.HypotenuseEnable || true;
                    //ScrollViewbox.IsEnabled = ViewLocator.OfficeViewModel.HypotenuseEnable;
                    break;
                #endregion

                #region FRONT
                case FACE.FRONT:

                    if (TmpSteelAttr.uPoint.Count == 0) return isHypotenuse;

                    if (TmpSteelAttr.uPoint.Select(x => x.X).Distinct().Count() > 2)
                    {
                        //ScrollViewbox.IsEnabled = false;
                        //ViewLocator.OfficeViewModel.HypotenuseEnable = false;
                        return !isHypotenuse;
                    }

                    //var tmp2 = TmpSteelAttr.uPoint.GroupBy(uu => uu.Y).Select(q => new
                    //{
                    //    key = q.Key,
                    //    max = q.Max(x => x.X),
                    //    min = q.Min(f => f.X)
                    //}).ToList();

                    //if (tmp2[0].key > tmp2[1].key)
                    //{
                    //    var swap = tmp2[0];
                    //    tmp2[0] = tmp2[1];
                    //    tmp2[1] = swap;
                    //}

                    //TmpDL = new Point3D(tmp2[0].min, tmp2[0].key);
                    //TmpDR = new Point3D(tmp2[0].max, tmp2[0].key);
                    //TmpUL = new Point3D(tmp2[1].min, tmp2[1].key);
                    //TmpUR = new Point3D(tmp2[1].max, tmp2[1].key);

                    //if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X)) {
                    //    //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
                    //    return isHypotenuse; }

                    //ScrollViewbox.IsEnabled = true;
                    //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
                    break;
                #endregion

                #region TOP
                case FACE.TOP:

                    if (TmpSteelAttr.vPoint.Count == 0) return isHypotenuse;

                    if (TmpSteelAttr.vPoint.Select(x => x.X).Distinct().Count() > 2)
                    {
                        //ScrollViewbox.IsEnabled = false;
                        //ViewLocator.OfficeViewModel.HypotenuseEnable = false;
                        return !isHypotenuse;
                    }
                    //var Vertices = model.Blocks[1].Entities[0].Vertices.Where(z=>z.Z==0).ToList();   

                    //var tmp3 = Vertices.GroupBy(uu => uu.Y).Select(q => new
                    // {
                    //     key = q.Key,
                    //     max = q.Max(x => x.X),
                    //     min = q.Min(f => f.X)
                    // }).OrderByDescending(aa=>aa.key).ToList();


                    // var YUP2List = tmp3.Where(aa => (aa.key == tmp3[0].key || aa.key == tmp3[0].key- TmpSteelAttr.t2)).OrderByDescending(a => a.key).ToList();
                    // if (YUP2List[0].max >= YUP2List[1].max)
                    //     TmpUR = new Point3D(YUP2List[0].max, YUP2List[0].key);
                    // else
                    //     TmpUR = new Point3D(YUP2List[1].max, YUP2List[1].key);


                    // if (YUP2List[0].min <= YUP2List[1].min)
                    //     TmpUL = new Point3D(YUP2List[0].min, YUP2List[0].key);
                    // else
                    //     TmpUL = new Point3D(YUP2List[1].min, YUP2List[1].key);


                    // var YDOWN2List = tmp3.Where(aa => (aa.key == tmp3[tmp3.Count-1].key || aa.key == tmp3[tmp3.Count - 1].key + TmpSteelAttr.t2)).OrderBy(a => a.key).Take(2).ToList();
                    // if (YDOWN2List[0].max >= YDOWN2List[1].max)
                    //     TmpDR = new Point3D(YDOWN2List[0].max, YDOWN2List[0].key);
                    // else
                    //     TmpDR = new Point3D(YDOWN2List[1].max, YDOWN2List[1].key);


                    // if (YDOWN2List[0].min <= YDOWN2List[1].min)
                    //     TmpDL = new Point3D(YDOWN2List[0].min, YDOWN2List[0].key);
                    // else
                    //     TmpDL = new Point3D(YDOWN2List[1].min, YDOWN2List[1].key);

                    //if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X)) {
                    //    ViewLocator.OfficeViewModel.HypotenuseEnable = true;
                    //    return true; }

                    //ScrollViewbox.IsEnabled = true;
                    //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
                    break;
                    #endregion
            }
            //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
            return isHypotenuse;
        }


        /// <summary>
        /// 取消所有動作
        /// </summary>
        private void Esc()
        {
#if DEBUG
            log4net.LogManager.GetLogger("Esc").Debug("");
#endif
            //drawing.SetCurrent(null);
            //model.SetCurrent(null);//層級 To 要編輯的 BlockReference

            model.ActionMode = actionType.SelectByBox;
            drawing.ActionMode = actionType.SelectByBox;


            model.Entities.ClearSelection();//清除全部選取的物件
            drawing.Entities.ClearSelection();

            ViewModel.Select3DItem.Clear();
            ViewModel.tem3DRecycle.Clear();
            ViewModel.Select2DItem.Clear();
            ViewModel.tem2DRecycle.Clear();

            model.ClearAllPreviousCommandData();
            drawing.ClearAllPreviousCommandData();

            drawing.SetCurrent(null);
            model.SetCurrent(null);//層級 To 要編輯的 BlockReference
            drawing.Refresh();
            model.Refresh();

            // 因為現在有OK按紐 所以必須按OK才能存檔
            //if (fNewPart)
            //if (!ViewModel.fAddSteelPart)
            //{
            //    SaveModel(false);//存取檔案
            //}
        }
        /// <summary>
        /// 存取模型
        /// </summary>
        /// <param name="add"></param>
        /// <param name="reflesh"></param>
        /// <param name="oldGuid">Grid上所選之零件GUID，若修正斷面規格，須異動原斷面規格之資料</param>
        public void SaveModel(bool add, bool reflesh = true, Guid? oldGuid = null)
        {
            model.SetCurrent(null);
            STDSerialization ser = new STDSerialization();
            // 取得目前鋼構資訊
            SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;

            //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ConvertToSurrogate();

            //bool t = (model.Blocks[1].Entities[0].EntityData).GetType().IsSerializable;
            //SteelAttrSurrogate a = new SteelAttrSurrogate((SteelAttr)model.Blocks[1].Entities[0].EntityData);

            ViewModel.ProfileIndex = (int)sa.Type;
            // 取出所有零件
            Dictionary<string, ObservableCollection<SteelPart>> part1 = ser.GetPart();
            // 所有零件
            var allPart1 = part1.SelectMany(x => x.Value).ToList();
            // 確認模型名稱
            model.Blocks[1].Name = sa.GUID.Value.ToString();
            //ser.SetPartModel(ViewModel.SteelAttr.GUID.ToString(), model);
            // model檔案匯出
            var stringFilePath = $@"{ApplicationVM.DirectoryModel()}\model檔案概況{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls";
            //ExcelBuyService.CreateModelOverView(stringFilePath, model);
            bool exclamationMark = true;
            // 取得該零件並更新驚嘆號            
            if (!Bolts3DBlock.CheckBolts(model, false))
            {
                ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                if (model.Entities.Count >= 1)
                {
                    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                }
                //((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = true;
                exclamationMark = true;
            }
            else
            {
                ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                if (model.Entities.Count >= 1)
                {
                    ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).ExclamationMark = false;
                }

                exclamationMark = false;
            }
            // model檔存檔
            ser.SetPartModel(sa.GUID.ToString(), model);

            #region 構件資訊
            ObservableCollection<SteelAssembly> SteelAssemblies = new ObservableCollection<SteelAssembly>();
            List<int> delFatherId = new List<int>();
            List<int> delFatherIndex = new List<int>();
            // 取得構件檔
            SteelAssemblies = ser.GetGZipAssemblies();
            if (SteelAssemblies == null)
            {
                SteelAssemblies = new ObservableCollection<SteelAssembly>();
            }
            var ass = new GD_STD.Data.SteelAssembly()
            {
                //GUID = Guid.NewGuid(),
                Count = sa.Number,
                IsTekla = false,
                Number = sa.AsseNumber,
                Length = sa.Length,
                ShippingDescription = new List<string>(new string[sa.Number]),
                ShippingNumber = new List<int>(new int[sa.Number]),
                Phase = new List<int>(new int[sa.Number]),

                //Number = ViewModel.SteelAttr.AsseNumber,
                //Length = ViewModel.SteelAttr.Length,
                //ShippingDescription = new List<string>(new string[ViewModel.SteelAttr.Number]),
                //ShippingNumber = new List<int>(new int[ViewModel.SteelAttr.Number]),
                //Phase = new List<int>(new int[ViewModel.SteelAttr.Number]),
            };
            List<int> buffer = new List<int>(), _buffer = new List<int>();
            Random random = new Random();
            // 若無構件資訊或異動數量，新增資訊
            //if (ViewModel.SteelAssemblies.IndexOf(ass) == -1 && add)
            if (SteelAssemblies.Count == 0 || SteelAssemblies == null || !SteelAssemblies.Any(x => x.Number == ass.Number && x.Length == ass.Length) && add)
            ////if (!ViewModel.SteelAssemblies.Where(x => x.Number == ass.Number && x.Count == ViewModel.SteelAttr.Number).Any() && add)
            {
                //ass = new SteelAssembly()
                //{
                //    //GUID = ViewModel.SteelAttr.GUID,
                //    Count = ViewModel.SteelAttr.Number,
                //    IsTekla = false,
                //    Length = ViewModel.SteelAttr.Length,
                //    ShippingDescription = new List<string>(new string[ViewModel.SteelAttr.Number]),
                //    ShippingNumber = new List<int>(new int[ViewModel.SteelAttr.Number]),
                //    Phase = new List<int>(new int[ViewModel.SteelAttr.Number]),
                //    Number = ViewModel.SteelAttr.AsseNumber,
                //};
                ass.ID = new List<int>();
                random = new Random();
                // 當構件數量 != buffer數量時，將ID加入buffer，直到數量相等時跳出
                while (buffer.Count != sa.Number)
                {
                    int id = random.Next(1000000, 90000000);
                    if (!buffer.Contains(id))
                    {
                        buffer.Add(id);
                    }
                }
                ass.ID.AddRange(buffer.ToArray());
                SteelAssemblies.Add(ass);
            }
            else
            {
                // 若此構件已存在(同編號 同長度 同數量，代表編輯零件)，取得ID
                // add 改為 false
                //if (SteelAssemblies.Where(x => x.Number == ass.Number && x.Count == ViewModel.SteelAttr.Number && x.Length == ViewModel.SteelAttr.Length
                if (SteelAssemblies.Where(x => x.Number == ass.Number && x.Count == sa.Number && x.Length == sa.Length).Any())
                {
                    // 取得目前構件ID
                    ass.ID = SteelAssemblies.FirstOrDefault(x =>
                    x.Number == ass.Number
                    && x.Count == sa.Number
                    //&& x.Count == ViewModel.SteelAttr.Number
                    //&& x.Length == ViewModel.SteelAttr.Length
                    ).ID;
                    // 不新增構件資料
                    add = false;
                }

                if (SteelAssemblies.Where(x => x.Number == ass.Number && x.Count != sa.Number && x.Length == sa.Length).Any())
                {
                    // 原始構件ID
                    //buffer = SteelAssemblies.FirstOrDefault(x => x.Number == ass.Number && x.Count != ViewModel.SteelAttr.Number && x.Length == ViewModel.SteelAttr.Length).ID;
                    buffer = SteelAssemblies.FirstOrDefault(x => x.Number == ass.Number && x.Count != sa.Number && x.Length == sa.Length).ID;
                    // 原始構件數量
                    //int c = SteelAssemblies.FirstOrDefault(x => x.Number == ass.Number && x.Count != ViewModel.SteelAttr.Number && x.Length == ViewModel.SteelAttr.Length).Count;
                    int c = SteelAssemblies.FirstOrDefault(x => x.Number == ass.Number && x.Count != sa.Number && x.Length == sa.Length).Count;
                    #region 修改數量大於原始數量，新增ID
                    if (c < sa.Number)
                    {
                        random = new Random();
                        while (buffer.Count != sa.Number)
                        {
                            int id = random.Next(1000000, 90000000);
                            if (!buffer.Contains(id))
                            {
                                buffer.Add(id);
                            }
                        }
                        ass.ID = buffer;
                    }
                    #endregion
                    #region 修改數量小於原始數量，移除ID
                    // 修改數量小於原始數量，移除ID
                    //if (c > ViewModel.SteelAttr.Number)
                    if (c > sa.Number)
                    {
                        //while (buffer.Count != ViewModel.SteelAttr.Number)
                        while (buffer.Count != sa.Number)
                        {
                            delFatherId.Add(buffer[buffer.Count - 1]);
                            buffer.RemoveAt(buffer.Count - 1);
                        }
                        ass.ID = buffer;
                    }
                    #endregion
                    // 不新增構件資料
                    //add = false;
                    // 異動構件數量
                    SteelAssemblies.FirstOrDefault(x =>
                    x.Number == ass.Number
                    && x.Length == sa.Length
                    //&& x.Count == ViewModel.SteelAttr.Number
                    //&& x.Length == ViewModel.SteelAttr.Length
                    //).Count = ViewModel.SteelAttr.Number;
                    ).Count = sa.Number;

                    SteelAssemblies.FirstOrDefault(x =>
                    x.Number == ass.Number
                    //&& x.Count == ViewModel.SteelAttr.Number
                    //&& x.Length == ViewModel.SteelAttr.Length
                    && x.Length == sa.Length
                    ).ID = ass.ID;

                    //random = new Random();
                    //while (c != ViewModel.SteelAttr.Number)
                    //{
                    //    int id = random.Next(1000000, 90000000);
                    //    if (!buffer.Contains(id))
                    //    {
                    //        buffer.Add(id);
                    //    }
                    //}
                    //ass.ID = buffer;
                }
                //    // 取得目前構件ID
                //    ass.ID = ViewModel.SteelAssemblies.FirstOrDefault(x =>
                //    x.Number == ass.Number
                //    && x.Count == ViewModel.SteelAttr.Number
                //    //&& x.Length == ViewModel.SteelAttr.Length
                //    ).ID;
                //    // 不新增構件資料
                //    add = false;
                //}
                //    //ViewModel.SteelAssemblies.Where(x => x.GUID == ViewModel.SteelAttr.GUID).FirstOrDefault().ID.Clear();
                //    //ViewModel.SteelAssemblies.Where(x => x.GUID == ViewModel.SteelAttr.GUID).FirstOrDefault().ID.AddRange(buffer.ToArray());
                //    //ass.ID = buffer.ToList();
            }


            if (add)
            {
                ser.SetSteelAssemblies(SteelAssemblies);
            }
            #endregion
            //GetViewToViewModel(false, ((SteelAttr)model.Blocks[1].Entities[0].EntityData).GUID);
            #region 斷面規格
            // 斷面規格
            var profile = ser.GetProfile();
            //if (!profile.Contains(ViewModel.SteelAttr.Profile))
            if (!profile.Contains(sa.Profile))
            {
                //profile.Add(ViewModel.SteelAttr.Profile);
                profile.Add(sa.Profile);
                ser.SetProfileList(profile);
            }
            #endregion


            if (allPart1.Count > 0 && allPart1.Any(x => x.GUID == sa.GUID))
            {
                var oriFather = allPart1.FirstOrDefault(x => x.GUID == sa.GUID).Father;
                ass.ID = ass.ID.Union(oriFather).ToList();
            }


            List<int> tempID = new List<int>(ass.ID);
            foreach (var item in ass.ID)
            {
                if (delFatherId.Contains(item))
                {
                    tempID.Remove(item);
                }
            }
            ass.ID = tempID;

            #region 零件列表
            // 2022/09/08 呂宗霖 與架構師討論後，零件編輯單純做編輯動作            
            // 零件列表
            string profileStr = sa.Profile;// cbx_SectionTypeComboBox.Text;
            var pfList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");

            SteelPart steelPart = new SteelPart();

            // 原始零件資料
            //SteelPart OriginalPart = allPart1.FirstOrDefault(x=>x.GUID==sa.GUID);
            // 

            ISteelProfile pf = pfList.Where(x => x.Profile == sa.Profile.Replace("*", "X").Replace(" ", "")).FirstOrDefault();
            if (pf == null)
            {
                Thread.Sleep(1000);
                pf = pfList.Where(x => x.Profile == sa.Profile.Replace("*", "X").Replace(" ", "")).FirstOrDefault();
            }
            pf.Type = sa.Type;
            steelPart = new SteelPart(
                pf,
                sa.Name, sa.PartNumber,
                sa.Length, sa.Number,// (int)ViewModel.ProductCountProperty,
                sa.GUID.Value, sa.Phase, sa.ShippingNumber,
                sa.Title1, sa.Title2, sa.Lock);
            //steelPart = new SteelPart(
            //    pf,
            //    ViewModel.ProductNameProperty, sa.PartNumber,
            //    sa.Length, sa.Number,
            //    sa.GUID.Value, sa.Phase, sa.ShippingNumber,
            //    sa.Title1, sa.Title2, sa.Lock);

            steelPart.ID = new List<int>();
            steelPart.Match = new List<bool>();
            steelPart.Material = sa.Material;

            steelPart.Father = ass.ID;
            steelPart.Profile = sa.Profile.Replace("*", "X").Replace(" ", "");
            steelPart.UnitWeight = sa.Kg;

            if (pf == null)
            {
                pfList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
                var pf2 = pfList.Where(x => x.Profile == sa.Profile).FirstOrDefault();
                steelPart.H = pf2.H;
                steelPart.W = pf2.W;
                steelPart.t1 = pf2.t1;
                steelPart.t2 = pf2.t2;
                steelPart.Profile = pf2.Profile;
                steelPart.Type = pf2.Type;
            }

            for (int i = 0; i < ass.ID.Count; i++)
            {
                steelPart.Match.Add(true);
            }
            List<int> buffer1 = new List<int>();
            //while (buffer1.Count != ViewModel.SteelAttr.Number)

            while (buffer1.Count != ass.ID.Count)
            {
                int id = random.Next(1000000, 9000000);
                if (!buffer1.Contains(id))
                {
                    buffer1.Add(id);
                }
            }
            // 給定 零件 數量vsID
            steelPart.ID = buffer1.ToList();
            steelPart.Count = ass.ID.Count();
            steelPart.Revise = (sa.Revise.HasValue ? sa.Revise.Value : DateTime.Now);
            steelPart.ExclamationMark = exclamationMark;
            oldGuid = (oldGuid == null ? steelPart.GUID : oldGuid);
            // 原零件之構建ID
            // 取出所有零件
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();
            // 所有零件攤平
            var allPart = part.SelectMany(x => x.Value).ToList();
            // 原始零件
            var oldPart = allPart.FirstOrDefault(x => x.GUID == oldGuid);
            if (allPart.Any(x => x.GUID == oldGuid))
            {
                // 取得原始零件之斷面規格之所有零件
                var old_Profile_Part = part[oldPart.Profile.GetHashCode().ToString() + ".lis"];
                // 比對目前畫面上斷面規格中的零件 是否有修改後的零件
                if (part.Any(x => x.Key == sa.Profile.GetHashCode().ToString() + ".lis" && x.Value.Any(y => y.Number == sa.PartNumber)))
                {
                    // 取得目前畫面上斷面規格之所屬零件
                    var new_profile_part = part[sa.Profile.GetHashCode().ToString() + ".lis"];
                    if (new_profile_part.Any(x => x.GUID == sa.GUID))
                    {
                        #region 更新零件資訊
                        var single = new_profile_part.FirstOrDefault(x => x.GUID == steelPart.GUID);
                        single.DrawingName = steelPart.DrawingName;
                        single.W = steelPart.W;
                        single.H = steelPart.H;
                        single.Father = ass.ID;
                        single.ID = steelPart.ID;
                        single.Match = steelPart.Match;
                        single.t1 = steelPart.t1;
                        single.t2 = steelPart.t2;
                        single.Phase = steelPart.Phase;
                        single.Count = steelPart.Count;
                        single.Length = steelPart.Length;
                        single.Title1 = steelPart.Title1;
                        single.Title2 = steelPart.Title2;
                        single.Revise = steelPart.Revise;
                        single.Material = steelPart.Material;
                        single.ShippingNumber = steelPart.ShippingNumber;
                        single.ExclamationMark = exclamationMark;
                        ser.SetPart($@"{oldPart.Profile.GetHashCode().ToString()}", new ObservableCollection<object>(new_profile_part));
                        #endregion
                    }
                }
                else
                {
                    // 若沒有找到，代表斷面規格異動，刪除所有零件中的該筆零件(比對GUID)、新增新的斷面規格之零件清單
                    #region 刪除舊有斷面規格之零件
                    old_Profile_Part.Remove(allPart.FirstOrDefault(x => x.GUID == sa.GUID));
                    ser.SetPart($@"{oldPart.Profile.GetHashCode().ToString()}", new ObservableCollection<object>(old_Profile_Part));
                    #endregion

                    #region 新增修改後斷面規格之零件

                    if (part.Any(x => x.Key == sa.Profile.GetHashCode().ToString() + ".lis"))
                    {
                        var new_part = part[sa.Profile.GetHashCode().ToString() + ".lis"];
                        steelPart.ExclamationMark = exclamationMark;
                        new_part.Add(steelPart);
                        ser.SetPart($@"{sa.Profile.GetHashCode().ToString()}", new ObservableCollection<object>(new_part));
                    }
                    else
                    {
                        ObservableCollection<object> new_part = new ObservableCollection<object>();
                        steelPart.ExclamationMark = exclamationMark;
                        new_part.Add(steelPart);
                        ser.SetPart($@"{sa.Profile.GetHashCode().ToString()}", new ObservableCollection<object>(new_part));
                    }

                    #endregion
                }
            }
            else
            {
                List<SteelPart> partList = new List<SteelPart>();
                partList = allPart.Where(x => x.Profile == steelPart.Profile).ToList();
                partList.Where(x => x.GUID == steelPart.GUID).ToList().ForEach(x =>
                { x.ExclamationMark = exclamationMark; });
                //steelPart.ExclamationMark = exclamationMark;
                partList.Add(steelPart);
                ser.SetPart($@"{sa.Profile.GetHashCode().ToString()}", new ObservableCollection<object>(partList));
            }


            #region 原版零件修改(已註解)
            //// 比對 GUID ， 不存在 新增 ，存在 編輯
            //if (!collection.Any(x =>
            ////x.Number == steelPart.Number &&
            ////x.Profile == steelPart.Profile &&
            ////x.Count == steelPart.Count &&
            ////x.Type == steelPart.Type &&
            //x.GUID == steelPart.GUID))
            //{
            //    // 不存在則新增
            //    collection.Add(steelPart);
            //}
            //else
            //{
            //    // 存在則編輯
            //    SteelPart sp = collection.Where(x =>
            //    //x.Number == steelPart.Number &&
            //    //x.Profile == steelPart.Profile &&
            //    //x.Type == steelPart.Type &&
            //    x.GUID == steelPart.GUID).FirstOrDefault();
            //    sp.Count = steelPart.Count;
            //    sp.DrawingName = steelPart.DrawingName;
            //    sp.Length = steelPart.Length;
            //    sp.W = steelPart.W;
            //    sp.H = steelPart.H;
            //    sp.t1 = steelPart.t1;
            //    sp.t2 = steelPart.t2;
            //    sp.Material = steelPart.Material;
            //    sp.Phase = steelPart.Phase;
            //    sp.ShippingNumber = steelPart.ShippingNumber;
            //    sp.Title1 = steelPart.Title1;
            //    sp.Title2 = steelPart.Title2;
            //    sp.Revise = steelPart.Revise;
            //    sp.ExclamationMark = steelPart.ExclamationMark;
            //    sp.ID = steelPart.ID;
            //    sp.Father = ass.ID;
            //}

            //ser.SetPart($@"{steelPart.Profile.GetHashCode()}", new ObservableCollection<object>(collection)); 
            #endregion
            #endregion
            //ViewModel.SteelAttr = sa;

            ViewModel.SaveDataCorrespond();

            // 重新載入
            if (reflesh)
            {
                GridReload();
            }
        }

        /// <summary>
        /// 載入模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\20464d9a-22c8-432b-9c81-923942ab5a01.dm", devDept.Serialization.contentType.GeometryAndTessellation);
            readFile.DoWork();
            readFile.AddToScene(model);
            model.Invalidate();
        }

        /// <summary>
        /// 角度標註
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AngleDim(object sender, EventArgs e)
        {
            Dim(out ModelExt modelExt);
            modelExt.drawingAngularDim = true;
        }

        /// <summary>
        /// 線性標註
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinearDim(object sender, EventArgs e)
        {
#if DEBUG
            log4net.LogManager.GetLogger("觸發線性標註").Debug("");
#endif
            ModelExt modelExt = null;

            if (tabControl.SelectedIndex == 0)
            {
                modelExt = model;
            }
            else
            {
                modelExt = drawing;
            }
            try
            {
                if (model.Entities.Count > 0)
                {
#if DEBUG
                    log4net.LogManager.GetLogger("層級 To 要主件的BlockReference").Debug("成功");
#endif
                    modelExt.Entities[0].Selectable = true;
                    modelExt.ClearAllPreviousCommandData();
                    modelExt.ActionMode = actionType.None;
                    modelExt.objectSnapEnabled = true;
                    modelExt.drawingLinearDim = true;
                    return;
                }
#if DEBUG
                else
                {
                    throw new Exception("層級 To 主件的BlockReference 失敗，找不到主件");
                }
#endif
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                //Debugger.Break();
            }
#if DEBUG
            log4net.LogManager.GetLogger("觸發線性標註").Debug("");
#endif
            //ModelExt modelExt= new ModelExt();
            Dim(out modelExt);
            modelExt.drawingLinearDim = true;

        }


        /// <summary>
        /// 線段打點
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HypotenuseLinearPoint(object sender, EventArgs e)
        {
#if DEBUG
            ScrollViewbox.IsEnabled = true;
            log4net.LogManager.GetLogger("觸發線段打點").Debug("");
#endif

            ModelExt modelExt = null;

            if (tabControl.SelectedIndex == 0)
            {
                //  modelExt = model;  // 因只能於2D操作, 取消此3D 
            }
            else
            {
                modelExt = drawing;
                modelExt.TmpStreelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            }
            try
            {
                if (model.Entities.Count > 0 && modelExt != null)
                {
#if DEBUG
                    log4net.LogManager.GetLogger("層級 To 要主件的BlockReference").Debug("成功");
#endif
                    modelExt.Entities[0].Selectable = true;
                    modelExt.ClearAllPreviousCommandData();
                    modelExt.ActionMode = actionType.None;
                    modelExt.objectSnapEnabled = true;
                    modelExt.drawingHypotenusePoint = true;
                    return;
                }
#if DEBUG
                else
                {
                    throw new Exception("層級 To 主件的BlockReference 失敗，找不到主件");
                }
#endif
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
            }
#if DEBUG
            log4net.LogManager.GetLogger("觸發線段打點").Debug("");
#endif
            //Dim(out modelExt);
            //modelExt.drawingHypotenusePoint = true;

        }


        /// <summary>
        /// 刪除線段打點
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelHypotenuseLinearPoint(object sender, EventArgs e)
        {
            //開啟Model焦點
            bool mFocus = model.Focus();

            if (!mFocus)
            {
                drawing.Focus();
            }
            SimulationDelete();
            Esc();
            SaveModel(false);
            model.Invalidate();
            drawing.Invalidate();

        }

        /// <summary>
        /// 標註動作
        /// </summary>
        private void Dim(out ModelExt modelExt)
        {
            //ModelExt modelExt = null;
            if (tabControl.SelectedIndex == 0)
            {
                modelExt = model;
            }
            else
            {
                modelExt = drawing;
            }
            try
            {
                if (modelExt.Entities.Count > 0)
                {
                    modelExt.Entities[modelExt.Entities.Count - 1].Selectable = true;
                    modelExt.ClearAllPreviousCommandData();
                    modelExt.ActionMode = actionType.None;
                    modelExt.objectSnapEnabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                Debugger.Break();
            }
        }

        /// <summary>
        /// 當項目從已載入項目的項目樹狀結構中移除時發生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            model.Dispose();//釋放資源
            drawing.Dispose();//釋放資源
            drawing.Loaded -= drawing_Loaded;
            GC.Collect();
        }
        /// <summary>
        /// 選擇面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void model_SelectionChanged(object sender, devDept.Eyeshot.Environment.SelectionChangedEventArgs e)
        {
            int selectedCount = 0;

            // 計算選定的實體
            object[] selected = new object[e.AddedItems.Count];

            selectedCount = 0;

            // 填充選定的數組
            for (int index = 0; index < e.AddedItems.Count; index++)
            {
                var item = e.AddedItems[index];

                if (item is SelectedFace)
                {
                    var faceItem = (SelectedFace)item;
                    var ent = faceItem.Item;

                    if (ent is Mesh)
                    {
                        var mesh = (Mesh)ent;
                        selected[selectedCount++] = mesh.Faces[faceItem.Index];
                        List<int> faceElement = ((FaceElement)selected[0]).Triangles;
                        Plane plane = new Plane(mesh.Vertices[mesh.Triangles[faceElement[0]].V2], mesh.Vertices[mesh.Triangles[faceElement[0]].V1], mesh.Vertices[mesh.Triangles[faceElement[0]].V3]);
                        model.SetDrawingPlan(plane);
                        model.ClearAllPreviousCommandData();
                        model.ActionMode = actionType.None;
                        model.objectSnapEnabled = true;
                        model.drawingLinearDim = true;
                    }
                }
            }

            if (ViewModel.EditObjectVisibility)
            {
                edit2D.Visibility = Visibility.Visible;
            }
            else
            {
                edit2D.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 此設定會影響2D 3D的顯示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlSelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (((System.Windows.FrameworkElement)tabControl.SelectedValue).Name == "drawingTab")
            {
                drawing.CurrentModel = true;
            }
            else
            {
                drawing.CurrentModel = false;
            }
        }

        private void drawing_Loaded(object sender, RoutedEventArgs e)
        {
            //平移滑鼠中鍵
            drawing.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
            drawing.ActionMode = actionType.SelectByBox;



            STDSerialization ser = new STDSerialization();
            //// 建立dm檔 for 尚未建立dm檔的零件
            appVM.CreateDMFile(model);
            //appVM.CreateDMFileSync(model);

            //首次匯入tekla資料後執行GridReload來更新驚嘆號顯示
            if (fAfterFirstImportTeklaData)
            {
                GridReload();
                fAfterFirstImportTeklaData = false;
            }
            drawing.Refresh();//刷新模型
            drawing.ZoomFit();//設置道適合的視口

        }


        public BlockReference SteelTriangulation(Mesh mesh)
        {
#if DEBUG
            log4net.LogManager.GetLogger("SteelTriangulation").Debug("");
            log4net.LogManager.GetLogger($"產生2D圖塊(TOP.FRONT.BACK)").Debug($"開始");
#endif
            //drawing.Blocks.Clear();
            //drawing.Entities.Clear();
            drawing.Clear();

            // 產生2D圖塊
            Steel2DBlock steel2DBlock = new Steel2DBlock(mesh, model.Blocks[1].Name);
            drawing.Blocks.Add(steel2DBlock);
            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊
                                                                                                //關閉三視圖用戶選擇
            block2D.Selectable = false;

            // 將TOP FRONT BACK圖塊加入drawing
            drawing.Entities.Add(block2D);
            //drawing.Entities.Add(steel2DBlock.Steel);
#if DEBUG
            log4net.LogManager.GetLogger("產生2D圖塊(TOP.FRONT.BACK)").Debug("結束");
#endif
            drawing.ZoomFit();//設置道適合的視口
            drawing.Refresh();//刷新模型
            return block2D;
        }

        /// <summary>
        /// 加入2d 孔位
        /// </summary>
        /// <param name="bolts"></param>
        /// <param name="refresh">刷新模型</param>
        /// <returns></returns>
        private BlockReference Add2DHole(Bolts3DBlock bolts, bool refresh = true)
        {
            try
            {
                /*2D螺栓*/
                BlockReference referenceMain = (BlockReference)drawing.Entities[drawing.Entities.Count - 1]; //主件圖形
                                                                                                             //BlockReference referenceMain = (BlockReference)drawing.Entities.Where(x=>x is BlockReference).LastOrDefault(); //主件圖形
                Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[referenceMain.BlockName]; //取得鋼構圖塊
#if DEBUG
                log4net.LogManager.GetLogger($"產生 {bolts.Name} 2D螺栓圖塊").Debug($"開始");

#endif
                string blockName = string.Empty; //圖塊名稱
#if DEBUG
                //log4net.LogManager.GetLogger($"產生2D螺栓圖塊").Debug($"開始");
#endif

                Bolts2DBlock bolts2DBlock = new Bolts2DBlock(bolts, steel2DBlock); //產生螺栓圖塊
#if DEBUG
                log4net.LogManager.GetLogger($"產生2D螺栓圖塊").Debug($"結束");
                log4net.LogManager.GetLogger($"2D畫布加入螺栓圖塊").Debug($"");
#endif
                bolts2DBlock.Entities.Regen();
                //if (drawing.Blocks.Any(x => x.Name == bolts2DBlock.Name))
                //{
                //    var a = drawing.Blocks.FirstOrDefault(x => x.Name == bolts2DBlock.Name);
                //    a = bolts2DBlock;
                //}
                //else {
                drawing.Blocks.Add(bolts2DBlock); //加入螺栓圖塊
                //}
                foreach (var block in drawing.Blocks)
                {
                    block.Entities.Regen();
                }
                blockName = bolts2DBlock.Name;
                BlockReference result = new BlockReference(0, 0, 0, bolts2DBlock.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
                                                                                                   // 將孔位加入到TOP FRONT BACK圖塊中
                drawing.Entities.Insert(0, result);

#if DEBUG
                log4net.LogManager.GetLogger($"2D畫布加入TOP FRONT BACK圖塊").Debug($"");
                log4net.LogManager.GetLogger($"產生 {bolts.Name} 2D螺栓圖塊").Debug($"結束");
#endif

                if (refresh)
                {
                    drawing.Entities.Regen();
                    drawing.Refresh();//刷新模型
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 取得model是否有非3D資料
        /// 無非3D資料，代表3D可直接轉2D(SteelTriangulation)
        /// </summary>
        /// <returns></returns>
        public int GetModelType()
        {
            return 0;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            //            if (fAddSteelPart)  //  新增零件功能
            //            {
            //                var ResultRtn = WinUIMessageBox.Show(null,
            //                     $"新增零件未存檔,是否存檔",
            //                     "通知",
            //                     MessageBoxButton.OKCancel,
            //                     MessageBoxImage.Exclamation,
            //                     MessageBoxResult.None,
            //                     MessageBoxOptions.None,
            //                      FloatingMode.Window);


            //                if (ResultRtn == MessageBoxResult.OK)
            //                    SaveModel(true);//存取檔案

            //                fAddSteelPart = false;
            //                fAddHypotenusePoint = false;
            //            }

            //            ////  執行斜邊打點功能
            //            //if (fAddHypotenusePoint)
            //            //{
            //            //    var ResultRtn = WinUIMessageBox.Show(null,
            //            //         $"切割線打點異動未存檔,是否存檔",
            //            //         "通知",
            //            //         MessageBoxButton.OKCancel,
            //            //         MessageBoxImage.Exclamation,
            //            //         MessageBoxResult.None,
            //            //         MessageBoxOptions.None,
            //            //          FloatingMode.Window);


            //            //    if (ResultRtn == MessageBoxResult.OK)
            //            //        SaveModel(true);//存取檔案

            //            //    fAddHypotenusePoint = false;                                                                                                               

            //            //}



            //            TreeView treeView = (TreeView)sender; //樹狀列表
            //            TreeNode data = (TreeNode)e.NewValue;
            //            if (data.DataName == null)
            //                return;
            //            STDSerialization ser = new STDSerialization();
            //            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            //            NcTemp ncTemp = ncTemps.GetData(data.DataName);//需要實體化的nc物件
            //            model.Clear(); //清除目前模型
            //            if (ncTemp == null) //NC 檔案是空值
            //            {
            //                ReadFile readFile = ser.ReadPartModel(data.DataName); //讀取檔案內容
            //                if (readFile == null)
            //                {
            //                    WinUIMessageBox.Show(null,
            //                        $"專案Dev_Part資料夾讀取失敗",
            //                        "通知",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Exclamation,
            //                        MessageBoxResult.None,
            //                        MessageBoxOptions.None,
            //                         FloatingMode.Window);
            //                    return;
            //                }
            //                readFile.DoWork();//開始工作
            //                readFile.AddToScene(model);//將讀取完的檔案放入到模型
            //                modelToView(data.DataName);
            ////                if (model.Entities[model.Entities.Count - 1].EntityData is null)
            ////                {
            ////                    return;
            ////                }
            ////                ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            ////                ViewModel.GetSteelAttr();
            ////                model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式(零件)
            ////#if DEBUG
            ////                log4net.LogManager.GetLogger($"GUID:{data.DataName}").Debug($"零件: {model.Blocks[1].Name}");
            ////#endif
            ////                //產生零件2D對應圖塊、座標及相關參數
            ////                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            ////                for (int i = 0; i < model.Entities.Count; i++)//逐步展開 3d 模型實體
            ////                {
            ////                    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
            ////                    {
            ////                        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
            ////                        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
            ////#if DEBUG
            ////                        log4net.LogManager.GetLogger($"").Debug($"取得 {blockReference.BlockName} 圖塊");
            ////#endif
            ////                        Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊//將孔位資料+孔位屬性填入bolts3DBlock中
            ////#if DEBUG
            ////                        log4net.LogManager.GetLogger($"產生3D螺栓圖塊").Debug($"產生 {blockReference.BlockName} 圖塊內部3D螺栓圖塊");
            ////#endif
            ////                        Add2DHole(bolts3DBlock, true);//加入孔位不刷新 2d 視圖



            //                //                    }
            //                //                }
            //            }
            //            else //如果需要載入 nc 設定檔
            //            {
            //                model.LoadNcToModel(data.DataName);
            //                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊
            //            }

            //            // 執行斜邊打點
            //           // AutoHypotenusePoint(FACE.TOP);
            //      //      HypotenusePoint(FACE.BACK);
            //       //     HypotenusePoint(FACE.FRONT);

            //            model.ZoomFit();//設置道適合的視口
            //            model.Invalidate();//初始化模型
            //            model.Refresh();
            //            drawing.ZoomFit();//設置道適合的視口
            //            drawing.Invalidate();
            //            drawing.Refresh();


        }

        /// <summary>
        /// 讀完dm檔丟到model再產生畫面
        /// </summary>
        /// <param name="dataName"></param>
        public void modelToView(string dataName)
        {
            if (model.Entities[model.Entities.Count - 1].EntityData is null)
            {
                return;
            }
            ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            ViewModel.GetSteelAttr();
            SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            // 加入Block
            Steel3DBlock block3D = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式(零件)
            model.AddModelSteelAttr(steelAttr, block3D);
#if DEBUG
            log4net.LogManager.GetLogger($"GUID:{dataName}").Debug($"零件: {model.Blocks[1].Name}");
#endif
            bool hasOutSteel = false;
            //產生零件2D對應圖塊、座標及相關參數

            //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            //SteelTriangulation((Mesh)model.Blocks.Where(x => x.GetType().Name == "Steel3DBlock").LastOrDefault().Entities[0]);//產生2D圖塊
            //teelTriangulation((Mesh)model.Blocks[dataName].Entities.Where(x => x.EntityData is SteelAttr).LastOrDefault());//產生2D圖塊
            SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            for (int i = 0; i < model.Entities.Count; i++)//逐步展開 3d 模型實體
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                {
                    BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
#if DEBUG
                    log4net.LogManager.GetLogger($"").Debug($"取得 {blockReference.BlockName} 圖塊");
#endif
                    Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊//將孔位資料+孔位屬性填入bolts3DBlock中
#if DEBUG
                    log4net.LogManager.GetLogger($"產生3D螺栓圖塊").Debug($"產生 {blockReference.BlockName} 圖塊內部3D螺栓圖塊");
#endif
                    if (!bolts3DBlock.hasOutSteel)
                    {
                        Add2DHole(bolts3DBlock, true);//加入孔位不刷新 2d 視圖
                    }

                }
            }
        }


        /// <summary>
        /// 轉換世界座標
        /// </summary>
        /// <param name="face"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point3D[] Coordinates(FACE face, Point3D p)
        {
            switch (face)
            {
                case GD_STD.Enum.FACE.TOP:
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    double y, z;
                    y = p.Z;
                    z = p.Y;
                    p.Y = y;
                    p.Z = z;
                    break;
                default:
                    break;
            }
            //X = p.X, Y = p.Y, Z = p.Z
            Point3D[] points = new Point3D[3];
            points[0] = new Point3D() { X = p.X, Y = p.Y, Z = p.Z };
            return points;
        }
        private void SetPlane(object sender, EventArgs e)
        {
            model.ClearAllPreviousCommandData();
            model.ActionMode = actionType.None;
            model.objectSnapEnabled = true;
            model.setPlane = true;
        }
        /// <summary>
        /// FRONT及BACK時，Y及Z座標需對調
        /// </summary>
        /// <param name="face"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point3D XYPlane(FACE face, Point3D p)
        {
            double y, z;
            switch (face)
            {
                case FACE.FRONT:
                case FACE.BACK:
                    y = p.Z;
                    z = p.Y;
                    p.Z = y;
                    p.Y = z;
                    break;
                default:
                    break;
            }
            return p;
        }
        private void Set_DrillSettingGrid_AllCheckboxChecked_Click(object sender, RoutedEventArgs e)
        {
            GetWpfLogicalChildClass.SetAllCheckBoxTrueOrFalse(DrillTabItem);
        }
        private void Set_CutSettingGrid_AllCheckboxChecked_Click(object sender, RoutedEventArgs e)
        {
            GetWpfLogicalChildClass.SetAllCheckBoxTrueOrFalse(CutTabItem);
        }
        /// <summary>
        /// 斷面規格改變時的事件 - 給予VM中SteelAttr, CurrentPartSteelAttr當前的零件資料
        /// </summary>
        private void CBOX_SectionTypeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbx_SectionTypeComboBox.SelectedIndex != -1)
            {
                ////this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                //ViewModel.ProfileIndex = cbx_SectionTypeComboBox.SelectedIndex;
                ////cbx_SectionTypeComboBox.SelectedIndex = ViewModel.ProfileIndex;
                //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(cbx_SteelTypeComboBox.Text)}.inp");
                ViewModel.SteelSectionProperty = ViewModel.ProfileList[cbx_SectionTypeComboBox.SelectedIndex].Profile;
                var pf = ViewModel.ProfileList.FirstOrDefault(x => x.Profile == ViewModel.SteelSectionProperty);
                if (pf == null)
                {
                    return;
                }
                ViewModel.SteelAttr.H = pf.H;
                ViewModel.HProperty = pf.H;
                ViewModel.SteelAttr.W = pf.W;
                ViewModel.WProperty = pf.W;
                ViewModel.SteelAttr.t1 = pf.t1;
                ViewModel.t1Property = pf.t1;
                ViewModel.SteelAttr.t2 = pf.t2;
                ViewModel.t2Property = pf.t2;
                ViewModel.SteelAttr.Kg = pf.Kg;
                ViewModel.KGProperty = pf.Kg;
                ViewModel.SteelAttr.Profile = pf.Profile;
                //ViewModel.CurrentPartSteelAttr = pf; //ViewModel.SteelAttr;
                ViewModel.SteelSectionProperty = pf.Profile;
                cbx_SectionTypeComboBox.Text = pf.Profile;
                if (ViewModel.CurrentPartSteelAttr != null)
                {
                    ViewModel.CurrentPartSteelAttr.H = pf.H;
                    ViewModel.CurrentPartSteelAttr.W = pf.W;
                    ViewModel.CurrentPartSteelAttr.t1 = pf.t1;
                    ViewModel.CurrentPartSteelAttr.t2 = pf.t2;
                    ViewModel.CurrentPartSteelAttr.Profile = pf.Profile;
                }
              
                //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            }
        }

        /// <summary>
        /// 由所選零件給予VM中所需零件資料
        /// </summary>
        private void ConfirmCurrentSteelSection(ProductSettingsPageViewModel CuurentSelectedPart)
        {
            ViewModel.fPartListOrManuall = true;

            string profile = CuurentSelectedPart.steelAttr.Profile;
            //ViewModel.ProfileType = 0;
            //ViewModel.SteelTypeProperty_int = 0;
            //ViewModel.ProfileIndex = 0;
            //ViewModel.ProfileType = (int)CuurentSelectedPart.steelAttr.Type;
            //ViewModel.SteelTypeProperty_int= (int)CuurentSelectedPart.steelAttr.Type;
            //ViewModel.ProfileIndex = ViewModel.ProfileList.FindIndex(x => x.Profile == profile);
            //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(CuurentSelectedPart.steelAttr.Type).ToString()}.inp");
            //cbx_SectionTypeComboBox.ItemsSource = ViewModel.ProfileList;
            //cbx_SectionTypeComboBox.Text = CuurentSelectedPart.Profile;
            //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
            //cbx_SectionTypeComboBox.Text = CuurentSelectedPart.Profile;
            ViewModel.SteelAttr = CuurentSelectedPart.steelAttr;
            ViewModel.PartNumberProperty = CuurentSelectedPart.steelAttr.PartNumber;
            ViewModel.AssemblyNumberProperty = CuurentSelectedPart.steelAttr.AsseNumber;
            ViewModel.ProductLengthProperty = CuurentSelectedPart.steelAttr.Length;
            ViewModel.SteelAttr.Length = CuurentSelectedPart.steelAttr.Length;
            ViewModel.KGProperty = CuurentSelectedPart.steelAttr.Kg;
            ViewModel.SteelAttr.Kg = CuurentSelectedPart.steelAttr.Kg;
            ViewModel.t1Property = CuurentSelectedPart.steelAttr.t1;
            ViewModel.t2Property = CuurentSelectedPart.steelAttr.t2;
            ViewModel.HProperty = CuurentSelectedPart.steelAttr.H;
            ViewModel.WProperty = CuurentSelectedPart.steelAttr.W;
            //if (CuurentSelectedPart.steelAttr.Kg != 0)
            //{
            //    ViewModel.ProductWeightProperty = (CuurentSelectedPart.steelAttr.Length / 1000) * CuurentSelectedPart.steelAttr.Kg;
            //}
            //else
            //{
            //    ViewModel.ProductWeightProperty = CuurentSelectedPart.steelAttr.Weight;
            //    if (CuurentSelectedPart.steelAttr.Weight == 0) ViewModel.ProductWeightProperty = ViewModel.CalculateSinglePartWeight();
            //}
            ViewModel.ProductWeightProperty = ViewModel.CalculateSinglePartWeight();// (ViewModel.ProductLengthProperty / 1000) * ViewModel.KGProperty;
            ViewModel.SteelAttr.Weight = ViewModel.ProductWeightProperty;
            CuurentSelectedPart.Weight = ViewModel.ProductWeightProperty;
            CuurentSelectedPart.steelAttr.Weight = ViewModel.ProductWeightProperty;
            ViewModel.ProductCountProperty = CuurentSelectedPart.Count;
            ViewModel.ProductNameProperty = CuurentSelectedPart.steelAttr.Name;
            ViewModel.ProductMaterialProperty = CuurentSelectedPart.steelAttr.Material;
            ViewModel.PhaseProperty = CuurentSelectedPart.steelAttr.Phase;
            ViewModel.ShippingNumberProperty = CuurentSelectedPart.steelAttr.ShippingNumber;
            ViewModel.Title1Property = CuurentSelectedPart.steelAttr.Title1;
            ViewModel.Title2Property = CuurentSelectedPart.steelAttr.Title2;
            ViewModel.SteelTypeProperty_int = (int)CuurentSelectedPart.steelAttr.Type;
            ViewModel.ProfileType = (int)CuurentSelectedPart.steelAttr.Type;
            ViewModel.SteelSectionProperty = profile;
            ViewModel.ProfileIndex = ViewModel.ProfileList.FindIndex(x => x.Profile == profile);
            //ViewModel.SteelSectionProperty = CuurentSelectedPart.steelAttr.Profile;
            ViewModel.GuidProperty = CuurentSelectedPart.steelAttr.GUID;

            ViewModel.PointBackProperty = CuurentSelectedPart.steelAttr.PointBack;
            ViewModel.PointFrontProperty = CuurentSelectedPart.steelAttr.PointFront;
            ViewModel.PointTopProperty = CuurentSelectedPart.steelAttr.PointTop;
            
            if (ViewModel.ProfileList.Any(x => x.Profile == profile))
            {
                ViewModel.CurrentPartSteelAttr = ViewModel.ProfileList.FirstOrDefault(x => x.Profile == profile);
                ViewModel.CurrentPartSteelAttr.PointTop.DL = CuurentSelectedPart.steelAttr.PointTop.DL;
                ViewModel.CurrentPartSteelAttr.PointTop.DR = CuurentSelectedPart.steelAttr.PointTop.DR;
                ViewModel.CurrentPartSteelAttr.PointTop.UL = CuurentSelectedPart.steelAttr.PointTop.UL;
                ViewModel.CurrentPartSteelAttr.PointTop.UR = CuurentSelectedPart.steelAttr.PointTop.UR;
                ViewModel.CurrentPartSteelAttr.PointFront.DL = CuurentSelectedPart.steelAttr.PointFront.DL;
                ViewModel.CurrentPartSteelAttr.PointFront.DR = CuurentSelectedPart.steelAttr.PointFront.DR;
                ViewModel.CurrentPartSteelAttr.PointFront.UL = CuurentSelectedPart.steelAttr.PointFront.UL;
                ViewModel.CurrentPartSteelAttr.PointFront.UR = CuurentSelectedPart.steelAttr.PointFront.UR;
                ViewModel.CurrentPartSteelAttr.PointBack.DL = CuurentSelectedPart.steelAttr.PointBack.DL;
                ViewModel.CurrentPartSteelAttr.PointBack.DR = CuurentSelectedPart.steelAttr.PointBack.DR;
                ViewModel.CurrentPartSteelAttr.PointBack.UL = CuurentSelectedPart.steelAttr.PointBack.UL;
                ViewModel.CurrentPartSteelAttr.PointBack.UR = CuurentSelectedPart.steelAttr.PointBack.UR;
            }

            // 讀取切割線設定檔，並顯示TOP上的值
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<SteelCutSetting> steelcutSettings = new ObservableCollection<SteelCutSetting>();
            steelcutSettings = ser.GetCutSettingList();
            steelcutSettings = steelcutSettings ?? new ObservableCollection<SteelCutSetting>();
            if (steelcutSettings.Any(x => x.GUID == ViewModel.GuidProperty))
            {
                SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == ViewModel.GuidProperty);

                ViewModel.rbtn_CutFace = (int)cs.face;
                ViewModel.DLPoint.X = cs.DLX;
                ViewModel.DLPoint.Y = cs.DLY;
                ViewModel.ULPoint.X = cs.ULX;
                ViewModel.ULPoint.Y = cs.ULY;
                ViewModel.DRPoint.X = cs.DRX;
                ViewModel.DRPoint.Y = cs.DRY;
                ViewModel.URPoint.X = cs.URX;
                ViewModel.URPoint.Y = cs.URY;
            }
            else
            {
                SteelCutSetting cs = new SteelCutSetting()
                {
                    GUID = ViewModel.GuidProperty,
                    face = (FACE)ViewModel.rbtn_CutFace,
                    DLX = 0,
                    DLY = 0,
                    ULX = 0,
                    ULY = 0,
                    DRX = 0,
                    DRY = 0,
                    URX = 0,
                    URY = 0,
                };
                steelcutSettings.Add(cs);
                ser.SetCutSettingList(steelcutSettings);

                ViewModel.DLPoint.X = 0;
                ViewModel.DLPoint.Y = 0;
                ViewModel.ULPoint.X = 0;
                ViewModel.ULPoint.Y = 0;
                ViewModel.DRPoint.X = 0;
                ViewModel.DRPoint.Y = 0;
                ViewModel.URPoint.X = 0;
                ViewModel.URPoint.Y = 0;
            }

            //ViewModel.rbtn_CutFace = (int)FACE.TOP;

            //ViewModel.SteelAttr.GUID =CuurentSelectedPart.steelAttr.GUID;//1110 暫時註解掉，避免 e.OldItem, e.NewItem 間同時指向VM層連動導致(因為有binding到)e.OldItem資料被變更 CYH


            //CuurentSelectedPart.steelAttr.Name = CuurentSelectedPart.TeklaName;
            //CuurentSelectedPart.steelAttr.Title2 = CuurentSelectedPart.Title2;
            //CuurentSelectedPart.steelAttr.Title1 = CuurentSelectedPart.Title1;
            //CuurentSelectedPart.steelAttr.ShippingNumber = CuurentSelectedPart.ShippingNumber;
            //CuurentSelectedPart.steelAttr.Phase = CuurentSelectedPart.Phase;
            //CuurentSelectedPart.steelAttr.Material = CuurentSelectedPart.Material;
            //CuurentSelectedPart.steelAttr.Weight = ViewModel.SteelAttr.Weight;
            //CuurentSelectedPart.steelAttr.Number = (int)CuurentSelectedPart.Count;
            //CuurentSelectedPart.Length = CuurentSelectedPart.steelAttr.Length;
            //ViewModel.SteelAttr = CuurentSelectedPart.steelAttr;//1110 暫時註解掉，避免 e.OldItem, e.NewItem 間同時指向VM層連動導致(因為有binding到)e.OldItem資料被變更 CYH
            //ViewModel.SteelSectionProperty = profile;

            ViewModel.fPartListOrManuall = false;



            //ViewModel.SteelAttr.PartNumber = CuurentSelectedPart.steelAttr.PartNumber.ToString();
            //ViewModel.SteelAttr.AsseNumber = CuurentSelectedPart.steelAttr.AsseNumber.ToString();
            //ViewModel.SteelAttr.Type = CuurentSelectedPart.Type;
            //ViewModel.SteelTypeProperty_int = (int)CuurentSelectedPart.Type;
            //ViewModel.SteelTypeProperty_enum = CuurentSelectedPart.Type;
            //cbx_SectionTypeComboBox.Text = profile;
            //ViewModel.SteelAttr.Length = CuurentSelectedPart.Length;
            //ViewModel.SteelAttr.Profile = CuurentSelectedPart.Profile;
            //ViewModel.SteelAttr.Name = CuurentSelectedPart.TeklaName;
            //ViewModel.SteelAttr.Material = CuurentSelectedPart.Material;
            //ViewModel.SteelAttr.Phase = CuurentSelectedPart.Phase;
            //ViewModel.SteelAttr.ShippingNumber = CuurentSelectedPart.ShippingNumber;
            //ViewModel.SteelAttr.Title1 = CuurentSelectedPart.Title1;
            //ViewModel.SteelAttr.Title2 = CuurentSelectedPart.Title2;
            //ViewModel.SteelAttr.oPoint = CuurentSelectedPart.oPoint;
            //ViewModel.SteelAttr.uPoint = CuurentSelectedPart.uPoint;
            //ViewModel.SteelAttr.vPoint = CuurentSelectedPart.vPoint;
            //ViewModel.SteelAttr.CutList = CuurentSelectedPart.CutList;

            //this.partNumber.Text = ViewModel.PartNumberProperty;
            //this.asseNumber.Text = ViewModel.AssemblyNumberProperty;
            //this.cbx_SteelTypeComboBox.SelectedIndex = ViewModel.ProfileType;
            //this.cbx_SectionTypeComboBox.Text = ViewModel.SteelSectionProperty;
            //this.Length.Text = CuurentSelectedPart.Length.ToString();
            //this.Weight.Text = ViewModel.ProductWeightProperty.ToString();
        }

        public ModelExt GetFinalModel(string dataName)
        {
            STDSerialization ser = new STDSerialization();
            ReadFile readFile = ser.ReadPartModel(dataName.ToString()); //讀取檔案內容
            if (readFile == null)
            {
                WinUIMessageBox.Show(null,
                    $"專案Dev_Part資料夾讀取失敗",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return null;
            }
            readFile.DoWork();//開始工作
            model.Blocks.Clear();
            model.Entities.Clear();
            drawing.Blocks.Clear();
            drawing.Entities.Clear();
            try
            {
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
            }
            catch (Exception ex)
            {
                return null;
            }

            if (model.Entities.Count() == 0 || model.Entities[model.Entities.Count - 1].EntityData == null)
            {
                return model;
            }

            //ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            ViewModel.GetSteelAttr();
            if (ViewModel.SteelAttr.PartNumber == null && ViewModel.SteelAttr.AsseNumber == null)
            {
                // 錯誤狀況;無此判斷及SLEEP會造讀到的ViewModel.SteelAttr是new SteelAttr()
                // 20220922 呂宗霖 測試後 覺得是延遲造成程式把null寫回ViewModel.SteelAttr, 所以先用Sleep解決
                Thread.Sleep(1000);
                ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內1000
                ViewModel.GetSteelAttr();
            }
            SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;


            SteelAttr saDeepClone = (SteelAttr)steelAttr.DeepClone();
            List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
            steelAttr = ViewModel.ReadNCInfo(saDeepClone, ref groups, false);
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = steelAttr.oPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = steelAttr.vPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = steelAttr.uPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = steelAttr.CutList;

            // 舊有形鋼上的孔群
            List<Block> blocks = model.GetBoltFromBlock(groups);
            model.LoadNcToModel(dataName, ObSettingVM.allowType, 0, null, steelAttr, null, blocks, true);
            // 步驟5.產生2D模型
            BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            model.sycnModelEntitiesAndNewBolt(blocks);
            ViewModel.AddBolts(model, drawing, out bool hasOutSteel, blocks, false, false);
            // 切割線打點
            ScrollViewbox.IsEnabled = !model.RunHypotenuseEnable();
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);



            //Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
            //model.AddModelSteelAttr(steelAttr, result);
            //ViewModel.SteelTriangulation(drawing, result.Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊

            //bool hasOutSteel = false;
            //List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();
            //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
            //{
            //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
            //    {
            //        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
            //        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
            //        Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);
            //        //Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
            //        if (bolts3DBlock.hasOutSteel)
            //        {
            //            hasOutSteel = true;
            //        }
            //        B3DB.Add(bolts3DBlock);
            //        //Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
            //    }
            //}
            //if (hasOutSteel)
            //{
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //}
            //else
            //{
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;

            //}
            //foreach (Bolts3DBlock item in B3DB)
            //{
            //    BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
            //}
            ModelExt newModel = new ModelExt();
            newModel = model;
            return newModel;
        }
        private IEnumerable<string> GetAllNcPath(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string ext = Path.GetExtension(d);//副檔名
                    if (ext == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        yield return d;
                    }
                    else
                    {
                        GetAllNcPath(d);
                    }
                }
            }
        }
        /// 斷面規格與NC檔不同，值為true為一般型鋼
        /// </summary>
        public static bool isNormalProfile = false;
        /// <summary>
        /// <summary>
        /// Grid Select Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_SelectedChange(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.OldItem != null)
            {
                if (model != null && PieceListGridControl.SelectedItem != null)
                {
                    Esc();

                    ProductSettingsPageViewModel item = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                    item.steelAttr.GUID = Guid.Parse(item.DataName);
                    item.steelAttr.Number = (int)item.Count;
                    // 異動指標
                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                    int selectIndex = ((ObservableCollection<ProductSettingsPageViewModel>)e.Source.ItemsSource).ToList().FindIndex(x => x.DataName == item.DataName && x.AssemblyNumber == item.AssemblyNumber && x.steelAttr.PartNumber == item.steelAttr.PartNumber);
                    // 目前選擇畫面上第幾筆
                    int focusedRowHandle = this.PieceListGridControl.View.FocusedRowHandle;



                    //var itemSource = this.PieceListGridControl.ItemsSource;
                    //ObservableCollection<ProductSettingsPageViewModel> itemSource1 = (ObservableCollection<ProductSettingsPageViewModel>)e.Source.ItemsSource;
                    //int visibleRowCount = (e.Source).VisibleRowCount;
                    //int itemCount = ((ObservableCollection<ProductSettingsPageViewModel>)e.Source.ItemsSource).Count();
                    //int rowHandleByVisibleIndex = this.PieceListGridControl.GetRowHandleByVisibleIndex(selectIndex);
                    //int rowHandleByListIndex = this.PieceListGridControl.GetRowHandleByListIndex(selectIndex);
                    //int focusedRowHandle = this.PieceListGridControl.View.FocusedRowHandle;






                    //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                    PieceListGridControl.SelectItem(focusedRowHandle);

                    //99999999
                    //PieceListGridControl.View.FocusedRowHandle = selectIndex;



                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{item.Type}.inp");
                    //cbx_SectionTypeComboBox.Text = item.Profile;
                    //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                    if (item == null || selectIndex == -1)
                    {
                        ViewModel.fclickOK = false;
                        return;
                    }
                    //////////                    ConfirmCurrentSteelSection(item);
                    //ProductSettingsPageViewModel SelectRow = RowToEntity(item);
                    //ProductSettingsPageViewModel FinalRow = new ProductSettingsPageViewModel();
                    STDSerialization ser = new STDSerialization();
                    //DataCorrespond = ser.GetDataCorrespond();

                    string focuseGUID = item.DataName;

                    #region 最後一列之處理
                    // 檢查最後一筆之guid是否存在dev_Part中
                    // 不存在，詢問是否存檔
                    // 　　　　存檔，將指標移至最後一筆並更新畫面資料及SaveModel，讀取最後一列
                    // 　　　　不存檔，刪除最後一列，清空畫面資訊，讀取原選列
                    if (this.PieceListGridControl.VisibleRowCount > 0)
                    {
                        ProductSettingsPageViewModel FinalRow = (ProductSettingsPageViewModel)this.PieceListGridControl.GetRow(this.PieceListGridControl.VisibleRowCount - 1);
                        ProductSettingsPageViewModel temp = FinalRow;
                        string guid = FinalRow.DataName;
                        if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{guid}.dm"))
                        {
                            var ResultRtn = WinUIMessageBox.Show(null,
                            $"最後一筆零件({FinalRow.steelAttr.PartNumber})未存檔,是否存檔",
                            "通知",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);

                            if (ResultRtn == MessageBoxResult.Yes)
                            {
                                //// 指向最後一列
                                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                PieceListGridControl.View.FocusedRowHandle = this.PieceListGridControl.VisibleRowCount;
                                ////this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                ////PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
                                ////PieceListGridControl.View.FocusedRowHandle = this.PieceListGridControl.VisibleRowCount - 1;
                                ////this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                                // 還原元件資訊
                                RowToView(FinalRow);
                                ConfirmCurrentSteelSection(FinalRow);
                                item = FinalRow;
                                // 指向最後一列的guid
                                focuseGUID = guid;

                                //if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{focuseGUID}.dm"))
                                //{
                                //    ApplicationVM appVM = new ApplicationVM();
                                //    ReadFile readFile1 = ser.ReadPartModel(focuseGUID); //讀取檔案內容
                                //    if (readFile1 != null)
                                //    {
                                //        readFile1.DoWork();//開始工作
                                //        model.Blocks.Clear();
                                //        model.Entities.Clear();
                                //        drawing.Blocks.Clear();
                                //        drawing.Entities.Clear();
                                //        try
                                //        {
                                //            readFile1.AddToScene(model);//將讀取完的檔案放入到模型
                                //            appVM.CreateDMFile(model);
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            WinUIMessageBox.Show(null,
                                //               $"專案Dev_Part資料夾讀取失敗",
                                //               "通知",
                                //               MessageBoxButton.OK,
                                //               MessageBoxImage.Exclamation,
                                //               MessageBoxResult.None,
                                //               MessageBoxOptions.None,
                                //                FloatingMode.Window);
                                //            return;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        WinUIMessageBox.Show(null,
                                //            $"專案Dev_Part資料夾讀取失敗",
                                //            "通知",
                                //            MessageBoxButton.OK,
                                //            MessageBoxImage.Exclamation,
                                //            MessageBoxResult.None,
                                //            MessageBoxOptions.None,
                                //             FloatingMode.Window);
                                //        return;
                                //    }
                                //}
                                // 新零件
                                SaveModel(true, true);
                                //await SaveModelAsync(true, true);
                            }
                            else
                            {
                                int y = ((GridControl)e.OriginalSource).View.FocusedRowHandle;
                                #region 還原零件清單
                                if (this.PieceListGridControl.VisibleRowCount > 0)
                                {
                                    //刪除最後一列明細
                                    ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                    //source.Remove(source.Where(x => x.DataName == guid.ToString()).FirstOrDefault());
                                    ////this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                    //PieceListGridControl.ItemsSource = source;
                                    //PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
                                    ((ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource).RemoveAt(PieceListGridControl.VisibleRowCount - 1);
                                    PieceListGridControl.View.FocusedRowHandle = PieceListGridControl.VisibleRowCount;
                                    //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                }
                                #endregion

                                #region 清空零件屬性
                                ClearPartAttribute();
                                //this.asseNumber.Clear();
                                //this.partNumber.Clear();
                                //this.PartCount.Clear();
                                //this.Length.Clear();
                                //this.Weight.Text = "";
                                //this.PartCount.Clear();
                                //this.teklaName.Clear();
                                //this.phase.Clear();
                                //this.shippingNumber.Clear();
                                //this.Title1.Clear();
                                //this.Title2.Clear();
                                ////this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                //this.cbx_SteelTypeComboBox.SelectedIndex = 0;
                                ////ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)0}.inp");
                                //this.cbx_SectionTypeComboBox.SelectedIndex = 0;
                                ////this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                #endregion
                            }
                        }
                        else
                        {
                            // 還原指標
                            this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                            //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                            //PieceListGridControl.SelectItem(selectIndex);
                            PieceListGridControl.View.FocusedRowHandle = focusedRowHandle;
                            this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                       
                            //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{item.Type}.inp");
                            cbx_SectionTypeComboBox.Text = item.Profile;
                            //ViewModel.SteelSectionProperty = item.Profile;
                            }
                    }
                    #endregion

                    ViewModel.fclickOK = true;
                    // 既有零件
                    ViewModel.fFirstAdd = true;
                    ViewModel.fNewPart = false;
                    ViewModel.fGrid = true;
                    ViewModel.StateParaSetting(true, false, true);



                    model.Blocks.Clear();
                    model.Entities.Clear();
                    drawing.Blocks.Clear();
                    drawing.Entities.Clear();
                    model.Secondary = drawing;
                    drawing.Secondary = model;


                    int steelType = 0;
                    ReadFile readFile = ser.ReadPartModel(focuseGUID); //讀取檔案內容
                    if (readFile == null)
                    {
                        WinUIMessageBox.Show(null,
                            $"專案Dev_Part資料夾讀取失敗",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                        return;
                    }
                    readFile.DoWork();//開始工作
                    //model.SetCurrent(null);
                    //drawing.SetCurrent(null);
                    model.Blocks.Clear();
                    model.Entities.Clear();
                    drawing.Blocks.Clear();
                    drawing.Entities.Clear();
                    model.Secondary.Clear();
                    drawing.Secondary.Clear();
                    model.RootBlock.Entities.Clear();
                    try
                    {
                        readFile.AddToScene(model);//將讀取完的檔案放入到模型
                    }
                    catch (Exception ex)
                    {

                        WinUIMessageBox.Show(null,
                           $"專案Dev_Part資料夾讀取失敗",
                           "通知",
                           MessageBoxButton.OK,
                           MessageBoxImage.Exclamation,
                           MessageBoxResult.None,
                           MessageBoxOptions.None,
                            FloatingMode.Window);
                        return;
                    }
                    if (model.Blocks.Count == 1)
                    {
                        var a = model.Blocks[0];
                        a.Name = focuseGUID;
                        ((SteelAttr)a.Entities[0].EntityData).GUID = Guid.Parse(focuseGUID);
                        model.Blocks.Add(a);


                        WinUIMessageBox.Show(null,
                            $"專案Dev_Part資料夾讀取失敗",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                        return;
                    }

                    // 回填零件屬性
                    ConfirmCurrentSteelSection(item);

                    ////產生2D圖塊
                    //BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);

                    ////2D主件圖形
                    //BlockReference referenceMain = (BlockReference)drawing.Entities[drawing.Entities.Count - 1]; //主件圖形
                    ////取得2D鋼構圖塊(實際上模型中的孔)
                    //Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[referenceMain.BlockName]; 
                    ////建立2D圖塊細項
                    //model.Entities
                    //    .Where(x => x.EntityData is GroupBoltsAttr)
                    //    .ForEach(x =>
                    //    {
                    //        GroupBoltsAttr gba = (GroupBoltsAttr)x.EntityData;
                    //        Bolts3DBlock bolts3DBlock = new Bolts3DBlock(model.Blocks[gba.GUID.ToString()].Entities, gba);
                    //        ViewModel.Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                    //    });



                    //SteelAttr sa = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                    //SteelAttr sa = ViewModel.SteelAttr;//(SteelAttr)model.Blocks[1].Entities[0].EntityData;//1110 暫時註解掉，避免 e.OldItem, e.NewItem 間同時指向VM層連動導致(因為有binding到)e.OldItem資料被變更 CYH

                    //SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;


                    // 1.nc檔上的圖塊為一個孔為一個孔群
                    // 2.若NC檔上有5個孔 在無修改狀態下，須為五個孔
                    // 3.若已修改成3個孔，儘管nc檔讀出為五個孔，顯示仍須為3個孔
                    // 4.Block為圖塊(model.Blocks.Entities單孔的基本資訊+ model.Entities孔群的基本資訊)
                    // 5.Entities為實體(每一個孔的位置)
                    // 6.因為nc檔上的某些屬性雖然使用過代理(Surrogate)寫法，但仍存不進去檔案內，故每次仍須重讀nc檔

                    // 步驟1.取得舊有dm檔上的圖塊
                    // 舊有形鋼上的圖塊
                    //List<GroupBoltsAttr> modelAllBoltList = model.Entities.Where(x => x.EntityData.GetType() == typeof(GroupBoltsAttr)).Select(x => (GroupBoltsAttr)x.EntityData).Where(x=>x.Mode== AXIS_MODE.PIERCE).ToList();//孔群
                    //List<Block> oldBoltBlock = model.Blocks.Where(x => (modelAllBoltList.Select(z => z.GUID.ToString())).Contains(x.Name)).ToList();
                    // 步驟2.讀取nc檔，紀錄opoint.upoint.vpoint

                    SteelAttr sa = item.steelAttr;//1110 改由e.NewItem的steelAttr給值 CYH
                    ViewModel.SteelAttr = item.steelAttr;
                    model.Blocks[1].Entities[0].EntityData = sa;
                    SteelAttr saDeepClone = (SteelAttr)sa.DeepClone();
                    List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
                    sa = ViewModel.ReadNCInfo(saDeepClone, ref groups, false);
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.vPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.uPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;

                    if (saDeepClone.Profile.Replace("*", "X") != sa.Profile)
                    {
                        isNormalProfile = true;
                    }
                    else { isNormalProfile = false; }


                    // 舊有形鋼上的孔群
                    List<Block> blocks = model.GetBoltFromBlock(groups);

                    // 步驟3.產生鋼構模型
                    model.LoadNcToModel(focuseGUID, ObSettingVM.allowType, 0, null, sa, null, blocks, false, isNormalProfile);
                    // 步驟5.產生2D模型
                    BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                    //model.sycnModelEntitiesAndNewBolt(blocks);
                    ViewModel.AddBolts(model, drawing, out bool hasOutSteel, blocks, false);
                    // 切割線打點
                    ScrollViewbox.IsEnabled = true;
                    if (model.RunHypotenuseEnable()) { /*ScrollViewbox.IsEnabled = false;*/ } else { ScrollViewbox.IsEnabled = true; }
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
                    WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);

                    if (!Bolts3DBlock.CheckBolts(model, false))
                    {
                        //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                        item.steelAttr.ExclamationMark = true;
                        item.ExclamationMark = true;
                        PieceListGridControl.RefreshRow(PieceListGridControl.View.FocusedRowHandle);
                        //PieceListGridControl.RefreshRow(selectIndex);
                    }
                    else
                    {
                        //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                        item.steelAttr.ExclamationMark = false;
                        item.ExclamationMark = false;
                        PieceListGridControl.RefreshRow(PieceListGridControl.View.FocusedRowHandle);
                        //PieceListGridControl.RefreshRow(selectIndex);
                    }

                    Dictionary<string, ObservableCollection<SteelAttr>> saFile = ser.GetSteelAttr();

                    //(sa).Weight = ObSettingVM.PartWeight(new ProductSettingsPageViewModel()
                    //{
                    //    steelAttr = sa,
                    //    Length = sa.Length,
                    //    SteelType = (int)sa.Type,
                    //    Profile = sa.Profile,
                    //}, saFile);
                    //ViewModel.ProductWeightProperty = (sa).Weight;
                    //ViewModel.SteelAttr.Weight = (sa).Weight;

                    model.Refresh();
                    model.ZoomFit();//設置道適合的視口
                    model.Invalidate();//初始化模型
                    drawing.Refresh();
                    drawing.ZoomFit();//設置道適合的視口
                    drawing.Invalidate();//初始化模型
                }
                else
                {
                    ProductSettingsPageViewModel item = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                    //Grid_SelectedChange(sender, e);


                    if (item == null)
                    {
                        ViewModel.fclickOK = true;
                        return;
                    }
                    item.steelAttr.GUID = Guid.Parse(item.DataName);

                    ConfirmCurrentSteelSection(item);
                }
            }
        }

        /// <summary>
        /// Row Data to Model
        /// </summary>
        /// <param name="GUID">GUID</param>
        /// <param name="item">零件清單</param>
        public void DMtoModel(ProductSettingsPageViewModel item)
        {
            STDSerialization ser = new STDSerialization();
            ReadFile readFile = ser.ReadPartModel(item.DataName); //讀取檔案內容
            if (readFile == null)
            {
                WinUIMessageBox.Show(null,
                    $"專案Dev_Part資料夾讀取失敗",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return;
            }
            readFile.DoWork();//開始工作
            model.Blocks.Clear();
            model.Entities.Clear();
            drawing.Blocks.Clear();
            drawing.Entities.Clear();
            BlockReference blockReference;
            try
            {
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
            }
            catch (Exception)
            {
                //ViewModel = (ObSettingVM)DataContext;
                Steel3DBlock steel = Steel3DBlock.AddSteel(ViewModel.GetSteelAttr(), model, out blockReference);
                //((SteelAttr)model.Entities[0].EntityData).GUID = Guid.Parse(item.DataName);
                ((SteelAttr)model.Entities[0].EntityData).GUID = Guid.Parse(item.DataName);
                //BlockReference steel2D = SteelTriangulation((Mesh)steel.Entities[0]);
                //steel2D = SteelTriangulation((Mesh)steel.Entities.Where(x => x.GetType().Name == "Mesh").FirstOrDefault());
            }
            if (model.Blocks.Count == 1)
            {
                ViewModel.SteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                Steel3DBlock steel = Steel3DBlock.AddSteel(ViewModel.GetSteelAttr(), model, out blockReference);
                ((SteelAttr)model.Entities[0].EntityData).GUID = Guid.Parse(item.DataName);
                //steel2D = SteelTriangulation((Mesh)steel.Entities.Where(x => x.GetType().Name == "Mesh").FirstOrDefault());
            }

            ConfirmCurrentSteelSection(item);

            //List<Block> Block_BlockReference = model.Blocks.Where(x => x.Entities.GetType() == typeof(BlockReference) && x.Entities[0].EntityData.GetType() == typeof(GroupBoltsAttr)).ToList();
            //List<Block> Block_mesh = model.Blocks.Where(x => x.Entities.GetType() == typeof(Mesh) && x.Entities[0].EntityData.GetType() == typeof(BoltAttr)).Select(x => (Block)x).ToList();
            //List<Block> blocks = new List<Block>();
            //Block_BlockReference.AddRange(Block_mesh);
            //blocks = Block_BlockReference; 


            SteelAttr sa = GetViewToSteelAttr(new SteelAttr(), false, Guid.Parse(item.DataName));
            List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
            SteelAttr saDeepClone = (SteelAttr)sa.DeepClone();
            sa = ViewModel.ReadNCInfo(saDeepClone, ref groups, false);
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.vPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.uPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;

            List<Block> blocks = model.GetBoltFromBlock(groups);

            #region 建模
            model.LoadNcToModel(item.DataName, ObSettingVM.allowType, 0, null, sa, null, blocks, false);
            // 步驟5.產生2D模型
            BlockReference steel2D = ViewModel.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            //model.sycnModelEntitiesAndNewBolt(blocks);
            ViewModel.AddBolts(model, drawing, out bool hasOutSteel, blocks, false);
            // 切割線打點
            ScrollViewbox.IsEnabled = true;
            if (model.RunHypotenuseEnable()) { /*ScrollViewbox.IsEnabled = false;*/ } else { ScrollViewbox.IsEnabled = true; }
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.TOP);
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.FRONT);
            WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, ViewModel, FACE.BACK);
            //// 建立型鋼
            //Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);
            //model.AddModelSteelAttr(sa, result);
            //// 建立2D模型
            //ViewModel.SteelTriangulation(drawing, result.Name, (Mesh)model.Blocks[1].Entities[0]);
            //// 建立2D/3D孔
            //ViewModel.AddBolts(model, drawing, out bool hasOutSteel, blocks);
            //// 切割線打點
            //ScrollViewbox.IsEnabled = true;
            //if (model.RunHypotenuseEnable()) { /*ScrollViewbox.IsEnabled = false;*/ } else { ScrollViewbox.IsEnabled = true; }
            //WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, FACE.TOP);
            //WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, FACE.FRONT);
            //WPFSTD105.Model.Expand.ManHypotenusePoint(model, drawing, FACE.BACK);
            #endregion



            ////model.Blocks[1].Entities[0].EntityData = sa;
            ////Steel3DBlock result = new Steel3DBlock(Steel3DBlock.GetProfile((SteelAttr)model.Blocks[1].Entities[0].EntityData));


            ////model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
            ////model.Blocks[1].Name = sa.GUID.Value.ToString();
            //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊



            //List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();
            ////List<string> BoltBlockName = (from a in model.Entities where a.EntityData.GetType() == typeof(GroupBoltsAttr) select ((BlockReference)a).BlockName).ToList();
            //foreach (GroupBoltsAttr bolt in groups)
            //{
            //    Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts(bolt, model, out BlockReference blockRef, out bool checkRef);
            //    if (bolts3DBlock.hasOutSteel)
            //    {
            //        hasOutSteel = true;
            //    }
            //        B3DB.Add(bolts3DBlock);
            //}
            //if (hasOutSteel)
            //{
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //    item.steelAttr.ExclamationMark = true;
            //    item.ExclamationMark = true;
            //}
            //else
            //{
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
            //    item.steelAttr.ExclamationMark = false;
            //    item.ExclamationMark = false;
            //}
            //foreach (Bolts3DBlock bolt in B3DB)
            //{
            //    BlockReference referenceBolts = Add2DHole(bolt);//加入孔位到2D
            //}

            Dictionary<string, ObservableCollection<SteelAttr>> saFile = ser.GetSteelAttr();
            //double length = (sa).Length;
            //steelType = (int)((sa).Type);
            //profile = (sa).Profile;

            (sa).Weight = ObSettingVM.PartWeight(new ProductSettingsPageViewModel()
            {
                Length = sa.Length,
                SteelType = (int)sa.Type,
                Profile = sa.Profile,
            }, saFile);
            ViewModel.ProductWeightProperty = (sa).Weight;
            ViewModel.SteelAttr.Weight = (sa).Weight;
            model.ZoomFit();//設置道適合的視口
            model.Invalidate();//初始化模型
            drawing.ZoomFit();//設置道適合的視口
            drawing.Invalidate();









            ////SteelAttr sa = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            //SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
            //sa = GetViewToSteelAttr(sa,false, sa.GUID);
            ////ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            //ViewModel.WriteSteelAttr(sa);//寫入到設定檔內
            //ViewModel.GetSteelAttr();
            //GetViewToViewModel(false, sa.GUID);
            //ViewModel.SteelAttr.PartNumber = ViewModel.PartNumberProperty;
            //ViewModel.SteelAttr.AsseNumber = ViewModel.AssemblyNumberProperty;
            //if (ViewModel.SteelAttr.PartNumber == null && ViewModel.SteelAttr.AsseNumber == null)
            //{
            //    // 錯誤狀況;無此判斷及SLEEP會造讀到的ViewModel.SteelAttr是new SteelAttr()
            //    // 20220922 呂宗霖 測試後 覺得是延遲造成程式把null寫回ViewModel.SteelAttr, 所以先用Sleep解決
            //    Thread.Sleep(1000);
            //    ViewModel.WriteSteelAttr(sa);//寫入到設定檔內1000
            //    ViewModel.GetSteelAttr();
            //}
            ////ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
            //cbx_SectionTypeComboBox.Text = sa.Profile;

            //Steel3DBlock result = new Steel3DBlock(Steel3DBlock.GetProfile(ViewModel.SteelAttr));
            //if (model.Blocks.Count > 1)
            //{
            //    model.Blocks.Remove(model.Blocks[1]);
            //}
            //model.Blocks.Insert(1, result);//加入鋼構圖塊到模型
            //BlockReference blockReference1 = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);
            //blockReference1.EntityData = ViewModel.SteelAttr;
            //blockReference1.Selectable = false;//關閉用戶選擇
            //blockReference1.Attributes.Add("steel", new AttributeReference(0, 0, 0));
            ////if (model.Entities.Count > 0)
            ////{
            ////    model.Entities.RemoveAt(model.Entities.Count - 1);
            ////}
            //model.Entities.Add(blockReference1);//加入參考圖塊到模型


            ////model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
            //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊

            //bool hasOutSteel = false;
            //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
            //{
            //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
            //    {
            //        blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
            //        Block block = model.Blocks[blockReference1.BlockName]; //取得圖塊
            //        Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);
            //        //Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
            //        if (bolts3DBlock.hasOutSteel)
            //        {
            //            hasOutSteel = true;
            //        }
            //        Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
            //    }
            //}
            //if (hasOutSteel)
            //{
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //    item.steelAttr.ExclamationMark = true;
            //    item.ExclamationMark = true;
            //}
            //ser = new STDSerialization();
            //Dictionary<string, ObservableCollection<SteelAttr>> saFile = ser.GetSteelAttr();
            //double length = (sa).Length;
            //int steelType = (int)((sa).Type);
            //string profile = (sa).Profile;

            //(sa).Weight = ObSettingVM.PartWeight(new ProductSettingsPageViewModel()
            //{
            //    Length = length,
            //    SteelType = steelType,
            //    Profile = profile,
            //}, saFile);
            //ViewModel.ProductWeightProperty = (sa).Weight;



        }
        /// <summary>
        /// 零件清單重載
        /// </summary>
        public void GridReload()
        {
#if DEBUG
            log4net.LogManager.GetLogger("GridReload()").Debug("");
#endif
            //List<ProductSettingsPageViewModel> old_source = new List<ProductSettingsPageViewModel>();
            //var old_source_list = (from item in old_source
            //                       select new ProductSettingsPageViewModel()
            //                       ).ToList();

            //ObservableCollection<ProductSettingsPageViewModel> new_source = new ObservableCollection<ProductSettingsPageViewModel>(old_source_list);
            //new_source.Add(new ProductSettingsPageViewModel());

            ProductSettingsPageViewModel aa = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
            if (aa != null)
            {
                ObservableCollection<ProductSettingsPageViewModel> collection = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                ViewModel.DataViews = collection;
                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                //PreIndex = collection.FindIndex(x => x.DataName == aa.DataName);
                PreIndex = collection.FindIndex(x => x.DataName == ViewModel.GuidProperty.ToString() && x.AssemblyNumber == ViewModel.AssemblyNumberProperty);
                if (PreIndex != -1)
                {
                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    PieceListGridControl.ItemsSource = collection;
                    var rowHandle = PieceListGridControl.GetRowHandleByListIndex(PreIndex);
                    PieceListGridControl.RefreshRow(rowHandle);
                    PieceListGridControl.View.FocusedRowHandle = rowHandle;
                    PieceListGridControl.SelectItem(rowHandle);
                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(collection[rowHandle].Type).ToString()}.inp");
                    //cbx_SectionTypeComboBox.Text = collection[PreIndex].Profile;
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                }
            }
            else
            {
                ////ObservableCollection<ProductSettingsPageViewModel> collection = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                ////ViewModel.DataViews = collection;
                //ObservableCollection<ProductSettingsPageViewModel> collection = ViewModel.DataViews;
                //if (collection.Count > 0)
                //{
                //    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                //    PieceListGridControl.ItemsSource = collection;
                //    PieceListGridControl.RefreshData();
                //    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                //    this.PieceListGridControl.SelectItem(0);
                //    PieceListGridControl.View.FocusedRowHandle = 0;
                //    aa = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                //    ConfirmCurrentSteelSection(aa);
                //}
                ObservableCollection<ProductSettingsPageViewModel> collection = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());

                ViewModel.DataViews = collection;
                //PreIndex = collection.FindIndex(x => x.DataName == aa.DataName);
                PreIndex = collection.FindIndex(x => x.DataName == ViewModel.GuidProperty.ToString() && x.AssemblyNumber == ViewModel.AssemblyNumberProperty);
                if (PreIndex != -1)
                {
                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    PieceListGridControl.ItemsSource = collection;
                    var rowHandle = PieceListGridControl.GetRowHandleByListIndex(PreIndex);
                    PieceListGridControl.RefreshRow(rowHandle);
                    PieceListGridControl.View.FocusedRowHandle = rowHandle;
                    PieceListGridControl.SelectItem(rowHandle);
                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(collection[rowHandle].Type).ToString()}.inp");
                    //cbx_SectionTypeComboBox.Text = collection[PreIndex].Profile;
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                }
            }
        }
        private void OKtoConfirmChanges(object sender, RoutedEventArgs e)
        {
            //if (fNewPart.Value)
            //{
            //    var ResultRtn = WinUIMessageBox.Show(null,
            //             $"新增零件是否存檔 ?",
            //             "通知",
            //             MessageBoxButton.OKCancel,
            //             MessageBoxImage.Exclamation,
            //             MessageBoxResult.None,
            //             MessageBoxOptions.None,
            //              FloatingMode.Window);
            //    if (ResultRtn == MessageBoxResult.Yes)
            //    { SaveModel(true,ViewModel.SteelAttr, true); }
            //    else
            //    {
            //        // 清空零件屬性
            //        this.asseNumber.Clear();
            //        this.partNumber.Clear();
            //        this.PartCount.Clear();
            //        this.Length.Clear();
            //        this.Weight.Text = "";
            //        this.PartCount.Clear();
            //        this.teklaName.Clear();
            //        this.phase.Clear();
            //        this.shippingNumber.Clear();
            //        this.Title1.Clear();
            //        this.Title2.Clear();
            //        this.cbx_SteelTypeComboBox.SelectedIndex = 0;
            //        this.cbx_SectionTypeComboBox.SelectedIndex = 0;
            //        GridReload();
            //    }




            //if (fAddSteelPart)
            //{
            //    var ResultRtn = WinUIMessageBox.Show(null,
            //             $"新增零件是否存檔 ?",
            //             "通知",
            //             MessageBoxButton.OKCancel,
            //             MessageBoxImage.Exclamation,
            //             MessageBoxResult.None,
            //             MessageBoxOptions.None,
            //              FloatingMode.Window);


            //    if (ResultRtn == MessageBoxResult.OK)
            //        SaveModel(true);//存取檔案

            //    GridReload();


            //    fAddSteelPart = false;
            //}
            //}
        }


        bool TableViewLoadedBoolen = false;
        private void Material_List_TableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
            TableViewLoadedBoolen = true;
        }


        private void PartListTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;

        }
        private void SoftCountTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
        }
        private void Material_List_GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (TableViewLoadedBoolen == false)
                return;

            var SenderC = sender as DevExpress.Xpf.Grid.GridControl;
            if (SenderC.View != null)
            {
                if (e.NewItem is GD_STD.Data.MaterialDataView)
                {
                    var ENewItem = (GD_STD.Data.MaterialDataView)e.NewItem;
                    ENewItem.ButtonEnable = true;

                    var NewHandle = SenderC.FindRow(e.NewItem);
                    SenderC.RefreshRow(NewHandle);//畫面裡刷新上面該列的設定值
                }
                if (e.OldItem is GD_STD.Data.MaterialDataView)
                {
                    var EOldItem = (GD_STD.Data.MaterialDataView)e.OldItem;
                    EOldItem.ButtonEnable = false;
                    var OldHandle = SenderC.FindRow(e.OldItem);
                    SenderC.RefreshRow(OldHandle);//畫面裡刷新上面該列的設定值
                }
            }
        }
        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            /*  if ((sender as DevExpress.Xpf.Grid.TableView).Name == PartListTableView.Name)
              {
                  IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(SoftCountTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                  if (SoftCountTableView_ScrollElement != null)
                      SoftCountTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
              }
              if ((sender as DevExpress.Xpf.Grid.TableView).Name == SoftCountTableView.Name)
              {
                  IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(PartListTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                  if (PartsTableView_ScrollElement != null)
                      PartsTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
              }*/

        }
        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PieceListGridControl.VisibleRowCount == 1)
            {
                ProductSettingsPageViewModel item = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                if (item != null)
                {
                    ProductSettingsPageViewModel row = (ProductSettingsPageViewModel)this.PieceListGridControl.GetRow(this.PieceListGridControl.VisibleRowCount - 1);
                    ProductSettingsPageViewModel temp = RowToEntity(row);
                    string guid = row.DataName;
                    if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{guid}.dm"))
                    {
                        var ResultRtn = WinUIMessageBox.Show(null,
                        $"新增零件未存檔,是否存檔",
                        "通知",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);

                        if (ResultRtn == MessageBoxResult.Yes)
                        {
                            // 指向最後一列
                            this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                            PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
                            PieceListGridControl.View.FocusedRowHandle = this.PieceListGridControl.VisibleRowCount - 1;
                            this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                            // 還原元件資訊
                            this.asseNumber.Text = temp.steelAttr.AsseNumber;
                            this.partNumber.Text = temp.steelAttr.PartNumber;
                            this.Length.Text = $"{temp.steelAttr.Length}";
                            this.Weight.Text = $"{temp.steelAttr.Weight}";
                            this.PartCount.Text = $"{temp.steelAttr.Number}";
                            this.teklaName.Text = temp.steelAttr.Name;
                            this.material.Text = temp.steelAttr.Material;
                            this.phase.Text = $"{temp.steelAttr.Phase}";
                            this.shippingNumber.Text = $"{temp.steelAttr.ShippingNumber}";
                            this.Title1.Text = temp.steelAttr.Title1;
                            this.Title2.Text = temp.steelAttr.Title2;
                            this.cbx_SteelTypeComboBox.SelectedIndex = (int)temp.SteelType;
                            ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{temp.Type}.inp");
                            ViewModel.SteelSectionProperty = temp.steelAttr.Profile;
                            this.cbx_SectionTypeComboBox.Text = temp.steelAttr.Profile;

                            this.H.Text = $"{temp.steelAttr.H}";
                            this.W.Text = $"{temp.steelAttr.W}";
                            this.t1.Text = $"{temp.steelAttr.t1}";
                            this.t2.Text = $"{temp.steelAttr.t2}";

                            // 指向最後一列的guid
                            string focuseGUID = guid;

                            if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{focuseGUID}.dm"))
                            {
                                ApplicationVM appVM = new ApplicationVM();
                                appVM.CreateDMFile(model);
                            }


                            // 新零件
                            SaveModel(true, true);
                            model.Refresh();
                            drawing.Refresh();
                        }
                        else
                        {
                            #region 還原零件清單
                            if (this.PieceListGridControl.VisibleRowCount > 0)
                            {
                                //刪除最後一列明細
                                ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                                source.Remove(source.Where(x => x.DataName == guid.ToString()).FirstOrDefault());
                                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                PieceListGridControl.ItemsSource = source;
                                PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
                                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                            }
                            #endregion

                            #region 清空零件屬性
                            this.asseNumber.Clear();
                            this.partNumber.Clear();
                            this.PartCount.Clear();
                            this.Length.Clear();
                            this.Weight.Text = "";
                            this.PartCount.Clear();
                            this.teklaName.Clear();
                            this.phase.Clear();
                            this.shippingNumber.Clear();
                            this.Title1.Clear();
                            this.Title2.Clear();
                            this.cbx_SteelTypeComboBox.SelectedIndex = 0;
                            this.cbx_SectionTypeComboBox.SelectedIndex = 0;
                            #endregion
                        }

                    }
                    else
                    {
                        // 還原指標
                        this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        PieceListGridControl.SelectItem(0);
                        PieceListGridControl.View.FocusedRowHandle = 0;
                        ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{item.Type}.inp");
                        cbx_SectionTypeComboBox.Text = item.Profile;
                        ViewModel.SteelSectionProperty = item.Profile;
                        this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                        ConfirmCurrentSteelSection(item);
                        DMtoModel(item);
                        //GetViewToViewModel(false, Guid.Parse(item.DataName));
                        //SaveModel(false, true);
                        GridReload();
                        model.Refresh();
                        drawing.Refresh();
                    }
                }
            }
        }
        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

            model.ZoomFit();
            model.Refresh();
        }
        private void GridSplitter_MouseMove(object sender, MouseEventArgs e)
        {
            model.ZoomFit();//設置道適合的視口
            drawing.ZoomFit();//設置道適合的視口


        }
        public bool GetverticesFromFile(string PartNumber, SteelAttr TmpSA, ref SteelAttr TmpSteeAttr, int SteelIndex = 1)
        {
            bool rtn = false;
            string path = ApplicationVM.DirectoryNc();
            string allPath = path + $"\\{PartNumber}.nc1";
            if (File.Exists($@"{allPath}"))
            {
                STDSerialization ser = new STDSerialization();
                TmpSA = (SteelAttr)model.Blocks[SteelIndex].Entities[0].EntityData;

                var profile = ser.GetSteelAttr();
                Steel3DBlock s3Db = new Steel3DBlock();
                SteelAttr steelAttrNC = new SteelAttr();
                List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
                s3Db.ReadNcFile($@"{ApplicationVM.DirectoryNc()}\{PartNumber}.nc1", profile, TmpSA, ref TmpSteeAttr, ref groups);
                #region 異動欄位後DeepClone
                TmpSA.oPoint = steelAttrNC.oPoint;
                TmpSA.vPoint = steelAttrNC.vPoint;
                TmpSA.uPoint = steelAttrNC.uPoint;
                TmpSA.CutList = steelAttrNC.CutList;
                #endregion
                TmpSteeAttr = (SteelAttr)TmpSA.DeepClone();

                rtn = true;
            }
            return rtn;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            if (drawing.drawingHypotenusePoint && drawing.points.Count == 2)
            {
                SaveModel(false, false);
                drawing.drawingHypotenusePoint = false;
            }
        }

    }
}