using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DevExpress.Data.Extensions;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;
using STD_105.Office;
using WPFWindowsBase;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using BlockReference = devDept.Eyeshot.Entities.BlockReference;
using MouseButton = devDept.Eyeshot.MouseButton;
using System.IO;
using GD_STD.Data;
using System.Collections.ObjectModel;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using System.Reflection;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using SplitLineSettingData;
using WPFSTD105.Tekla;
using System.Threading;
using DevExpress.Mvvm;
using System.Collections;
using GD_STD;
using System.Threading.Tasks;

namespace STD_105
{
    /// <summary>
    /// Product.xaml 的互動邏輯
    /// </summary>
    public partial class ProductSettingsPage_Machine : BasePage<ObSettingVM>
    {

        ApplicationVM appVM = new ApplicationVM();
        public ObSettingVM sr = new ObSettingVM();
        private SplashScreenManager ProcessingScreenWin = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });

        public bool? fFirstAdd = true;// 是否第一次按新增 有點
        public bool? fNewPart = true;// 是否為新零件 dm
        public bool? fGrid = false;// 是否點擊Grid 新增 修改後為false
        public bool? fclickOK = true; // 是否直接點擊OK
      
        public bool fAddSteelPart = false;       //  判斷執行新增零件及孔位
        public bool fAddHypotenusePoint = false;   //  判斷執行斜邊打點
        List<Bolts3DBlock> lstBoltsCutPoint = new List<Bolts3DBlock>();
        /// <summary>
        /// Grid Reload前的Index
        /// </summary>
        public int PreIndex { get; set; }
        /// <summary>
        /// 是否產生新零件
        /// 新增.修改為true
        /// 加入切割線為false
        /// </summary>
        public bool isNewPart = false;
        /// <summary>
        /// 是否為首次匯入NC&BOM的falg
        /// </summary>
        private bool fAfterFirstImportTeklaData { get; set; } = true;


        public ObservableCollection<DataCorrespond> DataCorrespond { get; set; } = new ObservableCollection<DataCorrespond>();
        /// <summary>
        /// 20220823 蘇冠綸 製品設定
        /// </summary>
        public ProductSettingsPage_Machine()
        {
            InitializeComponent();
            //2022.06.24 呂宗霖 此Class與GraphWin.xaml.cs皆有SteelTriangulation與Add2DHole
            //                  先使用本Class 若有問題再修改
            //GraphWin service = new GraphWin();

            /// <summary>
            /// 鑽孔radio button測試 20220906 張燕華
            /// </summary>
            this.ViewModel.CmdShowMessage = new RelayCommand(() =>
            {
                System.Windows.MessageBox.Show("You Select " + ViewModel.rbtn_DrillingFace.ToString());
            });

            #region 3D
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.InitializeViewports();
            //model.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            model.Secondary = drawing;
            #endregion
            #region 2D
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.InitializeViewports();
            //drawing.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            drawing.Secondary = model;
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
                    FloatingMode.Popup);
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
                    FloatingMode.Popup);
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
            }
            else
            {
                steelAttr.GUID = guid;
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
                       FloatingMode.Popup);
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
                      FloatingMode.Popup);
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
                       FloatingMode.Popup);
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
                           FloatingMode.Popup);
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
                           FloatingMode.Popup);
                        return false;
                    }

                    break;
            }
            return true;
        }

        /// <summary>
        /// 修改螺栓狀態
        /// </summary>
        public bool modifyHole { get; set; } = false;

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
                //    //if (!fAddSteelPart)
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
                        FloatingMode.Popup);
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
            if (ViewModel.PartNumberProperty == "" || ViewModel.PartNumberProperty == null)//檢測用戶是否有輸入零件編號
            {
                WinUIMessageBox.Show(null,
                $"請輸入零件編號",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Popup);
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
                    FloatingMode.Popup);
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
                    FloatingMode.Popup);
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
            //        FloatingMode.Popup);
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
        /// 自動斜邊打點
        /// </summary>
        public async void RunHypotenuseEnable()
        {
            lstBoltsCutPoint = new List<Bolts3DBlock>();
            ScrollViewbox.IsEnabled = true;

            if (model.Entities[model.Entities.Count - 1].EntityData is null)
                return;

            SteelAttr TmpSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            //GetViewToViewModel(false, TmpSteelAttr.GUID);

            // 
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
            if (isHypotenuse) ScrollViewbox.IsEnabled = false;
            //
            //if (isHypotenuse)
            //{
            //    //ViewLocator.OfficeViewModel.isHypotenuse = true;
            //    //ScrollViewbox.IsEnabled = false;
            //}
            //else
            //{
            //    //ViewLocator.OfficeViewModel.isHypotenuse = false;
            //    //ScrollViewbox.IsEnabled = true;
            //}


            //// 只有既有零件(NC/BOM匯入)才有斜邊
            //if (!fNewPart.Value)
            //    //if (!fAddSteelPart)   //  新建孔群是否於新增零件  : false 直接存檔
            //    SaveModel(false, false);//存取檔案 
            ////await SaveModelAsync(false, false);

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


            bool hasOutSteel = false;

            switch (face)
            {

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
            }
            //ViewLocator.OfficeViewModel.HypotenuseEnable = true;
            return isHypotenuse;
        }


        /// <summary>
        /// 手動斜邊打點
        /// </summary>
        public void ManHypotenusePoint(FACE face)
        {
#if DEBUG
            log4net.LogManager.GetLogger("ManHypotenusePoint").Debug("");
#endif

            double a, b;
            List<(double, double)> DRPoint = new List<(double, double)>();
            List<(double, double)> HypotenusePoint = new List<(double, double)>();
            List<Point3D> result = null;

            MyCs myCs = new MyCs();

            STDSerialization ser = new STDSerialization();
            ObservableCollection<SplitLineSettingClass> ReadSplitLineSettingData = ser.GetSplitLineData();//備份當前加工區域數值

            double PosRatioA = myCs.DivSymbolConvert(ReadSplitLineSettingData == null ? "0" : ReadSplitLineSettingData[0].A);   //  腹板斜邊打點比列(短)
            double PosRatioB = myCs.DivSymbolConvert(ReadSplitLineSettingData == null ? "0" : ReadSplitLineSettingData[0].B);    //  腹板斜邊打點比列(長)
            double PosRatioC = myCs.DivSymbolConvert(ReadSplitLineSettingData == null ? "0" : ReadSplitLineSettingData[0].C);    //  翼板斜邊打點比列(短)
            double PosRatioD = myCs.DivSymbolConvert(ReadSplitLineSettingData == null ? "0" : ReadSplitLineSettingData[0].D);     //  翼板斜邊打點比列(長)

            //SteelAttr steelAttr = ViewModel.GetSteelAttr();
            SteelAttr steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;


            bool hasOutSteel = false;
            List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();
            switch (face)
            {
                case FACE.TOP:

                    if (steelAttr.Top == null)
                        return;

                    //UL
                    result = steelAttr.Top.UL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, (PosRatioA * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, (PosRatioB * b) + result[0].Y));
                    }

                    //UR
                    result = steelAttr.Top.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, result[1].Y - (PosRatioA * b)));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, result[1].Y - (PosRatioB * b)));
                    }

                    //DL
                    result = steelAttr.Top.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioA * a), result[2].Y - (PosRatioA * b)));
                        HypotenusePoint.Add(((PosRatioB * a), result[2].Y - (PosRatioB * b)));
                    }

                    //DR
                    result = steelAttr.Top.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, (PosRatioA * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, (PosRatioB * b) + result[1].Y));
                    }

                    B3DB = new List<Bolts3DBlock>();
                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.TOP, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.HypotenusePOINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        TmpBoltsArr.BlockName = "ManHypotenuse";
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool check);
                        if (bolts.hasOutSteel)
                        {
                            hasOutSteel = true;
                        }
                        B3DB.Add(bolts);
                        //BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                        //Add2DHole(bolts, false);//加入孔位不刷新 2d 視圖
                    }
                    foreach (Bolts3DBlock item in B3DB)
                    {
                        BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
                    }
                    break;

                case FACE.BACK:

                    if (steelAttr.Back == null)
                        return;

                    //UL
                    result = steelAttr.Back.UL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[0].Y));
                    }

                    //UR
                    result = steelAttr.Back.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, result[1].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, result[1].Y - (PosRatioD * b)));
                    }

                    //DL
                    result = steelAttr.Back.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a), result[2].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a), result[2].Y - (PosRatioD * b)));
                    }

                    //DR
                    result = steelAttr.Back.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[1].Y));
                    }

                    B3DB = new List<Bolts3DBlock>();
                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.BACK, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.HypotenusePOINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        TmpBoltsArr.BlockName = "ManHypotenuse";
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        if (bolts.hasOutSteel)
                        {
                            hasOutSteel = true;
                        }
                        B3DB.Add(bolts);

                    }
                    foreach (Bolts3DBlock item in B3DB)
                    {
                        BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
                    }
                    break;

                case FACE.FRONT:
                    if (steelAttr.Front == null)
                        return;

                    //UL
                    result = steelAttr.Front.UL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[0].Y));
                    }

                    //UR                    
                    result = steelAttr.Front.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, result[1].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, result[1].Y - (PosRatioD * b)));
                    }

                    //DL
                    result = steelAttr.Front.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a), result[2].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a), result[2].Y - (PosRatioD * b)));
                    }

                    //DR
                    result = steelAttr.Front.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[DRPoint.Count - 1].Item1;
                        b = DRPoint[DRPoint.Count - 1].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[1].Y));
                    }

                    B3DB = new List<Bolts3DBlock>();
                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.FRONT, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.HypotenusePOINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.BlockName = "ManHypotenuse";
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        if (bolts.hasOutSteel)
                        {
                            hasOutSteel = true;
                        }
                        B3DB.Add(bolts);
                        //BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                    }
                    foreach (Bolts3DBlock item in B3DB)
                    {
                        BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
                    }
                    break;
            }

            steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;

            if (hasOutSteel)
            {
                steelAttr.ExclamationMark = true;
            }

            //if (!fNewPart.Value)
            //    //if (!fAddSteelPart)   //  新建孔群是否於新增零件  : false 直接存檔
            //    SaveModel(false, false);//存取檔案 


            //刷新模型
            model.Refresh();
            drawing.Refresh();

            fAddHypotenusePoint = true; //  執行斜邊打點功能


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
            //if (!fAddSteelPart)
            //{
            //    SaveModel(false);//存取檔案
            //}

        }



        public static async Task SaveModelAsync(bool add, bool reflesh = true)
        {
            await SaveModelAsync(add, reflesh);
        }

        /// <summary>
        /// 存取模型
        /// </summary>
        /// <param name="add"></param>
        /// <param name="steelAttr"></param>
        /// <param name="reflesh">是否更新Grid</param>
        public void SaveModel(bool add, bool reflesh = true)
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

            // 原零件之構建ID
            // 取出所有零件
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();
            // 所有零件攤平
            var allPart = part.SelectMany(x => x.Value).ToList();
            // 原始零件
            var oldPart = allPart.FirstOrDefault(x => x.GUID == Guid.Parse(((ProductSettingsPageViewModel)this.PieceListGridControl.SelectedItem).DataName));
            if (allPart.Any(x => x.GUID == Guid.Parse(((ProductSettingsPageViewModel)this.PieceListGridControl.SelectedItem).DataName)))
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
                steelPart.ExclamationMark = exclamationMark;
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
                Debugger.Break();
            }
