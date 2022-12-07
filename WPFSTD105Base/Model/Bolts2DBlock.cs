using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WPFSTD105.Attribute;
using static WPFSTD105.Properties.SofSetting;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 代表 2D 螺栓
    /// </summary>
    public class Bolts2DBlock : Block, I2DBlock
    {
        /// <inheritdoc/>
        public double MoveBack { get; private set; }
        /// <inheritdoc/>
        public double MoveFront { get; private set; }
        /// <summary>
        /// 螺栓的面
        /// </summary>
        public readonly FACE Face;
        /// <summary>
        /// 鋼構參數
        /// </summary>
        public readonly SteelAttr SteelAttr;
        /// <summary>
        /// 標準結構
        /// </summary>
        /// <param name="bolts3DBlock"></param>
        /// <param name="steel2DBlock"></param>
        public Bolts2DBlock(Bolts3DBlock bolts3DBlock, Steel2DBlock steel2DBlock) : base(bolts3DBlock.Name)
        {
            if (bolts3DBlock.Info.Mode == AXIS_MODE.PIERCE)
            {
                CreateHole(bolts3DBlock, steel2DBlock);
            }
            else if (bolts3DBlock.Info.Mode == AXIS_MODE.POINT || bolts3DBlock.Info.Mode == AXIS_MODE.HypotenusePOINT)
            {
                CreatePoint(bolts3DBlock, steel2DBlock);
            }
            this.Face = bolts3DBlock.Info.Face;
        }
        /// <summary>
        /// 創建打點
        /// </summary>
        /// <param name="bolts3DBlock"></param>
        /// <param name="steel2DBlock"></param>
        private void CreatePoint(Bolts3DBlock bolts3DBlock, Steel2DBlock steel2DBlock)
        {
            SteelAttr steelAttr = steel2DBlock.SteelAttr;
            MoveBack = steel2DBlock.MoveBack;
            MoveFront = steel2DBlock.MoveFront;

            for (int i = 0; i < bolts3DBlock.Entities.Count; i++)
            {
                BoltAttr boltAttr = (BoltAttr)bolts3DBlock.Entities[i].EntityData;
                List<Line> lines = new List<Line>();
                Point3D center = null;
                switch (boltAttr.Face)
                {
                    case FACE.TOP:
                        center = new Point3D(boltAttr.X, boltAttr.Y);
                        break;
                    case FACE.FRONT:
                        center = new Point3D(boltAttr.X, boltAttr.Y + MoveFront);
                        break;
                    case FACE.BACK:
                        center = new Point3D(boltAttr.X, MoveBack - boltAttr.Y);
                        break;
                    default:
                        break;
                }
                Random rand = new Random();
                //打點顏色在這修改
                lines = GetCross(5, center, boltAttr, true).ToList();
                lines.ForEach(el =>
                {
                    el.Color = Color.Green;
                    el.Rotate(Math.PI / 4, Vector3D.AxisZ, center);
                    el.EntityData = boltAttr;
                    this.Entities.Add(el);
                });
                Circle circle = new Circle(Plane.XY, new Point3D(center.X, center.Y), boltAttr.Dia / 2)
                {
                    Color = System.Drawing.ColorTranslator.FromHtml(Default.Point),
                    ColorMethod = colorMethodType.byEntity,
                    EntityData = boltAttr
                };
                this.Entities.Add(circle);
            }
        }
        /// <summary>
        /// 創建孔位
        /// </summary>
        /// <param name="bolts3DBlock"></param>
        /// <param name="steel2DBlock"></param>
        private void CreateHole(Bolts3DBlock bolts3DBlock, Steel2DBlock steel2DBlock)
        {
            SteelAttr steelAttr = steel2DBlock.SteelAttr;
            MoveBack = steel2DBlock.MoveBack;
            MoveFront = steel2DBlock.MoveFront;
            /*細部圖像*/
            LinearPath flange = new LinearPath(bolts3DBlock.Info.Dia, steelAttr.t2);//翼板孔位
            LinearPath web = new LinearPath(bolts3DBlock.Info.Dia, steelAttr.t1);//腹板孔位
            Mesh meshWeb = UtilityEx.Triangulate(flange.Vertices);//腹板孔位
            Mesh meshFlange = UtilityEx.Triangulate(web.Vertices);//翼板孔位
            //改變細部孔位顏色
            meshWeb.Color = meshFlange.Color = System.Drawing.ColorTranslator.FromHtml(Default.Hole);
            meshWeb.ColorMethod = meshFlange.ColorMethod = colorMethodType.byEntity;
            meshWeb.Selectable = meshFlange.Selectable = false;//禁止讓用戶選擇

            //細部移動 y 軸
            double yFront = MoveFront + (steelAttr.W / 2 - steelAttr.t1 / 2);//移動到前視圖腹板位置的 y 座標
            double yBack = MoveBack - (steelAttr.W / 2 + steelAttr.t1 / 2);//移動到背視圖腹板位置的 y 座標
            double yTop = steelAttr.H - steelAttr.t2;//移動到俯視圖翼板上緣的 y 座標
            //存取有加入到細部過的 X 座標
            List<double> pooinX = new List<double>();
            for (int i = 0; i < bolts3DBlock.Entities.Count; i++)
            {
                BoltAttr boltAttr = (BoltAttr)bolts3DBlock.Entities[i].EntityData;

                // 圓心點
                Point3D center = new Point3D(boltAttr.X, boltAttr.Y);
                Point3D[] temp= bolts3DBlock.Entities[i].Vertices;
                Point3D a = new Point3D(temp[0].X, temp[0].Y, temp[0].Z);
                Point3D b = new Point3D(temp[1].X, temp[1].Y, temp[1].Z);
                Point3D c = new Point3D(temp[2].X, temp[2].Y, temp[2].Z);
                center = CenterCalculator(boltAttr.Face, a, b, c);

                Circle circle = new Circle(Plane.XY, center, boltAttr.Dia / 2)
                {
                    Color = System.Drawing.ColorTranslator.FromHtml(Default.Hole),
                    ColorMethod = colorMethodType.byEntity,
                    EntityData = boltAttr
                };
                Mesh[] meshes = null;//細部孔位

                ////判斷是否加入過物件
                //if (!pooinX.Contains(circle.Center.X))
                //{
                switch (boltAttr.Face)
                {
                    case FACE.TOP:
                        meshes = new[] { (Mesh)meshFlange.Clone(), (Mesh)meshFlange.Clone() };
                        #region FRONT
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, yFront);
                        meshes[0].EntityData = boltAttr;                        
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yFront + steelAttr.t1 / 2), boltAttr));
#if DEBUG
                        log4net.LogManager.GetLogger($"TOP").Debug(
                            $"[FRONT]座標轉換\n" +
                            $"原始座標:X:{circle.Center.X},\t半徑:{boltAttr.Dia / 2},\tY:0,\t(X=X-半徑)\n" +
                            $"原始座標:({boltAttr.X},\t{boltAttr.Y},\t{boltAttr.Z})\t計算後如下\n" +
                            $"算後座標:({circle.Center.X - boltAttr.Dia / 2},\t{yFront},\t{0})");
                        log4net.LogManager.GetLogger($"TOP").Debug(
                            $"[FRONT]孔位穿過中心的線段\n" +
                            $"高度:{steelAttr.t2}\n" +
                            $"X:{circle.Center.X},\tY:{yFront},\t腹板厚度:{steelAttr.t1} (y=y+腹板厚度/2)\t計算後如下\n" +
                            $"X:{circle.Center.X},\tY:{yFront + steelAttr.t1 / 2}\n");
