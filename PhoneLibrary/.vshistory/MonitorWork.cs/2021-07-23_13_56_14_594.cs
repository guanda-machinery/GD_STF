using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 加工監控
    /// </summary>
    /// <remarks>
    /// 記憶體名稱 MonitorWork
    /// <para>PC 寫入/讀取</para> 
    /// <para>Phone 讀取/寫入</para>
    /// </remarks>
    [Serializable]
    [DataContract]
    public struct MonitorWork : ISharedMemory, Base.ISharedMemory
    {

        /// <summary>
        /// 初始化結構
        /// </summary>
        /// <returns></returns>
        public static MonitorWork Initialization()
        {
            MonitorWork result = new MonitorWork()
            {
                Count = 0,
                Schedule = 0,
            };

            WorkMaterial[] work = new WorkMaterial[result.Length(nameof(result.WorkMaterial))];
            WorkMaterial[] match = new WorkMaterial[result.Length(nameof(result.MatchWorkMaterial))];
            for (int i = 0; i < work.Length; i++)
            {
                if (i < match.Length)
                {

                    match[i] = work[i] = Phone.WorkMaterial.Initialization();
                }
                else
                {
                    work[i] = Phone.WorkMaterial.Initialization();
                }
            }
            result.ProjectName = new char[result.Length(nameof(result.ProjectName))];
            result.WorkMaterial = work;
            result.MatchWorkMaterial = match;
            return result;
        }
        /// <summary>
        /// 標準結構
        /// </summary>
        /// <param name="projectName">專案名稱</param>
        /// <param name="count">陣列數量</param>
        /// <param name="schedule">總進度完成趴數</param>
        /// <param name="workMaterial">等待加工料件列表</param>
        /// <param name="matchWorkMaterial">配對的料單</param>
        public MonitorWork(char[] projectName = null, int count = 0, ushort schedule = 0, WorkMaterial[] workMaterial = null, WorkMaterial[] matchWorkMaterial = null)
        {
            MonitorWork result = Initialization();

            if (projectName != null)
                if (projectName.Length == result.ProjectName.Length)
                    result.ProjectName = projectName;
                else
                    throw new MemoryException($"初始化失敗，{nameof(ProjectName)} 數量要等於 {result.ProjectName.Length}");


            if (workMaterial != null)
                if (workMaterial.Length == result.WorkMaterial.Length)
                    result.WorkMaterial = workMaterial;
                else
                    throw new MemoryException($"初始化失敗，{nameof(WorkMaterial)} 數量要等於 {result.WorkMaterial.Length}");

            if (matchWorkMaterial != null)
                if (matchWorkMaterial.Length == result.MatchWorkMaterial.Length)
                    result.MatchWorkMaterial = matchWorkMaterial;
                else
                    throw new MemoryException($"初始化失敗，{nameof(MatchWorkMaterial)} 數量要等於 {result.MatchWorkMaterial.Length}");


            result.Schedule = schedule;
            result.Count = count;

            this = result;
        }
        /// <summary>
        /// <see cref="WorkMaterial"/> 陣列長度，掃 QRCode 數量
        /// </summary>
        /// <remarks>
        ///  變更參數條件 <see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MATCH"/>
        /// </remarks>
        [DataMember]
        public int Count;
        /// <summary>
        /// 專案名稱
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20 
        /// <para>
        /// Phone 不可變更
        /// </para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="ProjectName"/> 不開放手機寫入。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        [DataMember]
        [PhoneCondition()]
        public char[] ProjectName;
        /// <summary>
        /// 總進度趴數
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Schedule"/> 不開放手機寫入。</exception>
        [DataMember]
        [PhoneCondition()]
        public ushort Schedule;
        /// <summary>
        /// 等待加工料件列表
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 70
        /// <para> 變更參數條件：</para> 
        /// <para>
        /// 變更參數條件 <see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MATCH"/>
        /// </para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MATCH。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        [DataMember]
        public WorkMaterial[] MatchWorkMaterial;
        /// <summary>
        /// PC 配置好的素材清單
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 1000
        /// <para>Phone 不可變更</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="WorkMaterial"/> 不開放手機寫入。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        [DataMember]
        [PhoneCondition()]
        public WorkMaterial[] WorkMaterial;
        #region 記憶體交握
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        private void ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(MonitorWork));
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                memory.ReadArray<byte>(0, buffer, 0, size);

                this = buffer.FromByteArray<MonitorWork>();
            }
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        private void WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MonitorWork));
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();

                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
        /// <inheritdoc/>
        void Base.ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }
        /// <inheritdoc/>
        void Base.ISharedMemory.WriteMemory()
        {
            WriteMemory();
        }
        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }
        /// <inheritdoc/>
        void ISharedMemory.WriteMemory()
        {
            Operating operating = SharedMemory<Operating>.GetValue();
            if (operating.Satus != PHONE_SATUS.MATCH)
            {
                throw new MemoryException($"寫入失敗。因 Operating.Satus 狀態不是 MATCH。");
            }
            MonitorWork initial = SharedMemory<MonitorWork>.GetValue();
            PhoneConditionAttribute.Condition(this, initial);
            WriteMemory();
        }
        #endregion

    }
}
