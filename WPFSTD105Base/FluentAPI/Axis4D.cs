using GD_STD.Base.Additional;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 三維座標與相位
    /// </summary>
    [Serializable]
    [DataContract]
    public class Axis4D : GD_STD.Base.IAxis4D
    {
        /// <inheritdoc/>
        [MVVM("相位", false)]
         [DataMember]
        public double MasterPhase { get; set; }
        /// <inheritdoc/>
        [MVVM("Z", false)]
        [DataMember]
        public double Z { get; set; }
        /// <inheritdoc/>
        [MVVM("X", false)]
        [DataMember]
        public double X { get; set; }
        /// <inheritdoc/>
        [MVVM("Y", false)]
        [DataMember]
        public double Y { get; set; }
        /// <summary>
        /// 轉換 Json 格式
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
           return JsonConvert.SerializeObject(this, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
