using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Graphics;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
namespace WPFSTD105.Model
{
    /// <summary>
    /// 代表鋼材 2D 塊繪製函數
    /// </summary>
    public class Steel2DBlock : Block, I2DBlock
    {

        /// <inheritdoc/>
        public double MoveBack { get => SteelAttr.H + SteelAttr.W + 150; }
        /// <summary>
        /// 虛線名稱
        /// </summary>
        public const string LineTypeName = "DottedLine";
        /// <inheritdoc/>
        public double MoveFront { get => -SteelAttr.W - 150; }

        /// <summary>
        /// 鋼構設定檔
        /// </summary>
        public SteelAttr SteelAttr { get => (SteelAttr)Steel.EntityData; }
        /// <summary>
        /// 實體線段
        /// </summary>
        public Mesh Steel { get; private set; }
        /// <summary>
        /// 標準函數
        /// </summary>
        /// <param name="mesh">3d形狀</param>
        /// <param name="name">圖塊名稱</param>
        public Steel2DBlock(Mesh mesh, string name) : base(name)
        {
            ChangeMesh(mesh);
            Triangulation();
            //_Triangulation();
        }
        /// <summary>
        /// 變更3d形狀
        /// </summary>
        /// <param name="mesh"></param>
        public void ChangeMesh(Mesh mesh)
        {

#if DEBUG
            log4net.LogManager.GetLogger("ChangeMesh").Debug("");
#endif
            //檢查自定義屬性檔是不是空值
            log4net.LogManager.GetLogger("檢查").Debug("屬性檔是不是空值");
            if (mesh.EntityData == null)
                throw new Exception("mesh EntityData is null");

            //檢查屬性檔案是否正確
            log4net.LogManager.GetLogger("檢查").Debug("屬性檔案是否正確");
            if (mesh.EntityData.GetType() != typeof(SteelAttr))
                throw new Exception($"mesh EntityData type is not {typeof(SteelAttr)}");
            Steel = mesh;

        }
        #region 公開方法

        #endregion

