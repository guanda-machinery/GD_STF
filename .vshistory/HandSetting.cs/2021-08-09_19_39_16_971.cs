//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace GD_STD
//{
//    /// <summary>
//    /// 手臂設定相關參數
//    /// </summary>
//    [DataContract]
//    public struct HandSetting
//    {
//        /// <summary>
//        /// 補正的值
//        /// </summary>
//        /// <remarks>
//        /// 補正 <see cref="MaterialZeroToWorkRange"/> 的值
//        /// <para>
//        /// 客戶端調整。
//        /// </para>
//        /// </remarks>
//        [DataMember]
//        public double MaterialCorrectionWorkRange { get; set; }
//        /// <summary>
//        /// 送料原點到加工第一點(X=300)的長度,預設500
//        /// </summary>
//        /// <remarks>
//        /// 初次送料到加工點，工件原點到加工點的值。
//        /// <para>
//        /// 工程模式調整。
//        /// </para>
//        /// </remarks>
//        [DataMember]
//        public double MaterialZeroToWorkRange { get; set; }
//        /// <summary>
//        /// 手臂工作範圍
//        /// </summary>
//        /// <remarks>
//        /// 手臂移動行程
//        /// </remarks>
//        [DataMember]
//        public double HandJobLimit { get; set; }
//        /// <summary>
//        /// 出口下壓夾料到加工0點的長度,下壓零件與主軸刀徑有錯開。
//        /// 意思是加工若可以做300,送料須加上此段長度才能讓出口處下壓固定處夾持
//        /// </summary>
//        [DataMember]
//        public double OutClampToZeroLength { get; set; }
//    }
//}
