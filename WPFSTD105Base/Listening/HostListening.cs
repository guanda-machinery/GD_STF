//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static WPFSTD105.ViewLocator;
//using static WPFSTD105.CodesysIIS;
//using GD_STD.Enum;
//using System.Windows;
//using GD_STD;

//namespace WPFSTD105.Listening
//{
//    /// <summary>
//    /// 聆聽 Codesys 狀態 
//    /// </summary>
//    public class HostListening : AbsListening
//    {
//        /// <inheritdoc/>
//        protected override void ReadCodeSysMemory()
//        {
//            try
//            {
//                ApplicationViewModel.Host = ReadCodesysMemor.GetHost(); //讀取 Codesys 狀態
//            }
//            catch (TimeoutException ex)
//            {
//                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, "嘗試重新連線");
//                ReadCodesysMemor.Abort();
//                ReadCodesysMemor = new Memor.ReadMemorClient();
//                if (TimeoutNumber < 10)
//                {
//                    TimeoutNumber++;
//                    return;
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            //if (ApplicationViewModel.PanelButton.Key != KEY_HOLE.MANUAL && result.PhoneOpen == PHONE_SATUS.WAIT_MANUAL)
//            //{
//            //    result.PhoneOpen = PHONE_SATUS.MONITOR;
//            //    WriteCodesysMemor.SetHost(result);

//            //    MessageBoxResult messageBoxResult = MessageBox.Show("手機連線須鑰匙轉到手動狀態，再重新操作。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);

//            //}
//            ////如果手機連線是等待狀態
//            //if (result.PhoneOpen == PHONE_SATUS.WAIT_MANUAL)
//            //{
//            //    //用戶確認是否讓手機連線
//            //    MessageBoxResult messageBoxResult = MessageBox.Show("是否開啟手機連線", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);

//            //    result = ReadCodesysMemor.GetHost(); //重新讀取 Codesys 狀態
//            //    if (messageBoxResult == MessageBoxResult.Yes)
//            //    {

//            //        //連線允許
//            //        result.PhoneOpen = PHONE_SATUS.MANUAL;
//            //    }
//            //    else
//            //    {
//            //        //連線失敗
//            //        result.PhoneOpen = PHONE_SATUS.REFUSE;
//            //    }
//            //    WriteCodesysMemor.SetHost(result);//寫入狀態
//            //}
//        }
//    }
//}
