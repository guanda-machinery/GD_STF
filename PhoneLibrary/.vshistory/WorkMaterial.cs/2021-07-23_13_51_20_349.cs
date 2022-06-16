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
            WorkMaterial result = new WorkMaterial();
            result.MaterialNumber = new char[result.Length(nameof(MaterialNumber))];
            result.Profile = new char[result.Length(nameof(Profile))];
            result.PartNumber = new char[result.Length(nameof(PartNumber))];
            result.SmeltingNumber = new char[result.Length(nameof(SmeltingNumber))];
            result.ID = new char[result.Length(nameof(ID))];
            result.Assembly = new char[result.Length(nameof(Assembly))];
            result.Source = new char[result.Length(nameof(Source))];
            result.Material = new char[result.Length(nameof(Material))];
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
        /// <param name="assembly">構件編號</param>
        /// <param name="source">廠商</param>
        /// <param name="smeltingNumber">熔煉號 (爐號)</param>
        public WorkMaterial(char[] materialNumber = null, char[] profile = null, char[] partNumber = null, double length = 0, ushort finish = 0, char[] material = null, char[] assembly = null, char[] id = null, char[] source = null, char[] smeltingNumber = null)
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

            if (assembly != null)
                if (assembly.Length == result.Assembly.Length)
                    result.Assembly = assembly;
                else
                    throw new MemoryException($"set 失敗，{nameof(Assembly)} 數量要等於 {result.Assembly.Length}");

            if (source != null)
                if (source.Length == result.Source.Length)
                    result.Source = source;
                else
                    throw new MemoryException($"set 失敗，{nameof(Source)} 數量要等於 {result.Source.Length}");

            if (material != null)
                if (material.Length == result.MaterialNumber.Length)
                    result.Material = material;
                else
                    throw new MemoryException($"set 失敗，{nameof(Material)} 數量要等於 {result.Source.Length}");

            result.Length = length;
            result.Finish = finish;
            this = result;
        }
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
        public char[] Assembly;
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
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10
        /// </remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        [DataMember]
        public char[] Material;
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
        public PROFILE_TYPE Type;
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
        /// 面對加工機出料中間的軸向
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 999)]
        public Drill[] DrMiddle;
        /// <summary>
        /// 面對加工機出料右邊的軸向
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 999)]
        public Drill[] DrRight;
        /// <summary>
        /// 面對加工機出料左邊的軸向
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999</remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 999)]
        public Drill[] DrLeft;
        /// <summary>
        /// 圖面 GUID 
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 16</remarks>
        [PhoneCondition()]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] GUID;
    }
}
