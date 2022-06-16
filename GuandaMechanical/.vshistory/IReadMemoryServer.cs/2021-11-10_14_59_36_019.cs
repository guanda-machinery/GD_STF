using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wsdl;

namespace GuandaMechanical
{
    [ServiceContract(Namespace = "http://Codesys.PhoneConnectDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IPhoneConnectDuplexCallback))]
    [WsdlDocumentation("手機連線服務合約")]
    interface IReadMemoryServer
    {
    }
}
