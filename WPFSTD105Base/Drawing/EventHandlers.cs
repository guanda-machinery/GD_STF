//using devDept.Eyeshot;
//using devDept.Eyeshot.Entities;
//using devDept.Geometry;
//using devDept.Graphics;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Input;
//using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
//using Point3D = devDept.Geometry.Point3D;
//using Vector3D = devDept.Geometry.Vector3D;
//using View = devDept.Eyeshot.Entities.View;

//namespace WPFSTD105
//{
//    public partial class DrawingsHelper
//    {
//        public delegate Size DrawText(int x, int y, string text, Font textFont, Color textColor, ContentAlignment textAlign);

//        public delegate void PaintBackBuffer();

//        public delegate void SwapBuffers();

//        public void DrawOverlay(DrawText drawText)
//        {
//            if (_isCursorOutside)
//                return;
//            if (_drawingLinearDim)
//            {
//                _drawings.ScreenToPlane(_mouseLocation, _plane, out _current);

//                if (_snappedPoint != null)
//                {
//                    DisplaySnappedVertex();
//                }


//                SetRenderContextStyle();
//                // 如果光標在圖形外部，則不需要在疊加層上繪製任何內容
//                if (!_isCursorOutside)
//                {
//                    DrawPositionMark(_current);

//                    DrawSelectionMark(_mouseLocation);

//                    if (_numPoints < 2 && _drawingLinearDim)
//                    {

//                        _drawings.renderContext.EnableXOR(false);
//                        string text = "Select the first point";
//                        if (_numPoints == 1)
//                            text = "Select the second point";


//                        drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                            text, new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    }

//                    DrawInteractiveLinearDim(drawText);

//                }
//            }
//            else if ((_drawingHorizontalOrdinateDim ||
//                      _drawingVerticalOrdinateDim))
//            {
//                _drawings.ScreenToPlane(_mouseLocation, _plane, out _current);

//                if (_snappedPoint != null)
//                {
//                    DisplaySnappedVertex();
//                }

//                SetRenderContextStyle();

//                // 如果光標在圖形外部，則不需要在疊加層上繪製任何內容
//                if (!_isCursorOutside)
//                {

//                    if (_numPoints == 1)
//                    {
//                        DrawPositionMark(_current, 5);
//                        DrawInteractiveOrdinateDim(drawText);
//                    }
//                    else
//                    {
//                        DrawPositionMark(_current);
//                        _drawings.renderContext.EnableXOR(false);
//                        string text = "Select the definition point";
//                        if (!_firstClick)
//                            text = "Select the leader end point";

//                        drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                            text, new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
          
//                        _drawings.renderContext.EnableXOR(true);
//                    }
//                }
//            }
//            else if ((_drawingRadialDim) && _waitingForSelection)
//            {
//                DrawSelectionMark(_mouseLocation);
//                //選擇圓
//                if (_overlayEntity != null && _transformation != null)
//                {
//                    if (_overlayEntity is Circle)
//                        DisplaySnappedCircle();
//                }



//                _drawings.renderContext.EnableXOR(false);

//                drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select Arc or Circle", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                _drawings.renderContext.EnableXOR(true);
//            }
//            else if ((_drawingAngularDim) && _waitingForSelection)
//            {
//                DrawSelectionMark(_mouseLocation);
//                //選擇圓
//                if (_overlayEntity != null && _transformation != null)
//                {
//                    if (_overlayEntity is Circle)
//                        DisplaySnappedCircle();
//                    else
//                        DisplaySnappedLine();
//                }
//                if (!_drawingAngularDimFromLines)
//                {
//                    _drawings.renderContext.EnableXOR(false);

//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select Arc or Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }
//                else if (_quadrantPoint == null && !_drawingQuadrantPoint)
//                {


//                    _drawings.renderContext.EnableXOR(false);

//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select second Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }
//                else if (_drawingQuadrantPoint)
//                {
//                    _drawings.renderContext.EnableXOR(false);

//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select a quadrant", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }

//            }
//            else if (_drawingRadialDim && !_waitingForSelection)
//            {
//                DrawSelectionMark(_mouseLocation);
//                //放置徑向標註
//                DrawInteractiveRadialDim(drawText);
//            }
//            else if (_drawingAngularDim && !_waitingForSelection)
//            {
//                DrawSelectionMark(_mouseLocation);
//                if (_quadrantPoint != null || _selectedEntity is Circle)
//                {

//                    _drawings.renderContext.EnableXOR(false);

