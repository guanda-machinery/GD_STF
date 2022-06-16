using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPFSTD105.PhoneMemor;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
namespace WPFSTD105
{
    /// <summary>
    /// <see cref="PhoneConnectDuplexClient"/> 回調處理程序
    /// </summary>
    public class CallbackHandler : IPhoneConnectDuplexCallback
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public CallbackHandler()
        {
        }
        /// <summary>
        /// Phone 操作功能請求
        /// </summary> 
        /// <param name="operating">請求連線狀態</param>
        public void RequestConnection(GD_STD.Phone.Operating operating)
        {
            PanelButton panelButton = ReadCodesysMemor.GetPanel();
            MessageBoxResult result;
            //判斷是不是手動操作，如是的話鎖住介面的手動操作功能
            if (ApplicationViewModel.CurrentPage < ApplicationPage.Home)
            {
                return;
            }
            //如果在急停期間手動操作請求
            if (panelButton.EMS == true && operating.Satus == PHONE_SATUS.WAIT_MANUAL)
            {
                WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機連線失敗", "請旋開警急停止，再重新連線手動操作。", true, false, false));
                operating.Satus = PHONE_SATUS.REFUSE;
            }
            //如果是手動操作請求
            else if (operating.Satus == PHONE_SATUS.WAIT_MANUAL)
            {
                result = WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機發送手動操作請求", "按下同意後，\n系統將會把手動操作狀態初始化，\n並將手動控權交給手機。"));
                //如果同意手動操作
                if (result == MessageBoxResult.Yes)
                {
                    //再次檢查是不是有按急停
                    panelButton = ReadCodesysMemor.GetPanel();
                    if (panelButton.EMS == true)
                    {
                        WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機連線失敗", "請旋開警急停止，再重新連線手動操作。"));
                        operating.Satus = PHONE_SATUS.REFUSE;
                    }
                    else
                    {
                        WriteCodesysMemor.SetPanel(new PanelButton { Key = ApplicationViewModel.PanelButton.Key, Alarm = panelButton.Alarm });
                        operating.Satus = PHONE_SATUS.MANUAL;
                    }
                }
                else
                {
                    operating.Satus = PHONE_SATUS.REFUSE;
                }
                //ApplicationViewModel.Message = new ViewModel.MessageVM();
            }
            //如果同意批配料單
            if (operating.Satus == PHONE_SATUS.WAIT_MATCH)
            {
                result = WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機發送配對料單請求", ""));
                if (result == MessageBoxResult.Yes)
                {
                    operating.Satus = PHONE_SATUS.MATCH;
                }
                else
                {
                    operating.Satus = PHONE_SATUS.REFUSE;
                }
            }
            if (operating.Satus == PHONE_SATUS.MANUAL)
            {
                ApplicationViewModel.AppManualConnect = true;
            }
            else
            {
                ApplicationViewModel.AppManualConnect = false;
            }
            //回復情求連線
            PhoneOperating.ReplyConnect(operating.Satus);
            //IIS 繼續監聽
            PhoneOperating.RunListening(true);
        }
    }
}
