#define Debug
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using DevExpress.DataProcessing.InMemoryDataProcessor;
//using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFSTD105.Surrogate;
using WPFSTD105.ViewModel;


namespace WPFSTD105
{
    /// <summary>
    /// 這是Model，它將擴展繪圖應用程序所需的行為。
    /// </summary>
    public partial class ModelExt : devDept.Eyeshot.Model
    {
        /// <summary>
        /// 次要模型
        /// </summary>
        public ModelExt Secondary { get; set; }
        /// <summary>
        /// 目前用戶顯示的模型
        /// </summary>
        public bool CurrentModel { get; set; }
        public Transformation CurrentTransformation { get => new Transformation(plane.Origin, plane.AxisX, plane.AxisY, plane.AxisZ); }
        /// <summary>
        /// 正在程式正在修改
        /// </summary>
        public bool Modify { get; set; }
        /// <summary>
        /// 預設值
        /// </summary>
        public ModelExt()
        {
            this.SelectionChanged += Model_SelectionChanged;
        }



        private List<Entity> selectionEntities = new List<Entity>();

        /// <summary>
        /// 模型物件選擇變更時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Modify == true)
                return;

            ObSettingVM vm = (ObSettingVM)DataContext;
                        
            if (ActionMode != actionType.SelectVisibleByPick)
            {
                if (Name == "model")
                {
                    log4net.LogManager.GetLogger("選擇變更").Debug("開始");
                    //如果尚未開始編輯模式
                    if (this.CurrentBlockReference == null)
                    {
#if DEBUG
                        log4net.LogManager.GetLogger("選擇變更").Debug("選擇圖塊");
#endif
                        foreach (var rem in e.RemovedItems)
                        {
                            vm.Select3DItem.Remove(rem);//選擇移除 3D
                            vm.Select2DItem.Remove(new SelectedItem() { Item = Selected(Secondary, (BlockReference)rem.Item, false) });//選擇移除 2D
                        }
                        foreach (var selected in e.AddedItems)
                        {
                            vm.Select3DItem.Add(selected);//加入到 3D 選擇物件
                            vm.Select2DItem.Add(new SelectedItem() { Item = Selected(Secondary, (BlockReference)selected.Item, true) });//加入到 2D 選擇物件

                        }
                        //foreach (var rem in e.RemovedItems)
                        //{
                        //    vm.Select3DItem.Remove(rem);//選擇移除 3D
                        //    vm.Select2DItem.Remove(new SelectedItem() { Item = Selected(Secondary, (BlockReference)rem.Item, false) });//選擇移除 2D
                        //}
                    }
                    else
                    {
                        log4net.LogManager.GetLogger("選擇變更").Debug("選擇圖塊內物件");
                        try
                        {
                            foreach (var selected in e.AddedItems)
                            {
                                vm.tem3DRecycle.Add((Entity)selected.Item); //加入到 3D 以選擇的物件內
                                vm.tem2DRecycle.AddRange(Selected(Secondary, (Entity)selected.Item, true));//加入到 2D 選擇物件

                            }
                            foreach (var rem in e.RemovedItems)
                            {
                                vm.tem3DRecycle.Remove((Entity)rem.Item); //選擇移除 3D
                                Selected(Secondary, (Entity)rem.Item, false).ForEach(el => vm.tem2DRecycle.Remove(el)); //選擇移除 2D
                            }
                        }
                        catch (Exception ex)
                        {
                            log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                            throw;
                        }
#if DEBUG
                        log4net.LogManager.GetLogger("選擇變更").Debug("結束");
#endif

                    }
                }
                else
                {
                    if (this.CurrentBlockReference == null)
                    {
                        
                        int i=0;
                        foreach (var item in e.AddedItems)
                        {

                            SelectedItem AddEntity = e.AddedItems[i];
                            i++;
                            if (AddEntity.Item is LinearDim)
                                continue;

                            vm.Select2DItem.Add(item);//加入到 2D 選擇物件
                            vm.Select3DItem.Add(new SelectedItem() { Item = Selected(Secondary, (BlockReference)item.Item, true) });//加入到 3D 選擇物件
                        }
                        i = 0;
                        foreach (var item in e.RemovedItems)
                        {

                            SelectedItem RemovedEntity = e.RemovedItems[i];
                            i++;
                            if (RemovedEntity.Item is LinearDim)
                                continue;

                            vm.Select2DItem.Remove(item); //選擇移除 2D
                            vm.Select3DItem.Remove(new SelectedItem() { Item = Selected(Secondary, (BlockReference)item.Item, false) }); //選擇移除 3D
                        }
                    }
                    else
                    {
                        foreach (var selected in e.AddedItems)
                        {
                            vm.tem2DRecycle.AddRange(Selected(this, (Entity)selected.Item, true));//加入到 2D 選擇物件
                            vm.tem3DRecycle.AddRange(Selected(Secondary, (Entity)selected.Item, true));//加入到 3D 選擇物件
                        }
                        foreach (var rem in e.RemovedItems)
                        {
                            Selected(this, (Entity)rem.Item, false).ForEach(el => vm.tem2DRecycle.Remove(el));
                            Selected(Secondary, (Entity)rem.Item, false).ForEach(el => vm.tem3DRecycle.Remove(el));
                        }
                    }
                }
            }
            else
            {

            }
        }

        #region 私有屬性
        /// <summary>
        /// 第一次點擊
        /// </summary>
        private bool firstClick { get; set; }

