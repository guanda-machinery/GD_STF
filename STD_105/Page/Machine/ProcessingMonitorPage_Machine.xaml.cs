using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using devDept.Eyeshot;
using WPFSTD105.ViewModel;
using DevExpress.Xpf.Grid;
using System.Windows.Controls.Primitives;
using DevExpress.Xpf.Core.Native;
using MouseButton = devDept.Eyeshot.MouseButton;
using System.Web.UI.WebControls;
using DevExpress.XtraSplashScreen;
using System.Diagnostics;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using DevExpress.Data.Extensions;
using GD_STD.Data;
using System.Collections.ObjectModel;

namespace STD_105
{
    /// <summary>
    /// 新加工監控 ProcessingMonitorPage_Machine.xaml 的互動邏輯
    /// </summary>
    public partial class ProcessingMonitorPage_Machine : BasePage<ProcessingMonitorVM>
    {
     //   public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.Create(() => new WaitIndicator(), new DevExpress.Mvvm.DXSplashScreenViewModel { });

        public ObSettingVM ViewModel { get; set; } = new ObSettingVM();
        /// <summary>
        /// nc 設定檔
        /// </summary>
        //public NcTemp NcTemp { get; set; }
        //public string DataPath { get; set; }
        public List<List<object>> DataList { get; set; }

        public ProcessingMonitorPage_Machine()
        {


            InitializeComponent();

            model.DataContext = ViewModel;
            drawing.DataContext = ViewModel;
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.ActionMode = actionType.None;
            DataList = new List<List<object>>();
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            model.Secondary = drawing;
            drawing.Secondary = model;
            //ControlDraw3D();
            //CheckReportLogoExist();

        }
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                try
                {
                    //model.Dispose();
                    disposable.Dispose();
                }
                catch(Exception ex)
                {

                }
            }
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
            if (ViewModel.Reductions == null)
            {
                ViewModel.Reductions = new ReductionList(model, drawing); //紀錄使用找操作
            }

            #endregion
        }


        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningCombinational_List_TableView.Name)
            {
                IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningSchedule_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (SoftCountTableView_ScrollElement != null)
                    SoftCountTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningSchedule_List_TableView.Name)
            {
                IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningCombinational_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (PartsTableView_ScrollElement != null)
                    PartsTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
        }

        private void MachineMessage_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            var ItemList = ((sender as GridControl).ItemsSource as IEnumerable<OperatingLog>).ToList();

            if (ItemList != null)
            {
               // var ItemList = ItemIEnum.ToList();
                if (ItemList.Count() > 0)
                {
                    //等待0.1秒 等頁面刷新後再捲動
                    //捲到最底下
                    Task.Run(() =>
                    {
                        Thread.Sleep(100);
                        MachineMessage_TableView.Dispatcher.Invoke(() =>
                        {
                            MachineMessage_TableView.TopRowIndex = ItemList.Count() - 1;
                            (sender as GridControl).SelectedItem = ItemList[ItemList.Count() - 1];
                        }
                        ); 
                    });
                }
            }


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
                edit2D.Visibility = Visibility.Visible;
            }
            //關閉刪除功能與編輯功能
            if (ViewModel.Select3DItem.Count == 0)
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
            ViewModel.Select3DItem.Clear();
            ViewModel.tem3DRecycle.Clear();
            ViewModel.Select2DItem.Clear();
            ViewModel.tem2DRecycle.Clear();
            model.ClearAllPreviousCommandData();
            drawing.ClearAllPreviousCommandData();
            drawing.SetCurrent(null);
            model.SetCurrent(null);//層級 To 要編輯的BlockReference
        }


        private void drawing_Loaded(object sender, RoutedEventArgs e)
        {
            //平移滑鼠中鍵
            drawing.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
            drawing.ActionMode = actionType.SelectByBox;

            drawing.ZoomFit();//設置道適合的視口
            drawing.Refresh();//刷新模型

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

        private void MachiningCombinational_List_TableView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlDraw3D();
        }



        private void ControlDraw3D()
        {
            var SelectedData = (GD_STD.Data.MaterialDataView)MachiningCombinational_List_GridControl.SelectedItem;
            //未選擇目標 不畫圖
            if (SelectedData == null)
                return;


            model.ActionMode = actionType.SelectByBox;
            string content = SelectedData.MaterialNumber; //素材編號


            model.AssemblyPart(content);
            AssemblyPart2D(model, content);

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
        public void AssemblyPart2D(devDept.Eyeshot.Model model, string materialNumber)
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
                    //FloatingMode.Window);
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


            for (int i = 0; i < place.Count; i++)
            {

                if (place.Count == 1) // count=1表素材沒有零件組成,只有(前端切除)程序紀錄
                {
                    return;
                }

                if (place[i].IsCut) //如果是切割物件
                {
                    Entity cut1 = Draw2DCutMesh(parts[0], model, place[i].Start, place[i].End, "Cut" + i);
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
                    int partIndex = parts.FindIndex(el => el.Number == place[i].Number);
                    findsteel = false;


                    if (parts[partIndex].GUID.ToString() != "") //如果圖面檔案
                    {
                        int FindSteelBlockIndex = model.Blocks.FindIndex(el => el.Name == parts[partIndex].GUID.ToString()); // 由model block裡 找尋對應零件位置

                        for (int searchboltindex = FindSteelBlockIndex; searchboltindex < model.Blocks.Count; searchboltindex++)
                        {
                            //沒找到零件
                            if (searchboltindex == -1)
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

                            if ((model.Blocks[searchboltindex].Entities[0].EntityData is BoltAttr) && findsteel)
                            {

                                Steel2DBlock steel2DBlock = new Steel2DBlock((devDept.Eyeshot.Entities.Mesh)model.Blocks[FindSteelBlockIndex].Entities[0], model.Blocks[FindSteelBlockIndex].Name);   // 產生2D

                                int icount;
                                for (icount = 0; icount < model.Entities.Count; icount++)
                                {
                                    BlockReference aa = (BlockReference)model.Entities[icount]; //取得參考圖塊
                                    devDept.Eyeshot.Block bb = model.Blocks[aa.BlockName]; //取得圖塊 
                                    if (model.Blocks[searchboltindex].Name == bb.Name)
                                    {
                                        break;
                                    }
                                }

                                BlockReference blockReference = (BlockReference)model.Entities[icount]; //取得參考圖塊
                                devDept.Eyeshot.Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 

                                Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生3D螺栓圖塊
                                Bolts2DBlock bolts2DBlock = new Bolts2DBlock(bolts3DBlock, steel2DBlock); //產生2D螺栓圖塊

                                int tmpindex = drawing.Blocks.FindIndex(x => x.Name == bolts2DBlock.Name);
                                if (tmpindex == -1)
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



















    }
}
