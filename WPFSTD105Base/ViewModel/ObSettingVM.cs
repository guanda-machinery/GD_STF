#define Debug
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using GD_STD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using static devDept.Eyeshot.Environment;
//using EntityList = WPFSTD105.EntityList;
using WPFBase = WPFWindowsBase;
using WPFSTD105.Properties;
using System.Windows;
using static WPFSTD105.ViewLocator;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using WPFWindowsBase;
using GD_STD.Enum;
using System.IO;
using DevExpress.Xpf.CodeView;
using GD_STD.Data;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using devDept.Eyeshot.Translators;
using WPFSTD105.Tekla;

namespace WPFSTD105.ViewModel
{    
    /// <summary>
    /// 物件設定
    /// </summary>
    public class ObSettingVM : WPFBase.BaseViewModel
    {
        #region 命令
        /// <summary>
        /// 平移
        /// </summary>
        /// <returns></returns>
        public ICommand Pan { get; set; }
        /// <summary>
        /// 旋轉
        /// </summary>
        public ICommand Rotate { get; set; }
        /// <summary>
        /// 放大到框選舉型視窗
        /// </summary>
        public ICommand ZoomWindow { get; set; }
        /// <summary>
        /// 放大縮小 
        /// </summary>
        public ICommand Zoom { get; set; }
        /// <summary>
        /// 編輯物件
        /// </summary>
        public ICommand EditObject { get; set; }
        /// <summary>
        /// 刪除物件
        /// </summary>
        public ICommand Delete { get; set; }
        /// <summary>
        /// 取消所有功能
        /// </summary>
        public ICommand Esc { get; set; }
        /// <summary>
        /// 恢復上一個動作
        /// </summary>
        public ICommand Recovery { get; set; }
        /// <summary>
        /// 恢復下一個動作
        /// </summary>
        public ICommand Next { get; set; }
        /// <summary>
        /// 加入零件
        /// </summary>
        public ICommand AddPart { get; set; }
        /// <summary>
        /// 修改零件
        /// </summary>
        public ICommand ModifyPart { get; set; }
        /// <summary>
        /// 讀取主件訊息
        /// </summary>
        public ICommand ReadPart { get; set; }
        /// <summary>
        /// 另存加入零件 20220902 張燕華
        /// </summary>
        public ICommand AddNewOne { get; set; }
        /// <summary>
        /// 檔案總覽 20221005 呂宗霖
        /// </summary>
        public ICommand FileOverView { get; set; }
        /// <summary>
        /// OK鈕 20220902 張燕華
        /// </summary>
        public ICommand OKtoConfirmChanges { get; set; }
        /// <summary>
        /// 刪除孔位
        /// </summary>
        public ICommand DeleteHole { get; set; }
        /// <summary>
        /// 加入孔位
        /// </summary>
        public ICommand AddHole { get; set; }
        /// <summary>
        /// 修改孔位
        /// </summary>
        public ICommand ModifyHole { get; set; }
        /// <summary>
        /// 讀取孔位訊息
        /// </summary>
        public ICommand ReadHole { get; set; }
        /// <summary>
        /// 加入切割線
        /// </summary>
        public ICommand AddCut { get; set; }
        /// <summary>
        /// 修改切割線
        /// </summary>
        public ICommand ModifyCut { get; set; }
        /// <summary>
        /// 清除標註
        /// </summary>
        public ICommand ClearDim { get; set; }
        /// <summary>
        /// 刪除切割線
        /// </summary>
        public ICommand DeleteCut { get; set; }
        /// <summary>
        /// 讀取切割線訊息
        /// </summary>
        public ICommand ReadCut { get; set; }
        /// <summary>
        /// 鏡射 X 軸
        /// </summary>
        public ICommand MirrorX { get; set; }
        /// <summary>
        /// 軸鏡射 Y 軸
        /// </summary>
        public ICommand MirrorY { get; set; }
        /// <summary>
        /// 顯示3D圖形
        /// </summary>
        public ICommand Display3DViewerCommand { get; set; }
        /// <summary>
        /// 顯示已建立清單
        /// </summary>
        public ICommand DisplayCreatedListCommand { get; set; }
        /// <summary>
        /// 顯示零件清單
        /// </summary>
        public ICommand DisplayPartsCommand { get; set; }
        /// <summary>
        /// 顯示孔位編輯器
        /// </summary>
        public ICommand DisplayHoleCommand { get; set; }
        /// <summary>
        /// 20220829 張燕華 選擇型鋼型態
        /// </summary>
        public ICommand ShowSteelTypeCommand { get; set; }
        /// <summary>
        /// 20220907 張燕華 選擇型鋼斷面規格
        /// </summary>
        public ICommand ShowSteelSectionCommand { get; set; }
        /// <summary>
        /// 20220907 張燕華 手動選擇型鋼斷面規格
        /// </summary>
        public ICommand ShowSteelTypeSectionManualCommand { get; set; }
        /// <summary>
        /// 20220913 張燕華 計算製品重量
        /// </summary>
        public ICommand CalculateWeightCommand { get; set; }
        /// <summary>
        /// 20220906 張燕華 鑽孔rbtn測試
        /// </summary>
        public ICommand CmdShowMessage { get; set; }

