using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    public static class Expand
    {
        /// <summary>
        /// 轉換三軸物件型態
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static PROFILE_TYPE AsAxisType(this OBJECT_TYPE type)
        {
            switch (type)
            {
                case OBJECT_TYPE.BH:
                case OBJECT_TYPE.RH:
                    return PROFILE_TYPE.H;
                case OBJECT_TYPE.CH:
                    return PROFILE_TYPE.U;
                case OBJECT_TYPE.L:
                    return PROFILE_TYPE.L;
                case OBJECT_TYPE.BOX:
                    return PROFILE_TYPE.BOX;
                case OBJECT_TYPE.PLATE:
                    return PROFILE_TYPE.PLATE;
                case OBJECT_TYPE.C:
                case OBJECT_TYPE.RB:
                case OBJECT_TYPE.FB:
                case OBJECT_TYPE.PIPE:
                case OBJECT_TYPE.Unknown:
                    throw new Exception("無法辨認斷面規格類型");
                default:
                    throw new Exception("無法辨認斷面規格類型");
            }
        }
    }
}
