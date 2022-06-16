using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    /// <summary>
    ///  交互式編輯不同實體所需的方法，例如偏移，鏡像，延伸，修剪，旋轉等。
    /// </summary>
    partial class ModelExt
    {
        /// <summary>
        ///嘗試將實體擴展到所選邊界實體。 對於短邊界線，它嘗試擴展選擇
        ///實體到延長線。
        /// </summary>
        private void ExtendEntity()
        {
            //如果第一個選擇的實體是 null
            if (firstSelectedEntity == null)
            {
                if (selEntityIndex != -1)
                {
                    firstSelectedEntity = Entities[selEntityIndex];
                    selEntityIndex = -1;
                    return;
                }
            }
            else if (secondSelectedEntity == null)
            {
                DrawSelectionMark(mouseLocation);
                renderContext.EnableXOR(false);//啟用或禁用XOR（反色）繪圖模式。如果enable為true，則將其設置為白色，並將devDept.Graphics.blendStateType.XOR混合狀態，否則將設置devDept.Graphics.blendStateType.NoBlend混合狀態。
                DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, "選擇要延伸的實體", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);
            }

            if (secondSelectedEntity == null)
            {
                if (selEntityIndex != -1)
                {
                    secondSelectedEntity = Entities[selEntityIndex];//獲取devDept.Eyeshot.Environment.CurrentBlock的EntityList。 這個集合，包含在視口中顯示的實體。
                }
            }

            if (firstSelectedEntity != null && secondSelectedEntity != null)
            {
                //如果是圓弧
                if (firstSelectedEntity is ICurve boundary && secondSelectedEntity is ICurve curve)
                {
                    //檢查曲線的哪一端接近邊界
                    double t1, t2;

                    boundary.ClosestPointTo(curve.StartPoint, out t1);//返回曲線上最接近給定3D點的點的參數。out 曲線參數
                    boundary.ClosestPointTo(curve.EndPoint, out t2);//返回曲線上最接近給定3D點的點的參數。out 曲線參數

                    Point3D projStartPt = boundary.PointAt(t1);//評估曲線上的一個點。傳回: 曲線上的點。
                    Point3D projEndPt = boundary.PointAt(t2);//評估曲線上的一個點。傳回: 曲線上的點。

                    double curveStartDistance = curve.StartPoint.DistanceTo(projStartPt);//計算到3D點b的距離。傳回: 此3D點與b之間的距離。
                    double curveEndDistance = curve.EndPoint.DistanceTo(projEndPt);//計算到3D點b的距離。傳回: 此3D點與b之間的距離。

                    bool success = false;//是否成功

                    if (curveStartDistance < curveEndDistance)
                    {
                        if (curve is Line)
                        {
                            success = ExtendLine(curve, boundary, true);
                        }
                        else if (curve is LinearPath)
                        {
                            success = ExtendPolyLine(curve, boundary, true);
                        }
                        else if (curve is Arc)
                        {
                            success = ExtendCircularArc(curve, boundary, true);
                        }
                        else if (curve is EllipticalArc)
                        {
                            success = ExtendEllipticalArc(curve, boundary, true);
                        }
                        else if (curve is Curve)
                        {
                            success = ExtendSpline(curve, boundary, true);
                        }
                    }
                    else
                    {
                        if (curve is Line)
                        {
                            success = ExtendLine(curve, boundary, false);
                        }
                        else if (curve is LinearPath)
                        {
                            success = ExtendPolyLine(curve, boundary, false);
                        }
                        else if (curve is Arc)
                        {
                            success = ExtendCircularArc(curve, boundary, false);
                        }
                        else if (curve is EllipticalArc)
                        {
                            success = ExtendEllipticalArc(curve, boundary, false);
                        }
                        else if (curve is Curve)
                        {
                            success = ExtendSpline(curve, boundary, false);
                        }
                    }
                    if (success)
                    {
                        Entities.Remove(secondSelectedEntity);//摘要：從此集合中刪除第一次出現的特定實體。參數：entity : 要從此集合中刪除的對象。傳回：如果成功刪除實體，則為true； 否則為假。 此方法也返回，如果在此集合中未找到實體，則返回false。

                        Entities.Regen();////僅修改和編譯需要它的實體。 將每個實體添加到devDept.Eyeshot.Environment.Blocks後，會自動對其進行重新生成和編譯。或devDept.Eyeshot.Block.Entities集合。 您需要調用此函數僅當您更改 / 轉換這些集合中已有的實體時。
                    }
                }
                ClearAllPreviousCommandData();
            }
        }
        /// <summary>
        /// 延伸不規則曲線
        /// </summary>
        /// <param name="curve">曲線</param>
        /// <param name="boundary">邊界</param>
        /// <param name="nearStart">附近開始</param>
        /// <returns></returns>
        public bool ExtendSpline(ICurve curve, ICurve boundary, bool nearStart)
        {
            Curve originalSpline = curve as Curve;

            Line tempLine = null;
            Vector3D direction = null;
            if (nearStart)
            {
                tempLine = new Line(curve.StartPoint, curve.StartPoint);
                direction = curve.StartTangent; //獲取曲線起點處的單位切向量。
                direction.Normalize(); //將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。
                direction.Negate();//反轉3D矢量方向。
                tempLine.EndPoint = tempLine.EndPoint + direction * extensionLength;
            }
            else
            {
                tempLine = new Line(curve.EndPoint, curve.EndPoint);
                direction = curve.EndTangent; //獲取曲線末端的單位切向量。
                direction.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。
                tempLine.EndPoint = tempLine.EndPoint + direction * extensionLength;
            }

            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempLine);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempLine);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。

            if (intersetionPoints.Length > 0)
            {
                List<Point4D> ctrlPoints = originalSpline.ControlPoints.ToList();
                List<Point3D> newCtrlPoints = new List<Point3D>();
                if (nearStart)
                {
                    newCtrlPoints.Add(GetClosestPoint(curve.StartPoint, intersetionPoints));
                    foreach (Point4D ctrlPt in ctrlPoints)
                    {
                        Point3D point = new Point3D(ctrlPt.X, ctrlPt.Y, ctrlPt.Z);
                        if (!point.Equals(originalSpline.StartPoint))
                            newCtrlPoints.Add(point);
                    }
                }
                else
                {
                    foreach (Point4D ctrlPt in ctrlPoints)
                    {
                        Point3D point = new Point3D(ctrlPt.X, ctrlPt.Y, ctrlPt.Z);
                        if (!point.Equals(originalSpline.EndPoint))
                            newCtrlPoints.Add(point);
                    }
                    newCtrlPoints.Add(GetClosestPoint(curve.EndPoint, intersetionPoints));
                }

                Curve newCurve = new Curve(originalSpline.Degree, newCtrlPoints);
                if (newCurve != null)
                {
                    AddAndRefresh(newCurve, ((Entity)curve).LayerName);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 延伸橢圓弧
        /// </summary>
        /// <param name="ellipticalArcCurve">橢圓弧線</param>
        /// <param name="boundary">邊界</param>
        /// <param name="start">附近開始</param>
        /// <returns></returns>
        private bool ExtendEllipticalArc(ICurve ellipticalArcCurve, ICurve boundary, bool start)
        {
            EllipticalArc selEllipseArc = ellipticalArcCurve as EllipticalArc;
            Ellipse tempEllipse = new Ellipse(selEllipseArc.Plane, selEllipseArc.Center, selEllipseArc.RadiusX, selEllipseArc.RadiusY);

            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempEllipse);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。

            if (intersetionPoints.Length == 0)
            {
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempEllipse);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。
            }

            EllipticalArc newArc = null;

            if (intersetionPoints.Length > 0)
            {
                Plane arcPlane = selEllipseArc.Plane;
                if (start)
                {
                    Point3D intPoint = GetClosestPoint(selEllipseArc.StartPoint, intersetionPoints);

                    newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX, selEllipseArc.RadiusY, selEllipseArc.EndPoint, intPoint, false);

                    //如果起點不在新弧線上，則需要翻轉
                    double t;
                    newArc.ClosestPointTo(selEllipseArc.StartPoint, out t);//返回曲線上最接近給定3D點的點的參數。out 曲線參數
                    Point3D proPt = newArc.PointAt(t);//評估曲線上的一個點。
                    //計算到3D點b的距離。> 0.1
                    if (proPt.DistanceTo(selEllipseArc.StartPoint) > 0.1)
                    {
                        newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                                selEllipseArc.RadiusY, selEllipseArc.EndPoint, intPoint, true);
                    }
                    AddAndRefresh(newArc, ((Entity)ellipticalArcCurve).LayerName);
                }
                else
                {
                    Point3D intPoint = GetClosestPoint(selEllipseArc.EndPoint, intersetionPoints);
                    newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                                selEllipseArc.RadiusY, selEllipseArc.StartPoint, intPoint, false);
                    //如果起點不在新弧線上，則需要翻轉
                    double t;
                    newArc.ClosestPointTo(selEllipseArc.EndPoint, out t);//返回曲線上最接近給定3D點的點的參數。out 曲線參數
                    Point3D projPt = newArc.PointAt(t);
                    //計算到3D點b的距離。> 0.1
                    if (projPt.DistanceTo(selEllipseArc.EndPoint) > 0.1)
                    {
                        newArc = new EllipticalArc(arcPlane, selEllipseArc.Center, selEllipseArc.RadiusX,
                                               selEllipseArc.RadiusY, selEllipseArc.StartPoint, intPoint, true);
                    }
                }
                if (newArc != null)
                {
                    AddAndRefresh(newArc, ((Entity)ellipticalArcCurve).LayerName);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 圓弧延伸方法
        /// </summary>
        /// <param name="arcCurve">圓弧曲線</param>
        /// <param name="boundary">邊界</param>
        /// <param name="nearStart">附近開始</param>
        /// <returns></returns>
        private bool ExtendCircularArc(ICurve arcCurve, ICurve boundary, bool nearStart)
        {
            Arc selCircularArc = arcCurve as Arc;//圓弧
            Circle tempCircle = new Circle(selCircularArc.Plane, selCircularArc.Center, selCircularArc.Radius);

            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempCircle);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。

            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempCircle);//查找提供的曲線相交的所有3D點。C1: 第一條曲線   C2: 第二條曲線  傳回：相交點列表。

            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                {
                    Point3D intPoint = GetClosestPoint(selCircularArc.StartPoint, intersetionPoints);
                    Vector3D xAxis = new Vector3D(selCircularArc.Center, selCircularArc.EndPoint);
                    xAxis.Normalize(); //將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);//兩個3D向量之間的叉積。傳回：產生的3D向量，未標準化。
                    yAxis.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    Plane arcPlane = new Plane(selCircularArc.Center, xAxis, yAxis);

                    Vector2D v1 = new Vector2D(selCircularArc.Center, selCircularArc.EndPoint);
                    v1.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    Vector2D v2 = new Vector2D(selCircularArc.Center, intPoint);
                    v2.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    double arcSpan = Vector2D.SignedAngleBetween(v1, v2);//計算兩個2D向量之間的角度。 u：第一個2D向量  v：第二個2D向量傳回：-PI <angle <+ PI範圍內的弧度角。
                    Arc newArc = new Arc(arcPlane, arcPlane.Origin, selCircularArc.Radius, 0, arcSpan);
                    AddAndRefresh(newArc, ((Entity)arcCurve).LayerName);
                }
                else
                {
                    Point3D intPoint = GetClosestPoint(selCircularArc.EndPoint, intersetionPoints);

                    //plane
                    Vector3D xAxis = new Vector3D(selCircularArc.Center, selCircularArc.StartPoint);
                    xAxis.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。
                    Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);//兩個3D向量之間的叉積。傳回：產生的3D向量，未標準化。
                    yAxis.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    Plane arcPlane = new Plane(selCircularArc.Center, xAxis, yAxis);

                    Vector2D v1 = new Vector2D(selCircularArc.Center, selCircularArc.StartPoint);
                    v1.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。
                    Vector2D v2 = new Vector2D(selCircularArc.Center, intPoint);
                    v2.Normalize();//將3D向量的大小調整為單位長度。 傳回：如果操作成功，則為true，否則為false。

                    double arcSpan = Vector2D.SignedAngleBetween(v1, v2);//計算兩個2D向量之間的角度。 u：第一個2D向量  v：第二個2D向量傳回：-PI <angle <+ PI範圍內的弧度角。
                    Arc newArc = new Arc(arcPlane, arcPlane.Origin, selCircularArc.Radius, 0, arcSpan);
                    AddAndRefresh(newArc, ((Entity)arcCurve).LayerName);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 多邊形線延伸方法
        /// </summary>
        /// <param name="lineCurve">線</param>
        /// <param name="boundary">邊界</param>
        /// <param name="nearStart">附近開始</param>
        /// <returns></returns>
        private bool ExtendPolyLine(ICurve lineCurve, ICurve boundary, bool nearStart)
        {
            LinearPath line = secondSelectedEntity as LinearPath;
            Point3D[] tempVertices = line.Vertices;

            //用正確的方向創建臨時線
            Line tempLine = new Line(line.StartPoint, line.StartPoint);
            Vector3D direction = new Vector3D(line.Vertices[1], line.StartPoint);

            if (!nearStart)
            {
                tempLine = new Line(line.EndPoint, line.EndPoint);
                direction = new Vector3D(line.Vertices[line.Vertices.Length - 2], line.EndPoint);
            }

            direction.Normalize();//將3D向量的大小調整為單位長度。如果操作成功，則為true，否則為false。
            tempLine.EndPoint = tempLine.EndPoint + direction * extensionLength;
            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempLine);//查找提供的曲線相交的所有3D點。參數：C1：第一條曲線 C2：第二條曲線。  傳回：相交點列表。
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempLine);//查找提供的曲線相交的所有3D點。參數：C1：第一條曲線 C2：第二條曲線。  傳回：相交點列表。

            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                    tempVertices[0] = GetClosestPoint(line.StartPoint, intersetionPoints);
                else
                    tempVertices[tempVertices.Length - 1] = GetClosestPoint(line.EndPoint, intersetionPoints);

                line.Vertices = tempVertices;
                AddAndRefresh((Entity)line.Clone(), ((Entity)lineCurve).LayerName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 將輸入線延伸到提供的邊界。
        /// </summary>
        /// <param name="lineCurve">線</param>
        /// <param name="boundary">邊界</param>
        /// <param name="nearStart">附近開始</param>
        /// <returns></returns>
        private bool ExtendLine(ICurve lineCurve, ICurve boundary, bool nearStart)
        {
            Line line = lineCurve as Line;

            //創建將與邊界曲線相交的臨時線，具體取決於要延伸的一端
            Line tempLine = null;//臨時存取
            Vector3D direction = null; //方向

            if (nearStart)
            {
                tempLine = new Line(line.StartPoint, line.StartPoint);
                direction = new Vector3D(line.EndPoint, line.StartPoint);
            }
            else
            {
                tempLine = new Line(line.EndPoint, line.EndPoint);
                direction = new Vector3D(line.StartPoint, line.EndPoint);
            }

            direction.Normalize();//將3D向量的大小調整為單位長度。如果操作成功，則為true，否則為false。
            tempLine.EndPoint = tempLine.EndPoint + direction * extensionLength;

            //獲取輸入線和邊界的交點
            //如果不相交且邊界是線，則可以嘗試使用擴展邊界
            Point3D[] intersetionPoints = Curve.Intersection(boundary, tempLine);//查找提供的曲線相交的所有3D點。參數：C1：第一條曲線 C2：第二條曲線。  傳回：相交點列表。
            if (intersetionPoints.Length == 0)
                intersetionPoints = Curve.Intersection(GetExtendedBoundary(boundary), tempLine); //查找提供的曲線相交的所有3D點。參數：C1：第一條曲線 C2：第二條曲線。  傳回：相交點列表。

            if (intersetionPoints.Length > 0)
            {
                if (nearStart)
                    line.StartPoint = GetClosestPoint(line.StartPoint, intersetionPoints);
                else
                    line.EndPoint = GetClosestPoint(line.EndPoint, intersetionPoints);

                AddAndRefresh((Entity)line.Clone(), ((Entity)lineCurve).LayerName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回給定輸入點與給定輸入點列表的最接近點。
        /// </summary>
        /// <param name="point3D"></param>
        /// <param name="intersetionPoints">插入點</param>
        /// <returns></returns>
        private Point3D GetClosestPoint(Point3D point3D, Point3D[] intersetionPoints)
        {
            double minsquaredDist = Double.MaxValue;
            Point3D result = null;

            foreach (Point3D pt in intersetionPoints)
            {
                double distSquared = Point3D.DistanceSquared(point3D, pt);
                if (distSquared < minsquaredDist && !point3D.Equals(pt))
                {
                    minsquaredDist = distSquared;
                    result = pt;
                }
            }
            return result;
        }
        /// <summary>
        /// 在為直線時創建細長的邊界。
        /// </summary>
        /// <param name="boundary">邊界</param>
        /// <returns></returns>
        private ICurve GetExtendedBoundary(ICurve boundary)
        {
            if (boundary is Line)
            {
                Line tempLine = new Line(boundary.StartPoint, boundary.EndPoint);
                Vector3D dir1 = new Vector3D(tempLine.StartPoint, tempLine.EndPoint);
                dir1.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + dir1 * extensionLength;

                Vector3D dir2 = new Vector3D(tempLine.EndPoint, tempLine.StartPoint);
                dir2.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + dir2 * extensionLength;

                boundary = tempLine;
            }
            return boundary;
        }
        /// <summary>
        /// 嘗試在所選位置（偏移距離）和側面為所選實體創建偏移實體。
        /// </summary>
        public void CreateOffsetEntity()
        {
            if (selEntity != null && selEntity is ICurve selCurve)
            {
                if (points.Count == 0)
                {
                    renderContext.EnableXOR(false);

                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, "點擊偏移位置", new Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);

                    return;
                }
                //曲線參數
                double t;

                bool success = selCurve.Project(points[0], out t);//返回給定3D的最接近垂直投影的參數。指向曲線。 如果曲線內沒有投影，我們尋找在曲線的延長線上的投影。 如果沒有找到投影，我們返回StartPoint的參數和false。
                Point3D projectedPt = selCurve.PointAt(t);//評估曲線上的一個點。
                double offsetDist = projectedPt.DistanceTo(points[0]);//計算到3D點b的距離。


                ICurve offsetCurve = selCurve.Offset(offsetDist, Vector3D.AxisZ, 0.001, true);//偏移指定量的曲線。
                success = offsetCurve.Project(points[0], out t);//返回給定3D的最接近垂直投影的參數。指向曲線。 如果曲線內沒有投影，我們尋找在曲線的延長線上的投影。 如果沒有找到投影，我們返回StartPoint的參數和false。
                projectedPt = offsetCurve.PointAt(t);//評估曲線上的一個點。

                //計算到3D點b的距離。> 1 * (10^-3) =0.0001
                if (projectedPt.DistanceTo(points[0]) > 1e-3)
                    offsetCurve = selCurve.Offset(-offsetDist, Vector3D.AxisZ, 0.001, true);

                AddAndRefresh((Entity)offsetCurve, ActiveLayerName);
            }
        }
        /// <summary>
        /// 為給定的鏡像軸創建所選實體的鏡像。 通過選擇兩個點形成鏡像軸。
        /// </summary>
        private void CreateMirrorEntity()
        {
            //我們需要選擇兩個參考點，可能是捕捉的頂點
            if (points.Count < 2)
            {
                //如果選擇了實體，請用戶選擇鏡像線
                renderContext.EnableXOR(false);//啟用或禁用XOR（反色）繪圖模式。
                if (points.Count == 0 && !waitingForSelection)
                {
                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, "請點選鏡射第一點", new System.Drawing.Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);//顯示動作提示
                }
                else if (points.Count == 1)
                {
                    DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10, "請點選鏡射結束點", new System.Drawing.Font("Tahoma", 8.25f), DrawingColor, ContentAlignment.BottomLeft);//顯示動作提示
                }
                DrawInteractiveLines();
            }
        }
        /// <summary>
        /// 第二個選定實體
        /// </summary>
        Entity secondSelectedEntity = null;
        /// <summary>
        /// 第一個選定實體
        /// </summary>
        Entity firstSelectedEntity = null;

        public bool lineTangents;
        public bool circleTangents;

        public double tangentsRadius = 10.0;
        public double filletRadius = 10.0;
        public double rotationAngle = 45.0;
        public double scaleFactor = 1.5;
        /// <summary>
        /// 延伸長度
        /// </summary>
        private double extensionLength = 500;


        #region 指示當前編輯模式的標誌
        /// <summary>
        /// 鏡射
        /// </summary>
        public bool doingMirror;
        /// <summary>
        /// 偏移
        /// </summary>
        public bool doingOffset;
        /// <summary>
        /// 倒圓角
        /// </summary>
        public bool doingFillet;
        /// <summary>
        /// 倒直角
        /// </summary>
        public bool doingChamfer;

        public bool doingTangents;
        /// <summary>
        /// 移動
        /// </summary>
        public bool doingMove;
        /// <summary>
        /// 旋轉
        /// </summary>
        public bool doingRotate;
        /// <summary>
        /// 縮放比例
        /// </summary>
        public bool doingScale;
        /// <summary>
        /// 修剪
        /// </summary>
        public bool doingTrim;
        /// <summary>
        /// 延伸
        /// </summary>
        public bool doingExtend;
        /// <summary>
        /// 編輯
        /// </summary>
        public bool editingModel;
        #endregion


        public bool flipTangent;
        public bool trimTangent;
    }
}