        /// <summary>
        /// 20220922 蘇冠綸 全選/全不選checkbox
        /// </summary>
        public ICommand SetAllCheckboxCheckedCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(obj =>
                {
                    GetWpfLogicalChildClass.SetAllCheckBoxTrueOrFalse(obj);
                });
            }
        }





        #endregion

        #region 公開屬性
        /// <summary>
        /// 限制Grid出現之內容
        /// </summary>
        public List<OBJECT_TYPE> allowType = new List<OBJECT_TYPE> {
            OBJECT_TYPE.RH, OBJECT_TYPE.BH};
        //, OBJECT_TYPE.H,
        //OBJECT_TYPE.BOX, OBJECT_TYPE.TUBE,
        //OBJECT_TYPE.LB, OBJECT_TYPE.CH

        /// <summary>
        /// 構件資訊列表
        /// </summary>
        public ObservableCollection<SteelAssembly> SteelAssemblies { get; private set; } = new ObservableCollection<SteelAssembly>();
        /// <summary>
        /// 零件字典檔
        /// </summary>
        public Dictionary<string, ObservableCollection<SteelPart>> DicSteelPart { get; set; }
        /// <summary>
        /// 變更直徑參數
        /// </summary>
        public bool CheckDia { get; set; } = true;
        /// <summary>
        /// 變更起始孔參數
        /// </summary>
        public bool CheckStartHole { get; set; } = true;
        /// <summary>
        /// 變更孔位水平向參數
        /// </summary>
        public bool CheckX { get; set; } = true;
        /// <summary>
        /// 變更孔位垂直向參數
        /// </summary>
        public bool CheckY { get; set; } = true;
        /// <summary>
        /// 鑽孔radio button 頂面,前面,後面
        /// </summary>
        public int rbtn_DrillingFace { get; set; } = 0;
        /// <summary>
        /// 切割線radio button 頂面,前面,後面
        /// </summary>
        public int rbtn_CutFace { get; set; }
        /// <summary>
        /// 選擇物件的面的功能開啟
        /// </summary>
        public bool CheckFace { get; set; } = true;
        /// <summary>
        /// 顯示3D視圖
        /// </summary>
        public bool ThreeDimensionalDisplayControl { get; set; } = true;
        /// <summary>
        /// 已建立清單顯示控制
        /// </summary>
        public bool DisplayCreatedListControl { get; set; }
        /// <summary>
        /// 零件清單顯示控制
        /// </summary>
        public bool DisplayPartsControl { get; set; }
        /// <summary>
        /// 孔位編輯器顯示控制
        /// </summary>
        public bool DisplayHoleControl { get; set; }
        /// <summary>
        /// 儲存的序列化檔案資料
        /// </summary>
        public ObservableCollection<DataCorrespond> DataCorrespond { get; set; } = new ObservableCollection<DataCorrespond>();
        /// <summary>
        /// 導回原本的ViewModel
        /// </summary>
        public ProductSettingsPageViewModel ProductSettingsPageViewModel = new ProductSettingsPageViewModel();

        public string PartNumber { get; set; }
        /// <summary>
        /// 樹狀邏輯
        /// </summary>
        public ObservableCollection<TreeNode> TreeNode { get; set; } = new ObservableCollection<TreeNode>();
        /// <summary>
        /// 孔位起始點 X 座標
        /// </summary>
        public double StartY { get; set; } = 0;
        /// <summary>
        /// 鑽孔模式
        /// </summary>
        /// <remarks>
        /// <see cref="GD_STD.Enum.AXIS_MODE"/>
        /// </remarks>
        public int AxisModeType
        {
            get => _AxisModeType;
            set
            {
                _AxisModeType = value;
                GroupBoltsAttr.Mode = (GD_STD.Enum.AXIS_MODE)value;
            }
        }
        /// <summary>
        /// 起始孔類型
        /// </summary>
        public int StartHoleType { get; set; } = 0;
        /// <summary>
        /// 加入螺栓選擇的面
        /// </summary>
        public int BoltsFaceType
        {
            get => _BoltsFaceType;
            set
            {
                _BoltsFaceType = value;
                GroupBoltsAttr.Face = (GD_STD.Enum.FACE)value;
            }
        }
        /// <summary>
        /// 主要零件設定檔
        /// </summary>
        public SteelAttr SteelAttr { get; set; } = new SteelAttr();
        /// <summary>
        /// 鋼構名稱
        /// </summary>
        public String TypeDesc { get; set; }
        /// <summary>
        /// 螺栓群組設定檔
        /// </summary>
        public GroupBoltsAttr GroupBoltsAttr { get; set; } = new GroupBoltsAttr();
        /// <summary>
        /// 圖層列表
        /// </summary>
        public LayerList LayerList { get; set; } = new LayerList();
        /// <summary>
        /// 取得或設置坐標系圖標。
        /// </summary>
        public CoordinateSystemIcon CoordinateSystemIcon { get; set; }
        /// <summary>
        /// 取得或設置原點符號列表。
        /// </summary>
        public ObservableCollection<OriginSymbol> OriginSymbols { get; set; }
        /// <summary>
        /// 獲取或設置活動視口動作。
        /// </summary>
        public actionType ActionMode { get; set; }
        /// <summary>
        /// 取得原點符號
        /// </summary>
        public OriginSymbol OriginSymbol { get => OriginSymbols[0]; }
        /// <summary>
        /// 用戶設定檔案
        /// </summary>
        public ModelAttr Setting { get; set; } = new ModelAttr();
        /// <summary>
        /// 以選擇的參考圖塊列表
        /// </summary>
        /// <remarks>
        /// 此處是暫存區，使用完畢請釋放
        /// </remarks>
        public List<SelectedItem> Select3DItem { get; set; } = new List<SelectedItem>();
        /// <summary>
        /// 被選中的圖塊內物件
        /// </summary>
        /// <remarks>
        /// 此處是暫存區，使用完畢請釋放
        /// </remarks>
        public List<Entity> tem3DRecycle { get; set; } = new List<Entity>();
        /// <summary>
        /// 以選擇的參考圖塊列表
        /// </summary>
        /// <remarks>
        /// 此處是暫存區，使用完畢請釋放
        /// </remarks>
        public List<SelectedItem> Select2DItem { get; set; } = new List<SelectedItem>();
        /// <summary>
        /// 被選中的圖塊內物件
        /// </summary>
        /// <remarks>
        /// 此處是暫存區，使用完畢請釋放
        /// </remarks>
        public List<Entity> tem2DRecycle { get; set; } = new List<Entity>();
        /// <summary>
        /// 還原動作列表
        /// </summary>
        public ReductionList Reductions { get; set; } = null;
        /// <summary>
        ///  選擇的斷面規格類型 <see cref="OBJECT_TYPE"/>索引值
        /// </summary>
        public int ProfileType
        {
            get => _ProfileType;
            set
            {
                try
                {
                    _ProfileType = value;
                    List<SteelAttr> list = new List<SteelAttr>();
                    OBJECT_TYPE TYPE = (OBJECT_TYPE)value;

                    if (!File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{TYPE}.inp"))
                    {
                        File.Copy($@"Profile\{TYPE}.inp", $@"{ApplicationVM.DirectoryPorfile()}\{TYPE}.inp");//複製 BH 斷面規格到模型內
                    }
#if DEBUG
                    log4net.LogManager.GetLogger("載入斷面規格").Debug(TYPE.ToString());
#endif
                    switch (TYPE)
                    {
                        case OBJECT_TYPE.TUBE://20220802 張燕華 新增斷面規格
                        case OBJECT_TYPE.H: //20220802 張燕華 新增斷面規格
                        case OBJECT_TYPE.LB: //20220802 張燕華 新增斷面規格
                        case OBJECT_TYPE.RH:
                        case OBJECT_TYPE.CH:
                        //case OBJECT_TYPE.L: //20220805 張燕華 新增斷面規格 - 已不在介面上顯示此規格
                        case OBJECT_TYPE.BOX:
                        case OBJECT_TYPE.BH:
                            ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(TYPE).ToString()}.inp");
                            break;
                        default:
                            throw new Exception($"找不到{TYPE.ToString()}");
                    }
#if DEBUG
                    log4net.LogManager.GetLogger("載入斷面規格").Debug("完成");
#endif
                    ProfileIndex = 0;
                }
                catch (Exception e)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                    throw;
                }
            }
        }
        /// <summary>
        /// 構件編號
        /// </summary>
        public string AssemblyNumberProperty { get; set; } = "";
        /// <summary>
        /// 零件編號
        /// </summary>
        public string PartNumberProperty { get; set; } = "";
        /// <summary>
        /// 製品長度
        /// </summary>
        public double ProductLengthProperty { get; set; }
        /// <summary>
        /// 製品重
        /// </summary>
        public double ProductWeightProperty { get; set; }
        /// <summary>
        /// 製品數量
        /// </summary>
        public double ProductCountProperty { get; set; }
        /// <summary>
        /// 製品名稱
        /// </summary>
        public string ProductNameProperty { get; set; } = "";
        /// <summary>
        /// 製品材質
        /// </summary>
        public string ProductMaterialProperty { get; set; } = "";
        /// <summary>
        /// Phase
        /// </summary>
        public int? PhaseProperty { get; set; }
        /// <summary>
        /// 拆運
        /// </summary>
        public int? ShippingNumberProperty { get; set; }
        /// <summary>
        /// 標題一
        /// </summary>
        public string Title1Property { get; set; }
        /// <summary>
        /// 標題二
        /// </summary>
        public string Title2Property { get; set; }
        /// <summary>
        /// 型鋼類型
        /// </summary>
        public int SteelTypeProperty_int { get; set; }
        /// <summary>
        /// 型鋼類型
        /// </summary>
        public OBJECT_TYPE SteelTypeProperty_enum { get; set; }
        /// <summary>
        /// 當前斷面規格
        /// </summary>
        public string SteelSectionProperty { get; set; } = "";
        /// <summary>
        /// 高
        /// </summary>
        public int HProperty { get; set; }
        /// <summary>
        /// 寬
        /// </summary>
        public int WProperty { get; set; }
        /// <summary>
        /// 腹板厚度
        /// </summary>
        public int t1Property { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        public int t2Property { get; set; }














        /// <summary>
        /// 標示資料來源的flag：true表示由零件清單, false表示由手動選擇
        /// </summary>
        public bool fPartListOrManuall { get; set; } = false;
        /// <summary>
        /// 紀錄前端當前所選的零件資訊
        /// </summary>
        public SteelAttr CurrentPartSteelAttr { get; set; } = new SteelAttr();
        /// <summary>
        /// 斷面規格列表
        /// </summary>
        public ObservableCollection<SteelAttr> ProfileList { get; set; }
        /// <summary>
        /// 選擇的斷面規格<see cref="ProfileList"/>索引值
        /// </summary>
        public int ProfileIndex
        {
            get => _ProfileIndex;
            set
            {
                _ProfileIndex = value;

                SteelAttr _steelAttr;
                if (value == -1)
                    _steelAttr = ProfileList[0];
                else
                    _steelAttr = ProfileList[value];

                SteelAttr = (SteelAttr)_steelAttr.DeepClone();
            }
        }

        /// <summary>
        /// 材質列表
        /// </summary>
        public ObservableCollection<SteelMaterial> Materials { get; set; } = new ObservableCollection<SteelMaterial>();
        /// <summary>
        /// 材質類型索引值
        /// </summary>
        public int MaterialIndex
        {
            get => _MaterialIndex;
            set
            {
                _MaterialIndex = value;
                if (value == -1)
                {
                    SteelAttr.Material = "";
                }
                else
                {
                    SteelAttr.Material = Materials[value].Name;
                }
#if DEBUG
                log4net.LogManager.GetLogger("變換材質").Debug(value.ToString());
#endif
            }
        }


        /// <summary>
        /// 切割面
        /// </summary>
        public int CutFaceType { get; set; } = 0;
        /// <summary>
        /// 左邊上緣切割點
        /// </summary>
        public CutPoint ULPoint { get; set; } = new CutPoint();
        /// <summary>
        /// 左邊下緣切割點
        /// </summary>
        public CutPoint DLPoint { get; set; } = new CutPoint();
        /// <summary>
        /// 右邊上緣切割點
        /// </summary>
        public CutPoint URPoint { get; set; } = new CutPoint();
        /// <summary>
        /// 右邊下緣切割點
        /// </summary>
        public CutPoint DRPoint { get; set; } = new CutPoint();
        /// <summary>
        /// 左邊上緣切割點產生
        /// </summary>
        public bool ULCheck { get; set; } = true;
        /// <summary>
        /// 左邊下緣切割點產生
        /// </summary>
        public bool DLCheck { get; set; } = true;
        /// <summary>
        /// 右邊上緣切割點產生
        /// </summary>
        public bool URCheck { get; set; } = true;
        /// <summary>
        /// 右邊下緣切割點產生
        /// </summary>
        public bool DRCheck { get; set; } = true;
        #endregion
        #region 公開方法
        /// <summary>
        /// 存取對應檔案資料
        /// </summary>
        public void SaveDataCorrespond()
        {
            DataCorrespond data = new DataCorrespond()
            {
                DataName = SteelAttr.GUID.ToString(),
                Number = SteelAttr.PartNumber,
                Type = SteelAttr.Type,
                Profile = SteelAttr.Profile,
                // 2022/09/08 彥谷
                oPoint = SteelAttr.oPoint.ToArray(),
                vPoint = SteelAttr.vPoint.ToArray(),
                uPoint = SteelAttr.uPoint.ToArray(),
            };
            bool save = (from el in new List<DataCorrespond>(DataCorrespond) where el.DataName == data.DataName select el).ToList().Count == 0;
            if (save)
            {
                DataCorrespond.Add(data);
                //SerializationHelper.SerializeBinary(DataCorrespond, ApplicationVM.FilePartList());
                STDSerialization ser = new STDSerialization();
                ser.SetDataCorrespond(DataCorrespond);
                if (data.Type == OBJECT_TYPE.BH)
                {

                }
                //AddNode(data);
            }
        }
        /// <summary>
        /// 取得設定好的值
        /// </summary>
        /// <param name="att">以前設定過的</param>
        /// <returns></returns>
        public GroupBoltsAttr GetGroupBoltsAttr(GroupBoltsAttr att)
        {
            //對調回世界座標
            switch (att.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    att.Coordinates();
                    break;
                default:
                    break;
            }
            Boltsbuffer = att;

            GroupBoltsAttr result = GetGroupBoltsAttr();
            result.GUID = att.GUID;
            return result;
        }


        /// <summary>
        /// 切割線打點設定值
        /// </summary>
        public GroupBoltsAttr GetHypotenuseBoltsAttr(FACE Face, START_HOLE SHoleType)
        {
            Boltsbuffer.GUID = GroupBoltsAttr.GUID;
            //直徑設定
            if (CheckDia)
            {
                Boltsbuffer.Dia = GroupBoltsAttr.Dia;
                Boltsbuffer.Mode = (AXIS_MODE)AxisModeType;
            }
            //水平螺栓間距
            if (CheckX)
            {
                Boltsbuffer.dX = GroupBoltsAttr.dX;
                Boltsbuffer.xCount = GroupBoltsAttr.xCount;
            }
            //垂直螺栓
            if (CheckY)
            {
                Boltsbuffer.dY = GroupBoltsAttr.dY;
                Boltsbuffer.yCount = GroupBoltsAttr.yCount;
            }
            //要產生的面
            if (CheckFace)
            {
                Boltsbuffer.Face = (GD_STD.Enum.FACE)Face;
            }
            //double value = 0d;
            if (CheckStartHole)
            {
                //目前座標是2D座標只是需要先判斷 X Y 座標
                switch (Face)
                {
                    case FACE.TOP:
                        Boltsbuffer.t = Steelbuffer.t1; //孔位高度
                        //斷面規格類型
                        switch (Steelbuffer.Type)
                        {
                            case OBJECT_TYPE.BH:
                            case OBJECT_TYPE.RH:
                                Boltsbuffer.Z = Steelbuffer.W * 0.5 - Steelbuffer.t1 * 0.5;
                                break;
                            case OBJECT_TYPE.BOX:
                            case OBJECT_TYPE.CH:
                                Boltsbuffer.Z = Steelbuffer.W - Steelbuffer.t1;
                                break;
                            case OBJECT_TYPE.L:
                                Boltsbuffer.Z = 0;
                                break;
                            default:
                                break;
                        }
                        //value =
                        break;
                    case FACE.FRONT:
                        Boltsbuffer.t = Steelbuffer.t2;
                        Boltsbuffer.Z = Steelbuffer.t2;
                        //value = Steelbuffer.W;
                        break;
                    case FACE.BACK:
                        Boltsbuffer.t = Steelbuffer.t2;
                        Boltsbuffer.Z = Steelbuffer.H;
                        //value = Steelbuffer.W;
                        break;
                    default:
                        break;
                }
                //改變 Y 座標起始點類型
                switch (SHoleType)
                {
                    case START_HOLE.MIDDLE:
                        Boltsbuffer.StartHole = START_HOLE.MIDDLE;
                        //Boltsbuffer.Y = (GetBoltZ() * 0.5) - (Boltsbuffer.SumdY() * 0.5);
                        break;
                    case START_HOLE.START:
                        Boltsbuffer.StartHole = START_HOLE.START;
                        //Boltsbuffer.Y = this.StartY;
                        break;
                    default:
                        break;
                }
                Boltsbuffer.X = GroupBoltsAttr.X;
            }
            //判斷 Y 軸起始座標
            if (Boltsbuffer.StartHole == START_HOLE.MIDDLE)
            {
                Boltsbuffer.Y = (GetBoltZ() * 0.5) - (Boltsbuffer.SumdY() * 0.5);
            }
            else
            {
                Boltsbuffer.Y = this.StartY;
            }
#if DEBUG
            log4net.LogManager.GetLogger("螺栓設定檔").Debug
                ($"直徑 {Boltsbuffer.Dia} 鑽孔類型 {Boltsbuffer.Mode.ToString()}\n起始孔類型 {((START_HOLE)StartHoleType).ToString()} X {Boltsbuffer.X} Y {Boltsbuffer.Y}\nX數量 {Boltsbuffer.xCount} 間距 {Boltsbuffer.dX}\nY數量 {Boltsbuffer.yCount} 間距{Boltsbuffer.dY}\n方向{Boltsbuffer.Face.ToString()}");
#endif
            return (GroupBoltsAttr)Boltsbuffer.DeepClone();
        }







        /// <summary>
        /// 取得設定好的值
        /// </summary>
        public GroupBoltsAttr GetGroupBoltsAttr()
        {
            Boltsbuffer.BlockName = GroupBoltsAttr.BlockName;
            Boltsbuffer.GUID = GroupBoltsAttr.GUID;
            //直徑設定
            if (CheckDia == true)
            {
                Boltsbuffer.Dia = GroupBoltsAttr.Dia;
                Boltsbuffer.Mode = (AXIS_MODE)AxisModeType;
            }
            //水平螺栓間距
            if (CheckX)
            {
                Boltsbuffer.dX = GroupBoltsAttr.dX;
                Boltsbuffer.xCount = CalBoltNumber(GroupBoltsAttr.dX);
                //Boltsbuffer.xCount = GroupBoltsAttr.xCount;
            }
            else {
                Boltsbuffer.dX = "0";
                Boltsbuffer.xCount = CalBoltNumber(Boltsbuffer.dX);
            }
            //垂直螺栓
            if (CheckY)
            {
                Boltsbuffer.dY = GroupBoltsAttr.dY;
                Boltsbuffer.xCount = CalBoltNumber(GroupBoltsAttr.dY);
                //Boltsbuffer.yCount = GroupBoltsAttr.yCount;
            }
            else {
                Boltsbuffer.dY = "0";
                Boltsbuffer.yCount = CalBoltNumber(Boltsbuffer.dY);
            }
            //要產生的面
            if (CheckFace)
            {
                int temp_BoltsFace;
                if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings)//若為新版製品設定頁面
                {
                    temp_BoltsFace = rbtn_DrillingFace;
                }
                else
                {
                    temp_BoltsFace = BoltsFaceType;
                }

                Boltsbuffer.Face = (GD_STD.Enum.FACE)temp_BoltsFace;
            }
            //double value = 0d;
            if (CheckStartHole)
            {
                //目前座標是2D座標只是需要先判斷 X Y 座標
                switch (Boltsbuffer.Face)
                {
                    case FACE.TOP:
                        Boltsbuffer.t = Steelbuffer.t1; //孔位高度
                        //斷面規格類型
                        switch (Steelbuffer.Type)
                        {
                            case OBJECT_TYPE.BH:
                            case OBJECT_TYPE.RH:
                                Boltsbuffer.Z = Steelbuffer.W * 0.5 - Steelbuffer.t1 * 0.5;
                                break;
                            case OBJECT_TYPE.TUBE:
                            case OBJECT_TYPE.BOX:
                            case OBJECT_TYPE.CH:
                            case OBJECT_TYPE.H:
                                Boltsbuffer.Z = Steelbuffer.W - Steelbuffer.t1;
                                break;
                            case OBJECT_TYPE.L:
                                Boltsbuffer.Z = 0;
                                break;
                            default:
                                break;
                        }
                        //value =
                        break;
                    case FACE.FRONT:
                        Boltsbuffer.t = Steelbuffer.t2;
                        Boltsbuffer.Z = Steelbuffer.t2;
                        //value = Steelbuffer.W;
                        break;
                    case FACE.BACK:
                        Boltsbuffer.t = Steelbuffer.t2;
                        Boltsbuffer.Z = Steelbuffer.H;
                        //value = Steelbuffer.W;
                        break;
                    default:
                        break;
                }
                //改變 Y 座標起始點類型
                switch ((START_HOLE)StartHoleType)
                {
                    case START_HOLE.MIDDLE:
                        Boltsbuffer.StartHole = START_HOLE.MIDDLE;
                        //Boltsbuffer.Y = (GetBoltZ() * 0.5) - (Boltsbuffer.SumdY() * 0.5);
                        break;
                    case START_HOLE.START:
                        Boltsbuffer.StartHole = START_HOLE.START;
                        //Boltsbuffer.Y = this.StartY;
                        break;
                    default:
                        break;
                }
                Boltsbuffer.X = GroupBoltsAttr.X;
            }
            //判斷 Y 軸起始座標
            if (Boltsbuffer.StartHole == START_HOLE.MIDDLE)
            {
                Boltsbuffer.Y = (GetBoltZ() * 0.5) - (Boltsbuffer.SumdY() * 0.5);
            }
            else
            {
                Boltsbuffer.Y = this.StartY;
            }
#if DEBUG
            log4net.LogManager.GetLogger("螺栓設定檔").Debug
                ($"直徑 {Boltsbuffer.Dia} 鑽孔類型 {Boltsbuffer.Mode.ToString()}\n起始孔類型 {((START_HOLE)StartHoleType).ToString()} X {Boltsbuffer.X} Y {Boltsbuffer.Y}\nX數量 {Boltsbuffer.xCount} 間距 {Boltsbuffer.dX}\nY數量 {Boltsbuffer.yCount} 間距{Boltsbuffer.dY}\n方向{Boltsbuffer.Face.ToString()}");
#endif
            return (GroupBoltsAttr)Boltsbuffer.DeepClone();
        }
        /// <summary>
        /// 取得螺栓目前主件要計算中間的距離
        /// </summary>
        /// <returns></returns>
        public double GetBoltZ()
        {
            switch (Boltsbuffer.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    return Steelbuffer.H;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    return Steelbuffer.W;
                default:
                    throw new Exception("找不到類型");
            }
        }
        /// <summary>
        /// 寫入主件設定檔 To VM
        /// </summary>
        /// <param name="steelAttr"></param>
        public void WriteSteelAttr(SteelAttr steelAttr)
        {
            this.SteelAttr = steelAttr;
            this.Steelbuffer = (SteelAttr)steelAttr.Clone();
            for (int i = 0; i < ProfileList.Count; i++)
            {
                if (ProfileList[i].Profile == steelAttr.Profile)
                {
                    ProfileIndex = i;
                    break;
                }
            }
            MaterialIndex = Materials.IndexOf(el => el.Name == steelAttr.Material);
        }

