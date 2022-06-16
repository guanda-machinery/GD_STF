using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    /// <summary>
    /// 送料手臂
    /// </summary>
    [Serializable]
    [DataContract]
    public struct Arm
    {
        /// <summary>
        /// 觸發自動夾料
        /// </summary>
        /// <remarks>
        /// 啟動回傳 true，尚未啟動則回傳false。
        /// </remarks>
        #region 服務合約
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        #endregion
        public bool AutoClamp;
        /// <summary>
        /// 手臂軸向選擇
        /// </summary>
        #region 服務合約
        [DataMember]
        #endregion
        public COORDINATE Axis;
        /// <summary>
        /// 觸發夾頭
        /// </summary>
        /// <remarks>
        /// 夾回傳 true，放回傳false。
        /// </remarks>
        #region 服務合約
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        #endregion
        public bool Clamping;
        /// <summary>
        /// 手臂移動(退料或下降)
        /// </summary>
        /// <remarks>
        /// 移動中回傳 true，尚未移動回傳 false。
        /// </remarks>
         #region 服務合約
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        #endregion
        public bool MoveDown;
        /// <summary>
        /// 手臂移動(送料或上升)
        /// </summary>
        /// <remarks>
        /// 移動中回傳 true，尚未移動回傳 false。
        /// </remarks>
       #region 服務合約
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        #endregion
        public bool MoveUp;
        /// <summary>
        /// 手臂回原點
        /// </summary>
        /// <remarks>
        /// 正在執行回傳 true，原點復歸完畢則回傳 false。
        /// </remarks>
        #region 服務合約
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        #endregion
        public bool Origin;
        /// <summary>
        /// 夾爪切換
        /// </summary>
        #region 服務合約
        [DataMember]
        #endregion
        public ARM_CLAMP WhichClamp { get; set; }
    }
}
