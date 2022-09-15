#define Debug 
using devDept.Geometry;
using System;
using System.Collections.Generic;
using WPFSTD105.Model;
using WPFSTD105.Surrogate;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 螺栓群組設定檔
    /// </summary>
    [Serializable]
    public class GroupBoltsAttr : BoltAttr, IBoltAttr, IGroupBoltsAttr
    {
        /// <summary>
        /// 取得孔位座標
        /// </summary>
        /// <returns></returns>
        public Point3D GetPoint()
        {
            return new Point3D(X, Y, Z);
        }
        /// <summary>
        /// 返回世界座標
        /// </summary>
        public void Coordinates()
        {
            switch (Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    return;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    double y, z;
                    y = Z;
                    z = Y;
                    Y = y;
                    Z = z;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 螺栓總數量
        /// </summary>
        public int Count { get => xCount * yCount; }
        /// <summary>
        /// 第一顆螺栓的絕對座標 X 向
        /// </summary>
        public string BlackName { get; set; } = "";
        public override double X { get; set; } = 35;
        /// <summary>
        /// 第一顆螺栓的絕對座標 Y 向
        /// </summary>
        public override double Y { get; set; }
        /// <summary>
        /// 第一顆螺栓的絕對座標 Z 向
        /// </summary>
        public override double Z { get;  set; }
        /// <inheritdoc/>
        public string dX { get; set; } = "60 2*70 60";
        /// <inheritdoc/>
        public string dY { get; set; } = "60 2*70 60";
        /// <summary>
        /// X 向螺栓數量
        /// </summary>
        public int xCount { get; set; } = 5;
        /// <summary>
        /// Y 向螺栓數量
        /// </summary>
        public int yCount { get; set; } = 5;
        /// <summary>
        /// 起始孔位置
        /// </summary>
        public START_HOLE StartHole { get; set; }
        /// <summary>
        /// 加總 X 向間距
        /// </summary>
        /// <returns></returns>
        public double SumdX()
        {
            List<double> list = dXs;
            double result = 0d;
            for (int i = 0; i < xCount - 1; i++)
            {
                if (list.Count - 1 > i)
                    result += list[i];
                else
                    result += list[list.Count - 1];
            }
#if DEBUG
            log4net.LogManager.GetLogger($"解析{nameof(SumdY)}").Debug($"{result}");
#endif
            return result;
        }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'GroupBoltsAttr.SumdY()' 的 XML 註解
        public double SumdY()
        {
            List<double> list = dYs;
            double result = 0d;
            for (int i = 0; i < yCount - 1; i++)
            {
                if (list.Count - 1 > i)
                    result += list[i];
                else
                    result += list[list.Count - 1];
            }
#if DEBUG
            log4net.LogManager.GetLogger($"解析{nameof(SumdY)}").Debug($"{result}");
#endif
            return result;
        }
        /// <summary>
        /// 螺栓 X 向矩陣間距
        /// </summary>
        public List<double> dXs
        {
            get
            {
                string[] value = dX.Split(' ');
                List<double> result = new List<double>();
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Contains("*")) //如果有新號
                    {
                        string[] str = value[i].Split('*');
                        int count = 0;
                        Int32.TryParse(str[0], out count);
                        for (int c = 0; c < count; c++)
                        {
                            result.Add(Convert.ToDouble(str[1]));
                        }
                    }
                    else if (value[i] != "")
                    {
                        result.Add(Convert.ToDouble(value[i]));
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 螺栓 Y 向矩陣間距
        /// </summary>
        public List<double> dYs
        {
            get
            {
                string[] value = dY.Split(' ');
                List<double> result = new List<double>();
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Contains("*")) //如果有新號
                    {
                        string[] str = value[i].Split('*');
                        int count = 0;
                        Int32.TryParse(str[0], out count);
                        for (int c = 0; c < count; c++)
                        {
                            result.Add(Convert.ToDouble(str[1]));
                        }
                    }
                    else if (value[i] != "")
                    {
                        result.Add(Convert.ToDouble(value[i]));
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// <see cref="BoltAttr"/> 轉換 <see cref="GroupBoltsAttrSurrogate"/>
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0114 // 'GroupBoltsAttr.ConvertToSurrogate()' 會隱藏繼承的成員 'BoltAttr.ConvertToSurrogate()'。若要讓目前的成員覆寫該實作，請加入 override 關鍵字; 否則請加入 new 關鍵字。
        public virtual GroupBoltsAttrSurrogate ConvertToSurrogate()
#pragma warning restore CS0114 // 'GroupBoltsAttr.ConvertToSurrogate()' 會隱藏繼承的成員 'BoltAttr.ConvertToSurrogate()'。若要讓目前的成員覆寫該實作，請加入 override 關鍵字; 否則請加入 new 關鍵字。
        {
            return new GroupBoltsAttrSurrogate(this);
        }
    }
}
