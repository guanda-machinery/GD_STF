namespace GD_STD.IBase
{
    /// <summary>
    /// 油壓介面
    /// </summary>
    public interface IOillSystem
    {
        /// <summary>
        /// 寫入液壓油系統參數給 Codesys 
        /// </summary>
        void WriteHydraulic();

        /// <summary>
        /// 讀取 Codesys 液壓油系統參數
        /// </summary>
        void ReadHydraulic();

        /// <summary>
        /// 寫入潤滑油系統參數給 Codesys 
        /// </summary>
        void WriteLubricant();

        /// <summary>
        /// 讀取 Codesys 滑道油系統參數
        /// </summary>
        void ReadLubricant();

        /// <summary>
        /// 寫入切削系統參數給 Codesys 
        /// </summary>
        void WriteCut();

        /// <summary>
        /// 讀取 Codesys 切削油系統參數
        /// </summary>
        void ReadCut();
    }
}
