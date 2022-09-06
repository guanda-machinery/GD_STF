using devDept.Eyeshot;
using devDept.Geometry;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.Model;

namespace WPFSTD105.Tekla
{
    /// <summary>
    /// NC 外部輪廓
    /// </summary>
    [Serializable]
    public class AK
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public AK()
        {
            Parameter = new List<string>();
        }
        ///// <summary>
        ///// Z 軸座標
        ///// </summary>
        //public double Z { get; set; } = -1;
        /// <summary>
        /// 厚度
        /// </summary>
        public double t { get; set; } = 0;
        /// <inheritdoc/>
        public List<string> Parameter { get; set; }
        /// <inheritdoc/>
        public List<NcPoint3D> GetNcPoint(OBJECT_TYPE type = OBJECT_TYPE.RB)
        {
            //if (Z == -1)
            //{
            //    throw new Exception("Z 不可以是 -1。");
            //}
            /*else*/
            //圓棒以外再比較
            if (t <=0 && type != OBJECT_TYPE.RB && type != OBJECT_TYPE.Unknown && type != OBJECT_TYPE.L && type != OBJECT_TYPE.C)
            { 
                throw new Exception("t 不可以是 0。");
            }
            List<NcPoint3D> result = new List<NcPoint3D>();
            foreach (var item in Parameter)
            {
                //將 string 轉換 double
                List<double> values = GetValues(item);
                NcPoint3D point3D = new NcPoint3D(values[0],
                                                                                    values[1],
                                                                                    r: values[2],
                                                                                    angle1: values[3],
                                                                                    startAngle1: values[4],
                                                                                    angle2: values[5],
                                                                                    startAngle2: values[6]);
                if (result.Count -1 != -1)
                {
                    NcPoint3D p1 = result[result.Count -1];
                    if (p1.R != 0)
                    {
                        Point3D cen = CircleCenter(p1, point3D, p1.R); //計算圓心座標
                        double angle = Angle(cen, p1, point3D); //旋轉角度
                        double div = angle/10; //旋轉角度分成 10 等分
                        for (int i = 1; i < 9; i++)
                        {
                            NcPoint3D p = Rotate(cen, result[result.Count -1], div);
                            result.Add(p);
                        }
                    }
                }
                result.Add(point3D);
            }
            return result;
        }
        /// <summary>
        /// 取得資料行的值
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected List<double> GetValues(string para)
        {
            string[] str = para.Split(' ');
            List<double> result = new List<double>();
            for (int i = 0; i < str.Length; i++)
            {
                double? _ = GetValue(str[i]);
                if (_ != null)
                {
                    result.Add(_.Value);
                }
            }
            return result;
        }
        /// <summary>
        /// 取得數值
        /// </summary>
        /// <param name="str"></param>
        protected double? GetValue(string str)
        {
            MatchCollection matches = Regex.Matches(str, @"^[+-]?[0-9.]+");//找出數值包含小數點
            double? result = null;
            if (matches.Count > 0)
            {
                result = Convert.ToDouble(matches[0].Value);
            }
            return result;
        }
        /// <summary>
        /// 座標是順時針
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static bool IsClockwise(double x1, double y1, double x2, double y2)
        {
            if (x1< x2)
            {
                return true;
            }
            else if (x1> x2)
            {
                return false;
            }
            else
            {
                throw new Exception("找不到]計算方向");
            }
        }
        /// <summary>
        /// 計算圓心
        /// </summary>
        /// <param name="x1">第一點 x 向</param>
        /// <param name="y1">第一點 y 向</param>
        /// <param name="x2">第二點 x 向</param>
        /// <param name="y2">第二點 y 向</param>
        /// <param name="r">半徑</param>
        /// <param name="x">圓心座標 x</param>
        /// <param name="y">圓心座標 y</param>
        public static void CircleCenter(double x1, double y1, double x2, double y2, double r, out double x, out double y)
        {
            double c1 = (x2*x2 - x1*x1 + y2*y2 - y1*y1) / (2 *(x2 - x1));
            double c2 = (y2 - y1) / (x2 - x1);  //斜率
            double A = (c2*c2 + 1);
            double B = (2 * x1*c2 - 2 * c1*c2 - 2 * y1);
            double C = x1*x1 - 2 * x1*c1 + c1*c1 + y1*y1 - r*r;
            bool isClockwise = IsClockwise(x1, y1, x2, y2);
            y =!isClockwise ? (-B + Math.Sqrt(B*B - 4 * A*C)) / (2 * A) : (-B - Math.Sqrt(B*B - 4 * A*C)) / (2 * A);
            x = c1 - c2 * y;
        }
        /// <summary>
        /// 計算圓心
        /// </summary>
        /// <param name="p1">第一點</param>
        /// <param name="p2">第二點</param>
        /// <param name="r">半徑</param>
        /// <returns>圓心</returns>
        public static System.Windows.Point CircleCenter(System.Windows.Point p1, System.Windows.Point p2, double r)
        {
            CircleCenter(p1.X, p1.Y, p2.X, p2.Y, r, out double x, out double y);
            return new System.Windows.Point(x, y);
        }
        /// <summary>
        /// 計算圓心
        /// </summary>
        /// <param name="p1">第一點</param>
        /// <param name="p2">第二點</param>
        /// <param name="r">半徑</param>
        /// <returns>圓心</returns>
        public static Point2D CircleCenter(Point2D p1, Point2D p2, double r)
        {
            CircleCenter(p1.X, p1.Y, p2.X, p2.Y, r, out double x, out double y);

            return new Point2D(x, y);
        }
        /// <summary>
        /// 計算圓心
        /// </summary>
        /// <param name="p1">第一點</param>
        /// <param name="p2">第二點</param>
        /// <param name="r">半徑</param>
        /// <returns>圓心</returns>
        public static Point3D CircleCenter(Point3D p1, Point3D p2, double r)
        {
            CircleCenter(p1.X, p1.Y, p2.X, p2.Y, r, out double x, out double y);

            return new Point3D(x, y);
        }
        /// <summary>
        /// 計算弧度
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Radian(Point2D center, Point2D p1, Point2D p2)
        {
            return Math.PI / 180 * Angle(center, p1, p2);
        }
        /// <summary>
        /// 旋轉 <see cref="Point3D"/>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Point3D Rotate(Point3D center, Point3D p1, double angle)
        {
            double angleHude = angle * Math.PI / 180;/*角度變成弧度*/
            double x = (p1.X - center.X) * Math.Cos(angleHude) + (p1.Y - center.Y) * Math.Sin(angleHude) + center.X;
            double y = -(p1.X - center.X) * Math.Sin(angleHude) + (p1.Y - center.Y) * Math.Cos(angleHude) + center.Y;
            Point3D result = new Point3D(x, y, p1.Z);
            return result;
        }
        /// <summary>
        /// 旋轉 <see cref="NcPoint3D"/>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static NcPoint3D Rotate(Point3D center, NcPoint3D p1, double angle)
        {
            ClippingPlane clippingPlane = new ClippingPlane();
            Point3D p = Rotate(center, (Point3D)p1, angle);
            NcPoint3D result = (NcPoint3D)p1.DeepClone();
            result.X = p.X;
            result.Y = p.Y;
            return result;
        }
        /// <summary>
        /// 計算三角形邊長 B
        /// </summary>
        /// <param name="a">a 邊長</param>
        /// <param name="betaRadian">弧度</param>
        /// <returns></returns>
        public static double SideLengthB(double a, double betaRadian)
        {
            double result = Math.Sin(betaRadian)* (a/Math.Cos(betaRadian));
            return result;
        }

