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
using System.Windows.Controls.Primitives;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Core.Native;
using static DevExpress.XtraEditors.Mask.MaskSettings;
using System.Web.UI.WebControls;
using System.Windows.Media;
using Color = System.Drawing.Color;
using static DevExpress.Utils.Menu.DXMenuItemPainter;
using DevExpress.Utils.Serializing;
using DevExpress.Pdf.ContentGeneration;
using System.Web.UI.WebControls.WebParts;
using DevExpress.XtraToolbox;
//using TriangleNet;

namespace STD_105.Office
{
    /// <summary>
    /// TypesettingsSetting.xaml 的互動邏輯
    /// </summary>
    public partial class TypesettingsSetting : BasePage<OfficeTypeSettingVM>
    {
        public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.Create(() => new WaitIndicator(), new DevExpress.Mvvm.DXSplashScreenViewModel { });

        public bool bResult;
       public ObSettingVM ObViewModel { get; set; } = new ObSettingVM();
        /// <summary>
        /// nc 設定檔
        /// </summary>
        public NcTemp NcTemp { get; set; }
        public string DataPath { get; set; }
        public List<List<object>> DataList { get; set; }

        /// <summary>
        /// 220818 蘇冠綸 排版設定版面完成
        /// </summary>
        public TypesettingsSetting()
        {
            InitializeComponent();

            model.DataContext = ObViewModel;
            drawing.DataContext = ObViewModel;
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.ActionMode = actionType.None;
            DataList = new List<List<object>>();
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            model.Secondary = drawing;
            drawing.Secondary = model;
            //ControlDraw3D();
            CheckReportLogoExist();
        }

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
            if (ObViewModel.Reductions == null)
            {
                ObViewModel.Reductions = new ReductionList(model, drawing); //紀錄使用找操作
            }

            #endregion
        }




