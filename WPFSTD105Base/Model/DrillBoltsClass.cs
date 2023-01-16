using DevExpress.DocumentView;
using GD_STD.Base;
using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 序列化專用
    /// </summary>
    [Serializable]
    public class DrillBoltsModel
    {
        public Dictionary<FACE, DrillBoltsBase> DrillBoltsDict { get; set; } = new Dictionary<FACE, DrillBoltsBase>();
    }

    [Serializable]
    public class DrillBoltsBase : WPFWindowsBase.BaseViewModel
    {
        private bool _dia_Identification = false;
        /// <summary>
        /// 當true時加工孔統一刀徑
        /// </summary>
        public bool Dia_Identification
        {
            get
            {
                return _dia_Identification;
            }
            set
            {
                _dia_Identification = value;
                DrillBoltList.ForEach(x => x.DrillHoleDiameterIsChangeBool = value);
            }
        }


        private bool _pinTestMode = false;
        /// <summary>
        /// 為true時開啟打點測試模式，會將所有加工孔轉化為點，並關閉加工點以外的所有加工方式
        /// </summary>
        public bool PinTestMode
        {
            get
            {
                return _pinTestMode;
            }
            set
            {
                _pinTestMode = value;
            }
        }

        /// <summary>
        /// 加工資訊
        /// </summary>
        public List<DrillBolt> DrillBoltList { get; set; } = new List<DrillBolt>();

        private double _unitaryToolTop = -1;
        /// <summary>
        /// 選擇刀具
        /// </summary>
        public double UnitaryToolTop
        {
            get
            {
                return _unitaryToolTop;
            }
            set
            {
                _unitaryToolTop = value;
                DrillBoltList.ForEach(x =>
                {
                    x.Changed_DrillHoleDiameter = _unitaryToolTop;
                });
            }
        }
    }

    /// <summary>
    /// 加工資訊
    /// </summary>
    [Serializable]
    public class DrillBolt : WPFWindowsBase.BaseViewModel
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
                var pageL = Work_LanguageDict[Actual_WorkAXIS_MODE];
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
        /// 素材編號(查詢用)
        /// </summary>
        //public string MaterialNumber { get; set; }
        /// <summary>
        /// 為true時才進行加工
        /// </summary>
        public bool DrillWork { get; set; }

        /// <summary>
        /// 工作模式(原始)
        /// </summary>
        public AXIS_MODE Origin_WorkAXIS_MODE { get; set; }


        private AXIS_MODE? _changed_WorkAXIS_MODE = null;
        /// <summary>
        /// 工作模式(變化)
        /// </summary>
        public AXIS_MODE Changed_WorkAXIS_MODE
        {
            get
            {
                if (_changed_WorkAXIS_MODE == null)
                    _changed_WorkAXIS_MODE = Origin_WorkAXIS_MODE;
                return _changed_WorkAXIS_MODE.Value;
            }
            set
            {
                _changed_WorkAXIS_MODE = value;
            }
        }
        /// <summary>
        /// 實際加工模式(依據變化後而定)
        /// </summary>
        public AXIS_MODE Actual_WorkAXIS_MODE
        {
            get
            {
                if (WorkAXIS_modeIsChanged)
                {
                    return Changed_WorkAXIS_MODE;
                }
                else
                {
                    return Origin_WorkAXIS_MODE;
                }
            }
        }

        private bool _workAXIS_modeIsChanged = false;
        /// <summary>
        /// 加工模式被變更被變更 -> 紀錄變更後的值來比較
        /// 可切換true false來得到值
        /// </summary>
        public bool WorkAXIS_modeIsChanged
        {
            get
            {
                return (Origin_WorkAXIS_MODE != Changed_WorkAXIS_MODE) ? _workAXIS_modeIsChanged : false;
            }
            set
            {
                _workAXIS_modeIsChanged = value;
                OnPropertyChanged(nameof(WorkAXIS_modeIsChanged));
            }
        }


        /// <summary>
        /// 面的方向
        /// </summary>
        //public GD_STD.Enum.FACE Face { get; set; }
        /// <summary>
        /// 孔位數
        /// </summary>
        public int DrillHoleCount { get; set; }
        /// <summary>
        /// 孔直徑(原始)
        /// </summary>
        public double Origin_DrillHoleDiameter { get; set; }


        private double? _changed_DrillHoleDiameter;
        /// <summary>
        /// 孔直徑(變更後)
        /// </summary>
        public double Changed_DrillHoleDiameter
        {
            get
            {
                if (_changed_DrillHoleDiameter == null)
                    _changed_DrillHoleDiameter = Origin_DrillHoleDiameter;
                return _changed_DrillHoleDiameter.Value;
            }
            set
            {
                _changed_DrillHoleDiameter = value;
            }
        }

        /// <summary>
        /// 孔直徑(實際加工值)
        /// </summary>
        public double DrillHoleDiameter
        {
            get
            {
                if (!DrillHoleDiameterIsChangeBool)
                {
                    return Origin_DrillHoleDiameter;
                }
                else
                {
                    return Changed_DrillHoleDiameter;
                }
            }
        }

        private bool _drillHoleDiameterIsChangeBool = false;
        /// <summary>
        /// 孔直徑被變更 -> 紀錄變更後的值來比較
        /// 可切換true false來得到值
        /// </summary>
        public bool DrillHoleDiameterIsChangeBool
        {
            get
            {
                return (Origin_DrillHoleDiameter != Changed_DrillHoleDiameter) ? _drillHoleDiameterIsChangeBool : false;
            }
            set
            {
                _drillHoleDiameterIsChangeBool = value;
                OnPropertyChanged("DrillHoleDiameter");
            }
        }
    }

}
