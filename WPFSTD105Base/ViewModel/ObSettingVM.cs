#define Debug
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using DevExpress.Utils;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DocumentFormat.OpenXml.EMMA;
using GD_STD;
using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.Properties;
using WPFSTD105.Tekla;
using WPFWindowsBase;
using static devDept.Eyeshot.Environment;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;

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
        /// 20221125 張燕華 根據型鋼位置載入切割線數值
        /// </summary>
        public ICommand rb_CutLinePosition_Command { get; set; }
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
        public String _componentVisible { get; set; } = Debugger.IsAttached ? "Visible" : "Collapsed";
        /// <summary>
        /// 限制Grid出現之內容
        /// </summary>
        public static List<OBJECT_TYPE> allowType = new List<OBJECT_TYPE> {
            OBJECT_TYPE.RH, OBJECT_TYPE.BH,OBJECT_TYPE.CH,OBJECT_TYPE.LB
        , OBJECT_TYPE.H};
        //OBJECT_TYPE.BOX, OBJECT_TYPE.TUBE,
        //OBJECT_TYPE.LB, 

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
       // public ProductSettingsPageViewModel ProductSettingsPageViewModel = new ProductSettingsPageViewModel();

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
        /// 編輯已選擇，可視性
        /// </summary>
        public bool EditObjectVisibility { get; set; } //= Visibility.Collapsed;
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
            get
            {
                //if (File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)_ProfileType}.inp"))
                //{
                //    ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)_ProfileType}.inp");
                //} 
                return _ProfileType;
            }
            set
            {
                try
                {
                    _ProfileType = value;
                    //SteelTypeProperty_int= value;
                    //this.SteelTypeProperty_int = _ProfileType;
                    List<SteelAttr> list = new List<SteelAttr>();
                    OBJECT_TYPE TYPE = (OBJECT_TYPE)value;

                    if (!File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{TYPE}.inp"))
                    {
                        File.Copy($@"Profile\{TYPE}.inp", $@"{ApplicationVM.DirectoryPorfile()}\{TYPE}.inp");//複製 BH 斷面規格到模型內
                    }
                    //ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{TYPE}.inp");
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
                        case OBJECT_TYPE.BOX:
                        case OBJECT_TYPE.BH:
                            //case OBJECT_TYPE.L: //20220805 張燕華 新增斷面規格 - 已不在介面上顯示此規格
                            ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(TYPE).ToString()}.inp");
                            break;
                        default:
                            ProfileList = new ObservableCollection<SteelAttr>();
                            break;
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
        private double _productWeightProperty = 0;
        /// <summary>
        /// 製品重
        /// </summary>
        public double ProductWeightProperty { get; set; }
        /// <summary>
        /// 製品數量
        /// </summary>
        public int ProductCountProperty { get; set; }
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



        private int _steelTypeProperty_int = -1;
        /// <summary>
        /// 型鋼類型
        /// </summary>
        public int SteelTypeProperty_int { get { return _steelTypeProperty_int; } set { _steelTypeProperty_int = value; } }
        /// <summary>
        /// 型鋼類型
        /// </summary>
        public OBJECT_TYPE SteelTypeProperty_enum { get; set; }
        /// <summary>
        /// 當前斷面規格
        /// </summary>
        public string SteelSectionProperty { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public Guid? GuidProperty { get; set; }
        /// <summary>
        /// 切割線
        /// </summary>
        public CutList PointTopProperty { get; set; } = new CutList();
        /// <summary>
        /// 切割線
        /// </summary>
        public CutList PointFrontProperty { get; set; } = new CutList();
        /// <summary>
        /// 切割線
        /// </summary>
        public CutList PointBackProperty { get; set; } = new CutList();
        /// <summary>
        /// 高
        /// </summary>
        public float HProperty { get; set; }
        /// <summary>
        /// 單位公斤
        /// </summary>
        public float KGProperty { get; set; }
        /// <summary>
        /// 寬
        /// </summary>
        public float WProperty { get; set; }
        /// <summary>
        /// 腹板厚度
        /// </summary>
        public float t1Property { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        public float t2Property { get; set; }
        /// <summary>
        /// 驚嘆號
        /// </summary>
        public bool? ExclamationMarkProperty { get; set; }


        public GroupBoltsType ComboxEdit_GroupBoltsTypeSelected { get; set; }


        /// <summary>
        /// 異孔偏移孔群
        /// </summary>          
        public ObservableCollection<JointPoint> JointPointList { get; set; }



        private ObservableCollection<JointPoint> _arbitrarilyJointPointList = null;

        /// <summary>
        /// 任意打點
        /// </summary>
        public ObservableCollection<JointPoint> ArbitrarilyJointPointList
        {
            get
            {
                if (_arbitrarilyJointPointList == null)
                {
                    var DefaultJPArray = new List<JointPoint>();
                    for (int i = 0; i < 8; i++)
                    {
                        DefaultJPArray.Add(new JointPoint() { X_Position = 0, Y_Position = 0 });
                    }
                    _arbitrarilyJointPointList = new ObservableCollection<JointPoint>(DefaultJPArray);
                 }

                return _arbitrarilyJointPointList;
            }
            set
            {
                _arbitrarilyJointPointList = value;
                OnPropertyChanged(nameof(ArbitrarilyJointPointList));
            }
        }








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
            get
            {
                //if (File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp"))
                //{
                //    ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp");
                //}
                
                return _ProfileIndex;
            }
            set
            {
                _ProfileIndex = value;
                //if (File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp"))
                //{
                //    ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp");
                //}

                SteelAttr _steelAttr;
                if (value == -1)
                    _steelAttr = ProfileList[0];
                else
                    _steelAttr = ProfileList[value];

                Steelbuffer = (SteelAttr)_steelAttr.DeepClone();
                
                
                //this.SteelSectionProperty= Steelbuffer.Profile;
                //this.CurrentPartSteelAttr.H = Steelbuffer.H;
                //this.HProperty = Steelbuffer.H;
                //this.CurrentPartSteelAttr.W = Steelbuffer.W;
                //this.WProperty = Steelbuffer.W;
                //this.CurrentPartSteelAttr.t1 = Steelbuffer.t1;
                //this.t1Property = Steelbuffer.t1;
                //this.CurrentPartSteelAttr.t2 = Steelbuffer.t2;
                //this.t2Property = Steelbuffer.t2;
                //this.ProductWeightProperty = (this.ProductLengthProperty / 1000) * Steelbuffer.Kg;
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
                DataName = GuidProperty.Value.ToString(),
                Number = PartNumberProperty,
                Type = (OBJECT_TYPE)SteelTypeProperty_int,
                Profile = SteelSectionProperty,
                //DataName = SteelAttr.GUID.ToString(),
                //Number = SteelAttr.PartNumber,
                //Type = SteelAttr.Type,
                //Profile = SteelAttr.Profile,
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
            this.Steelbuffer = (SteelAttr)SteelAttr.Clone();
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
            this.Steelbuffer = (SteelAttr)SteelAttr.Clone();
            Boltsbuffer.groupBoltsType = GroupBoltsAttr.groupBoltsType;
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
                            case OBJECT_TYPE.LB:
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
            Boltsbuffer.groupBoltsType = GroupBoltsAttr.groupBoltsType;
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
            else
            {
                Boltsbuffer.dX = "0";
                Boltsbuffer.xCount = CalBoltNumber(Boltsbuffer.dX);
            }
            //垂直螺栓
            if (CheckY)
            {
                Boltsbuffer.dY = GroupBoltsAttr.dY;
                Boltsbuffer.yCount = CalBoltNumber(GroupBoltsAttr.dY);
                //Boltsbuffer.yCount = GroupBoltsAttr.yCount;
            }
            else
            {
                Boltsbuffer.dY = "0";
                Boltsbuffer.yCount = CalBoltNumber(Boltsbuffer.dY);
            }
            //要產生的面
            if (CheckFace)
            {
                int temp_BoltsFace;
                if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings || ViewLocator.ApplicationViewModel.CurrentPage == ApplicationPage.MachineProductSetting)//若為新版製品設定頁面
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
                            case OBJECT_TYPE.H:
                                Boltsbuffer.Z = Steelbuffer.W * 0.5 - Steelbuffer.t1 * 0.5;
                                break;
                            case OBJECT_TYPE.TUBE:
                            case OBJECT_TYPE.BOX:
                            case OBJECT_TYPE.CH:
                            case OBJECT_TYPE.LB:
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
                Boltsbuffer.Y = this.StartY;// + (Boltsbuffer.Face == FACE.TOP ? SteelAttr.t2 : 0);
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
            //if (File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{steelAttr.Type}.inp"))
            //{
            //    this.ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{steelAttr.Type}.inp");
            //}

            for (int i = 0; i < ProfileList.Count; i++)
            {
                if (ProfileList[i].Profile == steelAttr.Profile)
                {

                    ProfileIndex = i;
                    //this.SteelAttr.PartNumber = steelAttr.PartNumber;
                    //this.SteelAttr.AsseNumber = steelAttr.AsseNumber;
                    //this.SteelAttr.Name = steelAttr.Name;
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
            if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings || ViewLocator.ApplicationViewModel.CurrentPage == ApplicationPage.MachineProductSetting)//若為新版製品設定頁面
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
                if (OfficeViewModel.CurrentPage == OfficePage.ProductSettings || ViewLocator.ApplicationViewModel.CurrentPage == ApplicationPage.MachineProductSetting)//若為新版製品設定頁面
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
                    //bufferCutList = new CutList
                    //  (URCheck ? URPoint : Steelbuffer.PointBack.UR,
                    //   DRCheck ? DRPoint : Steelbuffer.PointBack.DR,
                    //   ULCheck ? ULPoint : SteelAttr.PointBack.UL,
                    //   DLCheck ? DLPoint : SteelAttr.PointBack.DL);
                    //if (CutLogic(bufferCutList, SteelAttr.W))
                    //{
                    //    SteelAttr.PointBack = bufferCutList;
                    //}
                    //break;
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
                            //SteelAttr.PointBack = bufferCutList;
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

                // 讀取切割線設定檔
                STDSerialization ser = new STDSerialization(); //序列化處理器
                ObservableCollection<SteelCutSetting> steelcutSettings = new ObservableCollection<SteelCutSetting>();
                steelcutSettings = ser.GetCutSettingList();
                steelcutSettings = steelcutSettings ?? new ObservableCollection<SteelCutSetting>();

                //switch ((FACE)temp_CutFace)
                //{
                //    case FACE.TOP:
                //        if (steelcutSettings.Any(x => x.GUID == this.GuidProperty && x.face == (FACE)temp_CutFace))
                //        {
                //            // 有該零件 該面之斜邊資訊 更新資料
                //            SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == this.GuidProperty && x.face == (FACE)temp_CutFace);
                //            cs.face = (FACE)temp_CutFace;
                //            cs.DLX = this.DLPoint.X;
                //            cs.DLY = this.DLPoint.Y;
                //            cs.ULX = this.ULPoint.X;
                //            cs.ULY = this.ULPoint.Y;
                //            cs.DRX = this.DRPoint.X;
                //            cs.DRY = this.DRPoint.Y;
                //            cs.URX = this.URPoint.X;
                //            cs.URY = this.URPoint.Y;
                //            ser.SetCutSettingList(steelcutSettings);
                //        }
                //        else
                //        {
                //            // 沒有該零件 該面之斜邊資訊 新增資料
                //            SteelCutSetting cs = new SteelCutSetting()
                //            {
                //                GUID = this.GuidProperty,
                //                face = (FACE)temp_CutFace,
                //                DLX = this.DLPoint.X,
                //                DLY = this.DLPoint.Y,
                //                ULX = this.ULPoint.X,
                //                ULY = this.ULPoint.Y,
                //                DRX = this.DRPoint.X,
                //                DRY = this.DRPoint.Y,
                //                URX = this.URPoint.X,
                //                URY = this.URPoint.Y,
                //            };
                //            steelcutSettings.Add(cs);
                //            ser.SetCutSettingList(steelcutSettings);
                //        }
                //        break;
                //    case FACE.FRONT:
                //    //case FACE.BACK:
                //        if (steelcutSettings.Any(x => x.GUID == this.GuidProperty && (x.face == FACE.FRONT)))
                //        {
                //            // 有該零件 該面之斜邊資訊 更新資料
                //            SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == this.GuidProperty && x.face == FACE.FRONT);
                //            cs.face = FACE.FRONT;
                //            cs.DLX = this.DLPoint.X;
                //            cs.DLY = this.DLPoint.Y;
                //            cs.ULX = this.ULPoint.X;
                //            cs.ULY = this.ULPoint.Y;
                //            cs.DRX = this.DRPoint.X;
                //            cs.DRY = this.DRPoint.Y;
                //            cs.URX = this.URPoint.X;
                //            cs.URY = this.URPoint.Y;
                //            ser.SetCutSettingList(steelcutSettings);
                //        }
                //        else
                //        {
                //            // 沒有該零件 該面之斜邊資訊 新增資料
                //            steelcutSettings.Add(new SteelCutSetting()
                //            {
                //                GUID = this.GuidProperty,
                //                face = FACE.FRONT,
                //                DLX = this.DLPoint.X,
                //                DLY = this.DLPoint.Y,
                //                ULX = this.ULPoint.X,
                //                ULY = this.ULPoint.Y,
                //                DRX = this.DRPoint.X,
                //                DRY = this.DRPoint.Y,
                //                URX = this.URPoint.X,
                //                URY = this.URPoint.Y,
                //            });
                //        }
                //        if (steelcutSettings.Any(x => x.GUID == this.GuidProperty && (x.face == FACE.BACK)))
                //        {
                //            // 有該零件 該面之斜邊資訊 更新資料
                //            SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == this.GuidProperty && x.face == FACE.BACK);
                //            cs.face = FACE.BACK;
                //            cs.DLX = this.DLPoint.X;
                //            cs.DLY = this.DLPoint.Y;
                //            cs.ULX = this.ULPoint.X;
                //            cs.ULY = this.ULPoint.Y;
                //            cs.DRX = this.DRPoint.X;
                //            cs.DRY = this.DRPoint.Y;
                //            cs.URX = this.URPoint.X;
                //            cs.URY = this.URPoint.Y;
                //            ser.SetCutSettingList(steelcutSettings);
                //        }
                //        else
                //        {
                //            steelcutSettings.Add(new SteelCutSetting()
                //            {
                //                GUID = this.GuidProperty,
                //                face = FACE.BACK,
                //                DLX = this.DLPoint.X,
                //                DLY = this.DLPoint.Y,
                //                ULX = this.ULPoint.X,
                //                ULY = this.ULPoint.Y,
                //                DRX = this.DRPoint.X,
                //                DRY = this.DRPoint.Y,
                //                URX = this.URPoint.X,
                //                URY = this.URPoint.Y,
                //            });
                //            ser.SetCutSettingList(steelcutSettings);
                //        }
                //        break;
                //    default:
                //        break;
                //}

                if (steelcutSettings.Any(x => x.GUID == this.GuidProperty && x.face == (FACE)temp_CutFace))
                {
                    // 有該零件 該面之斜邊資訊 更新資料
                    SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == this.GuidProperty && x.face == (FACE)temp_CutFace);
                    cs.face = (FACE)temp_CutFace;
                    cs.DLX = this.DLPoint.X;
                    cs.DLY = this.DLPoint.Y;
                    cs.ULX = this.ULPoint.X;
                    cs.ULY = this.ULPoint.Y;
                    cs.DRX = this.DRPoint.X;
                    cs.DRY = this.DRPoint.Y;
                    cs.URX = this.URPoint.X;
                    cs.URY = this.URPoint.Y;
                    ser.SetCutSettingList(steelcutSettings);
                }
                else
                {
                    // 沒有該零件 該面之斜邊資訊 新增資料
                    SteelCutSetting cs = new SteelCutSetting()
                    {
                        GUID = this.GuidProperty,
                        face = (FACE)temp_CutFace,
                        DLX = this.DLPoint.X,
                        DLY = this.DLPoint.Y,
                        ULX = this.ULPoint.X,
                        ULY = this.ULPoint.Y,
                        DRX = this.DRPoint.X,
                        DRY = this.DRPoint.Y,
                        URX = this.URPoint.X,
                        URY = this.URPoint.Y,
                    };
                    steelcutSettings.Add(cs);
                    ser.SetCutSettingList(steelcutSettings);
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// 複製切割線設定檔
        /// </summary>
        /// <param name="oldGuid"></param>
        /// <param name="newGuid"></param>
        /// <param name=""></param>
        public void SaveCut(Guid? oldGuid, Guid? newGuid)
        {
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<SteelCutSetting> steelcutSettings = new ObservableCollection<SteelCutSetting>();
            ObservableCollection<SteelCutSetting> newSettings = new ObservableCollection<SteelCutSetting>();
            steelcutSettings = ser.GetCutSettingList();
            steelcutSettings = steelcutSettings ?? new ObservableCollection<SteelCutSetting>();
            if (steelcutSettings.Any(x => x.GUID == oldGuid))
            {
                foreach (SteelCutSetting item in steelcutSettings.Where(x => x.GUID == oldGuid))
                {
                    newSettings.Add(new SteelCutSetting()
                    {
                        GUID = newGuid,
                        face = item.face,
                        DLX = item.DLX,
                        DLY = item.DLY,
                        DRX = item.DRX,
                        DRY = item.DRY,
                        ULX = item.ULX,
                        ULY = item.ULY,
                        URX = item.URX,
                        URY = item.URY
                    });
                }
            }
            steelcutSettings.AddRange(newSettings);
            ser.SetCutSettingList(steelcutSettings);
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
        /// <summary>
        /// 是否顯示訊息
        /// </summary>
        public bool showMessage { get; set; } = true;
        /// <summary>
        /// 從編輯孔開始
        /// </summary>
        public bool fromModifyHole { get; set; } = false;

       private ObservableCollection<ProductSettingsPageViewModel> _dataviews;
        /// <summary>
        /// 零件資訊
        /// </summary>
        public ObservableCollection<ProductSettingsPageViewModel> DataViews
        {
            get 
            {
                if(_dataviews == null)
                {
                    _dataviews = new ObservableCollection<ProductSettingsPageViewModel>();
                }
                return _dataviews; 
            }
            set
            {
                _dataviews = value;
                OnPropertyChanged(nameof(DataViews));
            }
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
                try
                {
                    Materials.AddRange(SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(ApplicationVM.FileMaterial())); //材質序列化檔案
                }
                catch
                {

                }
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

            ShowSteelTypeCommand = ShowSteelType();//20220829 張燕華 選擇型鋼型態
            CalculateWeightCommand = CalculateWeight();
            rb_CutLinePosition_Command = rb_CutLinePosition();

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
                     FloatingMode.Window);
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
                     FloatingMode.Window);
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
                if ((int)SelectedIndex != -1)
                    {
                    ProfileType = Convert.ToInt32(SelectedIndex);
                    if (File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp"))
                    {
                        ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{(OBJECT_TYPE)ProfileType}.inp");
                    }
                }
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
        /// <summary>
        /// 20221125 張燕華 根據型鋼位置載入切割線數值
        /// </summary>
        private WPFBase.RelayCommand rb_CutLinePosition()
        {
            return new WPFBase.RelayCommand(() =>
            {
                // 讀取切割線設定檔
                STDSerialization ser = new STDSerialization(); //序列化處理器
                ObservableCollection<SteelCutSetting> steelcutSettings = new ObservableCollection<SteelCutSetting>();
                steelcutSettings = ser.GetCutSettingList();
                steelcutSettings = steelcutSettings ?? new ObservableCollection<SteelCutSetting>();


                if (steelcutSettings.Any(x => x.GUID == this.GuidProperty && x.face == (FACE)rbtn_CutFace))
                {
                    SteelCutSetting cs = steelcutSettings.FirstOrDefault(x => x.GUID == this.GuidProperty && x.face == (FACE)rbtn_CutFace);

                    DLPoint.X = cs.DLX;
                    DLPoint.Y = cs.DLY;
                    ULPoint.X = cs.ULX;
                    ULPoint.Y = cs.ULY;
                    DRPoint.X = cs.DRX;
                    DRPoint.Y = cs.DRY;
                    URPoint.X = cs.URX;
                    URPoint.Y = cs.URY;
                }
                else
                {
                    SteelCutSetting cs = new SteelCutSetting()
                    {
                        GUID = this.GuidProperty,
                        face = (FACE)rbtn_CutFace,
                        DLX = 0,
                        DLY = 0,
                        ULX = 0,
                        ULY = 0,
                        DRX = 0,
                        DRY = 0,
                        URX = 0,
                        URY = 0,
                    };
                    steelcutSettings.Add(cs);
                    ser.SetCutSettingList(steelcutSettings);

                    DLPoint.X = 0;
                    DLPoint.Y = 0;
                    ULPoint.X = 0;
                    ULPoint.Y = 0;
                    DRPoint.X = 0;
                    DRPoint.Y = 0;
                    URPoint.X = 0;
                    URPoint.Y = 0;
                }


            });
        }
        #endregion
        public void GetNoNCPart()
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();
            // 取得所有NC檔之路徑
            string path = ApplicationVM.DirectoryNc();
            string dataName = Path.GetFileName(path);//檔案名稱
            // 取得所有斷面規格
            List<SteelAttr> saAll = ser.GetSteelAttr().Values.SelectMany(x => x).ToList();
            // 取得NC檔之檔名(零件)
            List<String> hasNCPart = GetAllNcPath(path).Select(x => x.Substring(x.LastIndexOf("\\") + 1, x.LastIndexOf(".nc1") - x.LastIndexOf("\\") - 1)).ToList();
            // 取得無NC之零件
            List<SteelPart> hasNoNCPart = part.SelectMany(x => x.Value).Where(x => !hasNCPart.Contains(x.Number)).ToList();
            ObservableCollection<DataCorrespond> dc = new ObservableCollection<DataCorrespond>();
            ModelExt model = new ModelExt();
            foreach (SteelPart item in hasNoNCPart)
            {
                string profile = item.Profile;
                SteelAttr sa = saAll.Find(x => x.Profile == profile);
                model.InitializeViewports();
                Steel3DBlock steel = Steel3DBlock.AddSteel(sa, model, out BlockReference blockReference);
                DataCorrespond data = new DataCorrespond()
                {
                    DataName = sa.GUID.ToString(),
                    Number = sa.PartNumber,
                    Type = sa.Type,
                    Profile = sa.Profile,
                    TP = false,
                };
                ser.SetPartModel(model.Blocks[1].Name, model);
                dc.Add(data);
            }
            ser.SetDataCorrespond(dc);

        }

        /// <summary>
        /// 取得模型資料夾所有的 nc1 檔案
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>目前模型內的所有 .nc1 檔案</returns>
        private IEnumerable<string> GetAllNcPath(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string ext = Path.GetExtension(d);//副檔名
                    if (ext == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        yield return d;
                    }
                    else
                    {
                        GetAllNcPath(d);
                    }
                }
            }
        }

        /// <summary>
        /// 取得專案內零件資訊
        /// </summary>
        /// <returns></returns>
        public static List<ProductSettingsPageViewModel> GetData()
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

            var profileAll = ser.GetSteelAttr();
            Dictionary<string, List<SteelAttr>> NcSA = new Dictionary<string, List<SteelAttr>>();
            List<string> profileTemp = part.Keys.ToList();

            foreach (string key in profileTemp)
            {
                // 所有該斷面規格的零件
                List<SteelPart> sa1 = new List<SteelPart>();
                sa1 = (from a in part[key].ToList()
                       select new SteelPart() {Number = a.Number, t1 = a.t1, t2 = a.t2, H = a.H, W = a.W }).ToList();
                // 紀錄零件資訊
                List<SteelAttr> saList = new List<SteelAttr>();
                foreach (var item in DataCorrespond.Where(x => x.Profile.GetHashCode().ToString() + ".lis" == key))
                {
                    string path = ApplicationVM.DirectoryNc();
                    string allPath = path + $"\\{item.Number}.nc1";
                    string p = item.Profile;
                    SteelAttr saTemp = new SteelAttr();
                    if (File.Exists($@"{allPath}"))
                    {
                        string profileStr = item.Profile;
                        var sp = sa1.Where(x => x.Number == item.Number).FirstOrDefault();

                        if (sp != null)
                        {
                            saTemp = new SteelAttr()
                            {
                                Profile= profileStr,
                                PartNumber = sp.Number,
                                t1 = sp.t1,
                                t2 = sp.t2,
                                H = sp.H,
                                W = sp.W
                            };
                            Steel3DBlock s3Db = new Steel3DBlock();
                            SteelAttr steelAttrNC = new SteelAttr();
                            List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
                            s3Db.ReadNcFile($@"{ApplicationVM.DirectoryNc()}\{item.Number}.nc1", profileAll, saTemp, ref steelAttrNC, ref groups);
                            saTemp.GUID = Guid.NewGuid();
                            saTemp.oPoint = steelAttrNC.oPoint;
                            saTemp.vPoint = steelAttrNC.vPoint;
                            saTemp.uPoint = steelAttrNC.uPoint;
                            saTemp.CutList = steelAttrNC.CutList;
                            saTemp.Length = steelAttrNC.Length;
                            //saTemp.PointTop = steelAttrNC.PointTop;
                            //saTemp.PointBack = steelAttrNC.PointBack;
                            //saTemp.PointFront = steelAttrNC.PointFront;
                            //saTemp.Name = "";
                            saTemp.Material = "";
                            //saTemp.Phase = 0;
                            //saTemp.ShippingNumber = 0;
                            //saTemp.Title1 = "";
                            //saTemp.Title2 = "";
                            saTemp.PartNumber = item.Number;
                            saTemp.Profile = item.Profile;
                            if (profileAll.SelectMany(x => x.Value).Any(x => x.Profile == saTemp.Profile))
                            {
                                saTemp.Kg = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == saTemp.Profile).Kg;
                                saTemp.W = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == saTemp.Profile).W;
                            }
                        }
                        saList.Add(saTemp);
                    }
                    else
                    {
                        saTemp.PartNumber = item.Number;
                        saTemp.Profile = item.Profile;
                        if (profileAll.SelectMany(x => x.Value).Any(x => x.Profile == saTemp.Profile))
                        {
                            saTemp.Kg = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == saTemp.Profile).Kg;
                            saTemp.W = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == saTemp.Profile).W;
                        }
                        saList.Add(saTemp);
                    }
                }
                NcSA.Add(key, saList);
            }


            // 取得孔群資訊
            Dictionary<string, ObservableCollection<SteelBolts>> bolts = ser.GeBolts();

            List<Tuple<string, int, List<int>>> assNumber_ID = new List<Tuple<string, int, List<int>>>();

            // 構件 vs 該節點
            foreach (var item in assemblies)
            {
                assNumber_ID.Add(Tuple.Create(item.Number, item.Count, item.ID));
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
                    //x.CountList,
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
            foreach (Tuple<string, int, List<int>> assembliesItem in assNumber_ID)
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
                    // 2022/11/01 呂宗霖 因有相同零件出現再
                    var part_father = partNumber_ID.Where(x => x.Father.Contains(assemID)).ToList();
                    // 如果有找到的話
                    if (part_father.Any())
                    {
                        // 比對Father及零件ID之Index
                        foreach (var item in part_father)
                        {
                            // 零件ID List
                            var partList = new List<int>(partNumber_ID.Where(x => x.Number == item.Number && x.Profile == item.Profile && x.Length == item.Length && x.Father.Contains(assemID)).Select(x => x.ID).FirstOrDefault());
                            // 構件ID List
                            var fatherList = new List<int>(partNumber_ID.Where(x => x.Number == item.Number && x.Profile == item.Profile && x.Length == item.Length && x.Father.Contains(assemID)).Select(x => x.Father).FirstOrDefault());
                            // Father的index = Part的index
                            var partIndex = fatherList.IndexOf(assemID);
                            // 取得該筆零件ID
                            int partID = partList[partIndex];

                            int count = assembliesItem.Item3.Intersect(fatherList).Count();
                            // 當父節點中還找的到父節點ID時，代表該零件尚存在其他構件中
                            while (fatherList.Contains(assemID))
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
                                // 由零件Father往回抓構件的ID List                                
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
            // 群組條件依照 構件編號、零件編號、型鋼類型、斷面規格
            List<ProductSettingsPageViewModel> source = new List<ProductSettingsPageViewModel>();
            source = steelAttrList.Where(x => allowType.Contains(x.Type)).ToList();
            var group = (from a in source
                             //group a by new { AsseNumber = a.steelAttr.AsseNumber, a.steelAttr.PartNumber, a.TeklaName, a.Type, a.Profile, a.Weight } into g
                         group a by new { AsseNumber = a.steelAttr.AsseNumber, a.steelAttr.PartNumber, a.Length, a.Type, a.Profile } into g
                         select new
                         {
                             AssemblyNumber = g.Key.AsseNumber,
                             PartNumber = g.Key.PartNumber,
                             Profile = g.Key.Profile,
                             Weight = g.FirstOrDefault().Weight,
                             Length = g.FirstOrDefault().Length,
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
                         }).OrderBy(x => x.Profile).ThenBy(x => x.PartNumber).ToList();
            List<ProductSettingsPageViewModel> list = new List<ProductSettingsPageViewModel>();

            Dictionary<string, ObservableCollection<SteelAttr>> saFile = ser.GetSteelAttr();
            SteelAttr temp = new SteelAttr();
            int ItemCount = 0;

            foreach (var item in group)
            {
                double Per = (ItemCount * 100) / group.Count;
                //ProcessingScreenWin.ViewModel.Status = $"正在讀取{item.PartNumber} - {ItemCount} / {group.Count}";
                //ProcessingScreenWin.ViewModel.Progress = Per;

                //ProfileType = item.SteelType;
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
                aa.steelAttr.Creation = item.Creation;
                aa.steelAttr.Revise = item.Revise;
                aa.steelAttr.GUID = item.DataName;
                aa.steelAttr.PartNumber = item.PartNumber;
                aa.steelAttr.H = item.H;
                aa.steelAttr.W = item.W;
                aa.steelAttr.AsseNumber = item.AssemblyNumber;
                aa.steelAttr.Type = (OBJECT_TYPE)item.SteelType;
                aa.steelAttr.Profile = item.Profile;
                aa.steelAttr.Material = item.Material;
                aa.steelAttr.Length = item.Length;
                aa.steelAttr.Name = item.TeklaName;
                aa.steelAttr.t1 = float.Parse(item.t1.ToString());
                aa.steelAttr.t2 = float.Parse(item.t2.ToString());
                aa.steelAttr.Title1 = item.Title1;
                aa.steelAttr.Title2 = item.Title2;
                aa.steelAttr.Phase = item.Phase;
                aa.steelAttr.ShippingNumber = item.ShippingNumber;
                aa.steelAttr.ExclamationMark = item.ExclamationMark == null ? false : item.ExclamationMark;

                if (NcSA[aa.steelAttr.Profile.GetHashCode().ToString() + ".lis"].Any())
                {
                    var NcSASingle = NcSA[aa.steelAttr.Profile.GetHashCode().ToString() + ".lis"].Where(x => x.PartNumber == aa.steelAttr.PartNumber).FirstOrDefault();
                    if (NcSASingle != null)
                    {
                        aa.steelAttr.Kg = NcSASingle.Kg;
                        aa.steelAttr.W = NcSASingle.W;
                        aa.oPoint = NcSASingle.oPoint;
                        aa.uPoint = NcSASingle.uPoint;
                        aa.vPoint = NcSASingle.vPoint;
                        aa.CutList = NcSASingle.CutList;
                        aa.steelAttr.oPoint = NcSASingle.oPoint;
                        aa.steelAttr.uPoint = NcSASingle.uPoint;
                        aa.steelAttr.vPoint = NcSASingle.vPoint;
                        aa.steelAttr.CutList = NcSASingle.CutList;
                    }
                }

                if (profileAll.SelectMany(x => x.Value).Any(x => x.Profile == item.Profile))
                {
                    aa.steelAttr.Kg = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == item.Profile).Kg;
                    aa.steelAttr.W = profileAll.SelectMany(x => x.Value).FirstOrDefault(x => x.Profile == item.Profile).W;
                }

                aa.Weight = ObSettingVM.PartWeight(aa, saFile); //item.Weight,
                aa.steelAttr.Weight = aa.Weight;

                list.Add(aa);
                ItemCount++;
            }
            // NC/BOM匯入後 建立 BOM表與零件之長度關係，修改長度後需比對此長度再與模型頂點進行計算求得新頂點座標
            if (!File.Exists(ApplicationVM.FileBomLengthList()))
            {
                ObservableCollection<SteelAttr> list1 = new ObservableCollection<SteelAttr>();
                if (list.Any() && !string.IsNullOrEmpty(list.FirstOrDefault().DataName))
                {
                    list1.AddRange(list.Select(x => new SteelAttr { GUID = Guid.Parse(x.DataName), Length = x.Length }).ToList());
                    ser.SetBomLengthList(list1);
                }
               
            }
            return list;
        }
        /// <summary>
        /// 計算單位重
        /// </summary>
        /// <param name="part_tekla"></param>
        /// <param name="saFile"></param>
        /// <returns></returns>
        public static double PartWeight(ProductSettingsPageViewModel part_tekla, Dictionary<string, ObservableCollection<SteelAttr>> saFile)
        {
            double weight;
            
                weight = (part_tekla.steelAttr.Length / 1000) * part_tekla.steelAttr.Kg;
           
            return weight;



            //var profile = saFile[((OBJECT_TYPE)part_tekla.SteelType).ToString()]
            //    .Where(x => x.Profile == part_tekla.Profile)
            //    .FirstOrDefault();
            //
            //if (profile != null)
            //{
            //    return (part_tekla.Length / 1000) * profile.Kg;
            //}
            //return part_tekla.Weight;
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
        /// <summary>
        /// 從model移除斜邊打點(BlockName)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RemoveType"></param>
        public void RemoveHypotenusePoint(devDept.Eyeshot.Model model, string RemoveType)
        {
            //List<GroupBoltsAttr> delList = model.Blocks
            //        .SelectMany(x => x.Entities)
            //        .Where(y =>
            //        y.GetType() == typeof(BlockReference) &&
            //        y.EntityData.GetType() == typeof(GroupBoltsAttr) &&
            //        //((GroupBoltsAttr)y.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
            //        ((GroupBoltsAttr)y.EntityData).BlockName == RemoveType)
            //        .Select(y => (GroupBoltsAttr)y.EntityData).ToList();
            //model.Blocks.SelectMany(x=>x.Entities.Select(y=>y.EntityData)).ForEach(x=>x.)

            List<Block> delList = model.Blocks
                .Where(x => x.Name != "RootBlock")
                .SelectMany(x =>
                x.Entities, (a, b) => new { Block = a, a.Entities, b.EntityData })
                .Where(x =>
                (x.Block.GetType()==typeof(Bolts3DBlock) || x.Block.GetType() == typeof(Block)) && x.EntityData != null &&
                x.EntityData.GetType() == typeof(BoltAttr) &&
                //((BoltAttr)x.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
                ((BoltAttr)x.EntityData).BlockName== RemoveType)
                .Select(x => x.Block).ToList();

            foreach (Block del in delList)
            {
                model.Blocks.Remove(del);
            }

            //List<Block> delList = new List<Block>();
            //for (int i = 1; i < model.Blocks.Count; i++)
            //{
            //    Block b = model.Blocks[i];
            //    for (int j = 0; j < model.Blocks[i].Entities.Count; j++)
            //    {
            //        var a = model.Blocks[i].Entities[j];
            //        if (a.EntityData is BoltAttr && ((BoltAttr)a.EntityData).BlockName == RemoveType)
            //        {
            //            delList.Add(b);
            //            break;
            //        }
            //    }
            //}


            var entitiesList = model.Entities
                    .Where(y => y.EntityData != null &&
                    y.EntityData.GetType() == typeof(GroupBoltsAttr) &&
                    //((GroupBoltsAttr)y.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
                    ((GroupBoltsAttr)y.EntityData).BlockName == RemoveType)
                    .Select(y => y).ToList();
            foreach (var entities in entitiesList)
            {
                model.Entities.Remove(entities);
            }




            
        }

        /// <summary>
        /// 取得斜邊打點清單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelAllBoltList"></param>
        public void GetHypotenusePoint(devDept.Eyeshot.Model model, List<GroupBoltsAttr> modelAllBoltList)
        {
            var hb = model.Blocks.SelectMany(x => x.Entities.Select(y => y.EntityData));
            foreach (GroupBoltsAttr item in hb.Where(x => x.GetType() == typeof(GroupBoltsAttr) && ((GroupBoltsAttr)x).Mode == AXIS_MODE.HypotenusePOINT))
            {
                modelAllBoltList.Add((GroupBoltsAttr)item);
            }
        }

        /// <summary>
        /// 加入2D/3D孔
        /// 1.取出非斜邊打點之孔位Entities
        /// 2.取得BlockName與Entities相同之Bock
        /// 3.建立3D/2D模型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="drawing"></param>
        /// <param name="checkRef"></param>
        /// <param name="blocks"></param>
        /// <param name="isAdd3D"></param>
        public void AddBolts(devDept.Eyeshot.Model model, devDept.Eyeshot.Model drawing,
            out bool checkRef, List<Block> blocks, bool isAdd3D = true, bool isRotate = true)
        {
            checkRef = true;

            #region 一般孔位
            for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr && boltsAttr.Mode != AXIS_MODE.HypotenusePOINT) //是螺栓
                {
                    // 指定加入孔位
                    if (blocks != null)
                    {
                        BlockReference blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
                                                                                            // 產生3D螺栓
                        int index = blocks.FindIndex(x => x.Name == blockReference1.BlockName);
                        if (index != -1)
                        {
                            Block block = blocks[index]; //取得圖塊
                            EntityList meshes = block.Entities;
                            Bolts3DBlock bolts3DBlock = new Bolts3DBlock(meshes, (GroupBoltsAttr)model.Entities[i].EntityData);
                            Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                        }
                        else
                        {
                            Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts(boltsAttr, model, out BlockReference blockRef, out checkRef, null, isRotate);
                            if (drawing != null)
                            {
                                Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖   
                            }
                        }
                    }
                    else
                    {
                        // 無指定孔位 則為一般加孔
                        BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                        Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
                        Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out checkRef, null, isRotate);
                        Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                    }
                }
            }
            #endregion

            #region 斜邊打點(HypotenuseAuto)
            //移除斜邊打點
            RemoveHypotenusePoint(model, HypotenuseTYPE.HypotenuseAuto.ToString());
            // 可建立自動打點 及 使用者自定義打點不存在，則可執行自動打點
            if (model.RunHypotenuseEnable() && !WPFSTD105.Model.Expand.isHypotenuseCustomerExist(model))
            {
                
                //執行斜邊打點3D
                WPFSTD105.Model.Expand.RunHypotenusePoint(model, this, 0);
                //取出斜邊打點之Block
                var HypotenuseBlock = model.Blocks.Where(x => x.Name != "RootBlock")
                    .SelectMany(x => x.Entities, (entities, entityData) => new { Block = entities, entities.Entities, entityData.EntityData })
                    .Where(x =>
                    x.Block.GetType() == typeof(Bolts3DBlock) &&
                    x.EntityData.GetType() == typeof(BoltAttr) &&
                    ((BoltAttr)x.EntityData).Mode == AXIS_MODE.HypotenusePOINT)
                    .ToList();
                //執行斜邊打點2D
                foreach (var item in HypotenuseBlock)
                {
                    //取出相同BlockName的Entities(For GroupBoltsAttr)
                    var entities = model.Entities.FirstOrDefault(x => 
                    x.EntityData.GetType() == typeof(GroupBoltsAttr) &&
                    ((GroupBoltsAttr)x.EntityData).GUID == Guid.Parse(item.Block.Name));

                    GroupBoltsAttr groupBoltsAttr = new GroupBoltsAttr();
                    if (entities != null)
                    {
                        groupBoltsAttr = (GroupBoltsAttr)entities.EntityData;
                    }
                    Bolts3DBlock bolts3DBlock = new Bolts3DBlock(item.Entities, groupBoltsAttr);
                    Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                }


                //for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
                //{
                //    if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr && boltsAttr.Mode == AXIS_MODE.HypotenusePOINT) //是螺栓
                //    {
                //        // 指定加入孔位
                //        if (blocks.Any())
                //        {
                //            BlockReference blockReference1 = (BlockReference)model.Entities[i]; //取得參考圖塊
                //                                                                                // 產生3D螺栓
                //            int index = blocks.FindIndex(x => x.Name == blockReference1.BlockName);
                //            if (index != -1)
                //            {
                //                Block block = blocks[index]; //取得圖塊
                //                EntityList meshes = block.Entities;
                //                Bolts3DBlock bolts3DBlock = new Bolts3DBlock(meshes, (GroupBoltsAttr)model.Entities[i].EntityData);
                //                Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                //            }
                //            else
                //            {
                //                // 無中生有
                //                Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts(boltsAttr, model, out BlockReference blockRef, out checkRef, null, isRotate);
                //                if (drawing != null)
                //                {
                //                    Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖   
                //                }
                //            }
                //        }
                //        else
                //        {
                //            // 無指定孔位 則為一般加孔
                //            BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                //            Block block = model.Blocks[blockReference.BlockName]; //取得圖塊
                //            Bolts3DBlock bolts3DBlock = Bolts3DBlock.AddBolts((GroupBoltsAttr)model.Entities[i].EntityData, model, out BlockReference blockRef, out checkRef, block.Entities, isRotate);
                //            Add2DHole(drawing, bolts3DBlock, false);//加入孔位不刷新 2d 視圖
                //        }
                //    }




                //    //                model.Blocks.Where(x => x.GetType() == typeof(Bolts3DBlock)).ForEach(a =>
                //    //{
                //    //    a.Entities.Where(x => x.EntityData.GetType() == typeof(BoltAttr) && ((BoltAttr)x.EntityData).Mode == AXIS_MODE.HypotenusePOINT).ForEach(b =>
                //    //    {
                //    //        BoltAttr bolt = (BoltAttr)b.EntityData;
                //    //        Add2DHole(drawing, (Bolts3DBlock)a, false);//加入孔位不刷新 2d 視圖 
                //    //    });
                //    //});
                //}
            }
            #endregion
            #region 檢查孔位
            checkRef = Bolts3DBlock.CheckBolts(model);
            #endregion

            model.Refresh();
            drawing.Refresh();
        }

        /// <summary>
        /// 重讀NC檔時，孔的GUID會重新給定，故比對孔群資訊(model與blocks)，
        /// </summary>
        /// <param name="model"></param>
        /// <param name="blocks">已編輯過的孔</param>
        /// <param name="groupBoltsAttr"></param>
        public static void UpdateNewGroupBoltsAttrGUID(devDept.Eyeshot.Model model, GroupBoltsAttr groupBoltsAttr)
        {
            // 若在Entities中找到此孔群資訊，取得原圖塊之GUID
            Guid? guid = null;
            for (int i = 0; i < model.Entities.Count; i++)
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr gba)
                {
                    if (gba.X == groupBoltsAttr.X && gba.Y == groupBoltsAttr.Y && gba.Z == groupBoltsAttr.Z && gba.Face == groupBoltsAttr.Face && gba.Mode == groupBoltsAttr.Mode)
                    {
                        guid = gba.GUID;
                        break;
                    }
                }
            }
            // 若新圖塊中有此GUID的話，更新groupBoltsAttr的GUID
            if (guid != null && model.Blocks[guid.Value.ToString()] != null)
            {
                groupBoltsAttr.GUID = guid;
            }




            //if (blocks != null && blocks.Select(x => x.Entities.Select(y=>y.EntityData))
            //    .Any(x => x.GetType() == typeof(GroupBoltsAttr) &&
            //    ((GroupBoltsAttr)x).X == groupBoltsAttr.X &&
            //    ((GroupBoltsAttr)x).Y == groupBoltsAttr.Y &&
            //    ((GroupBoltsAttr)x).Z == groupBoltsAttr.Z &&
            //    ((GroupBoltsAttr)x).Face == groupBoltsAttr.Face &&
            //    ((GroupBoltsAttr)x).Mode == groupBoltsAttr.Mode))
            //{
            //    blocks.ForEach(b =>
            //    {
            //        bool exist = false;
            //        b.Entities.ForEach(e =>
            //        {
            //            if (
            //            ((GroupBoltsAttr)e.EntityData).X == groupBoltsAttr.X &&
            //            ((GroupBoltsAttr)e.EntityData).Y == groupBoltsAttr.Y &&
            //            ((GroupBoltsAttr)e.EntityData).Z == groupBoltsAttr.Z &&
            //            ((GroupBoltsAttr)e.EntityData).Face == groupBoltsAttr.Face &&
            //            ((GroupBoltsAttr)e.EntityData).Mode == groupBoltsAttr.Mode)
            //            {
            //                if (groupBoltsAttr.Face == FACE.FRONT && groupBoltsAttr.Face == FACE.BACK)
            //                {
            //                    //groupBoltsAttr.Coordinates();
            //                    //((BoltAttr)e.EntityData).X = groupBoltsAttr.X;
            //                    //((BoltAttr)e.EntityData).Y = groupBoltsAttr.Y;
            //                    //((BoltAttr)e.EntityData).Z = groupBoltsAttr.Z;
            //                }
            //                exist = true;
            //                return;
            //            }
            //        });
            //        if (exist)
            //        {
            //            if (!model.Blocks.Any(x=>x.Name==b.Name))
            //            {
            //                // 新形鋼中 若不存在此孔/孔群 則將原型鋼的Block/Entities加入model
            //                model.Blocks.Add(b);
            //                BlockReference block = new BlockReference(0, 0, 0, b.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
            //                block.EntityData = b.Entities;
            //                block.Attributes.Add("Bolts", new AttributeReference(0, 0, 0));
            //                model.Entities.Insert(0, block);//加入參考圖塊到模型
            //            }
            //            //model.Entities.InsertRange(0, b.Entities);
            //            return;
            //        }
            //    });
            //}
            //else if (blocks != null && blocks.SelectMany(x => x.Entities).Any(x => x.EntityData.GetType() == typeof(BoltAttr) &&
            //    ((BoltAttr)x.EntityData).X == groupBoltsAttr.X &&
            //    ((BoltAttr)x.EntityData).Y == groupBoltsAttr.Y &&
            //    ((BoltAttr)x.EntityData).Z == groupBoltsAttr.Z &&
            //    ((BoltAttr)x.EntityData).Face == groupBoltsAttr.Face &&
            //    ((BoltAttr)x.EntityData).Mode == groupBoltsAttr.Mode))
            //{
            //    blocks.ForEach(b =>
            //    {
            //        bool exist = false;
            //        b.Entities.ForEach(e =>
            //        {
            //            if (e.EntityData.GetType() == typeof(BoltAttr) &&
            //            ((BoltAttr)e.EntityData).X == groupBoltsAttr.X &&
            //            ((BoltAttr)e.EntityData).Y == groupBoltsAttr.Y &&
            //            ((BoltAttr)e.EntityData).Z == groupBoltsAttr.Z &&
            //            ((BoltAttr)e.EntityData).Face == groupBoltsAttr.Face &&
            //            ((BoltAttr)e.EntityData).Mode == groupBoltsAttr.Mode)
            //            {
            //                if (groupBoltsAttr.Face == FACE.FRONT && groupBoltsAttr.Face == FACE.BACK)
            //                {
            //                   //groupBoltsAttr.Coordinates();
            //                   //((BoltAttr)e.EntityData).X = groupBoltsAttr.X;
            //                   //((BoltAttr)e.EntityData).Y = groupBoltsAttr.Y;
            //                   //((BoltAttr)e.EntityData).Z = groupBoltsAttr.Z;
            //                }
            //                exist = true; 
            //                return;
            //            }
            //        });
            //        if (exist)
            //        {
            //            if (!model.Blocks.Any(x => x.Name == b.Name))
            //            {
            //                // 舊有形鋼中 若存在此孔/孔群 則將Block/Entities加入model
            //                model.Blocks.Add(b);
            //                BlockReference block = new BlockReference(0, 0, 0, b.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
            //                block.EntityData = groupBoltsAttr;
            //                block.Attributes.Add("Bolts", new AttributeReference(0, 0, 0));
            //                model.Entities.Insert(0, block);//加入參考圖塊到模型
            //            }
            //            //model.Entities.InsertRange(0, (BlockReference)b.Entities);
            //            return;
            //        }
            //    });
            //}
            //else if (blocks != null && blocks.SelectMany(x => x.Entities).Select(x => (Mesh)x).Select(x => (BoltAttr)x.EntityData)
            //    .Any(x => x.GetType() == typeof(BoltAttr) &&
            //    ((BoltAttr)x).X == groupBoltsAttr.X &&
            //    ((BoltAttr)x).Y == groupBoltsAttr.Y &&
            //    ((BoltAttr)x).Z == groupBoltsAttr.Z &&
            //    ((BoltAttr)x).Face == groupBoltsAttr.Face &&
            //    ((BoltAttr)x).Mode == groupBoltsAttr.Mode))
            //{
            //    blocks.ForEach(b =>
            //    {
            //        bool exist = false;
            //        b.Entities.ForEach(e =>
            //        {
            //            if (e.EntityData.GetType() == typeof(BoltAttr) &&
            //            ((BoltAttr)e.EntityData).X == groupBoltsAttr.X &&
            //            ((BoltAttr)e.EntityData).Y == groupBoltsAttr.Y &&
            //            ((BoltAttr)e.EntityData).Z == groupBoltsAttr.Z &&
            //            ((BoltAttr)e.EntityData).Face == groupBoltsAttr.Face &&
            //            ((BoltAttr)e.EntityData).Mode == groupBoltsAttr.Mode)
            //            {
            //                if (groupBoltsAttr.Face == FACE.FRONT && groupBoltsAttr.Face == FACE.BACK)
            //                {
            //                    //groupBoltsAttr.Coordinates();
            //                    //((BoltAttr)e.EntityData).X = groupBoltsAttr.X;
            //                    //((BoltAttr)e.EntityData).Y = groupBoltsAttr.Y;
            //                    //((BoltAttr)e.EntityData).Z = groupBoltsAttr.Z;
            //                }
            //                exist = true;
            //                return;
            //            }
            //        });
            //        if (exist)
            //        {
            //            if (!model.Blocks.Any(x => x.Name == b.Name))
            //            {
            //                // 舊有形鋼中 若存在此孔/孔群 則將Block/Entities加入model
            //                model.Blocks.Add(b);
            //                BlockReference block = new BlockReference(0, 0, 0, b.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
            //                block.EntityData = groupBoltsAttr;
            //                block.Attributes.Add("Bolts", new AttributeReference(0, 0, 0));
            //                model.Entities.Insert(0, block);//加入參考圖塊到模型
            //            }
            //            //model.Entities.InsertRange(0, b.Entities);
            //            return;
            //        }
            //    });
            //}
            //else
            //{
            //    Bolts3DBlock bolts = new Bolts3DBlock(groupBoltsAttr);
            //    Bolts3DBlock.AddBolts(groupBoltsAttr, model, out BlockReference blockReference1, out bool check,meshes, isRotate);
            //}
        }

        /// <summary>
        /// 加入2d 孔位
        /// </summary>
        /// <param name="drawing">2D畫布</param>
        /// <param name="bolts">3D螺栓</param>
        /// <param name="refresh">是否更新2D畫布</param>
        /// <returns></returns>
        public BlockReference Add2DHole(devDept.Eyeshot.Model drawing, Bolts3DBlock bolts, bool refresh = true)
        {
            try
            {
                /*2D螺栓*/
                BlockReference referenceMain = (BlockReference)drawing.Entities[drawing.Entities.Count - 1]; //主件圖形
                                                                                                             //BlockReference referenceMain = (BlockReference)drawing.Entities.Where(x=>x is BlockReference).LastOrDefault(); //主件圖形
                Steel2DBlock steel2DBlock = (Steel2DBlock)drawing.Blocks[referenceMain.BlockName]; //取得鋼構圖塊
#if DEBUG
                log4net.LogManager.GetLogger($"產生 {bolts.Name} 2D螺栓圖塊").Debug($"開始");

#endif
                string blockName = string.Empty; //圖塊名稱
#if DEBUG
                //log4net.LogManager.GetLogger($"產生2D螺栓圖塊").Debug($"開始");
#endif

                Bolts2DBlock bolts2DBlock = new Bolts2DBlock(bolts, steel2DBlock); //產生螺栓圖塊
#if DEBUG
                log4net.LogManager.GetLogger($"產生2D螺栓圖塊").Debug($"結束");
                log4net.LogManager.GetLogger($"2D畫布加入螺栓圖塊").Debug($"");
#endif
                bolts2DBlock.Entities.Regen();
                //if (drawing.Blocks.Any(x => x.Name == bolts2DBlock.Name))
                //{
                //    var a = drawing.Blocks.FirstOrDefault(x => x.Name == bolts2DBlock.Name);
                //    a = bolts2DBlock;
                //}
                //else {
                drawing.Blocks.Add(bolts2DBlock); //加入螺栓圖塊
                //}
                foreach (var block in drawing.Blocks)
                {
                    block.Entities.Regen();
                }
                blockName = bolts2DBlock.Name;
                BlockReference result = new BlockReference(0, 0, 0, bolts2DBlock.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
                                                                                                   // 將孔位加入到TOP FRONT BACK圖塊中
                drawing.Entities.Insert(0, result);

#if DEBUG
                log4net.LogManager.GetLogger($"2D畫布加入TOP FRONT BACK圖塊").Debug($"");
                log4net.LogManager.GetLogger($"產生 {bolts.Name} 2D螺栓圖塊").Debug($"結束");
#endif

                if (refresh)
                {
                    drawing.Entities.Regen();
                    drawing.Refresh();//刷新模型
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddSteel(ModelExt model, SteelAttr steelAttr)
        {
            Steel3DBlock steel = Steel3DBlock.AddSteel(steelAttr, model, out BlockReference blockReference);
            BlockReference steel2D = SteelTriangulation(model, model.Blocks[1].Name, ((Mesh)steel.Entities[0]));
            Reductions.Add(new Reduction()
            {
                Recycle = new List<List<Entity>>() { new List<Entity>() { blockReference } },
                SelectReference = null,
                User = new List<ACTION_USER>() { ACTION_USER.Add }
            }, new Reduction()
            {
                // 2022.06.24 呂宗霖 還原註解
                Recycle = new List<List<Entity>>() { new List<Entity>() { steel2D } },
                SelectReference = null,
                User = new List<ACTION_USER>() { ACTION_USER.Add }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawing">2D視圖</param>
        /// <param name="blockName">圖塊名稱</param>
        /// <param name="mesh">型鋼mesh((Mesh)model.Blocks[1].Entities[0])</param>
        /// <returns></returns>
        public BlockReference SteelTriangulation(devDept.Eyeshot.Model drawing, string blockName, Mesh mesh)
        {
#if DEBUG
            log4net.LogManager.GetLogger("SteelTriangulation").Debug("");
            log4net.LogManager.GetLogger($"產生2D圖塊(TOP.FRONT.BACK)").Debug($"開始");
#endif
            //drawing.Blocks.Clear();
            //drawing.Entities.Clear();
            drawing.Clear();

            // 產生2D圖塊
            Steel2DBlock steel2DBlock = new Steel2DBlock(mesh, blockName);
            drawing.Blocks.Add(steel2DBlock);
            BlockReference block2D = new BlockReference(0, 0, 0, steel2DBlock.Name, 1, 1, 1, 0);//產生鋼構參考圖塊
                                                                                                //關閉三視圖用戶選擇
            block2D.Selectable = false;

            // 將TOP FRONT BACK圖塊加入drawing
            drawing.Entities.Add(block2D);
            //drawing.Entities.Add(steel2DBlock.Steel);
#if DEBUG
            log4net.LogManager.GetLogger("產生2D圖塊(TOP.FRONT.BACK)").Debug("結束");
#endif
            drawing.ZoomFit();//設置道適合的視口
            drawing.Refresh();//刷新模型
            return block2D;
        }
        /// <summary>
        /// 複製NC檔，取代舊零件編號為新零件編號
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldPartNumber"></param>
        /// <param name="newPartNumber"></param>
        public void CopyNCFile(string path, string oldPartNumber, string newPartNumber)
        {
            string dataName = Path.GetFileName($"{path}");//檔案名稱
            if (File.Exists($@"{path}")) //檔案存在
            {
                //File.AppendAllText($@"{allPath}", File.ReadAllText($@"{ApplicationVM.DirectoryNc()}\{ViewModel.SteelAttr.PartNumber}.nc1")); //將正本寫入到副本內
                string text = File.ReadAllText(path, System.Text.Encoding.Default);
                text = text.Replace(oldPartNumber, newPartNumber);
                // 檔名為新零件
                File.WriteAllText($@"{ApplicationVM.DirectoryNc()}\{newPartNumber}.nc1", text, System.Text.Encoding.Default);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="steelAttr"></param>
        /// <param name="groups"></param>
        /// <param name="newGUID"></param>
        /// <returns>與第一個參數的SteelAttr不同</returns>
        public SteelAttr ReadNCInfo(SteelAttr steelAttr, ref List<GroupBoltsAttr> groups, bool newGUID = true)
        {
            STDSerialization ser = new STDSerialization();
            var profile = ser.GetSteelAttr();
            TeklaNcFactory t = new TeklaNcFactory();
            Steel3DBlock s3Db = new Steel3DBlock();
            SteelAttr steelAttrNC = new SteelAttr();
            //List<GroupBoltsAttr> groups = new List<GroupBoltsAttr>();
            SteelAttr saDeepClone = (SteelAttr)steelAttr.DeepClone();
            s3Db.ReadNcFile($@"{ApplicationVM.DirectoryNc()}\{steelAttr.PartNumber}.nc1", profile, steelAttr, ref steelAttrNC, ref groups);
            if (newGUID)
            {
                saDeepClone.GUID = Guid.NewGuid();
            }
            saDeepClone.oPoint = steelAttrNC.oPoint;
            saDeepClone.vPoint = steelAttrNC.vPoint;
            saDeepClone.uPoint = steelAttrNC.uPoint;
            saDeepClone.CutList = steelAttrNC.CutList;
            return saDeepClone;
        }

        public static void SaveErrorString(string FileName, string text)
        {
            FileStream fs = new FileStream($@"{ApplicationVM.DirectoryModel()}\{FileName}.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(text);
            sw.Flush();
            sw.Close();
        }

        public bool CheckData_AddCutLine(String partNumber, ModelExt model)
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();

            //if (!part.Any(x => x.Value.Any(y => y.Number == partNumber)) && showMessage)
            if (CheckOption_IsNotOfficialPart(part, partNumber) && showMessage)
            {
                WinUIMessageBox.Show(null,
               $"零件編號{partNumber}尚未點擊OK",
               "通知",
               MessageBoxButton.OK,
               MessageBoxImage.Exclamation,
               MessageBoxResult.None,
               MessageBoxOptions.None,
                FloatingMode.Window);
                fNewPart = true;
                fclickOK = false;
                return false;
            }

            //if (model.Entities.Count <= 0 && showMessage)
            if (CheckOption_IsThereNotAnyEntities(model) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"模型內找不到主件",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            //if (part.Values.SelectMany(x => x).Where(x => x.Number == this.PartNumberProperty && x.Match.Where(y => y == false).Count() > 0).Count() > 0 && showMessage)
            if (CheckOption_IsPartTypesetting(part, this.PartNumberProperty) && showMessage)
            {
                WinUIMessageBox.Show(null,
              $"零件已排版，不可編輯",
              "通知",
              MessageBoxButton.OK,
              MessageBoxImage.Exclamation,
              MessageBoxResult.None,
              MessageBoxOptions.None,
               FloatingMode.Window);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="model"></param>
        /// <returns>fasle不可繼續</returns>
        public bool CheckData_AddHole(String partNumber, ModelExt model)
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();

            //if (!part.Any(x => x.Value.Any(y => y.Number == partNumber)) && showMessage)
            if (CheckOption_IsNotOfficialPart(part, partNumber) && showMessage)
            {
                WinUIMessageBox.Show(null,
               $"零件編號{partNumber}尚未點擊OK",
               "通知",
               MessageBoxButton.OK,
               MessageBoxImage.Exclamation,
               MessageBoxResult.None,
               MessageBoxOptions.None,
                FloatingMode.Window);
                fNewPart = true;
                fclickOK = false;
                return false;
            }

            //if (model.Entities.Count <= 0 && showMessage)
            if (CheckOption_IsThereNotAnyEntities(model) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"模型內找不到主件",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            if (ComparisonBolts(model) && showMessage)  // 欲新增孔位重複比對
            {
                WinUIMessageBox.Show(null,
                $"新增孔位重複，請重新輸入",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fclickOK = true;
                return false;
            }

            //if (part.Values.SelectMany(x => x).Where(x => x.Number == this.PartNumberProperty && x.Match.Where(y => y == false).Count() > 0).Count() > 0 && showMessage)
            if (CheckOption_IsPartTypesetting(part, this.PartNumberProperty) && showMessage)
            {
                WinUIMessageBox.Show(null,
              $"零件已排版，不可編輯",
              "通知",
              MessageBoxButton.OK,
              MessageBoxImage.Exclamation,
              MessageBoxResult.None,
              MessageBoxOptions.None,
               FloatingMode.Window);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="model"></param>
        /// <param name="selected3DItem"></param>
        /// <returns>fasle不可繼續</returns>
        public bool CheckData_ModifyHole(String partNumber, ModelExt model, List<SelectedItem> selected3DItem)
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();

            //if (model.Entities.Count <= 0 && showMessage)
            if (CheckOption_IsThereNotAnyEntities(model) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"模型內找不到主件",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            //if (!part.Any(x => x.Value.Any(y => y.Number == partNumber)) && showMessage)
            if (CheckOption_IsNotOfficialPart(part, partNumber) && showMessage)
            {
                WinUIMessageBox.Show(null,
               $"零件編號{partNumber}尚未點擊OK",
               "通知",
               MessageBoxButton.OK,
               MessageBoxImage.Exclamation,
               MessageBoxResult.None,
               MessageBoxOptions.None,
                FloatingMode.Window);
                fNewPart = true;
                fclickOK = false;
                return false;
            }

            if (CheckOption_IsNotSelected(selected3DItem) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"請選擇孔，才可修改",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            //if (part.Values.SelectMany(x => x).Where(x => x.Number == this.PartNumberProperty && x.Match.Where(y => y == false).Count() > 0).Count() > 0 && showMessage)
            if (CheckOption_IsPartTypesetting(part, this.PartNumberProperty) && showMessage)
            {
                WinUIMessageBox.Show(null,
              $"零件已排版，不可編輯",
              "通知",
              MessageBoxButton.OK,
              MessageBoxImage.Exclamation,
              MessageBoxResult.None,
              MessageBoxOptions.None,
               FloatingMode.Window);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="model"></param>
        /// <returns>fasle不可繼續</returns>
        public bool CheckData_DelHole(String partNumber, ModelExt model, List<SelectedItem> selected3DItem)
        {
            STDSerialization ser = new STDSerialization();
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();

            //if (model.Entities.Count <= 0 && showMessage)
            if (CheckOption_IsThereNotAnyEntities(model) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"模型內找不到主件",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            //if (!part.Any(x => x.Value.Any(y => y.Number == partNumber)) && showMessage)
            if (CheckOption_IsNotOfficialPart(part, partNumber) && showMessage)
            {
                WinUIMessageBox.Show(null,
                    $"零件編號{partNumber}尚未點擊OK",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Window);
                fNewPart = true;
                fclickOK = false;
                return false;
            }            

            if (CheckOption_IsNotSelected(selected3DItem) && showMessage)
            {
                WinUIMessageBox.Show(null,
                $"請選擇孔，才可刪除",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                 FloatingMode.Window);
                fNewPart = false;
                fclickOK = false;
                return false;
            }

            //if ((part.Values.SelectMany(x => x).Where(x => x.Number == this.PartNumberProperty && x.Match.Where(y => y == false).Count() > 0).Count() > 0) && showMessage)
            if (CheckOption_IsPartTypesetting(part, this.PartNumberProperty) && showMessage)
            {
                WinUIMessageBox.Show(null,
              $"零件已排版，不可編輯",
              "通知",
              MessageBoxButton.OK,
              MessageBoxImage.Exclamation,
              MessageBoxResult.None,
              MessageBoxOptions.None,
               FloatingMode.Window);
                return false;
            }
            return true;
        }

        #region 是否為正式零件
        /// <summary>
        /// 是否為非正式零件
        /// </summary>
        /// <param name="part"></param>
        /// <param name="partNumber"></param>
        /// <returns>true 非正式零件</returns>
        public bool CheckOption_IsNotOfficialPart(Dictionary<string, ObservableCollection<SteelPart>> part,string partNumber) 
        {
            return !(part.Any(x => x.Value.Any(y => y.Number == partNumber)));
        }
        #endregion
        #region 模型是否無主件
        /// <summary>
        /// 模型是否無主件
        /// </summary>
        /// <param name="part"></param>
        /// <param name="partNumber"></param>
        /// <returns></returns>
        public bool CheckOption_IsThereNotAnyEntities(ModelExt model)
        {
            return model.Entities.Count <= 0;
        }
        #endregion
        #region 零件是否已排版
        /// <summary>
        /// 零件是否已排版
        /// </summary>
        /// <param name="part"></param>
        /// <param name="partNumber"></param>
        /// <returns></returns>
        public bool CheckOption_IsPartTypesetting(Dictionary<string, ObservableCollection<SteelPart>> part, string partNumber)
        {
            return (part.Values.SelectMany(x => x).Where(x => x.Number == this.PartNumberProperty && x.Match.Where(y => y == false).Count() > 0).Count() > 0);
        }
        #endregion
        #region 是否已選擇
        /// <summary>
        /// 是否已選擇
        /// </summary>
        /// <param name="Selected3DItem"></param>
        /// <returns></returns>
        public bool CheckOption_IsNotSelected(List<SelectedItem> Selected3DItem)
        {
            return (Selected3DItem.Count() <= 0);
        }
        #endregion










        /// <summary>
        /// 新增孔位比對
        /// </summary>
        public bool ComparisonBolts(ModelExt model)
        {
            GroupBoltsAttr TmpBoltsArr = new GroupBoltsAttr();
            TmpBoltsArr = GetGroupBoltsAttr();
            double valueX = 0d;
            double valueY = 0d;
            double TmpXPos = 0d;
            double TmpYPos = 0d;
            bool bFindSamePos = false;
            List<(double, double)> AddBoltsList = new List<(double, double)>();

            TmpXPos = TmpBoltsArr.X;
            TmpYPos = TmpBoltsArr.Y;

            // 分解與儲存預建立之孔群各孔座標於LIST
            for (int i = 1; i <= TmpBoltsArr.xCount; i++)
            {
                AddBoltsList.Add((TmpXPos, TmpYPos));

                for (int j = 1; j < TmpBoltsArr.yCount; j++)
                {
                    if (j < TmpBoltsArr.dYs.Count) //判斷孔位Y向矩陣列表是否有超出長度,超過都取最後一位偏移量
                        valueY = TmpBoltsArr.dYs[j - 1];
                    else
                        valueY = TmpBoltsArr.dYs[TmpBoltsArr.dYs.Count - 1];

                    TmpYPos = TmpYPos + valueY;

                    AddBoltsList.Add((TmpXPos, TmpYPos));
                }

                if (i < TmpBoltsArr.dXs.Count) //判斷孔位X向矩陣列表是否有超出長度,超過都取最後一位偏移量
                    valueX = TmpBoltsArr.dXs[i - 1];
                else
                    valueX = TmpBoltsArr.dXs[TmpBoltsArr.dXs.Count - 1];

                TmpXPos = TmpXPos + valueX;

                TmpYPos = TmpBoltsArr.Y;
            }
            TmpXPos = 0d;
            TmpYPos = 0d;

            // 原3D模組各孔位座標存於各LIST
            List<(double, double)> AllBoltsAddList = new List<(double, double)>();
            List<(double, double)> TopBoltsAddList = new List<(double, double)>();
            List<(double, double)> FRONTBoltsAddList = new List<(double, double)>();
            List<(double, double)> BACKBoltsAddList = new List<(double, double)>();

            for (int i = 0; i < model.Entities.Count; i++)//逐步展開孔群資訊
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //判斷孔群
                {
                    BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    Block block = model.Blocks[blockReference.BlockName]; //取得圖塊 
                    Bolts3DBlock bolts3DBlock = new Bolts3DBlock(block.Entities, (GroupBoltsAttr)blockReference.EntityData); //產生孔群圖塊

                    for (int j = 0; j < bolts3DBlock.Entities.Count; j++)
                    {
                        TmpXPos = ((BoltAttr)bolts3DBlock.Entities[j].EntityData).X;
                        TmpYPos = ((BoltAttr)bolts3DBlock.Entities[j].EntityData).Y;

                        switch (boltsAttr.Face)
                        {
                            case GD_STD.Enum.FACE.TOP:
                                TopBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            case GD_STD.Enum.FACE.FRONT:
                                FRONTBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            case GD_STD.Enum.FACE.BACK:
                                BACKBoltsAddList.Add((TmpXPos, TmpYPos));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // 將原3D各孔群座標存於共用LIST
            switch (TmpBoltsArr.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    AllBoltsAddList = TopBoltsAddList;
                    break;
                case GD_STD.Enum.FACE.FRONT:
                    AllBoltsAddList = FRONTBoltsAddList;
                    break;
                case GD_STD.Enum.FACE.BACK:
                    AllBoltsAddList = BACKBoltsAddList;
                    break;
                default:
                    break;
            }

            // 指定LIST比對是否有相同座標
            foreach (var x in AddBoltsList)
            {
                if (AllBoltsAddList.Contains(x))
                {
                    bFindSamePos = true;
                    break;
                }
                else
                    bFindSamePos = false;
            }
            return bFindSamePos;
        }

        #region 功能可用時機判斷參數
        /// <summary>
        /// 是否第一次按新增 點擊OK true
        /// </summary>
        public bool? fFirstAdd = true;
        /// <summary>
        /// 是否為新零件 dm
        /// </summary>
        public bool? fNewPart = true;
        /// <summary>
        /// 是否點擊Grid 新增 修改後為false
        /// </summary>
        public bool? fGrid = false;// 
        /// <summary>
        /// 是否直接點擊OK
        /// </summary>
        public bool? fclickOK = true; // 
        /// <summary>
        /// 判斷執行新增零件及孔位
        /// </summary>
        public bool fAddSteelPart = false;       //  
        /// <summary>
        /// 判斷執行斜邊打點
        /// </summary>
        public bool fAddHypotenusePoint = false;   //  
        /// <summary>
        /// 修改螺栓狀態
        /// </summary>
        public bool modifyHole { get; set; } = false;
        #endregion

        /// <summary>
        /// 流程參數設定(全null為初始值)
        /// </summary>
        /// <param name="firstAdd">是否第一次按新增</param>
        /// <param name="newPart">是否為新零件</param>
        /// <param name="grid">是否從Grid開始動作</param>
        public void StateParaSetting(bool? firstAdd, bool? newPart, bool? grid)
        {
            // 初始值
            if (firstAdd == null && newPart == null && grid == null)
            {
                fFirstAdd = true;
                fNewPart = true;
                fGrid = false;
            }
            else
            {
                // 第一次按新增
                fFirstAdd = firstAdd;
                // 是否為新零件
                fNewPart = newPart;
                // 是否在Grid進行動作
                fGrid = grid;
            }
        }





    }
}