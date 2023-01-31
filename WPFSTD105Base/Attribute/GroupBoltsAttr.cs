#define Debug 
using devDept.Geometry;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //public GroupBoltsAttr() { }
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
        //public string BlockName { get; set; } = "";
        public override double X { get; set; } = 35;
        /// <summary>
        /// 第一顆螺栓的絕對座標 Y 向
        /// </summary>
        public override double Y { get; set; }
        /// <summary>
        /// 第一顆螺栓的絕對座標 Z 向
        /// </summary>
        public override double Z { get; set; }
        /// <inheritdoc/>
        public string dX { get; set; } = "60 2*70 60";
        /// <inheritdoc/>
        public string dY { get; set; } = "60 2*70 60";
        /// <summary>
        /// X 向螺栓數量
        /// </summary>
        public int xCount { get => CalBoltNumber(dX); set => CalBoltNumber(dX); }
        /// <summary>
        /// Y 向螺栓數量
        /// </summary>
        public int yCount { get => CalBoltNumber(dY); set => CalBoltNumber(dY); }
        /// <summary>
        /// 起始孔位置
        /// </summary>
        public START_HOLE StartHole { get; set; } = START_HOLE.MIDDLE;
        /// <summary>
        /// 孔群種類
        /// </summary>
        public GroupBoltsType groupBoltsType { get; set; }
        /// <summary>
        /// x座標左起算或右起算
        /// </summary>
        public ArrayDirection X_BoltsArrayDirection { get; set; }

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

        /// <summary>
        /// 由使用者提供之孔距計算數量
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int CalBoltNumber(string str = "60 2*70 60")
        {
            if (str == "0")
            {
                return 1;
            }
            int count = 0;
            // 依照空格拆解
            var space = str.Split(' ').ToList();
            foreach (var item in space)
            {
                if (item.IndexOf('*') == -1) { count++; }
                else
                {
                    var start = item.Split('*');
                    count += int.Parse(start[0]);
                }
            }
            return count + 1;
        }

        /// <summary>
        /// 計算右側起步點
        /// </summary>
        /// <returns></returns>
        public double RightXStart(double SteelLength)
        {
            double x = 0;
            //第一顆螺栓的絕對座標 X 向
            double dx = this.X;
            //半徑
            double r = 0;// Dia / 2;

            if (dXs.Count() == 1 && dXs[0] == 0)
            {
                x = this.X;
            }
            else
            {
                //    [r]|[r]60 70 70 60[r]|[(dx)]
                //    dXs.Sum() = 60 + 70 + 70 + 60
                //    (dXs.Count() - 1) = 中間三個孔徑
                //    r * 2 = 中間三個直徑
                x = dx + r + dXs.Sum() + (dXs.Count() - 1) * (r * 2) + r;
            }
            double rightXStart = SteelLength - x;
            return rightXStart;
        }
    }
}
