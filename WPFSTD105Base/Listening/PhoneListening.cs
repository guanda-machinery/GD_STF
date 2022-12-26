using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
namespace WPFSTD105.Listening
{
    /// <summary>
    /// 手機聆聽端
    /// </summary>
    public class PhoneListening : AbsListening
    {
        public PhoneListening() : base()
        {
            _Thread.SetApartmentState(System.Threading.ApartmentState.STA);
        }
        int count = 0;
        /// <inheritdoc/>
        protected override void ReadCodeSysMemory()
        {
            try
            {
                GD_STD.Phone.Operating operating;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    operating = read.GetOperating();
                }
                if (operating.OpenApp)
                {
                    MessageBoxResult result;
                    //手動操作請求
                    if (operating.Satus == PHONE_SATUS.WAIT_MANUAL)
                    {
                        //如果有觸發警急停止
                        if (ApplicationViewModel.PanelButton.EMS == true)
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
                                //panelButton = ReadCodesysMemor.GetPanel();
                                if (ApplicationViewModel.PanelButton.EMS == true)
                                {
                                    WPFBase.CustomMessage.Show(new WPFBase.MessageVM("手機連線失敗", "請旋開警急停止，再重新連線手動操作。"));
                                    operating.Satus = PHONE_SATUS.REFUSE;
                                }
                                else
                                {
                                    operating.Satus = PHONE_SATUS.MANUAL;
                                }
                            }
                            else
                            {
                                operating.Satus = PHONE_SATUS.REFUSE;
                            }
                        }
                    }
                    //如果同意批配料單
                    if (operating.Satus == PHONE_SATUS.WAIT_MATCH)
                    {
                        FolderBrowserDialogService service = DevExpand.NewFolder("請選擇專案資料夾");
                        service.StartPath = Properties.SofSetting.Default.LoadPath; //起始路徑
                        WPFBase.MessageVM vm = new WPFBase.MessageVM
                        {
                            Path = Properties.SofSetting.Default.LoadPath,
                            ComBoxContent = ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath).ToArray(),
                            AutoClose = false,
                            CloseSecond = 0,
                            ComBoxTitle = "專案名稱",
                            ComBoxVisibility = Visibility.Visible,
                            Content = "請選擇要配對的料單的專案"

                        };
                        //開啟舊檔命令
                        ICommand openOldFile = new WPFWindowsBase.RelayCommand(() =>
                        {
                            IFolderBrowserDialogService folder = service;
                            folder.ShowDialog();//Show 視窗
                            Properties.SofSetting.Default.LoadPath = folder.ResultPath;//選擇的路徑
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
                    using(Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    {
                        write.SetPhoneOperating(operating);
                    }
                }
            }
            catch (Exception ex)
            {
                if (TimeoutNumber < 50)
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, $"嘗試重新連線 {count} 次...");
                    TimeoutNumber++;
                    ReadCodeSysMemory();
                }
                else
                {
                    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, "嘗試重新連線失敗");
                    throw new Exception(ex.Message) ;
                }
            }

        }
    }
}
