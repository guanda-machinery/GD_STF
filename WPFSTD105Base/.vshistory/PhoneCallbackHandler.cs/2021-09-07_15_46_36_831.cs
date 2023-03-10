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
using GD_STD.Phone;
using System.IO;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;

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
            if (ApplicationViewModel.CurrentPage < ApplicationPage.Lock)
            {
                WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機連線失敗", "請將 key 旋轉到手動或自動，再重新連線手動操作。", true, false, false));
                operating.Satus = PHONE_SATUS.REFUSE;
                return;
            }
            //手動操作請求
            if (operating.Satus == PHONE_SATUS.WAIT_MANUAL)
            {
                //如果有觸發警急停止
                if (panelButton.EMS == true)
                {
                    WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機連線失敗", "請旋開警急停止，再重新連線手動操作。", true, false, false));
                    operating.Satus = PHONE_SATUS.REFUSE;
                }
                //如果沒觸發警急停止
                else
                {
                    result = WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機發送手動操作請求", "按下同意後，\n系統將會把手動操作狀態初始化，\n並將手動控權交給手機。"));
                    //如果同意手動操作
                    if (result == MessageBoxResult.Yes)
                    {
                        //再次檢查是不是有觸發警急停止
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
            }

            //如果同意批配料單
            if (operating.Satus == PHONE_SATUS.WAIT_MATCH)
            {
                WPFBase.MessageVM vm = new WPFBase.MessageVM
                {
                    //TODO:等帶嵐這邊修改完畢
                    ComBoxContent = ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath).ToArray(),
                    AutoClose = false,
                    CloseSecond = 0,
                    ComBoxTitle = "專案名稱",
                    ComBoxVisibility = true,
                    Content = "請選擇要配對的料單的專案"
                };
                //開啟舊檔命令
                ICommand openOldFile = new WPFWindowsBase.RelayCommand(() =>
                {
                    FolderBrowserDialogService service = new FolderBrowserDialogService();
                    service.Description = "請選擇專案放置的資料夾"; //標題名稱
                    service.ShowNewFolderButton = false;//該值指示“新建文件夾”按鈕是否顯示在文件夾瀏覽器對話框
                    service.RootFolder = Environment.SpecialFolder.Desktop;
                    service.RestorePreviouslySelectedDirectory = true;
                    service.StartPath = Properties.SofSetting.Default.LoadPath; //起始路徑
                    IFolderBrowserDialogService folder = service; 
                    folder.ShowDialog();//Show 視窗
                    Properties.SofSetting.Default.LoadPath = folder.ResultPath;
                    Properties.SofSetting.Default.Save();
                    vm.ComBoxContent = ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath).ToArray();
                });
                vm.OpenOldFile = openOldFile;
                result = WPFBase.CustomMessage.Show(vm);
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
        List<short> list = new List<short>();
        /// <summary>
        /// 序列化 <see cref="MonitorWork.Index"/> 指向的位置
        /// </summary>
        /// <param name="index"></param>
        public void SerializationIndex(short[] index)
        {
            PhoneOperating.RunListening(false);
            if (list.Count <= 0)
            {
                list.AddRange(index);
            }
            else
            {
                var listExcpet = list.Except(index);
                var indexExcpet = index.Except(list);
            }
            PhoneOperating.RunListening(true);
        }
    }
}
