﻿using GD_STD;
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
        public static PhoneConnectDuplexClient PhoneOperating = new PhoneConnectDuplexClient(new InstanceContext(new CallbackHandler()));
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
        //private static Axis3D ToAxis3D(string position)
        //{
        //    string[] vs = position.Split(',');
        //    double xv = 0, yv = 0, zv = 0;
        //    double.TryParse(vs[0], out xv);
        //    double.TryParse(vs[1], out yv);
        //    double.TryParse(vs[2], out zv);
        //    return new Axis3D(xv, yv, zv);
        //}
        /// <summary>
        /// 通知 IIS 開啟與 Codesys 共享的記憶體
        /// </summary>
        public static void Open()
        {
            try
            {
                /*等待 Codesys all open 再給軟體 open */
                CodesysMemor.Open();
                WriteCodesysMemor.Open();
                ReadCodesysMemor.Open();
                PhoneOperating.Open();
                Host host = ReadCodesysMemor.GetHost();

                host.HandAuto = Optional.Default.HandAuto;
                host.LeftEntrance = Optional.Default.LeftEntrance;
                host.LeftExport = Optional.Default.LeftExport;
                host.Middle = Optional.Default.Middle;
                host.RightEntrance = Optional.Default.RightEntrance;
                host.RightExport = Optional.Default.RightExport;
                host.PCOpen = true;
                CodesysMemor.Create(host, Default.Company);

                if (Default.DrillWarehouse == null)
                    DrillWarehouseInitialization(host);

                PanelButton panelButton = ReadCodesysMemor.GetPanel();//主要只是讀取記憶體 key  的狀態
                PanelButton initialization = new PanelButton() //初始化
                {
                    Key = panelButton.Key,
                    Alarm = panelButton.Alarm,
                    HandSpeed = Default.HandSpeed
                };
                WriteCodesysMemor.SetDrillWarehouse(SetPosition(Default.DrillWarehouse));
                WriteCodesysMemor.SetPanel(initialization);

                //監聽手機連線
                PhoneOperating.SetSleepListening(500);
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
        /// <param name="host"></param>
        /// <remarks>
        /// 通常只在軟體第一次執行的時候才會
        /// </remarks>
        private static void DrillWarehouseInitialization(Host host)
        {
            Default.DrillWarehouse = SetPosition(DrillWarehouse.Initialization(host));
            //判定是否無刀庫版本
            if (host.LeftExport != true)
                Default.DrillWarehouse.LeftExport[0].IsCurrent = true;
            if (host.RightExport != true)
                Default.DrillWarehouse.RightExport[0].IsCurrent = true;
            if (host.Middle != true)
                Default.DrillWarehouse.Middle[0].IsCurrent = true;
            Default.Save();
        }
    }
}
