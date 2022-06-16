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
    //API文件 : 變更結構
    /// <summary>
    /// 送料手臂
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-08-01" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>刪除 AutoClamp</item>
    ///             <item>刪除 MoveDown</item>
    ///             <item>刪除 MoveUp</item>
    ///             <item>新增 <see cref="StepAside"/></item>
    ///             <item>新增 <see cref="Mov"/></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    [Serializable]
    [DataContract]
    public struct Arm
    {
        /// <summary>
        /// 手臂軸向選擇
        /// </summary>
        [DataMember]
        public COORDINATE Axis;
        /// <summary>
        /// 觸發夾頭
        /// </summary>
        /// <remarks>
        /// 夾回傳 true，放回傳false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Clamping;
        //TODO: 刪除
        #region 刪除
        /// <summary>
        /// 手臂移動(退料或下降)
        /// </summary>
        /// <remarks>
        /// 移動中回傳 true，尚未移動回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool MoveDown;
        /// <summary>
        /// 手臂移動(送料或上升)
        /// </summary>
        /// <remarks>
        /// 移動中回傳 true，尚未移動回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool MoveUp;
        #endregion
        /// <summary>
        /// 手臂回原點
        /// </summary>
        /// <remarks>
        /// 正在執行回傳 true，原點復歸完畢則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Origin;
        /// <summary>
        /// 夾爪切換
        /// </summary>
        [DataMember]
        public ARM_CLAMP WhichClamp;
        /// <summary>
        /// 靠邊裝置
        /// </summary>
        /// <remarks>
        /// 推料動作回傳 true，收回則回傳 false
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool StepAside;
        /// <summary>
        /// 手臂移動模式
        /// </summary>
        [DataMember]
        public MOBILE_RACK Mov;
    }
}