#endif
                        #endregion
                        #region BACK
                        meshes[1].Translate(circle.Center.X - boltAttr.Dia / 2, yBack);
                        meshes[1].EntityData = boltAttr;
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yBack + steelAttr.t1 / 2), boltAttr));
#if DEBUG
                        log4net.LogManager.GetLogger($"TOP").Debug(
                            $"[BACK]座標轉換\n" +
                            $"原始座標:X:{circle.Center.X},\t半徑:{boltAttr.Dia / 2},\tY:0,\t(X=X-半徑)\n" +
                            $"原始座標:({boltAttr.X},\t{boltAttr.Y},\t{boltAttr.Z})\t計算後如下\n" +
                            $"算後座標:({circle.Center.X},\t{yBack + steelAttr.t1 / 2},\t{0})");
                        log4net.LogManager.GetLogger($"TOP").Debug(
                            $"[BACK]孔位穿過中心的線段\n" +
                            $"高度:{steelAttr.t2}\n" +
                            $"X:{circle.Center.X},\tY:{yBack},\t腹板厚度:{steelAttr.t1} (y=y+腹板厚度/2)\t計算後如下\n" +
                            $"X:{circle.Center.X},\tY:{yBack + steelAttr.t1 / 2}\n");
#endif
                        #endregion
                        break;
                    case FACE.FRONT:
                        circle.Translate(0, MoveFront);
                        meshes = new[] { (Mesh)meshFlange.Clone() };
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, 0);
                        meshes[0].EntityData = boltAttr;
                        //高度    中心點 物件boltAttr
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, steelAttr.t2 / 2), boltAttr));
#if DEBUG
                        log4net.LogManager.GetLogger($"FRONT").Debug(
                            $"[FRONT]座標轉換\n" +
                            $"原始座標:X:{circle.Center.X},\t半徑:{boltAttr.Dia / 2},\tY:0,\t(X=X-半徑)\n" +
                            $"原始座標:({boltAttr.X},\t{boltAttr.Y},\t{boltAttr.Z})\t計算後如下\n" +
                            $"算後座標:({circle.Center.X - boltAttr.Dia / 2},\t{0},\t{0})");
                        log4net.LogManager.GetLogger($"FRONT").Debug(
                            $"[FRONT]孔位穿過中心的線段\n" +
                            $"高度:{steelAttr.t2}\n" +
                            $"X:{circle.Center.X},\tY:0,\t翼板厚度:{steelAttr.t2} (y=y+翼板厚度/2)\t計算後如下\n" +
                            $"(X:{circle.Center.X},\tY:{steelAttr.t2 / 2},\t0)\n");
