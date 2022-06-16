//using System;
//using devDept.Geometry;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using devDept.Eyeshot;
//using devDept.Eyeshot.Entities;
//using View = devDept.Eyeshot.Entities.View;

//namespace WPFSTD105
//{
//    public partial class DrawingsHelper
//    {

//        /// <summary>
//        /// 用於交互式尺寸標註圖的顏色
//        /// </summary>
//        public static Color DrawingColor = Color.Black;

//        #region Properties

//        /// <summary>
//        /// 獲取或設置尺寸圖層名稱
//        /// </summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public string DimensionsLayerName { get; set; }

//        /// <summary>
//        /// 獲取或設置尺寸文本高度。
//        /// </summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public double DimTextHeight { get; set; }

//        #endregion

//        #region Fields  
//        /// <summary>
//        /// 當前視圖轉換
//        /// </summary>
//        private Transformation _transformation;
//        /// <summary>
//        /// 在常規尺寸標註期間第一次單擊之前為true
//        /// </summary>
//        private bool _firstClick = true;
//        /// <summary>
//        /// 工程圖控件
//        /// </summary>
//        private readonly Drawings _drawings;
//        /// <summary>
//        /// 當用戶需要選擇實體進行標註時為true
//        /// </summary>
//        private bool _waitingForSelection;
//        /// <summary>
//        /// 繪製交互式線性尺寸時為true
//        /// </summary>
//        private bool _drawingLinearDim;
//        /// <summary>
//        /// 繪製交互式徑向尺寸時為true
//        /// </summary>
//        private bool _drawingRadialDim;
//        /// <summary>
//        /// 繪製交互式水平縱坐標尺寸時為true
//        /// </summary>
//        private bool _drawingHorizontalOrdinateDim;
//        /// <summary>
//        /// 繪製交互式縱坐標尺寸時為true
//        /// </summary>
//        private bool _drawingVerticalOrdinateDim;
//        /// <summary>
//        /// 從圓弧繪製交互式角度尺寸時為true
//        /// </summary>
//        private bool _drawingAngularDim;
//        /// <summary>
//        /// 從兩條線繪製交互式角度尺寸時為true
//        /// </summary>
//        private bool _drawingAngularDimFromLines;
//        /// <summary>
//        /// 繪製圓弧的中心線或兩線之間時為true
//        /// </summary>
//        private bool _drawingCenterLines;
//        /// <summary>
//        /// 繪製圓弧的中心標記時為true
//        /// </summary>
//        private bool _drawingCenterMarks;
//        /// <summary>
//        /// 在選定視圖中繪製所有圓弧的中心標記時為true
//        /// </summary>
//        private bool _drawingCenterMarksForAllItems;
//        /// <summary>
//        /// 從兩條線繪製交互式中心線時為true
//        /// </summary>
//        private bool _drawingCenterLinesFromLines;
//        /// <summary>
//        /// 視圖的比例因子
//        /// </summary>
//        private double _viewScale = 1;

//        /// <summary>
//        /// 當前選擇/位置
//        /// </summary>
//        private Point3D _current;
//        /// <summary>
//        /// 鼠標光標下的視圖索引
//        /// </summary>
//        private int _currentViewIndex = -1;
//        /// <summary>
//        /// 鼠標光標下視圖的所有頂點的列表
//        /// </summary>
//        private List<Point3D> _viewVertices;

//        /// <summary>
//        /// 當光標在Drawings控件之外時為true
//        /// </summary>
//        private bool _isCursorOutside = true;

//        /// <summary>
//        /// 當前鼠標位置
//        /// </summary>
//        private System.Drawing.Point _mouseLocation;

//        /// <summary>
//        /// <see cref="Plane.XY"/>
//        /// </summary>
//        private readonly Plane _plane = Plane.XY;

//        /// <summary>
//        /// 選定頂點的數組（折點）
//        /// </summary>
//        private Point3D[] _points = new Point3D[3];