        public void Draw()
        {
            if (NcTemp != null)
            {
                STDSerialization ser = new STDSerialization(); //序列化處理器
                
                Steel3DBlock.AddSteel(NcTemp.SteelAttr, model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
                SteelTriangulation((Mesh)model.Blocks[1].Entities[0]);//產生2D參考圖塊
                for (int i = 0; i < NcTemp.GroupBoltsAttrs.Count; i++) //逐步展開 nc 檔案的螺栓
                {
                    Bolts3DBlock B3DB = Bolts3DBlock.AddBolts(NcTemp.GroupBoltsAttrs[i], model, out BlockReference botsBlock, out bool check); //加入到 3d 視圖
                    if (!B3DB.hasOutSteel)
                    {
                        Add2DHole((Bolts3DBlock)model.Blocks[botsBlock.BlockName], false);//加入孔位不刷新 2d 視圖
                    }                    
                }
                ser.SetPartModel(NcTemp.SteelAttr.GUID.ToString(), model);//儲存 3d 視圖
            }
            else if (DataList != null)
            {
                model.Entities.Clear();
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
                model.Entities.Clear();
                ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{DataPath}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                readFile.DoWork();//開始工作
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                ObViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
                ObViewModel.GetSteelAttr();
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
            ////模擬鍵盤按下Delete
            //var c = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Delete)
            //{
            //    RoutedEvent = Keyboard.KeyDownEvent
            //};
            //InputManager.Current.ProcessInput(c);
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
            if (string.IsNullOrEmpty(ObViewModel.SteelAttr.PartNumber))//檢測用戶是否有輸入零件編號
            {
                //MessageBox.Show("請輸入零件編號", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            if (ObViewModel.SteelAttr.Length <= 0)//檢測用戶長度是否有大於0
            {
                //MessageBox.Show("長度不可以小於等於 0", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            if (ObViewModel.SteelAttr.Number <= 0) //檢測用戶是否零件數量大於0
            {
                //MessageBox.Show("數量不可以小於等於 0", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
            if (ObViewModel.DataCorrespond.FindIndex(el => el.Number == ObViewModel.SteelAttr.PartNumber) != -1)
            {
                //MessageBox.Show("重複編號", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"重複編號",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
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
            if (ObViewModel.Select3DItem.Count >= 1 && ObViewModel.Select3DItem[0].Item is BlockReference)
            {
#if DEBUG
                log4net.LogManager.GetLogger("啟用").Debug("刪除功能");
#endif
                //開啟取消功能
                esc.Visibility = Visibility.Visible;
                delete2D.Visibility = Visibility.Visible;
                esc2D.Visibility = Visibility.Visible;
            }
            //開啟編輯功能
            if (ObViewModel.Select3DItem.Count == 1 && ObViewModel.Select3DItem[0].Item is BlockReference)
            {
#if DEBUG
                log4net.LogManager.GetLogger("啟用").Debug("編輯功能");
#endif
                edit2D.Visibility = Visibility.Visible;
            }
            //關閉刪除功能與編輯功能
            if (ObViewModel.Select3DItem.Count == 0)
            {
#if DEBUG
                log4net.LogManager.GetLogger("關閉").Debug("編輯功能、刪除功能、取消功能");
#endif
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
            ObViewModel.Select3DItem.Clear();
            ObViewModel.tem3DRecycle.Clear();
            ObViewModel.Select2DItem.Clear();
            ObViewModel.tem2DRecycle.Clear();
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
            //}, $@"{ApplicationVM.DirectoryDevPart()}\{ObViewModel.SteelAttr.GUID.ToString()}.dm", new FileSerializerExt());
            //writeFile.DoWork();//存取檔案
            STDSerialization ser = new STDSerialization();
            ser.SetPartModel(ObViewModel.SteelAttr.GUID.ToString(), model);
            ObViewModel.SaveDataCorrespond();
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
            if (DrawTabControl.SelectedIndex == 0)
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
                    modelExt.ClearAllPreviousCommandData();
                    modelExt.Entities.ForEach(el => el.Selectable = true);// = true;
                    modelExt.ActionMode = actionType.None;
                    modelExt.objectSnapEnabled = true;
                    return;
                }
                //if (modelExt.Entities.Count > 0)
                //{
                //    modelExt.Entities.ForEach(el =>
                //    {
                //        if (el.EntityData is SteelAttr)
                //        {
                //            el.Selectable = true;
                //        }
                //    });
                //    modelExt.ClearAllPreviousCommandData();
                //    modelExt.ActionMode = actionType.None;
                //    modelExt.objectSnapEnabled = true;
                //    return;
                //}
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
            if (DrawTabControl.SelectedIndex == 1)
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

            // 將TOP FRONT BACK圖塊加入drawing
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

        bool TableViewLoadedBoolen = false;
        private void Material_List_TableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
            TableViewLoadedBoolen = true;
        }


        private void Material_List_GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (TableViewLoadedBoolen == false)
                return;

            //無選擇任何物件 直接跳過不處理
            if (e.NewItem == null)
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

        private void TableView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlDraw3D();
        }
        private void ControlDraw3D()
        {
            var SelectedData = (GD_STD.Data.MaterialDataView)Material_List_GridControl.SelectedItem;
          //未選擇目標 不畫圖
            if (SelectedData == null)
                return;


            model.ActionMode = actionType.SelectByBox;
            string content = SelectedData.MaterialNumber; //素材編號

            
            model.AssemblyPart(content);
            AssemblyPart2D(model,content);

            //drawing.Entities.Regen();
            //model.Entities.Regen();


            model.ZoomFit();        //設置道適合的視口
            drawing.ZoomFit();      //設置道適合的視口

            drawing.Refresh();
            model.Refresh();


            //model.Invalidate();     //初始化模型
            //drawing.Invalidate();   //初始化模型


            //SaveModel();
            // model.SelectionChanged -= model.Model_SelectionChanged;
            // model.SelectionChanged += model.Model_SelectionChanged; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="materialNumber"></param>
        public void AssemblyPart2D(devDept.Eyeshot.Model model,string materialNumber)
        {
            //ObSettingVM obvm = new ObSettingVM();
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            int index = materialDataViews.FindIndex(el => el.MaterialNumber == materialNumber);//序列化的列表索引
            MaterialDataView material = materialDataViews[index];
            ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            var guid = (from el in ncTemps
                        where _.ToList().Contains(el.SteelAttr.PartNumber)
                        select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            //產生nc檔案圖檔
            for (int i = 0; i < guid.Count; i++)
            {
                model.LoadNcToModel(guid[i], ObSettingVM.allowType);
            }


            var place = new List<(double Start, double End, bool IsCut, string Number)>();//放置位置參數
            place.Add((Start: 0, End: material.StartCut, IsCut: true, Number: "")); //素材起始切割物件
            Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            for (int i = 0; i < material.Parts.Count; i++)
            {
                int partIndex = parts.FindIndex(el => el.Number == material.Parts[i].PartNumber); //回傳要使用的陣列位置
                if (partIndex == -1)
                {
                    // 未找到對應零件編號
                    //string tmp = material.Parts[i].PartNumber;
                    //WinUIMessageBox.Show(null,
                    //$"未找到對應零件編號" + tmp,
                    //"通知",
                    //MessageBoxButton.OK,
                    //MessageBoxImage.Exclamation,
                    //MessageBoxResult.None,
                    //MessageBoxOptions.None,
                    // FloatingMode.Window);
                    return;
                  // throw new Exception($"在 ObservableCollection<SteelPart> 找不到 {material.Parts[i].PartNumber}");
                }
                else
                {
                    double startCurrent = place[place.Count - 1].End,//當前物件放置起始點的座標
                                  endCurrent = startCurrent + parts[partIndex].Length;//當前物件放置結束點的座標
                    place.Add((Start: startCurrent, End: endCurrent, IsCut: false, Number: parts[partIndex].Number));
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                    //計算切割物件
                    double startCut = place[place.Count - 1].End, //當前切割物件放置起始點的座標
                                  endCut;//當前切割物件放置結束點的座標
                    if (i + 1 >= material.Parts.Count) //下一次迴圈結束
                    {
                        //endCut = material.LengthStr + material.StartCut + material.EndCut;//當前切割物件放置結束點的座標
                        endCut = material.LengthStr;// - material.StartCut - material.EndCut;//當前切割物件放置結束點的座標
                    }
                    else //下一次迴圈尚未結束
                    {
                        endCut = startCut + material.Cut;//當前切割物件放置結束點的座標
                    }
                    place.Add((Start: startCut, End: endCut, IsCut: true, Number: "")); //素材零件位置
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                }
            }

            drawing.Clear();

            EntityList entities = new EntityList();

            bool findsteel = false;

            // 一支素材裡的第i之零件
            for (int i = 0; i < place.Count; i++)
            {

                if (place.Count == 1) // count=1表素材沒有零件組成,只有(前端切除)程序紀錄
                {
                    return;
                }

                if (place[i].IsCut) //如果是切割物件
                {
                    Entity cut1 = Draw2DCutMesh(parts[0], model, place[i].Start, place[i].End, "Cut"+ i);
                    if (cut1 != null)
                    {
                        entities.Add(cut1);
                    }

                    continue;
                }


                int placeIndex = place.FindIndex(el => el.Number == place[i].Number); //如果有重複的編號只會回傳第一個，以這個下去做比較。

                if (placeIndex != i) //查表,第二次出現相同零件 ,不可再新增BLOCK(會顯示已有相同BLOCK錯誤), 直接由entity複製一份做位移.
                {
                    EntityList ent = new EntityList();
                    entities.
                        Where(el => el.GroupIndex == placeIndex).
                        ForEach(el =>
                        {
                            Entity copy = (Entity)el.Clone(); //複製物件
                            copy.GroupIndex = i;
                            copy.Translate(place[i].Start - place[placeIndex].Start, 0);
                            ent.Add(copy);
                        });
                    entities.AddRange(ent);
                }

                else  // 如果第一次出現零件,由3D model 建立2D Block與 Entities
                {
                    int partIndex = parts.FindIndex(el => el.Number == place[i].Number);// 取得欲使用之第i之零件
                    findsteel = false;


                    if (parts[partIndex].GUID.ToString() != "") //如果圖面檔案
                    {
                        int FindSteelBlockIndex = model.Blocks.FindIndex(el => el.Name == parts[partIndex].GUID.ToString()); // 由model block裡 找尋對應零件位置

                        for (int searchboltindex = FindSteelBlockIndex; searchboltindex < model.Blocks.Count; searchboltindex++)
                        {
                            //沒找到零件
                            if(searchboltindex ==-1)
                            {
                                findsteel = false;
                                break;
                            }

                            if (model.Blocks[searchboltindex].Entities[0].EntityData is SteelAttr && findsteel)
                            {
                                findsteel = false;
                                break;
                            }
                            

                            if (model.Blocks[searchboltindex].Entities[0].EntityData is SteelAttr && !findsteel)
                            {
                                Steel2DBlock steel2DBlock = new Steel2DBlock((devDept.Eyeshot.Entities.Mesh)model.Blocks[FindSteelBlockIndex].Entities[0], model.Blocks[FindSteelBlockIndex].Name);   // 產生2D
                                drawing.Blocks.Add(steel2DBlock);
                                BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊


                                block2D.Selectable = false;
                                block2D.GroupIndex = i;
                                block2D.Translate(place[i].Start, 0);
                                block2D.Selectable = false;
                                entities.Add(block2D);//加入到暫存列表

                                findsteel = true;
                            }

                            if ((model.Blocks[searchboltindex].Entities[0].EntityData is BoltAttr) && findsteel )
                            {

                                Steel2DBlock steel2DBlock = new Steel2DBlock((devDept.Eyeshot.Entities.Mesh)model.Blocks[FindSteelBlockIndex].Entities[0], model.Blocks[FindSteelBlockIndex].Name);   // 產生2D

                                int icount;
                                for (icount = 0; icount < model.Entities.Count; icount++)
                                {
                                    BlockReference aa = (BlockReference)model.Entities[icount]; //取得參考圖塊
                                    Block bb = model.Blocks[aa.BlockName]; //取得圖塊 
                                    if (model.Blocks[searchboltindex].Name==bb.Name)
                                    {
                                        break;
                                    }
                                }

                                BlockReference blockReference = (BlockReference)model.Entities[icount]; //取得參考圖塊
                                Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                                
                                Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生3D螺栓圖塊
                                Bolts2DBlock bolts2DBlock = new Bolts2DBlock(bolts3DBlock, steel2DBlock); //產生2D螺栓圖塊

                                int tmpindex=drawing.Blocks.FindIndex(x => x.Name == bolts2DBlock.Name);
                                if (tmpindex ==-1)
                                drawing.Blocks.Add(bolts2DBlock); //加入螺栓圖塊

                                BlockReference block2D = new BlockReference(0, 0, 0, bolts2DBlock.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊

                                block2D.Selectable = false;
                                block2D.GroupIndex = i;
                                block2D.Translate(place[i].Start, 0);
                                block2D.Selectable = false;
                                entities.Add(block2D);//加入到暫存列表
                            }
                        }
                    }
                }
            }
            drawing.Entities.Clear();
            drawing.Entities.AddRange(entities);
            //ser.SetMaterialModel(materialNumber + "2D", drawing); //儲存素材



            #region 備份
            //STDSerialization ser = new STDSerialization(); //序列化處理器
            //ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            //int index = materialDataViews.FindIndex(el => el.MaterialNumber == materialNumber);//序列化的列表索引
            //MaterialDataView material = materialDataViews[index];
            //ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            //NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            //var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            //var guid = (from el in ncTemps
            //            where _.ToList().Contains(el.SteelAttr.PartNumber)
            //            select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            ////產生nc檔案圖檔
            //for (int i = 0; i < guid.Count; i++)
            //{
            //    model.LoadNcToModel(guid[i]);
            //}


            //var place = new List<(double Start, double End, bool IsCut, string Number)>();//放置位置參數
            //place.Add((Start: 0, End: material.StartCut, IsCut: true, Number: "")); //素材起始切割物件
            //Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            //for (int i = 0; i < material.Parts.Count; i++)
            //{
            //    int partIndex = parts.FindIndex(el => el.Number == material.Parts[i].PartNumber); //回傳要使用的陣列位置
            //    if (partIndex == -1)
            //    {
            //        throw new Exception($"在 ObservableCollection<SteelPart> 找不到 {material.Parts[i].PartNumber}");
            //    }
            //    else
            //    {
            //        double startCurrent = place[place.Count - 1].End,//當前物件放置起始點的座標
            //                      endCurrent = startCurrent + parts[partIndex].Length;//當前物件放置結束點的座標
            //        place.Add((Start: startCurrent, End: endCurrent, IsCut: false, Number: parts[partIndex].Number));
            //        Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            //        //計算切割物件
            //        double startCut = place[place.Count - 1].End, //當前切割物件放置起始點的座標
            //                      endCut;//當前切割物件放置結束點的座標
            //        if (i + 1 >= material.Parts.Count) //下一次迴圈結束
            //        {
            //            endCut = material.LengthStr + material.StartCut + material.EndCut;//當前切割物件放置結束點的座標
            //        }
            //        else //下一次迴圈尚未結束
            //        {
            //            endCut = startCut + material.Cut;//當前切割物件放置結束點的座標
            //        }
            //        place.Add((Start: startCut, End: endCut, IsCut: true, Number: "")); //素材零件位置
            //        Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            //    }
            //}

            //drawing.Clear();

            //EntityList entities = new EntityList();

            //for (int i = 0; i < place.Count; i++)
            //{
            //    if (place[i].IsCut) //如果是切割物件
            //    {
            //        //Entity cut = DrawCutMesh(parts[0], model, place[i].Start, place[i].End, "Cut");
            //        //if (cut != null)
            //        //{
            //        //    entities.Add(cut);
            //        //}

            //        continue;
            //    }


            //    int placeIndex = place.FindIndex(el => el.Number == place[i].Number); //如果有重複的編號只會回傳第一個，以這個下去做比較。

            //    if (placeIndex != i) //如果 i != 第一次出現的 index 代表需要使用複製
            //    {
            //        EntityList ent = new EntityList();
            //        entities.
            //            Where(el => el.GroupIndex == placeIndex).
            //            ForEach(el =>
            //            {
            //                Entity copy = (Entity)el.Clone(); //複製物件
            //                copy.GroupIndex = i;
            //                copy.Translate(place[i].Start - place[placeIndex].Start, 0);
            //                ent.Add(copy);
            //            });
            //        entities.AddRange(ent);
            //    }

            //    else
            //    {
            //        int partIndex = parts.FindIndex(el => el.Number == place[i].Number);

            //        if (parts[partIndex].GUID.ToString() != "") //如果圖面檔案
            //        {
            //            int FindSameSteelBlockIndex = model.Blocks.FindIndex(el => el.Name == parts[partIndex].GUID.ToString());

            //            Steel2DBlock steel2DBlock = new Steel2DBlock((devDept.Eyeshot.Entities.Mesh)model.Blocks[FindSameSteelBlockIndex].Entities[0], model.Blocks[FindSameSteelBlockIndex].Name);
            //            drawing.Blocks.Add(steel2DBlock);
            //            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊

            //            //關閉三視圖用戶選擇
            //            block2D.Selectable = false;
            //            block2D.GroupIndex = i;
            //            block2D.Translate(place[i].Start, 0);
            //            block2D.Selectable = false;
            //            entities.Add(block2D);//加入到暫存列表
            //        }
            //    }
            //}
            //drawing.Entities.Clear();
            //drawing.Entities.AddRange(entities);
            //ser.SetMaterialModel(materialNumber + "2D", drawing); //儲存素材
            #endregion

        }
        /// <summary>
        /// 繪製3D餘料形狀
        /// </summary>
        /// <param name="part">零件類別</param>
        /// <param name="model">3D模型類別</param>
        /// <param name="end">結束位置</param>
        /// <param name="start">開始位置</param>
        /// <param name="dic">判斷載入名稱 顯示不同顏色</param>
        /// <returns></returns>
        private Entity Draw2DCutMesh(SteelPart part, devDept.Eyeshot.Model model, double end, double start, string dic)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;

            if (steelAttr.Length == 0)
            {
                return null;
            }

            Steel2DBlock steel2DBlock = new Steel2DBlock(Steel3DBlock.GetProfile(steelAttr), dic);   // 產生2D
            drawing.Blocks.Add(steel2DBlock);
            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊
            block2D.Translate(start, 0);
            block2D.Selectable = false;

            return block2D;
        }

        private void GridSplitter_MouseMove(object sender, MouseEventArgs e)
        {
            model.ZoomFit();//設置道適合的視口
            drawing.ZoomFit();//設置道適合的視口
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {                                         
            model.ActionMode = actionType.SelectByBox;

            for (int i = 0; i < model.Entities.Count; i++)
            {
                model.Entities[i].Selectable = true;
            }

            //  SaveModel();
        }

        private void PartListTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;

            //最左側IndicatorWidth自動變寬
            ((DevExpress.Xpf.Grid.TableView)sender).IndicatorWidth = (((PartsGridControl.ItemsSource as System.Collections.ICollection).Count + 1).ToString().Length * 10) + 5;
        }
        private void SoftCountTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
        }

        
        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == PartListTableView.Name)
            {
                IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(SoftCountTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                SoftCountTableView_ScrollElement?.SetVerticalOffset(e.VerticalOffset);
            }
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == SoftCountTableView.Name)
            {
                IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(PartListTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                PartsTableView_ScrollElement?.SetVerticalOffset(e.VerticalOffset);
            }
        }







        private void CheckReportLogoExist()
        {
            string dirPath = ApplicationVM.FileReportLogo();
            string FilePath = ApplicationVM.FileReportLogo() + @"\ReportLogo.png";
            string startup_path = System.AppDomain.CurrentDomain.BaseDirectory;
            string GDLOGOPath = $@"{startup_path}\Logo\GD_ReportLogo.png";
            if (Directory.Exists(dirPath))
            {
                Console.WriteLine("The directory {0} exists.", dirPath);
            }
            else
            {
                Directory.CreateDirectory(dirPath);
                Console.WriteLine("The directory {0} was created.", dirPath);
            }

            try
            {
                if (!System.IO.File.Exists(FilePath))
                {
                    File.Copy(GDLOGOPath, FilePath, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //加入新零件
        private void InsertPartCommandClick(object sender, RoutedEventArgs e)
        {
            var SelectedMaterial = Material_List_GridControl.SelectedItem as GD_STD.Data.MaterialDataView;
            //取出相同斷面規格
            var ITEM_Profile = (this.PartsGridControl.ItemsSource as IEnumerable<GD_STD.Data.TypeSettingDataView>).ToList().FindAll(x => (x.Profile == SelectedMaterial.Profile));
            var IPW = new InsertPartsWin(Material_List_GridControl);
            foreach(var item in  ITEM_Profile)
           {
                item.SortCount = 0;
            }

            IPW.PartsGridControl.ItemsSource = ITEM_Profile;
            IPW.SoftGridControl.ItemsSource = ITEM_Profile;

            var Win = new Window
            {
                Title = "素材新增零件",
                ShowInTaskbar = false,
                Content = IPW,
                MinWidth =900,
                MinHeight = 550,
                Width=900,
                Height= 550,
                Padding = new Thickness(0),
                
            };

            Win.ShowDialog();

            Win.Close();

            //變更後將Dev_Material/*.dm檔移除 以防錯誤

            STDSerialization ser = new STDSerialization(); //序列化處理器
            ser.DeleteMaterialModel(SelectedMaterial.MaterialNumber);


            Material_List_GridControl.RefreshData();
            ScreenManager.ViewModel.Status = "加入零件中...";
            ScreenManager.Show(inputBlock: InputBlockMode.None, timeout: 100);
            ReloadMaterialGrid();

            ScreenManager.ViewModel.Status = "完成...";
            System.Threading.Thread.Sleep(100);
            ScreenManager.Close();


        }

        private void DeletePartButtonClick(object sender, RoutedEventArgs e)
        {
            
            var SelectedDataView = Material_List_GridControl.SelectedItem as GD_STD.Data.MaterialDataView;

            //ComponentGridControl
            if (SelectedDataView.Parts.Count == 0)
            {
                WinUIMessageBox.Show(null,
                    $"本素材不存在可刪除之零件，將會刪除本素材",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
            }
            else if(SelectedDataView.Parts.Count==1)
            {
                SelectedDataView.SelectedPart = SelectedDataView.Parts[0];
            }
            else if (SelectedDataView.SelectedPart == null)
            {
                WinUIMessageBox.Show(null,
                    $"需選擇要刪除之零件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return;
            }

            var MessageBoxReturn = MessageBoxResult.None;
            if (SelectedDataView.Parts.Count != 0 )
            {
                MessageBoxReturn = WinUIMessageBox.Show(null,
                        $"是否要刪除素材編號:{SelectedDataView.MaterialNumber}內的零件：\r\n" +
                        $"構件編號：{SelectedDataView.SelectedPart.AssemblyNumber}\r\n" +
                        $"零件編號：{SelectedDataView.SelectedPart.PartNumber}\r\n" +
                        $"按下「YES」會立即刪除",
                        "通知",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);
            }
            else
            {
                MessageBoxReturn = WinUIMessageBox.Show(null,
                    $"是否要刪除素材編號:{SelectedDataView.MaterialNumber}\r\n按下「YES」會立即刪除",
                    "通知",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
            }

            if (MessageBoxReturn == MessageBoxResult.Yes)
            {
                STDSerialization ser = new STDSerialization(); //序列化處理器    
                //以下代碼在第二階段需要重構->需放到VM層及讀取方式變更
                //var OTS_VM = this. as WPFSTD105.OfficeTypeSettingVM;

                ObservableCollection<SteelPart> steelParts = ser.GetPart(SelectedDataView.Profile.GetHashCode().ToString());
                if (SelectedDataView.Parts.Count != 0)
                {
                    int index = ViewModel.DataViews.FindIndex(x => x == SelectedDataView.SelectedPart);
                    if (index != -1)
                    {
                        int m = ViewModel.DataViews[index].Match.FindLastIndex(x => x == false);
                        if (m != -1)
                        {
                            ViewModel.DataViews[index].Match[m] = true;
                            ViewModel.DataViews[index].Revise = DateTime.Now;
                        }
                    }

                    int steelIndex = steelParts.FindIndex(x => x.Number == SelectedDataView.SelectedPart.PartNumber);
                    if (steelIndex != -1)
                    {
                        int partMatch = steelParts[steelIndex].Match.FindLastIndex(x => x == false);
                        if (partMatch != -1)
                            steelParts[steelIndex].Match[partMatch] = true;
                    }

                    SelectedDataView.Parts.Remove(SelectedDataView.SelectedPart);

                    ser.SetPart(SelectedDataView.Profile.GetHashCode().ToString(), new ObservableCollection<object>(steelParts));
                }
                else
                {
                    //無零件時 刪除素材
                    ViewModel.MaterialDataViews.Remove(SelectedDataView);
                }
                //存檔
                ser.SetMaterialDataView(Material_List_GridControl.ItemsSource as ObservableCollection<MaterialDataView>);

                //變更後將Dev_Material/*.dm檔移除 以防錯誤
                ser.DeleteMaterialModel(SelectedDataView.MaterialNumber);

                ScreenManager.ViewModel.Status = "刪除零件中...";
                ScreenManager.Show(inputBlock: InputBlockMode.None, timeout: 100);
                ReloadMaterialGrid();
                ScreenManager.ViewModel.Status = "完成....";
                System.Threading.Thread.Sleep(100);
                ScreenManager.Close();
                WinUIMessageBox.Show(null,
                    $"刪除成功！",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);



            }
        }
            
       private void DeleteAllPartButtonClick(object sender, RoutedEventArgs e)
        {
            var SelectedDataView = Material_List_GridControl.SelectedItem as GD_STD.Data.MaterialDataView;

            if (SelectedDataView.Parts.Count == 0)
            {
                WinUIMessageBox.Show(null,
                    $"本素材不存在可刪除之零件",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                return;
            }

            var MessageBoxReturn = WinUIMessageBox.Show(null,
                        $"是否要刪除素材編號:{SelectedDataView.MaterialNumber}內的所有零件：\r\n" +
                        $"按下「YES」會立即刪除",
                        "通知",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                         FloatingMode.Window);

            if (MessageBoxReturn == MessageBoxResult.Yes)
            {
                STDSerialization ser = new STDSerialization(); //序列化處理器
                                          
                //需要重構
                //var OTS_VM = this. as WPFSTD105.OfficeTypeSettingVM;
                ObservableCollection<SteelPart> steelParts = ser.GetPart(SelectedDataView.Profile.GetHashCode().ToString());
                SelectedDataView.Parts.ForEach(DelPart =>
                {
                    int index = ViewModel.DataViews.FindIndex(x => x == DelPart);
                    if (index != -1)
                    {
                        int m = ViewModel.DataViews[index].Match.FindLastIndex(x => x == false);
                        if (m != -1)
                            ViewModel.DataViews[index].Match[m] = true;

                        ViewModel.DataViews[index].Revise = DateTime.Now;
                    }

                    int steelIndex = steelParts.FindIndex(x => x.Number == DelPart.PartNumber);
                    if (steelIndex != -1)
                    {
                        int partMatch = steelParts[steelIndex].Match.FindLastIndex(x => x == false);
                        if (partMatch != -1)
                            steelParts[steelIndex].Match[partMatch] = true;
                    }
                    ser.SetPart(SelectedDataView.Profile.GetHashCode().ToString(), new ObservableCollection<object>(steelParts));
                });
                SelectedDataView.Parts.Clear();
                //存檔
                ser.SetMaterialDataView(Material_List_GridControl.ItemsSource as ObservableCollection<MaterialDataView>);


                //變更後將Dev_Material/*.dm檔移除 以防錯誤
                ser.DeleteMaterialModel(SelectedDataView.MaterialNumber);

                ScreenManager.ViewModel.Status = "刪除零件中...";
                ScreenManager.Show(inputBlock: InputBlockMode.None, timeout: 100);
                ReloadMaterialGrid();
                ScreenManager.ViewModel.Status = "完成...";
                System.Threading.Thread.Sleep(100);
                ScreenManager.Close();
                WinUIMessageBox.Show(null,
                    $"刪除成功！",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);

            }

        }


        private void ReloadMaterialGrid()
        {
            //紀錄卷軸位置
            double Material_TableView_VerticalOffset = 0;
            IScrollInfo Material_TableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(Material_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
            if (Material_TableView_ScrollElement != null)
                Material_TableView_VerticalOffset = Material_TableView_ScrollElement.VerticalOffset;

            //記錄所有有展開的datarow
            var ExpandedList = new Dictionary<int, bool>();
            for (int i = 0; i < (Material_List_GridControl.ItemsSource as ObservableCollection<MaterialDataView>).Count; i++)
            {
                ExpandedList[i] = Material_List_GridControl.IsMasterRowExpanded(i);
            }
            var Selected = Material_List_GridControl.SelectedItem as MaterialDataView;
            var Handle = (Material_List_GridControl.ItemsSource as ObservableCollection<MaterialDataView>).FindIndex(x => (x.MaterialNumber == Selected.MaterialNumber));

            //var OTS_VM = this. as WPFSTD105.OfficeTypeSettingVM;
            ViewModel.MaterialDataViews.Clear();
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ViewModel.MaterialDataViews = ser.GetMaterialDataView();

            if (Handle != -1)
            {
                //素材區展開歸位
                Material_List_GridControl.Dispatcher.Invoke(() =>
                {
                    Material_List_GridControl.SelectedItem = (Material_List_GridControl.ItemsSource as ObservableCollection<MaterialDataView>).First(x=>(x.MaterialNumber == Selected.MaterialNumber));
                    foreach (var Exp in ExpandedList)
                    {
                        if (Exp.Value == true)
                        {
                            Material_List_GridControl.ExpandMasterRow(Exp.Key);
                        }
                        else
                        {
                            Material_List_GridControl.CollapseMasterRow(Exp.Key);
                        }
                    }

                });
                //素材區卷軸歸位
                Material_TableView.Dispatcher.Invoke(() =>
                {
                    Material_TableView_ScrollElement?.SetVerticalOffset(Material_TableView_VerticalOffset)   ;
                });

            }

            PartsGridControl.Dispatcher.Invoke(() =>
            {
                //↓不要這樣寫!會導致表格 icommand失靈！
                //PartsGridControl.ItemsSource = (~ as OfficeTypeSettingVM).LoadDataViews(); 
          
                for (int i = 0; i < (PartsGridControl.ItemsSource as ObservableCollection<TypeSettingDataView>).Count; i++)
                {
                    PartsGridControl.RefreshRow(i);
                }
            });







        }



 







        /// <summary>
        /// 使兩個GridControl排序同步化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl_FilterGroupSortChanging(object sender, FilterGroupSortChangingEventArgs e)
        {
            var GridC = new DevExpress.Xpf.Grid.GridControl();

            if (SoftGridControl is null || PartsGridControl is null)
                return;

                if ((sender as DevExpress.Xpf.Grid.GridControl).Name == PartsGridControl.Name)
                {
                    GridC = SoftGridControl;
                }
            

                if ((sender as DevExpress.Xpf.Grid.GridControl).Name == SoftGridControl.Name)
                {
                    GridC = PartsGridControl;
                }
            
            foreach (var item in e.SortInfo)
            {
                var CSortOrder = DevExpress.Data.ColumnSortOrder.None;
                if (item.Direction == System.ComponentModel.ListSortDirection.Ascending)
                {
                    CSortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                else
                {
                    CSortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                }

                GridC.ClearSorting();
                GridC.SortBy(SoftGridControl.Columns[item.PropertyName], CSortOrder);
                GridC.RefreshData();
            }
        }


    }
}








