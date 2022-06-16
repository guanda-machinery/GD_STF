namespace GD_STD.Base
{
    /// <summary>
    /// PC 共享記憶體介面
    /// </summary>
    public interface ISharedMemory 
    {
        /// <summary>
        /// 讀取 Codesys 的共享記憶體
        /// </summary>
        void ReadMemory();
        /// <summary>
        /// 寫入 Codesys 的共享記憶體內
        /// </summary>
        void WriteMemory();
    }
}