namespace GD_STD.Base
{
    /// <summary>
    /// Phone 共享記憶體介面
    /// </summary>
    public interface IPhoneSharedMemoryOffset
    {
        /// <summary>
        /// Phone 寫入 Codesys 的共享記憶體內
        /// </summary>
        /// <typeparam name="T">寫入的結構型別</typeparam>
        /// <param name="position">T <see cref="byte"/>[] 會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的值</param>
        /// <param name="valueOffset"><see cref="byte"/>[] 中要開始寫入的索引</param>
        /// <param name="count"><see cref="byte"/>[] 中要寫入的結構數目</param>
        void WriteMemory<T>(long position, T value, int valueOffset, int count) where T : struct;
    }
}