//        /// <summary>
//        /// 選定頂點的數組（折點）
//        /// </summary>
//        private int _numPoints;

//        /// <summary>
//        /// 標註時所需的當前工程圖平面和延伸點
//        /// </summary>
//        private Plane _drawingPlane;
//        private Point3D _extPt1;
//        private Point3D _extPt2;

//        private Entity _selectedEntity;
//        private Entity _overlayEntity;
//        private LinearPath _firstLine;
//        private LinearPath _secondLine;
//        private Point3D _quadrantPoint;
//        private bool _drawingQuadrantPoint;
//        private View _currentView;

//        #endregion

//        /// <summary>
//        /// 標準構造函數。
//        /// </summary>
//        /// <param name="drawings">工程圖控件。</param
//        public DrawingsHelper(Drawings drawings)
//        {
//            _drawings = drawings;
//            DimensionsLayerName = "Dimensions";
//            DimTextHeight = 2.5;
//        }
//        /// <summary>
//        /// 禁用註釋並恢復默認變量
//        /// </summary>
//        public void DisableAnnotations()
//        {
//            _drawingCenterLines = false;
//            _drawingCenterMarks = false;
//            _drawingCenterMarksForAllItems = false;
//            _waitingForSelection = false;
//            _secondLine = null;
//            _firstLine = null;
//            _drawings.ActionMode = actionType.SelectVisibleByPickDynamic;
//        }

//        /// <summary>
//        /// 禁用尺寸標註並恢復默認變量
//        /// </summary>
//        public void DisableDimensioning()
//        {
//            DisableAngularDimensioning();
//            _currentView = null;
//            _currentViewIndex = -1;
//            _viewScale = 1;
//            _points = new Point3D[3];
//            _numPoints = 0;
//            _selectedEntity = null;
//            _overlayEntity = null;
//            _drawingLinearDim = false;
//            _drawingRadialDim = false;
//            _drawingHorizontalOrdinateDim = false;
//            _drawingVerticalOrdinateDim = false;
//            _drawingAngularDim = false;
//            _firstClick = true;
//            _waitingForSelection = false;
//            _drawings.ActionMode = actionType.SelectVisibleByPickDynamic;
//        }
//        /// <summary>
//        /// 恢復角度標註的默認變量
//        /// </summary>
//        public void DisableAngularDimensioning()
//        {
//            _drawingAngularDimFromLines = false;
//            _firstLine = null;
//            _secondLine = null;
//            _quadrantPoint = null;
//            _drawingQuadrantPoint = false;
//        }
//        /// <summary>
//        /// 啟用中心線
//        /// </summary>
//        public void EnableCenterLines()
//        {
//            DisableDimensioning();
//            _drawingCenterLines = true;
//            _waitingForSelection = true;

//        }
//        /// <summary>
//        /// 啟用尺寸標註
//        /// </summary>
//        public void EnableDimensioning(dimensionType dimType)
//        {
//            DisableDimensioning();
//            switch (dimType)
//            {
//                case dimensionType.Linear:
//                    _drawingLinearDim = true;
//                    break;
//                case dimensionType.OrdinateHor:
//                    _drawingHorizontalOrdinateDim = true;
//                    break;
//                case dimensionType.OrdinateVer:
//                    _drawingVerticalOrdinateDim = true;
//                    break;
//                case dimensionType.Radial:
//                    _drawingRadialDim = true;
//                    _waitingForSelection = true;
//                    break;
//                case dimensionType.Angular:
//                    _drawingAngularDim = true;
//                    _waitingForSelection = true;
//                    break;
//            }

//            _drawings.ActionMode = actionType.None;
//        }

//        /// <summary>
//        /// 啟用註釋
//        /// </summary>
//        public void EnableAnnotations(annotationType annotationType, bool centerMarksForAllItems)
//        {

