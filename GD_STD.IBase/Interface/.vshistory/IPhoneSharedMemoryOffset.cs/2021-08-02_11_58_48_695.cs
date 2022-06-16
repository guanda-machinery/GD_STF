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
        /// <typeparam name="T">要寫入的結構</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數</param>
        /// <param name="value">要寫入的值</param>
        /// <param name="valueOffet"></param>
        /// <param name="size"></param>
        void WriteMemory<T>(long position, T value, long valueOffet, long size) where T : struct;
    }
}
