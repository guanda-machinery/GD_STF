namespace GD_STD
{
    /// <summary>
    /// 側壓夾具
    /// </summary>
    /// <remarks>
    /// 代表著側壓夾具的相關資訊
    /// Codesys  Memory 讀取
    /// </remarks>
    public struct SideClamp : Base.IFixture
    {
        /// <inheritdoc/>
        public double EntranceL { get; set; }
        /// <inheritdoc/>
        public double ExportL { get; set; }
    }
}