//            DisableAnnotations();
//            DisableDimensioning();
//            switch (annotationType)
//            {
//                case annotationType.CenterMark:
//                    {
//                        if (centerMarksForAllItems)
//                        {
//                            _drawingCenterMarksForAllItems = true;
//                            _waitingForSelection = true;
//                        }
//                        else
//                        {
//                            _drawingCenterMarks = true;
//                            _waitingForSelection = true;
//                        }

//                    }
//                    break;
//                case annotationType.CenterLine:
//                    _drawingCenterLines = true;
//                    _waitingForSelection = true;

//                    break;
//            }
//            if (_drawingCenterMarksForAllItems)
//                _drawings.ActionMode = actionType.SelectVisibleByPickDynamic;
//            else
//                _drawings.ActionMode = actionType.None;
//        }

//        /// <summary>
//        /// 將尺寸實體添加到當前視圖中 <see cref="DimensionsLayerName"/> 圖層.
//        /// </summary>
//        /// <param name="dimension">The entity to add.</param>
//        /// <remarks>如果 <see cref="DimensionsLayerName"/> 不存在，它將創建它，並將其添加到圖形的圖層集合中。</remarks>
//        /// <seealso cref="AddDimensionsLayer"/>
//        public void AddDimension(Dimension dimension)
//        {
//            AddDimensionsLayer();
//            dimension.LayerName = DimensionsLayerName;

//            Transformation inverseTransformation = _transformation.Clone() as Transformation;
//            inverseTransformation.Invert();
//            dimension.TransformBy(inverseTransformation);
//            _drawings.Blocks[_currentView.BlockName].Entities.Add(dimension, DimensionsLayerName);
//            _currentView.UpdateBoundingBox(new TraversalParams(_drawings.Blocks));
//            _drawings.Invalidate();

//            DisableDimensioning();
//        }

//        /// <summary>
//        /// 將 <see cref="DimensionsLayerName"/> 圖層添加到圖形的圖層集合（如果尚不存在）。
//        /// </summary>
//        /// <seealso cref="AddDimension"/>
//        public void AddDimensionsLayer()
//        {
//            if (!_drawings.Layers.Contains(DimensionsLayerName)) _drawings.Layers.Add(new Layer(DimensionsLayerName, Color.Blue) { LineWeight = 0.15f });
//        }

//        /// <summary>
//        /// 繪製線條的水平/垂直尺寸預覽
//        /// </summary>
//        /// <param name="drawText"></param>
//        private void DrawInteractiveLinearDim(DrawText drawText)
//        {
//            // 繪製交互式LinearDim需要2點
//            if (_numPoints < 2)
//                return;

//            bool verticalDim = (_current.X > _points[0].X && _current.X > _points[1].X) || (_current.X < _points[0].X && _current.X < _points[1].X);

//            Vector3D axisX;

//            double convertedDimTextHeight = DimTextHeight * GetUnitsConversionFactor();

//            if (verticalDim)
//            {
//                axisX = Vector3D.AxisY;

//                _extPt1 = new Point3D(_current.X, _points[0].Y);
//                _extPt2 = new Point3D(_current.X, _points[1].Y);

//                if (_current.X > _points[0].X && _current.X > _points[1].X)
//                {
//                    _extPt1.X += convertedDimTextHeight / 2;
//                    _extPt2.X += convertedDimTextHeight / 2;
//                }
//                else
//                {
//                    _extPt1.X -= convertedDimTextHeight / 2;
//                    _extPt2.X -= convertedDimTextHeight / 2;
//                }

//            }
//            else // 用於水平LinearDim
//            {
//                axisX = Vector3D.AxisX;

//                _extPt1 = new Point3D(_points[0].X, _current.Y);
//                _extPt2 = new Point3D(_points[1].X, _current.Y);

//                if (_current.Y > _points[0].Y && _current.Y > _points[1].Y)
//                {
//                    _extPt1.Y += convertedDimTextHeight / 2;
//                    _extPt2.Y += convertedDimTextHeight / 2;
//                }
//                else
//                {
//                    _extPt1.Y -= convertedDimTextHeight / 2;
//                    _extPt2.Y -= convertedDimTextHeight / 2;
//                }
//            }

