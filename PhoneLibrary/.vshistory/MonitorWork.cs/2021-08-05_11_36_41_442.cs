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
    /// <para>Codesys 寫入/讀取</para> 
    /// <para>PC 寫入/讀取</para> 
    /// <para>Phone 讀取/寫入</para>
    /// </remarks>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item></item>
    ///             <item>刪除 MatchWorkMaterial</item>
    ///             <item>新增 <see cref="Current"/></item>
    ///             <item>新增 <see cref="Index"/></item>
    ///             <item>變更 <see cref="ProjectName"/> 變更 <see cref="ushort"/>[] (Unicode UTF-16 格式)</item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    [Serializable]
    [DataContract]
    public struct MonitorWork : IPhoneSharedMemory, IPhoneSharedMemoryOffset, IPCSharedMemory
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

            WorkMaterial[] work = new WorkMaterial[result.ArrayLength(nameof(result.WorkMaterial))];
            //初始化 = -1
            int[] index = new int[result.ArrayLength(nameof(result.Index))].
                                            Select(el => el = -1).ToArray();

            result.ProjectName = new ushort[result.ArrayLength(nameof(result.ProjectName))];
            result.WorkMaterial = work;
            //result.MatchWorkMaterial = match;
            return result;
        }
        /// <summary>
        /// 標準結構
        /// </summary>
        /// <param name="projectName">專案名稱</param>
        /// <param name="schedule">總進度完成趴數</param>
        /// <param name="index">陣列索引</param>
        /// <param name="workMaterial">等待加工料件列表</param>
        public MonitorWork(char[] projectName = null, ushort schedule = 0, short[] index = null, WorkMaterial[] workMaterial = null)
        {
            MonitorWork result = Initialization();

            if (projectName != null)
                if (projectName.Length == result.ProjectName.Length)
                    result.ProjectName = projectName.Select(el => Convert.ToUInt16(el)).ToArray();
                else
                    throw new MemoryException($"初始化失敗，{nameof(ProjectName)} 數量要等於 {result.ProjectName.Length}");


            if (workMaterial != null)
                if (workMaterial.Length == result.WorkMaterial.Length)
                    result.WorkMaterial = workMaterial;
                else
                    throw new MemoryException($"初始化失敗，{nameof(WorkMaterial)} 數量要等於 {result.WorkMaterial.Length}");

            if (index != null)
                if (index.Length == result.Index.Length)
                {
                    //重複數量
                    var repeat = index.GroupBy(el => el).
                                                                                                         Select(el => new { Key = el.Key, Count = el.Count() }).
                                                                                                            Where(el => el.Count > 1 && el.Key != -1);
                    if (repeat.Count() > 0)
                    {
                        throw new MemoryException($"初始化失敗，{nameof(Index)} 內容有重複的值。例如 {repeat.ElementAt(0).Key}。");
                    }
                    result.Index = index;
                }
                else
                    throw new MemoryException($"初始化失敗，{nameof(Index)} 數量要等於 {result.Index.Length}");


            result.Schedule = schedule;
            this = result;
        }
        /// <summary>
        /// PC 配置好的素材清單
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 1000
        /// <para>
        /// Phone 部分變更，詳細請看 <see cref="WorkMaterial"/>
        /// </para>
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.WAIT_MATCH"/></para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 WAIT_MATCH。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)]
        [DataMember]
        public WorkMaterial[] WorkMaterial;
        /// <summary>
        /// Codesys 讀取 <see cref="Index"/> 位置
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Current"/> 不開放手機寫入。</exception>
        [DataMember]
        [PhoneCondition()]
        public ushort Current;
        /// <summary>
        /// 工作索引位置。 <see cref="WorkMaterial"/> 陣列位置
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 3000
        /// </remarks>
        /// <exception cref="MemoryException">初始化失敗，因 <see cref="Index"/> 內容有重複的值。</exception>
        /// <exception cref="MemoryException">寫入失敗。因 offset 位置 <see cref="Index"/> 陣列位置不能小於 <see cref="Current"/> 。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)]
        [DataMember]
        public short[] Index;
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
        public ushort[] ProjectName;
        /// <summary>
        /// 總進度趴數
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Schedule"/> 不開放手機寫入。</exception>
        [DataMember]
        public ushort Schedule;
        /// <summary>
        /// <see cref="WorkMaterial"/> 陣列長度
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Count"/> 不開放手機寫入。</exception> 
        [DataMember]
        public ushort Count;
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
        /// <typeparam name="T">寫入的結構型別</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <param name="valueOffset">要從 value 位元組中開始寫入的索引</param>
        /// <param name="count">要從 value 位元組中要寫入的結構數目</param>
        private void WriteMemory<T>(long position, T value, int valueOffset, int count)
        {
            int structSize = Marshal.SizeOf(this.GetType());
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, structSize, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = value.ToByteArray();
                memory.WriteArray<byte>(position, buffer, valueOffset, count);
            }
        }
        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }
        /// <inheritdoc/>
        void IPhoneSharedMemory.WriteMemory()
        {
            throw new MemoryException($"寫入失敗。因整個 MonitorWork 寫入記憶體會造成衝突，請改用 SharedMemory.SetValue<T, TVaule>(position, value)");
        }
        void IPhoneSharedMemoryOffset.WriteMemory<T>(long position, T value, int valueOffset, int count)
        {
            //判斷連線狀態
            Operating operating = SharedMemory.GetValue<Operating>();
            if (operating.Satus != PHONE_SATUS.MATCH)
            {
                throw new MemoryException($"寫入失敗。因 Operating.Satus 狀態不是 MATCH。");
            }
            WriteMemory(position, value, valueOffset, count);
        }

        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MonitorWork));
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();

                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
        #endregion

    }
}
