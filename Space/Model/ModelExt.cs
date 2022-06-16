using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Space
{
    /// <summary>
    /// 這是Modell，它將擴展繪圖應用程序所需的行為。
    /// </summary>
    public partial class ModelExt : devDept.Eyeshot.Model
    {
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
        private Plane plane = Plane.XY;

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
        private Plane drawingPlane;

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

        public static Color DrawingColor = Color.Black;
        #endregion

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
                editingModel = doingOffset || doingMirror || doingExtend || doingTrim || doingFillet || doingChamfer || doingTangents;

                ScreenToPlane(mousePos, plane, out current);
                //捕捉物件鎖點
                if (objectSnapEnabled && snapPoint != null)
                {
                    if (!(editingModel && firstClick))
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
                    if (!(editingModel && firstClick))
                    {
                        SnapToGrid(ref current);
                        points.Add(current);
                    }
                }
                else //任意位置
                {
                    if (!(editingModel && firstClick))
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
                else if (drawingLine && points.Count == 3)
                {
                    LinearDim linearDim = new LinearDim(drawingPlane, points[0], points[1], current, dimTextHeight);
                    AddAndRefresh(linearDim, ActiveLayerName);

                    drawingLinearDim = false;
                }
                //對齊式標註
                else if (drawingAlignedDim && points.Count == 3)
                {
                    LinearDim linearDim = new LinearDim(drawingPlane, points[0], points[1], current, dimTextHeight);
                    AddAndRefresh(linearDim, ActiveLayerName);

                    drawingLinearDim = false;
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
                else if (drawingAngularDim)
                {
                    if (!drawingAngularDimFromLines)
                    {
                        if (selEntity is Arc arc && points.Count == 2 && !drawingQuadrantPoint)
                        {
                            Plane plane = (Plane)arc.Plane.Clone();
                            Point3D startPoint = arc.StartPoint,
                                endPoint = arc.EndPoint;

                            //檢查圓弧是否為順時針方向
                            if (Utility.IsOrientedClockwise(arc.Vertices))
                            {
                                plane.Flip();
                                startPoint = arc.EndPoint;
                                endPoint = arc.StartPoint;
                            }

                            AngularDim angularDim = new AngularDim(plane, startPoint, endPoint, points[points.Count - 1], dimTextHeight);

                            angularDim.TextSuffix = "°"; //角度單位

                            AddAndRefresh(angularDim, ActiveLayerName);
                            drawingAngularDim = false;
                        }
                    }
                    ///如果不是時候設置<see cref="quadrantPoint"/>，則添加角度標註的行
                    if (selEntity is Line selectedLine && !drawingQuadrantPoint && quadrantPoint == null)
                    {
                        if (firstLine == null)
                            firstLine = selectedLine;
                        else if (secondLine == null && !ReferenceEquals(firstLine, selectedLine))
                        {
                            secondLine = selectedLine;
                            drawingQuadrantPoint = true;
                            //重置點以僅獲得像限點和文本位置點
                            points.Clear();
                        }
                        drawingAngularDimFromLines = true;
                    }
                    else if (drawingQuadrantPoint)
                    {
                        ScreenToPlane(mousePos, plane, out quadrantPoint);
                        drawingQuadrantPoint = false;
                    }
                    //如果所有參數都存在，則變為角度標註
                    else if (points.Count == 2 && quadrantPoint != null)
                    {
                        AngularDim angularDim = new AngularDim(plane, (Line)firstLine.Clone(), (Line)secondLine.Clone(), quadrantPoint, points[points.Count - 1], dimTextHeight);

                        angularDim.TextSuffix = "°";

                        AddAndRefresh(angularDim, ActiveLayerName);

                        drawingAngularDim = false;
                        drawingAngularDimFromLines = false;
                    }
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

            base.OnMouseDown(e);
            #endregion
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
        /// 滑鼠進入繪圖Modell時所發生的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            cursorOutside = false;
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// 滑鼠在Modell移動時所發生的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //RenderContextUtility 提供渲染上下文實用程序方法的類。
            mouseLocation = RenderContextUtility.ConvertPoint(GetMousePosition(e));//將System.Windows.Point轉換為System.Drawing.Point的方法。
            //如果ObjectSnap為true，則需要找到最接近的頂點（如果有）
            if (objectSnapEnabled)
            {
                this.snapPoint = null;
                snapPoints = GetSnapPoints(mouseLocation);
            }
            //如果start有效且actionModel為None且不在工具欄區域中
            if (current == null || ActionMode != actionType.None || ActiveViewport.ToolBar.Contains(mouseLocation))
            {
                base.OnMouseMove(e);
                return;
            }
            base.OnMouseMove(e);
        }
        /// <summary>
        /// 目前捕捉
        /// </summary>
        bool currentlySnapping = false;
        protected override void DrawOverlay(DrawSceneParams data)
        {
            ScreenToPlane(mouseLocation, plane, out current);

            currentlySnapping = false;

            // 如果ObjectSnap為ON，我們需要找到最接近的頂點（如果有）
            if (objectSnapEnabled && snapPoints != null && snapPoints.Length > 0)
            {
                snap = FindClosestPoint(snapPoints);
                current = snap;
                currentlySnapping = true;
            }

            //為交互式繪圖或彈性線設置GL
            renderContext.SetLineSize(1);

            renderContext.EnableXOR(true);

            renderContext.SetState(depthStencilStateType.DepthTestOff);

            base.DrawOverlay(data);
        }
        /// <summary>
        /// 存入已捕捉的鎖點列表
        /// </summary>
        private SnapPoint[] snapPoints { get; set; }
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

            activeOperationLabel = "";
            ActionMode = actionType.None;
            Entities.ClearSelection();
            ObjectManipulator.Cancel();

        }

        public EntityList EntityList { get; set; }
        public static readonly DependencyProperty MyEntityListProperty = DependencyProperty.Register(
          nameof(EntityList), typeof(EntityList), typeof(ModelExt), new FrameworkPropertyMetadata(default(EntityList), OnMyEntityListChanged));
        private static void OnMyEntityListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vp = (ModelExt)d;
            var oldValue = (EntityList)e.OldValue;
            if (oldValue != null)
                oldValue.Model = null;
            var newValue = (EntityList)e.NewValue;
            if (newValue != null)
                newValue.Model = vp;
        }
    }
}