        /// <summary>
        /// 表示模型的頂點類型
        /// </summary>
        public enum objectSnapType
        {
            None,
            Point,
            End,
            Mid,
            Center,
            Quad
        }
        SnapPoint snap;
        /// <summary>
        /// 始終在XY平面上繪製，視圖始終為頂視圖
        /// </summary>
        private Plane plane { get; set; } = Plane.XY;
        private Plane dimPlane { get; set; }

        /// <summary>
        /// 當前選擇/位置
        /// </summary>
        private Point3D current;

        /// <summary>
        /// 鼠標左鍵選擇或選取的點的列表
        /// </summary>
        private List<Point3D> points { get; set; } = new List<Point3D>();
        /// <summary>
        /// 目前鼠標位置
        /// </summary>
        private System.Drawing.Point mouseLocation { get; set; }

        /// <summary>
        /// 所選實體，存儲在LMB上，請點擊
        /// </summary>
        private int selEntityIndex { get; set; }
        private Entity selEntity { get; set; } = null;

        /// <summary>
        /// 標註時需要當前工程圖平面和延伸點
        /// </summary>
        private Plane drawingPlane { get; set; } = Plane.XY;

        /// <summary>
        /// 延伸點p1
        /// </summary>
        private Point3D extPt1;
        /// <summary>
        /// 延伸點p2
        /// </summary>
        private Point3D extPt2;

        /// <summary>
        /// 當前弧半徑
        /// </summary>
        private double radius { get; set; }
        /// <summary>
        /// 圓弧半徑 Y
        /// </summary>
        private double radiusY { get; set; }

        /// <summary>
        /// 當前弧形跨度角
        /// </summary>
        private double arcSpanAngle { get; set; }

        /// <summary>
        /// 了解折線或曲線是否必須閉合的閾值
        /// </summary>
        private const int magnetRange = 3;

        /// <summary>
        /// 當前選擇顯示操作的標籤
        /// </summary>
        private string activeOperationLabel = "";

        /// <summary>
        /// 標籤以顯示如何從命令退出（僅在當前選擇操作的情況下可用）
        /// </summary>
        private string rmb { get; set; } = "  RMB to exit.";
        #endregion

        #region 公用屬性
        /// <summary>
        /// 活動圖層索引
        /// </summary>
        public string ActiveLayerName { get; set; }
        /// <summary>
        /// 選擇的實體列表
        /// </summary>
        public List<Entity> selEntities { get; set; } = new List<Entity>();
        /// <summary>
        /// 標註第一點
        /// </summary>
        public Line firstLine { get; set; } = null;
        /// <summary>
        /// 標註第二點
        /// </summary>
        public Line secondLine { get; set; } = null;
        /// <summary>
        /// 象限點
        /// </summary>
        public Point3D quadrantPoint = null;
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ModelExt.DrawingColor' 的 XML 註解
        public static Color DrawingColor_White = Color.White;
        public static Color DrawingColor_Red = Color.Red;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ModelExt.DrawingColor' 的 XML 註解
        #endregion
        /// <summary>
        /// 按下滑鼠鍵
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var mousePos = RenderContextUtility.ConvertPoint(GetMousePosition(e));
            this.selEntityIndex = GetEntityUnderMouseCursor(mousePos);
            if (waitingForSelection)
            {
                if (this.selEntityIndex != -1)
                {
                    if (selEntity == null || drawingAngularDim)
                    {
                        this.selEntity = this.selEntities[this.selEntityIndex];
                        if (activeOperationLabel != "")
                            this.selEntity.Selected = true;
                    }
                    //從直線上繪製drawingAngularDim需要多個選擇
                    if (!drawingAngularDim || this.Entities[selEntityIndex] is Arc)
                        waitingForSelection = false;
                }
            }

