using System;
namespace WPFSTD105
{
    
#pragma warning disable CS1574 // XML 註解有無法解析的 cref 屬性 'ModelVM'
/// <summary>
    /// 提供用戶設定的 <see cref="ModelVM"/> 屬性
    /// </summary>
    [Serializable]
#pragma warning restore CS1574 // XML 註解有無法解析的 cref 屬性 'ModelVM'
    public class ModelAttr
    {
        /// <summary>
        /// 已選擇物件的顏色
        /// </summary>
        /// <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string SelectionColor { get; set; } = "ffDEB887";
        /// <summary>
        /// 原點顯示
        /// </summary>
        public bool OsVisible { get; set; }
        /// <summary>
        /// X 座標箭頭顏色
        /// </summary>
        ///  <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string ArrowColorX { get; set; } = "FF0000";
        /// <summary>
        /// Y 座標箭頭顏色
        /// </summary>
        ///  <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string ArrowColorY { get; set; } = "66ff00";
        /// <summary>
        /// Z 座標箭頭顏色
        /// </summary>
        ///  <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string ArrowColorZ { get; set; } = "0000ff";
        /// <summary>
        /// 立方圖標顏色
        /// </summary>
        ///  <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string ViewCubeIconColor { get; set; } = "ff303030";
        /// <summary>
        /// 單主件顏色
        /// </summary>
        ///  <remarks>
        /// 16進制色碼。例如 3653ff
        /// </remarks>
        public string MainColor { get; set; }
    }
}
