using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    /// <summary>
    /// NC 座標
    /// </summary>
    [Serializable]
    public class NcPoint3D : Point3D
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public NcPoint3D() : base()
        {

        }
        /// <summary>
        /// NC 座標
        /// </summary>
        /// <param name="x">X 座標</param>
        /// <param name="y">Y 座標</param>
        public NcPoint3D(double x, double y) : base(x, y)
        {

        }
        ///// <summary>
        ///// NC 座標
        ///// </summary>
        ///// <param name="x">X 座標</param>
        ///// <param name="y">Y 座標</param>
        ///// <param name="r">半徑</param>
        ///// <param name="angle">斜面角度</param>
        //public NcPoint3D(double x, double y, double r = 0, double angle = 0) : base(x, y)
        //{
        //    R=r;
        //    Angle=angle;
        //}
        /// <summary>
        /// NC 座標
        /// </summary>
        /// <param name="x">X 座標</param>
        /// <param name="y">Y 座標</param>
        /// <param name="r">半徑</param>
        /// <param name="angle1">斜面角度</param>
        /// <param name="startAngle1">角度起始位置</param>
        /// <param name="angle2">斜面角度</param>
        /// <param name="startAngle2">角度起始位置</param>
        public NcPoint3D(double x, double y, double r = 0, double angle1 = 0, double startAngle1 = 0, double angle2 =0, double startAngle2 = 0) : base(x, y)
        {
            R=r;
            Angle1=angle1;
            Angle2 = angle2;
            StartAngle1 = startAngle1;
            StartAngle2 = startAngle2; 
        }
        /// <summary>
        /// 半徑
        /// </summary>
        public double R { get; set; }
        /// <summary>
        /// 角度1
        /// </summary>
        public double Angle1 { get; set; }
        /// <summary>
        /// 角度1起始位置
        /// </summary>
        public double StartAngle1 { get; set; }
        /// <summary>
        /// 角度2
        /// </summary>
        public double Angle2 { get; set; }
        /// <summary>
        /// 角度2起始位置
        /// </summary>
        public double StartAngle2 { get; set; }
        /// <summary>
        /// 深層複製物件
        /// </summary>
        /// <returns></returns>
        public object DeepClone()
        {
            using (Stream stream = new MemoryStream())
            {

                IFormatter formatter = new BinaryFormatter(); //序列化物件格式
                formatter.Serialize(stream, this); //將自己所有資料序列化
                stream.Seek(0, SeekOrigin.Begin);//複寫資料流位置,返回最前端
                NcPoint3D result = (NcPoint3D)formatter.Deserialize(stream); //再將Stream反序列化回去 
                return result;
            }
        }
    }
}
