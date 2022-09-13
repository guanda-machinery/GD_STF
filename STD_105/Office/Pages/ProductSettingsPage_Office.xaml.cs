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

namespace STD_105.Office
{
    public partial class ProductSettingsPage_Office : BasePage<ObSettingVM>  
    {
        public ObSettingVM sr = new ObSettingVM();

        public ObservableCollection<DataCorrespond> DataCorrespond { get; set; } = new ObservableCollection<DataCorrespond>();
        /// <summary>
        /// 20220823 蘇冠綸 製品設定
        /// </summary>
        public ProductSettingsPage_Office()
        {
            InitializeComponent();
            //2022.06.24 呂宗霖 此Class與GraphWin.xaml.cs皆有SteelTriangulation與Add2DHole
            //                  先使用本Class 若有問題再修改
            //GraphWin service = new GraphWin();

        
            
         




            /// <summary>
            /// 鑽孔radio button測試 20220906 張燕華
            /// </summary>
            ViewModel.CmdShowMessage = new RelayCommand(() =>
            {
                System.Windows.MessageBox.Show("You Select " + ViewModel.rbtn_DrillingFace.ToString());
            });

            #region 3D
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            //model.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            model.Secondary = drawing;
            #endregion
            #region 2D
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
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
            //刪除物件
            ViewModel.Delete = new RelayCommand(() =>
            {
                SimulationDelete();
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
                #region 3D 
                ViewModel.SteelAttr.PartNumber = this.partNumber.Text;
                if (!CheckPart()) //檢測用戶輸入的參數是否有完整
                    return;
                model.Entities.Clear();//清除模型物件
                model.Blocks.Clear(); //清除模型圖塊

                GetViewToVM();

                //ViewModel.SteelAttr.GUID = Guid.NewGuid();//產生新的 id
                //ViewModel.SteelAttr.Creation = DateTime.Now;
                //ViewModel.SteelAttr.Revise = null;

                //// 2022/07/14 呂宗霖 guid2區分2d或3d
                ////ViewModel.SteelAttr.GUID2 = ViewModel.SteelAttr.GUID;
                //ViewModel.SteelAttr.PointFront = new CutList();//清除切割線
                //ViewModel.SteelAttr.PointTop = new CutList();//清除切割線
                //ViewModel.SteelAttr.AsseNumber = this.asseNumber.Text;
                //ViewModel.SteelAttr.Length = string.IsNullOrEmpty(this.Length.Text) ? 0 : Double.Parse(this.Length.Text);
                //ViewModel.SteelAttr.Weight = string.IsNullOrEmpty(this.Weight.Text) ? 0 : double.Parse(this.Weight.Text);
                //ViewModel.SteelAttr.Name = this.teklaName.Text;
                //ViewModel.SteelAttr.Material = this.material.Text;
                //ViewModel.SteelAttr.Phase = string.IsNullOrEmpty(this.phase.Text) ? 0 : int.Parse(this.phase.Text);
                //ViewModel.SteelAttr.ShippingNumber = string.IsNullOrEmpty(this.shippingNumber.Text) ? 0 : int.Parse(this.shippingNumber.Text);
                //ViewModel.SteelAttr.Title1 = this.title1.Text;
                //ViewModel.SteelAttr.Title2 = this.title2.Text;
                //ViewModel.SteelAttr.Type = (OBJECT_TYPE)this.cbx_SteelType.SelectedIndex;
                //ViewModel.SteelAttr.Profile = this.cbx_SectionType.Text;
                //ViewModel.SteelAttr.H = string.IsNullOrEmpty(this.H.Text) ? 0 : float.Parse(this.H.Text);
                //ViewModel.SteelAttr.W = string.IsNullOrEmpty(this.W.Text) ? 0 : float.Parse(this.W.Text);
                //ViewModel.SteelAttr.Number = string.IsNullOrEmpty(this.PartCount.Text) ? 0 : int.Parse(this.PartCount.Text);
                //ViewModel.SteelAttr.t1 = string.IsNullOrEmpty(this.t1.Text) ? 0 : float.Parse(this.t1.Text);
                //ViewModel.SteelAttr.t2 = string.IsNullOrEmpty(this.t2.Text) ? 0 : float.Parse(this.t2.Text);
                //ViewModel.SteelAttr.ExclamationMark = false;
#if DEBUG
                log4net.LogManager.GetLogger("加入主件").Debug("產生圖塊");
#endif
               


                #region 已改為AddSteel,故註解
                //SteelAttr steelAttr = ViewModel.GetSteelAttr();
                //Steel3DBlock steel = new Steel3DBlock(Steel3DBlock.GetProfile(steelAttr)); //產生鋼構圖塊
                //model.Blocks.Add(steel);//加入鋼構圖塊到模型
                //BlockReference blockReference = new BlockReference(0, 0, 0, steel.Name, 1, 1, 1, 0);//產生鋼構參考圖塊
                //blockReference.EntityData = steelAttr;
                //blockReference.Selectable = false;//關閉用戶選擇
                //blockReference.Attributes.Add("Steel", new AttributeReference(0, 0, 0));
                //model.Entities.Add(blockReference);//加入參考圖塊到模型 
                #endregion

                Steel3DBlock steel = Steel3DBlock.AddSteel(ViewModel.GetSteelAttr(), model, out BlockReference blockReference);
                BlockReference steel2D = SteelTriangulation((Mesh)steel.Entities[0]);
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
                fAddPartAndBolt = true;
                model.ZoomFit();//設置道適合的視口
                model.Refresh();//刷新模型
                //drawing.ZoomFit();
                //drawing.Refresh();
                SaveModel(true);

                //ObSettingVM sr = new ObSettingVM();
                ObservableCollection<ProductSettingsPageViewModel> collection = new ObservableCollection<ProductSettingsPageViewModel>(sr.GetData());

                ViewModel.DataViews = collection;
                // var A = PieceListGridControl.ItemsSource;
                PieceListGridControl.ItemsSource = collection;
                //PieceListGridControl.EndSelection();    
                //PieceListGridControl.Dispatcher.Invoke(() =>
                //{

                //    PieceListGridControl.RefreshData();
                //    PieceListGridControl.EndDataUpdate();
                //});
                #endregion
            });
            //修改主零件
            ViewModel.ModifyPart = new RelayCommand(() =>
            {
                if (!CheckPart()) //檢測用戶輸入的參數是否有完整
                    return;
                if (model.CurrentBlockReference != null)
                {
                    WinUIMessageBox.Show(null,
                    $"退出編輯模式，才可修改主件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                    return;
                }
#if DEBUG
                log4net.LogManager.GetLogger("修改主件").Debug("開始");
#endif

                SelectedItem sele3D = new SelectedItem(model.Entities[model.Entities.Count - 1]);
                SelectedItem sele2D = new SelectedItem(drawing.Entities[drawing.Entities.Count - 1]);

                BlockReference reference3D = (BlockReference)sele3D.Item;
                BlockReference reference2D = (BlockReference)sele2D.Item;

                //模擬用戶實際選擇編輯
                ViewModel.Select3DItem.Add(sele3D);
                ViewModel.Select2DItem.Add(sele2D);

                //層級 To 要編輯的BlockReference
                model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);
                drawing.SetCurrent((BlockReference)drawing.Entities[0]);
                ////0→     drawing.Entities.Count - 1
                //drawing.SetCurrent((BlockReference)drawing.Entities[drawing.Entities.Count - 1]);
                
                SteelAttr steelAttr = ViewModel.GetSteelAttr();
                steelAttr = GetViewToAttr(steelAttr, model);
                //steelAttr.Length = string.IsNullOrEmpty(this.Length.Text) ? 0 : double.Parse(this.Length.Text);
                //steelAttr.Phase = string.IsNullOrEmpty(this.phase.Text) ? 0 : int.Parse(this.phase.Text);
                //steelAttr.Name = this.teklaName.Text;
                //steelAttr.ShippingNumber = string.IsNullOrEmpty(this.shippingNumber.Text) ? 0 : int.Parse(this.shippingNumber.Text);
                //steelAttr.Title1 = this.title1.Text;
                //steelAttr.Title2 = this.title2.Text;
                //steelAttr.Type = (OBJECT_TYPE)this.cbx_SteelType.SelectedIndex;
                //steelAttr.Profile=this.cbx_SectionType.Text;

                //steelAttr.GUID = ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).GUID;//修改唯一識別ID
                ViewModel.SteelAttr.Length = steelAttr.Length;
                ViewModel.SteelAttr.Phase = steelAttr.Phase;
                ViewModel.SteelAttr.Number = steelAttr.Number;
                ViewModel.SteelAttr.Profile = steelAttr.Profile;
                ViewModel.SteelAttr.Material = steelAttr.Material;
                ViewModel.SteelAttr.Name = steelAttr.Name;
                ViewModel.SteelAttr.Weight = steelAttr.Weight;
                ViewModel.SteelAttr.H = steelAttr.H;
                ViewModel.SteelAttr.W = steelAttr.W;
                ViewModel.SteelAttr.t1 = steelAttr.t1;
                ViewModel.SteelAttr.t2 = steelAttr.t2;


                Mesh modify = Steel3DBlock.GetProfile(steelAttr); //修改的形狀
                ViewModel.tem3DRecycle.Add(model.Entities[model.Entities.Count - 1]);//加入垃圾桶準備刪除
                ViewModel.tem2DRecycle.AddRange(drawing.Entities);//加入垃圾桶準備刪除


                //model.Entities[0].Selected = true;//選擇物件
                drawing.Entities.ForEach(el => el.Selected = true);
                List<Entity> steel2D = new Steel2DBlock(modify, "123").Entities.ToList();
                Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[reference2D.BlockName];//drawing.CurrentBlockReference.BlockName→1
                steel2DBlock.ChangeMesh(modify);
                //加入到垃圾桶內
                
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
                //刪除指定物件
                model.Entities.RemoveAt(0);
                drawing.Entities.Clear();
                //清空選擇物件
                ViewModel.Select2DItem.Clear();
                ViewModel.Select3DItem.Clear();
                //清空圖塊內物件
                ViewModel.tem3DRecycle.Clear();
                ViewModel.tem2DRecycle.Clear();

                ViewModel.Reductions.AddContinuous(new List<Entity>() { modify }, steel2D);

                model.Entities.Insert(0, modify);//加入參考圖塊到模型
                drawing.Entities.AddRange(steel2D);
                Esc();
                //刷新模型
                model.Invalidate();
                drawing.Invalidate();

                if (!fAddPartAndBolt)
                    SaveModel(false);//存取檔案

                // 執行斜邊打點
                //HypotenusePoint(FACE.TOP);
                //HypotenusePoint(FACE.BACK);
                //HypotenusePoint(FACE.FRONT);

                //AddHypotenusePoint(FACE.TOP);

                

                
#if DEBUG
                log4net.LogManager.GetLogger("修改主件").Debug("結束");
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
                    FloatingMode.Popup);
                }
                //如果模型裡面有物件
                else if (model.Entities.Count > 0)
                {
                    model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);  // 取得主件資訊
                    ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData); // 寫入主件設定檔 To VM
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
                    FloatingMode.Popup);
                }
            });
            //另存加入零件 20220902 張燕華
            ViewModel.AddNewOne = new RelayCommand(() =>
            {
                //在此撰寫程式碼..
            }); 
            //OK鈕 20220902 張燕華
            ViewModel.OKtoConfirmChanges = new RelayCommand(() =>
            {
                //在此撰寫程式碼..
            });
            //加入孔
            ViewModel.AddHole = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("加入孔位").Debug("產生圖塊");
