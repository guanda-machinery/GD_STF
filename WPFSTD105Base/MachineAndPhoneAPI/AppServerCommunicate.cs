using DevExpress.Xpf.Core.Native;
using MachineAndPhoneAPI;
using MachineAndPhoneAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static MachineAndPhoneAPI.AppServerCommunicate;

namespace MachineAndPhoneAPI
{
    /// <summary>
    /// 機台與appserver通訊 port = {AppServerPort}
    /// </summary>
    public class AppServerCommunicate
    {
        private static string _AppServerIP;
        public static string AppServerIP
        { 
            get
            {
                if (_AppServerIP == null)
                {
                    var _IPAddress = WPFSTD105.Properties.SofSetting.Default.Address;
                    var ColonIndex = _IPAddress.IndexOf(":");
                    if (ColonIndex != -1)
                    {
                        //移除port
                        _IPAddress = _IPAddress.Remove(ColonIndex);
                    }
                    _AppServerIP = _IPAddress;
                }
                return _AppServerIP;
            }
            set
            {
                _AppServerIP = value;
            }
        }
        /// <summary>
        /// appserver port
        /// </summary>
        public static int AppServerPort = 8084;

        #region Token
        const string token_access = "83ic7f4394nvnqf4vg7grr99b6yn6yn8r96bay7bveemyns9";
        #endregion
        /// <summary>
        /// 啟用手機配對
        /// </summary>
        public static bool SetPhoneEnableAppPairing()
        {
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/enable-app-pairing");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("enable", "true");
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<EnableAppPairing>(response.Content);
                Console.WriteLine(result);
                if (result.errorCode == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 啟用機台配對
        /// </summary>
        public static bool SetMachineenableAppPairing()
        {
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/enable-app-pairing");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            request.AddParameter("enable", "false");
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<EnableAppPairing>(response.Content);
                Console.WriteLine(result);
                if(result.errorCode == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查詢目前手機APP/機台配對模式是否開啟
        /// </summary>
        public static bool GetEnableAppPairing(out bool PhonePairingMode)
        {
            PhonePairingMode = false;
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/enable-app-pairing");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<EnableAppPairing>(response.Content);
                Console.WriteLine(result);
                PhonePairingMode = result.data;
                              if(result.errorCode == 0){return true;}else{ return false; }    ;
            }
            else
            {
                return false;
            }
        }


        public static string StringToBase64Converter(string SourceString)
        {
           return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(SourceString.ToCharArray()));
        }
        /// <summary>
        /// 機台呼叫註冊配料
        /// </summary>
        public static bool SetRegisterAssembly(string ProjectName,string Register_MaterialNumber, string Register_material, string Register_profile, double Register_length , out RegisterAssembly result)
        {
            result=new RegisterAssembly();
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/register-material");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            PreProcessing _preProcessing = new PreProcessing();
            _preProcessing.matchMaterial = false;
            _preProcessing.insert = false;
            _preProcessing.data = new List<DataList>();

            var PID = StringToBase64Converter(ProjectName) + Register_MaterialNumber;

            var ReadyToRegisterMaterialData = new DataList() { materialNumber = Register_MaterialNumber, smeltingNumber = "1", source = "1", length = Register_length.ToString(), id = PID, material = Register_material, profile = Register_profile };
            _preProcessing.data.Add(ReadyToRegisterMaterialData);

             var body = JsonConvert.SerializeObject(_preProcessing);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                result = JsonConvert.DeserializeObject<RegisterAssembly>(response.Content);
                Console.WriteLine(result);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 機台呼叫註銷配料
        /// </summary>
        public static bool UnregisterAssembly(string ProjectName , List<string> MaterialNumberList, out UnreigisterAssembly result)
        {
            if(MaterialNumberList.Count ==0)
            {
                result = new UnreigisterAssembly();
                return true;
            }

            var IDListStringList = new List<string>();
            //@" ""0001"" + , +""0002"" + , +""0003"" "
            foreach (var _MNL in MaterialNumberList)
                IDListStringList.Add(StringToBase64Converter(ProjectName) + _MNL);
            


            result =new UnreigisterAssembly();
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/unregister-material");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            var UNregClass = new UnregisterClass();
            UNregClass.idList = IDListStringList;
            var body = JsonConvert.SerializeObject(UNregClass);

            /*var body = @"{
                             " + "\n" +
                            $@" ""idList"": [{IDListString} + , + ""nulldata""]
                             " + "\n" +
                            @"}";*/

            //$@"  ""idList"": [ ""0001"" + , + ""0002"" + , + ""0003"" ]
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                result = JsonConvert.DeserializeObject<UnreigisterAssembly>(response.Content);
                Console.WriteLine(result);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 輪詢加工進度表中手機掃描之配料
        /// </summary>
        public static bool GetAppPairingData(out Assemblyinfo result)
        {
            result = new Assemblyinfo();
            //            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/pc-material-list"); 
            //            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/app-material-list");
            var client = new RestClient($"http://{AppServerIP}:{AppServerPort}/api/pc/material-list");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                result = JsonConvert.DeserializeObject<Assemblyinfo>(response.Content);
                Console.WriteLine(result);
                if(result.errorCode == 0)
                    return true;
                else
                    return false;     
            }
            else
            {
                return false;
            }
        }


        public class PreProcessing
        {
            public bool matchMaterial { get; set; }
            public bool insert { get; set; }
            public List<DataList> data { get; set; }
        }

        public class DataList
        {
            public string id { get; set; }
            public string material { get; set; }
            public string materialNumber { get; set; }
            public string profile { get; set; }
            public string smeltingNumber { get; set; }
            public string source { get; set; }
            public string length { get; set; }
        }


        public class UnregisterClass
        {
            public List<string> idList { get; set; }
        }






    }
}
