using GD_STD.Base;
using System.Collections.Generic;

namespace GD_STD.Data
{
    /// <summary>
    /// Tekla 相關資訊
    /// </summary>
    public interface ITekla :  IRectangle
    {
        /// <summary>
        /// Tekla IDList
        /// </summary>
        List<int> ID { get; set; }
        /// <summary>
        /// Tekla 圖名稱
        /// </summary>
        string DrawingName { get; set; }
    }
}