//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select text position", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }
//                //放置徑向標註
//                DrawInteractiveAngularDim(drawText);
//            }
//            else if ((_drawingCenterLines) && _waitingForSelection)
//            {
//                DrawSelectionMark(_mouseLocation);
//                //選擇圈子
//                if (_overlayEntity != null && _transformation != null)
//                {
//                    if (_overlayEntity is LinearPath && _overlayEntity.Vertices.Length == 2)
//                        DisplaySnappedLine();
//                }
//                if (_firstLine == null)
//                {

//                    _drawings.renderContext.EnableXOR(false);


//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select the first Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }
//                else
//                {

//                    _drawings.renderContext.EnableXOR(false);

//                    drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                        "Select the second Line", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//                    _drawings.renderContext.EnableXOR(true);
//                }

//            }
//            else if ((_drawingCenterMarks || _drawingCenterMarksForAllItems) && _waitingForSelection)
//            {


//                if (!_drawingCenterMarksForAllItems)
//                    DrawSelectionMark(_mouseLocation);

//                if (_overlayEntity != null && _transformation != null && _overlayEntity is Circle)
//                {
//                    DisplaySnappedCircle();
//                }
//                _drawings.renderContext.EnableXOR(false);

//                string text = _drawingCenterMarksForAllItems ? "Select a Vector View" : "Select an Arc";

//                drawText(_mouseLocation.X, _drawings.Size.Height - _mouseLocation.Y + 10,
//                    text, new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
//            }
//        }



//        // 設置交互式繪圖的渲染上下文
//        private void SetRenderContextStyle()
//        {

//            _drawings.renderContext.SetLineSize(1);

//            _drawings.renderContext.EnableXOR(true);

//            _drawings.renderContext.SetState(depthStencilStateType.DepthTestOff);
//        }

//        public void OnMouseEnter()
//        {
//            _isCursorOutside = false;
//        }

//        public void OnMouseLeave()
//        {
//            _isCursorOutside = true;

//            _drawings.Invalidate();
//        }

//        /// <summary>
//        /// 尺寸標註的輔助方法。
//        /// </summary>
//        /// <returns>將尺寸添加到場景時為true，否則為false。</returns>
//        public bool OnMouseDown(MouseEventArgs e)
//        {
//            double unitsConversionFactor = GetUnitsConversionFactor();
//            if (_drawings.ToolBar.Contains(e.Location))
//            {
//                return false;
//            }
//            bool dimAdded = false;

//            if (_drawingLinearDim)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int index = _drawings.GetEntityUnderMouseCursor(_mouseLocation);
//                    if (index != -1)
//                    {
//                        _currentView = _drawings.Entities[index] as View;
//                    }

//                    if (_numPoints < 2)
//                    {
//                        if (_snappedPoint != null)
//                        {
//                            _points[_numPoints++] = _snappedPoint; // adds the snapped point to the list of points
//                        }

//                        if (_numPoints == 1)
//                        {
//                            index = _drawings.GetEntityUnderMouseCursor(_mouseLocation);
//                            if (index != -1)
//                            {
//                                _currentView = _drawings.Entities[index] as View;
//                                _transformation = _currentView.GetFullTransformation(_drawings.Blocks);
//                                if (_currentView != null)
//                                    _viewScale = ((View)_currentView).Scale;
//                                else
//                                    _viewScale = 1;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // 以下幾行需要將LinearDim添加到工程圖中
//                        _drawings.ScreenToPlane(e.Location, _plane, out _current);
//                        var linearDim = new LinearDim(_drawingPlane, _points[0],
//                            _points[1], _current, DimTextHeight * unitsConversionFactor);
//                        AddDimension(linearDim);
//                        dimAdded = true;


