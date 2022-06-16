using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Model
{
    /// <summary>
    /// 螺栓陣列
    /// </summary>
    public class ArrayBolts : List<Bolt>
    {
        #region 公用屬性
        /// <summary>
        /// 直徑
        /// </summary>
        public double Dia { get; private set; }
        /// <summary>
        /// 尚未完成的圓柱體高度
        /// </summary>
        /// <remarks>
        /// 因要辨別完成的孔位，所以尚未鑽破會有圓柱體當時體擋住挖空孔位。
        /// </remarks>
        public double t { get; private set; }
        /// <summary>
        /// 實體面
        /// </summary>
        public FACE Face { get; private set; }
        public AXIS_MODE Model { get; set; }
        #endregion

        /// <summary>
        /// 螺栓陣列
        /// </summary>
        /// <param name="xNumber">X向數量</param>
        /// <param name="yNumber">Y向數量</param>
        /// <param name="dx">X向間距</param>
        /// <param name="dy">Y向間距</param>
        ///  <param name="sx">X起始距離</param>
        ///  <param name="sy">Y起始距離</param>
        /// <param name="diameter">直徑</param>
        /// <param name="t">尚未完成的圓柱體高度</param>
        /// <param name="face">實體面的位置</param>
        /// <param name="mode">鑽孔模式</param>
        public ArrayBolts(int xNumber, int yNumber, double dx, double dy, double sx, double sy, double diameter, double t, FACE face, AXIS_MODE mode = AXIS_MODE.PIERCE)
        {
            this.Dia = diameter;
            this.t = t;
            this.Face = face;
        }

        #region 私有屬性
        /// <summary>
        /// 創建螺栓群組
        /// </summary>
        /// <param name="xNumber">X向數量</param>
        /// <param name="yNumber">Y向數量</param>
        /// <param name="dx">X向間距</param>
        /// <param name="dy">Y向間距</param>
        ///  <param name="sx">X起始距離</param>
        ///  <param name="sy">Y起始距離</param>
        private void CreateBolts(int xNumber, int yNumber, double dx, double dy, double sx, double sy)
        {
            List<Bolt> resultY = new List<Bolt>();//Y向螺栓結果，要給X向複製用
            resultY.Add(Mesh.CreateCone<Bolt>(this.Dia * 0.5, this.Dia * 0.5, this.t, 11));//創建一個圓柱體
            resultY[0].Translate(sx, dy); //使用相對移動到指定位置。

            //先產生Y向，原因是，三軸機先Y向移動工作效率較大
            for (int i = 0; i < yNumber; i++)
            {
                Bolt bolt = (Bolt)resultY[0].Clone();//複製物件
                bolt.Translate(0, dy, 0);//使用相對移動到指定位置。
                resultY.Add(bolt);//存取物件
                this.Add(bolt);//存到列表內
            }
            //產生X向螺栓
            for (int i = 0; i < xNumber; i++)
            {
                resultY = BoltsX(resultY, dx);
                this.AddRange(resultY);
            }
        }
        /// <summary>
        /// 螺栓 X 向列表
        /// </summary>
        /// <param name="boltY">y 向螺栓列表</param>
        /// <param name="dx">X向間距</param>
        /// <returns></returns>
        private List<Bolt> BoltsX(List<Bolt> boltY, double dx)
        {
            List<Bolt> result = new List<Bolt>();
            for (int i = 0; i < boltY.Count; i++)
            {
                Bolt bolt = (Bolt)boltY[0].Clone();
                bolt.Translate(dx, 0, 0);
                result.Add(bolt);
            }
            //順便排序加工位置
            result.Reverse();
            return result;
        }
        #endregion

        #region 公開方法
        /// <summary>
        /// 移動螺栓位置
        /// </summary>
        /// <param name="x">x相對座標</param>
        /// <param name="y">y相對座標</param>
        /// <param name="z">z相對座標</param>
        public void Move(double x, double y, double z)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Translate(x, y, z);
            }
        }
        /// <summary>
        /// 將實體繞任何軸旋轉指定角度。
        /// </summary>
        /// <param name="angleInRadians"></param>
        /// <param name="axis"></param>
        public void Rotate(double angleInRadians, Vector3D axis)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Rotate(angleInRadians, axis);
            }
        }
        /// <summary>
        /// 轉換 Codesys Memory 可用資料 
        /// </summary>
        /// <returns></returns>
        public Drill[] ToCodesys()
        {
            Drill[] result = new Drill[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                result[i] = this[i].ToCodesys(Dia, Model, Face);
            }
            return result;
        }
        #endregion
    }
}
