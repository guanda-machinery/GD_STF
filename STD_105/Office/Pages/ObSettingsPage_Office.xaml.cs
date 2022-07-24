using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Data;
using GD_STD.Enum;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using BlockReference = devDept.Eyeshot.Entities.BlockReference;
using MouseButton = devDept.Eyeshot.MouseButton;
using static GD_STD.ServerLogHelper;

namespace STD_105.Office
{
    //TODO:假
    /// <summary>
    /// ObSettingsPage_Office.xaml 的互動邏輯
    /// </summary>
    public partial class ObSettingsPage_Office : BasePage<ObSettingVM>
    {
        public ObSettingsPage_Office()
        {
            InitializeComponent();
            //2022.06.24 呂宗霖 此Class與GraphWin.xaml.cs皆有SteelTriangulation與Add2DHole
            //                  先使用本Class 若有問題再修改
            //GraphWin service = new GraphWin();
            #region 3D
            //model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            model.Secondary = drawing;
            #endregion
            #region 2D
            //drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.Unlock("UF20-HN12H-22P6C-71M1-FXP4");
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            drawing.Secondary = model;
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
                    //MessageBox.Show("目前已在編輯模式內，如要離開請按下Esc", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                //模擬鍵盤按下Delete
                //var c = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Delete)
                //{
                //    RoutedEvent = Keyboard.KeyDownEvent
                //};
                //InputManager.Current.ProcessInput(c);
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
                if (!CheckPart()) //檢測用戶輸入的參數是否有完整
                    return;
                model.Entities.Clear();//清除模型物件
                model.Blocks.Clear(); //清除模型圖塊

                ViewModel.SteelAttr.GUID = Guid.NewGuid();//產生新的 id
                // 2022/07/14 呂宗霖 guid2區分2d或3d
                //ViewModel.SteelAttr.GUID2 = ViewModel.SteelAttr.GUID;
                ViewModel.SteelAttr.PointFront = new CutList();//清除切割線
                ViewModel.SteelAttr.PointTop = new CutList();//清除切割線
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
                model.ZoomFit();//設置道適合的視口
                model.Refresh();//刷新模型
                //drawing.ZoomFit();
                //drawing.Refresh();
                SaveModel(true);
                #endregion
            });
            //修改主零件
            ViewModel.ModifyPart = new RelayCommand(() =>
            {
            if (!CheckPart()) //檢測用戶輸入的參數是否有完整
                return;
            if (model.CurrentBlockReference != null)
            {
                //MessageBox.Show("退出編輯模式，才可修改主件", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                BlockReference reference2D =(BlockReference)sele2D.Item;



                //模擬用戶實際選擇編輯
                ViewModel.Select3DItem.Add(sele3D);
            ViewModel.Select2DItem.Add(sele2D);
            //層級 To 要編輯的BlockReference
            model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);


                drawing.SetCurrent((BlockReference)drawing.Entities[0]);

                SteelAttr steelAttr = ViewModel.GetSteelAttr();
                steelAttr.GUID = ((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData).GUID;//修改唯一識別ID
                Mesh modify = Steel3DBlock.GetProfile(steelAttr); //修改的形狀
                ViewModel.tem3DRecycle.Add(model.Entities[model.Entities.Count - 1]);//加入垃圾桶準備刪除
                ViewModel.tem2DRecycle.AddRange(drawing.Entities);//加入垃圾桶準備刪除

                //model.Entities[0].Selected = true;//選擇物件
                drawing.Entities.ForEach(el => el.Selected = true);
                List<Entity> steel2D = new Steel2DBlock(modify, "123").Entities.ToList();
                Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[drawing.CurrentBlockReference.BlockName];
                steel2DBlock.ChangeMesh(modify);
                //加入到垃圾桶內
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
                SaveModel(false);//存取檔案

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
                    //MessageBox.Show("退出編輯模式，才可讀取主件", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                    model.SetCurrent((BlockReference)model.Entities[model.Entities.Count - 1]);
                    ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);
                    model.SetCurrent(null);
                }
                else
                {
                    //MessageBox.Show("模型內找不到物件", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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


                Bolts3DBlock bolts = Bolts3DBlock.AddBolts(ViewModel.GetGroupBoltsAttr(), model, out BlockReference blockReference);
                BlockReference referenceBolts = Add2DHole(bolts);//加入孔位到2D

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
                            //MessageBox.Show("退出編輯模式，才可修改孔", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                        //MessageBox.Show("退出編輯模式，才可讀取", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                        //MessageBox.Show("選擇類型必須是孔，才可讀取", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                    //MessageBox.Show("請選擇孔，才可修讀取", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            //讀取切割線設定
            ViewModel.ReadCut = new RelayCommand(() =>
            {
                //如果是在編輯模式
                if (model.CurrentBlockReference != null)
                {
                    //MessageBox.Show("退出編輯模式，才可讀取切割線", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                    //MessageBox.Show("模型內找不到物件", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                                //MessageBox.Show("退出編輯模式，才可鏡射", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                            Point3D boxMin, boxMax;
                            //model.Entities.ForEach(el => points.AddRange(el.Vertices));

                            var modelSelected = model.Blocks[reference3D.BlockName].Entities.Where(x => x.Selectable).ToList();
                            var drawingSelected = drawing.Blocks[reference2D.BlockName].Entities.Where(x => x.Selectable && x.GetType().Name == "Circle").ToList();

                            modelSelected.ForEach(x =>
                            {
                                BoltAttr ba = (BoltAttr)x.EntityData;
                                Point3D p = new Point3D(ba.X, ba.Y, ba.Z);
                                points.Add(p);
                            });
                            //drawingSelected.ForEach(x =>
                            //{
                            //    BoltAttr ba = (BoltAttr)x.EntityData;
                            //    Point3D p = new Point3D(ba.X, ba.Y, ba.Z);
                            //    points.Add(p);
                            //});
                            points.ForEach(p =>
                            {
                                finalPoint.Add(new Point3D { X = p.X, Y = p.Y, Z = p.Z });
                            });
                            finalPoint = finalPoint.Distinct().ToList();                     

                            Utility.ComputeBoundingBox(null, finalPoint, out boxMin, out boxMax);
                            Point3D center = (boxMin + boxMax) / 2; //鏡射中心點

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

                            //FACE face = groupBolts.Info.Face; //孔位的面

                            //3D鏡射參數
                            Point3D p3D1 = new Point3D(), p3D2 = new Point3D(), p3D3 = new Point3D();//鏡射座標

                            //2D鏡射
                            Point3D p2D1 = p21, p2D2 = p22;//鏡射座標
                            Vector3D axis2D = new Vector3D();//鏡射軸
                            Vector3D axis3D = new Vector3D();//鏡射軸

                            Bolts2DBlock bolts2DBlock = (Bolts2DBlock)drawing.Blocks[reference2D.BlockName];
                            // 鏡射 X 軸 : 需要三個點
                            // 俯視圖:XY平面.Y軸.中心點
                            // 前後視圖:XZ平面.Z軸.中心點
                            switch (face)
                            {
                                case GD_STD.Enum.FACE.TOP:
                                    p3D1 = p31;
                                    p3D2 = p32;
                                    p3D3 = p33;
                                    axis3D = Vector3D.AxisY;

                                    //axis3D = new Vector3D(1, 1, 0);
                                    //p2D1.Y = p2D2.Y = steelAttr.H / 2;
                                    p2D1 = new Point3D(center.X, center.Y);
                                    axis2D = Vector3D.AxisY;
                                    break;
                                case GD_STD.Enum.FACE.BACK:
                                case GD_STD.Enum.FACE.FRONT:
                                    //p3D1 = new Point3D(center.X, center.Y, steelAttr.W / 2);//+ steelAttr.W / 2
                                    //p3D2 = new Point3D(center.X, 0, steelAttr.W / 2);//+ steelAttr.W / 2
                                    //p3D3 = new Point3D(center.X, center.Y, center.Z+ steelAttr.W / 2);//+ steelAttr.W / 2
                                    //以Back及FRONT來說，Z=Y,Y=Z
                                    p3D1 = new Point3D { X = p31.X,Z = p31.Y, Y = p31.Z + steelAttr.W / 2 };//+ steelAttr.W / 2
                                    p3D2 = new Point3D { X = p32.X,Z = p32.Y, Y = p32.Z + steelAttr.W / 2 };//+ steelAttr.W / 2
                                    p3D3 = new Point3D { X = p33.X, Z = p33.Y, Y = p33.Z + steelAttr.W / 2 };// + steelAttr.W / 2
                                    axis3D = Vector3D.AxisX;

                                    axis2D = Vector3D.AxisX;
                                    switch (face)
                                    {
                                        case FACE.FRONT:
                                            //axis3D = new Vector3D(0, 0, 0);
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + steelAttr.W / 2;
                                            //p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront + (steelAttr.W / 2 - steelAttr.t1 / 2);
                                            break;
                                        case FACE.BACK:
                                            p2D1.Y = p2D2.Y = bolts2DBlock.MoveBack - steelAttr.W / 2;
                                            //p2D1.Y = p2D2.Y = bolts2DBlock.MoveFront - (steelAttr.W / 2 - steelAttr.t1 / 2);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            //清除要鏡射的物件
                            model.Entities.Clear();
                            drawing.Entities.Clear();

                            //Vector3D axis3DX = new Vector3D();
                            Vector3D axis2DX = new Vector3D();

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

                            Plane mirror3DPlane = new Plane(center, axis3D);
                            Plane mirror2DPlane = new Plane(center, axis2D);

                            //鏡像轉換。
                            Mirror mirror3D = new Mirror(mirror3DPlane);
                            Mirror mirror2D = new Mirror(mirror2DPlane);

                            List<Entity> modify3D = new List<Entity>(), modify2D = new List<Entity>();
                            foreach (Entity item in buffer3D)
                            {
                                //Entity entity = (Entity)item.Clone();
                                ////entity.Mirror(axis3D, center, new Point3D() { X = center.X, Y = 0, Z = 0 });
                                //entity.TransformBy(mirror3D);
                                //modify3D.Add(entity);
                                Entity entity = (Entity)item.Clone();
                                //entity.Mirror(axis3D, p1, p2);
                                ////暫時註解 轉換世界座標
                                //switch (face)
                                //{
                                //    case FACE.TOP:
                                //        break;
                                //    case FACE.FRONT:
                                //    case FACE.BACK:
                                //        double y = 0, z = 0;
                                //        var vt = entity.Vertices;
                                //        vt.ForEach(v =>
                                //        {
                                //            y = v.Z;
                                //            z = v.Y;
                                //            v.Y = z;
                                //            v.Z = y;
                                //        });
                                //        entity.Vertices = vt;
                                //        break;
                                //    default:
                                //        break;
                                //}


                                entity.TransformBy(mirror3D);
                                modify3D.Add(entity);







                            }
                            foreach (var item in buffer2D)
                            {
                                //Entity entity = (Entity)item.Clone();
                                ////entity.Mirror(axis2D, center, new Point3D() { X = p2D2.X, Y = p2D2.Y, Z = 1 });
                                //entity.TransformBy(mirror2D);
                                //modify2D.Add(entity);
                                Entity entity = (Entity)item.Clone();
                                //entity.Mirror(axis2D, p1, p2);
                                entity.TransformBy(mirror2D);
                                modify2D.Add(entity);
                            }
                            drawing.Entities.AddRange(modify2D);
                            model.Entities.AddRange(modify3D);

                            #region 若無此段去更改EntityData的XYZ的話，切換零件時，2D顯示會打回異常

                            var baList = new List<BoltAttr>();
                            foreach (var et in model.Entities)
                            {
                                BoltAttr ba = (BoltAttr)et.EntityData;
                                Point3D ori = new Point3D() { X = ba.X, Y = ba.Y, Z = ba.Z };
                                ori.TransformBy(mirror3D);
                                ba.X = ori.X;
                                ba.Y = ori.Y;
                                ba.Z = ori.Z;
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
                                //MessageBox.Show("退出編輯模式，才可鏡射", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                                //MessageBox.Show("選擇類型必須是孔，才可鏡射", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            //刪除孔位
            ViewModel.DeleteHole = new RelayCommand(() =>
            {
                //開啟Model焦點
                bool mFocus = model.Focus();
                if (!mFocus)
                {
                    drawing.Focus();
                }
                SimulationDelete();
                SaveModel(false);
            });
            #endregion
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
            //SaveModel();
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
            else if (Keyboard.IsKeyDown(Key.Delete))
            {
                SimulationDelete();
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
            if (ViewModel.DataCorrespond.FindIndex(el => el.Number == ViewModel.SteelAttr.PartNumber) != -1)
            {
                WinUIMessageBox.Show(null,
                    $"重複編號",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return false;
            }
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
            List<(double,double)> AddBoltsList =new List<(double,double)>();

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
        /// 取消所有動作
        /// </summary>
        private void Esc()
        {
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
        }
        /// <summary>
        /// 存取模型
        /// </summary>
        public void SaveModel(bool add)
        {
            STDSerialization ser = new STDSerialization();
            ser.SetPartModel(ViewModel.SteelAttr.GUID.ToString(), model);
            //ser.SetPartModel(ViewModel.SteelAttr.GUID.ToString(), drawing);

            var ass = new GD_STD.Data.SteelAssembly()
            {
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
            // 零件列表
            SteelPart steelPart = new SteelPart(ViewModel.SteelAttr, ViewModel.SteelAttr.PartNumber, ViewModel.SteelAttr.Number, ViewModel.SteelAttr.GUID.Value);
            steelPart.ID = new List<int>();
            steelPart.Match = new List<bool>();
            steelPart.Material = ViewModel.SteelAttr.Material;
            steelPart.Father = ass.ID;
            steelPart.Length = ViewModel.SteelAttr.Length;

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
            collection.Add(steelPart);
            ser.SetPart($@"{steelPart.Profile.GetHashCode()}", new ObservableCollection<object>(collection));
            #endregion

            //if (add)
            //{
            ser.SetSteelAssemblies(ViewModel.SteelAssemblies);
            //}
            ViewModel.SaveDataCorrespond();
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

        private void TabControlSelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 1)
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
            model.ZoomFit();//設置道適合的視口
            model.Invalidate();//初始化模型
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
    }
}