        /// <summary>
        /// 判斷點位是否在形狀內部
        /// </summary>
        /// <returns>在形狀內部回傳 ture，不是則回傳 false。</returns>
        public static bool InsideShape(Point3D[] shape, Point3D test)
        {
            int i;
            int j;
            bool result = false;
            for (i = 0, j = shape.Length - 1; i < shape.Length; j = i++)
            {
                if ((shape[i].Y > test.Y) != (shape[j].Y > test.Y) && (test.X < (shape[j].X - shape[i].X) * (test.Y - shape[i].Y) / (shape[j].Y - shape[i].Y) + shape[i].X))
                {
                    result = !result;
                }
            }
            return result;
        }
        /// <summary>
        /// 計算三角形邊長 C
        /// </summary>
        /// <param name="a">a 邊長</param>
        /// <param name="betaRadian">弧度</param>
        /// <returns></returns>
        public static double SideLengthC(double a, double betaRadian)
        {
            double result = a/Math.Cos(betaRadian);
            return result;
        }
        /// <summary>
        /// 轉換角度
        /// </summary>
        /// <param name="radian">弧度</param>
        public static double ConvertToAngle(double radian)
        {
            return 180 / Math.PI  * radian;
        }
        /// <summary>
        /// 轉換角度
        /// </summary>
        /// <param name="angle">角度</param>
        public static double ConvertToRadian(double angle)
        {
            return Math.PI /180  * angle;
        }
        /// <summary>
        /// 計算角度
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Angle(Point2D center, Point2D p1, Point2D p2)
        {
            double ma_x = p1.X - center.X;
            double ma_y = p1.Y - center.Y;
            double mb_x = p2.X - center.X;
            double mb_y = p2.Y - center.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double result = Math.Acos(cosM) *  180d / Math.PI;
            return result;
        }
    }
}
