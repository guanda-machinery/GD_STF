using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HMI
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IDrivesInfo"。
    [ServiceContract]
    public interface IDrivesInfo
    {
        [OperationContract]
        void DoWork();
    }
}