//            // 定義Y軸
//            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);

//            List<Point3D> pts = new List<Point3D>();

//            // 繪製尺寸界線1
//            pts.Add(_drawings.WorldToScreen(_points[0]));
//            pts.Add(_drawings.WorldToScreen(_extPt1));

//            // 繪製尺寸界線2
//            pts.Add(_drawings.WorldToScreen(_points[1]));
//            pts.Add(_drawings.WorldToScreen(_extPt2));

//            // 繪製尺寸線
//            Segment3D extLine1 = new Segment3D(_points[0], _extPt1);
//            Segment3D extLine2 = new Segment3D(_points[1], _extPt2);
//            Point3D pt1 = _current.ProjectTo(extLine1);
//            Point3D pt2 = _current.ProjectTo(extLine2);

//            pts.Add(_drawings.WorldToScreen(pt1));
//            pts.Add(_drawings.WorldToScreen(pt2));

//            _drawings.renderContext.DrawLines(pts.ToArray());

//            // stores dimensioning plane
//            _drawingPlane = new Plane(_points[0], axisX, axisY);

//            // draws dimension text
//            _drawings.renderContext.EnableXOR(false);

//            // calculates the scaled distance
//            var scaledDistance = _extPt1.DistanceTo(_extPt2) * (1 / _viewScale);
//            string dimText = "L " + scaledDistance.ToString("f3");

//#if WINFORMS
//            drawText(_mouseLocation.X, _drawings.Height - _mouseLocation.Y + 10, dimText,
//                new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#else
//            drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10, dimText,
//                new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#endif
//        }
//        /// <summary>
//        /// 繪製圓弧的徑向尺寸預覽
//        /// </summary>
//        /// <param name="drawText"></param>
//        private void DrawInteractiveRadialDim(DrawText drawText)
//        {
//            if (_selectedEntity != null && _selectedEntity is Circle) //圓弧是一個圓
//            {
//                Circle c = (Circle)_selectedEntity;

//                Circle temp = (Circle)c.Clone();

//                if (_transformation != null)
//                {
//                    temp.TransformBy(_transformation);
//                }
//                _drawings.renderContext.EnableXOR(true);
//                //繪製中心標記
//                DrawPositionMark(temp.Center);

//                //在中心和光標點之間繪製彈性線
//                Point3D current;
//                _drawings.ScreenToPlane(_mouseLocation, Plane.XY, out current);
//                _drawings.renderContext.DrawLine(_drawings.WorldToScreen(temp.Center), _drawings.WorldToScreen(current));

//                string dimText = "R" + (temp.Radius / ((View)_currentView).Scale).ToString("f3");

//                if (c is Arc && ((Arc)c).IsCircle)

//                    dimText = "D" + (temp.Diameter / ((View)_currentView).Scale).ToString("f3");

//#if WINFORMS
//                drawText(_mouseLocation.X, _drawings.Height - _mouseLocation.Y + 10, dimText,
//                    new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#else
//                drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10, dimText,
//                    new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#endif
//            }
//            _drawings.renderContext.EnableXOR(false);

//        }

//        /// <summary>
//        /// 繪製弧的角度尺寸預覽
//        /// </summary>
//        /// <param name="drawText"></param>
//        private void DrawInteractiveAngularDim(DrawText drawText)
//        {
//            if (_drawingAngularDimFromLines && _quadrantPoint != null)
//            {
//                Point3D current;
//                _drawings.ScreenToPlane(_mouseLocation, Plane.XY, out current);
//                _drawingPlane = Plane.XY;
//                //繪製象限點標記
//                DrawPositionMark(_quadrantPoint);

//                //在像限點和光標點之間繪製彈性線
//                _drawings.renderContext.DrawLine(_drawings.WorldToScreen(_quadrantPoint),
//                        _drawings.WorldToScreen(current));