#endif
                if (model.Entities.Count <= 0)
                {
                    WinUIMessageBox.Show(null,
                    $"模型內找不到主件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                    return;
                }


                if (ComparisonBolts())  // 欲新增孔位重複比對
                {
                    WinUIMessageBox.Show(null,
                    $"新增孔位重複，請重新輸入",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                    return;
                }

                /*3D螺栓*/
                ViewModel.GroupBoltsAttr.GUID = Guid.NewGuid();


                Bolts3DBlock bolts = Bolts3DBlock.AddBolts(ViewModel.GetGroupBoltsAttr(), model, out BlockReference blockReference, out bool check);
                BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                if (check)
                {
                    SaveModel(false);//存取檔案

                    //不是修改孔位狀態
                    if (!modifyHole)
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
                    //刷新模型
                    model.Refresh();
                    drawing.Refresh();
                    SaveModel(false);//存取檔案
                }
                else
                {
                    WinUIMessageBox.Show(null,
                                 $"孔群落入非加工區域，請再確認",
                                 "通知",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation,
                                 MessageBoxResult.None,
                                 MessageBoxOptions.None,
                                 FloatingMode.Popup);
                    return;
                }
                //刷新模型
                model.Refresh();
                drawing.Refresh();
                if (!fAddPartAndBolt)
                    SaveModel(false);//存取檔案

            });
            //修改孔
            ViewModel.ModifyHole = new RelayCommand(() =>
            {
                //查看用戶是否有選擇圖塊
                if (ViewModel.Select3DItem.Count > 0)
                {
                    List<SelectedItem> selectItem = ViewModel.Select3DItem.ToList();//暫存容器
                    GroupBoltsAttr original = (GroupBoltsAttr)ViewModel.GroupBoltsAttr.DeepClone(); //原有設定檔
                    selectItem.ForEach(el => el.Item.Selected = false);//取消選取
                    for (int i = 0; i < selectItem.Count; i++)
                    {

                        BlockReference blockReference = (BlockReference)selectItem[i].Item;
                        //如果在編輯模式
                        if (model.CurrentBlockReference != null)
                        {
                            WinUIMessageBox.Show(null,
                                $"出編輯模式，才可修改孔",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                FloatingMode.Popup);
                            return;
                        }
                        //如果選擇的物件不是孔位
                        else if (blockReference == null || blockReference.EntityData.Equals(typeof(GroupBoltsAttr)))
                        {
                            WinUIMessageBox.Show(null,
                                $"選擇類型必須是孔，才可修改",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                FloatingMode.Popup);
                            WinUIMessageBox.Show(null,
                                $"",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                FloatingMode.Popup);

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
                        modifyHole = true;
                        ViewModel.AddHole.Execute(null);
                    }
                    Esc();
                    modifyHole = false;
                    ViewModel.GroupBoltsAttr = original;
                    model.Invalidate();//刷新模型
                    if (!fAddPartAndBolt)
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
                    FloatingMode.Popup);
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
                            FloatingMode.Popup);
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
                            FloatingMode.Popup);
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
                        FloatingMode.Popup);
                }
            });
            //加入或修改切割線
            ViewModel.AddCut = new RelayCommand(() =>
            {
#if DEBUG
                log4net.LogManager.GetLogger("加入切割線").Debug("開始");
#endif
                ViewModel.ReadPart.Execute(null);
                ViewModel.SaveCut();
                ViewModel.ModifyPart.Execute(null);

                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D三視圖
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
                    FloatingMode.Popup);
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
                    FloatingMode.Popup);
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
                                    FloatingMode.Popup);
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
                                    FloatingMode.Popup);
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
                            FloatingMode.Popup);
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
                                    FloatingMode.Popup);
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
                                    FloatingMode.Popup);
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
                            FloatingMode.Popup);
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
                //開啟Model焦點
                bool mFocus = model.Focus();
                if (!mFocus)
                {
                    drawing.Focus();
                }
                SimulationDelete();
                if (!fAddPartAndBolt)
                    SaveModel(false);
            });
            #endregion

            model.Invalidate();
            drawing.Invalidate();
        }

        public void GetViewToVM() 
        {
            ViewModel.SteelAttr.GUID = Guid.NewGuid();//產生新的 id
            ViewModel.SteelAttr.Creation = DateTime.Now;
            ViewModel.SteelAttr.Revise = null;

            // 2022/07/14 呂宗霖 guid2區分2d或3d
            //ViewModel.SteelAttr.GUID2 = ViewModel.SteelAttr.GUID;
            ViewModel.SteelAttr.PointFront = new CutList();//清除切割線
            ViewModel.SteelAttr.PointTop = new CutList();//清除切割線
            ViewModel.SteelAttr.AsseNumber = this.asseNumber.Text;
            ViewModel.SteelAttr.Length = string.IsNullOrEmpty(this.Length.Text) ? 0 : Double.Parse(this.Length.Text);
            ViewModel.SteelAttr.Weight = string.IsNullOrEmpty(this.Weight.Text) ? 0 : double.Parse(this.Weight.Text);
            ViewModel.SteelAttr.Name = this.teklaName.Text;
            ViewModel.SteelAttr.Material = this.material.Text;
            ViewModel.SteelAttr.Phase = string.IsNullOrEmpty(this.phase.Text) ? 0 : int.Parse(this.phase.Text);
            ViewModel.SteelAttr.ShippingNumber = string.IsNullOrEmpty(this.shippingNumber.Text) ? 0 : int.Parse(this.shippingNumber.Text);
            ViewModel.SteelAttr.Title1 = this.title1.Text;
            ViewModel.SteelAttr.Title2 = this.title2.Text;
            ViewModel.SteelAttr.Type = (OBJECT_TYPE)this.cbx_SteelType.SelectedIndex;
            ViewModel.SteelAttr.Profile = this.cbx_SectionType.Text;
            ViewModel.SteelAttr.H = string.IsNullOrEmpty(this.H.Text) ? 0 : float.Parse(this.H.Text);
            ViewModel.SteelAttr.W = string.IsNullOrEmpty(this.W.Text) ? 0 : float.Parse(this.W.Text);
            ViewModel.SteelAttr.Number = string.IsNullOrEmpty(this.PartCount.Text) ? 0 : int.Parse(this.PartCount.Text);
            ViewModel.SteelAttr.t1 = string.IsNullOrEmpty(this.t1.Text) ? 0 : float.Parse(this.t1.Text);
            ViewModel.SteelAttr.t2 = string.IsNullOrEmpty(this.t2.Text) ? 0 : float.Parse(this.t2.Text);
            ViewModel.SteelAttr.ExclamationMark = false;
        }

        /// <summary>
        /// 編輯塞值用
        /// </summary>
        /// <param name="steelAttr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public SteelAttr GetViewToAttr(SteelAttr steelAttr, ModelExt model)
        {
            steelAttr.Creation = DateTime.Now;
            steelAttr.Revise = null;
            steelAttr.PointFront = new CutList();//清除切割線
            steelAttr.PointTop = new CutList();//清除切割線
            steelAttr.AsseNumber = this.asseNumber.Text;
            steelAttr.Length = string.IsNullOrEmpty(this.Length.Text) ? 0 : Double.Parse(this.Length.Text);
            steelAttr.Weight = string.IsNullOrEmpty(this.Weight.Text) ? 0 : double.Parse(this.Weight.Text);
            steelAttr.Name = this.teklaName.Text;
            steelAttr.Material = this.material.Text;
            steelAttr.Phase = string.IsNullOrEmpty(this.phase.Text) ? 0 : int.Parse(this.phase.Text);
            steelAttr.ShippingNumber = string.IsNullOrEmpty(this.shippingNumber.Text) ? 0 : int.Parse(this.shippingNumber.Text);
            steelAttr.Title1 = this.title1.Text;
            steelAttr.Title2 = this.title2.Text;
            steelAttr.Type = (OBJECT_TYPE)this.cbx_SteelType.SelectedIndex;
            steelAttr.Profile = this.cbx_SectionType.Text;
            steelAttr.H = (string.IsNullOrEmpty(this.H.Text) || float.Parse(this.H.Text) == 0) ? steelAttr.H : float.Parse(this.H.Text);
            steelAttr.W = (string.IsNullOrEmpty(this.W.Text) || float.Parse(this.W.Text) == 0) ? steelAttr.W : float.Parse(this.W.Text);
            steelAttr.Number = string.IsNullOrEmpty(this.PartCount.Text) ? 0 : int.Parse(this.PartCount.Text);
            steelAttr.t1 = (string.IsNullOrEmpty(this.t1.Text) || float.Parse(this.t1.Text) == 0) ? steelAttr.t1 : float.Parse(this.t1.Text);
            steelAttr.t2 = (string.IsNullOrEmpty(this.t2.Text) || float.Parse(this.t2.Text) == 0) ? steelAttr.t2 : float.Parse(this.t2.Text);
            steelAttr.ExclamationMark = false;
            steelAttr.GUID = ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).GUID;
            // 調整修改日期
            steelAttr.Revise = DateTime.Now;
            return steelAttr;
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
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.P)) //俯視圖
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + P");
#endif
                model.InitialView = viewType.Top;
                model.ZoomFit();//在視口控件的工作區中適合整個模型。
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.Z)) //退回
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Z");
#endif
                ViewModel.Reductions.Previous();
