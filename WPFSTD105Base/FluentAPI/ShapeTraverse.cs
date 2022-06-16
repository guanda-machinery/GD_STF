using GD_STD.Attribute;
using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 可以規劃擺放移動料架的結構，
    /// Y 或 U 型 
    /// </summary>
    [Serializable]
    public class ShapeTraverse : Traverse
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public ShapeTraverse()
        {
            ConveyorPosition = new double[4];
        }
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。最大4個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM("可排放物件的座標", false)]
        public double[] ConveyorPosition { get; set; }
        /// <summary>
        /// 安全距離
        /// </summary>
        [MVVM("安全距離", false)]
        public double Safety { get; set; }
    }
}
