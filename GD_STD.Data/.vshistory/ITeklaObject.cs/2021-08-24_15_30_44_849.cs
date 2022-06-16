using GD_STD.Base;
using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface ITekla : IPart
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