            if (ActiveViewport.ToolBar.Contains(mousePos))
            {
                base.OnMouseDown(e);
                return;
            }
            #region 處理滑鼠左鍵點擊
            if (ActionMode == actionType.None && e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                //需要跳過添加點來選擇實體
                editingMode = doingOffset || doingMirror || doingExtend || doingTrim || doingFillet || doingChamfer || doingTangents;

                //捕捉物件鎖點
                if (objectSnapEnabled && snapPoint != null)
                {
                    if (!(editingMode && firstClick))
                    {
                        points.Add(snapPoint);
                    }
                }
                //當鼠標位於折線或曲線的起點附近時，需要關閉曲線和折線的控件
                else if (IsPolygonClosed())
                {
                    //如果從當前點到存儲的第一個點的距離小於給定的臨界值
                    points.Add((Point3D)points[0].Clone()); //要添加到點的點是存儲的第一個點。
                    current = (Point3D)points[0].Clone();
                }
                //網格鎖點
                else if (gridSnapEnabled)
                {
                    if (!(editingMode && firstClick))
                    {
                        SnapToGrid(ref current);
                        points.Add(current);
                    }
                }
                else //任意位置
                {
                    if (!(editingMode && firstClick))
                        points.Add(current);
                }
                firstClick = false;
                if (drawingPoints)//如果要繪製點，請在每個滑鼠左鍵點擊上創建並添加新的點實體
                {
                    devDept.Eyeshot.Entities.Point point;

                    if (objectSnapEnabled && snapPoint != null)
                        point = new devDept.Eyeshot.Entities.Point(snap);
                    else
                        point = new devDept.Eyeshot.Entities.Point(current);

                    AddAndRefresh(point, ActiveLayerName);
                }
                //產生文字
                else if (drawingText)
                {
                    devDept.Eyeshot.Entities.Text text = new Text(current, "隨便貼", 5);
                    AddAndRefresh(text, ActiveLayerName);
                }
                // 如果是標註註解
                else if (drawingLeader)
                {
                    if (points.Count == 3) //標註註解一定要點三次才執行
                    {
                        Leader leader = new Leader(Plane.XY, points);
                        leader.ArrowheadSize = 3;
                        AddAndRefresh(leader, ActiveLayerName);
                        devDept.Eyeshot.Entities.Text text = new Text((Point3D)current.Clone(), "Sample Text", leader.ArrowheadSize);
                        AddAndRefresh(text, ActiveLayerName);
                        drawingLeader = false;

                    }
                }
                else if (setPlane && points.Count == 3)
                {
                    ActiveViewport.OriginSymbols.Clear();
                    plane = new Plane(points[0], points[1], points[2]);
                    Transformation transformation1 = new Transformation(plane.Origin, plane.AxisX, plane.AxisY, plane.AxisZ);
                    OriginSymbol symbol = new OriginSymbol(1, "", transformation1, false);

                    ActiveViewport.OriginSymbols.Add(symbol);
                    ActiveViewport.CompileUserInterfaceElements();
                    setPlane = false;
                    objectSnapEnabled = false;
                }
                //如果繪圖線完成，則創建線實體並將其添加到模型中
                else if (drawingLine && points.Count == 2)
                {
                    Line line = new Line(points[0], points[1]);
                    AddAndRefresh(line, ActiveLayerName);
                    drawingLine = false;
                }
                //如果完成了繪圖圓，則創建一個圓形實體並將其添加到模型中
                else if (drawingCircle && points.Count == 2)
                {
                    Circle circle = new Circle(drawingPlane, drawingPlane.Origin, radius);
                    AddAndRefresh(circle, ActiveLayerName);

                    drawingCircle = false;
                }
                //如果ARC繪圖完成，則創建一個弧實體並將其添加到模型中
                //輸入-中心點和兩個端點
                else if (drawingArc && points.Count == 3)
                {
                    Arc arc = new Arc(drawingPlane, drawingPlane.Origin, radius, 0, arcSpanAngle);
                    AddAndRefresh(arc, ActiveLayerName);

                    drawingArc = false;
                }
                //如果繪製橢圓，則創建橢圓實體並將其添加到模型中
                //輸入-橢圓中心，第一個軸的終點，第二個軸的終點
                else if (drawingEllipse && points.Count == 3)
                {
                    Ellipse ellipse = new Ellipse(drawingPlane, drawingPlane.Origin, radius, radiusY);
                    AddAndRefresh(ellipse, ActiveLayerName);

                    drawingEllipse = false;
                }
                //如果EllipticalArc繪製完成，則創建EllipticalArc實體並將其添加到模型中
                //輸入-橢圓中心，第一軸終點，第二軸終點，終點
                else if (drawingEllipticalArc && points.Count == 4)
                {
                    EllipticalArc ellipticalArc = new EllipticalArc(drawingPlane, drawingPlane.Origin, radius, radiusY, 0, arcSpanAngle);
                    AddAndRefresh(ellipticalArc, ActiveLayerName);

                    drawingEllipticalArc = false;
                }
                //線性標註
                else if (drawingLinearDim && points.Count == 3)
                {

                    LinearDim linearDim = new LinearDim(drawingPlane, points[0], points[1], current, dimTextHeight);
                    linearDim.Selectable = false;
                    AddAndRefresh(linearDim, ActiveLayerName);
                    drawingLinearDim = false;
                    objectSnapEnabled = false;
                    this.Entities[Entities.Count - 1].Selectable = false;
                    this.ActionMode = actionType.SelectByBox;
                }
                //對齊式標註
                else if (drawingAlignedDim && points.Count == 3)
                {
                    LinearDim linearDim = new LinearDim(plane, points[0], points[1], current, dimTextHeight);
                    AddAndRefresh(linearDim, ActiveLayerName);

                    drawingLinearDim = false;
                    //objectSnapEnabled = false;
                }
                //座標標註
                else if (drawingOrdinateDim && points.Count == 2)
                {
                    OrdinateDim ordinateDim = new OrdinateDim(Plane.XY, points[0], points[1], drawingOrdinateDim, dimTextHeight);
                    AddAndRefresh(ordinateDim, ActiveLayerName);

                    drawingOrdinateDim = false;
                }
                //半徑標註或圓徑標註
                else if ((drawingRadialDim || drawingDiametricDim) && points.Count == 2)
                {
                    if (selEntity is Circle circle)
                    {
                        //確保radialDim平面始終具有正確的法線
                        Circle orientedCircle = new Circle(Plane.XY, circle.Center, circle.Radius);

                        if (drawingRadialDim)
                        {
                            RadialDim radialDim = new RadialDim(orientedCircle, points[points.Count - 1], dimTextHeight);
                            AddAndRefresh(radialDim, ActiveLayerName);

                            drawingRadialDim = false;
                        }
                        else // 直徑標註
                        {
                            DiametricDim diametricDim = new DiametricDim(orientedCircle, points[points.Count - 1], dimTextHeight);
                            AddAndRefresh(diametricDim, ActiveLayerName);
                            drawingDiametricDim = false;
                        }
                    }
                }
                //角度標註
                else if (drawingAngularDim && points.Count >= 3)
                {
                    Point3D p1 = points[1], p2 = points[2];
                    if (p1.X > p2.X || p1.Y > p2.Y)
                    {
                        Point3D point1 = points[2], point2 = points[1];
                        p1 = point1;
                        p2 = point2;
                    }
                    AngularDim angularDim = new AngularDim(new Plane(points[0], p1, p2), p1, p2, new Line(p1, p2).MidPoint, dimTextHeight);
                    angularDim.Selectable = false;
                    AddAndRefresh(angularDim, ActiveLayerName);
                    drawingAngularDim = false;
                    #region 備份
                    //if (!drawingAngularDimFromLines)
                    //{
                    //    if (selEntity is Arc arc && points.Count == 2 && !drawingQuadrantPoint)
                    //    {
                    //        Plane plane = (Plane)arc.Plane.Clone();
                    //        Point3D startPoint = arc.StartPoint,
                    //            endPoint = arc.EndPoint;

                    //        //檢查圓弧是否為順時針方向
                    //        if (Utility.IsOrientedClockwise(arc.Vertices))
                    //        {
                    //            plane.Flip();
                    //            startPoint = arc.EndPoint;
                    //            endPoint = arc.StartPoint;
                    //        }

                    //        AngularDim angularDim = new AngularDim(plane, startPoint, endPoint, points[points.Count - 1], dimTextHeight);

                    //        angularDim.TextSuffix = "°"; //角度單位

                    //        AddAndRefresh(angularDim, ActiveLayerName);
                    //        drawingAngularDim = false;
                    //    }
                    //}
                    /////如果不是時候設置<see cref="quadrantPoint"/>，則添加角度標註的行
                    //if (selEntity is Line selectedLine && !drawingQuadrantPoint && quadrantPoint == null)
                    //{
                    //    if (firstLine == null)
                    //        firstLine = selectedLine;
                    //    else if (secondLine == null && !ReferenceEquals(firstLine, selectedLine))
                    //    {
                    //        secondLine = selectedLine;
                    //        drawingQuadrantPoint = true;
                    //        //重置點以僅獲得像限點和文本位置點
                    //        points.Clear();
                    //    }
                    //    drawingAngularDimFromLines = true;
                    //}
                    //else if (drawingQuadrantPoint)
                    //{
                    //    ScreenToPlane(mousePos, plane, out quadrantPoint);
                    //    drawingQuadrantPoint = false;
                    //}
                    ////如果所有參數都存在，則變為角度標註
                    //else if (points.Count == 2 && quadrantPoint != null)
                    //{
                    //    AngularDim angularDim = new AngularDim(plane, (Line)firstLine.Clone(), (Line)secondLine.Clone(), quadrantPoint, points[points.Count - 1], dimTextHeight);

                    //    angularDim.TextSuffix = "°";

                    //    AddAndRefresh(angularDim, ActiveLayerName);

                    //    drawingAngularDim = false;
                    //    drawingAngularDimFromLines = false;
                    //}
                    #endregion
                }
                //偏移
                else if (doingOffset && points.Count == 1)
                {
                    CreateOffsetEntity();
                    ClearAllPreviousCommandData();
                }
                //鏡射
                else if (doingMirror && points.Count == 2 && selEntity != null)
                {
                    CreateMirrorEntity();
                    ClearAllPreviousCommandData();
                }
                //延伸線段
                else if (doingExtend && firstSelectedEntity != null && secondSelectedEntity != null)
                {
                    ExtendEntity();
                    ClearAllPreviousCommandData();
                }
            }
            #endregion
            #region 處理滑鼠右鍵點擊
            //            else if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            //            {
            //                ScreenToPlane(mousePos, plane, out current);