        #region 私有方法
        /// <summary>
        /// 關聯面角角度
        /// </summary>
        readonly double _angularTol = Utility.DegToRad(80);
        /// <summary>
        /// 按法線分割三角形。將三角形和頂點分成，頂面。
        /// </summary>
        /// <param name="section">要拆解成部分面的圖形</param>
        /// <param name="count">保留的三角數量</param>
        /// <param name="vector3D"></param>
        private void SplitTrianglesByNormal(Mesh section, out int count, Vector3D vector3D)
        {
            count = 0;
            for (int i = 0; i < Steel.Triangles.Length; i++)
            {
                IndexTriangle indexTriangle = Steel.Triangles[i];//使用頂點索引定義三角形。
                Triangle triangle = new Triangle(Steel.Vertices[indexTriangle.V1], Steel.Vertices[indexTriangle.V2], Steel.Vertices[indexTriangle.V3]);//三角形
                triangle.Regen(0.1);//計算曲線或曲面細分。
                double angle = Vector3D.AngleBetween(vector3D, triangle.Normal);//計算兩個向量之間的角度。
                if (Math.PI / 2 - Math.Abs(angle) > _angularTol)
                {
                    count++;
                }
                else
                {
                    section.Triangles[i] = null;
                }

            }
        }
        /// <summary>
        /// 按法線分割三角形。將三角形和頂點分成，頂面 / 側面 / 底面
        /// </summary>
        /// <param name="originalEntity">原始實體</param>
        /// <param name="direction">方向</param>
        /// <param name="originalT">原始頂點索引定義三角形。</param>
        /// <param name="top">頂面</param>
        /// <param name="side">側面</param>
        /// <param name="bottom">底面</param>
        /// <param name="topPoints"></param>
        /// <param name="sidePoints"></param>
        /// <param name="bottomPoints"></param>
        /// <param name="topT"></param>
        /// <param name="sideT"></param>
        /// <param name="bottomT"></param>
        private void SplitTrianglesByNormal(Mesh originalEntity, Vector3D direction, IndexTriangle[] originalT,
            Mesh top, Mesh side, Mesh bottom, int[] topPoints, int[] sidePoints, int[] bottomPoints,
            ref int topT, ref int sideT, ref int bottomT)
        {
            for (int i = 0; i < originalT.Length; i++)
            {
                IndexTriangle it = originalT[i];
                Triangle t = new Triangle(originalEntity.Vertices[it.V1], originalEntity.Vertices[it.V2],
                    originalEntity.Vertices[it.V3]);
                t.Regen(0.1);
                double angle = Vector3D.AngleBetween(direction, t.Normal);

                //頂面
                if (Math.PI / 2 - Math.Abs(angle) > _angularTol)
                {
                    top.Triangles[i] = null;
                    side.Triangles[i] = null;
                    bottomT++;

                    topPoints[it.V3] = topPoints[it.V2] = topPoints[it.V1] = 1;
                }
                // 側面
                else if (Math.Abs(angle - Math.PI / 2) < _angularTol)
                {
                    bottom.Triangles[i] = null;
                    top.Triangles[i] = null;
                    sideT++;

                    sidePoints[it.V3] = sidePoints[it.V2] = sidePoints[it.V1] = 1;
                }
                //底面
                else
                {

                    bottom.Triangles[i] = null;
                    side.Triangles[i] = null;
                    topT++;

                    bottomPoints[it.V3] = bottomPoints[it.V2] = bottomPoints[it.V1] = 1;
                }
            }
        }
        /// <summary>
        /// 刪除未使用的三角面
        /// </summary>
        /// <param name="top">上視圖</param>
        /// <param name="front">前視圖</param>
        /// <param name="back">後視圖</param>
        /// <param name="topTriangleCount">上視圖三角面數量</param>
        /// <param name="fromTriangleCount">前視圖角面數量</param>
        /// <param name="backTriangleCount">後視圖角面數量</param>
        void DeleteUnusedTriangle(ref Mesh top, ref Mesh front, ref Mesh back, int topTriangleCount, int fromTriangleCount, int backTriangleCount)
        {
            int topDestCount = 0,
                frontDestCount = 0,
                backDestCount = 0;

            IndexTriangle[] topTriangles = new IndexTriangle[topTriangleCount],
                frontTriangles = new IndexTriangle[fromTriangleCount],
                backTriangles = new IndexTriangle[backTriangleCount];

            for (int i = 0; i < Steel.Triangles.Count(); i++)
            {
                if (top.Triangles[i] != null)
                {
                    topTriangles[topDestCount] = (IndexTriangle)top.Triangles[i].Clone();
                    topDestCount++;
                }
                if (front.Triangles[i] != null)
                {
                    frontTriangles[frontDestCount] = (IndexTriangle)front.Triangles[i].Clone();
                    frontDestCount++;
                }
                if (back.Triangles[i] != null)
                {
                    backTriangles[backDestCount] = (IndexTriangle)back.Triangles[i].Clone();
                    backDestCount++;
                }
            }
            top.Triangles = topTriangles;
            front.Triangles = frontTriangles;
            back.Triangles = backTriangles;
        }
        void _Triangulation()
        {

            Mesh top = (Mesh)Steel.Clone(); //上視圖
            Mesh front = (Mesh)Steel.Clone();//前視圖
            Mesh back = (Mesh)Steel.Clone();//後視圖

            //旋轉到到正確角度
            front.Rotate(Math.PI / -2, Vector3D.AxisX);
            back.Rotate(Math.PI / 2, Vector3D.AxisX);

            //移動到正確位置
            front.Translate(0, MoveFront);
            back.Translate(0, MoveBack);

            //用於儲存三角面數量
            int topCount, frontCount, backCount;

            //按法線分割三角形
            SplitTrianglesByNormal(top, out topCount, Vector3D.AxisZ);
            SplitTrianglesByNormal(front, out frontCount, new Vector3D(0, -1));
            SplitTrianglesByNormal(back, out backCount, Vector3D.AxisY);
            DeleteUnusedTriangle(ref top, ref front, ref back, topCount, frontCount, backCount);
            
            //刪除了未使用的頂點
            top = DeleteUnusedVertices(top);
            front = DeleteUnusedVertices(front);
            back = DeleteUnusedVertices(back);
            top.UpdateNormals();

            //加入物件
            this.Entities.Add(top);
            this.Entities.Add(front);
            this.Entities.Add(back);
            #region 備份
            //int topT = 0;
            //int bottomT = 0;
            //int sideT = 0;
            //Mesh topSection = (Mesh)Steel.Clone();
            //Mesh bottomSection = (Mesh)Steel.Clone();
            //Mesh sideSection = (Mesh)Steel.Clone();

            //int[] topPoints = new int[Steel.Vertices.Length];
            //int[] sidePoints = new int[Steel.Vertices.Length];
            //int[] bottomPoints = new int[Steel.Vertices.Length];


            //// 獲取三角形的第一個列表
            //SplitTrianglesByNormal(Steel, new Vector3D(0, 0, 1), Steel.Triangles, topSection, sideSection, bottomSection, topPoints, sidePoints, bottomPoints,
            //    ref topT, ref sideT, ref bottomT);


            //// 更新三角形截面數組
            //IndexTriangle[] bottom = new IndexTriangle[bottomT];
            //IndexTriangle[] top = new IndexTriangle[topT];
            //IndexTriangle[] side = new IndexTriangle[sideT];

            //int topDestCount = 0;
            //int bottomDestCount = 0;
            //int sideDestCount = 0;
            //for (int i = 0; i < Steel.Triangles.Length; i++)
            //{
            //    if (topSection.Triangles[i] != null)
            //    {
            //        top[topDestCount] = (IndexTriangle)topSection.Triangles[i].Clone();
            //        topDestCount++;
            //    }
            //    if (bottomSection.Triangles[i] != null)
            //    {
            //        bottom[bottomDestCount] = (IndexTriangle)bottomSection.Triangles[i].Clone();
            //        bottomDestCount++;
            //    }
            //    if (sideSection.Triangles[i] != null)
            //    {
            //        side[sideDestCount] = (IndexTriangle)sideSection.Triangles[i].Clone();
            //        sideDestCount++;
            //    }
            //}
            //topSection.Triangles = top;
            //bottomSection.Triangles = bottom;
            //sideSection.Triangles = side;

            ////刪除和重新排序頂點列表
            //sideSection = DeleteUnusedVertices(sideSection);
            //sideSection.Translate(0, -1000);
            ////topSection = DeleteUnusedVertices(topSection);
            //bottomSection = DeleteUnusedVertices(bottomSection);
            //bottomSection.Translate(0, 1000);
            //this.Entities.Add(bottomSection);
            //this.Entities.Add(topSection);
            //this.Entities.Add(sideSection);
            #endregion
        }

