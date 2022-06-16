using GD_STD.Base;
using GD_STD.Enum;
using GD_STD.IBase;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 給予加工軸的加工資訊
    /// <para>可序列化的結構</para>
    /// </summary>
    /// <remarks>備註 : Codesys Memory 讀取/寫入</remarks>
    [Serializable()]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Drill : IAxis2D, IPainted
    {
        /// <summary>
        /// 給予加工軸的座標與鑽孔直徑資訊
        /// </summary>
        /// <param name="x">鑽孔 X 絕對座標</param>
        /// <param name="y">鑽孔 Y 絕對座標</param>
        /// <param name="dia">直徑</param>
        /// <param name="noMatch">匹配狀態</param>
        /// <param name="ox">圓心座標 X 絕對座標</param>
        /// <param name="oy">圓心座標 Y 絕對座標</param>
        /// <param name="through">貫穿(只限中軸)</param>
        /// <param name="AXIS_MODE">鑽頭的工作模式</param>
        public Drill(AXIS_MODE AXIS_MODE, double x = 0, double y = 0, double dia = 0, bool noMatch = false, double ox = 0, double oy = 0, bool through = false)
        {
            this.Dia = dia;
            this.Finish = false;
            this.X = x;
            this.Y = y;
            this.P1 = new Axis2D();
            this.P2 = new Axis2D();
            this.P3 = new Axis2D();
            this.AXIS_MODE = AXIS_MODE;
            this.NoMatch = noMatch;
            this.Ox = ox;
            this.Oy = oy;
            this.Through = through;
            this.G_Code = 0;
        }
        /// <summary>
        ///  給予加工軸的座標與鑽孔直徑資訊
        /// </summary>
        /// <param name="axis2D">中心二維座標</param>
        /// <param name="dia">直徑</param>
        /// <param name="noMatch">匹配狀態</param>
        /// <param name="through">貫穿(只限中軸)</param>
        /// <param name="AXIS_MODE">鑽頭的工作模式</param>
        public Drill(IAxis2D axis2D, AXIS_MODE AXIS_MODE, double dia = 0, bool noMatch = false, bool through = false)
        {
            this = new Drill(AXIS_MODE, axis2D.X, axis2D.Y, dia)
            {
                NoMatch = noMatch,
                Through = through
            };
        }
        /// <summary>
        /// 鑽孔 X
        /// </summary>
        [DataMember]
        public double X { get; set; }
        /// <summary>
        ///鑽孔 Y
        /// </summary>
        [DataMember]
        public double Y { get; set; }
        /// <summary>
        /// 中心點 X 絕對座標
        /// </summary>
        [DataMember]
        public double Ox { get; set; }
        /// <summary>
        /// 中心點 Y 絕對座標
        /// </summary>
        public double Oy { get; set; }
        /// <summary>
        /// 機械加工要執行直徑
        /// </summary>
        [DataMember]
        public double Dia { get; set; }
        /// <summary>
        /// 完成訊號
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Finish;
        /// <summary>
        /// 第一點畫線座標 or 刻字最小範圍
        /// </summary>
        [DataMember]
        public Axis2D P1 { get; set; }
        /// <summary>
        ///  第二點畫線座標 or 刻字最大範圍
        /// </summary>
        [DataMember]
        public Axis2D P2 { get; set; }
        /// <summary>
        /// 鑽頭的工作模式
        /// </summary>
        [DataMember]
        public AXIS_MODE AXIS_MODE { get; set; }
        /// <summary>
        /// 畫弧第三點座標
        /// </summary>
        [DataMember]
        public Axis2D P3 { get; set; }
        /// <summary>
        /// 鑽孔參數與目前刀庫不匹配
        /// </summary>
        /// <remarks>
        /// 如果不匹配回傳 true，匹配則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool NoMatch;
        /// <summary>
        /// 貫穿 (只限中軸)
        /// </summary>
        /// <remarks>
        /// 要貫穿物件回傳 true，不貫穿則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Through;
        /// <summary>
        /// 刻字 G-Code 內容
        /// </summary>
        [DataMember]
        public byte G_Code;
        /// <inheritdoc/>
        public void AddP1(IAxis2D axis2D)
        {
            P1 = ConvertToAxis(axis2D);
        }
        /// <inheritdoc/>
        public void AddP2(IAxis2D axis2D)
        {
            P2 = ConvertToAxis(axis2D);
        }
        /// <summary>
        /// 轉換類型
        /// </summary>
        /// <param name="axis2D"></param>
        /// <returns></returns>
        private Axis2D ConvertToAxis(IAxis2D axis2D)
        {
            Axis2D result = new Axis2D
            {
                X = axis2D.X,
                Y = axis2D.Y
            };
            return result;
        }
        /// <inheritdoc/>
        public void Reverse()
        {
            Axis2D p1 = P1, p2 = P2;
            AddP2(p1);
            AddP1(p2);
        }
    }
}