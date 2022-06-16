using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    /// <summary>
    /// 等待加工料件
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>新增<see cref="Material"/></item>
    ///             <item>新增<see cref="H"/></item>
    ///             <item>新增<see cref="W"/></item>
    ///             <item>新增<see cref="t1"/></item>
    ///             <item>新增<see cref="t2"/></item>
    ///             <item>新增<see cref="BoltsCountL"/></item>
    ///             <item>新增<see cref="BoltsCountM"/></item>
    ///             <item>新增<see cref="BoltsCountR"/></item>
    ///             <item>新增<see cref="IndexBoltsL"/></item>
    ///             <item>新增<see cref="IndexBoltsM"/></item>
    ///             <item>新增<see cref="IndexBoltsR"/></item>
    ///             <item>新增<see cref="ProfileType"/></item>
    ///             <item>新增<see cref="Stamp"/></item>
    ///             <item>新增<see cref="StampData"/></item>
    ///             <item>新增<see cref="DrMiddle"/></item>
    ///             <item>新增<see cref="DrRight"/></item>
    ///             <item>新增<see cref="DrLeft"/></item>
    ///             <item>新增<see cref="GUID"/></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    [Serializable]
    [DataContract]
    public struct WorkMaterial
    {
        /// <summary>
        /// 初始化結構
        /// </summary>
        /// <returns></returns>
        public static WorkMaterial Initialization()
        {
            WorkMaterial result = new WorkMaterial(null);
            result.MaterialNumber = new char[result.ArrayLength(nameof(MaterialNumber))];
            result.Profile = new char[result.ArrayLength(nameof(Profile))];
            result.PartNumber = new char[result.ArrayLength(nameof(PartNumber))];
            result.SmeltingNumber = new char[result.ArrayLength(nameof(SmeltingNumber))];
            result.ID = new char[result.ArrayLength(nameof(ID))];
            result.AssemblyNumber = new char[result.ArrayLength(nameof(AssemblyNumber))];
            result.Source = new char[result.ArrayLength(nameof(Source))];
            return result;
        }
        /// <summary>
        /// 標準結構
        /// </summary>
        /// <param name="materialNumber">素材編號</param>
        /// <param name="profile">斷面</param>
        /// <param name="partNumber">零件編號</param>
        /// <param name="length">長度</param>
        /// <param name="finish">素材完成進度</param>
        /// <param name="material">材質</param> 
        /// <param name="id">素材ID</param>
        /// <param name="assemblyNumber">構件編號</param>
        /// <param name="source">廠商</param>
        /// <param name="smeltingNumber">熔煉號 (爐號)</param>
        /// <param name="h">高度</param>
        /// <param name="w">寬度</param>
        /// <param name="t1">腹板厚度</param>
        /// <param name="t2">翼板厚度</param>
        /// <param name="boltsCountL">左軸加工孔位的陣列長度</param>
        /// <param name="boltsCountM">中間軸加工孔位的陣列長度</param>
        /// <param name="boltsCountR">右軸加工孔位的陣列長度</param>
        /// <param name="indexBoltsL">左軸加工的陣列起始位置</param>
        /// <param name="indexBoltsM">中間軸加工的陣列起始位置</param>
        /// <param name="indexBoltsR">右軸加工的陣列起始位置</param>
        /// <param name="profileType">斷面規格類型</param>
        /// <param name="stamp">執行敲鋼印</param>
        /// <param name="stampData">鋼印資料param>
        /// <param name="drMiddle">中軸加工訊息</param>
        /// <param name="drRight">右軸加工訊息</param>
        /// <param name="drLeft">左軸加工訊息</param>
        /// <param name="gUID">圖面 GUID </param>
        private WorkMaterial(char[] materialNumber = null, char[] profile = null, char[] partNumber = null,
            double length = 0, ushort finish = 0, MATERIAL material = default, char[] assemblyNumber = null,
            char[] id = null, char[] source = null, char[] smeltingNumber = null, float h = 0, float w = 0,
            float t1 = 0, float t2 = 0, short boltsCountL = 0, short boltsCountM = 0, short boltsCountR = 0,
            short indexBoltsL = 0, short indexBoltsM = 0, short indexBoltsR = 0, PROFILE_TYPE profileType = default,
            bool stamp = false, Stamp[] stampData = null, Drill[] drMiddle = null, Drill[] drRight = null, Drill[] drLeft = null, char[] gUID = null)
        {
            WorkMaterial result = Initialization();
            if (materialNumber != null)
                if (materialNumber.Length == result.MaterialNumber.Length)
                    result.MaterialNumber = materialNumber;
                else
                    throw new MemoryException($"set 失敗，{nameof(MaterialNumber)} 數量要等於 {result.MaterialNumber.Length}");

            if (profile != null)
                if (profile.Length == result.Profile.Length)
                    result.Profile = profile;
                else
                    throw new MemoryException($"set 失敗，{nameof(Profile)} 數量要等於 {result.Profile.Length}");

            if (partNumber != null)
                if (partNumber.Length == result.PartNumber.Length)
                    result.PartNumber = partNumber;
                else
                    throw new MemoryException($"set 失敗，{nameof(PartNumber)} 數量要等於 {result.PartNumber.Length}");

            if (smeltingNumber != null)
                if (smeltingNumber.Length == result.SmeltingNumber.Length)
                    result.SmeltingNumber = smeltingNumber;
                else
                    throw new MemoryException($"set 失敗，{nameof(smeltingNumber)} 數量要等於 {result.SmeltingNumber.Length}");

            if (id != null)
                if (id.Length == result.ID.Length)
                    result.ID = id;
                else
                    throw new MemoryException($"set 失敗，{nameof(ID)} 數量要等於 {result.ID.Length}");

            if (assemblyNumber != null)
                if (assemblyNumber.Length == result.AssemblyNumber.Length)
                    result.AssemblyNumber = assemblyNumber;
                else
                    throw new MemoryException($"set 失敗，{nameof(AssemblyNumber)} 數量要等於 {result.AssemblyNumber.Length}");

            if (source != null)
                if (source.Length == result.Source.Length)
                    result.Source = source;
                else
                    throw new MemoryException($"set 失敗，{nameof(Source)} 數量要等於 {result.Source.Length}");

            result.Length = length;
            result.Finish = finish;
            this = result;
            H = h;
            W = w;
            this.t1 = t1;
            this.t2 = t2;
            BoltsCountL = boltsCountL;
            BoltsCountM = boltsCountM;
            BoltsCountR = boltsCountR;
            IndexBoltsL = indexBoltsL;
            IndexBoltsM = indexBoltsM;
            IndexBoltsR = indexBoltsR;
            ProfileType = profileType;
            Stamp = stamp;
            StampData = stampData;
            DrMiddle = drMiddle;
            DrRight = drRight;
            DrLeft = drLeft;
            GUID = gUID;
            Material = material;
        }
        //public WorkMaterial(ISteelBase steelBase, char[] materialNumber)
        //{
        //    this = new WorkMaterial(materialNumber, steelBase.Profile.PadRight(30, '\0').ToArray(),
        //        steelBase.GetPartNumber().PadRight(310, '\0').ToArray(), steelBase.Length, Finish = 0,
        //        steelBase.Material, steelBase.GetAsseNumber().PadRight(25, '\0').ToArray(),
        //        null, null, S );
        //}
        /// <summary>
        /// 素材完成進度
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        public ushort Finish;
        /// <summary>
        /// 素材長度
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [PhoneCondition]
        [DataMember]
        public double Length;
        /// <summary>
        /// 素材編號
        /// </summary>
        /// <remarks>
        ///  指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20
        /// <para>Phone 不可變更的參數</para>
        /// </remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        [DataMember]
        public char[] MaterialNumber;
        /// <summary>
        /// 零件編號
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 310
        /// </remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 310)]
        [DataMember]
        public char[] PartNumber;
        /// <summary>
        /// 斷面規格
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 30
        /// </remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        [DataMember]
        public char[] Profile;
        // TODO: 小霖增加
        /// <summary>
        /// 熔煉號 (爐號)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        [DataMember]
        public char[] SmeltingNumber;
        /// <summary>
        /// 素材 ID 
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 45
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        [DataMember]
        public char[] ID;
        /// <summary>
        /// 構件編號
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 25
        /// </remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        [DataMember]
        public char[] AssemblyNumber;
        /// <summary>
        /// 來源廠商
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        [DataMember]
        public char[] Source;
        //TODO:新增
        /// <summary>
        /// 材質
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public MATERIAL Material;
        /// <summary>
        /// 高度
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public float H;
        /// <summary>
        /// 寬度
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public float W;
        /// <summary>
        /// 腹板厚度
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public float t1;
        /// <summary>
        /// 翼板厚度
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public float t2;
        /// <summary>
        /// 左軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的左邊主軸</remarks>
        [PhoneCondition()]
        [DataMember]
        public short BoltsCountL;
        /// <summary>
        /// 中間軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的中間主軸</remarks>
        [PhoneCondition()]
        [DataMember]
        public short BoltsCountM;
        /// <summary>
        /// 右軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的右邊主軸</remarks>
        [PhoneCondition()]
        [DataMember]
        public short BoltsCountR;
        /// <summary>
        /// 左軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的左邊主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        public short IndexBoltsL;
        /// <summary>
        /// 中間軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的中間主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        public short IndexBoltsM;
        /// <summary>
        /// 右軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的右邊主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        public short IndexBoltsR;
        /// <summary>
        /// 斷面規格類型
        /// </summary>
        [PhoneCondition()]
        [DataMember]
        public PROFILE_TYPE ProfileType;
        /// <summary>
        /// 執行敲鋼印
        /// </summary>
        /// <remarks>
        /// 要執行敲鋼印回傳 true，不執行則回傳false。
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Stamp;
        /// <summary>
        /// 鋼印資料
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 30
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Stamp[] StampData;
        /// <summary>
        /// 中軸加工訊息
        /// </summary>
        /// <remarks>面對出料口的中間主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrMiddle;
        /// <summary>
        /// 右軸加工訊息
        /// </summary>
        /// <remarks>面對出料口的右邊主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrRight;
        /// <summary>
        /// 左軸加工訊息
        /// </summary>
        /// <remarks>面對出料口的左邊主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrLeft;
        /// <summary>
        /// 圖面 GUID 
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 16</remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] GUID;
        /// <summary>
        /// 插單
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition()]
        public bool Insert;
    }
}