            //                if (drawingPoints)
            //                {
            //                    points.Clear();
            //                    drawingPoints = false;
            //                }
            //                else if (drawingText)
            //                {
            //                    drawingText = false;
            //                }
            //                else if (drawingLeader)
            //                {
            //                    drawingLeader = false;
            //                }

            //                //如果繪製折線，則創建並添加LinearPath實體進行建模
            //                else if (drawingPolyLine)
            //                {
            //                    LinearPath lp = new LinearPath(points);
            //                    AddAndRefresh(lp, ActiveLayerName);

            //                    drawingPolyLine = false;
            //                }
            //                //如果繪製樣條線，則創建曲線實體並將其添加到模型中
            //                else if (drawingCurve)
            //                {
            //#if NURBS
            //                    Curve curve = Curve.CubicSplineInterpolation(points);
            //                    AddAndRefresh(curve, ActiveLayerName);
            //#endif
            //                    drawingCurve = false;
            //                }
            //                else
            //                {
            //                    ClearAllPreviousCommandData();
            //                }
            //            }
            #endregion
            base.OnMouseDown(e);
        }
        /// <summary>
        /// 滑鼠離開
        /// </summary>
        private bool cursorOutside { get; set; }
        /// <summary>
        /// 滑鼠離開繪圖Modell時所發生的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            cursorOutside = true;
            base.OnMouseLeave(e);
            Invalidate();//當心“渲染器”設置為devDept.Eyeshot.rendererType.NativeExperimental，此方法執行與Refresh（）方法相同的操作。
        }
        /// <summary>
        /// 滑鼠進入繪圖Model時所發生的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
   
            base.OnMouseEnter(e);
            cursorOutside = false;
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 滑鼠在Model移動時所發生的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //20221111 滑鼠移動時 強制把十字改成游標
            if (this.Cursor == Cursors.Cross)
            {
                this.Cursor = Cursors.Arrow;
            }
            //RenderContextUtility 提供渲染上下文實用程序方法的類。
            mouseLocation = RenderContextUtility.ConvertPoint(GetMousePosition(e));//將System.Windows.Point轉換為System.Drawing.Point的方法。

            //如果ObjectSnap為true，則需要找到最接近的頂點（如果有）
            if (objectSnapEnabled)
            {
                Entities[Entities.Count - 1].Selectable = true;
                this.snapPoint = null;
                snapPoints = GetSnapPoints(mouseLocation);
                Entities[Entities.Count - 1].Selectable = false;
            }
            //如果start有效且actionMode為None且不在工具欄區域中
            if (current == null || ActionMode != actionType.None || ActiveViewport.ToolBar.Contains(mouseLocation))
            {
                base.OnMouseMove(e);
                return;
            }

            //在不重新繪製整個場景的情況下繪製視口表面。
            PaintBackBuffer();

            // 交換前緩衝區和後緩衝區。
            SwapBuffers();

            if (drawingPoints)
                activeOperationLabel = "點 : ";
            else if (drawingText)
                activeOperationLabel = "文字 : ";
            else if (drawingLeader)
                activeOperationLabel = "標示 : ";
            else if (drawingLine)
                activeOperationLabel = "畫線 : ";
            else if (drawingEllipse)
                activeOperationLabel = "橢圓 : ";
            else if (drawingEllipticalArc)
                activeOperationLabel = "橢圓弧 : ";
            else if (drawingCircle)
                activeOperationLabel = "圓形 : ";
            else if (drawingArc)
                activeOperationLabel = "弧 : ";
            else if (drawingPolyLine)
                activeOperationLabel = "多邊形線段 : ";
            else if (drawingCurve)
                activeOperationLabel = "繪製曲線 : ";
            else if (doingMirror)
                activeOperationLabel = "鏡射 : ";
            else if (doingOffset)
                activeOperationLabel = "偏移 : ";
            else if (doingTrim)
                activeOperationLabel = "修剪 : ";
            else if (doingExtend)
                activeOperationLabel = "延伸 : ";
            else if (doingFillet)
                activeOperationLabel = "倒圓角 : ";
            else if (doingChamfer)
                activeOperationLabel = "倒直角 : ";
            else if (doingMove)
                activeOperationLabel = "移動 : ";
            else if (doingRotate)
                activeOperationLabel = "旋轉 : ";
            else if (doingScale)
                activeOperationLabel = "比例 : ";
            else if (doingTangents)
                activeOperationLabel = "切線 : ";
            else
                activeOperationLabel = "";

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 目前捕捉
        /// </summary>
        private bool CurrentlySnapping { get; set; } = false;
        /// <summary>
        /// 繪製疊加的UI元素
        /// </summary>
        /// <param name="data">描繪數據</param>
        protected override void DrawOverlay(DrawSceneParams data)
        {
            //ScreenToPlane(mouseLocation, plane, out current);
            ScreenToPlane(mouseLocation, Camera.FarPlane, out current);
            CurrentlySnapping = false;
            //current.TransformBy(new Transformation(plane.Origin, plane.AxisX, plane.AxisY, plane.AxisZ));
            // 如果ObjectSnap為ON，我們需要找到最接近的頂點（如果有）
            if (objectSnapEnabled && snapPoints != null && snapPoints.Length > 0)
            {
                snap = FindClosestPoint(snapPoints);
                current = snap;
                CurrentlySnapping = true;
            }
            //為交互式繪圖或彈性線設置GL
            renderContext.SetLineSize(1);

            renderContext.EnableXOR(true);

            renderContext.SetState(depthStencilStateType.DepthTestOff);

            if (!(CurrentlySnapping) && !(waitingForSelection) && ActionMode == actionType.None &&
              !(doingExtend || doingTrim || doingFillet || doingChamfer || doingTangents || drawingOrdinateDim) && !ObjectManipulator.Visible)
            {
                //if (!cursorOutside)
                //DrawPositionMark(current);
            }
            if (setPlane)
            {
                if (points.Count == 0)
                {
                    ActionPrompt("原點");
                }
                else if (points.Count == 1)
                {
                    ActionPrompt("X 座標");
                }
                else if (points.Count == 2)
                {
                    ActionPrompt("Y 座標");
                }
            }
            if (drawingLine || drawingPolyLine)
            {
                DrawInteractiveLines();
            }
            else if (drawingCircle && points.Count > 0)
            {
                if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation))
                {
                    //DrawInteractiveCircle();
                }
            }
            else if (drawingArc && points.Count > 0)
            {
                if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation))
                {
                    //DrawInteractiveArc();
                }
            }
            else if (drawingEllipse && points.Count > 0)
            {
                //DrawInteractiveEllipse();
            }
            else if (drawingEllipticalArc && points.Count > 0)
            {
                //DrawInteractiveEllipticalArc();
            }
            else if (drawingCurve)
            {
                //DrawInteractiveCurve();
            }
            else if (drawingLeader)
            {
                //DrawInteractiveLeader();
            }
            else if (drawingLinearDim || drawingAlignedDim)
            {
                if (points.Count < 2)
                {
                    if (!cursorOutside)
                    {
                        DrawInteractiveLines();
                        DrawSelectionMark(mouseLocation);
                        if (!firstClick)
                            ActionPrompt("第二點標註");
                        else
                            ActionPrompt("第一點標註");

                    }
                }
                else
                {
                    if (drawingLinearDim)
                        DrawInteractiveLinearDim();
                    else if (drawingAlignedDim)
                        DrawInteractiveAlignedDim();

                }
            }
            else if (drawingOrdinateDim)
            {
                if (!cursorOutside)
                {
                    if (points.Count == 1)
                    {
                        //DrawPositionMark(current, 5);
                        DrawInteractiveOrdinateDim();
                    }
                    else
                    {
                        //DrawPositionMark(current);
                        //
                        //摘要：
                        //啟用或禁用XOR（反色）繪圖模式。
                        //
                        //參數：
                        //啟用：
                        //啟用狀態
                        //
                        //備註：
                        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                        renderContext.EnableXOR(false);
                        string text = "Select the definition point";
                        if (!firstClick)
                            text = "Select the leader end point";

                        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                            text, new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                        //
                        //摘要：
                        //啟用或禁用XOR（反色）繪圖模式。
                        //
                        //參數：
                        //啟用：
                        //啟用狀態
                        //
                        //備註：
                        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                        renderContext.EnableXOR(true);
                    }


                }
            }
            else if (drawingRadialDim || drawingDiametricDim)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select Arc or Circle", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);

                }
                DrawInteractiveDiametricDim();
            }
            else if (drawingAngularDim)
            {
                if (points.Count < 2)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);
                    string text = "選擇第一點";
                    if (!firstClick)
                        text = "選擇第二點";

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        text, new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                else
                {
                    //計算目前角度
                    double ma_x = points[1].X - points[0].X;
                    double ma_y = points[1].Y - points[0].Y;
                    double mb_x = current.X - points[0].X;
                    double mb_y = current.Y - points[0].Y;
                    double v1 = (ma_x * mb_x) + (ma_y * mb_y);
                    double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
                    double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
                    double cosM = v1 / (ma_val * mb_val);
                    double angleAMB = Math.Acos(cosM) * 180 / Math.PI;

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                       $"選擇第三點。目前角度 : {Math.Round(angleAMB, 2, MidpointRounding.AwayFromZero) }", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);

                    DrawInteractiveArc();
                }

                #region 備份
                //if (waitingForSelection)
                //{
                //    if (!drawingAngularDimFromLines)
                //    {
                //        DrawSelectionMark(mouseLocation);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(false);

                //        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                //            "Select Arc or Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(true);
                //    }
                //    else if (quadrantPoint == null && !drawingQuadrantPoint)
                //    {

                //        DrawSelectionMark(mouseLocation);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(false);

                //        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                //            "Select second Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(true);
                //    }
                //    else if (drawingQuadrantPoint)
                //    {
                //        DrawSelectionMark(mouseLocation);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(false);

                //        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                //            "Select a quadrant", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(true);
                //    }
                //    else if (quadrantPoint != null)
                //    {
                //        DrawSelectionMark(mouseLocation);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(false);

                //        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                //            "Select text position", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
                //        //
                //        //摘要：
                //        //啟用或禁用XOR（反色）繪圖模式。
                //        //
                //        //參數：
                //        //啟用：
                //        //啟用狀態
                //        //
                //        //備註：
                //        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                //        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                //        renderContext.EnableXOR(true);
                //    }
                //}
                //DrawInteractiveAngularDim();
                #endregion
            }
            else if (doingMirror)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select entity to mirror", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                CreateMirrorEntity();
            }
            else if (doingOffset)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select entity to offset", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                CreateOffsetEntity();
            }
            else if (doingMove)
            {
                //MoveEntity();
            }
            else if (doingScale)
            {
                //ScaleEntity();
            }
            else if (doingRotate)
            {
                //RotateEntity();
            }
            else if (doingFillet)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select first curve", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                //CreateFilletEntity();
            }
            else if (doingTangents)
            {
                if (waitingForSelection)
                {
                    {
                        DrawSelectionMark(mouseLocation);
                        //
                        //摘要：
                        //啟用或禁用XOR（反色）繪圖模式。
                        //
                        //參數：
                        //啟用：
                        //啟用狀態
                        //
                        //備註：
                        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                        renderContext.EnableXOR(false);

                        DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                            "Select first circle", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                        //
                        //摘要：
                        //啟用或禁用XOR（反色）繪圖模式。
                        //
                        //參數：
                        //啟用：
                        //啟用狀態
                        //
                        //備註：
                        //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                        //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                        renderContext.EnableXOR(true);
                    }

                }
                //CreateTangentEntity();
            }
            else if (doingChamfer)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select first curve", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                //CreateChamferEntity();
            }
            else if (doingExtend)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select boundary entity", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                ExtendEntity();
            }
            else if (doingTrim)
            {
                if (waitingForSelection)
                {
                    DrawSelectionMark(mouseLocation);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                        "Select trimming entity", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                    //
                    //摘要：
                    //啟用或禁用XOR（反色）繪圖模式。
                    //
                    //參數：
                    //啟用：
                    //啟用狀態
                    //
                    //備註：
                    //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
                    //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                    renderContext.EnableXOR(true);
                }
                //TrimEntity();
            }
            //
            //摘要：
            //啟用或禁用XOR（反色）繪圖模式。
            //
            //參數：
            //啟用：
            //啟用狀態
            //
            //備註：
            //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
            //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
            renderContext.EnableXOR(false);


            //文字繪圖
            if (!(drawingDiametricDim || drawingAlignedDim || drawingLinearDim || drawingOrdinateDim || drawingLeader || drawingRadialDim || drawingAngularDim ||
                doingMirror || doingOffset || doingTangents || doingExtend || doingTrim || doingFillet || doingChamfer || doingMove || doingScale || doingRotate) && ActionMode == actionType.None)
            {
                if (!(drawingEllipticalArc && points.Count >= 3) && !cursorOutside)
                {
                    //label on mouse
                    string exitCommand = "";
                    if (drawingCurve || drawingPolyLine || drawingPoints)
                        exitCommand = rmb;
                    else
                        exitCommand = "";

                    //DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                    //    activeOperationLabel +
                    //    "X = " + current.X.ToString("f2") + ", " +
                    //    "Y = " + current.Y.ToString("f2") +
                    //    exitCommand,
                    //    new Font("Tahoma", 8.25f),
                    //    DrawingColor, ContentAlignment.BottomLeft);
                }
            }

            base.DrawOverlay(data);
        }
        /// <summary>
        /// 動作提示
        /// </summary>
        private void ActionPrompt(string titel)
        {
            //
            //摘要：
            //啟用或禁用XOR（反色）繪圖模式。
            //
            //參數：
            //啟用：
            //啟用狀態
            //
            //備註：
            //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
            //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
            renderContext.EnableXOR(false);

            if (current != null)
            {
                titel += $"{current.X.ToString("f1") }, {current.Y.ToString("f1")}, {current.Z.ToString("f1")}";
            }
            DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                titel, new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
            //
            //摘要：
            //啟用或禁用XOR（反色）繪圖模式。
            //
            //參數：
            //啟用：
            //啟用狀態
            //
            //備註：
            //如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR
            //混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
            renderContext.EnableXOR(true);
        }

        /// <summary>
        /// 繪製具有選定中心點位置和兩個端點的交互式弧
        /// </summary>
        private void DrawInteractiveArc()
        {
            Point2D[] screenPts = GetScreenVertices(points);

            renderContext.DrawLineStrip(screenPts);

            if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation) && points.Count > 0)
            {
                //繪製彈性線
                renderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(current));

                //畫三點弧
                if (points.Count == 2)
                {
                    radius = points[0].DistanceTo(points[1]);
                    if (radius > 1e-3)
                    {
                        drawingPlane = GetPlane(points[1]);
                        Vector2D v1 = new Vector2D(points[0], points[1]);
                        v1.Normalize();
                        Vector2D v2 = new Vector2D(points[0], current);
                        v2.Normalize();
                        arcSpanAngle = Vector2D.SignedAngleBetween(v1, v2);
                        if (Math.Abs(arcSpanAngle) > 1e-3)
                        {
                            Arc tempArc = new Arc(drawingPlane, drawingPlane.Origin, radius, 0, arcSpanAngle);
                            Draw(tempArc);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 取得平面
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        private Plane GetPlane(Point3D next)
        {
            Vector3D xAxis = new Vector3D(points[0], next);
            xAxis.Normalize();
            Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);
            yAxis.Normalize();

            Plane plane = new Plane(points[0], xAxis, yAxis);

            return plane;
        }
        /// <summary>
        /// 繪製屏幕曲線
        /// </summary>
        /// <param name="curve"></param>
        private void DrawScreenCurve(ICurve curve)
        {
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            renderContext.DrawLineStrip(pts);
        }
        /// <summary>
        /// 繪製
        /// </summary>
        /// <param name="theCurve"></param>
        private void Draw(ICurve theCurve)
        {
            if (theCurve is CompositeCurve)
            {
                CompositeCurve compositeCurve = theCurve as CompositeCurve;
                Entity[] explodedCurves = compositeCurve.Explode();
                foreach (Entity ent in explodedCurves)

                    DrawScreenCurve((ICurve)ent);
            }
            else
            {
                DrawScreenCurve(theCurve);
            }
        }
        /// <summary>
        /// 按下鍵盤
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DataContext is ObSettingVM obSettingVM)
            {
                if (e.Key == Key.Delete)
                {
                    BlockReference modelBlock = null;
                    BlockReference drawingBlock = null;

                    log4net.LogManager.GetLogger("在3D按下鍵盤").Debug("Delete");
                    if (obSettingVM.Select3DItem.Count > 0 && CurrentBlockReference == null) //判斷是否有選擇到物件
                    {
                        List<Entity> sele3D = new List<Entity>(), sele2D = new List<Entity>();
                        obSettingVM.Select3DItem.ForEach(el => sele3D.Add((BlockReference)el.Item));
                        obSettingVM.Select2DItem.ForEach(el => sele2D.Add((BlockReference)el.Item));
                        obSettingVM.Reductions.Add(new Reduction() //加入到垃圾桶內
                        {
                            SelectReference = null,
                            Recycle = new List<List<Entity>>() { sele3D },
                            User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                        }, new Reduction() //加入到垃圾桶內
                        {
                            SelectReference = null,
                            Recycle = new List<List<Entity>>() { sele2D },
                            User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                        });

                        if (Name == "model")
                        {
                            obSettingVM.Select2DItem.ForEach(el => Secondary.Entities.Remove((BlockReference)el.Item));
                        }
                        else
                        {
                            obSettingVM.Select3DItem.ForEach(el => Secondary.Entities.Remove((BlockReference)el.Item));
                        }
                        //STDSerialization ser = new STDSerialization();
                        //ser.SetPartModel(obSettingVM.SteelAttr.GUID.ToString(), this);
                    }
                    else //編輯模式
                    {
                        if (this.Name == "model")
                        {
                            modelBlock = this.CurrentBlockReference;
                            if (Secondary != null)
                                drawingBlock = Secondary.CurrentBlockReference;

                            obSettingVM.tem2DRecycle.ForEach(el => Secondary.Entities.Remove(el));
                        }
                        else
                        {
                            if (Secondary != null)
                                drawingBlock = this.CurrentBlockReference;

                            modelBlock = Secondary.CurrentBlockReference;
                            obSettingVM.tem3DRecycle.ForEach(el => Secondary.Entities.Remove(el));
                        }
                        obSettingVM.Reductions.Add(new Reduction() //加入到垃圾桶內
                        {
                            SelectReference = modelBlock,
                            Recycle = new List<List<Entity>>() { obSettingVM.tem3DRecycle.ToList() },
                            User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                        }, new Reduction() //加入到垃圾桶內
                        {
                            SelectReference = drawingBlock,
                            Recycle = new List<List<Entity>>() { obSettingVM.tem2DRecycle.ToList() },
                            User = new List<ACTION_USER>() { ACTION_USER.DELETE }
                        });
                    }
                    log4net.LogManager.GetLogger("在2D按下鍵盤").Debug("Delete");

                    //清空選擇物件
                    obSettingVM.Select2DItem.Clear();
                    obSettingVM.Select3DItem.Clear();
                    //清空圖塊內物件
                    obSettingVM.tem3DRecycle.Clear();
                    obSettingVM.tem3DRecycle.Clear();

                    //20221111 修改 避免按下任意按鍵都會導致連續重新整理
                    STDSerialization ser = new STDSerialization();
                    ser.SetPartModel(obSettingVM.SteelAttr.GUID.ToString(), this);
                    this.Refresh();
                }
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// 連動目前模型的選取物件狀態
        /// </summary>
        /// <param name="model"></param>
        /// <param name="blockReference">要選擇的參考圖塊</param>
        /// <param name="selected">選擇狀態</param>
        private BlockReference Selected(ModelExt model, BlockReference blockReference, bool selected)
        {
            try
            {
                BlockReference result = (BlockReference)model.Entities.Find(el => ((BlockReference)el).BlockName == blockReference.BlockName);
                if (result != null)
                {
                    result.Selected = selected;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 連動目前模型的選取物件狀態
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity">要選擇的物件</param>
        /// <param name="selected">選擇狀態</param>
        private List<Entity> Selected(ModelExt model, Entity entity, bool selected)
        {
            //if (!Secondary.CurrentModel)
            //{
            List<Entity> result = model.Entities.FindAll(el => ((BoltAttr)entity.EntityData).GUID == ((BoltAttr)el.EntityData).GUID);
            result.ForEach(el => el.Selected = selected);
            return result;
            //}
        }
        /// <summary>
        /// 刪除次要模型一個物件
        /// </summary>
        /// <param name="entity">要刪除的物件</param>
        /// <param name="refresh">立即刷新模型</param>
        private void DeleteSecondary(Entity entity, bool refresh = false)
        {
            Secondary.Entities.Remove(entity);
            if (refresh)
            {
                Secondary.Refresh();
            }
        }
        /// <summary>
        /// 存入已捕捉的鎖點列表
        /// </summary>
        private SnapPoint[] snapPoints { get; set; }
        /// <summary>
        /// 設置繪圖平面
        /// </summary>
        /// <param name="plane"></param>
        public void SetDrawingPlan(Plane plane)
        {
            drawingPlane = plane;
        }
        /// <summary>
        /// 清除所有先前的選擇，捕捉信息等。
        /// </summary>
        public void ClearAllPreviousCommandData()
        {
            points.Clear();
            selEntity = null;
            selEntityIndex = -1;
            snapPoint = null;
            drawingArc = false;
            drawingCircle = false;
            drawingCurve = false;
            drawingEllipse = false;
            drawingEllipticalArc = false;
            drawingLine = false;
            drawingLinearDim = false;
            drawingOrdinateDim = false;
            drawingPoints = false;
            drawingText = false;
            drawingLeader = false;
            drawingPolyLine = false;
            drawingRadialDim = false;
            drawingAlignedDim = false;
            drawingQuadrantPoint = false;
            drawingAngularDim = false;
            drawingAngularDimFromLines = false;
            setPlane = false;
            firstClick = true;
            doingMirror = false;
            doingOffset = false;
            doingTrim = false;
            doingExtend = false;
            doingChamfer = false;
            doingMove = false;
            doingScale = false;
            doingRotate = false;
            doingFillet = false;
            doingTangents = false;
            firstSelectedEntity = null;
            secondSelectedEntity = null;

            firstLine = null;
            secondLine = null;
            quadrantPoint = null;
            objectSnapEnabled = false;
            activeOperationLabel = "";
            ActionMode = actionType.SelectByBox;
            Entities.ClearSelection();
            ObjectManipulator.Cancel();

        }

    }
}
