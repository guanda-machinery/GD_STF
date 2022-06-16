using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GD_STF
{
    /// <summary>
    /// 代表面板功能結構
    /// </summary>
    /// <remarks>
    /// 對應人機操控面板的按鈕
    /// Codesys Memory 讀取/寫入
    /// </remarks>
    public struct PanelButtom
    {
        #region 共享記憶
        /// <summary>
        /// 代表操作面板鑰匙孔的功能
        /// </summary>
        public KEY_HOLE Key { get; set; }
        /// <summary>
        /// 代表操作面板緊急停止
        /// </summary>
        public bool EMS { get; set; }
        /// <summary>
        /// 主軸模式
        /// </summary>
        public bool MainAxisMode
        {
            get => _MainAxisMode;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _MainAxisMode = value;
            }
        }
        /// <summary>
        /// 主軸旋轉
        /// </summary>
        public bool MainAxisRotation
        {
            get => _MainAxisRotation;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        if (PromptMainAxisMode())//如果主軸模式 = true
                            _MainAxisRotation = value;
            }
        }
        /// <summary>
        /// 主軸靜止
        /// </summary>
        public bool MainAxisStop
        {
            get => _MainAxisStop;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        if (PromptMainAxisMode())//如果主軸模式 = true
                            _MainAxisStop = value;
            }
        }
        /// <summary>
        /// 中心出水
        /// </summary>
        public bool Effluent
        {
            get => _Effluent;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        if (PromptMainAxisMode())//如果主軸模式 = true
                            _Effluent = Effluent;
            }
        }
        /// <summary>
        /// 鬆刀
        /// </summary>
        public bool LooseKnife
        {
            get => _LooseKnife;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        if (PromptMainAxisMode())//如果主軸模式 = true
                            _LooseKnife = value;
            }
        }
        /// <summary>
        /// 入口料架
        /// </summary>
        public bool EntranceRack
        {
            get => _EntranceRack;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _EntranceRack = value;
            }
        }
        /// <summary>
        /// 出口料架
        /// </summary>
        public bool ExportRack
        {
            get => _ExportRack;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _ExportRack = value;
            }
        }
        /// <summary>
        /// 捲削機
        /// </summary>
        public bool Volume
        {
            get => _Volume;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _Volume = value;
            }
        }
        /// <summary>
        /// 側壓夾具
        /// </summary>
        public bool SideClamp
        {
            get => _SideClamp;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _SideClamp = value;
            }
        }
        /// <summary>
        /// 側壓夾具
        /// </summary>
        public bool ClampDown
        {
            get => _ClampDown;
            set
            {
                if (!PromptEMS())
                    if (IsManualMode())
                        _ClampDown = value ;
            }
        }
        /// <summary>
        /// 開始執行
        /// </summary>
        public bool Run { get; set; }
        /// <summary>
        /// 暫停執行
        /// </summary>
        public bool Stop { get; set; }
        #endregion

        #region 私有屬性
        private bool _MainAxisRotation { get; set; }
        private bool _MainAxisMode { get; set; }
        private bool _MainAxisStop { get; set; }
        private bool _Effluent { get; set; }
        private bool _LooseKnife { get; set; }
        private bool _EntranceRack { get; set; }
        private bool _ExportRack { get; set; }
        private bool _Volume { get; set; }
        private bool _SideClamp { get; set; }
        private bool _ClampDown { get; set; }
        #endregion
        #region 私有方法
        /// <summary>
        /// 緊急停止提示
        /// </summary>
        private bool PromptEMS()
        {
            if (EMS)
            {
                MessageBox.Show("以按下緊急停止實體按鈕，請先一鍵復歸，在操作子此功能", "通知", MessageBoxButton.OK);
            }
            return EMS;
        }
        /// <summary>
        /// 主軸模式提示
        /// </summary>
        /// <returns></returns>
        private bool PromptMainAxisMode()
        {
            if (!MainAxisMode)//如果主軸狀態 = true
            {
                MessageBox.Show("請先觸發主軸模式", "通知", MessageBoxButton.OK);
            }
            return MainAxisMode;
        }
        /// <summary>
        /// 如果 <see cref="Key"/>不是<see cref="KEY_HOLE.MANUAL"/>回傳 false，是的話回傳 true
        /// </summary>
        /// <returns></returns>
        private bool IsManualMode()
        {
            if (Key != KEY_HOLE.MANUAL)
            {
                MessageBox.Show("請將鑰匙旋轉到手動模式", "通知", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
        #endregion
    }
}