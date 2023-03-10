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
        #endregion

        #region 公開屬性
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
        /// 選擇物件的面的功能開啟
        /// </summary>
        public bool CheckFace { get; set; } = true;
        /// <summary>
        /// 儲存的序列化檔案資料
        /// </summary>
        public ObservableCollection<DataCorrespond> DataCorrespond { get; set; } = new ObservableCollection<DataCorrespond>();
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
        ///  選擇的斷面規格類型 <see cref="OBJETC_TYPE"/>索引值
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
                    OBJETC_TYPE TYPE = (OBJETC_TYPE)value;
#if DEBUG
                    log4net.LogManager.GetLogger("載入斷面規格").Debug(TYPE.ToString());
#endif
                    switch (TYPE)
                    {
                        case OBJETC_TYPE.RH:
                        case OBJETC_TYPE.CH:
                        case OBJETC_TYPE.L:
                        case OBJETC_TYPE.BOX:
                        case OBJETC_TYPE.BH:
                            ProfileList = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\{ (TYPE).ToString()}.inp");
                            break;
                        default:
                            throw new Exception($"找不到{ TYPE.ToString() }");
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

                SteelAttr steelAttr;
                if (value == -1)
                    steelAttr = ProfileList[0];
                else
                    steelAttr = ProfileList[value];

                steelAttr.GUID = SteelAttr.GUID;
                steelAttr.AsseNumber = SteelAttr.AsseNumber;
                steelAttr.Length = SteelAttr.Length;
                steelAttr.Material = SteelAttr.Material;
                steelAttr.Number = SteelAttr.Number;
                steelAttr.PartNumber = SteelAttr.PartNumber;
                SteelAttr = (SteelAttr)steelAttr.DeepClone();

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
                SteelAttr.Material = Materials[value].Name;
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
                Profile = SteelAttr.Profile
            };
            bool save = (from el in new List<DataCorrespond>(DataCorrespond) where el.DataName == data.DataName select el).ToList().Count == 0;
            if (save)
            {
                DataCorrespond.Add(data);
                //SerializationHelper.SerializeBinary(DataCorrespond, ApplicationVM.FilePartList());
                STDSerialization ser = new STDSerialization();
                ser.SetDataCorrespond(DataCorrespond);
                AddNode(data);
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
        /// 取得設定好的值
        /// </summary>
        public GroupBoltsAttr GetGroupBoltsAttr()
        {
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
                Boltsbuffer.Face = (GD_STD.Enum.FACE)BoltsFaceType;
            }
            //double value = 0d;
            if (CheckStartHole)
            {
                //目前座標是2D座標只是需要先判斷 X Y 座標
                switch (Boltsbuffer.Face)
                {
                    case FACE.TOP:
                        Boltsbuffer.t = Steelbuffer.t1;
                        //斷面規格類型
                        switch (Steelbuffer.Type)
                        {
                            case OBJETC_TYPE.BH:
                            case OBJETC_TYPE.RH:
                                Boltsbuffer.Z = Steelbuffer.W * 0.5 - Steelbuffer.t1 * 0.5;
                                break;
                            case OBJETC_TYPE.BOX:
                            case OBJETC_TYPE.CH:
                                Boltsbuffer.Z = Steelbuffer.W - Steelbuffer.t1;
                                break;
                            case OBJETC_TYPE.L:
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
            for (int i = 0; i < ProfileList.Count; i++)
                if (ProfileList[i].Profile == steelAttr.Profile)
                {
                    ProfileIndex = i;
                    break;
                }
            MaterialIndex = Materials.IndexOf(el => el.Name == steelAttr.Material);

        }
        /// <summary>
        /// 寫入切割設定 To VM
        /// </summary>
        /// <param name="steelAttr">主件設定檔</param>
        public void WriteCutAttr(SteelAttr steelAttr)
        {
            switch ((GD_STD.Enum.FACE)CutFaceType)
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
            try
            {
                Steelbuffer = (SteelAttr)SteelAttr.DeepClone();
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
                CutList bufferCutList = null;
                switch ((GD_STD.Enum.FACE)CutFaceType)
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
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public ObSettingVM()
        {
            //Reductions = new ReductionList(Model);
            //TODO:可以設定 X Y Z 軸向顏色
            //初始化數據綁定的坐標系圖標。
            CoordinateSystemIcon = new CoordinateSystemIcon();

            //初始化數據綁定的原始符號。
            OriginSymbols = new ObservableCollection<OriginSymbol>(new List<OriginSymbol>() { OriginSymbol.GetDefaultOriginSymbol() });

            //如果模型有材質設定
            if (!File.Exists(ApplicationVM.FileMaterial()))
            {
                Materials.AddRange(SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(ApplicationVM.FileMaterial())); //材質序列化檔案
            }

            ObservableCollection<SteelMaterial> _ = SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(@"Mater.lis"); //材質序列化檔案
            Materials.AddRange(_);
            LoadAttribute();//載入用戶屬性

            //初始化選單
            ProfileType = 0;
            ProfileIndex = 0;

            DataCorrespond = SerializationHelper.GZipDeserialize<ObservableCollection<DataCorrespond>>(ApplicationVM.FilePartList());

            var groupData = from el in DataCorrespond group el by el.Type.GetType().GetMember(el.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description into el orderby el.Key select el;
            for (int i = 0; i < DataCorrespond.Count; i++)
            {
                AddNode(DataCorrespond[i]);
            }

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
        /// 選擇的斷面規格類型 <see cref="OBJETC_TYPE"/>索引值
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
        /// <summary>
        /// 加入節點
        /// </summary>
        public void AddNode(DataCorrespond data)
        {
            string level1Key = data.Type.GetType().GetMember(data.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description; //第一層要設置的 key 值
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
                MessageBox.Show("切割線 Y 大於翼板寬度，請重新輸入。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return false;
            }
            else if (bufferCutList.UL.X + bufferCutList.UR.X > SteelAttr.Length || bufferCutList.DL.X + bufferCutList.DR.X > SteelAttr.Length)
            {
                MessageBox.Show("切割線大於物件長度寬度，請重新輸入。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
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
        #endregion
    }
}