//                // 在像限點和光標點之間插入彈性線
//                _drawings.renderContext.EnableXOR(false);
//#if WINFORMS
//                    drawText(_mouseLocation.X, _drawings.Height - _mouseLocation.Y + 10, "",
//                            new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#else
//                drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10, "",
//                        new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#endif
//            }
//        }

//        /// <summary>
//        /// 繪製特定點的縱坐標尺寸的預覽
//        /// </summary>
//        /// <param name="drawText"></param>
//        private void DrawInteractiveOrdinateDim(DrawText drawText)
//        {
//            // 我們需要至少有一點。
//            if (_points[0] == null)
//                return;

//            List<Point3D> pts = new List<Point3D>();
//            Point3D leaderEndPoint;
//            Point3D current;
//            _drawings.ScreenToPlane(_mouseLocation, Plane.XY, out current);
//            double unitsConversionFactor = GetUnitsConversionFactor();
//            Segment3D[] segments = OrdinateDim.Preview(Plane.XY, _points[0], current, _drawingVerticalOrdinateDim, DimTextHeight * unitsConversionFactor, DimTextHeight * unitsConversionFactor, 0.625 * unitsConversionFactor, 3.0 * unitsConversionFactor, 0.625 * unitsConversionFactor, out leaderEndPoint);

//            foreach (Segment3D segment3D in segments)
//            {
//                pts.Add(_drawings.WorldToScreen(segment3D.P0));
//                pts.Add(_drawings.WorldToScreen(segment3D.P1));
//            }

//            //畫線段
//            _drawings.renderContext.DrawLines(pts.ToArray());

//            //繪製尺寸文字
//            _drawings.renderContext.EnableXOR(false);

//            double distance = _drawingVerticalOrdinateDim ? Math.Abs(Plane.XY.Origin.X - _points[0].X / GetUnitsConversionFactor()) : Math.Abs(Plane.XY.Origin.Y - _points[0].Y / GetUnitsConversionFactor());
//            distance = distance * (1 / _viewScale);
//            string dimText = "D " + distance.ToString("f3");
//#if WINFORMS
//            drawText(_mouseLocation.X, _drawings.Height - _mouseLocation.Y + 10, dimText,
//                new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#else
//            drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10, dimText,
//                new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//#endif
//        }

//        private double GetUnitsConversionFactor()
//        {
//            return Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, _drawings.ActiveSheet != null ? _drawings.ActiveSheet.Units : linearUnitsType.Millimeters);
//        }
//    }
//    /// <summary>
//    /// 工作表格式類型。
//    /// </summary>
//    public enum formatType
//    {
//        A0_ISO, 
//        A1_ISO, 
//        A2_ISO, 
//        A3_ISO, 
//        A4_ISO, 
//        A4_LANDSCAPE_ISO, 
//        A_ANSI, 
//        A_LANDSCAPE_ANSI, 
//        B_ANSI, 
//        C_ANSI, 
//        D_ANSI,
//        E_ANSI
//    }
//    /// <summary>
//    /// 尺寸類型。
//    /// </summary>
//    public enum dimensionType
//    {
//        /// <summary>
//        /// 線性
//        /// </summary>
//        Linear,
//        /// <summary>
//        /// 徑向
//        /// </summary>
//        Radial,
//        /// <summary>
//        /// 縱向
//        /// </summary>
//        OrdinateHor,
//        /// <summary>
//        /// 水平
//        /// </summary>
//        OrdinateVer,
//        /// <summary>
//        /// 角度
//        /// </summary>
//        Angular

//    }
//    /// <summary>
//    /// 註釋類型。
//    /// </summary>
//    public enum annotationType
//    {
//        /// <summary>
//        /// 中心標註
//        /// </summary>
//        CenterMark, 
//        /// <summary>
//        /// 中心線
//        /// </summary>
//        CenterLine
//    }

//}
