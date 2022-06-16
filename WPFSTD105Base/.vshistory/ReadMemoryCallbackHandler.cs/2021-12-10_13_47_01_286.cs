using GD_STD;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.ReadMemoryDuplex;

namespace WPFSTD105
{
    /// <summary>
    /// <see cref="ReadMemoryDuplexClient"/> 回調處理程序
    /// </summary>
    public class ReadMemoryCallbackHandler : IReadMemoryDuplexCallback
    {
        /// <summary>
        /// 接收 PC 與 Codesys 主機交握狀態
        /// </summary>
        /// <param name="host"></param>
        public void SendHost(Host host)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 接收帳號登入狀態
        /// </summary>
        /// <param name="login"></param>
        public void SendLogin(Login login)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 接收三軸追隨座標位置
        /// </summary>
        /// <param name="axisLocation"></param>
        public void SendMainAxisLocation(MainAxisLocation axisLocation)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 接收 Codesys 功能面板目前訊息
        /// </summary>
        /// <param name="panelButton"></param>
        public void SendPanel(PanelButton panelButton)
        {
            throw new NotImplementedException();
        }
    }
}
