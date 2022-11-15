using System;

namespace MachineAndPhoneAPI
{
    public class EnableAppPairing
    {
        public int errorCode { get; set; }  //0代表request成功且有response
        public bool data { get; set; }   //GET回傳目前是否啟用手機APP配對，true為啟用手機APP配對，false為啟用機台配對
    }
}
