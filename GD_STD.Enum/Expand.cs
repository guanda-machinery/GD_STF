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
        public static PROFILE_TYPE AsAxisType(this OBJETC_TYPE type)
        {
            switch (type)
            {
                case OBJETC_TYPE.BH:
                case OBJETC_TYPE.RH:
                    return PROFILE_TYPE.H;
                case OBJETC_TYPE.CH:
                    return PROFILE_TYPE.U;
                case OBJETC_TYPE.L:
                    return PROFILE_TYPE.L;
                case OBJETC_TYPE.BOX:
                    return PROFILE_TYPE.BOX;
                case OBJETC_TYPE.PLATE:
                    return PROFILE_TYPE.PLATE;
                case OBJETC_TYPE.C:
                case OBJETC_TYPE.RB:
                case OBJETC_TYPE.FB:
                case OBJETC_TYPE.PIPE:
                case OBJETC_TYPE.Unknown:
                    throw new Exception("無法辨認斷面規格類型");
                default:
                    throw new Exception("無法辨認斷面規格類型");
            }
        }
    }
}
