using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 切割線
    /// </summary>
    [Serializable]
    public class CutContour
    {
        /// <summary>
        /// 右邊上緣
        /// </summary>
        public List<Point3D> UR { get; set; } = new List<Point3D>();
        /// <summary>
        /// 右邊下緣
        /// </summary>
        public List<Point3D> DR { get; set; } = new List<Point3D>();
        /// <summary>
        /// 左邊上緣
        /// </summary>
        public List<Point3D> UL { get; set; } = new List<Point3D>();
        /// <summary>
        /// 左邊下緣
        /// </summary>
        public List<Point3D> DL { get; set; } = new List<Point3D>();
        /// <summary>
        /// 獲取有角度的線段
        /// </summary>
        public static Line GetAngleLine(List<Point3D> points)
        {
            double angleOfLine = Math.Atan2((points[0].Y - points[2].Y), (points[0].X - points[2].X)) * 180 / Math.PI;
            if (angleOfLine % 90 != 0)
            {
                return new Line(points[0], points[2]);
            }
            for (int i = 1; i < points.Count; i++)
            {
                angleOfLine = Math.Atan2((points[i - 1].Y - points[i].Y), (points[i - 1].X - points[i].X)) * 180 / Math.PI;
                if (angleOfLine % 90 != 0)
                {
                    return new Line(points[i - 1], points[i]);
                }
            }

            throw new Exception("找不到有角度的線段");
        }
        double x = 500;
        /// <summary>
        /// 取得<see cref="DL"/>切割點
        /// </summary>
        /// <returns>放大過後的切割點。防止破圖</returns>
        public List<Point3D> GetDLCutPoint()
        {
            List<Point3D> result = DL.ToList();
            if (result.Count == 0)
            {
                return result;
            }
            double alpha = Alpha(result[2].Y, result[1].X);
            double beta = Math.PI / 2 - alpha;

            //double x = 200; //延伸距離

            //result[2].X -= x;
            //result[2].Y += Math.Tan(alpha) * x;
            //result[1].X += Math.Tan(beta) * x;
            //result[1].Y -= x;
            return result;
        }
        /// <summary>
        ///  取得<see cref="DR"/>切割點
        /// </summary>
        /// <returns>放大過後的切割點。防止破圖</returns>
        public List<Point3D> GetDRCutPoint()
        {
            List<Point3D> result = DR.ToList();
            if (result.Count == 0)
            {
                return result;
            }

            double alpha = Alpha(result[2].Y, result[0].X - result[2].X);
            double beta = Math.PI / 2 - alpha;

            //double x = 200; //延伸距離

            //result[2].X += x;
            //result[2].Y += Math.Tan(alpha) * x;
            //result[0].X -= Math.Tan(beta) * x;
            //result[0].Y -= x;
           
            return result;
        }
        /// <summary>
        /// 取得<see cref="UL"/>切割點
        /// </summary>
        /// <returns>放大過後的切割點。防止破圖</returns>
        public List<Point3D> GetULCutPoint()
        {
            List<Point3D> result = UL.ToList();
            if (result.Count == 0)
            {
                return result;
            }

            double alpha = Alpha(result[1].Y - result[0].Y, result[2].X);
            double beta = Math.PI / 2 - alpha;

            //result[2].X += Math.Tan(beta) * x;
            //result[2].Y += x;
            //result[0].X -= x;
            //result[0].Y -= Math.Tan(alpha) * x;

            return result;
        }
        /// <summary>
        /// 取得<see cref="UR"/>切割點
        /// </summary>
        /// <returns>放大過後的切割點。防止破圖</returns>
        public List<Point3D> GetURCutPoint()
        {
            List<Point3D> result = UR.ToList();
            if (result.Count == 0)
            {
                return result;
            }

            double alpha = Alpha(result[1].Y - result[2].Y, result[0].X - result[2].X);
            double beta = Math.PI / 2 - alpha;

            //double x = 200; //延伸距離

            //result[0].X -= Math.Tan(beta) * x;
            //result[0].Y += x;
            //result[1].X += x;
            //result[1].Y -= Math.Tan(beta) * x;
            return result;
        }
        /// <summary>
        /// 取得阿爾法 (α),
        /// </summary>
        /// <returns></returns>
        private double Alpha(double a, double b)
        {
            // 斜邊
            double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            // a/斜邊
            return Math.Asin(a / c);
        }
    }
}
