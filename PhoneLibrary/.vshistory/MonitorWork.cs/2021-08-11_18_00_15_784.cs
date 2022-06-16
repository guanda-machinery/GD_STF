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
    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorWork : IPhoneSharedMemory, IPhoneSharedMemoryOffset
#if LogicYeh
        , IPCSharedMemory, IPCSharedMemoryOffset
#endif
    {
        /// <summary>
        /// 初始化結構
        /// </summary>
        /// <returns></returns>
        public static MonitorWork Initialization()
        {
            #region 使用受保護的方法 (Ban)
            Ban = true;
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
            Ban = false;
            #endregion
            result.WorkMaterial = work;
            result.Current = -1;
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
        /// PC 配置好的素材清單 (Field 1)
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
        /// Codesys 讀取 <see cref="Index"/> 位置。初始值 -1  (Field 2)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Current"/> 不開放手機寫入。</exception>
        [DataMember]
        [PhoneCondition()]
        public short Current;
        /// <summary>
        /// 工作索引位置。 <see cref="WorkMaterial"/> 陣列位置。陣列內容初始值 -1  (Field 3)
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 3000
        /// </remarks>
        /// <exception cref="MemoryException">初始化失敗，因 <see cref="Index"/> 內容有重複的值。</exception>
        /// <exception cref="MemoryException">寫入 <see cref="Index"/> 失敗。因陣列位置不能小於 <see cref="Current"/> 。</exception>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)]
        [DataMember]
        public short[] Index;
        /// <summary>
        /// 專案名稱   (Field 4)
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
        /// 總進度趴數 (Field 5)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Schedule"/> 不開放手機寫入。</exception>
        [DataMember]
        public ushort Schedule;
        /// <summary>
        /// <see cref="WorkMaterial"/> 陣列長度 (Field 6)
        /// </summary>
        /// <remarks>
        /// Phone 不可變更
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 <see cref="Count"/> 不開放手機寫入。</exception> 
        [DataMember]
        public ushort Count;
        /// <summary>
        /// 通知移動料架搬運 (Field 7)
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Move;
        /// <summary>
        /// 等待加工占用長度 (Field 8)
        /// </summary>
        [DataMember]
        public double ExportOccupy;
        /// <summary>
        /// 入口移動料架占用長度 (Field 9)
        /// </summary>
        [DataMember]
        public double EntranceOccupy;
        #region 記憶體交握
        T ISharedMemoryOffset.ReadMemory<T>(long position, int size)
        {
            int structSize = Marshal.SizeOf(this.GetType());
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, structSize, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];
                memory.ReadArray<byte>(position, buffer, 0, size);
                #region 使用受保護的方法 (Ban)
                Ban = true;
                T result = buffer.FromByteArray<T>();
                Ban = false;
                #endregion
                return result;
            }
        }
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
                this = (MonitorWork)Marshal.PtrToStructure(memory.SafeMemoryMappedViewHandle.DangerousGetHandle(), typeof(MonitorWork));
            }
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <typeparam name="T">寫入的結構型別</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        private void WriteMemory(long position, byte[] value)
        {
            int structSize = Marshal.SizeOf(this.GetType());
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, structSize, MemoryMappedFileAccess.Write))
            {

                memory.WriteArray<byte>(position, value, 0, value.Length);
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
        void IPhoneSharedMemoryOffset.WriteMemory(long position, byte[] value)
        {
            //判斷連線狀態
            Operating operating = SharedMemory.GetValue<Operating>();
            if (operating.Satus != PHONE_SATUS.MATCH || operating.Satus != PHONE_SATUS.INSERT_MATCH)
            {
                throw new MemoryException($"寫入失敗。因 Operating.Satus 狀態不是 MATCH OR INSERT_MATCH。");
            }
            #region 檢測寫入
            #region 使用受保護的方法 (Ban)
            Ban = true;
            Type type = typeof(WorkMaterial);
            long workMaterialSize = Marshal.SizeOf(type);
            //可寫入 WorkMaterial 的起始位置
            long materialNumberOffsetStart = Marshal.OffsetOf(type, nameof(GD_STD.Phone.WorkMaterial.MaterialNumber)).ToInt64(); //素材編號起始位置
            long materialNumberOffsetCount = new WorkMaterial().GetSizeof(nameof(GD_STD.Phone.WorkMaterial.MaterialNumber));//素材編號 Szieof
            long smeltingNumberOffsetStart = Marshal.OffsetOf(type, nameof(GD_STD.Phone.WorkMaterial.SmeltingNumber)).ToInt64(); //熔煉號 (爐號) 起始位置
            long smeltingNumberOffsetCount = new WorkMaterial().GetSizeof(nameof(GD_STD.Phone.WorkMaterial.SmeltingNumber));//熔煉號 (爐號) Szieof
            long idOffsetStart = Marshal.OffsetOf(type, nameof(GD_STD.Phone.WorkMaterial.ID)).ToInt64(); //素材 ID 起始位置
            long idOffsetCount = new WorkMaterial().GetSizeof(nameof(GD_STD.Phone.WorkMaterial.ID)); //素材 ID 結束位置
            long sourceStart = Marshal.OffsetOf(type, nameof(GD_STD.Phone.WorkMaterial.Source)).ToInt64(); //熔煉號 (爐號) 起始位置
            long sourceCount = new WorkMaterial().GetSizeof(nameof(GD_STD.Phone.WorkMaterial.Source)); //素材 ID 結束位置
            long move = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Move)).ToInt64(); //移動料架搬運訊號位置
            Ban = false;
            #endregion

            //餘數
            long writeMaterialNumber = (position - materialNumberOffsetStart) % workMaterialSize;
            long writeSmeltingNumber = (position - smeltingNumberOffsetStart) % workMaterialSize;
            long writeIDOffset = (position - idOffsetStart) % workMaterialSize;
            long writeSource = (position - sourceStart) % workMaterialSize;
            if ((writeMaterialNumber == 0 && value.Length <= materialNumberOffsetCount) ||
                (writeSmeltingNumber == 0 && value.Length <= smeltingNumberOffsetCount) ||
                (writeIDOffset == 0 && value.Length <= idOffsetCount) ||
                (writeSource == 0 && value.Length <= sourceCount) ||
                (move == position && value.Length <= sizeof(bool)))
            {
                WriteMemory(position, value);
                return;
            }
            throw new MemoryException($"寫入失敗，因寫入位置受到保護。");
            #endregion
        }
#if LogicYeh
        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MonitorWork));
            using (var memory = MonitorWorkMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                #region 使用受保護的方法 (Ban)
                Ban = true;
                byte[] buffer = this.ToByteArray();
                Ban = false;
                #endregion
                
                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
        void IPCSharedMemoryOffset.WriteMemory(long position, byte[] value)
        {
            WriteMemory(position, value);
        }
#endif
        #endregion

    }
}
