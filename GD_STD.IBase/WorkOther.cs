using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 加工其他參數
    /// </summary>
    [DataContract]
    [Serializable]
    public class WorkOther
    {
        /// <summary>
        /// 加工位置
        /// </summary>
        [DataMember]
        public short Current { get; set; }
        /// <summary>
        /// 出口移動料架占用長度
        /// </summary>
        [DataMember]
        public double EntranceOccupy { get; set; }
        /// <summary>
        /// 出口移動料架占用長度
        /// </summary>
        [DataMember]
        public double ExportOccupy1 { get; set; }
        /// <summary>
        /// 出口移動料架占用長度
        /// </summary>
        [DataMember]
        public double ExportOccupy2 { get; set; }   
    }
}