        /// <summary>
        /// 返回一個網格，其中刪除了未使用的頂點，並且頂點引用在網格m的“三角形”列表中重新排序。
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected Mesh DeleteUnusedVertices(Mesh m)
        {
            int[] newV = new int[m.Vertices.Length];
            int count = 1;

            foreach (IndexTriangle it in m.Triangles)
            {
                if (newV[it.V1] == 0)
                {
                    newV[it.V1] = count;
                    count++;
                }
                it.V1 = newV[it.V1] - 1;

                if (newV[it.V2] == 0)
                {
                    newV[it.V2] = count;
                    count++;
                }
                it.V2 = newV[it.V2] - 1;

                if (newV[it.V3] == 0)
                {
                    newV[it.V3] = count;
                    count++;
                }
                it.V3 = newV[it.V3] - 1;
            }

            Point3D[] finalV = new Point3D[count - 1];
            for (int i = 0; i < m.Vertices.Length; i++)
            {
                if (newV[i] != 0)
                {
                    finalV[newV[i] - 1] = m.Vertices[i];
                    //finalV[newV[i] - 1].Z = 0;
                }
            }
            m.Vertices = finalV;
            return m;
        }

        /// <summary>
        /// 三角畫法
        /// </summary>
        private void Triangulation()
        {

            //儲存列表
            List<Line>
                topList = new List<Line>(),
                backList = new List<Line>(),
                fromList = new List<Line>();
            foreach (Solid.Portion solidPortion in Steel.ConvertToSolid().Portions)
            {
                foreach (var item in solidPortion.Edges)
                {
                    //產生線段 0826依需求更換顏色(彥谷)
                    Line top =   new Line(solidPortion.Vertices[item.V1], solidPortion.Vertices[item.V2]) { Color = Color.White, ColorMethod = colorMethodType.byEntity };
                    //Line top = new Line(solidPortion.Vertices[item.V1], solidPortion.Vertices[item.V2]) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity };
                    //複製線段
                    Line from = (Line)top.Clone();
                    //from.Color = System.Drawing.Color.Blue;
                    Line back = (Line)top.Clone();
                    //back.Color = System.Drawing.Color.Gray;
                    //旋轉線段到正確位置
                    from.Rotate(Math.PI / -2, Vector3D.AxisX);
                    back.Rotate(Math.PI / 2, Vector3D.AxisX);
                    //移動線段到正確位置
                    from.Translate(0, MoveFront);
                    back.Translate(0, MoveBack);

                    //如果集合內數量 == 0 先加入到集合內
                    if (topList.Count == 0 && new Line(top.StartPoint.X, top.StartPoint.Y, top.EndPoint.X, top.EndPoint.Y).Length() > 0)
                        topList.Add(top);
                    if (backList.Count == 0 && new Line(back.StartPoint.X, back.StartPoint.Y, back.EndPoint.X, back.EndPoint.Y).Length() > 0)
                        backList.Add(back);
                    if (fromList.Count == 0 && new Line(from.StartPoint.X, from.StartPoint.Y, from.EndPoint.X, from.EndPoint.Y).Length() > 0)
                        fromList.Add(from);

                    AddLine(topList, top);//加入線段進 topList 內
                    AddLine(backList, back);//加入線段進 backList 內
                    AddLine(fromList, from);//加入線段進 fromList 內

                }
            }
            //找尋虛線
            int recursiveTime = 0;
            switch (SteelAttr.Type)
            {
                case OBJECT_TYPE.BH:
                case OBJECT_TYPE.RH:
                    recursiveTime = 0;
                    topList = ModifyLine(topList, SteelAttr.W / 2 + SteelAttr.t1 / 2, ref recursiveTime);
                    break;
                case OBJECT_TYPE.TUBE:
                case OBJECT_TYPE.BOX:
                case OBJECT_TYPE.L:
                case OBJECT_TYPE.CH:
                    recursiveTime = 0;
                    topList = ModifyLine(topList, SteelAttr.W, ref recursiveTime);
                    break;
                default:
                    break;
            }
            double correctionY = 0; //如果是角鐵自動補正 Y座標
            if (SteelAttr.Type == OBJECT_TYPE.L)
            {
                correctionY = SteelAttr.W - SteelAttr.t1 + MoveBack;
            }
            recursiveTime = 0;
            backList = ModifyLine(backList, SteelAttr.H, ref recursiveTime, correctionY);
            recursiveTime = 0;
            fromList = ModifyLine(fromList, 0, ref recursiveTime);
            //加入到圖塊內
            this.Entities.AddRange(topList);
            this.Entities.AddRange(backList);
            this.Entities.AddRange(fromList);

        }
        /// <summary>
        /// 加入線段。
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="line"></param>
        /// <remarks>
        /// 找到集合內有相同的線段( 只比較 X Y )。如果找不到相同的線段就加入到列表內。
        /// <para>
        /// 如果有找到相同的線段( X Y 相同 )，就會判斷相同的線哪個條線段 Z 軸較高，並且取代。
        /// </para>
        /// </remarks>
        private void AddLine(List<Line> lines, Line line)
        {
            //如果這條線段沒有在集合內
            if (lines.TrueForAll(el => !el.Contains2D(line)))
            {
                Point2D start2D = line.StartPoint;
                Point2D end2D = line.EndPoint;
                if (new Line(start2D.X, start2D.Y, end2D.X, end2D.Y).Length() > 0)
                {
                    AdjustLine(line);
                    //加入到集合內
                    lines.Add(line);
                }
            }
            //如果這條線段有在集合內
            else
            {
                //找到相同線段的陣列位置
                for (int i = 0; i < lines.Count; i++)
                {
                    //如果相同
                    if (lines[i].Contains2D(line))
                    {
                        //比較 Z 軸哪個較高並且回傳 Z 軸比較高的線段。
                        lines[i] = lines[i].StartPoint.Z > line.StartPoint.Z ? lines[i] : line;
                        return; //結束函數
                    }
                }
            }
        }
        /// <summary>
        /// 調整線段讓他由左至右
        /// </summary>
        /// <param name="line"></param>
        private void AdjustLine(Line line)
        {
            //調換線段的 X 座標 讓他由左至右
            Point3D start3D = line.StartPoint.X <= line.EndPoint.X && line.StartPoint.Y <= line.EndPoint.Y ? (Point3D)line.StartPoint.Clone() : (Point3D)line.EndPoint.Clone();
            Point3D end3D = line.StartPoint.X <= line.EndPoint.X && line.StartPoint.Y <= line.EndPoint.Y ? (Point3D)line.EndPoint.Clone() : (Point3D)line.StartPoint.Clone();
            line.StartPoint = start3D;
            line.EndPoint = end3D;
        }

