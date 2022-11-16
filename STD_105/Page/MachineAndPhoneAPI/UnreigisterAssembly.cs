using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineAndPhoneAPI
{
    public class UnreigisterAssembly
    {
        public int errorCode { get; set; }      ////0代表request成功且有response
        public class Data
        {
            public string id { get; set; }
        }
        public List<Data> data;
    }
}
