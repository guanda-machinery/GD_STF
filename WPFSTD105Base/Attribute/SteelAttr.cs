using devDept.Geometry;
using DevExpress.Utils.Extensions;
using GD_STD.Data;
using GD_STD.Enum;
using GD_STD.IBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WPFSTD105.Model;
using WPFSTD105.Surrogate;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 主要鋼構物件自定義資訊
    /// </summary>
    [Serializable]
    public class SteelAttr: AbsAttr, ISteelAttr, ISteelProfile, INotifyPropertyChanged
    {        
        /// <summary>
        /// 轉換器
        /// </summary>
        /// <param name="steelPart"></param>
        public SteelAttr(SteelPart steelPart)
        {
            WriteProfile(steelPart);
            Length = steelPart.Length;
        }
        /// <summary>
        /// 標準建構式
        /// </summary>
        public SteelAttr()
        {
        }
        /// <summary>
        /// 寫入 <see cref="SteelPart"/> 
        /// </summary>
        /// <param name="profile"></param>
        public void WriteProfile(ISteelProfile profile)
        {
            H = profile.H;
            W = profile.W;
            t1 = profile.t1;
            t2 = profile.t2;
            Profile = profile.Profile.Replace("*", "X").Replace(" ", "");
            Type = profile.Type;
            Material = profile.Material;
            //GUID = Guid.NewGuid();
        }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime Creation { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? Revise { get; set; }
        /// <summary>
        /// Phase
        /// </summary>
        public int? Phase { get; set; }
        /// <summary>
        /// 拆運 車次
        /// </summary>
        public int? ShippingNumber { get; set; }
        /// <summary>
        /// 標題1
        /// </summary>
        public string Title1 { get; set; }
        /// <summary>
        /// 標題2
        /// </summary>
        public string Title2 { get; set; }
        /// <summary>
        /// 上鎖
        /// </summary>
        public bool Lock { get; set; }
        /// <summary>
        /// 驚嘆號
        /// </summary>
        public bool? ExclamationMark { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        //20220728 張燕華 轉換出inp檔案->新增斷面規格屬性
        /// <summary>
        /// 圓角半徑r1(mm)
        /// </summary>
        public float r1 { get; set; }
        /// <summary>
        /// 圓角半徑r2(mm)
        /// </summary>
        public float r2 { get; set; }
        /// <summary>
        /// 表面積(m2/m)
        /// </summary>
        public float surface_area { get; set; }
        /// <summary>
        /// 斷面積(m2)
        /// </summary>
        public float section_area { get; set; }
        /// <summary>
        /// 密度(kg/m3)
        /// </summary>
        public float density { get; set; }
        /// <summary>
        /// 突出肢長度e(mm)
        /// </summary>
        public float e { get; set; }
        /// <summary>
        /// 直徑d(mm)
        /// </summary>
        public float diameter { get; set; }

        /// <inheritdoc/>
        public double Length { get; set; } = 5000;
        /// <summary>
        /// 製品重
        /// </summary>
        public double Weight { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public float W { get; set; }
        /// <inheritdoc/>
        public float t1 { get; set; }
        /// <inheritdoc/>
        public float t2 { get; set; }
        /// <inheritdoc/>
        public float Kg { get; set; }
        /// <inheritdoc/>
        public string Material { get; set; }
        /// <summary>
        /// 製品長度
        /// </summary>
        public double pieceLength{ get; set; }
        /// <inheritdoc/>
        public string PartNumber { get; set; }
        /// <inheritdoc/>
        public string AsseNumber { get; set; }
        /// <inheritdoc/>
        public int Number { get; set; } = 1;
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <inheritdoc/>
        public CutList PointTop { get; set; } = new CutList().ConvertToSurrogate();
        /// <inheritdoc/>
        public CutList PointFront { get; set; } = new CutList().ConvertToSurrogate();
        /// <inheritdoc/>
        public CutList PointBack { get; set; } = new CutList().ConvertToSurrogate();
        /// <inheritdoc/>
        public CutContour Top { get => GetCutPoint(PointTop, H); }
        /// <inheritdoc/>
        public CutContour Front { get => GetCutPoint(PointFront, W); }
        /// <inheritdoc/>
        public CutContour Back { get => GetCutPoint(PointBack, W); }
        /// <inheritdoc/>
        public bool IsMainPart { get => MainPartNumber == PartNumber; }


        /// <summary>
        /// NC 頂面形狀
        /// </summary>
        public List<NcPoint3D> oPoint { get; set; } = new List<NcPoint3D>();
        /// <summary>
        /// NC 前面形狀
        /// </summary>
        public List<NcPoint3D> vPoint { get; set; } = new List<NcPoint3D>();
        /// <summary>
        /// NC 底面形狀
        /// </summary>
        public List<NcPoint3D> uPoint { get; set; } = new List<NcPoint3D>();
        /// <inheritdoc/>
        [Html("PartID")]
        public string TeklaPartID { get; set; }
        /// <inheritdoc/>
        [Html("AssemblyID")]
        public string TeklaAssemblyID { get; set; }
        /// <inheritdoc/>
        [Html("MainPartNumber")]
        public string MainPartNumber { get; set; }
        /// <summary>
        /// 切割列表
        /// </summary>
        public List<Point3D> CutList { get; set; }
        /// <summary>
        /// 旋轉面
        /// </summary>
        public FACE Face { get; set; }
        /// <summary>
        /// 前段角度
        /// </summary>
        public double StartAngle { get; set; }
        /// <summary>
        /// 後段角度
        /// </summary>
        public double EndAngle { get; set; }
        ///// <inheritdoc/>
        //public OBJECT_TYPE ProfileType { get; set; }
        /// <summary>
        /// 預防破圖
        /// </summary>
        const double d = 10;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// 取得切割線的三個點位
        /// </summary>
        /// <param name="cutList">切割列表</param>
        /// <param name="value">寬度或高度</param>
        private CutContour GetCutPoint(CutList cutList, double value)
        {
            if (cutList == null)
            {
                return null;
            }
            CutContour result = null;


            if (cutList.DL?.X > 0 && cutList.DL?.Y > 0) //左邊下緣切割線
            {
                if (result == null)
                    result = new CutContour();

                result.DL.AddRange(new List<Point3D>()
                            {
                                new Point3D(0,0),
                                new Point3D(cutList.DL.X , 0),
                                new Point3D(0, cutList.DL.Y),
                            });
            }

            if (cutList.UL?.X > 0 && cutList.UL?.Y > 0)//左邊上緣切割線
            {
                if (result == null)
                    result = new CutContour();

                result.UL.AddRange(new List<Point3D>()
                            {
                                new Point3D(0,value - cutList.UL.Y),
                                new Point3D(0, value),
                                new Point3D(cutList.UL.X, value),
                            });
            }
            if (cutList.DR?.X > 0 && cutList.DR?.Y > 0)//右邊下緣切割線
            {
                if (result == null)
                    result = new CutContour();

                result.DR.AddRange(new List<Point3D>()
                            {
                                new Point3D(Length - cutList.DR.X,0),
                                new Point3D(Length, 0),
                                new Point3D(Length, cutList.DR.Y),
                            });
            }
            if (cutList.UR?.X > 0 && cutList.UR?.Y > 0)//右邊上緣切割線
            {
                if (result == null)
                    result = new CutContour();

                result.UR.AddRange(new List<Point3D>()
                            {
                                new Point3D(Length - cutList.UR.X, value),
                                new Point3D(Length, value),
                                new Point3D(Length, value - cutList.UR.Y),
                            });
            }
            return result;
        }
        
        /// <summary>
        /// <see cref="SteelAttr"/> 轉換 <see cref="SteelAttrSurrogate"/>
        /// </summary>
        /// <returns></returns>
        public virtual SteelAttrSurrogate ConvertToSurrogate()
        {
            return new SteelAttrSurrogate(this);
        }
    }
}
