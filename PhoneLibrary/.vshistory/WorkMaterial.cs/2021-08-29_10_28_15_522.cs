using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 等待加工料件
    /// </summary>
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct WorkMaterial
    {
        /// <summary>
        /// 初始化結構
        /// </summary> 
        /// <returns></returns>
        public static WorkMaterial Initialization()
        {
            WorkMaterial result = new WorkMaterial();
            #region 使用受保護的方法 (Ban)
#if !LogicYeh
            Ban = true;
#endif
            result.MaterialNumber = new ushort[typeof(WorkMaterial).ArrayLength(nameof(MaterialNumber))];
            result.Profile = new byte[typeof(WorkMaterial).ArrayLength(nameof(Profile))];
            result.PartNumber = new ushort[typeof(WorkMaterial).ArrayLength(nameof(PartNumber))];
            result.SmeltingNumber = new byte[typeof(WorkMaterial).ArrayLength(nameof(SmeltingNumber))];
            result.ID = new byte[typeof(WorkMaterial).ArrayLength(nameof(ID))];
            result.AssemblyNumber = new ushort[typeof(WorkMaterial).ArrayLength(nameof(AssemblyNumber))];
            result.Source = new ushort[typeof(WorkMaterial).ArrayLength(nameof(Source))];
#if !LogicYeh
            Ban = false;
#endif
            #endregion
            result.DrLeft = new Drill[1000];
            result.DrMiddle = new Drill[1000];
            result.DrRight = new Drill[1000];
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
        private WorkMaterial(string materialNumber = null, string profile = null, string partNumber = null,
            double length = 0, ushort finish = 0, string material = default, string assemblyNumber = null,
            string id = null, string source = null, string smeltingNumber = null, float h = 0, float w = 0,
            float t1 = 0, float t2 = 0, ushort boltsCountL = 0, ushort boltsCountM = 0, ushort boltsCountR = 0,
            ushort indexBoltsL = 0, ushort indexBoltsM = 0, ushort indexBoltsR = 0, PROFILE_TYPE profileType = default,
            bool stamp = false, Stamp[] stampData = null, Drill[] drMiddle = null, Drill[] drRight = null, Drill[] drLeft = null, string gUID = null)
        {
            WorkMaterial result = Initialization();
            if (materialNumber != null)
                if (materialNumber.Length == result.MaterialNumber.Length)
                    result.MaterialNumber = materialNumber.Select(el => Convert.ToUInt16(el)).ToArray();
                else
                    throw new MemoryException($"set 失敗，{nameof(MaterialNumber)} 數量要等於 {result.MaterialNumber.Length}");

            if (profile != null)
                if (profile.Length == result.Profile.Length)
                    result.Profile = Encoding.ASCII.GetBytes(profile);
                else
                    throw new MemoryException($"set 失敗，{nameof(Profile)} 數量要等於 {result.Profile.Length}");

            if (partNumber != null)
                if (partNumber.Length == result.PartNumber.Length)
                    result.PartNumber = partNumber.Select(el => Convert.ToUInt16(el)).ToArray();
                else
                    throw new MemoryException($"set 失敗，{nameof(PartNumber)} 數量要等於 {result.PartNumber.Length}");

            if (smeltingNumber != null)
                if (smeltingNumber.Length == result.SmeltingNumber.Length)
                    result.SmeltingNumber = Encoding.ASCII.GetBytes(profile);
                else
                    throw new MemoryException($"set 失敗，{nameof(smeltingNumber)} 數量要等於 {result.SmeltingNumber.Length}");

            if (id != null)
                if (id.Length == result.ID.Length)
                    result.ID = Encoding.ASCII.GetBytes(id);
                else
                    throw new MemoryException($"set 失敗，{nameof(ID)} 數量要等於 {result.ID.Length}");

            if (assemblyNumber != null)
                if (assemblyNumber.Length == result.AssemblyNumber.Length)
                    result.AssemblyNumber = assemblyNumber.Select(el => Convert.ToUInt16(el)).ToArray();
                else
                    throw new MemoryException($"set 失敗，{nameof(AssemblyNumber)} 數量要等於 {result.AssemblyNumber.Length}");

            if (source != null)
                if (source.Length == result.Source.Length)
                    result.Source = source.Select(el => Convert.ToUInt16(el)).ToArray();
                else
                    throw new MemoryException($"set 失敗，{nameof(Source)} 數量要等於 {result.Source.Length}");

            if (drLeft != null)
                if (drLeft.Length == result.DrLeft.Length)
                    result.DrLeft = drLeft;
                else
                    throw new MemoryException($"set 失敗，{nameof(DrLeft)} 數量要等於 {result.DrLeft.Length}");

            if (drRight != null)
                if (drRight.Length == result.DrRight.Length)
                    result.DrRight = drRight;
                else
                    throw new MemoryException($"set 失敗，{nameof(DrRight)} 數量要等於 {result.DrRight.Length}");

            if (drMiddle != null)
                if (drMiddle.Length == result.DrMiddle.Length)
                    result.DrMiddle = drMiddle;
                else
                    throw new MemoryException($"set 失敗，{nameof(DrMiddle)} 數量要等於 {result.DrMiddle.Length}");

            if (stampData != null)
                if (stampData.Length == result.StampData.Length)
                    result.StampData = stampData;
                else
                    throw new MemoryException($"set 失敗，{nameof(StampData)} 數量要等於 {result.StampData.Length}");

            if (gUID != null)
                if (gUID.Length == result.GUID.Length)
                    result.GUID = Encoding.ASCII.GetBytes(gUID);
                else
                    throw new MemoryException($"set 失敗，{nameof(StampData)} 數量要等於 {result.GUID.Length}");

            result.Finish = finish;
            result.Length = length;
            result.H = h;
            result.W = w;
            result.t1 = t1;
            result.t2 = t2;
            result.BoltsCountL = boltsCountL;
            result.BoltsCountM = boltsCountM;
            result.BoltsCountR = boltsCountR;
            result.IndexBoltsL = indexBoltsL;
            result.IndexBoltsM = indexBoltsM;
            result.IndexBoltsR = indexBoltsR;
            result.ProfileType = profileType;
            result.Stamp = stamp;
            result.Material = Encoding.ASCII.GetBytes(material);
            this = result;
        }
        /// <summary>
        /// 素材完成進度 (Field 1)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public ushort Finish;
        /// <summary>
        /// 左軸加工孔位的陣列長度 (Field 2)
        /// </summary>
        /// <remarks>面對出料口的左邊主軸<para>Phone 不可變更的參數</para></remarks>
        [DataMember]
        public ushort BoltsCountL;
        /// <summary>
        /// 中間軸加工孔位的陣列長度 (Field 3)
        /// </summary>
        /// <remarks>面對出料口的中間主軸<para>Phone 不可變更的參數</para></remarks>
        [DataMember]
        public ushort BoltsCountM;
        /// <summary>
        /// 右軸加工孔位的陣列長度 (Field 4)
        /// </summary>
        /// <remarks>面對出料口的右邊主軸<para>Phone 不可變更的參數</para></remarks>
        [DataMember]
        public ushort BoltsCountR;
        /// <summary>
        /// 左軸加工的陣列起始位置 (Field 5)
        /// </summary>
        /// <remarks>
        /// 面對出料口的左邊主軸，代表機械加工軸是要從第幾的陣列開始執行<para>Phone 不可變更的參數</para>
        /// </remarks>
        [DataMember]
        public ushort IndexBoltsL;
        /// <summary>
        /// 中間軸加工的陣列起始位置 (Field 6)
        /// </summary>
        /// <remarks>
        /// 面對出料口的中間主軸，代表機械加工軸是要從第幾的陣列開始執行<para>Phone 不可變更的參數</para>
        /// </remarks>
        [DataMember]
        public ushort IndexBoltsM;
        /// <summary>
        /// 右軸加工的陣列起始位置 (Field 7)
        /// </summary>
        /// <remarks>
        /// 面對出料口的右邊主軸，代表機械加工軸是要從第幾的陣列開始執行<para>Phone 不可變更的參數</para>
        /// </remarks>
        [DataMember]
        public ushort IndexBoltsR;
        /// <summary>
        /// 材質 (Field 8)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20
        /// <para>Phone 不可變更的參數</para>
        /// </remarks>
        [DataMember]
        public byte[] Material;
        /// <summary>
        /// 高度 (Field 9)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public float H;
        /// <summary>
        /// 寬度 (Field 10)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public float W;
        /// <summary>
        /// 腹板厚度 (Field 11)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public float t1;
        /// <summary>
        /// 翼板厚度 (Field 12)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public float t2;
        /// <summary>
        /// 圖面 GUID Ascii Code (Field 13)
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 16</remarks>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] GUID;
        /// <summary>
        /// 素材編號 Unicode UTF-16 (Field 14)
        /// </summary>
        /// <remarks>
        ///  指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20
        /// <para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        [DataMember]
        public ushort[] MaterialNumber;
        /// <summary>
        /// 零件編號 Unicode UTF-16 (Field 15)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 310<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 310)]
        [DataMember]
        public ushort[] PartNumber;
        /// <summary>
        /// 構件編號 Unicode UTF-16 (Field 16)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 25<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        [DataMember]
        public ushort[] AssemblyNumber;
        /// <summary>
        /// 來源廠商 Unicode UTF-16 (Field 17)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        [DataMember]
        public ushort[] Source;
        /// <summary>
        /// 斷面規格類型 (Field 18)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public PROFILE_TYPE ProfileType;
        /// <summary>
        /// 橫移擺放位置 (Field 19)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public float Position;
        /// <summary>
        /// 斷面規格 Ascii Code (Field 20)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 30<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        [DataMember]
        public byte[] Profile;
        /// <summary>
        /// 熔煉號 (爐號) Ascii Code (Field 21)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        [DataMember]
        public byte[] SmeltingNumber;
        /// <summary>
        /// 素材 ID Ascii Code (Field 22)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 45<para>Phone 不可變更的參數</para>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        [DataMember]
        public byte[] ID;
        /// <summary>
        /// 插單 (Field 23)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [MarshalAs(UnmanagedType.I1)]
        [DataMember]
        [PhoneCondition()]
        public bool Insert;
        /// <summary>
        /// 素材長度 (Field 24)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        public double Length;
        /// <summary>
        /// 執行敲鋼印 (Field 25)
        /// </summary>
        /// <remarks>
        /// 要執行敲鋼印回傳 true，不執行則回傳false。<para>Phone 不可變更的參數</para>
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Stamp;
        /// <summary>
        /// 鋼印資料 (Field 26)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 30<para>Phone 不可變更的參數</para>
        /// </remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Stamp[] StampData;
        /// <summary>
        /// 中軸加工訊息 (Field 27)
        /// </summary>
        /// <remarks>面對出料口的中間主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999<para>Phone 不可變更的參數</para></remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrMiddle;
        /// <summary>
        /// 右軸加工訊息 (Field 28)
        /// </summary>
        /// <remarks>面對出料口的右邊主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999<para>Phone 不可變更的參數</para></remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrRight;
        /// <summary>
        /// 左軸加工訊息 (Field 29)
        /// </summary>
        /// <remarks>面對出料口的左邊主軸。指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 999<para>Phone 不可變更的參數</para></remarks>
        [PhoneCondition()]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public Drill[] DrLeft;
    }
}