#if DEBUG
            log4net.LogManager.GetLogger("觸發線性標註").Debug("");
#endif
            //ModelExt modelExt= new ModelExt();
            Dim(out modelExt);
            modelExt.drawingLinearDim = true;

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

            drawing.ZoomFit();//設置道適合的視口
            drawing.Refresh();//刷新模型

            model.ZoomFit();
            model.Refresh();

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
            //                     FloatingMode.Popup);


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
            //            //         FloatingMode.Popup);


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
            //                        FloatingMode.Popup);
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
                var pf = ViewModel.ProfileList[cbx_SectionTypeComboBox.SelectedIndex];
                ViewModel.SteelAttr.H = pf.H;
                ViewModel.SteelAttr.W = pf.W;
                ViewModel.SteelAttr.t1 = pf.t1;
                ViewModel.SteelAttr.t2 = pf.t2;
                ViewModel.SteelAttr.Profile = pf.Profile;
                ViewModel.CurrentPartSteelAttr = ViewModel.ProfileList[cbx_SectionTypeComboBox.SelectedIndex]; //ViewModel.SteelAttr;
                ViewModel.SteelSectionProperty = pf.Profile;
                cbx_SectionTypeComboBox.Text = pf.Profile;

                ViewModel.CurrentPartSteelAttr.H = pf.H;
                ViewModel.CurrentPartSteelAttr.W = pf.W;
                ViewModel.CurrentPartSteelAttr.t1 = pf.t1;
                ViewModel.CurrentPartSteelAttr.t2 = pf.t2;
                ViewModel.CurrentPartSteelAttr.Profile = pf.Profile;
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

            ViewModel.PartNumberProperty = CuurentSelectedPart.steelAttr.PartNumber;
            ViewModel.AssemblyNumberProperty = CuurentSelectedPart.steelAttr.AsseNumber;
            ViewModel.ProductLengthProperty = CuurentSelectedPart.steelAttr.Length;
            if (CuurentSelectedPart.steelAttr.Kg != 0)
            {
                ViewModel.ProductWeightProperty = (CuurentSelectedPart.steelAttr.Length / 1000) * CuurentSelectedPart.steelAttr.Kg;
            }
            else
            {
                ViewModel.ProductWeightProperty = CuurentSelectedPart.steelAttr.Weight;
                if (CuurentSelectedPart.steelAttr.Weight == 0) ViewModel.ProductWeightProperty = ViewModel.CalculateSinglePartWeight();
            }
            ViewModel.SteelAttr.Weight = ViewModel.ProductWeightProperty;
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
                    FloatingMode.Popup);
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
            Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
            model.AddModelSteelAttr(steelAttr, result);
            sr.SteelTriangulation(drawing, result.Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊

            bool hasOutSteel = false;
            List<Bolts3DBlock> B3DB = new List<Bolts3DBlock>();
            for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                {
                    BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
                    Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);
                    //Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
                    if (bolts3DBlock.hasOutSteel)
                    {
                        hasOutSteel = true;
                    }
                    B3DB.Add(bolts3DBlock);
                    //Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                }
            }
            if (hasOutSteel)
            {
                ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            }
            else
            {
                ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;

            }
            foreach (Bolts3DBlock item in B3DB)
            {
                BlockReference referenceBolts = Add2DHole(item);//加入孔位到2D
            }
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
                    //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                    PieceListGridControl.SelectItem(selectIndex);
                    PieceListGridControl.View.FocusedRowHandle = selectIndex;
                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{item.Type}.inp");
                    //cbx_SectionTypeComboBox.Text = item.Profile;
                    //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                    if (item == null || selectIndex == -1)
                    {
                        fclickOK = false;
                        return;
                    }
                    //////////                    ConfirmCurrentSteelSection(item);
                    ProductSettingsPageViewModel SelectRow = RowToEntity(item);
                    //ProductSettingsPageViewModel FinalRow = new ProductSettingsPageViewModel();
                    STDSerialization ser = new STDSerialization();
                    DataCorrespond = ser.GetDataCorrespond();

                    string focuseGUID = item.DataName;

                    #region 最後一列之處理
                    // 檢查最後一筆之guid是否存在dev_Part中
                    // 不存在，詢問是否存檔
                    // 　　　　存檔，將指標移至最後一筆並更新畫面資料及SaveModel，讀取最後一列
                    // 　　　　不存檔，刪除最後一列，清空畫面資訊，讀取原選列
                    if (this.PieceListGridControl.VisibleRowCount > 0)
                    {
                        ProductSettingsPageViewModel FinalRow = (ProductSettingsPageViewModel)this.PieceListGridControl.GetRow(this.PieceListGridControl.VisibleRowCount - 1);
                        ProductSettingsPageViewModel temp = RowToEntity(FinalRow);
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
                            FloatingMode.Popup);

                            if (ResultRtn == MessageBoxResult.Yes)
                            {
                                // 指向最後一列
                                this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
                                PieceListGridControl.View.FocusedRowHandle = this.PieceListGridControl.VisibleRowCount - 1;
                                //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);

                                // 還原元件資訊
                                RowToView(FinalRow);
                                ConfirmCurrentSteelSection(FinalRow);

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
                                //               FloatingMode.Popup);
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
                                //            FloatingMode.Popup);
                                //        return;
                                //    }
                                //}
                                // 新零件
                                SaveModel(true, true);
                                //await SaveModelAsync(true, true);
                            }
                            else
                            {
                                #region 還原零件清單
                                if (this.PieceListGridControl.VisibleRowCount > 0)
                                {
                                    //刪除最後一列明細
                                    ObservableCollection<ProductSettingsPageViewModel> source = (ObservableCollection<ProductSettingsPageViewModel>)PieceListGridControl.ItemsSource;
                                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                                    source.Remove(source.Where(x => x.DataName == guid.ToString()).FirstOrDefault());
                                    //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                                    PieceListGridControl.ItemsSource = source;
                                    PieceListGridControl.SelectItem(this.PieceListGridControl.VisibleRowCount - 1);
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
                            PieceListGridControl.SelectItem(selectIndex);
                            PieceListGridControl.View.FocusedRowHandle = selectIndex;
                            //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                            //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{item.Type}.inp");
                            cbx_SectionTypeComboBox.Text = item.Profile;
                            //ViewModel.SteelSectionProperty = item.Profile;
                            this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                        }
                    }
                    #endregion

                    fclickOK = true;
                    // 既有零件
                    fFirstAdd = true;
                    fNewPart = false;
                    fGrid = true;
                    StateParaSetting(true, false, true);



                    model.Blocks.Clear();
                    model.Entities.Clear();
                    drawing.Blocks.Clear();
                    drawing.Entities.Clear();


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
                            FloatingMode.Popup);
                        return;
                    }
                    readFile.DoWork();//開始工作
                    //model.SetCurrent(null);
                    //drawing.SetCurrent(null);
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

                        WinUIMessageBox.Show(null,
                           $"專案Dev_Part資料夾讀取失敗",
                           "通知",
                           MessageBoxButton.OK,
                           MessageBoxImage.Exclamation,
                           MessageBoxResult.None,
                           MessageBoxOptions.None,
                           FloatingMode.Popup);
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
                            FloatingMode.Popup);
                        return;
                    }

                    //////////                    
                    ConfirmCurrentSteelSection(item);

                    //SteelAttr sa = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                    //SteelAttr sa = ViewModel.SteelAttr;//(SteelAttr)model.Blocks[1].Entities[0].EntityData;//1110 暫時註解掉，避免 e.OldItem, e.NewItem 間同時指向VM層連動導致(因為有binding到)e.OldItem資料被變更 CYH
                    SteelAttr sa = item.steelAttr;//1110 改由e.NewItem的steelAttr給值 CYH
                    //SteelAttr sa = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                    SteelAttr saTemp = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                    model.Blocks[1].Entities[0].EntityData = sa;

                    // 1.nc檔上的圖塊為一個孔為一個孔群
                    // 2.若NC檔上有5個孔 在無修改狀態下，須為五個孔
                    // 3.若已修改成3個孔，儘管nc檔讀出為五個孔，顯示仍須為3個孔
                    // 4.Block為圖塊(model.Blocks孔群的資本資訊 model.Entities單顆孔的資訊)
                    // 5.Entities為實體(每一個孔的位置)
                    // 6.因為nc檔上的某些屬性雖然使用過代理(Surrogate)寫法，但仍存不進去檔案內，故每次仍須重讀nc檔

                    // 步驟1.取得舊有dm檔上的圖塊
                    // 舊有形鋼上的圖塊
                    //List<GroupBoltsAttr> modelAllBoltList = model.Entities.Where(x => x.EntityData.GetType() == typeof(GroupBoltsAttr)).Select(x => (GroupBoltsAttr)x.EntityData).Where(x=>x.Mode== AXIS_MODE.PIERCE).ToList();//孔群
                    //List<Block> oldBoltBlock = model.Blocks.Where(x => (modelAllBoltList.Select(z => z.GUID.ToString())).Contains(x.Name)).ToList();
                    // 步驟2.讀取nc檔，紀錄opoint.upoint.vpoint

                    SteelAttr saDeepClone = (SteelAttr)sa.DeepClone();
                    List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
                    sa = sr.ReadNCInfo(saDeepClone, ref groups, false);
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.vPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.uPoint;
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;

                    // 舊有形鋼上的孔群:目前有兩種
                    List<Block> blocks = model.GetBoltFromBlock(groups);

                    //sr.AddBolts(model, drawing, out bool checkRef, blocks);


                    ////ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
                    ////model.Blocks[1].ConvertToSurrogate();
                    //string path = ApplicationVM.DirectoryNc();
                    //string allPath = path + $"\\{sa.PartNumber}.nc1";
                    //var profile = ser.GetSteelAttr();
                    //TeklaNcFactory t = new TeklaNcFactory();
                    //Steel3DBlock s3Db = new Steel3DBlock();
                    //SteelAttr steelAttrNC = new SteelAttr();
                    //SteelAttr saT = new SteelAttr() { Profile = sa.Profile, Type = sa.Type, t1 = sa.t1, t2 = sa.t2, H = sa.H, W = sa.W };

                    //List<Bolts3DBlock> b3d = model.Blocks.Where(x => x.GetType() == typeof(Bolts3DBlock)).Select(x => (Bolts3DBlock)x).ToList();
                    //// groups NC上的孔
                    //s3Db.ReadNcFile($@"{allPath}", profile, saT, ref steelAttrNC, ref groups);
                    //if (File.Exists(allPath))
                    //{
                    //    //sa.GUID = sa.GUID;
                    //    sa.oPoint = steelAttrNC.oPoint;
                    //    sa.vPoint = steelAttrNC.vPoint;
                    //    sa.uPoint = steelAttrNC.uPoint;
                    //    sa.CutList = steelAttrNC.CutList;
                    //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
                    //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.vPoint;
                    //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.uPoint;
                    //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;
                    //}
                    //// 步驟. 若有舊有孔圖塊，則取代nc檔中的孔
                    //// 如果型鋼上無孔，則取NC檔中的孔
                    //if (!modelAllBoltList.Any())
                    //{
                    //    modelAllBoltList = groups;
                    //}
                    // 步驟3.產生鋼構模型
                    model.LoadNcToModel(focuseGUID, ObSettingVM.allowType, 0, null, sa, null, blocks, false);
                    // 步驟5.產生2D模型
                    BlockReference steel2D = sr.SteelTriangulation(drawing, model.Blocks[1].Name, (Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                    model.sycnModelEntitiesAndNewBolt(blocks);
                    sr.AddBolts(model, drawing, out bool hasOutSteel, blocks, false);




                    //else { sa = steelAttrNC; }



                    //ViewModel.WriteSteelAttr(sa);//寫入到設定檔內
                    //ViewModel.GetSteelAttr();
                    ////////////                    GetViewToViewModel(false, sa.GUID);
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
                    //cbx_SectionTypeComboBox.Text = sa.Profile;
                    //this.cbx_SectionTypeComboBox.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);
                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(sa.Type).ToString()}.inp");
                    //cbx_SectionTypeComboBox.Text = profile;
                    //this.cbx_SectionTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CBOX_SectionTypeChanged);

                    ////////////Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);
                    //////////Steel3DBlock result = new Steel3DBlock(Steel3DBlock.GetProfile((SteelAttr)model.Blocks[1].Entities[0].EntityData));
                    //////////if (model.Blocks.Count > 1)
                    //////////{
                    //////////    model.Blocks.Remove(model.Blocks[1]);
                    //////////}
                    //////////model.Blocks.Insert(1, result);//加入鋼構圖塊到模型
                    //////////BlockReference blockReference = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);
                    //////////blockReference.EntityData = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
                    //////////blockReference.Selectable = false;//關閉用戶選擇
                    //////////blockReference.Attributes.Add("steel", new AttributeReference(0, 0, 0));
                    ////////////if (model.Entities.Count > 0)
                    ////////////{
                    ////////////    model.Entities.RemoveAt(model.Entities.Count - 1);
                    ////////////}
                    //////////model.Entities.Insert(model.Entities.Count, blockReference);//加入參考圖塊到模型
                    //////////model.Entities.Regen();
                    //////////drawing.Blocks.Clear();
                    //////////drawing.Entities.Clear();
                    //model.LoadNcToModel(focuseGUID, ObSettingVM.allowType, 0, null, sa, modelAllBoltList, blocks, false);
                    //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                    //ScrollViewbox.IsEnabled = true;
                    //ManHypotenusePoint(FACE.FRONT);
                    //ManHypotenusePoint(FACE.BACK);
                    //ManHypotenusePoint(FACE.TOP);

                    //if (model.RunHypotenuseEnable())
                    //{
                    //RunHypotenuseEnable();
                    // false表示沒有斜邊 可使用切割線
                    ScrollViewbox.IsEnabled = !model.RunHypotenuseEnable();
                    //}
                    //AutoHypotenuseEnable(FACE.TOP);
                    //AutoHypotenuseEnable(FACE.FRONT);
                    //AutoHypotenuseEnable(FACE.BACK);                    
                    //SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                    //sr.AddBolts(model, drawing, modelAllBoltList, ref hasOutSteel);

                    //model.Entities.RemoveRange(0, model.Entities.Count - 1);

                    //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                    //{
                    //if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                    //{
                    //BlockReference blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
                    //Block block = model.Blocks[blockReference1.BlockName]; //取得圖塊
                    ////Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);
                    //Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference1.EntityData); //產生螺栓圖塊                            
                    //Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖                        
                    //}
                    //}
                    //var HPoint = blocks.SelectMany(x => x.Entities).Where(x => x.GetType() == typeof(GroupBoltsAttr) && ((GroupBoltsAttr)x.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
                    //    .Select(x => (Mesh)x).ToList();
                    //int Hindex = 0;
                    //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                    //{
                    //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                    //    {
                    //        BlockReference blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
                    //        int index = blocks.FindIndex(x => x.Name == blockReference1.BlockName);
                    //        if (index!=-1)// -1 斜邊打點
                    //        {
                    //            Block block = blocks[index]; //取得圖塊
                    //            Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference1.EntityData); //產生螺栓圖塊                            
                    //            Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖          
                    //        }
                    //        else
                    //        {
                    //            double X = ((GroupBoltsAttr)model.Entities[i].EntityData).X;
                    //            double Y = ((GroupBoltsAttr)model.Entities[i].EntityData).Y;
                    //            double Z = ((GroupBoltsAttr)model.Entities[i].EntityData).Z;
                    //            blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
                    //            Block a = new Block();
                    //            a.Entities.AddRange(blocks.SelectMany(x => x.Entities).Where(x => x.EntityData.GetType() == typeof(GroupBoltsAttr) &&
                    //            ((GroupBoltsAttr)x.EntityData).Mode == AXIS_MODE.HypotenusePOINT && 
                    //            ((GroupBoltsAttr)x.EntityData).X == X &&
                    //            ((GroupBoltsAttr)x.EntityData).Y == Y && 
                    //            ((GroupBoltsAttr)x.EntityData).Z == Z).ToList());
                    //            Bolts3DBlock bolts3DBlock = new Bolts3DBlock(a.Entities, (GroupBoltsAttr)blockReference1.EntityData); //產生螺栓圖塊
                    //            Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖 
                    //        }
                    //    }
                    //}

                    //sr.RemoveHypotenusePoint(model);
                    //WPFSTD105.Model.Expand.RunHypotenusePoint(model,new ObSettingVM(), 0);


                    //if (meshes != null)
                    //{
                    //    //model.Entities.RemoveRange(0, model.Entities.Count - 1);
                    //    //foreach (var b in meshes)
                    //    //{
                    //    //    model.Entities.Insert(1, b);

                    //    //}
                    //    //foreach (var item1 in b3d)
                    //    //{
                    //    //    Add2DHole(item1, false);//加入孔位不刷新 2d 視圖       
                    //    //}
                    //}




                    //Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts(bolt, model, out BlockReference blockRef, out bool checkRef);
                    //if (bolts3DBlock.hasOutSteel)
                    //{
                    //    hasOutSteel = true;
                    //}
                    //    B3DB.Add(bolts3DBlock);

                    //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                    //{
                    //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                    //    {
                    //        blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    //        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                    //        Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out bool checkRef);

                    //        if (bolts3DBlock.hasOutSteel)
                    //        {
                    //            hasOutSteel = true;
                    //        }
                    //         B3DB.Add(bolts3DBlock); 
                    //    }
                    //}
                    if (!Bolts3DBlock.CheckBolts(model, false))
                    {
                        //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                        item.steelAttr.ExclamationMark = true;
                        item.ExclamationMark = true;
                        PieceListGridControl.RefreshRow(PieceListGridControl.View.FocusedRowHandle);
                    }
                    else
                    {
                        //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false;
                        item.steelAttr.ExclamationMark = false;
                        item.ExclamationMark = false;
                        PieceListGridControl.RefreshRow(PieceListGridControl.View.FocusedRowHandle);
                    }

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
                    //////////                    ConfirmCurrentSteelSection(item);
                    //////////                    GetViewToViewModel(false, Guid.Parse(focuseGUID));
                    // 執行斜邊打點
                    //ManHypotenusePoint((FACE)ViewModel.rbtn_CutFace);


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
                    item.steelAttr.GUID = Guid.Parse(item.DataName);
                    //Grid_SelectedChange(sender, e);


                    if (item == null)
                    {
                        fclickOK = true;
                        return;
                    }

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
                    FloatingMode.Popup);
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
                BlockReference steel2D = SteelTriangulation((Mesh)steel.Entities.Where(x => x.GetType().Name == "Mesh").FirstOrDefault());
            }
            if (model.Blocks.Count == 1)
            {
                ViewModel.SteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                Steel3DBlock steel = Steel3DBlock.AddSteel(ViewModel.GetSteelAttr(), model, out blockReference);
                ((SteelAttr)model.Entities[0].EntityData).GUID = Guid.Parse(item.DataName);
                BlockReference steel2D = SteelTriangulation((Mesh)steel.Entities.Where(x => x.GetType().Name == "Mesh").FirstOrDefault());
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
            sa = sr.ReadNCInfo(saDeepClone, ref groups, false);
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = sa.oPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = sa.vPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = sa.uPoint;
            ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = sa.CutList;

            List<Block> blocks = model.GetBoltFromBlock(groups);
            //string path = ApplicationVM.DirectoryNc();
            //string allPath = path + $"\\{sa.PartNumber}.nc1";
            //var profile = ser.GetSteelAttr();
            //TeklaNcFactory t = new TeklaNcFactory();
            //Steel3DBlock s3Db = new Steel3DBlock();
            //SteelAttr steelAttrNC = new SteelAttr();
            //SteelAttr saT = new SteelAttr() { Profile = sa.Profile, Type = sa.Type, t1 = sa.t1, t2 = sa.t2, H = sa.H, W = sa.W };

            //s3Db.ReadNcFile($@"{allPath}", profile, saT, ref steelAttrNC, ref groups);
            //if (File.Exists(allPath))
            //{
            //    //sa.GUID = sa.GUID;
            //    sa.oPoint = steelAttrNC.oPoint;
            //    sa.vPoint = steelAttrNC.vPoint;
            //    sa.uPoint = steelAttrNC.uPoint;
            //    sa.CutList = steelAttrNC.CutList;
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).oPoint = steelAttrNC.oPoint;
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).uPoint = steelAttrNC.vPoint;
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).vPoint = steelAttrNC.uPoint;
            //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).CutList = steelAttrNC.CutList;
            //}

            #region 建模
            // 建立型鋼
            Steel3DBlock result = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);
            model.AddModelSteelAttr(sa, result);
            // 建立2D模型
            sr.SteelTriangulation(drawing, result.Name, (Mesh)model.Blocks[1].Entities[0]);
            // 建立2D/3D孔
            sr.AddBolts(model, drawing, out bool hasOutSteel, blocks);
            ManHypotenusePoint(FACE.TOP);
            ManHypotenusePoint(FACE.FRONT);
            ManHypotenusePoint(FACE.BACK);
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
        /// 流程參數設定(全null為初始值)
        /// </summary>
        /// <param name="firstAdd">是否第一次按新增</param>
        /// <param name="newPart">是否為新零件</param>
        /// <param name="grid">是否從Grid開始動作</param>
        private void StateParaSetting(bool? firstAdd, bool? newPart, bool? grid)
        {
            // 初始值
            if (firstAdd == null && newPart == null && grid == null)
            {
                fFirstAdd = true;
                fNewPart = true;
                fGrid = false;
            }
            else
            {
                // 第一次按新增
                fFirstAdd = firstAdd;
                // 是否為新零件
                fNewPart = newPart;
                // 是否在Grid進行動作
                fGrid = grid;
            }
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

                ViewModel.DataViews = collection;
                PreIndex = collection.FindIndex(x => x.DataName == aa.DataName);
                if (PreIndex != -1)
                {
                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    PieceListGridControl.ItemsSource = collection;
                    PieceListGridControl.RefreshData();
                    var rowHandle = PieceListGridControl.GetRowHandleByVisibleIndex(PreIndex);
                    PieceListGridControl.View.FocusedRowHandle = rowHandle;
                    PieceListGridControl.SelectItem(rowHandle);
                    //ViewModel.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(collection[rowHandle].Type).ToString()}.inp");
                    cbx_SectionTypeComboBox.Text = collection[rowHandle].Profile;
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                }
            }
            else
            {
                //ObservableCollection<ProductSettingsPageViewModel> collection = new ObservableCollection<ProductSettingsPageViewModel>(ObSettingVM.GetData());
                //ViewModel.DataViews = collection;
                ObservableCollection<ProductSettingsPageViewModel> collection = ViewModel.DataViews;
                if (collection.Count > 0)
                {
                    this.PieceListGridControl.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    PieceListGridControl.ItemsSource = collection;
                    PieceListGridControl.RefreshData();
                    this.PieceListGridControl.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(this.Grid_SelectedChange);
                    this.PieceListGridControl.SelectItem(0);
                    PieceListGridControl.View.FocusedRowHandle = 0;
                    aa = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;
                    ConfirmCurrentSteelSection(aa);
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
            //             FloatingMode.Popup);
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
            //             FloatingMode.Popup);


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
                        FloatingMode.Popup);

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



    }
}