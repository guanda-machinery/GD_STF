using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MachineAndPhoneAPI
{
    public static class Helper
    {
        #region Token
        const string token_access = "83ic7f4394nvnqf4vg7grr99b6yn6yn8r96bay7bveemyns9";
        #endregion
        /// <summary>
        /// 啟用手機配對
        /// </summary>
        public static EnableAppPairing PhoneenableAppPairing()
        {
            var client = new RestClient("http://192.168.31.152:8084/api/pc/enable-app-pairing");
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
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 啟用機台配對
        /// </summary>
        public static EnableAppPairing MachineenableAppPairing()
        {
            var client = new RestClient("http://192.168.31.152:8084/api/pc/enable-app-pairing");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            request.AddParameter("enable", "false");
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<EnableAppPairing>(response.Content);
                Console.WriteLine(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查詢目前手機APP/機台配對狀態
        /// </summary>
        public static EnableAppPairing GetenableAppPairing()
        {
            var client = new RestClient("http://192.168.31.152:8084/api/pc/enable-app-pairing");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<EnableAppPairing>(response.Content);
                Console.WriteLine(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 機台呼叫註冊配料
        /// </summary>
        public static RegisterAssembly RegisterAssembly(string ProjectName,string Register_id, string Register_material, string Register_MaterialNumber, string Register_profile, int Register_length)
        {
            var client = new RestClient("http://192.168.31.152:8084/api/pc/register-material");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            PreProcessing _preProcessing = new PreProcessing();
            _preProcessing.matchMaterial = false;
            _preProcessing.insert = false;
            _preProcessing.data = new List<DataList>();
            var ProjectNameBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ProjectName.ToCharArray()));
            {
                new DataList() {materialNumber = Register_MaterialNumber,  smeltingNumber = "1" , source = "1", length = Register_length.ToString(), id = ProjectNameBase64 + Register_id, material = Register_material , profile = Register_profile}
            };
            var body = JsonConvert.SerializeObject(_preProcessing);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<RegisterAssembly>(response.Content);
                Console.WriteLine(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 機台呼叫註銷配料
        /// </summary>
        public static UnreigisterAssembly UnregisterAssembly()
        {
            var client = new RestClient("http://192.168.31.152:8084/api/pc/unregister-material");
            client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
            var request = new RestRequest(Method.POST);
            var body = @"{
                             " + "\n" +
                            @"  ""idList"": [ ""0001"" + , + ""0002"" + , + ""0003"" ]
                             " + "\n" +
                            @"}";
            var response = client.Execute(request);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<UnreigisterAssembly>(response.Content);
                Console.WriteLine(result);
                return result;
            }
            else
            {
                return null;
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
        public class Polling
        {
            /// <summary>
            /// 5秒執行輪詢一次
            /// </summary>
            public static void GetAppPairingDataEveryFiveSeconds()
            {
                Task.Factory.StartNew(() =>
                {
                    //if ()//進入加工監控畫面
                    {
                        Thread.Sleep(5000);
                        GetAppPairingData();
                    }
                });
            }

            /// <summary>
            /// 輪詢加工進度表中手機掃描之配料
            /// </summary>
            public static Assemblyinfo GetAppPairingData()
            {
                var client = new RestClient("http://192.168.31.152:8084/api/pc/material-list");
                client.AddDefaultHeader("Authorization", $"Bearer {token_access}");
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<Assemblyinfo>(response.Content);
                    Console.WriteLine(result);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
