using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    //API文件: 新增結構
    /// <summary>
    /// 移動料架參數設定
    /// </summary>
    [DataContract()]
    public struct Traverse
    {
        /// <summary>
        /// 台車長度
        /// </summary>
        [DataMember]
        public double CarLength;
        /// <summary>
        /// 台車凸起區域長度
        /// </summary>
        [DataMember]
        public double CarRaisedLength;
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        [DataMember]
        public double OriginToLocationPoint;
        /// <summary>
        /// 台車排序總空間
        /// </summary>
        [DataMember]
        public double SortTotalLength;
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        [DataMember]
        public double Limit;
        /// <summary>
        /// 移動到料與料中間的尻料位置抓中心點的補正
        /// </summary>
        [DataMember]
        public double SortCorrection;
        /// <summary>
        /// 台車車凸點在料與料的中心補正
        /// </summary>
        [DataMember]
        public double ToArmSideCorrection;
    }
}
