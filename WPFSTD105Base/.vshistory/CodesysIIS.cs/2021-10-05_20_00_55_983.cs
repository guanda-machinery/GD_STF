using GD_STD;
using GD_STD.Properties;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using WPFSTD105.Memor;
using static WPFSTD105.ViewLocator;
using static WPFSTD105.Properties.MecSetting;
using WPFSTD105.Properties;
using WPFSTD105.PhoneMemor;
using static WPFSTD105.SettingHelper;
using GD_STD.Phone;
using DrillWarehouse = GD_STD.DrillWarehouse;

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
        public static MemorClient CodesysMemor = new MemorClient();
        /// <summary>
        /// 寫入 Codesys 記憶體
        /// </summary>
        public static WriteMemorClient WriteCodesysMemor = new WriteMemorClient();
        /// <summary>
        /// 讀取 Codesys 記憶體
        /// </summary>
        public static ReadMemorClient ReadCodesysMemor = new ReadMemorClient();
        /// <summary>
        /// 手機連線狀態
        /// </summary>
        public static PhoneConnectDuplexClient PhoneOperating = new PhoneConnectDuplexClient(new InstanceContext(new PhoneCallbackHandler()));
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
                CodesysMemor.Open();
                WriteCodesysMemor.Open();
                ReadCodesysMemor.Open();
                PhoneOperating.Open();
//                Host host = ReadCodesysMemor.GetHost();
//                MecOptional mecOptional = new MecOptional();
//                mecOptional.HandAuto = Optional.Default.HandAuto;//自動手臂選配
//                /*刀庫選配*/
//                mecOptional.LeftEntrance = Optional.Default.LeftEntrance;
//                mecOptional.LeftExport = Optional.Default.LeftExport;
//                mecOptional.Middle = Optional.Default.Middle;
//                mecOptional.RightEntrance = Optional.Default.RightEntrance;
//                mecOptional.RightExport = Optional.Default.RightExport;

//                host.Deserialize = Properties.MecSetting.Default.Deserialize;//通知 Codesys 反序列化
//                host.PCOpen = true;

//                CodesysMemor.Create(host, Default.Company);//創建共享記憶體
//                DrillWarehouseInitialization(mecOptional); //初始化刀庫
//                PanelButton panelButton = ReadCodesysMemor.GetPanel();//主要只是讀取記憶體 key  的狀態
//                PanelButton initialization = new PanelButton() //初始化
//                {
//#if _DEBUG
//                    Key = GD_STD.Enum.KEY_HOLE.MANUAL,
//#else
//                                    Key = panelButton.Key,
//#endif
//                    Alarm = panelButton.Alarm,
//                    HandSpeed = Default.HandSpeed,
//                };
//                WriteCodesysMemor.SetDrillWarehouse(SetPosition(Default.DrillWarehouse));//寫入刀庫資料
//                WriteCodesysMemor.SetPanel(initialization);//初始化面板
//                WriteCodesysMemor.SetMechanicalSetting(GetMechanicalSetting());//修改機械參數
//                WriteCodesysMemor.SetMecOptional(mecOptional);//寫入選配參數
                //監聽手機連線
                PhoneOperating.SetSleepListening(700);
                PhoneOperating.RunListening(true);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show($"{ex.InnerException.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                log4net.LogManager.GetLogger("嚴重錯誤").Error($"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException.Message}");
                throw;
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"{ex.InnerException.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                log4net.LogManager.GetLogger("嚴重錯誤").Error($"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException.Message}");
                throw;
            }
        }
        /// <summary>
        /// 刀庫初始化
        /// </summary>
        /// <param name="mecOptional"></param>
        /// <remarks>
        /// 通常只在軟體第一次執行的時候才會
        /// </remarks>
        private static void DrillWarehouseInitialization(MecOptional mecOptional)
        {
            Default.DrillWarehouse = SetPosition(DrillWarehouse.Initialization(mecOptional));
            //判定是否無刀庫版本
            if (mecOptional.LeftExport != true)
                Default.DrillWarehouse.LeftExport[0].IsCurrent = true;
            if (mecOptional.RightExport != true)
                Default.DrillWarehouse.RightExport[0].IsCurrent = true;
            if (mecOptional.Middle != true)
                Default.DrillWarehouse.Middle[0].IsCurrent = true;
            Default.Save();
        }
    }
}
