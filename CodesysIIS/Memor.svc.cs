using GD_STD;
using GD_STD.Base;
using GD_STD.Enum;
using GD_STD.Phone;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wsdl;
using static GD_STD.MemoryHelper;
using static GD_STD.ServerLogHelper;

namespace CodesysIIS
{
    /// <summary>
    /// 開啟與 Codesys 共同記憶體
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class Memor : IMemor, IReadMemor, IWriteMemor
    {
        /// <summary>
        /// 以二進位制模式開啟指定的檔案 
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <param name="iReadWrite"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        /// <summary>
        /// 關閉一個核心物件。其中包括檔案、檔案對映、程序、執行緒、安全和同步物件等 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("offset")]
        public int GetMonitorWorkOffset(string FieldName)
        {
            return Marshal.OffsetOf(typeof(MonitorWork), FieldName).ToInt32();
        }
        /// <summary>
        /// 查看檔案是否被佔用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileOccupied(string filePath)
        {
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            CloseHandle(vHandle);
            return vHandle == HFILE_ERROR;
        }

        /// <summary>
        /// 網際網路狀態
        /// </summary>
        /// <returns>有網路回傳 true</returns>
        public static bool NetworkStatus()
        {
            const string url = "http://www.google.com";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 建構式
        /// </summary>
        public Memor()
        {
            try
            {
                OpenSharedMemory();
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        #region Memor
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("回傳主機斷電保持的電量")]
        public BAVT BAVT()
        {
            using (var memory = BAVTMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(double)), MemoryMappedFileAccess.Read))
            {
                double result;
                memory.Read(0, out result);
                WriteInfo(WriteMemorLog, "斷電保持電量", result.ToString());
                if (result > 2.6)
                    return GD_STD.Enum.BAVT.FULL;
                else if (result > 2.5 && result <= 2.6)
                    return GD_STD.Enum.BAVT.MIDDLE;
                else
                    return GD_STD.Enum.BAVT.LOW;
            }

        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("回傳當前值，出入口的占用長度。")]
        public WorkOther GetWorkOther()
        {
            try
            {
                WorkOther result = new WorkOther();
                Type type = typeof(MonitorWork);
                long currentOffset = Marshal.OffsetOf(type, nameof(MonitorWork.Current)).ToInt64();//current 記憶體起始偏移位置
                long enOffset = Marshal.OffsetOf(type, nameof(MonitorWork.EntranceOccupy)).ToInt64();//入口占用料架偏移量
                long ex1Offset = Marshal.OffsetOf(type, nameof(MonitorWork.ExportOccupy1)).ToInt64();//出口1占用料架偏移量
                long ex2Offset = Marshal.OffsetOf(type, nameof(MonitorWork.ExportOccupy2)).ToInt64();//出口2占用料架偏移量
                result.Current = SharedMemory.GetValue<MonitorWork, short>(currentOffset, Marshal.SizeOf(typeof(short))); //當前位置
                result.EntranceOccupy = SharedMemory.GetValue<MonitorWork, double>(enOffset, Marshal.SizeOf(typeof(double))); //入口占用位置
                result.ExportOccupy1 = SharedMemory.GetValue<MonitorWork, double>(ex1Offset, Marshal.SizeOf(typeof(double))); //出口占用位置
                result.ExportOccupy2 = SharedMemory.GetValue<MonitorWork, double>(ex2Offset, Marshal.SizeOf(typeof(double))); //出口占用位置
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "讀取加工其他參數", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("回傳所有陣列除了 -1 以後的值")]
        public short[] GetIndex()
        {
            try
            {
                Type type = typeof(MonitorWork);
                long indexOffset = Marshal.OffsetOf(type, nameof(MonitorWork.Index)).ToInt64();//index 記憶體起始偏移位置
                long countOffset = Marshal.OffsetOf(type, nameof(MonitorWork.Count)).ToInt64();//數量記憶體偏移位置
                int indexSize = Marshal.SizeOf(typeof(short));
                List<short> result = new List<short>(); //index 儲存列表
                int count = SharedMemory.GetValue<MonitorWork, short>(countOffset, Marshal.SizeOf(typeof(ushort))); //目前加工陣列的數量
                for (int i = 0; i < count; i++)
                {
                    long cIndexOffet = indexOffset +(indexSize * i); //index 陣列偏移位置
                    short cIndex = SharedMemory.GetValue<MonitorWork, short>(cIndexOffet, Marshal.SizeOf(typeof(short)));
                    if (cIndex != -1) //如果不是 -1 
                    {
                        result.Add(cIndex);//加入到index 列表
                    }
                    else
                    {
                        break;
                    }
                }
                Task.Factory.StartNew(() =>
                {
                    if (result.Count > 1)
                    {
                        string info = result.Select(el => el.ToString()).Aggregate((str1, str2) => $@"{str1},{str2}");
                        WriteInfo(ReadMemorLog, "讀取 MonitorWork.Index 內容", info);
                    }
                });
                return result.ToArray();
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }

        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("接觸回傳 true，沒有接觸則回傳 false。")]
        public bool GetMaterialTouch()
        {
            bool result = GD_STD.MemoryHelper.GetMaterialTouch();
            string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
            WriteInfo(WriteMemorLog, "接觸到工件訊息", info);
            return result;
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("創建成功回傳 true，開啟失敗則回傳 false")]
        public Host Create(Host host, string company)
        {
            try
            {
                //System.Threading.Mutex g3a = new System.Threading.Mutex(true, @"..\G3AAS\g3a-api-server.exe", out bool retG3A);
                //System.Threading.Mutex g3as = new System.Threading.Mutex(true, @"..\G3AS\Guanda3AxisService.exe", out bool retG3AS);
                //if (!retG3AS)
                //{
                //    OpenExe(@"..\G3AS\Guanda3AxisService.exe");
                //}
                //if (!retG3A)
                //{
                //    OpenExe(@"..\CodesysIIS\G3AAS\g3a-api-server.exe");
                //}
                //using (var memory = CompanyMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(byte)) * 20, MemoryMappedFileAccess.Write))
                //{
                //    byte[] companyBtye = Encoding.ASCII.GetBytes(company);
                //    memory.WriteArray<byte>(0, companyBtye, 0, companyBtye.Length);
                //}

                PCSharedMemory.SetValue<Host>(host); //寫入記憶體
                //if (NetworkStatus() && !host.CodesysWrite)//如果有網際網路丟資料給 MSSQL
                //    Task.Run(() => SendBackup(new object()));//上傳尚未上傳到 MSSQL 資料庫的備份 Data

                PCSharedMemory.SetValue<Host>(host); //寫入記憶體
                string info = JsonConvert.SerializeObject(host, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(WriteMemorLog, "開啟 Codesys 共享記憶體", info);

                /*寫入公司名稱給 Codesysy*/

                return host;
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        private static void OpenExe(string fileName)
        {
            try
            {
                Process process = new Process()
                {
                    StartInfo =
                {
                    FileName = fileName,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
            }

        }

        /// <summary>
        /// 上傳尚未上傳到 MSSQL 資料庫的備份 Data
        /// </summary>
        /// <param name="Info"></param>
        public void SendBackup(object Info)
        {
            Thread.Sleep(50);

            //有人佔用檔案取消作業
            if (IsFileOccupied(FileName.CBackup))
            {
                WriteInfo(CollectLog, $"{FileName.CBackup}檔案占用中取消上傳");
                Debug.Print($"{FileName.CBackup}檔案占用中取消上傳");
                return;
            }
            try
            {
                string msdat = $"{Path.GetDirectoryName(FileName.CBackup)}\\codesys.msdat"; //Codesys 上傳到 MSSQL 的副本檔案
                                                                                            //如果沒有副本
                if (!File.Exists(msdat))
                {
                    File.Copy(FileName.CBackup, msdat);
                }
                else
                {
                    File.AppendAllText(msdat, File.ReadAllText(FileName.CBackup)); //將正本寫入到副本內
                }
                File.WriteAllText(FileName.CBackup, ""); //清除 Codesys 上傳到 MSSQL 的正本檔案的內容
                Host host = PCSharedMemory.GetValue<Host>(); //先讀取 Codesys 本機的值。再去覆蓋 PC 要寫入的值
                host.CodesysWrite = true; //完成資料處理動作，將 Codesys 上傳到 MSSQL 的正本檔案權限交還
                PCSharedMemory.SetValue<Host>(host);

                Queue<string> readRow = new Queue<string>(File.ReadAllLines(msdat));//讀取內容
                int count = 1;
                using (MecEntities mec = new MecEntities())
                {
                    mec.Configuration.AutoDetectChangesEnabled = false; //自動更新資料表

                    while (readRow.Count != 0)
                    {
                        //計算程式秒數
#if DEBUG
                        DateTime date = DateTime.Now;
                        Debug.Print(string.Format("秒數計算").PadLeft(15, '-').PadRight(30, '-'));
#endif
                        //轉換
                        object result = ConvertMS(readRow.Dequeue(), count);
                        PropertyInfo pInfo = mec.GetType().GetProperty(result.GetType().Name); //反射要存入的 table
                        Type tableType = pInfo.PropertyType.GenericTypeArguments[0];//資料表物件 type
                        mec.Set(tableType).Add(result);//儲存物件到表單內

                        File.WriteAllLines(msdat, readRow);
                        mec.SaveChanges();//存取資料
#if DEBUG
                        Debug.Print(string.Format("花費 {0} 秒", (DateTime.Now - date).TotalMilliseconds.ToString()).PadLeft(15, '-').PadRight(30, '-'));
#endif
                        count++;
                    }
                    File.Delete(msdat);//刪除副本
                    mec.Configuration.AutoDetectChangesEnabled = true; //自動更新資料表
                }
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
#if DEBUG
                throw;
#endif
            }
        }
        ///// <summary>
        ///// 取得更新過後的刀庫
        ///// </summary>
        ///// <param name="pc"></param>
        ///// <param name="phone"></param>
        ///// <returns></returns>
        //private static GD_STD.Phone.DrillWarehouse GetUpdatePhoneDrillWarehouse(GD_STD.Phone.DrillWarehouse pc, GD_STD.Phone.DrillWarehouse phone)
        //{
        //    for (int i = 0; i < phone.LeftEntrance.Length; i++)
        //    {
        //        if (pc.LeftEntrance.Length > i)
        //        {
        //            phone.LeftEntrance[i] = pc.LeftEntrance[i];
        //        }
        //        if (pc.LeftExport.Length > i)
        //        {
        //            phone.LeftExport[i] = pc.LeftExport[i];
        //        }
        //        if (pc.RightEntrance.Length > i)
        //        {
        //            phone.RightEntrance[i] = pc.RightEntrance[i];
        //        }
        //        if (pc.RightExport.Length > i)
        //        {
        //            phone.RightExport[i] = pc.RightExport[i];
        //        }
        //        if (pc.Middle.Length > i)
        //        {
        //            phone.Middle[i] = pc.Middle[i];
        //        }
        //    }
        //    return phone;
        //}
        /// <summary>
        /// 轉換 MSSQL 物件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public object ConvertMS(string value, int count)
        {
            string[] vs = value.Split(',');
            object result = null;
            if (vs.Length >= 2)//判斷是不是空值
            {
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();//反射資源
                    Type type = assembly.GetType($"CodesysIIS.{vs[0]}");//反射物件取得 type
                                                                        //找不到物件Type
                    if (type == null)
                    {
                        throw new Exception("組件內找不到指定的 type。");
                    }
                    result = Activator.CreateInstance(type);//創建類型
                    var properties = type.GetProperties().Where(el => el.Name != "ID").ToList();//資料行 ID 是系統預設所以不需要變更
                                                                                                //#if DEBUG
                                                                                                //                    JObject root = new JObject(), jvalue = new JObject();
                                                                                                //#endif
                                                                                                //                    Parallel.For(0, properties.Count, (int i) =>
                                                                                                //                      {
                                                                                                //                          object propertyValue = Convert.ChangeType(vs[i + 1], properties[i].PropertyType);//反射出屬性 TYPE ，並轉換
                                                                                                //                          properties[i].SetValue(result, propertyValue);//存取值
                                                                                                //#if DEBUG
                                                                                                //                          jvalue.Add(properties[i].Name, vs[i + 1]);
                                                                                                //                          Debug.Print($"{properties[i].Name} in {i + 1}");
                                                                                                //#endif
                                                                                                //                      });
#if DEBUG
                    JObject root = new JObject(), jvalue = new JObject();
#endif
                    for (int i = 0; i < properties.Count(); i++)
                    {
                        object propertyValue = Convert.ChangeType(vs[i + 1], properties[i].PropertyType);//反射出屬性 TYPE ，並轉換
                        properties[i].SetValue(result, propertyValue);//存取值
#if DEBUG
                        jvalue.Add(properties[i].Name, vs[i + 1]);
                        Debug.Print($"{properties[i].Name} in {i + 1}");
#endif
                    }
#if DEBUG
                    root.Add($"{vs[0]}", jvalue);
                    string info = JsonConvert.SerializeObject(root, Formatting.Indented);
                    WriteInfo(CollectLog, $"{vs[0]} 數據行在第{count + 1}。", info); //參數訊息轉為 Json 方便寫入 Log
#endif
                    Debug.Print($"{vs[0]} 數據行在第{count + 1}。\n{info}");
                }
                catch (Exception ex)
                {
                    WriteError(CollectLog, ex);
                    Debug.Print(ex.Message);
#if DEBUG
                    throw;
#endif
                }
            }
            return result;
        }
        #endregion Memor

        #region ReadMemor
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("APP 及時監控參數")]
        public GD_STD.Phone.MonitorMec GetAPPMonitorMec()
        {
            try
            {
                GD_STD.Phone.MonitorMec result = PCSharedMemory.GetValue<GD_STD.Phone.MonitorMec>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented }); //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "APP 及時監控參數", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("取得 APP 手動操作")]
        public GD_STD.Phone.APP_Struct GetAPP_Struct()
        {
            try
            {
                GD_STD.Phone.APP_Struct result = PCSharedMemory.GetValue<GD_STD.Phone.APP_Struct>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented }); //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "APP 手動操作狀態", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("目前 Codesys 操控面板 IO 狀態")]
        public PanelButton GetPanel()
        {
            try
            {
                PanelButton result = PCSharedMemory.GetValue<PanelButton>(); //讀取記憶體

                if (!Enum.IsDefined(typeof(ERROR_CODE), result.Alarm))
                {
                    result.Alarm = ERROR_CODE.Unknown;
                    PCSharedMemory.SetValue<PanelButton>(result);
                    result = PCSharedMemory.GetValue<PanelButton>();
                    try
                    {
                        if (result.Alarm != ERROR_CODE.Unknown)
                        {
                            throw new Exception();
                        }
                    }
#pragma warning disable CS0168 // 已宣告變數 'ex'，但從未使用過它
                    catch (Exception ex)
#pragma warning restore CS0168 // 已宣告變數 'ex'，但從未使用過它
                    {
                        throw;
                    }
                }
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented }); //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "操控面板 IO 狀態", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的刀庫")]
        public GD_STD.DrillWarehouse GetDrillWarehouse()
        {
            try
            {
                GD_STD.Phone.APP_Struct app = PCSharedMemory.GetValue<GD_STD.Phone.APP_Struct>(); //讀取記憶體
                GD_STD.DrillWarehouse result = new GD_STD.DrillWarehouse(app.DrillWarehouse);
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "用戶在 Codesys 設定的刀庫", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的完整油壓系統參數")]
        public OillSystem GetOill()
        {
            try
            {
                OillSystem result = PCSharedMemory.GetValue<OillSystem>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "用戶在 Codesys 設定的完整油壓系統參數", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的液壓油系統參數")]
        public HydraulicSystem[] GetHydraulic()
        {
            try
            {
                OillSystem result = PCSharedMemory.GetValue<OillSystem>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result.HydraulicSystem, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "用戶在 Codesys 設定的液壓油系統參數", info);
                return result.HydraulicSystem;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的潤滑油系統參數")]
        public LubricantSystem GetLubricant()
        {
            try
            {
                OillSystem result = PCSharedMemory.GetValue<OillSystem>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result.LubricantSystem, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "用戶在 Codesys 設定的潤滑油系統參數", info);
                return result.LubricantSystem;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的切削油系統參數")]
        public CutOilSystem GetCut()
        {
            try
            {
                OillSystem result = PCSharedMemory.GetValue<OillSystem>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result.CutOilSystem, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "用戶在 Codesys 設定的切削油系統參數", info);
                return result.CutOilSystem;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("目前 Codesys 軸向訊息")]
        public AxisInfo GetAxisInfo()
        {
            try
            {
                AxisInfo result = PCSharedMemory.GetValue<AxisInfo>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "目前 Codesys 軸向訊息", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("目前 PC 與 Codesys 主機交握狀態")]
        public Host GetHost()
        {
            try
            {
                Host result = PCSharedMemory.GetValue<Host>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "目前 PC 與 Codesys 主機交握狀態", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("目前的斷電保持數值")]
        public Outage GetOutage()
        {
            try
            {
                Outage result = PCSharedMemory.GetValue<Outage>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "目前斷電保持的數值", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("文字錯誤代碼")]
        public string GetUnknownCode()
        {
            try
            {
                using (var memory = UnknownMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(byte)) * 10, MemoryMappedFileAccess.Read))
                {
                    byte[] bytes = new byte[10];
                    memory.ReadArray(0, bytes, 0, bytes.Length);
                    string result = Encoding.ASCII.GetString(bytes).TrimStart('\0');
                    WriteInfo(ReadMemorLog, "文字錯誤代碼", result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("APP 控制訊號")]
        public GD_STD.Phone.Operating GetOperating()
        {
            try
            {
                GD_STD.Phone.Operating result = PCSharedMemory.GetValue<GD_STD.Phone.Operating>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "目前 APP 控制訊號", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("如果沒專案回傳 null，有專案則回傳 MonitorWork.ProjectName。")]
        public string GetProjectName()
        {
            try
            {
                long offset = SharedMemory.GetMemoryOffset(typeof(MonitorWork), $"{nameof(MonitorWork.ProjectName)}");
                string fieldName = nameof(MonitorWork.ProjectName);
                int readLength = typeof(MonitorWork).ArrayLength(nameof(MonitorWork.ProjectName));
                char[] vs = SharedMemory.ReadChar<MonitorWork>(offset, readLength, fieldName);
                string result = new string(vs.Where(el => el != '\0').ToArray());
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "取得專案名稱", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("GD_STD.Phone.MonitorWork.WorkMaterial[index]")]
        public WorkMaterial GetWorkMaterial(ushort index)
        {
            try
            {
                Type type = typeof(MonitorWork);
                long workOffset = Marshal.OffsetOf(type, nameof(MonitorWork.WorkMaterial)).ToInt64(); //加工陣列記憶體起始位置偏移位置
                int workSize = Marshal.SizeOf(typeof(WorkMaterial)); //WorkMaterial 結構大小 
                long cWorkOffset = workOffset + (workSize * index);
                WorkMaterial result = SharedMemory.GetValue<MonitorWork, WorkMaterial>(cWorkOffset, Marshal.SizeOf(typeof(WorkMaterial)));
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, $"GD_STD.Phone.MonitorWork.WorkMaterial[{index}]", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        #endregion ReadMemor

        #region WriteMemor
        /// <inheritdoc/>
        public void SetPanel([WsdlParamOrReturnDocumentation("要修改的值")] PanelButton value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<PanelButton>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 修改人機操控面板按 IO 狀態", info);
                //寫入參數到手機共享記憶體
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetDrillWarehouse([WsdlParamOrReturnDocumentation("要修改的值")] GD_STD.DrillWarehouse value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log

                GD_STD.Phone.APP_Struct app = PCSharedMemory.GetValue<GD_STD.Phone.APP_Struct>();

                if (value.LeftEntrance.Length != 0)
                    app.DrillWarehouse.LeftEntrance = value.LeftEntrance;
                if (value.LeftExport.Length != 0)
                    app.DrillWarehouse.LeftExport = value.LeftExport;
                if (value.RightEntrance.Length != 0)
                    app.DrillWarehouse.RightEntrance = value.RightEntrance;
                if (value.RightExport.Length != 0)
                    app.DrillWarehouse.RightExport = value.RightExport;
                if (value.Middle.Length != 0)
                    app.DrillWarehouse.Middle = value.Middle;

                PCSharedMemory.SetValue<APP_Struct>(app);
                //app.DrillWarehouse = GetUpdatePhoneDrillWarehouse(value, app.DrillWarehouse);
                WriteInfo(WriteMemorLog, "修改 Codesys 刀庫設定", info);

            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetOill([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<OillSystem>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 完整油壓系統設定", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetHydraulic([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value)
        {
            try
            {
                OillSystem result = new OillSystem();
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                ((IOillSystem)result).WriteHydraulic(); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 液壓油系統參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetLubricant([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value)
        {
            try
            {
                OillSystem result = new OillSystem();
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                ((IOillSystem)result).WriteLubricant(); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 潤滑油系統參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetCut([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value)
        {
            try
            {
                OillSystem result = new OillSystem();
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
                ((IOillSystem)result).WriteCut(); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 切削油系統參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetHost([WsdlParamOrReturnDocumentation("要修改的值")] Host value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<Host>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 PC 主機狀態", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetOutage(Outage value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<Outage>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 Codesys 斷電保持數值", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetAPPStruct([WsdlParamOrReturnDocumentation("修改 APP 手動操作參數")] GD_STD.Phone.APP_Struct value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<GD_STD.Phone.APP_Struct>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 APP 手動操作參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetAPPMonitorMec([WsdlParamOrReturnDocumentation("修改 APP 及時監控參數")] GD_STD.Phone.MonitorMec value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<GD_STD.Phone.MonitorMec>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 APP 及時監控參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetMechanicalSetting([WsdlParamOrReturnDocumentation("修改機械設定參數")] GD_STD.Phone.MechanicalSetting value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<GD_STD.Phone.MechanicalSetting>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改機械參數設定", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetPhoneOperating([WsdlParamOrReturnDocumentation("修改連線狀態參數")] GD_STD.Phone.Operating value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<GD_STD.Phone.Operating>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改手機連線狀態參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetMecOptional([WsdlParamOrReturnDocumentation("修改選配參數")] GD_STD.Phone.MecOptional value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<GD_STD.Phone.MecOptional>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改選配參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetMonitorWorkOffset([WsdlParamOrReturnDocumentation("修改參數")] byte[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset)
        {
            PCSharedMemory.SetValue<MonitorWork>(offset, value);
        }
        /// <inheritdoc/>
        public void SetWorkMaterial([WsdlParamOrReturnDocumentation("修改參數")] WorkMaterial[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset)
        {
            for (int i = 0; i < value.Length; i++)
            {
                PCSharedMemory.SetValue<MonitorWork>(offset * (i + 1), value[i].ToByteArray());
            }
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Memor.Game()' 的 XML 註解
        public void Game()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Memor.Game()' 的 XML 註解
        {
            Host value = PCSharedMemory.GetValue<Host>();
            value.Withdraw = true;
            PCSharedMemory.SetValue<Host>(value);
        }

        //public void PortGame(string port)
        //{
        //    try
        //    {
        //        byte[] value = new byte[20];
        //        byte[] write = Encoding.ASCII.GetBytes(port);
        //        Array.Copy(write, value, write.Length);
        //        Game _ = new Game();
        //        _.vs = value;
        //        _.Run = true;
        //        string info = JsonConvert.SerializeObject(port); //參數訊息轉為 Json 方便寫入 Log
        //        PCSharedMemory.SetValue<Game>(_); //寫入記憶體
        //        WriteInfo(WriteMemorLog, "修改選配參數", port);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError(WriteMemorLog, ex);
        //        throw;
        //    }
        //}

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Memor.DeleteGame()' 的 XML 註解
        public void DeleteGame()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Memor.DeleteGame()' 的 XML 註解
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.StandardInput.WriteLine($"netsh advfirewall firewall delete rule name=\"檔案列印共用\"");
            p.StandardInput.WriteLine("exit"); //需要有這句，不然程式會掛機
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Memor.GetAndroidTest()' 的 XML 註解
        public AndroidTest GetAndroidTest()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Memor.GetAndroidTest()' 的 XML 註解
        {
            try
            {
                AndroidTest result = PCSharedMemory.GetValue<AndroidTest>(); //讀取記憶體
                string info = JsonConvert.SerializeObject(result, new JsonSerializerSettings { Formatting = Formatting.Indented }); //參數訊息轉為 Json 方便寫入 Log
                WriteInfo(ReadMemorLog, "AndroidTest", info);
                return result;
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
        public void SetAndroidTest(AndroidTest value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                PCSharedMemory.SetValue<AndroidTest>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "修改 APP 手動操作參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void SetLogin([WsdlParamOrReturnDocumentation("修改連線登入狀態")] Login value)
        {
            try
            {
                string info = JsonConvert.SerializeObject(value); //參數訊息轉為 Json 方便寫入 Log
                GD_STD.Phone.SharedMemory.SetValue<Login>(value); //寫入記憶體
                WriteInfo(WriteMemorLog, "初始化登入參數", info);
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("Codesys 回復訊息 Unicode UTF-16")]
        public ushort[] GetInstantMessage()
        {
            return GD_STD.Phone.MemoryHelper.GetInstantMessage();
        }

        public void IniWork()
        {
            Task.Run(() => PCSharedMemory.SetValue<MonitorWork>(0, new byte[Marshal.SizeOf(typeof(MonitorWork))]));

        }

        #endregion WriteMemor
    }
}