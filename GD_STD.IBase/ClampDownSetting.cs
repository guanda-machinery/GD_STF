using GD_STD.Base;
using GD_STD.Base.Additional;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 下壓夾具設定
    /// </summary>
    [DataContract]
    public struct ClampDownSetting : IFixture, IClampDown
    {
        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double EntranceL { get; set; }
        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double ExportL { get; set; }
        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double EntranceR { get; set; }
        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double ExportR { get; set; }
    }
}
