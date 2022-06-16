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
        /// 台車總長
        /// </summary>
        [DataMember]
        public double Vehicle_Length;
        /// <summary>
        /// 台車凸起區域長度
        /// </summary>
        [DataMember]
        public double Vehicle_Raised_Length;
        /// <summary>
        /// 台車原點到定位輪的距離
        /// </summary>
        [DataMember]
        public double Vehicle_Origin_To_Location_Point_Length;
        /// <summary>
        /// 橫移排序極限
        /// </summary>
        [DataMember]
        public double Traverse_Sort_Total_Length;
        /// <summary>
        /// 橫移行程極限(進入到手臂送料區的極限)
        /// </summary>
        [DataMember]
        public double In_Traverse_Limit_Total_Length;
    }
}