//                    }
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                {
//                    DisableDimensioning();
//                }
//            }
//            else if ((_drawingHorizontalOrdinateDim || _drawingVerticalOrdinateDim))
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    if (_numPoints < 1)
//                    {
//                        if (_snappedPoint != null)
//                        {
//                            _points[_numPoints++] = _snappedPoint; // 將捕捉的點添加到點列表中
//                        }
//                        int index = _drawings.GetEntityUnderMouseCursor(_mouseLocation);
//                        if (index != -1)
//                        {
//                            View view = _drawings.Entities[index] as View;
//                            _currentView = view;
//                            _transformation = view.GetFullTransformation(_drawings.Blocks);
//                            _viewScale = view.Scale;

//                        }
//                    }

//                    else if (_numPoints == 1 && !_firstClick)
//                    {
//                        Point3D pos;
//                        _drawings.ScreenToPlane(e.Location, Plane.XY, out pos);
//                        OrdinateDim ordinateDim = new OrdinateDim(Plane.XY, _points[0], pos, _drawingVerticalOrdinateDim, DimTextHeight * unitsConversionFactor);
//                        AddDimension(ordinateDim);
//                        dimAdded = true;

//                    }


//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                {
//                    _points = new Point3D[3];
//                    _numPoints = 0;
//                    _viewScale = 1;
//                    DisableDimensioning();
//                }
//            }
//            else if (_drawingRadialDim && !_waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    if (_selectedEntity is Circle)
//                    {
//                        Circle c = (Circle)_selectedEntity;

//                        Circle temp = (Circle)c.Clone();

//                        if (_transformation != null)
//                            temp.TransformBy(_transformation);

//                        Point3D pos;
//                        _drawings.ScreenToPlane(e.Location, Plane.XY, out pos);

//                        if (temp is Arc && ((Arc)temp).IsCircle == false)
//                        {
//                            RadialDim radialDim = new RadialDim(temp, pos, DimTextHeight * unitsConversionFactor);
//                            AddDimension(radialDim);
//                        }
//                        else
//                        {
//                            DiametricDim diamDim = new DiametricDim(temp, pos, DimTextHeight * unitsConversionFactor);
//                            AddDimension(diamDim);
//                        }
//                        dimAdded = true;
//                    }
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                    DisableDimensioning();

//            }
//            else if (_drawingAngularDim && !_waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {

//                    if (!_drawingAngularDimFromLines)
//                    {
//                        if (_selectedEntity is Arc && !((Arc)_selectedEntity).IsCircle && !_drawingQuadrantPoint)
//                        {
//                            Arc arc = (Arc)_selectedEntity;
//                            Point3D pos;

//                            _drawings.ScreenToPlane(e.Location, Plane.XY, out pos);

//                            Plane myPlane = (Plane)arc.Plane.Clone();
//                            bool clockwise = Utility.IsOrientedClockwise(arc.Vertices);
//                            Point3D startPoint =
//                                clockwise ? (Point3D)arc.EndPoint.Clone() : (Point3D)arc.StartPoint.Clone();
//                            Point3D endPoint =
//                                clockwise ? (Point3D)arc.StartPoint.Clone() : (Point3D)arc.EndPoint.Clone();


//                            // 檢查圓弧是否為順時針方向
//                            if (clockwise)
//                            {
//                                myPlane.Flip();
//                            }

//                            if (_transformation != null)
//                            {
//                                startPoint.TransformBy(_transformation);
//                                endPoint.TransformBy(_transformation);
//                                myPlane.TransformBy(_transformation);
//                            }
//                            AngularDim angularDim = new AngularDim(myPlane, startPoint, endPoint, pos, DimTextHeight * unitsConversionFactor);

//                            AddDimension(angularDim);

//                            dimAdded = true;
//                            DisableAngularDimensioning();
//                        }
//                        else
//                        {
//                            DisableDimensioning();
//                            DisableAngularDimensioning();
//                        }
//                    }

//                    //如果存在所有參數，則角度標註
//                    if (_quadrantPoint != null)
//                    {
//                        Point3D pos;
//                        _drawings.ScreenToPlane(e.Location, _plane, out pos);
//                        LinearPath first = (LinearPath)_firstLine.Clone();
//                        first.TransformBy(_transformation);
//                        LinearPath second = (LinearPath)_secondLine.Clone();
//                        second.TransformBy(_transformation);
//                        AngularDim angularDim = new AngularDim(_drawingPlane, new Line(first.StartPoint, first.EndPoint), new Line(second.StartPoint, second.EndPoint),
//                            _quadrantPoint, pos, DimTextHeight * unitsConversionFactor);

//                        AddDimension(angularDim);
//                        dimAdded = true;
//                        DisableAngularDimensioning();
//                    }
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                    DisableDimensioning();
//            }
//            else if (_drawingRadialDim && _waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                    if (viewIndex != -1)
//                    {
//                        View view = (View)_drawings.Entities[viewIndex];

//                        if (view != null)
//                            _viewScale = view.Scale;
//                        else
//                            _viewScale = 1;

//                        _currentView = (View)_drawings.Entities[viewIndex];

//                        Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);
//                        Entity ent = GetEntity(_mouseLocation, _currentView.GetEntities(_drawings.Blocks).Where(c => c is Circle).ToList(),
//                            accumulatedParentTransform.Clone() as Transformation);
//                        if (ent != null && ent is Circle)
//                        {
//                            _selectedEntity = ent;

//                            _transformation = accumulatedParentTransform;
//                        }


//                    }

//                    _waitingForSelection = false;
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                    DisableDimensioning();
//            }
//            else if (_drawingAngularDim && _waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                    if (viewIndex != -1)
//                    {
//                        View view = (View)_drawings.Entities[viewIndex];

//                        if (view != null)
//                            _viewScale = view.Scale;
//                        else
//                            _viewScale = 1;

//                        _currentView = (View)_drawings.Entities[viewIndex];

//                        Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);

//                        Entity ent = GetEntity(_mouseLocation, _currentView.GetEntities(_drawings.Blocks),
//                            accumulatedParentTransform.Clone() as Transformation);

//                        if (ent != null && ent is LinearPath && ent.Vertices.Length == 2)
//                        {
//                            _selectedEntity = ent;
//                            _transformation = accumulatedParentTransform;
//                        }
//                        else if (ent != null && ent is Arc)
//                        {
//                            _selectedEntity = ent;
//                            _transformation = accumulatedParentTransform;
//                        }
//                        if (_selectedEntity is Arc && ((Arc)_selectedEntity).IsCircle)
//                        {
//                            DisableAngularDimensioning();
//                            DisableDimensioning();
//                        }
//                    }

//                    // 如果不是時候設置QuadrantPoint，則添加角度變暗的行
//                    if (_selectedEntity is LinearPath && _selectedEntity.Vertices.Length == 2 && !_drawingQuadrantPoint && _quadrantPoint == null)
//                    {
//                        LinearPath selectedLine = (LinearPath)_selectedEntity;

//                        if (_firstLine == null)
//                            _firstLine = selectedLine;
//                        else if (_secondLine == null && !ReferenceEquals(_firstLine, selectedLine))
//                        {
//                            _secondLine = selectedLine;
//                            _drawingQuadrantPoint = true;
//                            // 重置點以獲得僅象限點和文本位置點
//                            _points = new Point3D[3];
//                            _overlayEntity = null;
//                            _selectedEntity = null;
//                        }

//                        _drawingAngularDimFromLines = true;
//                    }
//                    else if (_drawingQuadrantPoint)
//                    {
//                        _drawings.ScreenToPlane(e.Location, Plane.XY, out _quadrantPoint);
//                        _drawingQuadrantPoint = false;
//                    }

//                    if (_selectedEntity is Arc || (_firstLine != null && _secondLine != null && _quadrantPoint != null))
//                    {
//                        _waitingForSelection = false;
//                    }
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                {
//                    DisableAngularDimensioning();
//                    DisableDimensioning();
//                }

//            }
//            else if (_drawingCenterLines && _waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                    if (viewIndex != -1)
//                    {
//                        View view = (View)_drawings.Entities[viewIndex];

//                        if (view != null)
//                            _viewScale = view.Scale;
//                        else
//                            _viewScale = 1;

//                        _currentView = (View)_drawings.Entities[viewIndex];

//                        Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);

//                        Entity ent = GetEntity(_mouseLocation, _currentView.GetEntities(_drawings.Blocks),
//                            accumulatedParentTransform.Clone() as Transformation);

//                        if (ent != null && ent is LinearPath && ent.Vertices.Length == 2)
//                        {
//                            _selectedEntity = ent;
//                            _transformation = accumulatedParentTransform;
//                        }
//                        else if (ent != null && ent is Arc)
//                        {
//                            _selectedEntity = ent;
//                            _transformation = accumulatedParentTransform;
//                        }


//                    }

//                    //如果不是時候設置QuadrantPoint，則添加角度標註
//                    if (_selectedEntity is LinearPath)
//                    {
//                        LinearPath selectedLine = (LinearPath)_selectedEntity;

//                        if (_firstLine == null)
//                            _firstLine = selectedLine;
//                        else if (_secondLine == null && !ReferenceEquals(_firstLine, selectedLine))
//                        {
//                            _secondLine = selectedLine;

//                            // 重置點以獲得僅象限點和文本位置點
//                            _points = new Point3D[3];
//                            _overlayEntity = null;
//                            _selectedEntity = null;
//                        }

//                        _drawingCenterLinesFromLines = true;
//                    }

//                    if (_selectedEntity is Arc || (_firstLine != null && _secondLine != null))
//                    {
//                        _waitingForSelection = false;
//                    }

//                    if (!_waitingForSelection && _drawingCenterLinesFromLines)
//                    {
//                        LinearPath first = (LinearPath)_firstLine.Clone();
//                        LinearPath second = (LinearPath)_secondLine.Clone();
//                        if (_currentView is VectorView)
//                        {
//                            AddCenterLines(first, second, (VectorView)_currentView);
//                            DisableDimensioning();
//                        }
//                        dimAdded = true;
//                        DisableDimensioning();
//                        DisableAnnotations();
//                    }
//                    else if (!_waitingForSelection && !_drawingCenterLinesFromLines)
//                    {
//                        AddCenterMark((Arc)_selectedEntity, (VectorView)_currentView);
//                        dimAdded = true;
//                        DisableDimensioning();
//                        DisableAnnotations();


//                    }

//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                {
//                    DisableDimensioning();
//                }

//            }
//            else if (_drawingCenterMarks && _waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);
//                    if (viewIndex != -1)
//                    {
//                        View view = (View)_drawings.Entities[viewIndex];

//                        if (view != null)
//                            _viewScale = view.Scale;
//                        else
//                            _viewScale = 1;

//                        _currentView = (View)_drawings.Entities[viewIndex];

//                        Transformation accumulatedParentTransform =
//                            _currentView.GetFullTransformation(_drawings.Blocks);
//                        Entity ent = GetEntity(_mouseLocation,
//                            _currentView.GetEntities(_drawings.Blocks).Where(c => c is Circle).ToList(),
//                            accumulatedParentTransform.Clone() as Transformation);
//                        if (ent != null && ent is Circle)
//                        {
//                            _selectedEntity = ent;
//                            _transformation = accumulatedParentTransform;
//                        }

//                        if (_selectedEntity is Arc)
//                            AddCenterMark((Arc)_selectedEntity, (VectorView)_currentView);
//                        DisableDimensioning();
//                        DisableAnnotations();

//                    }

//                    _waitingForSelection = false;
//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                {
//                    DisableAnnotations();
//                    DisableDimensioning();
//                }
//            }
//            else if (_drawingCenterMarksForAllItems && _waitingForSelection)
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                    if (viewIndex != -1)
//                    {
//                        View view = (View)_drawings.Entities[viewIndex];

//                        if (view != null)
//                            _viewScale = view.Scale;
//                        else
//                            _viewScale = 1;

//                        _currentView = view;
//                        if (_currentView is VectorView)
//                        {
//                            AddCenterMarksForAllItems((VectorView)_currentView);
//                        }
//                        DisableAnnotations();
//                        DisableDimensioning();
//                        dimAdded = true;
//                    }

//                }
//                else if (e.Button == MouseButtons.Right) // 重新開始標註
//                    DisableDimensioning();
//            }

//            if (_drawingHorizontalOrdinateDim || _drawingVerticalOrdinateDim)
//                _firstClick = false;

//            return dimAdded;
//        }


//        public void OnMouseMove(System.Drawing.Point mouseLocation, PaintBackBuffer paintBackBuffer,
//            SwapBuffers swapBuffers)

//        {
//            // 保存當前鼠標位置
//            _mouseLocation = mouseLocation;

//            if ((_drawingLinearDim || _drawingHorizontalOrdinateDim || _drawingVerticalOrdinateDim) &&
//                _numPoints < 2)
//            {
//                // 保存當前鼠標位置
//                _mouseLocation = mouseLocation;

//                _snappedPoint = null;
//                int index = _drawings.GetEntityUnderMouseCursor(_mouseLocation);
//                if (index != -1 && (_drawings.Entities[index] is VectorView))
//                {
//                    VectorView view = _drawings.Entities[index] as VectorView;

//                    _viewScale = view.Scale;

//                    _snappedPoint = FindSnappedPoint(view, _mouseLocation, 1, index != _currentViewIndex);

//                    _currentViewIndex = index;

//                }


//            }
//            else if ((_drawingRadialDim || (_drawingCenterMarks && !_drawingCenterMarksForAllItems) && _waitingForSelection))
//            {

//                int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                if (viewIndex != -1 && _drawings.Entities[viewIndex] is VectorView)
//                {
//                    _currentView = (VectorView)_drawings.Entities[viewIndex];

//                    Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);

//                    Entity ent = GetEntity(_mouseLocation, _currentView.GetEntities(_drawings.Blocks).Where(c => c is Circle).ToList(),
//                        accumulatedParentTransform.Clone() as Transformation);
//                    if (ent != null && IsHidden(ent))
//                        return;

//                    else if (ent is Circle)
//                    {
//                        _overlayEntity = ent;
//                        _transformation = accumulatedParentTransform;
//                    }
//                    else
//                    {
//                        _overlayEntity = null;

//                    }

//                }
//                else
//                {
//                    _overlayEntity = null;
//                }
//            }

//            else if (_drawingAngularDim && _waitingForSelection && (_quadrantPoint == null && !_drawingQuadrantPoint))
//            {

//                int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                if (viewIndex != -1 && _drawings.Entities[viewIndex] is VectorView)
//                {
//                    _currentView = (VectorView)_drawings.Entities[viewIndex];

//                    Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);

//                    Entity ent = GetEntity(mouseLocation, _currentView.GetEntities(_drawings.Blocks),
//                        accumulatedParentTransform.Clone() as Transformation);
//                    if (ent != null && IsHidden(ent))
//                        return;

//                    if ((ent is LinearPath && ent.Vertices.Length == 2) || ent is Circle)
//                    {
//                        _overlayEntity = ent;
//                        _transformation = accumulatedParentTransform;
//                    }
//                    else
//                    {
//                        _overlayEntity = null;
//                        _transformation = null;

//                    }

//                }
//                else
//                {
//                    _overlayEntity = null;
//                }

//            }
//            else if (_drawingCenterLines)
//            {
//                int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                if (viewIndex != -1 && _drawings.Entities[viewIndex] is VectorView)
//                {
//                    _currentView = (VectorView)_drawings.Entities[viewIndex];

//                    Transformation accumulatedParentTransform = _currentView.GetFullTransformation(_drawings.Blocks);

//                    Entity ent = GetEntity(mouseLocation, _currentView.GetEntities(_drawings.Blocks),
//                        accumulatedParentTransform.Clone() as Transformation);
//                    if (ent != null && IsHidden(ent))
//                        return;
//                    if (ent != null)
//                    {

//                    }
//                    if (ent is LinearPath)
//                    {
//                        LinearPath lp = (LinearPath)ent;

//                        if (lp.Vertices.Length == 2)
//                        {
//                            _transformation = accumulatedParentTransform;
//                            _overlayEntity = ent;
//                        }
//                        else
//                        {
//                            _overlayEntity = null;
//                            _transformation = null;
//                        }

//                    }
//                    else
//                    {
//                        _overlayEntity = null;
//                        _transformation = null;
//                    }
//                }
//                else
//                {
//                    _overlayEntity = null;
//                    _transformation = null;
//                }
//            }
//            else if (_drawingCenterMarksForAllItems)
//            {
//                int viewIndex = _drawings.GetEntityUnderMouseCursor(_mouseLocation);

//                if (viewIndex != -1 && _drawings.Entities[viewIndex] is VectorView)
//                {
//                    _currentView = (View)_drawings.Entities[viewIndex];
//                }
//                else
//                    _currentView = null;


//            }

//            if (Dimensioning())
//            {

//                // 繪製視口表面
//                paintBackBuffer();

//                // 合併圖紙
//                swapBuffers();

//            }

//        }

//        private Point3D FindSnappedPoint(BlockReference blockReference, System.Drawing.Point mouseLocation, double gap, bool computeVertices)
//        {
//            Point3D mouse2D;
//            _drawings.ScreenToPlane(mouseLocation, Plane.XY, out mouse2D);

//            if (mouse2D == null)
//                return null;

//            if (computeVertices)
//            {
//                _viewVertices = new List<Point3D>();
//                IList<Entity> ents = blockReference.GetEntities(_drawings.Blocks);
//                foreach (Entity ent in ents)
//                {
//                    if (_drawings.Layers[ent.LayerName].Visible == false)
//                        continue;
//                    if (ent is LinearPath)
//                    {
//                        LinearPath line = ent as LinearPath;
//                        _viewVertices.AddRange(line.Vertices);

//                    }
//                }

//                Transformation transf = blockReference.GetFullTransformation(_drawings.Blocks);
//                for (int i = 0; i < _viewVertices.Count; i++)
//                {
//                    _viewVertices[i] = transf * _viewVertices[i];
//                }

//            }

//            Point3D closest = null;

//            foreach (Point3D point3D in _viewVertices)
//            {
//                double dist = Point2D.DistanceSquared(point3D, mouse2D);
//                if (dist < gap)
//                {
//                    gap = dist;
//                    closest = point3D;
//                }
//            }

//            return closest;
//        }

//        private bool Dimensioning()
//        {
//            return _drawingAngularDim || _drawingLinearDim || _drawingCenterLines || _drawingAngularDimFromLines ||
//                     _drawingRadialDim || _drawingVerticalOrdinateDim || _drawingHorizontalOrdinateDim ||
//                   _drawingVerticalOrdinateDim || _drawingCenterMarks;
//        }


//#if WPF
//        /// <summary>
//        /// 尺寸標註的輔助方法。
//        /// </summary>
//        /// <returns>將尺寸添加到場景時為true，否則為false。</returns>
//        public bool OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
//        {
//            return OnMouseDown(WpfArgsToWinForms(e));
//        }

//        #region WPF to WinForms arguments conversions

//        private System.Windows.Forms.MouseEventArgs WpfArgsToWinForms(System.Windows.Input.MouseButtonEventArgs e)
//        {
//            System.Windows.Point pos = _drawings.GetMousePosition(e);
//            return new System.Windows.Forms.MouseEventArgs(GetMouseButtons(e.ChangedButton), _drawings.GetMouseClicks(e), (int)pos.X, (int)pos.Y, 0);
//        }

//        private System.Windows.Forms.MouseEventArgs WpfArgsToWinForms(System.Windows.Input.MouseEventArgs e)
//        {
//            System.Windows.Point pos = _drawings.GetMousePosition(e);
//            return new System.Windows.Forms.MouseEventArgs(MouseButtons.None, 0, (int)pos.X, (int)pos.Y, 0);
//        }

//        private static MouseButtons GetMouseButtons(System.Windows.Input.MouseEventArgs e)
//        {
//            if (e.LeftButton == MouseButtonState.Pressed)
//                return MouseButtons.Left;

//            if (e.RightButton == MouseButtonState.Pressed)
//                return MouseButtons.Right;

//            if (e.MiddleButton == MouseButtonState.Pressed)
//                return MouseButtons.Middle;

//            if (e.XButton1 == MouseButtonState.Pressed)
//                return MouseButtons.XButton1;

//            if (e.XButton2 == MouseButtonState.Pressed)
//                return MouseButtons.XButton2;

//            return MouseButtons.None;
//        }

//        private static MouseButtons GetMouseButtons(System.Windows.Input.MouseButton mouseButton)
//        {
//            switch (mouseButton)
//            {
//                case System.Windows.Input.MouseButton.Left:
//                    return MouseButtons.Left;

//                case System.Windows.Input.MouseButton.Middle:
//                    return MouseButtons.Middle;

//                case System.Windows.Input.MouseButton.Right:
//                    return MouseButtons.Right;

//                case System.Windows.Input.MouseButton.XButton1:
//                    return MouseButtons.XButton1;

//                case System.Windows.Input.MouseButton.XButton2:
//                    return MouseButtons.XButton2;

//                default:
//                    return MouseButtons.None;
//            }
//        }

//        #endregion
//#endif
//        private Entity GetEntity(System.Drawing.Point mousePos, IList<Entity> entList,
//            Transformation accumulatedParentTransform)
//        {
//            double x1, x2, y1, y2;
//            if (GetRectangle(mousePos, accumulatedParentTransform, out x1, out x2, out y1, out y2))
//            {
//                for (var i = 0; i < entList.Count; i++)
//                {
//                    Entity entity = entList[i];
//                    if (_drawings.Layers[entity.LayerName].Visible == false)
//                        continue;
//                    if (Crossing(entity, x1, x2, y1, y2))
//                        return entity;
//                }
//            }
//            return null;
//        }

//        private bool GetRectangle(System.Drawing.Point mousePos, Transformation acc, out double x1, out double x2, out double y1, out double y2)
//        {
//            x1 = 0;
//            x2 = 0;
//            y1 = 0;
//            y2 = 0;
//            int boxSize = _drawings.PickBoxSize;
//            System.Drawing.Point P1 = new System.Drawing.Point(mousePos.X - boxSize / 2, mousePos.Y - boxSize / 2);
//            System.Drawing.Point P3 = new System.Drawing.Point(mousePos.X + boxSize / 2, mousePos.Y + boxSize / 2);
//            Point3D p1World = _drawings.ScreenToWorld(P1);
//            Point3D p3World = _drawings.ScreenToWorld(P3);
//            if (p1World == null || p3World == null)
//                return false;
//            acc.Invert();
//            p1World.TransformBy(acc);
//            p3World.TransformBy(acc);
//            x1 = p1World.X;
//            x2 = p3World.X;
//            y1 = p3World.Y;
//            y2 = p1World.Y;
//            return true;

//        }

//        private bool Crossing(Entity ent, double x1, double x2, double y1, double y2)
//        {

//            Point2D[] vertices = ent.Vertices;

//            for (var index = 0; index < vertices.Length - 1; index++)
//            {
//                Point2D p1 = vertices[index];
//                Point2D p2 = vertices[index + 1];
//                if (Intersects(p1.X, p1.Y, p2.X, p2.Y, x1, y1, x2, y2) || Intersects(p1.X, p1.Y, p2.X, p2.Y, x2, y1, x1, y2))
//                    return true;
//            }

//            return false;
//        }

//        private bool Intersects(double a1X, double a1Y, double a2X, double a2Y, double x1, double y1, double x2, double y2)
//        {

//            double bX = a2X - a1X;
//            double bY = a2Y - a1Y;
//            double dX = x2 - x1;
//            double dY = y2 - y1;
//            double bDotDPerp = bX * dY - bY * dX;

//            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
//            if (bDotDPerp == 0)
//                return false;

//            double cX = x1 - a1X;
//            double cY = y1 - a1Y;
//            double t = (cX * dY - cY * dX) / bDotDPerp;
//            if (t < 0 || t > 1)
//                return false;

//            double u = (cX * bY - cY * bX) / bDotDPerp;
//            if (u < 0 || u > 1)
//                return false;

//            return true;
//        }

//        private LinearPath[] CreateCenterMark(Circle arc, VectorView currentView)
//        {
//            double radiusEx = arc.Radius + currentView.CenterlinesExtensionAmount / currentView.Scale;
//            Point3D[] horVertices = new Point3D[2]
//            {
//                new Point3D(arc.Center.X - radiusEx, arc.Center.Y), new Point3D(arc.Center.X + radiusEx, arc.Center.Y)
//            };
//            LinearPath hor = new LinearPath(horVertices);
//            Point3D[] verticalVertices = new Point3D[2]
//            {
//                new Point3D(arc.Center.X, arc.Center.Y - radiusEx), new Point3D(arc.Center.X, arc.Center.Y + radiusEx)
//            };

//            LinearPath ver = new LinearPath(verticalVertices);

//            hor.LayerName = _drawings.CenterlinesLayerName;
//            hor.LineTypeScale = (float)GetUnitsConversionFactor();
//            ver.LayerName = _drawings.CenterlinesLayerName;
//            ver.LineTypeScale = (float)GetUnitsConversionFactor();

//            return new LinearPath[2] { hor, ver };
//        }

//        /// <summary>
//        /// 添加圓弧的中心標記
//        /// </summary>
//        /// <param name="arc"></param>
//        /// <param name="currentView"></param>
//        private void AddCenterMark(Circle arc, VectorView currentView)
//        {

//            LinearPath[] centerMark = CreateCenterMark(arc, currentView);

//            Block block = _drawings.Blocks[currentView.BlockName];

//            block.Entities.AddRange(centerMark);


//            block.Entities.Regen();
//        }
//        /// <summary>
//        /// 在兩個線段之間添加中心線
//        /// </summary>
//        /// <param name="line1"></param>
//        /// <param name="line2"></param>
//        /// <param name="currentView"></param>
//        private void AddCenterLines(LinearPath line1, LinearPath line2, VectorView currentView)
//        {
//            Point3D start1 = line1.StartPoint.Clone() as Point3D;

//            Point3D end1 = line1.EndPoint.Clone() as Point3D;

//            Point3D start2 = line2.StartPoint.Clone() as Point3D;

//            Point3D end2 = line2.EndPoint.Clone() as Point3D;

//            Point3D start, end;

//            Segment3D seg1 = new Segment3D(start1, start2);

//            Segment3D seg2 = new Segment3D(end1, end2);

//            Point3D pointOnA, PointOnB;

//            if (Segment3D.Intersection(seg1, seg2, false, out pointOnA, out PointOnB))
//            {
//                start = Point3D.MidPoint(start1, end2);

//                end = Point3D.MidPoint(end1, start2);
//            }
//            else
//            {
//                start = Point3D.MidPoint(start1, start2);

//                end = Point3D.MidPoint(end1, end2);
//            }

//            Vector3D dir = new Vector3D(start, end);

//            dir.Normalize();

//            double len = currentView.CenterlinesExtensionAmount / currentView.Scale;
//            Point3D[] vertices = new Point3D[2] { start - len * dir, end + len * dir };
//            LinearPath centerLine = new LinearPath(vertices);
//            centerLine.LayerName = _drawings.CenterlinesLayerName;
//            centerLine.LineTypeScale = (float)GetUnitsConversionFactor();

//            Block block = _drawings.Blocks[currentView.BlockName];

//            block.Entities.Add(centerLine);

//            _currentView.UpdateBoundingBox(new TraversalParams(_drawings.Blocks));
//        }


//        private void AddCenterMarksForAllItems(VectorView currentView)
//        {
//            Block block = _drawings.Blocks[currentView.BlockName];
//            List<Entity> centerMarks = new List<Entity>();


//            foreach (Entity ent in block.Entities)
//            {

//                if (ent is Arc && !IsHidden(ent))
//                {
//                    Arc arc = (Arc)ent;

//                    LinearPath[] centerMark = CreateCenterMark(arc, currentView);

//                    centerMarks.AddRange(centerMark);

//                }
//            }
//            block.Entities.AddRange(centerMarks);
//            _currentView.UpdateBoundingBox(new TraversalParams(_drawings.Blocks));
//        }
//        /// <summary>
//        /// 如果實體是隱藏線段或隱藏弧線，則返回true
//        /// </summary>
//        /// <param name="ent"></param>
//        /// <returns></returns>
//        private bool IsHidden(Entity ent)
//        {
//            string layerName = ent.LayerName;
//            return !_drawings.Layers[layerName].Visible;
//        }

//    }
//}
