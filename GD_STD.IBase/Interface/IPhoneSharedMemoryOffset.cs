namespace GD_STD.Base
{
    /// <summary>
    /// Phone d
    /// </summary>
    public interface IPhoneSharedMemoryOffset : ISharedMemoryOffset, IMemoryPointer
    {
        /// <summary>
        /// Phone 寫入 Codesys 的共享記憶體內
        /// </summary>
        /// <param name="position"><see cref="byte"/>[] 會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的值</param>
        void WriteMemory(long position, byte[] value);

    }
}
