using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace WPFSTD105
{

    /// <summary>
    /// 包含確定不同實體尺寸所需的方法。
    /// </summary>
    partial class ModelExt
    {
        private void DrawInteractiveLinearDim()
        {
            //需要選擇兩個參考點，可能是捕捉的頂點
            if (points.Count < 2)
                return;
            if (current == null)
            {
                return;
            }
            List<Point3D> pList = new List<Point3D>();
            Point3D pCurrent = (Point3D)current.Clone();
            for (int i = 0; i < points.Count; i++)
            {
                pList.Add((Point3D)points[i].Clone());
                pList[i].TransformBy(CurrentTransformation);
            }
            //判斷是不是水平向標註
            bool verticalDim = (pCurrent.X > pList[0].X && pCurrent.X > pList[1].X) || (pCurrent.X < pList[0].X && pCurrent.X < pList[1].X);
            Vector3D axisX, axisY;

            if (verticalDim)
            {
                axisX = plane.AxisY;
                
                extPt1 = new Point3D(current.X, points[0].Y, points[0].Z);
                extPt2 = new Point3D(current.X, points[1].Y, points[1].Z);

                if (current.X > points[0].X && current.X > points[1].X)
                {
                    extPt1.X += dimTextHeight / 2;
                    extPt2.X += dimTextHeight / 2;
                }
                else
                {
                    extPt1.X -= dimTextHeight / 2;
                    extPt2.X -= dimTextHeight / 2;
                }

            }
            else//水平標註
            {

                axisX = plane.AxisX;

                extPt1 = new Point3D(points[0].X, current.Y, points[0].Z);
                extPt2 = new Point3D(points[1].X, current.Y, points[1].Z);

                if (current.Y > points[0].Y && current.Y > points[1].Y)
                {
                    extPt1.Y += dimTextHeight / 2;
                    extPt2.Y += dimTextHeight / 2;
                }
                else
                {
                    extPt1.Y -= dimTextHeight / 2;
                    extPt2.Y -= dimTextHeight / 2;
                }

            }

            axisY = Vector3D.Cross(plane.AxisZ, axisX);


            List<Point3D> pts = new List<Point3D>();

            // 畫延長線1
            pts.Add(WorldToScreen(points[0]));
            pts.Add(WorldToScreen(extPt1));

            // 畫延長線2
            pts.Add(WorldToScreen(points[1]));
            pts.Add(WorldToScreen(extPt2));

            //繪製尺寸線
            Segment3D extLine1 = new Segment3D(points[0], extPt1);
            Segment3D extLine2 = new Segment3D(points[1], extPt2);
            Point3D pt1 = current.ProjectTo(extLine1);
            Point3D pt2 = current.ProjectTo(extLine2);

            pts.Add(WorldToScreen(pt1));
            pts.Add(WorldToScreen(pt2));

            renderContext.DrawLines(pts.ToArray());

            //尺寸標註平面
            drawingPlane = new Plane(points[0], axisX, axisY);


            //繪製尺寸文字
            renderContext.EnableXOR(false);

            string dimText = "長度 : " + extPt1.DistanceTo(new Point3D(extPt2.X, extPt2.Y, extPt1.Z)).ToString("f3");
            DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText,
                new Font("Tahoma", 8.25f), DrawingColor_Red, ContentAlignment.BottomLeft);
        }
        /// <summary>
        /// 繪製線條的水平/垂直尺寸預覽
        /// </summary>
        private void _DrawInteractiveLinearDim()
        {
            //需要選擇兩個參考點，可能是捕捉的頂點
            if (points.Count < 2)
                return;
            if (current == null)
            {
                return;
            }

            //判斷是不是水平向標註
            bool verticalDim = (current.X > points[0].X && current.X > points[1].X) || (current.X < points[0].X && current.X < points[1].X);

            Vector3D axisX, axisY;

            if (verticalDim)
            {
                axisX = plane.AxisY;

                extPt1 = new Point3D(current.X, points[0].Y, points[0].Z);
                extPt2 = new Point3D(current.X, points[1].Y, points[1].Z);

                if (current.X > points[0].X && current.X > points[1].X)
                {
                    extPt1.X += dimTextHeight / 2;
                    extPt2.X += dimTextHeight / 2;
                }
                else
                {
                    extPt1.X -= dimTextHeight / 2;
                    extPt2.X -= dimTextHeight / 2;
                }

            }
            else//水平標註
            {

                axisX = plane.AxisX;

                extPt1 = new Point3D(points[0].X, current.Y, points[0].Z);
                extPt2 = new Point3D(points[1].X, current.Y, points[1].Z);

                if (current.Y > points[0].Y && current.Y > points[1].Y)
                {
                    extPt1.Y += dimTextHeight / 2;
                    extPt2.Y += dimTextHeight / 2;
                }
                else
                {
                    extPt1.Y -= dimTextHeight / 2;
                    extPt2.Y -= dimTextHeight / 2;
                }

            }

            axisY = Vector3D.Cross(plane.AxisZ, axisX);


            List<Point3D> pts = new List<Point3D>();

            // 畫延長線1
            pts.Add(WorldToScreen(points[0]));
            pts.Add(WorldToScreen(extPt1));

            // 畫延長線2
            pts.Add(WorldToScreen(points[1]));
            pts.Add(WorldToScreen(extPt2));

            //繪製尺寸線
            Segment3D extLine1 = new Segment3D(points[0], extPt1);
            Segment3D extLine2 = new Segment3D(points[1], extPt2);
            Point3D pt1 = current.ProjectTo(extLine1);
            Point3D pt2 = current.ProjectTo(extLine2);

            pts.Add(WorldToScreen(pt1));
            pts.Add(WorldToScreen(pt2));

            renderContext.DrawLines(pts.ToArray());

            //尺寸標註平面
            drawingPlane = new Plane(points[0], axisX, axisY);

            //繪製尺寸文字
            renderContext.EnableXOR(false);

            string dimText = "長度 : " + extPt1.DistanceTo(new Point3D(extPt2.X, extPt2.Y, extPt1.Z)).ToString("f3");
            DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText,
                new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);

        }
        /// <summary>
        /// 繪製對齊尺寸的預覽
        /// </summary>
        private void DrawInteractiveAlignedDim()
        {
            //我們需要選擇兩個參考點，可能是捕捉的頂點
            if (points.Count < 2)
                return;

            if (points[1].X < points[0].X || points[1].Y < points[0].Y)
            {
                Point3D p0 = points[0];
                Point3D p1 = points[1];

                Utility.Swap(ref p0, ref p1);

                points[0] = p0;
                points[1] = p1;
            }

            Vector3D axisX = new Vector3D(points[0], points[1]);
            Vector3D axisY = Vector3D.Cross(Vector3D.AxisZ, axisX);

            drawingPlane = new Plane(points[0], axisX, axisY);

            Vector2D v1 = new Vector2D(points[0], points[1]);
            Vector2D v2 = new Vector2D(points[0], current);

            double sign = Math.Sign(Vector2D.SignedAngleBetween(v1, v2));

            //當前 p0-p1 偏移
            Segment2D segment = new Segment2D(points[0], points[1]);
            double offsetDist = current.DistanceTo(segment);
            extPt1 = points[0] + sign * drawingPlane.AxisY * (offsetDist + dimTextHeight / 2);
            extPt2 = points[1] + sign * drawingPlane.AxisY * (offsetDist + dimTextHeight / 2);
            Point3D dimPt1 = points[0] + sign * drawingPlane.AxisY * offsetDist;
            Point3D dimPt2 = points[1] + sign * drawingPlane.AxisY * offsetDist;

            List<Point3D> pts = new List<Point3D>();

            //畫延長線1
            pts.Add(WorldToScreen(points[0]));
            pts.Add(WorldToScreen(extPt1));

            //畫延長線2
            pts.Add(WorldToScreen(points[1]));
            pts.Add(WorldToScreen(extPt2));

            //繪製尺寸線
            pts.Add(WorldToScreen(dimPt1));
            pts.Add(WorldToScreen(dimPt2));
            //畫線段
            renderContext.DrawLines(pts.ToArray());

            //繪製尺寸文字
            renderContext.EnableXOR(false);

            string dimText = "L " + extPt1.DistanceTo(extPt2).ToString("f3");
            DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText,
                new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
        }
        /// <summary>
        /// 繪製縱坐標尺寸的預覽
        /// </summary>
        private void DrawInteractiveOrdinateDim()
        {
            //至少有一點
            if (points.Count < 0)
                return;

            List<Point3D> pts = new List<Point3D>();
            Point3D leaderEndPoint;
            Segment3D[] segments = OrdinateDim.Preview(Plane.XY, points[0], current, drawingOrdinateDimVertical, dimTextHeight / 2, dimTextHeight, 0.625, 3.0, 0.625, out leaderEndPoint);

            foreach (var item in segments)
            {
                pts.Add(WorldToScreen(item.P0));
                pts.Add(WorldToScreen(item.P1));
            }

            //畫線段
            renderContext.DrawLines(pts.ToArray());

            //繪製尺寸文字
            renderContext.EnableXOR(false);

            double distance = drawingOrdinateDimVertical ? Math.Abs(Plane.XY.Origin.X - points[0].X) : Math.Abs(Plane.XY.Origin.Y - points[0].Y);

            string dimText = "D " + distance.ToString("f3");
            DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText,
                new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
        }
        /// <summary>
        /// 使用R5.25，Ø12.62之類的文字繪製半徑/直徑尺寸的預覽 ( 圓弧 ) 
        /// </summary>
        /// <remarks>圓弧</remarks>
        private void DrawInteractiveDiametricDim()
        {
            if (selEntityIndex != -1)
            {
                Entity entity = this.Entities[selEntityIndex];
                if (entity is Circle cicularEntity)//弧是一個圓
                {
                    //繪製中心標記
                    //DrawPositionMark(cicularEntity.Center);

                    renderContext.DrawLine(WorldToScreen(cicularEntity.Center), WorldToScreen(current));

                    //禁用反轉繪製
                    renderContext.EnableXOR(false);

                    string dimText;

                    if (drawingDiametricDim)
                        dimText = "Ø" + cicularEntity.Diameter.ToString("f3");
                    else
                        dimText = "R" + cicularEntity.Radius.ToString("f3");

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText,
                        new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                }
            }
        }
        /// <summary>
        /// 使用R5.25，Ø12.62之類的文字繪製徑向/直徑尺寸的預覽 ( 圓形 )
        /// </summary>
        private void DrawInteractiveAngularDim()
        {
            if (selEntityIndex != -1)
            {
                Entity entity = Entities[selEntityIndex];

                if (entity is Arc && !drawingAngularDimFromLines)
                {
                    Arc selectedArc = entity as Arc;

                    //畫中心標記
                    //DrawPositionMark(selectedArc.Center);

                    //在中心和光標點之間繪製性線
                    renderContext.DrawLine(WorldToScreen(selectedArc.Center), WorldToScreen(current));

                    //禁用反轉繪製
                    renderContext.EnableXOR(false);

                    string dimText = "A " + Utility.RadToDeg(selectedArc.Domain.Length).ToString("f3") + "°";

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, dimText, new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
                }
            }
            else if (drawingAngularDimFromLines && quadrantPoint != null)
            {
                //繪製象限點標記
                //DrawPositionMark(quadrantPoint);

                //在像限點和光標點之間繪製性線
                renderContext.DrawLine(WorldToScreen(quadrantPoint), WorldToScreen(current));

                //禁用反轉繪製
                renderContext.EnableXOR(false);

                DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, "", new Font("Tahoma", 8.25f), DrawingColor_White, ContentAlignment.BottomLeft);
            }
        }

        #region 尺寸標註
        /// <summary>
        /// 線性標註
        /// </summary>
        public bool drawingLinearDim;
        /// <summary>
        /// 對其式標註
        /// </summary>
        public bool drawingAlignedDim;
        /// <summary>
        /// 設置平面
        /// </summary>
        public bool setPlane;
        /// <summary>
        /// 半徑標註
        /// </summary>
        public bool drawingRadialDim;
        /// <summary>
        /// 直徑標註
        /// </summary>
        public bool drawingDiametricDim;
        /// <summary>
        /// 角度標註
        /// </summary>
        public bool drawingAngularDim;
        /// <summary>
        /// 角度標註從線
        /// </summary>
        public bool drawingAngularDimFromLines;

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ModelExt.drawingOrdinateDim' 的 XML 註解
        public bool drawingOrdinateDim;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ModelExt.drawingOrdinateDim' 的 XML 註解
        /// <summary>
        /// 絕對座標是垂直
        /// </summary>
        public bool drawingOrdinateDimVertical;

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ModelExt.drawingQuadrantPoint' 的 XML 註解
        public bool drawingQuadrantPoint;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ModelExt.drawingQuadrantPoint' 的 XML 註解

        /// <summary>
        /// 標註尺寸文字高度
        /// 2022/08/04 呂宗霖 副總指示 35→20
        /// </summary>
        public double dimTextHeight = 20;

        #endregion
    }
}
