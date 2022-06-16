using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using System;
using System.Collections.Generic;

namespace WPFSTD105
{
    /// <summary>
    /// 包含網格捕捉或模型頂點捕捉所需的實用程序
    /// </summary>
    partial class ModelExt
    {
        /// <summary>
        /// 當前捕捉點，它是模型的頂點之一
        /// </summary>
        private Point3D snapPoint = null;

        /// <summary>
        /// 指示當前捕捉模式的標誌
        /// </summary>
        public bool objectSnapEnabled { get; set; } = false;
        /// <summary>
        /// 啟用網格捕捉
        /// </summary>
        public bool gridSnapEnabled { get; set; } = false;
        /// <summary>
        /// 等待選擇
        /// </summary>
        public bool waitingForSelection { get; set; }
        /// <summary>
        /// 嘗試捕捉當前鼠標點的網格頂點
        /// </summary>
        /// <param name="ptToSnap">捕捉點</param>
        /// <returns></returns>
        private bool SnapToGrid(ref Point3D ptToSnap)
        {
            Point2D gridPoint = new Point2D(Math.Round(ptToSnap.X / 10) * 10, Math.Round(ptToSnap.Y / 10) * 10);

            //判斷目前滑鼠座標是否接近網格的臨界值
            if (Point2D.Distance(gridPoint, ptToSnap) < magnetRange)
            {
                ptToSnap.X = gridPoint.X;
                ptToSnap.Y = gridPoint.Y;

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 將實體添加到活動層上的場景並刷新螢幕
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="layerName"></param>
        private void AddAndRefresh(Entity entity, string layerName)
        {
            // 增加點數
            if (entity is devDept.Eyeshot.Entities.Point)
            {
                entity.LineWeightMethod = colorMethodType.byEntity;
                entity.LineWeight = 3;
            }

            //避免寬度大於1的尺寸
            if (entity is Dimension || entity is Leader)
            {
                entity.LayerName = layerName;
                entity.LineWeightMethod = colorMethodType.byEntity;

                Entities.Add(entity, System.Drawing.Color.White);
            }
            else
            {
                Entities.Add(entity);
            }

            Entities.Regen();
            Invalidate();
        }
        /// <summary>
        /// 取得物件捕捉點
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public SnapPoint[] GetSnapPoints(System.Drawing.Point point)
        {

            //更改PickBox Size以定義顯示捕捉點的範圍
            int oldSize = PickBoxSize;
            PickBoxSize = 10;

            //選擇鼠標光標下的實體
            Transformation accumulatedParentTransform = new Identity();//ID轉換。
            Entity entity = GetNestedEntity(mouseLocation, Entities, ref accumulatedParentTransform);
            PickBoxSize = oldSize;
            List<SnapPoint> result = new List<SnapPoint>();

            if (entity != null)
            {
                //提取使用GetEntityUnderMouseCursor選擇的實體

                //檢查實體類型，然後確定捕捉點
                if (entity is Point pt)
                {
                    result.Add(new SnapPoint(pt.Vertices[0], objectSnapType.Point));
                }
                else if (entity is Line line)
                {
                    result.AddRange(GetSnapLinePoint(line));
                }
                else if (entity is LinearPath linear)
                {
                    foreach (var el in linear.ConvertToLines())//返回線性路徑各行陣列。
                    {
                        List<SnapPoint> snapPoints = GetSnapLinePoint(el);
                        for (int i = 0; i < snapPoints.Count; i++)//查看是否有重複的鎖點位置
                        {
                            if (!result.Contains(snapPoints[i]))
                            {
                                result.Add(snapPoints[i]);
                            }
                        }
                    }
                }
                else if (entity is Circle circle)//圓形
                {
                    result.Add(new SnapPoint(circle.EndPoint, objectSnapType.End));
                    result.Add(new SnapPoint(circle.PointAt(circle.Domain.Mid), objectSnapType.Mid));//中間點
                    result.Add(new SnapPoint(circle.Center, objectSnapType.Center));//圓心點

                    /*四分點*/
                    Point3D quad1 = new Point3D(circle.Center.X, circle.Center.Y + circle.Radius);
                    Point3D quad2 = new Point3D(circle.Center.X + circle.Radius, circle.Center.Y);
                    Point3D quad3 = new Point3D(circle.Center.X, circle.Center.Y - circle.Radius);
                    Point3D quad4 = new Point3D(circle.Center.X - circle.Radius, circle.Center.Y);
                    result.AddRange(new SnapPoint[] { new SnapPoint(quad1, objectSnapType.Quad),
                                                           new SnapPoint(quad2, objectSnapType.Quad),
                                                           new SnapPoint(quad3, objectSnapType.Quad),
                                                           new SnapPoint(quad4, objectSnapType.Quad)});
                }
                else if (entity is BlockReference blockReference)
                {
                    SetCurrent(blockReference);
                    result.AddRange(this.GetSnapPoints(point));
                    SetCurrent(null);
                }
                else if (entity is Mesh mesh) //TODO : 實體鎖點
                {
                    foreach (Solid.Portion solidPortion in mesh.ConvertToSolid().Portions)
                    {
                        foreach (var item in solidPortion.Edges)
                        {
                            //產生線段
                            Line line1 = new Line(solidPortion.Vertices[item.V1], solidPortion.Vertices[item.V2]);
                            result.AddRange(new SnapPoint[] {new SnapPoint(line1.StartPoint, objectSnapType.End),
                                                                                 new SnapPoint(line1.EndPoint, objectSnapType.End),
                                                                                 new SnapPoint(line1.MidPoint, objectSnapType.Mid)});
                        }
                    }
                }
            }
            if (accumulatedParentTransform != new Identity())
            {
                Point3D p_tmp;
                foreach (SnapPoint sp in result)
                {
                    p_tmp = accumulatedParentTransform * sp;
                    sp.X = p_tmp.X;
                    sp.Y = p_tmp.Y;
                    sp.Z = p_tmp.Z;
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// 取得線段捕捉點
        /// </summary>
        /// <returns></returns>
        private List<SnapPoint> GetSnapLinePoint(Line line)
        {
            List<SnapPoint> result = new List<SnapPoint>();
            result.Add(new SnapPoint(line.StartPoint, objectSnapType.End));
            result.Add(new SnapPoint(line.EndPoint, objectSnapType.End));
            result.Add(new SnapPoint(line.MidPoint, objectSnapType.Mid));
            return result;
        }
        //public objectSnapType activeObjectSnap { get; set; }
        /// <summary>
        /// 獲取鼠標指針下方BlockReference中的嵌套實體，併計算其轉換
        /// </summary>
        /// <param name="mousePos">滑鼠位置</param>
        /// <param name="entList">實體列表</param>
        /// <param name="accumulatedParentTransform">堆積的Parent轉換</param>
        /// <returns></returns>
        private Entity GetNestedEntity(System.Drawing.Point mousePos, IList<Entity> entList, ref Transformation accumulatedParentTransform)
        {
            int[] index;
            Entity ent;
            /*返回與指定對象交叉的所有可見和可選實體的列表  選擇框。
             * 參數：
             * selectionBox：
             * 屏幕坐標中的選擇矩形框
             * entList：
             * 實體的自定義列表
             * firstOnly：
             * 為true時，選擇第一個實體後立即返回
             * selectableOnly：
             * 為true時，檢查devDept.Eyeshot.Entities.Entity.Selectable屬性，否則為不。
             * accParentTransform：
             * 累積的父轉換或null / Nothing
             * 傳回：
             * 代表所選實體位置的索引數組。
             * 備註：
             * 考慮selectionBox中的所有實體，甚至包括其他實體所覆蓋的實體。*/
            index = GetCrossingEntities(new System.Drawing.Rectangle(mousePos.X - 5, mousePos.Y - 5, 10, 10), entList, true, true, accumulatedParentTransform);
            //判斷是否有物件
            if (index != null && index.Length > 0)
            {

                if (entList[index[0]] is BlockReference br) /*摘要：BlockReference實體。 備註：請注意，縮放devDept.Eyeshot.Entities.Mesh系列的實體不推薦。 由於應用了縮放，將導致顏色更改 也可以正常顯示三角形。*/
                {
                    accumulatedParentTransform = accumulatedParentTransform * br.GetFullTransformation(Blocks); //摘要：塊收集。 該集合包含塊定義。
                    ent = GetNestedEntity(mousePos, Blocks[br.BlockName].Entities, ref accumulatedParentTransform);
                    return ent;
                }
                else
                {
                    return entList[index[0]];
                }
            }
            return null;
        }
        /// <summary>
        /// 查找最接近的捕捉點。
        /// </summary>
        /// <param name="snapPoints">捕捉點數組</param>
        /// <returns>最近的捕捉點。</returns>
        private SnapPoint FindClosestPoint(SnapPoint[] snapPoints)
        {
            double minDist = double.MaxValue;

            int i = 0;
            int index = 0;

            foreach (var el in snapPoints)
            {
                /*摘要：
                 * 將世界坐標映射到屏幕坐標。
                 * 參數：
                 * Point3D：
                 * 3D點投影到屏幕上
                 * 傳回：
                 * 關聯的投影屏幕點（底部為零）
                 * 備註：
                 * 返回點的z分量位於規範化的設備坐標中
                 * 空格[0,1]。 超出[0,1]範圍的值表示該點位於
                 * 相機的遠近裁剪平面。*/
                Point3D vertexScreen = WorldToScreen(el);
                //Point3D currentScreen;
                Point2D currentScreen = new Point2D(mouseLocation.X, Size.Height - mouseLocation.Y); ;
                /*摘要：
                 * 將屏幕坐標映射到活動視口中的世界坐標。
                 * 參數：
                 * mousePos：
                 * 鼠標光標位置（頂部為零）
                 * plan：
                 * 平面
                 * intPoint：
                 * 相交點。 如果平面垂直於屏幕，則為null / Nothing。
                 * 傳回：
                 * 如果映射成功，則為true，否則為false。*/
                //ScreenToPlane(mouseLocation, plane, out currentScreen);
                double dist = Point3D.Distance(vertexScreen, currentScreen);//計算兩個3D點之間的距離。

                if (dist < minDist)
                {
                    index = i;
                    minDist = dist;
                }
                i++;
            }
            SnapPoint result = (SnapPoint)snapPoints.GetValue(index);
            DisplaySnappedVertex(result);

            return result;
        }
        /// <summary>
        /// 用renderContext繪製三角形以呈現MID捕捉點
        /// </summary>
        void DrawTriangle(System.Drawing.Point onScreen)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 2);
            double dim2 = onScreen.Y + (snapSymbolSize / 2);
            double dim3 = onScreen.X - (snapSymbolSize / 2);
            double dim4 = onScreen.Y - (snapSymbolSize / 2);
            double dim5 = onScreen.X;

            Point3D topCenter = new Point3D(dim5, dim2);

            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLineLoop(new Point3D[]
            {
                bottomLeftVertex,
                bottomRightVertex,
                topCenter
            });
        }
        /// <summary>
        /// 顯示與捕捉的頂點類型關聯的符號
        /// </summary>
        /// <param name="snap"></param>
        private void DisplaySnappedVertex(SnapPoint snap)
        {
            renderContext.SetLineSize(2);

            renderContext.SetColorWireframe(System.Drawing.Color.Peru);
            renderContext.SetState(depthStencilStateType.DepthTestOff);//設置深度模版狀態類型。

            Point2D onScreen = WorldToScreen(snap);

            this.snapPoint = snap;

            switch (snap.Type)
            {
                case objectSnapType.Point:
                    DrawCircle(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    DrawCross(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    break;
                case objectSnapType.Center:
                    DrawCircle(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    break;
                case objectSnapType.End:
                    DrawQuad(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    break;
                case objectSnapType.Mid:
                    DrawTriangle(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    break;
                case objectSnapType.Quad:
                    renderContext.SetLineSize(3.0f);
                    DrawRhombus(new System.Drawing.Point((int)onScreen.X, (int)(onScreen.Y)));
                    break;
            }

            renderContext.SetLineSize(5);
        }
        /// <summary>
        /// 四分點
        /// </summary>
        /// <param name="onScreen"></param>
        void DrawRhombus(System.Drawing.Point onScreen)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 1.5);
            double dim2 = onScreen.Y + (snapSymbolSize / 1.5);
            double dim3 = onScreen.X - (snapSymbolSize / 1.5);
            double dim4 = onScreen.Y - (snapSymbolSize / 1.5);

            Point3D topVertex = new Point3D(onScreen.X, dim2);
            Point3D bottomVertex = new Point3D(onScreen.X, dim4);
            Point3D rightVertex = new Point3D(dim1, onScreen.Y);
            Point3D leftVertex = new Point3D(dim3, onScreen.Y);

            renderContext.DrawLineLoop(new Point3D[]
            {
                bottomVertex,
                rightVertex,
                topVertex,
                leftVertex,
            });
        }
        public int snapSymbolSize { get; set; } = 12;
        /// <summary>
        /// 畫十字。 用圓圈繪製表示單個點
        /// </summary>
        public void DrawCross(System.Drawing.Point onScreen)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 2);
            double dim2 = onScreen.Y + (snapSymbolSize / 2);
            double dim3 = onScreen.X - (snapSymbolSize / 2);
            double dim4 = onScreen.Y - (snapSymbolSize / 2);

            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLines(
                new Point3D[]
                {
                    bottomLeftVertex,
                    topRightVertex,

                    topLeftVertex,
                    bottomRightVertex,

                });
        }
        /// <summary>
        /// 用renderContext繪製四邊形以渲染END捕捉點
        /// </summary>
        public void DrawQuad(System.Drawing.Point onScreen)
        {
            double dim1 = onScreen.X + (snapSymbolSize / 2);
            double dim2 = onScreen.Y + (snapSymbolSize / 2);
            double dim3 = onScreen.X - (snapSymbolSize / 2);
            double dim4 = onScreen.Y - (snapSymbolSize / 2);

            Point3D topLeftVertex = new Point3D(dim3, dim2);
            Point3D topRightVertex = new Point3D(dim1, dim2);
            Point3D bottomRightVertex = new Point3D(dim1, dim4);
            Point3D bottomLeftVertex = new Point3D(dim3, dim4);

            renderContext.DrawLineLoop(new Point3D[]
            {
                bottomLeftVertex,
                bottomRightVertex,
                topRightVertex,
                topLeftVertex
            });
        }
        /// <summary>
        /// 使用renderContext繪製圓以呈現圓中心點捕捉點
        /// </summary>
        public void DrawCircle(System.Drawing.Point onScreen)
        {
            double radius = snapSymbolSize / 2;

            double x2 = 0, y2 = 0;

            List<Point3D> pts = new List<Point3D>();

            for (int angle = 0; angle < 360; angle += 10)
            {
                double rad_angle = Utility.DegToRad(angle);

                x2 = onScreen.X + radius * Math.Cos(rad_angle);
                y2 = onScreen.Y + radius * Math.Sin(rad_angle);

                Point3D circlePoint = new Point3D(x2, y2);
                pts.Add(circlePoint);
            }

            renderContext.DrawLineLoop(pts.ToArray());
        }
        #region 捕捉數據
        /// <summary>
        /// 表示來自模型頂點和關聯捕捉類型的3D點。
        /// </summary>
        public class SnapPoint : devDept.Geometry.Point3D
        {
            public objectSnapType Type;

            public SnapPoint()
                : base()
            {
                Type = objectSnapType.None;
            }

            public SnapPoint(Point3D point3D, objectSnapType objectSnapType) : base(point3D.X, point3D.Y, point3D.Z)
            {
                this.Type = objectSnapType;
            }

            public override string ToString()
            {
                return base.ToString() + " | " + Type;
            }
        }
        #endregion
    }
}