#pragma warning disable CS1572 // XML 註解中的 'steelAttr' 有 param 標籤，但沒有該名稱的參數
        /// <summary>
        /// 寫入螺栓設定檔 To VM
        /// </summary>
        /// <param name="steelAttr"></param>
#pragma warning disable CS1573 // 參數 'boltsAttr' 在 'ObSettingVM.WriteGroupBoltsAttr(GroupBoltsAttr)' 的 XML 註解中沒有相符的 param 標籤 (但其他參數有)
        public void WriteGroupBoltsAttr(GroupBoltsAttr boltsAttr)
#pragma warning restore CS1573 // 參數 'boltsAttr' 在 'ObSettingVM.WriteGroupBoltsAttr(GroupBoltsAttr)' 的 XML 註解中沒有相符的 param 標籤 (但其他參數有)
#pragma warning restore CS1572 // XML 註解中的 'steelAttr' 有 param 標籤，但沒有該名稱的參數
        {
            this.GroupBoltsAttr = boltsAttr;
            this.Boltsbuffer = (GroupBoltsAttr)boltsAttr.Clone();
            this.BoltsFaceType = (int)boltsAttr.Face;
            this.StartHoleType = (int)boltsAttr.StartHole;
            this.StartY = boltsAttr.Y;
        }
        /// <summary>
        /// 寫入切割設定 To VM
        /// </summary>
        /// <param name="steelAttr">主件設定檔</param>
        public void WriteCutAttr(SteelAttr steelAttr)
        {
            int temp_CutFace;
            if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings)//若為新版製品設定頁面
            {
                temp_CutFace = rbtn_CutFace;
            }
            else
            {
                temp_CutFace = CutFaceType;
            }

            switch ((GD_STD.Enum.FACE)temp_CutFace)
            {
                case GD_STD.Enum.FACE.TOP:
                    WriteCutAttr(steelAttr.PointTop);
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    WriteCutAttr(steelAttr.PointFront);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 取得用戶選擇的主件設定檔
        /// </summary>
        /// <returns>因為 WPF VM的關係所以需要使用複製並存在緩衝區</returns>
        public SteelAttr GetSteelAttr()
        {

#if DEBUG
            log4net.LogManager.GetLogger("GetSteelAttr").Debug("");
#endif
            try
            {
                Steelbuffer = (SteelAttr)SteelAttr.DeepClone();
                if (Steelbuffer.Material == null)
                {
                    Steelbuffer.Material = Materials[_MaterialIndex].Name;
                }
                return (SteelAttr)Steelbuffer.DeepClone();
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// 存入切割輪廓到 <see cref="SteelAttr"/> 
        /// </summary>
        public void SaveCut()
        {
            try
            {
                int temp_CutFace;
                if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings)//若為新版製品設定頁面
                {
                    temp_CutFace = rbtn_CutFace;
                }
                else
                {
                    temp_CutFace = CutFaceType;
                }

                CutList bufferCutList = null;
                switch ((GD_STD.Enum.FACE)temp_CutFace)
                {
                    case GD_STD.Enum.FACE.BACK:
                    case GD_STD.Enum.FACE.FRONT:
                        //SteelAttr.PointFront = new CutList(URPoint, DRPoint, ULPoint, DLPoint);
                        bufferCutList = new CutList
                          (URCheck ? URPoint : Steelbuffer.PointFront.UR,
                           DRCheck ? DRPoint : Steelbuffer.PointFront.DR,
                           ULCheck ? ULPoint : SteelAttr.PointFront.UL,
                           DLCheck ? DLPoint : SteelAttr.PointFront.DL);
                        if (CutLogic(bufferCutList, SteelAttr.W))
                        {
                            SteelAttr.PointFront = bufferCutList;
                        }
                        break;
                    case GD_STD.Enum.FACE.TOP:

                        bufferCutList = new CutList
                            (URCheck ? URPoint : Steelbuffer.PointTop.UR,
                             DRCheck ? DRPoint : Steelbuffer.PointTop.DR,
                             ULCheck ? ULPoint : SteelAttr.PointTop.UL,
                             DLCheck ? DLPoint : SteelAttr.PointTop.DL);
                        if (CutLogic(bufferCutList, SteelAttr.H))
                        {
                            SteelAttr.PointTop = bufferCutList;
                        }
                        break;
                    default:
                        throw new Exception("找不到面");
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 計算單一支型鋼重量
        /// </summary>
        public double CalculateSinglePartWeight()
        {
            double weight;
            if (SteelAttr.Kg == 0)
            {
                weight = (ProductLengthProperty / 1000) * CurrentPartSteelAttr.Kg;
            }
            else
            {
                weight = (ProductLengthProperty / 1000) * SteelAttr.Kg;
            }
            return weight;
        }
        #endregion

        #region 新版屬性
        private ObservableCollection<ProductSettingsPageViewModel> _dataviews = new ObservableCollection<ProductSettingsPageViewModel>();

        /// <summary>
        /// 零件資訊
        /// </summary>
        public ObservableCollection<ProductSettingsPageViewModel> DataViews
        {
            get { return _dataviews; }
            set { _dataviews = value; }
        }
        /// <summary>
        /// 所選到斷面規格在其斷面規格list中的index
        /// </summary>
        private int CurrentSection_ListIndex;
        /// <summary>
        /// true:所選到斷面規格來源是零件清單，false:所選到斷面規格來源是手動選擇
        /// </summary>
        private bool fCurrentSectionSource = false;
        #endregion


        /// <summary>
        /// 初始化
        /// </summary>
        public ObSettingVM()
        {
            //Reductions = new ReductionList(c);
            //TODO:可以設定 X Y Z 軸向顏色
            //初始化數據綁定的坐標系圖標。
            CoordinateSystemIcon = new CoordinateSystemIcon();

            //初始化數據綁定的原始符號。
            OriginSymbols = new ObservableCollection<OriginSymbol>(new List<OriginSymbol>() { OriginSymbol.GetDefaultOriginSymbol() });

            STDSerialization ser = new STDSerialization(); //序列化處理器

            //如果模型有構件列表
            if (File.Exists(ApplicationVM.FileSteelAssembly()))
            {
                SteelAssemblies = ser.GetGZipAssemblies();
                if (SteelAssemblies == null)
                {
                    SteelAssemblies = new ObservableCollection<SteelAssembly>();
                }
            }
            //如果模型有材質設定
            if (!File.Exists(ApplicationVM.FileMaterial()))
            {
                Materials.AddRange(SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(ApplicationVM.FileMaterial())); //材質序列化檔案
            }
            DicSteelPart = ser.GetPart();
            Materials = ser.GetMaterial(); //材質序列化檔案

            LoadAttribute();//載入用戶屬性

            //初始化選單
            ProfileType = 0;
            ProfileIndex = 0;

            DataCorrespond = ser.GetDataCorrespond();
            var groupData = from el in DataCorrespond group el by el.Type.GetType().GetMember(el.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description into el orderby el.Key select el;
            //for (int i = 0; i < DataCorrespond.Count; i++)
            //{
            //    AddNode(DataCorrespond[i]);
            //}
            Display3DViewerCommand = Display3DViewer();
            DisplayCreatedListCommand = DisplayCreatedList();
            DisplayPartsCommand = DisplayParts();
            DisplayHoleCommand = DisplayHole();

            // 取得專案Grid資訊
            DataViews = new ObservableCollection<ProductSettingsPageViewModel>(GetData());
            SteelAttr = new SteelAttr();

            ShowSteelTypeCommand = ShowSteelType(); //20220829 張燕華 選擇型鋼型態
            CalculateWeightCommand = CalculateWeight();

            InitializeSteelAttr();
        }
        #region 私有屬性
        /// <summary>
        /// 第一層樹狀圖索引
        /// </summary>
        private Dictionary<string, int> level1 = new Dictionary<string, int>();
        /// <summary>
        /// 第二層樹狀索引
        /// </summary>
        private Dictionary<string, int> level2 = new Dictionary<string, int>();
        /// <summary>
        /// 選擇的斷面規格類型 <see cref="OBJECT_TYPE"/>索引值
        /// </summary>
        private int _ProfileType { get; set; } = -1;
        /// <summary>
        /// 選擇的斷面規格<see cref="ProfileList"/>索引值
        /// </summary>
        private int _ProfileIndex { get; set; } = -1;
        /// <summary>
        /// 材質類型
        /// </summary>
        private int _MaterialIndex { get; set; } = 0;
        /// <summary>
        /// 鑽孔模式
        /// </summary>
        /// <remarks>
        /// <see cref="GD_STD.Enum.AXIS_MODE"/>
        /// </remarks>
        private int _AxisModeType { get; set; } = 0;
        /// <summary>
        /// 加入螺栓選擇的面
        /// </summary>
        private int _BoltsFaceType { get; set; } = 0;
        /// <summary>
        /// 孔位暫存緩衝區
        /// </summary>
        private GroupBoltsAttr Boltsbuffer { get; set; } = new GroupBoltsAttr();
        /// <summary>
        /// 鋼構物件設定檔緩衝區
        /// </summary>
        private SteelAttr Steelbuffer { get; set; } = new SteelAttr();
        #endregion

        #region 私有方法  
        private void InitializeSteelAttr()
        {
            SteelAttr.H = 0;
            SteelAttr.W = 0;
            SteelAttr.t1 = 0;
            SteelAttr.t2 = 0;
        }
        public ObservableCollection<ProductSettingsPageViewModel> GetPartData(ModelExt model)
        {
            // get all dm file
            ApplicationVM appVM = new ApplicationVM();
            List<string> dmList = appVM.GetAllDevPart();

            // foreach dm file
            foreach (var dataName in dmList)
            {
                // 讀取dm檔
                ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{dataName}.dm", devDept.Serialization.contentType.GeometryAndTessellation);
                readFile.DoWork();
                readFile.AddToScene(model);//將讀取完的檔案放入到模型
                                           // 加入VM
                DataViews.Add(new ProductSettingsPageViewModel() { steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData });
            }
            return DataViews;
        }

        /// <summary>
        /// 加入節點
        /// </summary>
        public void AddNode(DataCorrespond data)
        {
            //20220805 張燕華 新增斷面規格 - 因斷面規格已經在data中有定義, 故略過這層判斷直接執行以下程式
            //if (data.Type != OBJECT_TYPE.RH)
            //{
            //    return;
            //}
            string level1Key = WPFWindowsBase.BaseEnumValueConverter<OBJECT_TYPE>.GetDescription(data.Type);
            if (string.IsNullOrEmpty(level1Key))
            {
                return;
            }

            //string level1Key = data.Type.GetType().GetMember(data.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description; //第一層要設置的 key 值
            string level2Key = data.Profile; //第二層要設置的 key 值

            if (!level1.ContainsKey(level1Key))
            {
                TreeNode.Add(new TreeNode() { ItemName = level1Key, Children = new ObservableCollection<TreeNode>() });
                level1.Add(level1Key, TreeNode.Count - 1);
            }
            if (!level2.ContainsKey(level2Key))
            {
                TreeNode[level1[level1Key]].Children.Add(new TreeNode() { ItemName = level2Key, Children = new ObservableCollection<TreeNode>() });
                level2.Add(level2Key, TreeNode[level1[level1Key]].Children.Count - 1);
            }


            TreeNode[level1[level1Key]].Children[level2[level2Key]].Children.Add(new TreeNode() { ItemName = data.Number, DataName = data.DataName });
        }
        /// <summary>
        /// 載入用戶屬性
        /// </summary>
        private void LoadAttribute()
        {
            OriginSymbol.Visible = SofSetting.Default.OsVisible;
        }
        /// <summary>
        /// 切割線邏輯判斷
        /// </summary>
        /// <param name="bufferCutList">要判斷的切割線</param>
        /// <param name="d">極限寬度或高度</param>
        /// <returns></returns>
        private bool CutLogic(CutList bufferCutList, double d)
        {
            if (bufferCutList.DL.Y + bufferCutList.UL.Y > d || bufferCutList.DR.Y + bufferCutList.UR.Y > d)
            {
                //MessageBox.Show("切割線 Y 大於翼板寬度，請重新輸入。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"切割線 Y 大於翼板寬度，請重新輸入",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return false;
            }
            else if (bufferCutList.UL.X + bufferCutList.UR.X > SteelAttr.Length || bufferCutList.DL.X + bufferCutList.DR.X > SteelAttr.Length)
            {
                //MessageBox.Show("切割線大於物件長度寬度，請重新輸入。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"切割線大於物件長度寬度",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return false;
            }
            return true;
        }
        /// <summary>
        ///  寫入切割設定 To VM
        /// </summary>
        /// <param name="cutList">鋼構切割設定檔</param>
        private void WriteCutAttr(CutList cutList)
        {
            URPoint = cutList.UR;
            DRPoint = cutList.DR;
            DLPoint = cutList.DL;
            ULPoint = cutList.UL;
        }
        private WPFBase.RelayCommand Display3DViewer()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (ThreeDimensionalDisplayControl)
                    ThreeDimensionalDisplayControl = false;
                else
                    ThreeDimensionalDisplayControl = true;
            });
        }

        private WPFBase.RelayCommand DisplayCreatedList()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (DisplayCreatedListControl)
                    DisplayCreatedListControl = false;
                else
                    DisplayCreatedListControl = true;
            });
        }
        private WPFBase.RelayCommand DisplayParts()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (DisplayPartsControl)
                    DisplayPartsControl = false;
                else
                    DisplayPartsControl = true;
            });
        }
        private WPFBase.RelayCommand DisplayHole()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (DisplayHoleControl)
                    DisplayHoleControl = false;
                else
                    DisplayHoleControl = true;
            });
        }
        /// <summary>
        /// 選擇型鋼型態 20220829 張燕華
        /// </summary>
        private WPFBase.RelayParameterizedCommand ShowSteelType()
        {
            return new WPFBase.RelayParameterizedCommand((object SelectedIndex) =>
            {
                if ((int)SelectedIndex != -1) ProfileType = Convert.ToInt32(SelectedIndex);
            });
        }
        /// <summary>
        /// 計算製品重量 20220913 張燕華
        /// </summary>
        private WPFBase.RelayCommand CalculateWeight()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (fPartListOrManuall == false)
                {
                    ProductWeightProperty = CalculateSinglePartWeight();
                }
            });
        }
        #endregion

        /// <summary>
        /// 取得專案內零件資訊
        /// </summary>
        /// <returns></returns>
        public List<ProductSettingsPageViewModel> GetData()
        {
            STDSerialization ser = new STDSerialization();
            // 限制Grid出現之內容
            //List<OBJECT_TYPE> allowType = new List<OBJECT_TYPE> { OBJECT_TYPE.RH, OBJECT_TYPE.BH, OBJECT_TYPE.H, OBJECT_TYPE.BOX, OBJECT_TYPE.TUBE, OBJECT_TYPE.LB, OBJECT_TYPE.CH };

            // 取得dm檔與零件之對應
            ObservableCollection<DataCorrespond> DataCorrespond = ser.GetDataCorrespond();

            // 取得構件資訊
            ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies();
            if (assemblies == null)
            {
                return new List<ProductSettingsPageViewModel>();
            }
            //取得零件資訊
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();

            // 取得孔群資訊
            Dictionary<string, ObservableCollection<SteelBolts>> bolts = ser.GeBolts();

            List<Tuple<string, int, List<int>>> assNumber_ID = new List<Tuple<string, int, List<int>>>();

            // 構件 vs 該節點
            foreach (var item in assemblies)
            {
                assNumber_ID.Add(Tuple.Create(item.Number,item.Count,item.ID));
            }
            //var assNumber_ID = assemblies
            //    .Select(x => new Tuple() { 
            //        Number = x.Number,
            //        Count = x.Count,
            //        ID= x.ID })
            //    .ToList()
            //    .ToDictionary(x => new Tuple() { x.Number, x.Count }, y => y.ID);//.SelectMany(x => x.Key, (x, y) =>new { x.Key, x.Value }).Select(x => new {x.Key,x.Value})

            // 零件 vs 父節點
            // SelectMany 把資料攤平
            var partNumber_ID = part.Values
                .SelectMany(x => x)
                .Select(x => new
                {
                    x.ExclamationMark,
                    x.GUID,
                    x.Lock,
                    x.Creation,
                    x.Revise,
                    x.Phase,
                    x.ShippingNumber,
                    x.Title1,
                    x.Title2,
                    x.H,
                    x.W,
                    x.t1,
                    x.t2,
                    x.Number,
                    x.Type,
                    x.DrawingName,
                    x.Profile,
                    x.Material,
                    x.Count,
                    x.Length,
                    x.UnitWeight,
                    x.Father,
                    x.ID
                }).Where(x => allowType.Contains(x.Type)).ToList();

            // 孔 vs 父節點
            var boltsFather_ID = (bolts.Values).SelectMany(x => x).Select(x => new
            {
                Creation = x.Creation,
                Revise = x.Revise,
                Count = (x == null ? x.Count : 0),
                Father = (x == null ? null : x.Father),
                Profile = (x == null ? "" : x.Profile),
                Type = "Bolts",
                Material = (x == null ? "" : x.Material)
            });

            var boltsList = bolts.Values.ToList();

            List<ProductSettingsPageViewModel> steelAttrList = new List<ProductSettingsPageViewModel>();
            ProductSettingsPageViewModel steelAttrVM = new ProductSettingsPageViewModel();
            #region 構件開始
            //foreach (KeyValuePair<object, List<int>> assembliesItem in assNumber_ID)
            foreach (Tuple<string,int, List<int>> assembliesItem in assNumber_ID)
            {
                // 構件編號
                string assem = assembliesItem.Item1;
                // 構件數量
                int AssCount = assembliesItem.Item2;
                // 構件ID List
                foreach (int id in assembliesItem.Item3)
                {
                    // 構件ID
                    int assemID = id;
                    #region 零件資訊
                    // 在零件清單中，比對Father找到此構件
                    var part_father = partNumber_ID.Where(x => x.Father.Contains(assemID) && x.Count == AssCount).ToList();
                    // 如果有找到的話
                    if (part_father.Any())
                    {
                        // 比對Father及零件ID之Index
                        foreach (var item in part_father)
                        {
                            // 當父節點中還找的到父節點ID時，代表該零件尚存在其他構件中
                            while (item.Father.Contains(assemID))
                            {
                                #region Grid零件內容
                                steelAttrVM = new ProductSettingsPageViewModel();
                                // 建立日期
                                steelAttrVM.Creation = item.Creation;
                                steelAttrVM.steelAttr.Creation = item.Creation;
                                // 修改日期
                                steelAttrVM.Revise = item.Revise;
                                steelAttrVM.steelAttr.Revise = item.Revise;
                                // Tekla構件ID
                                steelAttrVM.TeklaAssemblyID = assemID.ToString();
                                // 構件編號
                                steelAttrVM.steelAttr.AsseNumber = assem;
                                steelAttrVM.AssemblyNumber = assem;
                                // 零件編號
                                steelAttrVM.steelAttr.PartNumber = item.Number;
                                // 斷面規格
                                string profile = item.Profile;
                                steelAttrVM.steelAttr.Profile = profile;
                                steelAttrVM.Profile = profile;
                                // 零件長
                                double length = item.Length;
                                steelAttrVM.steelAttr.Length = length;
                                steelAttrVM.Length = length;
                                // 零件ID List
                                var partList = partNumber_ID.Where(x => x.Number == item.Number && x.Profile == profile && x.Length == length && x.Father.Contains(assemID)).Select(x => x.ID).FirstOrDefault();
                                // 構件ID List
                                var fatherList = partNumber_ID.Where(x => x.Number == item.Number && x.Profile == profile && x.Length == length && x.Father.Contains(assemID)).Select(x => x.Father).FirstOrDefault();
                                // Father的index = Part的index
                                var partIndex = fatherList.IndexOf(assemID);
                                // 取得該筆零件ID
                                int partID = partList[partIndex];
                                //partNumber_ID.Where(x => x.Number == item.Number && x.Father[i] == assemID && x.ID[i] == partID).FirstOrDefault();
                                //var idList = partNumber_ID.Where(x => x.Number == item.Number).Select(x => x.ID).FirstOrDefault();
                                // 移除本次構件ID 避免下次 FirstOrDefault 重複抓到
                                fatherList.Remove(assemID);
                                // 移除本次零件ID 避免下次 FirstOrDefault 重複抓到
                                partList.Remove(partID);
                                // 零件ID
                                steelAttrVM.steelAttr.TeklaPartID = partID.ToString();
                                // Tekla 圖名稱 // 匯入時 請檢查是匯入"NuLL還是空白"
                                steelAttrVM.TeklaName = (string.IsNullOrEmpty(item.DrawingName) || item.DrawingName == "null") ? "" : item.DrawingName;
                                steelAttrVM.steelAttr.Name = (string.IsNullOrEmpty(item.DrawingName) || item.DrawingName == "null") ? "" : item.DrawingName;
                                // 鋼材類別
                                var aa = item.Type.GetType().GetMember(item.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>();
                                string type = aa == null ? "" : aa.Description;

                                steelAttrVM.TypeDesc = type;

                                steelAttrVM.Type = item.Type;
                                steelAttrVM.steelAttr.Type = item.Type;

                                steelAttrVM.SteelType = Convert.ToInt32(item.Type);
                                // 材質
                                string material = item.Material;
                                steelAttrVM.steelAttr.Material = material;
                                steelAttrVM.Material = material;
                                // 數量
                                int count = item.Count;
                                steelAttrVM.steelAttr.Number = count;
                                steelAttrVM.Count = count;
                                // 零件重
                                double weight = item.UnitWeight;
                                steelAttrVM.steelAttr.Weight = weight;
                                steelAttrVM.Weight = weight;

                                steelAttrVM.steelAttr.H = item.H;
                                steelAttrVM.steelAttr.W = item.W;
                                steelAttrVM.steelAttr.t1 = item.t1;
                                steelAttrVM.t1 = item.t1;
                                steelAttrVM.steelAttr.t2 = item.t2;
                                steelAttrVM.t2 = item.t2;

                                steelAttrVM.Title1 = item.Title1;
                                steelAttrVM.steelAttr.Title1 = item.Title1;
                                steelAttrVM.Title2 = item.Title2;
                                steelAttrVM.steelAttr.Title2 = item.Title2;
                                steelAttrVM.Phase = item.Phase;
                                steelAttrVM.steelAttr.Phase = item.Phase;
                                steelAttrVM.ShippingNumber = item.ShippingNumber;
                                steelAttrVM.steelAttr.ShippingNumber = item.ShippingNumber;

                                // 上鎖
                                steelAttrVM.steelAttr.Lock = item.Lock;
                                steelAttrVM.Lock = item.Lock;
                                // 驚嘆號
                                steelAttrVM.steelAttr.ExclamationMark = item.ExclamationMark;
                                steelAttrVM.ExclamationMark = item.ExclamationMark;

                                //// GUID (Data Name)
                                //DataCorrespond single = DataCorrespond.FirstOrDefault(x =>
                                //x.Profile == steelAttrVM.Profile &&
                                //x.Number == steelAttrVM.steelAttr.PartNumber &&
                                //allowType.Contains(x.Type));
                                //if (single != null)
                                steelAttrVM.steelAttr.GUID = item.GUID;
                                steelAttrVM.DataName = item.GUID.ToString();
                                //partNumber_ID.Remove(delPart);
                                steelAttrList.Add(steelAttrVM);
                                #endregion
                            }
                        }

                        #region 孔資訊
                        if (boltsList.Count() > 0 && boltsList[0] != null)
                        {
                            // 取得孔的父節點ID
                            var bolt_father = boltsFather_ID.Where(x => x.Father.Contains(assemID)).ToList();
                            foreach (var item in bolt_father)
                            {
                                // // 當父節點中還找的到父節點ID時，代表該零件尚存在其他構件中
                                while (item.Father.Contains(assemID))
                                {
                                    #region Grid零件內容
                                    steelAttrVM = new ProductSettingsPageViewModel();
                                    steelAttrVM.Creation = item.Creation;
                                    steelAttrVM.Revise = item.Revise;
                                    steelAttrVM.TeklaAssemblyID = assemID.ToString();
                                    steelAttrVM.steelAttr.Name = assemID.ToString();
                                    steelAttrVM.AssemblyNumber = assem;
                                    steelAttrVM.steelAttr.AsseNumber = assem;
                                    steelAttrVM.Profile = item.Profile;
                                    steelAttrVM.steelAttr.Profile = item.Profile;
                                    steelAttrVM.Type = OBJECT_TYPE.Unknown;
                                    steelAttrVM.steelAttr.Type = OBJECT_TYPE.Unknown;
                                    steelAttrVM.TypeDesc = "Bolts";
                                    steelAttrVM.Count = item.Count;
                                    steelAttrVM.steelAttr.Number = item.Count;
                                    steelAttrVM.Material = item.Material;
                                    steelAttrVM.steelAttr.Material = item.Material;
                                    var fatherList = boltsFather_ID.Where(x => x.Profile == steelAttrVM.Profile).Select(x => x.Father).FirstOrDefault();
                                    fatherList.Remove(assemID);
                                    steelAttrList.Add(steelAttrVM);
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion
            List<ProductSettingsPageViewModel> source = new List<ProductSettingsPageViewModel>();
            source = steelAttrList.Where(x => allowType.Contains(x.Type)).ToList();
            var group = (from a in source
                         group a by new { AsseNumber = a.steelAttr.AsseNumber, a.steelAttr.PartNumber, a.TeklaName, a.Type, a.Length,a.Profile, a.Weight } into g
                         select new
                         {
                             AssemblyNumber = g.Key.AsseNumber,
                             PartNumber = g.Key.PartNumber,
                             Profile = g.Key.Profile,
                             Length = g.Key.Length,
                             Weight = g.Key.Weight,
                             Creation = g.FirstOrDefault().Creation,
                             Revise = g.FirstOrDefault().Revise,
                             DataName = g.FirstOrDefault().steelAttr.GUID,
                             TeklaName = g.FirstOrDefault().TeklaName,
                             TypeDesc = g.FirstOrDefault().TypeDesc,
                             SteelType = g.FirstOrDefault().SteelType,
                             Material = g.FirstOrDefault().Material,
                             //Count = g.Sum(x => x.Count),
                             Count = g.FirstOrDefault().Count,
                             Phase = g.FirstOrDefault().Phase,
                             ShippingNumber = g.FirstOrDefault().ShippingNumber,
                             Title1 = g.FirstOrDefault().Title1,
                             Title2 = g.FirstOrDefault().Title2,
                             ExclamationMark = g.FirstOrDefault().ExclamationMark,
                             t1 = g.FirstOrDefault().steelAttr.t1,
                             t2 = g.FirstOrDefault().steelAttr.t2,
                             H = g.FirstOrDefault().steelAttr.H,
                             W = g.FirstOrDefault().steelAttr.W
                         }).ToList();
            List<ProductSettingsPageViewModel> list = new List<ProductSettingsPageViewModel>();

            Dictionary<string, ObservableCollection<SteelAttr>> saFile = ser.GetSteelAttr();
            SteelAttr temp = new SteelAttr();
            //foreach (var item in source)
            foreach (var item in group)
            {
                ProfileType = item.SteelType;
                ProductSettingsPageViewModel aa = new ProductSettingsPageViewModel()
                {
                    Type = (OBJECT_TYPE)item.SteelType,
                    Creation = item.Creation,
                    Revise = item.Revise,
                    DataName = item.DataName == null ? "" : item.DataName.ToString(),
                    AssemblyNumber = item.AssemblyNumber,
                    TeklaName = item.TeklaName,
                    TypeDesc = item.TypeDesc,
                    SteelType = item.SteelType,
                    Profile = item.Profile,
                    Material = item.Material,
                    Count = item.Count,
                    Length = item.Length,
                    Phase = item.Phase,
                    ShippingNumber = item.ShippingNumber,
                    Title1 = item.Title1,
                    Title2 = item.Title2,
                    t1 = item.t1,
                    t2 = item.t2,
                    ExclamationMark = item.ExclamationMark == null ? false : item.ExclamationMark,
                };
                //// source專用
                //aa.steelAttr.GUID = item.steelAttr.GUID;
                //aa.steelAttr.PartNumber = item.steelAttr.PartNumber;
                //aa.steelAttr.H = item.steelAttr.H;
                //aa.steelAttr.W = item.steelAttr.W;
                // group專用
                aa.steelAttr.GUID = item.DataName;
                aa.steelAttr.PartNumber = item.PartNumber;
                aa.steelAttr.H = item.H;
                aa.steelAttr.W = item.W;
                aa.steelAttr.AsseNumber = item.AssemblyNumber;
                aa.steelAttr.Type = (OBJECT_TYPE)item.SteelType;
                aa.steelAttr.Profile = item.Profile;
                aa.steelAttr.Material = item.Material;
                aa.steelAttr.Length = item.Length;
                aa.Weight = PartWeight(aa, saFile); //item.Weight,
                aa.steelAttr.Weight = aa.Weight;
                aa.steelAttr.Name = item.TeklaName;
                aa.steelAttr.t1 = float.Parse(item.t1.ToString());
                aa.steelAttr.t2 = float.Parse(item.t2.ToString());
                aa.steelAttr.Title1 = item.Title1;
                aa.steelAttr.Title2 = item.Title2;
                aa.steelAttr.Phase = item.Phase;
                aa.steelAttr.ShippingNumber = item.ShippingNumber;
                aa.steelAttr.ExclamationMark = item.ExclamationMark;
                list.Add(aa);
            }
            return list;
        }
        /// <summary>
        /// 計算單位重
        /// </summary>
        /// <param name="part_tekla"></param>
        /// <param name="saFile"></param>
        /// <returns></returns>
        public double PartWeight(ProductSettingsPageViewModel part_tekla, Dictionary<string, ObservableCollection<SteelAttr>> saFile)
        {
            var profile = saFile[((OBJECT_TYPE)part_tekla.SteelType).ToString()]
                .Where(x => x.Profile == part_tekla.Profile)
                .FirstOrDefault();

            if (profile != null)
            {
                return (part_tekla.Length/ 1000) * profile.Kg;
            }
            return part_tekla.Weight;
        }
        /// <summary>
        /// 由使用者提供之孔距計算數量
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int CalBoltNumber(string str = "60 2*70 60")
        {
            int count = 0;
            // 依照空格拆解
            var space = str.Split(' ').ToList();
            foreach (var item in space)
            {
                if (item.IndexOf('*') == -1) { count++; }
                else
                {
                    var start = item.Split('*');
                    count = count + int.Parse(start[0]);
                }
            }
            return count + 1;
        }
    }
}