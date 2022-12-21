using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineAndPhoneAPI.Models
{
    public class UnreigisterAssembly
    {
        public int errorCode { get; set; }      ////0代表request成功且有response

        public List<string> data;
    }
}
