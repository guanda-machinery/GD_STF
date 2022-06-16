using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wsdl;

namespace GuandaContract
{
    [ServiceContract(Namespace = "http://Codesys.PhoneConnectDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IReadMemoryDuplexCallback))]
    public interface IReadMemoryServer
    {
        [OperationContract(IsOneWay = true)]
        void GetPanel();
    }
    public interface IReadMemoryDuplexCallback
    {
        void SendPanel(PanelButton panelButton);
    }
}
