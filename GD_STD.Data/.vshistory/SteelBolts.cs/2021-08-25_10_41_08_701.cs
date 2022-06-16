using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    public class SteelBolts
    {
        [TeklaBom(1)]
        public string Profile { get; set; }
        [TeklaBom(2)]
        public BOLT_TYPE type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }
}