#if DEBUG
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Z 完成");
#endif
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.Y)) //退回
            {
#if DEBUG
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Y");
#endif
                ViewModel.Reductions.Next();//回到上一個動作
#if DEBUG
                log4net.LogManager.GetLogger("按下鍵盤").Debug("Ctrl + Y 完成");
#endif
            }
            // 2020/08/04 呂宗霖 因按Delete會造成無窮迴圈 跳不出去系統造成當掉 故先停用直接按Delete的動作
            //else if (Keyboard.IsKeyDown(Key.Delete))
            //{
            //SimulationDelete();
            //}
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
            if (ViewModel.SteelAttr.PartNumber == "" || ViewModel.SteelAttr.PartNumber == null)//檢測用戶是否有輸入零件編號
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
            if (ViewModel.SteelAttr.Length <= 0)//檢測用戶長度是否有大於0
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
            if (ViewModel.SteelAttr.Number <= 0) //檢測用戶是否零件數量大於0
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


        bool fAddPartAndBolt = false;       //  判斷執行新增零件及孔位
        bool fAddHypotenusePoint = false;   //  判斷執行斜邊打點
        List<Bolts3DBlock> lstBoltsCutPoint = new List<Bolts3DBlock>();
        
        /// <summary>
        /// 
        /// </summary>
        public void RunHypotenusePoint()
        {

            Viewbox.IsEnabled = true;

            // 由選取零件判斷三面是否為斜邊
            if (model.Entities[model.Entities.Count - 1].EntityData is null)
                return;


            // 斜邊自動執行程式
            SteelAttr TmpSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;


            if (TmpSteelAttr.vPoint.Count != 0)         //  頂麵斜邊
            {
                AutoHypotenusePoint(FACE.TOP);
            }
            if(TmpSteelAttr.uPoint.Count != 0)     //  前面斜邊
            {
                AutoHypotenusePoint(FACE.FRONT);
            }
            if (TmpSteelAttr.oPoint.Count != 0)    //  後面斜邊
            {
                AutoHypotenusePoint(FACE.BACK);
            }

        }


        /// <summary>
        /// 自動斜邊打點
        /// </summary>
        public void AutoHypotenusePoint(FACE face)
        {
            MyCs myCs=new MyCs();

            STDSerialization ser = new STDSerialization();
            ObservableCollection<SplitLineSettingClass> ReadSplitLineSettingData = ser.GetSplitLineData();//備份當前加工區域數值

            double PosRatioA = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].A);     //  腹板斜邊打點比列(短)
            double PosRatioB = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].B);     //  腹板斜邊打點比列(長)
            double PosRatioC = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].C);     //  翼板斜邊打點比列(短)
            double PosRatioD = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].D);     //  翼板斜邊打點比列(長)

            List<Point3D> tmplist1 = new List<Point3D>() { };
            Point3D PointUL1 = new Point3D() ;
            Point3D PointUR1 = new Point3D() ;
            Point3D PointDL1 = new Point3D() ;
            Point3D PointDR1 = new Point3D() ;
            Point3D PointUL2 = new Point3D() ;
            Point3D PointUR2 = new Point3D() ;
            Point3D PointDL2 = new Point3D() ;
            Point3D PointDR2 = new Point3D() ;
            Point3D TmpDL = new Point3D() ;
            Point3D TmpDR = new Point3D() ;
            Point3D TmpUL = new Point3D() ;
            Point3D TmpUR = new Point3D() ;

            if (model.Entities[model.Entities.Count - 1].EntityData is null)
                return;

            SteelAttr TmpSteelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;

            switch (face)
            {

                case FACE.BACK:

                    if (TmpSteelAttr.oPoint.Count == 0) return;

                    var tmp1 = TmpSteelAttr.oPoint.GroupBy(uu => uu.Y).Select(q => new
                    {
                        key = q.Key,
                        max = q.Max(x => x.X),
                        min = q.Min(f => f.X)
                    }).ToList();

                    if (tmp1[0].key > tmp1[1].key)
                    {
                        var swap = tmp1[0];
                        tmp1[0] = tmp1[1];
                        tmp1[1] = swap;
                    }
                   
                    TmpDL = new Point3D(tmp1[0].min, tmp1[0].key);
                    TmpDR = new Point3D(tmp1[0].max, tmp1[0].key);
                    TmpUL = new Point3D(tmp1[1].min, tmp1[1].key);
                    TmpUR = new Point3D(tmp1[1].max, tmp1[1].key);

                    if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X))
                    return;


                    if (TmpUL.X > TmpDL.X)
                    {
                        PointDL1 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioC, (TmpUL.Y - TmpDL.Y) * PosRatioC) + TmpDL;
                        PointDL2 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioD, (TmpUL.Y - TmpDL.Y) * PosRatioD) + TmpDL;
                        tmplist1.Add(PointDL1);
                        tmplist1.Add(PointDL2);
                    }
                    else if (TmpUL.X < TmpDL.X)
                    {
                        PointDR1 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioC, (TmpUR.Y - TmpDR.Y) * PosRatioC) + TmpDR;
                        PointDR2 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioD, (TmpUR.Y - TmpDR.Y) * PosRatioD) + TmpDR;
                        tmplist1.Add(PointDR1);
                        tmplist1.Add(PointDR2);
                    }

                    if (TmpUR.X > TmpDR.X)
                    {
                        PointUL1 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioC, (TmpDL.Y - TmpUL.Y) * PosRatioC) + TmpUL;
                        PointUL2 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioD, (TmpDL.Y - TmpUL.Y) * PosRatioD) + TmpUL;
                        tmplist1.Add(PointUL1);
                        tmplist1.Add(PointUL2);
                    }
                    else if (TmpUR.X < TmpDR.X)
                    {
                        PointUR1 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioC, (TmpDR.Y - TmpUR.Y) * PosRatioC) + TmpUR;
                        PointUR2 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioD, (TmpDR.Y - TmpUR.Y) * PosRatioD) + TmpUR;
                        tmplist1.Add(PointUR1);
                        tmplist1.Add(PointUR2);
                    }

                    for (int z = 0; z < tmplist1.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.BACK, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = tmplist1[z].X;
                        TmpBoltsArr.Y = tmplist1[z].Y;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                        lstBoltsCutPoint.Add(bolts);
                    }

                    Viewbox.IsEnabled = false;
                    break;

                case FACE.FRONT:

                    if (TmpSteelAttr.uPoint.Count == 0) return;

                    var tmp2 = TmpSteelAttr.uPoint.GroupBy(uu => uu.Y).Select(q => new
                    {
                        key = q.Key,
                        max = q.Max(x => x.X),
                        min = q.Min(f => f.X)
                    }).ToList();

                    if (tmp2[0].key > tmp2[1].key)
                    {
                        var swap = tmp2[0];
                        tmp2[0] = tmp2[1];
                        tmp2[1] = swap;
                    }

                    TmpDL = new Point3D(tmp2[0].min, tmp2[0].key);
                    TmpDR = new Point3D(tmp2[0].max, tmp2[0].key);
                    TmpUL = new Point3D(tmp2[1].min, tmp2[1].key);
                    TmpUR = new Point3D(tmp2[1].max, tmp2[1].key);

                    if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X))
                        return;


                    if (TmpUL.X > TmpDL.X)
                    {
                        PointDL1 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioC, (TmpUL.Y - TmpDL.Y) * PosRatioC) + TmpDL;
                        PointDL2 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioD, (TmpUL.Y - TmpDL.Y) * PosRatioD) + TmpDL;
                        tmplist1.Add(PointDL1);
                        tmplist1.Add(PointDL2);
                    }
                    else if (TmpUL.X < TmpDL.X)
                    {
                        PointDR1 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioC, (TmpUR.Y - TmpDR.Y) * PosRatioC) + TmpDR;
                        PointDR2 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioD, (TmpUR.Y - TmpDR.Y) * PosRatioD) + TmpDR;
                        tmplist1.Add(PointDR1);
                        tmplist1.Add(PointDR2);
                    }

                    if (TmpUR.X > TmpDR.X)
                    {
                        PointUL1 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioC, (TmpDL.Y - TmpUL.Y) * PosRatioC) + TmpUL;
                        PointUL2 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioD, (TmpDL.Y - TmpUL.Y) * PosRatioD) + TmpUL;
                        tmplist1.Add(PointUL1);
                        tmplist1.Add(PointUL2);
                    }
                    else if (TmpUR.X < TmpDR.X)
                    {
                        PointUR1 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioC, (TmpDR.Y - TmpUR.Y) * PosRatioC) + TmpUR;
                        PointUR2 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioD, (TmpDR.Y - TmpUR.Y) * PosRatioD) + TmpUR;
                        tmplist1.Add(PointUR1);
                        tmplist1.Add(PointUR2);
                    }

                    for (int z = 0; z < tmplist1.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.FRONT, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = tmplist1[z].X;
                        TmpBoltsArr.Y = tmplist1[z].Y;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                        lstBoltsCutPoint.Add(bolts);
                    }
                    Viewbox.IsEnabled = false;
                    break;


                case FACE.TOP:

                    if (TmpSteelAttr.vPoint.Count == 0) return;

                    var tmp3 = TmpSteelAttr.vPoint.GroupBy(uu => uu.Y).Select(q => new
                    {
                        key = q.Key,
                        max = q.Max(x => x.X),
                        min = q.Min(f => f.X)
                    }).ToList();

                    if (tmp3[0].key > tmp3[1].key)
                    {
                        
                        var swap = tmp3[0];
                        tmp3[0] = tmp3[1];
                        tmp3[1] = swap;
                    }

                    TmpDL = new Point3D(tmp3[0].min, tmp3[0].key);
                    TmpDR = new Point3D(tmp3[0].max, tmp3[0].key);
                    TmpUL = new Point3D(tmp3[1].min, tmp3[1].key);
                    TmpUR = new Point3D(tmp3[1].max, tmp3[1].key);

                    if ((TmpUL.X == TmpDL.X) && (TmpUR.X == TmpDR.X))
                        return;


                    if (TmpUL.X > TmpDL.X)
                    {
                        PointDL1 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioA, (TmpUL.Y - TmpDL.Y) * PosRatioA) + TmpDL;
                        PointDL2 = new Point3D((TmpUL.X - TmpDL.X) * PosRatioB, (TmpUL.Y - TmpDL.Y) * PosRatioB) + TmpDL;
                        tmplist1.Add(PointDL1);
                        tmplist1.Add(PointDL2);
                    }
                    else if(TmpUL.X < TmpDL.X)
                    {
                        PointUL1 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioA, (TmpDL.Y - TmpUL.Y) * PosRatioA) + TmpUL;
                        PointUL2 = new Point3D((TmpDL.X - TmpUL.X) * PosRatioB, (TmpDL.Y - TmpUL.Y) * PosRatioB) + TmpUL;
                        tmplist1.Add(PointUL1);
                        tmplist1.Add(PointUL2);
                    }

                    if (TmpUR.X > TmpDR.X)
                    {
                        PointDR1 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioA, (TmpUR.Y - TmpDR.Y) * PosRatioA) + TmpDR;
                        PointDR2 = new Point3D((TmpUR.X - TmpDR.X) * PosRatioB, (TmpUR.Y - TmpDR.Y) * PosRatioB) + TmpDR;
                        tmplist1.Add(PointDR1);
                        tmplist1.Add(PointDR2);
                    }
                    else if(TmpUR.X < TmpDR.X)
                    {
                        PointUR1 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioA, (TmpDR.Y - TmpUR.Y) * PosRatioA) + TmpUR;
                        PointUR2 = new Point3D((TmpDR.X - TmpUR.X) * PosRatioB, (TmpDR.Y - TmpUR.Y) * PosRatioB) + TmpUR;
                        tmplist1.Add(PointUR1);
                        tmplist1.Add(PointUR2);
                    }


                    for (int z = 0; z < tmplist1.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(FACE.TOP, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = tmplist1[z].X;
                        TmpBoltsArr.Y = tmplist1[z].Y;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                        lstBoltsCutPoint.Add(bolts);
                    }

                    ViewModel.SteelAttr = TmpSteelAttr;
                    model.Entities[model.Entities.Count - 1].EntityData= TmpSteelAttr;
                    Viewbox.IsEnabled = false;
                    break;
            }

            //刷新模型
            model.Refresh();
            drawing.Refresh();




            if (!fAddPartAndBolt)   //  是否新增零件及孔群 : false 直接存檔
            {
                SaveModel(true,false);//存取檔案 

            }




        }

        /// <summary>
        /// 手動斜邊打點
        /// </summary>
        public void ManHypotenusePoint(FACE face)
        {

            double a, b;
            List<(double, double)> DRPoint = new List<(double, double)>();
            List<(double, double)> HypotenusePoint = new List<(double, double)>();
            List<Point3D> result = null;

            MyCs myCs = new MyCs();

            STDSerialization ser = new STDSerialization();
            ObservableCollection<SplitLineSettingClass> ReadSplitLineSettingData = ser.GetSplitLineData();//備份當前加工區域數值

            double PosRatioA = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].A);     //  腹板斜邊打點比列(短)
            double PosRatioB = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].B);     //  腹板斜邊打點比列(長)
            double PosRatioC = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].C);     //  翼板斜邊打點比列(短)
            double PosRatioD = myCs.DivSymbolConvert(ReadSplitLineSettingData[0].D);     //  翼板斜邊打點比列(長)

            SteelAttr steelAttr = ViewModel.GetSteelAttr();
 





            switch (face)
            {
                case FACE.BACK:
                    if (steelAttr.Back == null)
                        return;

                    //UL
                    result = steelAttr.Back.UL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[0].Y));
                    }

                    //UR
                    result = steelAttr.Back.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, result[1].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, result[1].Y - (PosRatioD * b)));
                    }

                    //DL
                    result = steelAttr.Back.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a), result[2].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a), result[2].Y - (PosRatioD * b)));
                    }

                    //DR
                    result = steelAttr.Back.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[1].Y));
                    }


                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(face, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                    }
                    break;

                case FACE.TOP:
                    if (steelAttr.Top == null)
                        return;

                    //UL
                    result = steelAttr.Top.UL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, (PosRatioA * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, (PosRatioB * b) + result[0].Y));
                    }

                    //UR
                    result = steelAttr.Top.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, result[1].Y - (PosRatioA * b)));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, result[1].Y - (PosRatioB * b)));
                    }

                    //DL
                    result = steelAttr.Top.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioA * a), result[2].Y - (PosRatioA * b)));
                        HypotenusePoint.Add(((PosRatioB * a), result[2].Y - (PosRatioB * b)));
                    }

                    //DR
                    result = steelAttr.Top.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioA * a) + result[0].X, (PosRatioA * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioB * a) + result[0].X, (PosRatioB * b) + result[1].Y));
                    }

                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(face, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool check);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
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
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[0].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[0].Y));
                    }

                    //UR                    
                    result = steelAttr.Front.UR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, result[1].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, result[1].Y - (PosRatioD * b)));
                    }

                    //DL
                    result = steelAttr.Front.DL;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a), result[2].Y - (PosRatioC * b)));
                        HypotenusePoint.Add(((PosRatioD * a), result[2].Y - (PosRatioD * b)));
                    }

                    //DR
                    result = steelAttr.Front.DR;
                    if (result.Count > 0)
                    {
                        DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
                        a = DRPoint[0].Item1;
                        b = DRPoint[0].Item2;
                        HypotenusePoint.Add(((PosRatioC * a) + result[0].X, (PosRatioC * b) + result[1].Y));
                        HypotenusePoint.Add(((PosRatioD * a) + result[0].X, (PosRatioD * b) + result[1].Y));
                    }


                    for (int z = 0; z < HypotenusePoint.Count; z++)
                    {
                        GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr(face, START_HOLE.START);
                        TmpBoltsArr.dX = "0";
                        TmpBoltsArr.dY = "0";
                        TmpBoltsArr.xCount = 1;
                        TmpBoltsArr.yCount = 1;
                        TmpBoltsArr.Mode = AXIS_MODE.POINT;
                        TmpBoltsArr.X = HypotenusePoint[z].Item1;
                        TmpBoltsArr.Y = HypotenusePoint[z].Item2;
                        TmpBoltsArr.GUID = Guid.NewGuid();
                        Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference, out bool CheckArea);
                        BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
                    }
                    break;

            }

            //刷新模型
            model.Refresh();
            drawing.Refresh();

            fAddHypotenusePoint = true; //  執行斜邊打點功能

            if (!fAddPartAndBolt)   //  是否新增零件及孔群 : false 直接存檔
                SaveModel(false);//存取檔案 

        }



        //************************************************************

        //        switch ((FACE)ViewModel.CutFaceType)
        //{
        //    case FACE.BACK:
        //        if (steelAttr.Back == null)
        //            return;


        //        for (int z = 0; z < lstBoltsCutPoint.Count; z++)
        //        {
        //            if (lstBoltsCutPoint[z].Info.Face ==FACE.BACK)
        //            {
        //                lstBoltsCutPoint[z].Entities[0].Selected= true;
        //            }
        //        }
        //        model.Entities.DeleteSelected();

        //        //UL
        //        result = steelAttr.Back.UL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, (PosRatio1 * b) + result[0].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, (PosRatio2 * b) + result[0].Y));
        //        }

        //        //UR
        //        result = steelAttr.Back.UR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, result[1].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, result[1].Y - (PosRatio2 * b)));
        //        }

        //        //DL
        //        result = steelAttr.Back.DL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a), result[2].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a), result[2].Y - (PosRatio2 * b)));
        //        }

        //        //DR
        //        result = steelAttr.Back.DR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, (PosRatio1 * b) + result[1].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, (PosRatio2 * b) + result[1].Y));
        //        }


        //        for (int z = 0; z < HypotenusePoint.Count; z++)
        //        {
        //            GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr((FACE)ViewModel.CutFaceType, START_HOLE.START);
        //            TmpBoltsArr.dX = "0";
        //            TmpBoltsArr.dY = "0";
        //            TmpBoltsArr.xCount = 1;
        //            TmpBoltsArr.yCount = 1;
        //            TmpBoltsArr.Mode = AXIS_MODE.POINT;
        //            TmpBoltsArr.X = HypotenusePoint[z].Item1;
        //            TmpBoltsArr.Y = HypotenusePoint[z].Item2;
        //            TmpBoltsArr.GUID = Guid.NewGuid();
        //            Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference);
        //            BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
        //            lstBoltsCutPoint.Add(bolts);
        //        }


        //        break;



        //    case FACE.TOP:
        //        if (steelAttr.Top == null)
        //            return;


        //        //找尋並記錄斜邊打點LIST內,為TOP面的孔群名稱(GUID)
        //        List<string> qa  = lstBoltsCutPoint.Where(x => x.Info.Face == FACE.TOP).Select(x => (x.Info.GUID.ToString())).ToList();

        //        // 依找出為TOP的GUID，找尋MODEL內所有BLOCK對應GUID並記錄
        //        var ssss = model.Blocks.Where(x => qa.Contains(x.Name)).ToList();

        //        foreach (var item in ssss)
        //        {
        //            item.Entities.ForEach(aaa => aaa.Selected = true);
        //            model.Entities.DeleteSelected();

        //        }



        //        //foreach (var item in model.Blocks.Where(x => qa.Contains(x.Name)))
        //        //{

        //        //}


        //        //for (int z = 0; z < lstBoltsCutPoint.Count; z++)
        //        //{
        //        //    if (lstBoltsCutPoint[z].Info.Face == FACE.TOP)
        //        //    {
        //        //        lstBoltsCutPoint[z].Info.GUID;
        //        //        model.Entities[z].Selected = true;
        //        //    }
        //        //}
        //      //   model.Entities.DeleteSelected();


        //        //int[] array1 = new int[10] {-1, -1 , -1 , -1 , -1 , -1 , -1 , -1 , -1 , -1 };
        //        //int acount = 0;

        //        //for (int z = 0; z < lstBoltsCutPoint.Count; z++)
        //        //{
        //        //    if (lstBoltsCutPoint[z].Info.Face == FACE.TOP)
        //        //    {

        //        //        int xx=model.Entities[z].FindIndex(el => el.Name == lstBoltsCutPoint[z].Name);
        //        //        model.Blocks[xx].Entities[0].Selected = true;
        //        //       // lstBoltsCutPoint[z].Entities[0].Selected = true;
        //        //        array1[acount] = z;
        //        //        lstBoltsCutPoint.RemoveAt(z);
        //        //        acount++;

        //        //    }
        //        //}

        //        //model.Entities.DeleteSelected();

        //        //for (int z = array1.Length - 1; z >= 0; z--)
        //        //{
        //        //    if (array1[z]!=-1)
        //        //        lstBoltsCutPoint.RemoveAt(array1[z]);

        //        //}
        //        //if (!fAddPartAndBolt)   //  是否新增零件及孔群 : false 直接存檔
        //        //    SaveModel(false);//存取檔案 


        //        //for (int i = 0; i < model.Entities.Count; i++)//逐步展開孔群資訊
        //        //{
        //        //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr)
        //        //    {

        //        //        model.Entities[i].Selected = true;

        //        //    }
        //        //}


        //        //model.Entities.DeleteSelected();
        //        SaveModel(false);
        //        model.Refresh();
        //        drawing.Refresh();

        //        //UL
        //        result = steelAttr.Top.UL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a), (PosRatio1 * b) + result[0].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a), (PosRatio2 * b) + result[0].Y));
        //        }

        //        //UR
        //        result = steelAttr.Top.UR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, result[1].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, result[1].Y - (PosRatio2 * b)));
        //        }

        //        //DL
        //        result = steelAttr.Top.DL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a), result[2].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a), result[2].Y - (PosRatio2 * b)));
        //        }

        //        //DR
        //        result = steelAttr.Top.DR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, (PosRatio1 * b) + result[1].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, (PosRatio2 * b) + result[1].Y));
        //        }

        //        for (int z = 0; z < HypotenusePoint.Count; z++)
        //        {
        //            GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr((FACE)ViewModel.CutFaceType, START_HOLE.START);
        //            TmpBoltsArr.dX = "0";
        //            TmpBoltsArr.dY = "0";
        //            TmpBoltsArr.xCount = 1;
        //            TmpBoltsArr.yCount = 1;
        //            TmpBoltsArr.Mode = AXIS_MODE.POINT;
        //            TmpBoltsArr.X = HypotenusePoint[z].Item1;
        //            TmpBoltsArr.Y = HypotenusePoint[z].Item2;
        //            TmpBoltsArr.GUID = Guid.NewGuid();
        //            Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference);
        //            BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
        //            lstBoltsCutPoint.Add(bolts);
        //        }
        //        break;


        //    case FACE.FRONT:
        //        if (steelAttr.Front == null)
        //            return;



        //        for (int z = 0; z < lstBoltsCutPoint.Count; z++)
        //        {
        //            if (lstBoltsCutPoint[z].Info.Face == FACE.FRONT)
        //            {
        //                lstBoltsCutPoint[z].Entities[0].Selected = true;
        //                model.Entities.DeleteSelected();
        //            }
        //        }

        //        //UL
        //        result = steelAttr.Front.UL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[2].X - result[1].X, result[1].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, (PosRatio1 * b) + result[0].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, (PosRatio2 * b) + result[0].Y));
        //        }

        //        //UR                    
        //        result = steelAttr.Front.UR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[1].Y - result[2].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, result[1].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, result[1].Y - (PosRatio2 * b)));
        //        }

        //        //DL
        //        result = steelAttr.Front.DL;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[0].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a), result[2].Y - (PosRatio1 * b)));
        //            HypotenusePoint.Add(((PosRatio2 * a), result[2].Y - (PosRatio2 * b)));
        //        }

        //        //DR
        //        result = steelAttr.Front.DR;
        //        if (result.Count > 0)
        //        {
        //            DRPoint.Add((result[1].X - result[0].X, result[2].Y - result[1].Y));
        //            a = DRPoint[DRPoint.Count - 1].Item1;
        //            b = DRPoint[DRPoint.Count - 1].Item2;
        //            HypotenusePoint.Add(((PosRatio1 * a) + result[0].X, (PosRatio1 * b) + result[1].Y));
        //            HypotenusePoint.Add(((PosRatio2 * a) + result[0].X, (PosRatio2 * b) + result[1].Y));
        //        }


        //        for (int z = 0; z < HypotenusePoint.Count; z++)
        //        {
        //            GroupBoltsAttr TmpBoltsArr = ViewModel.GetHypotenuseBoltsAttr((FACE)ViewModel.CutFaceType, START_HOLE.START);
        //            TmpBoltsArr.dX = "0";
        //            TmpBoltsArr.dY = "0";
        //            TmpBoltsArr.xCount = 1;
        //            TmpBoltsArr.yCount = 1;
        //            TmpBoltsArr.Mode = AXIS_MODE.POINT;
        //            TmpBoltsArr.X = HypotenusePoint[z].Item1;
        //            TmpBoltsArr.Y = HypotenusePoint[z].Item2;
        //            TmpBoltsArr.GUID = Guid.NewGuid();
        //            Bolts3DBlock bolts = Bolts3DBlock.AddBolts(TmpBoltsArr, model, out BlockReference blockReference);
        //            BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D
        //            lstBoltsCutPoint.Add(bolts);
        //        }
        //        break;

        //}






        /// <summary>
        /// 取消所有動作
        /// </summary>
        private void Esc()
        {





            drawing.SetCurrent(null);
            model.SetCurrent(null);//層級 To 要編輯的 BlockReference

            model.ActionMode = actionType.SelectByBox;
            drawing.ActionMode = actionType.SelectByBox;


            model.Entities.ClearSelection();//清除全部選取的物件
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


            if (!fAddPartAndBolt)
                SaveModel(false);//存取檔案


        }
        /// <summary>
        /// 存取模型
        /// </summary>
        /// <param name="add"></param>
        /// <param name="reflesh">是否更新Grid</param>
        public void SaveModel(bool add,bool reflesh=true)
        {
            STDSerialization ser = new STDSerialization();
            ser.SetPartModel(ViewModel.SteelAttr.GUID.ToString(), model);

            var ass = new GD_STD.Data.SteelAssembly()
            {
                //GUID = ViewModel.SteelAttr.GUID,
                Count = ViewModel.SteelAttr.Number,
                IsTekla = false,
                Length = ViewModel.SteelAttr.Length,
                ShippingDescription = new List<string>(new string[ViewModel.SteelAttr.Number]),
                ShippingNumber = new List<int>(new int[ViewModel.SteelAttr.Number]),
                Phase = new List<int>(new int[ViewModel.SteelAttr.Number]),
                Number = ViewModel.SteelAttr.PartNumber,
            };
            List<int> buffer = new List<int>(), _buffer = new List<int>();
            Random random = new Random();

            #region 構件資訊
            // 若無構件資訊，新增資訊
            if (ViewModel.SteelAssemblies.IndexOf(ass) == -1 && add)
            {
                //ass = new SteelAssembly()
                //{
                //    GUID = ViewModel.SteelAttr.GUID,
                //    Count = ViewModel.SteelAttr.Number,
                //    IsTekla = false,
                //    Length = ViewModel.SteelAttr.Length,
                //    ShippingDescription = new List<string>(new string[ViewModel.SteelAttr.Number]),
                //    ShippingNumber = new List<int>(new int[ViewModel.SteelAttr.Number]),
                //    Phase = new List<int>(new int[ViewModel.SteelAttr.Number]),
                //    Number = ViewModel.SteelAttr.PartNumber,
                //};
                ass.ID = new List<int>();
                for (int i = 0; i < ViewModel.SteelAssemblies.Count; i++)
                {
                    _buffer.AddRange(ViewModel.SteelAssemblies[i].ID);
                }
                random = new Random();
                while (buffer.Count != ViewModel.SteelAttr.Number)
                {
                    int id = random.Next(1000000, 90000000);
                    if (!buffer.Contains(id))
                    {
                        buffer.Add(id);

                    }
                }
                ass.ID.AddRange(buffer.ToArray());
                ViewModel.SteelAssemblies.Add(ass);
            }
            //else 
            //{
            //    if (ViewModel.SteelAssemblies.Where(x => x.GUID == ViewModel.SteelAttr.GUID).FirstOrDefault().Count!= ViewModel.SteelAttr.Number)
            //    {
            //        buffer.Clear();
            //        while (buffer.Count != ViewModel.SteelAttr.Number)
            //        {
            //            int id = random.Next(1000000, 90000000);
            //            if (!buffer.Contains(id))
            //            {
            //                buffer.Add(id);
            //            }
            //        }
            //    }
            //    ViewModel.SteelAssemblies.Where(x => x.GUID == ViewModel.SteelAttr.GUID).FirstOrDefault().ID.Clear();
            //    ViewModel.SteelAssemblies.Where(x => x.GUID == ViewModel.SteelAttr.GUID).FirstOrDefault().ID.AddRange(buffer.ToArray());
            //    ass.ID = buffer.ToList();
            //}
            #endregion

            #region 斷面規格
            // 斷面規格
            var profile = ser.GetProfile();
            if (!profile.Contains(ViewModel.SteelAttr.Profile))
            {
                profile.Add(ViewModel.SteelAttr.Profile);
                ser.SetProfileList(profile);
            }
            #endregion

            #region 零件列表
            // 2022/09/08 呂宗霖 與架構師討論後，零件編輯單純做編輯動作            
            // 零件列表
            SteelPart steelPart = new SteelPart(
                ViewModel.SteelAttr,
                ViewModel.SteelAttr.Name,
                ViewModel.SteelAttr.PartNumber,
                ViewModel.SteelAttr.Length,
                ViewModel.SteelAttr.Number,
                ViewModel.SteelAttr.GUID.Value,
                ViewModel.SteelAttr.Phase,
                ViewModel.SteelAttr.ShippingNumber,
                ViewModel.SteelAttr.Title1,
                ViewModel.SteelAttr.Title2,ViewModel.SteelAttr.Lock);
            steelPart.ID = new List<int>();
            steelPart.Match = new List<bool>();
            steelPart.Material = ViewModel.SteelAttr.Material;
            steelPart.Father = ass.ID;
            //steelPart.Length = ViewModel.SteelAttr.Length;

            for (int i = 0; i < steelPart.Count; i++)
            {
                steelPart.Match.Add(true);
            }
            buffer.Clear();
            while (buffer.Count != ViewModel.SteelAttr.Number)
            {
                int id = random.Next(1000000, 9000000);
                if (!buffer.Contains(id))
                {
                    buffer.Add(id);
                }
            }
            steelPart.ID = buffer.ToList();
            ObservableCollection<SteelPart> collection = new ObservableCollection<SteelPart>();
            if (File.Exists($@"{ApplicationVM.DirectorySteelPart()}\{steelPart.Profile.GetHashCode()}.lis"))
            {
                collection = ser.GetPart($@"{steelPart.Profile.GetHashCode()}");
            }
            //if (!collection.Any(x => x.Number == steelPart.Number && x.Profile == steelPart.Profile && x.Type == x.Type && x.GUID == steelPart.GUID))
            //{
                // 不存在則新增
                collection.Add(steelPart);
            //}
            //else {
            //    // 存在則編輯
            //    SteelPart sp = collection.Where(x => x.Number == steelPart.Number && x.Profile == steelPart.Profile && x.Type == x.Type && x.GUID == steelPart.GUID).FirstOrDefault();
            //    sp.Length = steelPart.Length;
            //    sp.W = steelPart.W;
            //    sp.t1 = steelPart.t1;
            //    sp.t2 = steelPart.t2;
            //    sp.H = steelPart.H;  
            //    sp.Material = steelPart.Material;                
            //    sp.Phase = steelPart.Phase;
            //    sp.ShippingNumber = steelPart.ShippingNumber;
            //    sp.Title1 = steelPart.Title1;
            //    sp.Title2 = steelPart.Title2;
            //    sp.Revise = steelPart.Revise;
            //    sp.Father = steelPart.Father;
            //    sp.ID = steelPart.ID;
            //}

            ser.SetPart($@"{steelPart.Profile.GetHashCode()}", new ObservableCollection<object>(collection));
            #endregion

            if (add)
            {
                ser.SetSteelAssemblies(ViewModel.SteelAssemblies);
            }
            ViewModel.SaveDataCorrespond();
            if (reflesh)
            {
                ObservableCollection<ProductSettingsPageViewModel> data = new ObservableCollection<ProductSettingsPageViewModel>(sr.GetData());
                ViewModel.DataViews = data;
                PieceListGridControl.ItemsSource = data;

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
        }

        /// <summary>
        /// 此設定會影響2D 3D的顯示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlSelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (((System.Windows.FrameworkElement)tabControl.SelectedValue).Name== "drawingTab")
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
            ApplicationVM appVM = new ApplicationVM();
            appVM.CreateDMFile(model);



        }

        private BlockReference SteelTriangulation(Mesh mesh)
        {
#if DEBUG
            //log4net.LogManager.GetLogger("產生2D").Debug("開始");
            log4net.LogManager.GetLogger($"產生2D圖塊(TOP.FRONT.BACK)").Debug($"開始");
#endif
            drawing.Blocks.Clear();
            drawing.Entities.Clear();

            Steel2DBlock steel2DBlock = new Steel2DBlock(mesh, model.Blocks[1].Name);
            drawing.Blocks.Add(steel2DBlock);
            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊
            //關閉三視圖用戶選擇
            block2D.Selectable = false;

            // 將TOP FRONT BACK圖塊加入drawing
            drawing.Entities.Add(block2D);

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
                drawing.Blocks.Add(bolts2DBlock); //加入螺栓圖塊
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

            if (fAddPartAndBolt)  //  新增零件功能
            {
                var ResultRtn = WinUIMessageBox.Show(null,
                     $"新增零件與鑽孔位置未存檔,是否存檔",
                     "通知",
                     MessageBoxButton.OKCancel,
                     MessageBoxImage.Exclamation,
                     MessageBoxResult.None,
                     MessageBoxOptions.None,
                     FloatingMode.Popup);


                if (ResultRtn == MessageBoxResult.OK)
                    SaveModel(true);//存取檔案

                fAddPartAndBolt = false;
                fAddHypotenusePoint = false;
            }

            //  執行斜邊打點功能
            if (fAddHypotenusePoint)
            {
                var ResultRtn = WinUIMessageBox.Show(null,
                     $"切割線打點異動未存檔,是否存檔",
                     "通知",
                     MessageBoxButton.OKCancel,
                     MessageBoxImage.Exclamation,
                     MessageBoxResult.None,
                     MessageBoxOptions.None,
                     FloatingMode.Popup);


                if (ResultRtn == MessageBoxResult.OK)
                    SaveModel(true);//存取檔案

                fAddHypotenusePoint = false;                                                                                                               

            }
            


            TreeView treeView = (TreeView)sender; //樹狀列表
            TreeNode data = (TreeNode)e.NewValue;
            if (data.DataName == null)
                return;
            STDSerialization ser = new STDSerialization();
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            NcTemp ncTemp = ncTemps.GetData(data.DataName);//需要實體化的nc物件
            model.Clear(); //清除目前模型
            if (ncTemp == null) //NC 檔案是空值
            {
                ReadFile readFile = ser.ReadPartModel(data.DataName); //讀取檔案內容
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
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                if (model.Entities[model.Entities.Count - 1].EntityData is null)
                {
                    return;
                }
                ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
                ViewModel.GetSteelAttr();
                model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式(零件)
#if DEBUG
                log4net.LogManager.GetLogger($"GUID:{data.DataName}").Debug($"零件: {model.Blocks[1].Name}");
#endif
                //產生零件2D對應圖塊、座標及相關參數
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
                        Add2DHole(bolts3DBlock, true);//加入孔位不刷新 2d 視圖



                    }
                }
            }
            else //如果需要載入 nc 設定檔
            {
                model.LoadNcToModel(data.DataName);
                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊
            }

            // 執行斜邊打點
           // AutoHypotenusePoint(FACE.TOP);
      //      HypotenusePoint(FACE.BACK);
       //     HypotenusePoint(FACE.FRONT);

            model.ZoomFit();//設置道適合的視口
            model.Invalidate();//初始化模型
            model.Refresh();
            drawing.ZoomFit();//設置道適合的視口
            drawing.Invalidate();
            drawing.Refresh();


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
        private void Grid_SelectedChange(object sender, SelectedItemChangedEventArgs e)
        {

            if (model != null)
            {
                ProductSettingsPageViewModel item = (ProductSettingsPageViewModel)PieceListGridControl.SelectedItem;

                STDSerialization ser = new STDSerialization();
                DataCorrespond = ser.GetDataCorrespond();

                    ReadFile readFile = ser.ReadPartModel(item.DataName.ToString()); //讀取檔案內容
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
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
                ViewModel.GetSteelAttr();





                var aaa = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                var bbb = DataCorrespond.Where(p => p.Number == aaa.PartNumber).ToList();
                {
                 
                    aaa.oPoint = bbb[0].oPoint.ToList();
                    aaa.vPoint = bbb[0].vPoint.ToList();
                    aaa.uPoint = bbb[0].uPoint.ToList();
                    ViewModel.WriteSteelAttr(aaa);
                    ViewModel.GetSteelAttr();
                }




                model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                
                
                for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                {
                    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                    {
                        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                        Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
                        Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                    }
                }

                model.ZoomFit();//設置道適合的視口
                model.Invalidate();//初始化模型
                drawing.ZoomFit();//設置道適合的視口
                drawing.Invalidate();

                // 執行斜邊打點
                RunHypotenusePoint();
   
            }

        }

        private void OKtoConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (fAddPartAndBolt)
            {
                var ResultRtn = WinUIMessageBox.Show(null,
                         $"新增零件是否存檔 ?",
                         "通知",
                         MessageBoxButton.OKCancel,
                         MessageBoxImage.Exclamation,
                         MessageBoxResult.None,
                         MessageBoxOptions.None,
                         FloatingMode.Popup);


                if (ResultRtn == MessageBoxResult.OK)
                    SaveModel(true);//存取檔案

                fAddPartAndBolt = false;
            }
        }
    }
}
