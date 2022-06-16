using GD_STD.Base.Additional;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 側壓夾具
    /// </summary>
    /// <remarks>
    /// 代表著側壓夾具的相關資訊
    /// Codesys  Memory 讀取
    /// </remarks>
    [DataContract]
    public struct SideClamp : Base.IFixture
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="entranceL"></param>
        /// <param name="exportL"></param>
        public SideClamp(double entranceL, double exportL)
        {
            EntranceL=entranceL;
            ExportL=exportL;
        }

        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double EntranceL { get; set; }
        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double ExportL { get; set; }
    }
}
