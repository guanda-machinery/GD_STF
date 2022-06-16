using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;
using GD_STD;
namespace GuandaMechanical
{
    public class ReadMemoryServer : IReadMemoryServer
    {
        /// <summary>
        /// Gets the Callback.
        /// </summary>
        public IReadMemoryDuplexCallback Callback;
        public void GetPanel()
        {
            Callback.SendPanel(PCSharedMemory.GetValue<PanelButton>());
        }
        /// <summary>
        /// 解構式
        /// </summary>
        ~ReadMemoryServer()
        {

        }
    }
    public interface IReadMemoryDuplexCallback
    {
        void SendPanel(PanelButton panelButton);
    }
}
