using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using static GD_STD.MemoryHelper;
using static GD_STD.SerializationHelper;
using GD_STD;
namespace GD_STD
{
    /// <summary>素材加工訊息
    /// <para>可序列化的結構</para></summary>
    /// <remarks>代表著素材要加工的相關資訊
    /// Codesys  Memory 寫入</remarks>
    /// <seealso cref="DataContract">附加屬性</seealso>
    [Serializable()]
    [DataContract]
    public struct JobInfo : IPart, IPCSharedMemory
    {
        /// <summary>
        /// 素材編號
        /// </summary>
        /// <remarks>表示素材配料完成的編號，非單一零件的編號</remarks>
        [DataMember]
        public int Number { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public double Length { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public MATERIAL Material { get; set; }
        //TODO:刪除
        #region 刪除區塊               
        /// <inheritdoc/>
        [DataMember]
        public float H { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public float W { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public float t1 { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public float t2 { get; set; }
        /// <summary>
        /// 左軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的左邊主軸</remarks>
        [DataMember]
        public short BoltsCountL { get; set; }
        /// <summary>
        /// 中間軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的中間主軸</remarks>
        [DataMember]
        public short BoltsCountM { get; set; }
        /// <summary>
        /// 右軸加工孔位的陣列長度
        /// </summary>
        /// <remarks>面對出料口的右邊主軸</remarks>
        [DataMember]
        public short BoltsCountR { get; set; }
        /// <summary>
        /// 左軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的左邊主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [DataMember]
        public short IndexBoltsL { get; set; }
        /// <summary>
        /// 中間軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的中間主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [DataMember]
        public short IndexBoltsM { get; set; }
        /// <summary>
        /// 右軸加工的陣列起始位置
        /// </summary>
        /// <remarks>
        /// 面對出料口的右邊主軸，代表機械加工軸是要從第幾的陣列開始執行
        /// </remarks>
        [DataMember]
        public short IndexBoltsR { get; set; }
        /// <summary>
        /// 斷面規格類型
        /// </summary>
        [DataMember]
        public PROFILE_TYPE Type { get; set; }
        /// <summary>
        /// 執行敲鋼印
        /// </summary>
        /// <remarks>
        /// 要執行敲鋼印回傳 true，不執行則回傳false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Stamp;
        /// <summary>
        /// 鋼印資料
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 30
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Stamp[] StampData;
        #endregion
        /// <summary>
        /// 加入零件訊息
        /// </summary>
        public void AddPartInfo(IPart part)
        {
            H = part.H;
            W = part.W;
            t1 = part.t1;
            t2 = part.t2;
            Material = part.Material;
        }
        /// <summary>
        /// 初始化結構
        /// </summary>
        /// <returns></returns>
        public static JobInfo Initialization()
        {
            JobInfo result = new JobInfo();
            result.StampData = new Stamp[30];
            for (int i = 0; i < result.StampData.Length; i++)
            {
                result.StampData[i] = new Stamp(content: null);
            }
            return result;
        }
        void ISharedMemory.ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(JobInfo));
            using (var jobInfo = JobInfoMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                jobInfo.ReadArray<byte>(0, buffer, 0, size);

                this = SharedMemory.FromByteArray<JobInfo>(buffer);
            }
        }

        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(JobInfo));
            using (var jobInfo = JobInfoMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = SharedMemory.ToByteArray<JobInfo>(this);

                jobInfo.WriteArray<byte>(0, buffer, 0, size);
            }
        }
    }
}