using DevExpress.DocumentView;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 零件內加工之總數
    /// </summary>
    public class DrillBolts
    {
        private struct LangStruct
        {
            public string ZH { get; set; }
            public string EN { get; set; }
            public string VN { get; set; }
            public string TH { get; set; }
        }

        private Dictionary<AXIS_MODE, LangStruct> Work_LanguageDict
        {
            get
            {
                var LangDict = new Dictionary<AXIS_MODE, LangStruct>();
                LangDict[AXIS_MODE.PIERCE] = new LangStruct
                {
                    ZH = "孔",
                    EN = "Hole",
                    VN = "Hố",
                    TH = "รู"
                };
                LangDict[AXIS_MODE.POINT] = new LangStruct
                {
                    ZH = "點",
                    EN = "POINT",
                    VN = "ĐIỂM",
                    TH = "จุด"
                };
                LangDict[AXIS_MODE.LINE] = new LangStruct
                {
                    ZH = "線",
                    EN = "Line",
                    VN = "Hàng",
                    TH = "เส้น"
                };
                LangDict[AXIS_MODE.Arc] = new LangStruct
                {
                    ZH = "弧",
                    EN = "Arc",
                    VN = "vòng cung",
                    TH = "อาร์ค"
                };
                LangDict[AXIS_MODE.Round] = new LangStruct
                {
                    ZH = "圓",
                    EN = "Round",
                    VN = "Chung quanh",
                    TH = "กลม"
                };
                LangDict[AXIS_MODE.String] = new LangStruct
                {
                    ZH = "刻",
                    EN = "String",
                    VN = "sợi dây",
                    TH = "สตริง"
                }; LangDict[AXIS_MODE.HypotenusePOINT] = new LangStruct
                {
                    ZH = "斜邊打點",
                    EN = "HypotenusePOINT",
                    VN = "điểm huyền",
                    TH = "จุดด้านตรงข้ามมุมฉาก"
                }; 

                return LangDict;
            }
        }
        public string WorkType
        {
            get
            {
                var WorkTypeName = "unknown";
                var pageL = Work_LanguageDict[WorkAXIS_MODE];
                switch (WPFSTD105.Properties.SofSetting.Default.Language)
                {
                    case 0:
                        WorkTypeName = pageL.ZH;
                        break;
                    case 1:
                        WorkTypeName = pageL.EN;
                        break;
                    case 2:
                        WorkTypeName = pageL.VN;
                        break;
                    case 3:
                        WorkTypeName = pageL.TH;
                        break;
                    default:
                        break;
                }

                return WorkTypeName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public AXIS_MODE WorkAXIS_MODE { get; set; } 
        /// <summary>
        /// 面的方向
        /// </summary>
        public GD_STD.Enum.FACE Face { get; set; }
        /// <summary>
        /// 孔位數
        /// </summary>
        public int DrillHoleCount { get; set; }
        /// <summary>
        /// 孔直徑
        /// </summary>
        public double DrillHoleDiameter { get; set; }
    }
}
