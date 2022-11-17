using System.Collections.Generic;

namespace MachineAndPhoneAPI.Models
{
    public class RegisterAssembly
    {
        //加工組合清單之素材內容
        public int errorCode { get; set; }

        public class Data
        {
            public string id { get; set; }
            public int errorCode { get; set; }

            public string errorMessage { get; set; }
        }
        public List<Data> data { get; set; }
    }
}
