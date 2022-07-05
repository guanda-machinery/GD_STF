using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD.Enum;
using GD_STD;
using devDept.Eyeshot;
using static WPFSTD105.Properties.SofSetting;
using devDept.Graphics;
using System.Drawing;

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
            else if (bolts3DBlock.Info.Mode == AXIS_MODE.POINT)
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

                //打點顏色在這修改
                lines = GetCross(5, center, boltAttr, true).ToList();
                lines.ForEach(el =>
                {
                    el.Color = Color.Green;
                    el.Rotate(Math.PI / 4, Vector3D.AxisZ, center);
                    el.EntityData = boltAttr;
                    this.Entities.Add(el);
                });
                Circle circle = new Circle(Plane.XY, new Point3D(boltAttr.X, boltAttr.Y), boltAttr.Dia / 2)
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
            
                Circle circle = new Circle(Plane.XY, new Point3D(boltAttr.X, boltAttr.Y), boltAttr.Dia / 2)
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
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, yFront);
                        meshes[0].EntityData = boltAttr;
                        meshes[1].Translate(circle.Center.X - boltAttr.Dia / 2, yBack);
                        meshes[1].EntityData = boltAttr;
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yFront + steelAttr.t1 / 2), boltAttr));
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yBack + steelAttr.t1 / 2), boltAttr));
                        break;
                    case FACE.FRONT:
                        circle.Translate(0, MoveFront);
                        meshes = new[] { (Mesh)meshWeb.Clone() };
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, 0);
                        meshes[0].EntityData = boltAttr;
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, steelAttr.t2 / 2), boltAttr));
                        break;
                    case FACE.BACK:
                        circle.Rotate(Math.PI, Vector3D.AxisX);
                        circle.Translate(0, MoveBack);
                        meshes = new[] { (Mesh)meshWeb.Clone() };
                        meshes[0].Translate(circle.Center.X - boltAttr.Dia / 2, yTop);
                        meshes[0].EntityData = boltAttr;
                        this.Entities.Add(Piercing(steelAttr.t2, new Point3D(circle.Center.X, yTop + steelAttr.t2 / 2), boltAttr));
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
