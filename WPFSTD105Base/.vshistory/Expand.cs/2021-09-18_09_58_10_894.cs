using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// 擴展
    /// </summary>
    public static class Expand
    {
        /// <summary>
        /// <see cref="System.Drawing.Color"/> To <see cref="System.Windows.Media.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color Color(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.R, color.G, color.B); ;
        }
        /// <summary>
        /// <see cref="System.Windows.Media.Color"/> To <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color Color(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B); ;
        }
        public static bool Contains(this string str, params string[] values)
        {
            foreach (var item in values)
            {
                if (str.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