#endif
                        break;
                    case FACE.BACK:
                        circle.Rotate(Math.PI, Vector3D.AxisX);
                        circle.Translate(0, MoveBack);
                        meshes = new[] { (Mesh)meshWeb.Clone() };
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, yTop);
                        meshes[0].EntityData = boltAttr;
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yTop + steelAttr.t2 / 2), boltAttr));
#if DEBUG
                        log4net.LogManager.GetLogger($"BACK").Debug(
                            $"[BACK]座標轉換\n" +
                            $"原始座標:X:{circle.Center.X},\t半徑:{boltAttr.Dia / 2},\tY:{yTop},\t(X=X-半徑)\n" +
                            $"原始座標:({boltAttr.X},\t{boltAttr.Y},\t{boltAttr.Z})\t計算後如下\n" +
                            $"算後座標:({circle.Center.X - boltAttr.Dia / 2},\t{yTop},\t{0})");
                        log4net.LogManager.GetLogger($"BACK").Debug(
                            $"[BACK]孔位穿過中心的線段\n" +
                            $"高度:{steelAttr.t2}\n" +
                            $"X:{circle.Center.X},\tY:{yTop},\t翼板厚度:{steelAttr.t2} (y=y+翼板厚度/2)\t計算後如下\n" +
                            $"X:({circle.Center.X},\tY:{yTop + steelAttr.t2 / 2},\t0)\n");