        /// <summary>
        /// 取得最小線段集合位置
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="minX">最小 X 陣列位置</param>
        /// <param name="maxX">最大 X 陣列位置</param>
        private void GetStartLineIndex(List<Line> lines, ref int minX, ref int maxX)
        {
            //int result = 0;
            for (int i = 1; i < lines.Count; i++)
            {
                minX = MinX(lines[i], lines[minX]) == true ? i : minX;
                maxX = MaxX(lines[i], lines[maxX]) == true ? i : maxX;
            }
        }
        private bool MaxX(Line Comparison, Line Compared)
        {
            if (Comparison.StartPoint.X > Compared.StartPoint.X)
            {
                return true;
            }
            else if (Comparison.StartPoint.X == Compared.StartPoint.X)
            {
                return OtherComparisons(Comparison, Compared);
            }
            return false;
        }
        /// <summary>
        /// 取得最小值。比較值以 X 作主軸。比較方式 X -> Y -> Z
        /// </summary>
        /// <param name="Comparison">比較值</param>
        /// <param name="Compared">被比較值</param>
        /// <returns>比對值比較小回傳 true，比對值較大則回傳 false。</returns>
        private bool MinX(Line Comparison, Line Compared)
        {
            if (Comparison.StartPoint.X < Compared.StartPoint.X)
            {
                return true;
            }
            else if (Comparison.StartPoint.X == Compared.StartPoint.X)
            {
                return OtherComparisons(Comparison, Compared);
            }
            return false;
        }
        /// <summary>
        /// 比較 Y 軸與 Z 軸。比較方式 Y -> Z
        /// </summary>
        /// <param name="Comparison">比較值</param>
        /// <param name="Compared">被比較值</param>
        /// <returns>比對值比較小回傳 true，比對值較大則回傳 false。</returns>
        private bool OtherComparisons(Line Comparison, Line Compared)
        {
            if (Comparison.StartPoint.Y < Compared.StartPoint.Y)
            {
                return true;
            }
            else if (Comparison.StartPoint.Y == Compared.StartPoint.Y)
            {
                if (Comparison.StartPoint.Z > Compared.StartPoint.Z)
                {
                    return true;
                }
                else if (Comparison.StartPoint.Z == Compared.StartPoint.Z)
                {
                    return Comparison.Length() > Compared.Length() ? false : true;
                }
            }
            return false;
        }

