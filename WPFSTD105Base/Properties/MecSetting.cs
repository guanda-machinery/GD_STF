namespace WPFSTD105.Properties {
    
    
    // 這個類別可以讓您處理設定類別上的特定事件:
    //  在設定值變更之前引發 SettingChanging 事件。
    //  在設定值變更之後引發 PropertyChanged 事件。
    //  在載入設定值之後引發 SettingsLoaded 事件。
    //  在儲存設定值之前引發 SettingsSaving 事件。
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MecSetting' 的 XML 註解
    public sealed partial class MecSetting {
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MecSetting' 的 XML 註解
        
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MecSetting.MecSetting()' 的 XML 註解
        public MecSetting() {
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MecSetting.MecSetting()' 的 XML 註解
            // // 若要加入用於儲存與變更設定的事件處理常式，請取消註解下列程式行:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // 在此加入程式碼以處理 SettingChangingEvent 事件。
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // 在此加入程式碼以處理 SettingsSaving 事件。
        }
    }
}
