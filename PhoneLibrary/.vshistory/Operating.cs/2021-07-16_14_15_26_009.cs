using GD_STD.Base;
using GD_STD.Enum;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.Phone.MemoryHelper;

namespace GD_STD.Phone
{
    /// <summary>
    /// 控制訊號
    /// </summary>
    /// <remarks>
    ///<para>PC 寫入/讀取</para>
    /// <para>Codesys 寫入/讀取</para>
    /// <para>Phone 寫入/讀取</para>
    /// </remarks>
    [DataContract]
    public struct Operating : ISharedMemory, Base.ISharedMemory
    {
        /// <summary>
        /// 開啟控制訊號
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool OpenApp;
        /// <summary>
        /// 控制訊號連線狀態
        /// </summary>
        /// <remarks>
        /// 狀態只能變更成 <see cref="PHONE_SATUS.MONITOR"/> 或  <see cref="PHONE_SATUS.WAIT_MANUAL"/> 或  <see cref="PHONE_SATUS.WAIT_MATCH"/>
        /// </remarks>
        [DataMember]
        [PhoneCondition()]
        public PHONE_SATUS Satus;
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        private void ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(Operating));
            using (var memory = OperatingMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                memory.ReadArray<byte>(0, buffer, 0, size);

                this = buffer.FromByteArray<Operating>();
            }
        }
        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }
        /// <inheritdoc/>
        void Base.ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }

        /// <summary>
        /// 寫入記憶體
        /// </summary>
        private void WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(Operating));
            using (var memory = OperatingMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();

                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
        /// <inheritdoc/>
        void ISharedMemory.WriteMemory()
        {
            Operating initial = SharedMemory<Operating>.GetValue();
            if ((int)this.Satus < 3)
            {
                WriteMemory();
            }
            else if (initial.Satus != Satus && (int)this.Satus >= 3)
            {
                throw new MemoryException($"寫入失敗。因 {nameof(Satus)} 只允許改變 {nameof(PHONE_SATUS.MONITOR)} 或 {nameof(PHONE_SATUS.WAIT_MANUAL)} 或 {nameof(PHONE_SATUS.WAIT_MATCH)}");
            }
            //PhoneConditionAttribute.Condition(this, initial);
        }
        /// <inheritdoc/>
        void Base.ISharedMemory.WriteMemory()
        {
            WriteMemory();
        }
    }
}