#endif
                        break;
                    default:
                        throw new Exception($"找不到 {boltAttr.Face.ToString()}");
                }

                this.Entities.AddRange(GetCross(boltAttr.Dia, circle.Center, boltAttr, true));
                this.Entities.AddRange(meshes);
                //}
                this.Entities.Add(circle);
            }
        }

        /// <summary>
        /// 圓心計算
        /// 給定三點ABC
        /// 計算AB.BC.AC長
        /// (AX*AB+BX*BC+CX*AC)/(AB+BC+AC)=X座標
        /// 小葉有寫圓心計算公式 若不對再參考CircleCenter
        /// </summary>
        /// <returns></returns>
        public Point3D CenterCalculator(FACE face, Point3D a, Point3D b, Point3D c)
        {
            //// 計算長度
            //double ab = Math.Sqrt(Math.Abs((a.X * a.X - b.X * b.X) + (a.Y * a.Y - b.Y * b.Y) + (a.Z * a.Z - b.Z * b.Z)));
            //double bc = Math.Sqrt(Math.Abs((c.X * c.X - b.X * b.X) + (c.Y * c.Y - b.Y * b.Y) + (c.Z * c.Z - b.Z * b.Z)));
            //double ac = Math.Sqrt(Math.Abs((a.X * a.X - c.X * c.X) + (a.Y * a.Y - c.Y * c.Y) + (a.Z * a.Z - c.Z * c.Z)));
            //
            //// X座標
            //double x = (ab * a.X + bc * b.X + ac * c.X) / (ab + bc + ac);
            //// Y座標
            //double y = (ab * a.Y + bc * b.Y + ac * c.Y) / (ab + bc + ac);
            //// Z座標
            //double z = (ab * a.Z + bc * b.Z + ac * c.Z) / (ab + bc + ac);

            //return new Point3D(x, y, z);

            double fix = 0;
            switch (face)
            {
                case FACE.TOP:
                    fix = a.Z;
                    break;
                case FACE.FRONT:
                case FACE.BACK:
                    // Y軸依樣
                    fix = a.Y;
                    a = new Point3D(a.X, a.Z, 0);
                    b = new Point3D(b.X, b.Z, 0);
                    c = new Point3D(c.X, c.Z, 0);
                    break;
                default:
                    break;
            }

            double x1, y1, z1, x2, y2, z2, x3, y3, z3;
            double e, f, a1, b1, c1, d, g;
            x1 = a.X;
            y1 = a.Y;
            x2 = b.X;
            y2 = b.Y;
            x3 = c.X;
            y3 = c.Y;
            e = 2 * (x2 - x1);
            f = 2 * (y2 - y1);
            g = x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1;
            a1 = 2 * (x3 - x2);
            b1 = 2 * (y3 - y2);
            d = x3 * x3 - x2 * x2 + y3 * y3 - y2 * y2;

            double X = (g * b1 - d * f) / (e * b1 - a1 * f);
            double Y = (g * a1 - d * e) / (a1 * f - b1 * e);

            double R = (float)Math.Sqrt((X - x1) * (X - x1) + (Y - y1) * (Y - y1));

            switch (face)
            {
                case FACE.TOP:
                    // TOP視角 Z軸不變
                    return new Point3D(X, Y, fix);
                case FACE.FRONT:
                case FACE.BACK:
                    // 前後視角 Y軸不變
                    Point3D r = new Point3D(X, fix, Y);
                    double z = 0, y = 0;
                    y = r.Z;
                    z = r.Y;
                    r.Y = y;
                    r.Z = z;
                    return r;
                default:
                    return new Point3D();
            }




            return new Point3D();
        }



        const double extend = 10;
        /// <summary>
        /// 取得圓形十字線
        /// </summary>
        /// <param name="dia">直徑</param>
        /// <param name="center">中心</param>
        /// <param name="boltAttr">物件識別 ID </param>
        /// <param name="selectable"></param>
        /// <returns></returns>
        private Line[] GetCross(double dia, Point3D center, BoltAttr boltAttr, bool selectable = false)
        {
            Line line1 = Piercing(dia, center, boltAttr, selectable);
            Line line2 = (Line)line1.Clone();

            Vector3D vector3D = new Vector3D(center, new Point3D(center.X + 1, center.Y), new Point3D(center.X, center.Y + 1));
            line2.Rotate(Math.PI / 2, Vector3D.AxisZ, center);

            return new[] { line1, line2 };
        }
        /// <summary>
        /// 孔位穿過中心的線段
        /// </summary>
        /// <param name="dy">高度</param>
        /// <param name="center">中心點</param>
        /// <param name="boltAttr">物件識別 ID </param>
        /// <param name="selectable"></param>
        /// <returns></returns>
        private Line Piercing(double dy, Point3D center, BoltAttr boltAttr, bool selectable = false)
        {
            Line result = new Line(center.X, center.Y - dy / 2 - extend, center.X, center.Y + dy / 2 + extend);
            result.EntityData = boltAttr;
            result.Selectable = selectable;
            result.Color = System.Drawing.ColorTranslator.FromHtml(Default.Hole);
            result.ColorMethod = colorMethodType.byEntity;

            return result;
        }
    }
}
