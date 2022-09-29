using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
namespace WPFSTD105
{
    /// <summary>
    /// 聆聽使用者操控實體面板的訊號與網際網路狀態
    /// </summary>
    public class PanelListening : AbsListening
    {

        /// <inheritdoc/>
        protected override void ReadCodeSysMemory()
        {
            try
            {
                PanelButton panelButton;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    panelButton = read.GetPanel();
                    PanelSelect(ref panelButton);

                    //檢測用戶在實體面板的操控，並且跳通知提示或換頁
                    ApplicationViewModel.PanelButton = panelButton; //更新VM面板
                    if (!panelButton.EMS && ApplicationViewModel.FirstOrigin)
                    {
                        AxisMode(ref panelButton);
                        Other(ref panelButton);
                        ChangePage(panelButton);
                    }
                    else
                    {
                        ApplicationViewModel.FirstOrigin = false;
                        if (!ApplicationViewModel.FirstOrigin)
                        {
                            bool origin = read.GetHost().Origin;
                            ApplicationViewModel.FirstOrigin = origin;
                            return;
                        }
                    }
                }

                //如果內容有變更過
                if (_Change)
                {
                    WriteCodesysMemor.SetPanel(panelButton);//寫入操控面板 IO
                    _Change = false;
                }
            }
            catch (TimeoutException ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, "嘗試重新連線");
                //ReadDuplexMemory.ChannelFactory.CreateChannel();
                if (TimeoutNumber < 10)
                {
                    TimeoutNumber++;
                    return;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                throw;
            }
            //PanelButton panel = new PanelButton();
            //try
            //{
            //    //ReadMemoryCallbackHandler.PanelResetEvent.Reset();
            //    panel = ReadCodesysMemor.GetPanel();//讀取操控面板 IO
            //                                        //ReadDuplexMemory.GetPanel();//讀取操控面板 IO
            //                                        //ReadMemoryCallbackHandler.PanelResetEvent.WaitOne(Timeout.Infinite);//等待完成訊號
            //    PanelSelect(ref panel);
            //    //檢測用戶在實體面板的操控，並且跳通知提示或換頁
            //    AxisMode(ref panel);
            //    Other(ref panel);
            //    ChangePage(panel);
            //    ApplicationViewModel.PanelButton = panel; //更新VM面板
            //    //如果內容有變更過
            //    if (_Change)
            //    {
            //        WriteCodesysMemor.SetPanel(panel);//寫入操控面板 IO
            //        _Change = false;
            //    }
            //}
            //catch (TimeoutException ex)
            //{
            //    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, "嘗試重新連線");
            //    ReadCodesysMemor.ChannelFactory.CreateChannel();
            //    if (TimeoutNumber < 10)
            //    {
            //        TimeoutNumber++;
            //        return;
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            //catch (FaultException ex)
            //{
            //    log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
            //    throw;
            //}
        }
        #region 私有方法
        /// <summary>
        /// 變更頁面
        /// </summary>
        /// <param name="panelButton"></param>
        private void ChangePage(PanelButton panelButton)
        {
            ApplicationPage currentPage = ApplicationViewModel.CurrentPage;
            if (currentPage != ApplicationPage.ObSetting && currentPage != ApplicationPage.Monitor) //如果頁面不再製品設定或是加工監控
            {
                //如果按下，下壓夾具
                if (panelButton.ClampDown)
                {
                    //如果下壓夾具選擇是入料口處，但是頁面不在出口下壓夾具畫面，就換到對應畫面
                    if (panelButton.ClampDownSelected == CLAMP_DOWN.Entrance && currentPage != ApplicationPage.EnClampDown)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.EnClampDown;
                    }
                    //如果下壓夾具選擇是出料口處，但是頁面不在出口下壓夾具畫面，就換到對應畫面
                    else if (panelButton.ClampDownSelected == CLAMP_DOWN.Export && currentPage != ApplicationPage.ExClampDown)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.ExClampDown;
                    }
                }
                //如果按下側壓夾具，但是頁面不在側壓夾具畫面，就換到對應畫面
                else if (panelButton.SideClamp && currentPage != ApplicationPage.SideClamp)
                {
                    if (IsReplacePage(currentPage))
                        ApplicationViewModel.CurrentPage = ApplicationPage.SideClamp;
                }
                //如果按下送料手臂，但是頁面不在送料手臂畫面，就換到對應畫面
                else if (panelButton.Hand && currentPage != ApplicationPage.Hand)
                {
                    if (IsReplacePage(currentPage))
                        ApplicationViewModel.CurrentPage = ApplicationPage.Hand;
                }
                //如果按下，主軸模式
                else if (panelButton.MainAxisMode)
                {
                    //如果下壓夾具選擇是左軸，但是頁面不在左軸畫面，就換到對應畫面
                    if (panelButton.AxisSelect == AXIS_SELECTED.Left && currentPage != ApplicationPage.LeftAxis)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.LeftAxis;
                    }
                    //如果下壓夾具選擇是中軸，但是頁面不在中軸畫面，就換到對應畫面
                    else if (panelButton.AxisSelect == AXIS_SELECTED.Middle && currentPage != ApplicationPage.MiddleAxis)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.MiddleAxis;
                    }
                    //如果下壓夾具選擇是右軸，但是頁面不在右軸畫面，就換到對應畫面
                    else if (panelButton.AxisSelect == AXIS_SELECTED.Right && currentPage != ApplicationPage.RightAxis)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.RightAxis;
                    }
                }
                //如果選擇出入口料架，但是頁面不在料架頁面，就換到對應畫面
                else if ((panelButton.EntranceRack || panelButton.ExportRack) && currentPage != ApplicationPage.RackOperation)
                {
                    if (IsReplacePage(currentPage))
                        ApplicationViewModel.CurrentPage = ApplicationPage.RackOperation;
                }
                //如果按下，刀庫
                else if (panelButton.DrillWarehouse)
                {
                    //如果刀庫選擇是左軸出料口，但是頁面不在左軸出料口畫面，就換到對應畫面
                    if (panelButton.DrillSelected == DRILL_POSITION.ENTRANCE_L && currentPage != ApplicationPage.DrillEntrance_L)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.DrillEntrance_L;
                    }
                    //如果刀庫選擇是右軸出料口，但是頁面不在右軸出料口畫面，就換到對應畫面
                    else if (panelButton.DrillSelected == DRILL_POSITION.ENTRANCE_R && currentPage != ApplicationPage.DrillEntrance_R)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.DrillEntrance_R;
                    }
                    //如果刀庫選擇是左軸入料口，但是頁面不在左軸入料口畫面，就換到對應畫面
                    else if (panelButton.DrillSelected == DRILL_POSITION.EXPORT_L && currentPage != ApplicationPage.DrillExport_L)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.DrillExport_L;
                    }
                    //如果刀庫選擇是右軸入料口，但是頁面不在右軸入料口畫面，就換到對應畫面
                    else if (panelButton.DrillSelected == DRILL_POSITION.EXPORT_R && currentPage != ApplicationPage.DrillExport_R)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.DrillExport_R;
                    }
                    //如果刀庫選擇是中軸，但是頁面不在中軸畫面，就換到對應畫面
                    else if (panelButton.DrillSelected == DRILL_POSITION.MIDDLE && currentPage != ApplicationPage.DrillMiddle)
                    {
                        if (IsReplacePage(currentPage))
                            ApplicationViewModel.CurrentPage = ApplicationPage.DrillMiddle;
                    }
                }
                else if (panelButton.Volume && currentPage != ApplicationPage.Volume)
                {
                    if (IsReplacePage(currentPage))
                        ApplicationViewModel.CurrentPage = ApplicationPage.Volume;
                }
                //如果沒有選擇主軸模式，但是目前頁面是在主軸模式相關的畫面，就返回首頁
                else if (!panelButton.MainAxisMode &&
                     (currentPage == ApplicationPage.LeftAxis ||
                      currentPage == ApplicationPage.RightAxis ||
                      currentPage == ApplicationPage.MiddleAxis))
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果尚未發生警報，但是目前頁面是在警報畫面，就返回首頁
                else if (panelButton.Alarm == ERROR_CODE.Null && currentPage == ApplicationPage.Alarm)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇下壓夾具，但是目前頁面是在下壓夾具相關的畫面，就返回首頁
                else if (!panelButton.ClampDown &&
                           (currentPage == ApplicationPage.EnClampDown ||
                            currentPage == ApplicationPage.ExClampDown))
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇刀庫，但是目前頁面是在刀庫相關畫面，就返回首頁
                else if (!panelButton.DrillWarehouse &&
                            (currentPage == ApplicationPage.DrillEntrance_L ||
                             currentPage == ApplicationPage.DrillEntrance_R ||
                             currentPage == ApplicationPage.DrillExport_L ||
                             currentPage == ApplicationPage.DrillExport_R ||
                             currentPage == ApplicationPage.DrillMiddle))
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇周邊料架，但是目前頁面是在周邊料架畫面，就返回首頁
                else if (!panelButton.ExportRack && !panelButton.EntranceRack && currentPage == ApplicationPage.RackOperation)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇送料手臂，但是目前頁面是在送料手臂畫面，就返回首頁
                else if (!panelButton.Hand && currentPage == ApplicationPage.Hand)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇側壓夾具，但是目前頁面是在側壓夾具畫面，就返回首頁
                else if (!panelButton.SideClamp && currentPage == ApplicationPage.SideClamp)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                //如果沒有選擇捲削機，但是目前頁面是在捲削機畫面，就返回首頁
                else if (!panelButton.Volume && currentPage == ApplicationPage.Volume)
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                else if (panelButton.Alarm == ERROR_CODE.Unknown && ApplicationPage.Alarm != currentPage)
                {
                    string errorCode = ReadCodesysMemor.GetUnknownCode().Replace("\0", "");
                    ApplicationViewModel.ErrorInfo = errorCode;
                }
            }

        }
        /// <summary>
        /// 如果是在設定頁面
        /// </summary>
        /// <returns>確定換頁回傳true，取消則回傳false</returns>
        public bool IsReplacePage(ApplicationPage current)
        {
            if (current == ApplicationPage.SettingPar || 
                current == ApplicationPage.SofSettings || 
                current == ApplicationPage.ObSetting || 
                current == ApplicationPage.TypeSetting||
                current == ApplicationPage.MachineTypeSetting||
                current == ApplicationPage.MachineProductSetting
                )
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"是否確定離開此頁面，離開後將不儲存相關設定。", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 面板頁面選擇
        /// <para>聆聽 Codesys 的<see cref="KEY_HOLE"/>的狀態</para>
        /// </summary>
        private void PanelSelect(ref PanelButton panelButton)
        {
            //返回上一頁
            if (ApplicationViewModel.CurrentPage == ApplicationPage.Lock && panelButton.Key != GD_STD.Enum.KEY_HOLE.LOCK)
            {
                ApplicationViewModel.CurrentPage = ApplicationViewModel.PreviousPage;
            }
            //畫面上鎖
            if (panelButton.Key == KEY_HOLE.LOCK && ApplicationViewModel.CurrentPage != ApplicationPage.Lock)
            {
                panelButton = new PanelButton(); //初始值
                WriteCodesysMemor.SetPanel(panelButton);//寫入初始值
                ApplicationViewModel.CurrentPage = ApplicationPage.Lock;
            }
        }
        /// <summary>
        /// 接收到 Codesys 主軸模式實體按鈕
        /// </summary>
        private void AxisMode(ref PanelButton panelButton)
        {
            if (TriggerAxisAction(panelButton))
            {
                if (PanelListening.SLPEMS() || !PanelListening.IsManualMode() || !PanelListening.PromptMainAxisMode(panelButton.MainAxisMode)) //如果 按下急停 || 不是手動模式 || 沒有按下主軸模式
                {
                    PanelListening.ClearAxisMode(ref panelButton);
                    _Change = true;
                }
            }
        }
        /// <summary>
        /// 觸發軸向動作
        /// </summary>
        /// <returns>如果有觸發某一個軸向動作則回傳 true ，沒有觸發則回傳 false</returns>
        private bool TriggerAxisAction(PanelButton panelButton)
        {
            return panelButton.AxisLooseKnife || panelButton.AxisRotation || panelButton.AxisEffluent ? true : false;
        }

        /// <summary>
        /// 接收到 Codesys 其他實體按鈕
        /// <para>
        /// 入口料架， 出口料架， 送料手臂，夾具下壓，夾具側壓，捲削機，開始
        /// </para>
        /// </summary>
        /// <param name="panelButton"></param>
        private void Other(ref PanelButton panelButton)
        {
            if (/*panelButton.RackOperation ||*/ panelButton.Hand || panelButton.ClampDown || panelButton.SideClamp || panelButton.Volume)
            {
                //如果按下急停 || 沒有手動模式
                if (PanelListening.SLPEMS() || !PanelListening.IsManualMode())
                {
                    //panelButton.EntranceRack = false;
                    //panelButton.ExportRack = false;
                    //panelButton.Hand = false;
                    //panelButton.ClampDown = false;
                    //panelButton.SideClamp = false;
                    //panelButton.Volume = false;
                    ClearManual(ref panelButton);
                    _Change = true;
                }
            }
        }
        /// <summary>
        /// 清除所有手動狀態
        /// </summary>
        /// <param name="panelButton"></param>
        private void ClearManual(ref PanelButton panelButton)
        {
            panelButton.EntranceRack = false;
            panelButton.ExportRack = false;
            //panelButton.RackOperation = false;
            panelButton.Hand = false;
            panelButton.ClampDown = false;
            panelButton.SideClamp = false;
            panelButton.Volume = false;
            panelButton.AxisStop = false;
            panelButton.AxisRotation = false;
            panelButton.AxisLooseKnife = false;
            panelButton.AxisEffluent = false;
            panelButton.MainAxisMode = false;
            _Change = true;
        }
        #endregion

        #region 私有欄位
        /// <summary>
        /// 變更過監聽內容
        /// </summary>
#pragma warning disable CS0414 // 已指派欄位 'PanelListening._Change'，但從未使用過其值
        private static bool _Change = false;
#pragma warning restore CS0414 // 已指派欄位 'PanelListening._Change'，但從未使用過其值
        #endregion

        #region 公開的靜態方法
        /// <summary>
        /// 按下實體面板緊急停止提示
        /// </summary>
        /// <returns>如果有觸發急停止回傳 treu，沒有則回傳 false</returns>
        public static bool SLPEMS()
        {
            if (ApplicationViewModel.PanelButton.EMS)
            {
                //MessageBox.Show("請旋開警急停止", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"請旋開警急停止",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
            }
            return ApplicationViewModel.PanelButton.EMS;
        }
        /// <summary>
        /// 清除主軸模式相關狀態
        /// </summary>
        /// <param name="panelButton"></param>
        public static void ClearAxisMode(ref PanelButton panelButton)
        {
            panelButton.AxisStop = false;
            panelButton.AxisRotation = false;
            panelButton.AxisLooseKnife = false;
            panelButton.AxisEffluent = false;
            panelButton.MainAxisMode = false;
        }
        /// <summary>
        /// 警報發生時提示
        /// </summary>
        /// <returns></returns>
        public static bool SLPAlarm()
        {
            if (ApplicationViewModel.PanelButton.Alarm > ERROR_CODE.Null)
            {
                //MessageBox.Show("請先按下警報鈕，查看警報位置，在排除警報。\n排除完畢，請按下實體按鈕\"警報復位\"", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"請先按下警報鈕，查看警報位置，在排除警報。\n排除完畢，請按下實體按鈕\"警報復位\"",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 主軸模式提示
        /// </summary>
        /// <param name="axisMode"><see cref="PanelButton.MainAxisMode"/></param>
        /// <returns>如果有觸發主軸模式回傳 true</returns>
        public static bool PromptMainAxisMode(bool axisMode)
        {
            if (!axisMode)//如果主軸狀態 = false
            {
                //MessageBox.Show("請先觸發主軸模式", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"請先觸發主軸模式",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
            }
            return axisMode;
        }

#pragma warning disable CS1574 // XML 註解有無法解析的 cref 屬性 'Key'
        /// <summary>
        /// 手動模式提示
        /// </summary>
        /// <returns>如果 <see cref="Key"/>不是<see cref="KEY_HOLE.MANUAL"/>回傳 false，是的話回傳 true</returns>
        public static bool IsManualMode()
#pragma warning restore CS1574 // XML 註解有無法解析的 cref 屬性 'Key'
        {
            if (ApplicationViewModel.PanelButton.Key != KEY_HOLE.MANUAL)
            {
                //MessageBox.Show("請將鑰匙旋轉到手動模式", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"請將鑰匙旋轉到手動模式",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                return false;
            }
            return true;
        }
        #endregion
    }
}