        /// <summary>
        /// 修改看不到的線段為虛線
        /// 2022/08/09 呂宗霖 因遞迴造成StackOverFlow故限制20次(StackOverFlow無法利用try抓到錯誤)
        /// </summary>
        /// <param name="lines">線段集合</param>
        /// <param name="minZ">最小 Z 軸範圍</param>
        /// <param name="correctionY">補正 Y 軸</param>
        private List<Line> ModifyLine(List<Line> lines, double minZ,ref int recursiveTime, double correctionY = 0)
        {
            try
            {
                int outTime = 20;
                var result = lines.Where(el => el.StartPoint.Z >= minZ && el.EndPoint.Z >= minZ)?.ToList(); //在指定 Z 軸範圍內的線段
                if (result == null || recursiveTime> outTime)
                {
                    if (result == null)
                    {
                        return new List<Line>();
                    }
                    else {
                        return result;
                    }
                    
                }
                var noSuitable = lines.Where(el => !result.Contains(el)).ToList(); //在不再指定 Z 軸範圍內的線段(可能要改變的線段)
                double maxX = result.Max(el => el.EndPoint.X);//在指定 Z 軸範圍內的線段最大 X 座標
                double minX = result.Min(el => el.StartPoint.X);//在指定 Z 軸範圍內的線段最小 X 座標
                double maxY = result.Max(el => el.EndPoint.Y) - correctionY;//在指定 Z 軸範圍內的線段最大 Y 座標

                for (int i = 0; i < noSuitable.Count; i++)
                {
                    if ((maxY < noSuitable[i].StartPoint.Y || maxY < noSuitable[i].EndPoint.Y && SteelAttr.Type == OBJECT_TYPE.L)
                        || (noSuitable[i].StartPoint.X < minX || noSuitable[i].EndPoint.X > maxX && SteelAttr.Type != OBJECT_TYPE.L))
                    {
                        minZ = noSuitable[i].StartPoint.Z;
                        recursiveTime++;
                        return ModifyLine(lines, minZ,ref recursiveTime);
                    }
                    else
                    {
                       // noSuitable[i].LineTypeName = LineTypeName;
                        noSuitable[i].LineTypeMethod = colorMethodType.byEntity;
                    }
                    result.Add(noSuitable[i]);
                }
                //result.ForEach(el =>
                //{
                //    el.StartPoint.Z = 0;
                //    el.EndPoint.Z = 0;
                //});
                return result;
            }
            catch (Exception)
            {

                return new List<Line>();
            }
           
            #region 備份
            //var result = lines.Where(el => el.StartPoint.Z >= minZ && el.EndPoint.Z >= minZ).ToList(); //在指定 Z 軸範圍內的線段
            //var noSuitable = lines.Where(el => !result.Contains(el)).ToList(); //在不再指定 Z 軸範圍內的線段(可能要改變的線段)
            //double maxX = result.Max(el => el.EndPoint.X);//在指定 Z 軸範圍內的線段最大 X 座標
            //double minX = result.Min(el => el.StartPoint.X);//在指定 Z 軸範圍內的線段最小 X 座標
            //double maxY = result.Max(el => el.EndPoint.Y) - correctionY;//在指定 Z 軸範圍內的線段最大 Y 座標

            //for (int i = 0; i < noSuitable.Count; i++)
            //{
            //    if ((maxY < noSuitable[i].StartPoint.Y || maxY < noSuitable[i].EndPoint.Y && SteelAttr.Type == OBJECT_TYPE.L)
            //        || (noSuitable[i].StartPoint.X < minX || noSuitable[i].EndPoint.X > maxX && SteelAttr.Type != OBJECT_TYPE.L))
            //    {
            //        minZ = noSuitable[i].StartPoint.Z;
            //        return ModifyLine(lines, minZ);
            //    }
            //    else
            //    {
            //        noSuitable[i].LineTypeName = LineTypeName;
            //        noSuitable[i].LineTypeMethod = colorMethodType.byEntity;
            //    }
            //    result.Add(noSuitable[i]);
            //}
            //result.ForEach(el =>
            //{
            //    el.StartPoint.Z = 0;
            //    el.EndPoint.Z = 0;
            //});
            //return result;
            #endregion
        }
        #endregion
    }
}
