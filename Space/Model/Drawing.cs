using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Space
{
    /// <summary>
    /// 包含交互式繪製不同實體所需的所有方法
    /// </summary>
    partial class ModelExt
    {
        /// <summary>
        /// 在當前鼠標位置繪製選擇框
        /// </summary>
        /// <param name="current"></param>
        public void DrawSelectionMark(System.Drawing.Point current)
        {
            double size = PickBoxSize;//獲取或設置選取框的大小（以像素為單位）。
            double dim1 = current.X + (size / 2);
            double dim2 = Size.Height - current.Y + (size / 2);
            double dim3 = current.X - (size / 2);
            double dim4 = Size.Height - current.Y - (size / 2);

            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);
            //繪製一組線
            renderContext.DrawLines(
            new Point3D[]
            {
                    bottomLeftVertex,
                    bottomRightVertex,
                    bottomRightVertex,
                    topRightVertex,
                    topRightVertex,
                    topLeftVertex,
                    topLeftVertex,
                    bottomLeftVertex
            });
            renderContext.SetLineSize(1);//設置線條大小並啟用（或禁用）粗線渲染。
        }
        /// <summary>
        /// 獲取屏幕頂點
        /// </summary>
        /// <param name="vertices">頂點列表</param>
        /// <returns></returns>
        private Point2D[] GetScreenVertices(IList<Point3D> vertices)
        {
            Point2D[] screenPts = new Point2D[vertices.Count]; //屏幕點的數量
            for (int i = 0; i < vertices.Count; i++)
            {
                screenPts[i] = WorldToScreen(vertices[i]);//將世界坐標映射到屏幕坐標。
            }
            return screenPts;
        }
        /// <summary>
        /// 根據用戶單擊鼠標移動繪製交互式/線性
        /// </summary>
        private void DrawInteractiveLines()
        {
            if (points.Count == 0)
                return;

            Point2D[] screenPts = GetScreenVertices(points);

            renderContext.DrawLineStrip(screenPts); //繪製線條。

            if (ActionMode == devDept.Eyeshot.actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation) && points.Count > 0)
            {
                //繪製線性
                renderContext.DrawLine(screenPts[screenPts.Length - 1], WorldToScreen(current));
            }
        }

        /// <summary>
        ///  繪製網格鎖點，在當前鼠標位置繪製一個加號（+）
        /// </summary>
        /// <param name="current">目前位置</param>
        /// <param name="crossSide">十字大小</param>
        /// <remarks>繪製網格鎖點</remarks>
        private void DrawPositionMark(Point3D current, double crossSide = 20.0)
        {
            if (IsPolygonClosed())
                current = points[0];

            //啟用網格鎖點
            if (gridSnapEnabled)
                if (SnapToGrid(ref current))
                    renderContext.SetLineSize(4);

            Point3D currentScreen = WorldToScreen(current);

            //計算水平線在屏幕上的方向
            Point2D left = WorldToScreen(current.X - 1, current.Y, 0);
            Vector2D dirHorizontal = Vector2D.Subtract(left, currentScreen);
            dirHorizontal.Normalize();

            // 計算線端點在屏幕上的位置
            left = currentScreen + dirHorizontal * crossSide;
            Point2D right = currentScreen - dirHorizontal * crossSide;

            renderContext.DrawLine(left, right);

            //計算垂直線在屏幕上的方向
            Point2D bottom = WorldToScreen(current.X, current.Y - 1, 0);
            Vector2D dirVertical = Vector2D.Subtract(bottom, currentScreen);
            dirVertical.Normalize();

            //計算線端點在屏幕上的位置
            bottom = currentScreen + dirVertical * crossSide;
            Point2D top = currentScreen - dirVertical * crossSide;

            renderContext.DrawLine(bottom, top);

            renderContext.SetLineSize(1);
        }
        /// <summary>
        /// 檢查折線或曲線是否可以閉合多邊形
        /// </summary>
        /// <returns></returns>
        /// <remarks>多邊形是否已關閉</remarks>
        public bool IsPolygonClosed()
        {
            if (points.Count > 0 && (drawingCurve || drawingPolyLine) && (points[0].DistanceTo(current) < magnetRange))
                return true;
            else
                return false;
        }

        #region 指示當前繪圖模式的標誌

        /// <summary>
        /// 畫點
        /// </summary>
        public bool drawingPoints;
        /// <summary>
        /// 繪圖文字
        /// </summary>
        public bool drawingText;
        /// <summary>
        /// 標註 註解
        /// </summary>
        public bool drawingLeader;
        /// <summary>
        /// 繪製橢圓形
        /// </summary>
        public bool drawingEllipse;
        /// <summary>
        /// 繪製橢圓弧
        /// </summary>
        public bool drawingEllipticalArc;
        /// <summary>
        /// 繪製線段
        /// </summary>
        public bool drawingLine;
        /// <summary>
        /// 繪製曲線
        /// </summary>
        public bool drawingCurve;
        /// <summary>
        /// 繪製圓形功能
        /// </summary>
        public bool drawingCircle;
        /// <summary>
        /// 繪製弧形功能
        /// </summary>
        public bool drawingArc;
        /// <summary>
        /// 繪製多邊形
        /// </summary>
        public bool drawingPolyLine;

        #endregion
    }
}
