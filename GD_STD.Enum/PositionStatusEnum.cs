using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{   
    
    //機台配對:使用api完成之配對
    //手動配對:使用codesys完成之配對
    public enum PositionStatusEnum
    {
        初始化,
        未取得狀態,
        完成,
        等待出料,
        等待入料,
        加工中,
        不可配對,
        等待配對,
        手機配對,
        軟體配對,
        手動配對,
    }
}
