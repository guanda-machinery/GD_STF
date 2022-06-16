//using System.Collections.Generic;
//using System.Drawing;
//using devDept.Eyeshot.Entities;
//using devDept.Geometry;
//using devDept.Graphics;

//namespace WPFSTD105
//{
//    public partial class DrawingsHelper
//    {
//        /// <summary>
//        /// 當前捕捉點，它是視圖的頂點之一
//        /// </summary>
//        private Point3D _snappedPoint;

//        private const int SnapQuadSize = 12;

//        /// <summary>
//        /// 繪製定義捕捉點的四邊形
//        /// </summary>
//        private void DrawQuad(System.Drawing.Point onScreen)
//        {
//            double x1 = onScreen.X - (SnapQuadSize / 2);
//            double y1 = onScreen.Y - (SnapQuadSize / 2);
//            double x2 = onScreen.X + (SnapQuadSize / 2);
//            double y2 = onScreen.Y + (SnapQuadSize / 2);

//            Point3D bottomLeftVertex = new Point3D(x1, y1);
//            Point3D bottomRightVertex = new Point3D(x2, y1);
//            Point3D topRightVertex = new Point3D(x2, y2);
//            Point3D topLeftVertex = new Point3D(x1, y2);

//            _drawings.renderContext.DrawLineLoop(new Point3D[]
//            {
//                bottomLeftVertex,
//                bottomRightVertex,
//                topRightVertex,
//                topLeftVertex
//            });
//        }

//        /// <summary>
//        /// 顯示捕捉點
//        /// </summary>
//        private void DisplaySnappedVertex()
//        {
//            _drawings.renderContext.SetLineSize(2);

//            // 藍色
//            _drawings.renderContext.SetColorWireframe(Color.FromArgb(0, 0, 255));
//            _drawings.renderContext.SetState(depthStencilStateType.DepthTestOff);

//            Point2D onScreen = _drawings.WorldToScreen(_snappedPoint);

//            DrawQuad(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
//            _drawings.renderContext.SetLineSize(1);
//        }


//        private void DisplaySnappedCircle()
//        {
//            if (!(_overlayEntity is Circle))
//                return;
//            Circle overlayCircle = (Circle)_overlayEntity;
//            _drawings.renderContext.SetLineSize(4);

//            // 藍色
//            _drawings.renderContext.SetColorWireframe(Color.FromArgb(0, 0, 255));

//            _drawings.renderContext.SetState(depthStencilStateType.DepthTestOff);


//            List<Point3D> onScreenVertices = new List<Point3D>();

//            for (var i = 0; i < overlayCircle.Vertices.Length; i++)
//            {
//                Point3D overlayCircleVertex = overlayCircle.Vertices[i];
//                Point3D vertex = overlayCircleVertex.Clone() as Point3D;

//                if (_transformation != null)
//                    vertex.TransformBy(_transformation);

//                Point3D onScreen = _drawings.WorldToScreen(vertex);

//                onScreenVertices.Add(onScreen);
//                if (i != 0 && i != overlayCircle.Vertices.Length - 1)
//                    onScreenVertices.Add(onScreen);
//            }

//            _drawings.renderContext.DrawLines(onScreenVertices.ToArray());

//            _drawings.renderContext.SetLineSize(1);
//        }

//        private void DisplaySnappedLine()
//        {
//            if (!(_overlayEntity is LinearPath))
//                return;

//            LinearPath overlayLine = (LinearPath)_overlayEntity;

//            _drawings.renderContext.SetLineSize(4);

//            // 藍色
//            _drawings.renderContext.SetColorWireframe(Color.FromArgb(0, 0, 255));

//            _drawings.renderContext.SetState(depthStencilStateType.DepthTestOff);

//            Point3D start = (Point3D)overlayLine.StartPoint.Clone();
//            Point3D end = (Point3D)overlayLine.EndPoint.Clone();
//            if (_transformation != null)
//            {
//                start.TransformBy(_transformation);
//                end.TransformBy(_transformation);
//            }

//            Point3D startOnScreen = _drawings.WorldToScreen(start);
//            Point3D endOnScreen = _drawings.WorldToScreen(end);

//            _drawings.renderContext.DrawLine(startOnScreen, endOnScreen);

//            _drawings.renderContext.SetLineSize(1);
//        }

//    }
//}
