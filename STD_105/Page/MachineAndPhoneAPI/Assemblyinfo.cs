﻿using System.Collections.Generic;

namespace MachineAndPhoneAPI
{
    public class Assemblyinfo
    {
        public class Data
        {
            public string id { get; set; }                           // ID
            //public string material { get; set; }                     // 材質
            //public string materialNumber { get; set; }               // 素材編號
            //public string profile { get; set; }                      // 斷面規格
            //public string smeltingNumber { get; set; }               // 爐號
            //public string source { get; set; }                       // 廠商
            //public string length { get; set; }                       // 長度
        }
        public int errorCode { get; set; }                           //0代表request成功且有response
        public List<Data> data { get; set; }
    }
}
