using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

namespace STD_105
{
    /// <summary>
    /// GraphWin.xaml 的互動邏輯
    /// </summary>
    public partial class GraphWin : Window
    {
        public ObSettingVM ViewModel { get; set; } = new ObSettingVM();
        /// <summary>
        /// nc 設定檔
        /// </summary>
        public NcTemp NcTemp { get; set; }
        public string DataPath { get; set; }
        public List<List<object>> DataList { get; set; }
        //public GraphWin()
        //{
        //    InitializeComponent();
        //    DataContext = ViewModel;
        //    model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //    drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //}
        //public GraphWin(NcTemp ncTemp)
        //{

        //    InitializeComponent();
        //    DataContext = ViewModel;
        //    model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //    drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //    NcTemp = ncTemp;
        //    drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
        //    model.Secondary = drawing;
        //    drawing.Secondary = model;
        //}
        //public GraphWin(string path)
        //{

        //    InitializeComponent();
        //    DataContext = ViewModel;
        //    model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //    drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
        //    DataPath = path;
        //    drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
        //    model.Secondary = drawing;
        //    drawing.Secondary = model;
        //}
        public GraphWin()
        {
            InitializeComponent();
            DataContext = ViewModel;
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.ActionMode = actionType.None;
            DataList = new List<List<object>>();
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            model.Secondary = drawing;
            drawing.Secondary = model;
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
            //model.Rotate.RotationCenter = rotationCenterType.CursorLocation;
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
        public void Draw()
        {
            bool hasOutSteel = false;
            if (NcTemp != null)
            {
                STDSerialization ser = new STDSerialization(); //序列化處理器
                Steel3DBlock.AddSteel(NcTemp.SteelAttr, model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊
                for (int i = 0; i < NcTemp.GroupBoltsAttrs.Count; i++) //逐步展開 nc 檔案的螺栓
                {
                    Bolts3DBlock bolts = Bolts3DBlock.AddBolts(NcTemp.GroupBoltsAttrs[i], model, out BlockReference botsBlock,out bool check); //加入到 3d 視圖
                    if (bolts.hasOutSteel)
                    {
                        hasOutSteel = true;
                    }
                    Add2DHole((Bolts3DBlock)model.Blocks[botsBlock.BlockName], false);//加入孔位不刷新 2d 視圖
                }
                if (hasOutSteel)
                {
                    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
                }
                ser.SetPartModel(NcTemp.SteelAttr.GUID.ToString(), model);//儲存 3d 視圖
            }
            else if (DataList != null)
            {
                dr.Visibility = Visibility.Collapsed;
                ReadFile readFile = new ReadFile($@"31c18603-88cc-47ce-8654-2a2bf0400e7e.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                readFile.DoWork();//開始工作
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                SteelAttr steel = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
                SteelAttr steelAttr = (SteelAttr)steel.Clone();
                steelAttr.Length = 13;
                Mesh mesh1 = Steel3DBlock.GetProfile(steelAttr);
                mesh1.Translate(-13, 0);
                model.Entities.Add(mesh1, Color.Orange);
                double le = 3252 - steel.Length + 13;
                SteelAttr attr = (SteelAttr)steelAttr.Clone();
                attr.Length = le;
                Mesh mesh2 = Steel3DBlock.GetProfile(attr);
                mesh2.Translate(steel.Length, 0);
                model.Entities.Add(mesh2, Color.Orange);
                for (int i = 0; i < model.Entities.Count; i++)
                {
                    model.Entities[i].Selectable = false; 
                }
            }
            else if (DataPath != null)
            {
                ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{DataPath}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                readFile.DoWork();//開始工作
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
                ViewModel.GetSteelAttr();
                model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
                for (int i = 0; i < model.Entities.Count; i++)//逐步展開 3d 模型實體
                {
                    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                    {
                        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                        Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
                        Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                    }
                }
            }
            model.Refresh();
            model.ZoomFit();//設置道適合的視口
            model.Invalidate();//初始化模型
            drawing.ZoomFit();//設置道適合的視口
            drawing.Invalidate();
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
                //MessageBox.Show("請輸入零件編號", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                //MessageBox.Show("長度不可以小於等於 0", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                //MessageBox.Show("數量不可以小於等於 0", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
                //MessageBox.Show("重複編號", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            model.SetCurrent(null);//層級 To 要編輯的BlockReference
        }
        /// <summary>
        /// 存取模型
        /// </summary>
        public void SaveModel()
        {
            //WriteFile writeFile = new WriteFile(new WriteFileParams(model)  //產生序列化檔案
            //{
            //    Content = devDept.Serialization.contentType.GeometryAndTessellation,
            //    SerializationMode = devDept.Serialization.serializationType.WithLengthPrefix,
            //    SelectedOnly = false,
            //    Purge = true
            //}, $@"{ApplicationVM.DirectoryDevPart()}\{ViewModel.SteelAttr.GUID.ToString()}.dm", new FileSerializerExt());
            //writeFile.DoWork();//存取檔案
            STDSerialization ser = new STDSerialization();
            ser.SetPartModel(ViewModel.SteelAttr.GUID.ToString(), model);
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
            ModelExt modelExt;
            Dim(out modelExt);
            modelExt.drawingAngularDim = true;
        }
        /// <summary>
        /// 線性標註
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinearDim(object sender, EventArgs e)
        {
            //#if DEBUG
            //            log4net.LogManager.GetLogger("觸發線性標註").Debug("");
            //#endif
            //            ModelExt modelExt = null;

            //            if (tabControl.SelectedIndex == 0)
            //            {
            //                modelExt = model;
            //            }
            //            else
            //            {
            //                modelExt = drawing;
            //            }
            //            try
            //            {
            //                if (model.Entities.Count > 0)
            //                {
            //#if DEBUG
            //                    log4net.LogManager.GetLogger("層級 To 要主件的BlockReference").Debug("成功");
            //#endif
            //                    modelExt.Entities[0].Selectable = true;
            //                    modelExt.ClearAllPreviousCommandData();
            //                    modelExt.ActionMode = actionType.None;
            //                    modelExt.objectSnapEnabled = true;
            //                    modelExt.drawingLinearDim = true;
            //                    return;
            //                }
            //#if DEBUG
            //                else
            //                {
            //                    throw new Exception("層級 To 主件的BlockReference 失敗，找不到主件");
            //                }
            //#endif

            //            }
            //            catch (Exception ex)
            //            {
            //                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
            //                Debugger.Break();
            //            }
#if DEBUG
            log4net.LogManager.GetLogger("觸發線性標註").Debug("");
#endif
            ModelExt modelExt;
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
                    modelExt.Entities.ForEach(el =>
                    {
                        if (el.EntityData is SteelAttr)
                        {
                            el.Selectable = true;
                        }
                    });
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

        public BlockReference SteelTriangulation(Mesh mesh)
        {
#if DEBUG
            log4net.LogManager.GetLogger("產生2D").Debug("開始");
#endif
            drawing.Blocks.Clear();
            drawing.Entities.Clear();

            Steel2DBlock steel2DBlock = new Steel2DBlock(mesh, model.Blocks[1].Name);
            drawing.Blocks.Add(steel2DBlock);
            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊

            //關閉三視圖用戶選擇
            block2D.Selectable = false;

            drawing.Entities.Add(block2D);
#if DEBUG
            log4net.LogManager.GetLogger("產生2D").Debug("結束");
#endif
            //drawing.ZoomFit();//設置道適合的視口
            //drawing.Refresh();//刷新模型
            return block2D;
        }
        /// <summary>
        /// 加入2d 孔位
        /// </summary>
        /// <param name="bolts"></param>
        /// <param name="refresh">刷新模型</param>
        /// <returns></returns>
        public BlockReference Add2DHole(Bolts3DBlock bolts, bool refresh = true)
        {
            /*2D螺栓*/
            BlockReference referenceMain = (BlockReference)drawing.Entities[drawing.Entities.Count - 1]; //主件圖形
            Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[referenceMain.BlockName]; //取得鋼構圖塊

            string blockName = string.Empty; //圖塊名稱
            Bolts2DBlock bolts2DBlock = new Bolts2DBlock(bolts, steel2DBlock); //產生螺栓圖塊
            drawing.Blocks.Add(bolts2DBlock); //加入螺栓圖塊
            blockName = bolts2DBlock.Name;
            BlockReference result = new BlockReference(0, 0, 0, bolts2DBlock.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
            drawing.Entities.Insert(0, result);
            if (refresh)
            {
                drawing.Refresh();//刷新模型
            }

            return result;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TreeView treeView = (TreeView)sender; //樹壯列表
            //IModelData data = (IModelData)treeView.SelectedValue;
            //if (data.DataName == null)
            //    return;
            //STDSerialization ser = new STDSerialization();
            //NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            //NcTemp ncTemp = ncTemps.GetData(data.DataName);//需要實體化的nc物件
            //model.Clear(); //清除目前模型
            //if (ncTemp == null) //NC 檔案是空值
            //{
            //    ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{data.DataName}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
            //    readFile.DoWork();//開始工作
            //    readFile.AddToScene(model);//將讀取完的檔案放入到模型
            //    ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            //    ViewModel.GetSteelAttr();
            //    model.Blocks[1] = new Steel3DBlock((Mesh)model.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式
            //    SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D圖塊
            //    for (int i = 0; i < model.Entities.Count; i++)//逐步展開 3d 模型實體
            //    {
            //        if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
            //        {
            //            BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
            //            Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
            //            Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生螺栓圖塊
            //            Add2DHole(bolts3DBlock, false);//加入孔位不刷新 2d 視圖
            //        }
            //    }
            //}
            //else //如果需要載入 nc 設定檔
            //{
            //    Steel3DBlock.AddSteel(ncTemp.SteelAttr, model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
            //    SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊
            //    for (int i = 0; i < ncTemp.GroupBoltsAttrs.Count; i++) //逐步展開 nc 檔案的螺栓
            //    {
            //        Bolts3DBlock.AddBolts(ncTemp.GroupBoltsAttrs[i], model, out BlockReference botsBlock); //加入到 3d 視圖
            //        Add2DHole((Bolts3DBlock)model.Blocks[botsBlock.BlockName], false);//加入孔位不刷新 2d 視圖
            //    }
            //    ser.SetNcTempList(ncTemps);//儲存檔案
            //    ser.SetPartModel(ncTemp.SteelAttr.GUID.ToString(), model);//儲存 3d 視圖

            //}
            //model.ZoomFit();//設置道適合的視口
            //model.Invalidate();//初始化模型
            //drawing.ZoomFit();//設置道適合的視口
            //drawing.Invalidate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.Dispose();//釋放資源
            drawing.Dispose();//釋放資源
            drawing.Loaded -= drawing_Loaded;
            GC.Collect();
        }
    }
}
