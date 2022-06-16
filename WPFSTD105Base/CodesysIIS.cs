using GD_STD;
using GD_STD.Properties;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using WPFSTD105.Memor;
using static WPFSTD105.ViewLocator;
using static WPFSTD105.Properties.MecSetting;
using WPFSTD105.Properties;
using static WPFSTD105.SettingHelper;
using GD_STD.Phone;
using DrillWarehouse = GD_STD.DrillWarehouse;
using WPFSTD105.ReadMemoryDuplex;
using System;
using Newtonsoft.Json;
using WPFSTD105.MonitorDuplex;
using System.Threading.Tasks;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;

namespace WPFSTD105
{
    /// <summary>
    /// Codesys IIS 
    /// </summary>
    [System.Runtime.InteropServices.Guid("4E5A5307-0987-4331-A85B-30B7C7AC6DE3")]
    public static class CodesysIIS
    {
        /// <summary>
        /// 開啟或關閉與 Codesys 記憶體操作
        /// </summary>
        public static MemorClient CodesysMemor;
        /// <summary>
        /// 寫入 Codesys 記憶體
        /// </summary>
        public static WriteMemorClient WriteCodesysMemor;
        /// <summary>
        /// 讀取 Codesys 記憶體
        /// </summary>
        public static ReadMemorClient ReadCodesysMemor;
        ///// <summary>
        ///// 手機連線狀態
        ///// </summary>
        //public static PhoneConnectDuplexClient PhoneOperating;
        /// <summary>
        /// 讀取 Codesys 記憶體 (雙向通訊)
        /// </summary>
        public static ReadMemoryDuplexClient ReadDuplexMemory;
        ///// <summary>
        ///// 加工監控
        ///// </summary>
        //public static MonitorDuplexClient MonitorClient;
        /// <summary>
        /// 軟體關閉
        /// </summary>
        public static void PCClose()
        {
            //if (Default.HMI)
            //{
            //    return;
            //}
            ApplicationViewModel.DisposeListening(); //釋放掉聆聽執行緒

            Thread.Sleep(1000);
            //通知 Codesys， PC 已關閉
            Host host = ReadCodesysMemor.GetHost();
            host.PCOpen = false;
            WriteCodesysMemor.SetHost(host);

            CodesysMemor.Abort();
            ReadCodesysMemor.Abort();
            WriteCodesysMemor.Abort();
        }

        /// <summary>
        /// 通知 IIS 開啟與 Codesys 共享的記憶體
        /// </summary>
        public static void Open()
        {
            try
            {
                CodesysMemor = new MemorClient();
                WriteCodesysMemor = new WriteMemorClient();
                ReadCodesysMemor = new ReadMemorClient();
                ReadDuplexMemory=   new ReadMemoryDuplexClient(new InstanceContext(new ReadMemoryCallbackHandler()));
                ReadDuplexMemory.InnerChannel.Faulted += InnerChannel_Faulted;
                CodesysIIS.WriteCodesysMemor.SetLogin(new Login());
                Host host = ReadCodesysMemor.GetHost();
                ApplicationViewModel.FirstOrigin = host.Origin;
                //持續偵測第一次原點復歸
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    while (!ApplicationViewModel.FirstOrigin)
                    {
                        Thread.Sleep(1000);
                        using (Memor.ReadMemorClient read = new ReadMemorClient())
                        {
                            bool origin = read.GetHost().Origin;
                            ApplicationViewModel.FirstOrigin = origin;
                        }
                    }
                });

                if (!host.PCOpen)
                {

                    STDSerialization ser = new STDSerialization();
                    FluentAPI.MecSetting mecSetting = ser.GetMecSetting();

                    FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();
                    MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(optionSettings.ToString());
                    host.Deserialize = Default.Deserialize;//通知 Codesys 反序列化
                    host.PCOpen = true;
                    CodesysMemor.Create(host, Default.Company);//創建共享記憶體
                    PanelButton panelButton = ReadCodesysMemor.GetPanel();//主要只是讀取記憶體 key  的狀態
                    PanelButton initialization = new PanelButton() //初始化
                    {
#if _DEBUG
                        Key = GD_STD.Enum.KEY_HOLE.MANUAL,
#else
                        Key = panelButton.Key,
#endif
                        Alarm = panelButton.Alarm,
                        HandSpeed = Default.HandSpeed,
                        Count = 2,
                    };
                    DrillWarehouse drillWarehouse = SetPosition(ReadCodesysMemor.GetDrillWarehouse());
                    WriteCodesysMemor.SetDrillWarehouse(drillWarehouse);
                    WriteCodesysMemor.SetMechanicalSetting(JsonConvert.DeserializeObject<MechanicalSetting>(mecSetting.ToString()));//修改機械參數
                    WriteCodesysMemor.SetMecOptional(mecOptional);//寫入選配參數
                    WriteCodesysMemor.SetPanel(initialization);//初始化面板
                }
            }
            catch (EndpointNotFoundException ex)//找不到或無法連線遠端端點時，所擲回的例外狀況
            {
                //MessageBox.Show($"{ex.InnerException.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                $"{ex.InnerException.Message}",
                "錯誤",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Popup);
                log4net.LogManager.GetLogger("嚴重錯誤").Error($"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException.Message}");
                throw;
            }
            catch (FaultException ex) //SOAP 錯誤
            {
                //MessageBox.Show($"{ex.InnerException.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                $"{ex.InnerException.Message}",
                "錯誤",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Window);
                log4net.LogManager.GetLogger("嚴重錯誤").Error($"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException.Message}");
                throw;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"{ex.InnerException.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                $"{ex.InnerException.Message}",
                "錯誤",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Popup);
                log4net.LogManager.GetLogger("嚴重錯誤").Error($"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException.Message}");
                throw;
            }
        }

        private static void InnerChannel_Faulted(object sender, EventArgs e)
        {
            (sender as  ICommunicationObject).Abort();
            ReadDuplexMemory = new ReadMemoryDuplexClient(new InstanceContext(new ReadMemoryCallbackHandler()));
            ReadDuplexMemory.InnerChannel.Faulted += InnerChannel_Faulted;

            ApplicationViewModel.PanelListening.Interrupt();
            ApplicationViewModel.PanelListening.Mode = true;
        }

        ///// <summary>
        ///// 初始化刀庫
        ///// </summary>
        ///// <param name="mecOptional"></param>
        ///// <remarks>
        ///// 通常只在軟體第一次執行的時候才會
        ///// </remarks>
        //private static void DrillWarehouseInitialization(MecOptional mecOptional)
        //{
        //    if (SofSetting.Default.DrillBrands == null)//在軟體第一次執行時才會有用
        //    {
        //        SofSetting.Default.DrillBrands = new DrillBrands();
        //        SofSetting.Default.Save();
        //    }
        //    STDSerialization ser = new STDSerialization();
        //    DrillWarehouse drill = ser.GetDrillWarehouse();
        //    drill = SetPosition(DrillWarehouse.Initialization(mecOptional));
        //    //判定是否無刀庫版本
        //    if (mecOptional.LeftExport != true)
        //        drill.LeftExport[0].IsCurrent = true;
        //    if (mecOptional.RightExport != true)
        //        drill.RightExport[0].IsCurrent = true;
        //    if (mecOptional.Middle != true)
        //        drill.Middle[0].IsCurrent = true;

        //}
    }
}
