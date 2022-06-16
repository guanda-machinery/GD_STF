﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp12.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Codesys.Memory", ConfigurationName="ServiceReference1.IMemor")]
    public interface IMemor {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/Create", ReplyAction="http://Codesys.Memory/IMemor/CreateResponse")]
        GD_STD.Host Create(GD_STD.Host host, string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/Create", ReplyAction="http://Codesys.Memory/IMemor/CreateResponse")]
        System.Threading.Tasks.Task<GD_STD.Host> CreateAsync(GD_STD.Host host, string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/BAVT", ReplyAction="http://Codesys.Memory/IMemor/BAVTResponse")]
        GD_STD.Enum.BAVT BAVT();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/BAVT", ReplyAction="http://Codesys.Memory/IMemor/BAVTResponse")]
        System.Threading.Tasks.Task<GD_STD.Enum.BAVT> BAVTAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/GetMonitorWorkOffset", ReplyAction="http://Codesys.Memory/IMemor/GetMonitorWorkOffsetResponse")]
        int GetMonitorWorkOffset(string FieldName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IMemor/GetMonitorWorkOffset", ReplyAction="http://Codesys.Memory/IMemor/GetMonitorWorkOffsetResponse")]
        System.Threading.Tasks.Task<int> GetMonitorWorkOffsetAsync(string FieldName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMemorChannel : ConsoleApp12.ServiceReference1.IMemor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MemorClient : System.ServiceModel.ClientBase<ConsoleApp12.ServiceReference1.IMemor>, ConsoleApp12.ServiceReference1.IMemor {
        
        public MemorClient() {
        }
        
        public MemorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MemorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public GD_STD.Host Create(GD_STD.Host host, string company) {
            return base.Channel.Create(host, company);
        }
        
        public System.Threading.Tasks.Task<GD_STD.Host> CreateAsync(GD_STD.Host host, string company) {
            return base.Channel.CreateAsync(host, company);
        }
        
        public GD_STD.Enum.BAVT BAVT() {
            return base.Channel.BAVT();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Enum.BAVT> BAVTAsync() {
            return base.Channel.BAVTAsync();
        }
        
        public int GetMonitorWorkOffset(string FieldName) {
            return base.Channel.GetMonitorWorkOffset(FieldName);
        }
        
        public System.Threading.Tasks.Task<int> GetMonitorWorkOffsetAsync(string FieldName) {
            return base.Channel.GetMonitorWorkOffsetAsync(FieldName);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Codesys.Memory", ConfigurationName="ServiceReference1.IReadMemor")]
    public interface IReadMemor {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetPanel", ReplyAction="http://Codesys.Memory/IReadMemor/GetPanelResponse")]
        GD_STD.PanelButton GetPanel();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetPanel", ReplyAction="http://Codesys.Memory/IReadMemor/GetPanelResponse")]
        System.Threading.Tasks.Task<GD_STD.PanelButton> GetPanelAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetDrillWarehouse", ReplyAction="http://Codesys.Memory/IReadMemor/GetDrillWarehouseResponse")]
        GD_STD.DrillWarehouse GetDrillWarehouse();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetDrillWarehouse", ReplyAction="http://Codesys.Memory/IReadMemor/GetDrillWarehouseResponse")]
        System.Threading.Tasks.Task<GD_STD.DrillWarehouse> GetDrillWarehouseAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOill", ReplyAction="http://Codesys.Memory/IReadMemor/GetOillResponse")]
        GD_STD.OillSystem GetOill();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOill", ReplyAction="http://Codesys.Memory/IReadMemor/GetOillResponse")]
        System.Threading.Tasks.Task<GD_STD.OillSystem> GetOillAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetHydraulic", ReplyAction="http://Codesys.Memory/IReadMemor/GetHydraulicResponse")]
        GD_STD.HydraulicSystem[] GetHydraulic();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetHydraulic", ReplyAction="http://Codesys.Memory/IReadMemor/GetHydraulicResponse")]
        System.Threading.Tasks.Task<GD_STD.HydraulicSystem[]> GetHydraulicAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetLubricant", ReplyAction="http://Codesys.Memory/IReadMemor/GetLubricantResponse")]
        GD_STD.LubricantSystem GetLubricant();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetLubricant", ReplyAction="http://Codesys.Memory/IReadMemor/GetLubricantResponse")]
        System.Threading.Tasks.Task<GD_STD.LubricantSystem> GetLubricantAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetCut", ReplyAction="http://Codesys.Memory/IReadMemor/GetCutResponse")]
        GD_STD.CutOilSystem GetCut();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetCut", ReplyAction="http://Codesys.Memory/IReadMemor/GetCutResponse")]
        System.Threading.Tasks.Task<GD_STD.CutOilSystem> GetCutAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAxisInfo", ReplyAction="http://Codesys.Memory/IReadMemor/GetAxisInfoResponse")]
        GD_STD.AxisInfo GetAxisInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAxisInfo", ReplyAction="http://Codesys.Memory/IReadMemor/GetAxisInfoResponse")]
        System.Threading.Tasks.Task<GD_STD.AxisInfo> GetAxisInfoAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetHost", ReplyAction="http://Codesys.Memory/IReadMemor/GetHostResponse")]
        GD_STD.Host GetHost();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetHost", ReplyAction="http://Codesys.Memory/IReadMemor/GetHostResponse")]
        System.Threading.Tasks.Task<GD_STD.Host> GetHostAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOutage", ReplyAction="http://Codesys.Memory/IReadMemor/GetOutageResponse")]
        GD_STD.Outage GetOutage();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOutage", ReplyAction="http://Codesys.Memory/IReadMemor/GetOutageResponse")]
        System.Threading.Tasks.Task<GD_STD.Outage> GetOutageAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetUnknownCode", ReplyAction="http://Codesys.Memory/IReadMemor/GetUnknownCodeResponse")]
        string GetUnknownCode();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetUnknownCode", ReplyAction="http://Codesys.Memory/IReadMemor/GetUnknownCodeResponse")]
        System.Threading.Tasks.Task<string> GetUnknownCodeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAPP_Struct", ReplyAction="http://Codesys.Memory/IReadMemor/GetAPP_StructResponse")]
        GD_STD.Phone.APP_Struct GetAPP_Struct();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAPP_Struct", ReplyAction="http://Codesys.Memory/IReadMemor/GetAPP_StructResponse")]
        System.Threading.Tasks.Task<GD_STD.Phone.APP_Struct> GetAPP_StructAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAPPMonitorMec", ReplyAction="http://Codesys.Memory/IReadMemor/GetAPPMonitorMecResponse")]
        GD_STD.Phone.MonitorMec GetAPPMonitorMec();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAPPMonitorMec", ReplyAction="http://Codesys.Memory/IReadMemor/GetAPPMonitorMecResponse")]
        System.Threading.Tasks.Task<GD_STD.Phone.MonitorMec> GetAPPMonitorMecAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOperating", ReplyAction="http://Codesys.Memory/IReadMemor/GetOperatingResponse")]
        GD_STD.Phone.Operating GetOperating();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetOperating", ReplyAction="http://Codesys.Memory/IReadMemor/GetOperatingResponse")]
        System.Threading.Tasks.Task<GD_STD.Phone.Operating> GetOperatingAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetProjectName", ReplyAction="http://Codesys.Memory/IReadMemor/GetProjectNameResponse")]
        string GetProjectName();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetProjectName", ReplyAction="http://Codesys.Memory/IReadMemor/GetProjectNameResponse")]
        System.Threading.Tasks.Task<string> GetProjectNameAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetWorkMaterial", ReplyAction="http://Codesys.Memory/IReadMemor/GetWorkMaterialResponse")]
        GD_STD.Phone.WorkMaterial GetWorkMaterial(ushort index);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetWorkMaterial", ReplyAction="http://Codesys.Memory/IReadMemor/GetWorkMaterialResponse")]
        System.Threading.Tasks.Task<GD_STD.Phone.WorkMaterial> GetWorkMaterialAsync(ushort index);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAndroidTest", ReplyAction="http://Codesys.Memory/IReadMemor/GetAndroidTestResponse")]
        GD_STD.AndroidTest GetAndroidTest();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IReadMemor/GetAndroidTest", ReplyAction="http://Codesys.Memory/IReadMemor/GetAndroidTestResponse")]
        System.Threading.Tasks.Task<GD_STD.AndroidTest> GetAndroidTestAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReadMemorChannel : ConsoleApp12.ServiceReference1.IReadMemor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReadMemorClient : System.ServiceModel.ClientBase<ConsoleApp12.ServiceReference1.IReadMemor>, ConsoleApp12.ServiceReference1.IReadMemor {
        
        public ReadMemorClient() {
        }
        
        public ReadMemorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReadMemorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReadMemorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReadMemorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public GD_STD.PanelButton GetPanel() {
            return base.Channel.GetPanel();
        }
        
        public System.Threading.Tasks.Task<GD_STD.PanelButton> GetPanelAsync() {
            return base.Channel.GetPanelAsync();
        }
        
        public GD_STD.DrillWarehouse GetDrillWarehouse() {
            return base.Channel.GetDrillWarehouse();
        }
        
        public System.Threading.Tasks.Task<GD_STD.DrillWarehouse> GetDrillWarehouseAsync() {
            return base.Channel.GetDrillWarehouseAsync();
        }
        
        public GD_STD.OillSystem GetOill() {
            return base.Channel.GetOill();
        }
        
        public System.Threading.Tasks.Task<GD_STD.OillSystem> GetOillAsync() {
            return base.Channel.GetOillAsync();
        }
        
        public GD_STD.HydraulicSystem[] GetHydraulic() {
            return base.Channel.GetHydraulic();
        }
        
        public System.Threading.Tasks.Task<GD_STD.HydraulicSystem[]> GetHydraulicAsync() {
            return base.Channel.GetHydraulicAsync();
        }
        
        public GD_STD.LubricantSystem GetLubricant() {
            return base.Channel.GetLubricant();
        }
        
        public System.Threading.Tasks.Task<GD_STD.LubricantSystem> GetLubricantAsync() {
            return base.Channel.GetLubricantAsync();
        }
        
        public GD_STD.CutOilSystem GetCut() {
            return base.Channel.GetCut();
        }
        
        public System.Threading.Tasks.Task<GD_STD.CutOilSystem> GetCutAsync() {
            return base.Channel.GetCutAsync();
        }
        
        public GD_STD.AxisInfo GetAxisInfo() {
            return base.Channel.GetAxisInfo();
        }
        
        public System.Threading.Tasks.Task<GD_STD.AxisInfo> GetAxisInfoAsync() {
            return base.Channel.GetAxisInfoAsync();
        }
        
        public GD_STD.Host GetHost() {
            return base.Channel.GetHost();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Host> GetHostAsync() {
            return base.Channel.GetHostAsync();
        }
        
        public GD_STD.Outage GetOutage() {
            return base.Channel.GetOutage();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Outage> GetOutageAsync() {
            return base.Channel.GetOutageAsync();
        }
        
        public string GetUnknownCode() {
            return base.Channel.GetUnknownCode();
        }
        
        public System.Threading.Tasks.Task<string> GetUnknownCodeAsync() {
            return base.Channel.GetUnknownCodeAsync();
        }
        
        public GD_STD.Phone.APP_Struct GetAPP_Struct() {
            return base.Channel.GetAPP_Struct();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Phone.APP_Struct> GetAPP_StructAsync() {
            return base.Channel.GetAPP_StructAsync();
        }
        
        public GD_STD.Phone.MonitorMec GetAPPMonitorMec() {
            return base.Channel.GetAPPMonitorMec();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Phone.MonitorMec> GetAPPMonitorMecAsync() {
            return base.Channel.GetAPPMonitorMecAsync();
        }
        
        public GD_STD.Phone.Operating GetOperating() {
            return base.Channel.GetOperating();
        }
        
        public System.Threading.Tasks.Task<GD_STD.Phone.Operating> GetOperatingAsync() {
            return base.Channel.GetOperatingAsync();
        }
        
        public string GetProjectName() {
            return base.Channel.GetProjectName();
        }
        
        public System.Threading.Tasks.Task<string> GetProjectNameAsync() {
            return base.Channel.GetProjectNameAsync();
        }
        
        public GD_STD.Phone.WorkMaterial GetWorkMaterial(ushort index) {
            return base.Channel.GetWorkMaterial(index);
        }
        
        public System.Threading.Tasks.Task<GD_STD.Phone.WorkMaterial> GetWorkMaterialAsync(ushort index) {
            return base.Channel.GetWorkMaterialAsync(index);
        }
        
        public GD_STD.AndroidTest GetAndroidTest() {
            return base.Channel.GetAndroidTest();
        }
        
        public System.Threading.Tasks.Task<GD_STD.AndroidTest> GetAndroidTestAsync() {
            return base.Channel.GetAndroidTestAsync();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Codesys.Memory", ConfigurationName="ServiceReference1.IWriteMemor")]
    public interface IWriteMemor {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetPanel", ReplyAction="http://Codesys.Memory/IWriteMemor/SetPanelResponse")]
        void SetPanel(GD_STD.PanelButton value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetPanel", ReplyAction="http://Codesys.Memory/IWriteMemor/SetPanelResponse")]
        System.Threading.Tasks.Task SetPanelAsync(GD_STD.PanelButton value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetDrillWarehouse", ReplyAction="http://Codesys.Memory/IWriteMemor/SetDrillWarehouseResponse")]
        void SetDrillWarehouse(GD_STD.DrillWarehouse value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetDrillWarehouse", ReplyAction="http://Codesys.Memory/IWriteMemor/SetDrillWarehouseResponse")]
        System.Threading.Tasks.Task SetDrillWarehouseAsync(GD_STD.DrillWarehouse value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetOill", ReplyAction="http://Codesys.Memory/IWriteMemor/SetOillResponse")]
        void SetOill(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetOill", ReplyAction="http://Codesys.Memory/IWriteMemor/SetOillResponse")]
        System.Threading.Tasks.Task SetOillAsync(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetHydraulic", ReplyAction="http://Codesys.Memory/IWriteMemor/SetHydraulicResponse")]
        void SetHydraulic(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetHydraulic", ReplyAction="http://Codesys.Memory/IWriteMemor/SetHydraulicResponse")]
        System.Threading.Tasks.Task SetHydraulicAsync(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetLubricant", ReplyAction="http://Codesys.Memory/IWriteMemor/SetLubricantResponse")]
        void SetLubricant(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetLubricant", ReplyAction="http://Codesys.Memory/IWriteMemor/SetLubricantResponse")]
        System.Threading.Tasks.Task SetLubricantAsync(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetCut", ReplyAction="http://Codesys.Memory/IWriteMemor/SetCutResponse")]
        void SetCut(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetCut", ReplyAction="http://Codesys.Memory/IWriteMemor/SetCutResponse")]
        System.Threading.Tasks.Task SetCutAsync(GD_STD.OillSystem value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetHost", ReplyAction="http://Codesys.Memory/IWriteMemor/SetHostResponse")]
        void SetHost(GD_STD.Host value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetHost", ReplyAction="http://Codesys.Memory/IWriteMemor/SetHostResponse")]
        System.Threading.Tasks.Task SetHostAsync(GD_STD.Host value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetOutage", ReplyAction="http://Codesys.Memory/IWriteMemor/SetOutageResponse")]
        void SetOutage(GD_STD.Outage value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetOutage", ReplyAction="http://Codesys.Memory/IWriteMemor/SetOutageResponse")]
        System.Threading.Tasks.Task SetOutageAsync(GD_STD.Outage value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAPPStruct", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAPPStructResponse")]
        void SetAPPStruct(GD_STD.Phone.APP_Struct value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAPPStruct", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAPPStructResponse")]
        System.Threading.Tasks.Task SetAPPStructAsync(GD_STD.Phone.APP_Struct value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAPPMonitorMec", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAPPMonitorMecResponse")]
        void SetAPPMonitorMec(GD_STD.Phone.MonitorMec value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAPPMonitorMec", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAPPMonitorMecResponse")]
        System.Threading.Tasks.Task SetAPPMonitorMecAsync(GD_STD.Phone.MonitorMec value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMechanicalSetting", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMechanicalSettingResponse")]
        void SetMechanicalSetting(GD_STD.Phone.MechanicalSetting value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMechanicalSetting", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMechanicalSettingResponse")]
        System.Threading.Tasks.Task SetMechanicalSettingAsync(GD_STD.Phone.MechanicalSetting value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetPhoneOperating", ReplyAction="http://Codesys.Memory/IWriteMemor/SetPhoneOperatingResponse")]
        void SetPhoneOperating(GD_STD.Phone.Operating value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetPhoneOperating", ReplyAction="http://Codesys.Memory/IWriteMemor/SetPhoneOperatingResponse")]
        System.Threading.Tasks.Task SetPhoneOperatingAsync(GD_STD.Phone.Operating value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMecOptional", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMecOptionalResponse")]
        void SetMecOptional(GD_STD.Phone.MecOptional value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMecOptional", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMecOptionalResponse")]
        System.Threading.Tasks.Task SetMecOptionalAsync(GD_STD.Phone.MecOptional value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMonitorWorkOffset", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMonitorWorkOffsetResponse")]
        void SetMonitorWorkOffset(byte[] value, long offset);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetMonitorWorkOffset", ReplyAction="http://Codesys.Memory/IWriteMemor/SetMonitorWorkOffsetResponse")]
        System.Threading.Tasks.Task SetMonitorWorkOffsetAsync(byte[] value, long offset);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetWorkMaterial", ReplyAction="http://Codesys.Memory/IWriteMemor/SetWorkMaterialResponse")]
        void SetWorkMaterial(GD_STD.Phone.WorkMaterial[] value, long offset);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetWorkMaterial", ReplyAction="http://Codesys.Memory/IWriteMemor/SetWorkMaterialResponse")]
        System.Threading.Tasks.Task SetWorkMaterialAsync(GD_STD.Phone.WorkMaterial[] value, long offset);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetLogin", ReplyAction="http://Codesys.Memory/IWriteMemor/SetLoginResponse")]
        void SetLogin(GD_STD.Phone.Login value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetLogin", ReplyAction="http://Codesys.Memory/IWriteMemor/SetLoginResponse")]
        System.Threading.Tasks.Task SetLoginAsync(GD_STD.Phone.Login value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAndroidTest", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAndroidTestResponse")]
        void SetAndroidTest(GD_STD.AndroidTest value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.Memory/IWriteMemor/SetAndroidTest", ReplyAction="http://Codesys.Memory/IWriteMemor/SetAndroidTestResponse")]
        System.Threading.Tasks.Task SetAndroidTestAsync(GD_STD.AndroidTest value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWriteMemorChannel : ConsoleApp12.ServiceReference1.IWriteMemor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WriteMemorClient : System.ServiceModel.ClientBase<ConsoleApp12.ServiceReference1.IWriteMemor>, ConsoleApp12.ServiceReference1.IWriteMemor {
        
        public WriteMemorClient() {
        }
        
        public WriteMemorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WriteMemorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WriteMemorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WriteMemorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void SetPanel(GD_STD.PanelButton value) {
            base.Channel.SetPanel(value);
        }
        
        public System.Threading.Tasks.Task SetPanelAsync(GD_STD.PanelButton value) {
            return base.Channel.SetPanelAsync(value);
        }
        
        public void SetDrillWarehouse(GD_STD.DrillWarehouse value) {
            base.Channel.SetDrillWarehouse(value);
        }
        
        public System.Threading.Tasks.Task SetDrillWarehouseAsync(GD_STD.DrillWarehouse value) {
            return base.Channel.SetDrillWarehouseAsync(value);
        }
        
        public void SetOill(GD_STD.OillSystem value) {
            base.Channel.SetOill(value);
        }
        
        public System.Threading.Tasks.Task SetOillAsync(GD_STD.OillSystem value) {
            return base.Channel.SetOillAsync(value);
        }
        
        public void SetHydraulic(GD_STD.OillSystem value) {
            base.Channel.SetHydraulic(value);
        }
        
        public System.Threading.Tasks.Task SetHydraulicAsync(GD_STD.OillSystem value) {
            return base.Channel.SetHydraulicAsync(value);
        }
        
        public void SetLubricant(GD_STD.OillSystem value) {
            base.Channel.SetLubricant(value);
        }
        
        public System.Threading.Tasks.Task SetLubricantAsync(GD_STD.OillSystem value) {
            return base.Channel.SetLubricantAsync(value);
        }
        
        public void SetCut(GD_STD.OillSystem value) {
            base.Channel.SetCut(value);
        }
        
        public System.Threading.Tasks.Task SetCutAsync(GD_STD.OillSystem value) {
            return base.Channel.SetCutAsync(value);
        }
        
        public void SetHost(GD_STD.Host value) {
            base.Channel.SetHost(value);
        }
        
        public System.Threading.Tasks.Task SetHostAsync(GD_STD.Host value) {
            return base.Channel.SetHostAsync(value);
        }
        
        public void SetOutage(GD_STD.Outage value) {
            base.Channel.SetOutage(value);
        }
        
        public System.Threading.Tasks.Task SetOutageAsync(GD_STD.Outage value) {
            return base.Channel.SetOutageAsync(value);
        }
        
        public void SetAPPStruct(GD_STD.Phone.APP_Struct value) {
            base.Channel.SetAPPStruct(value);
        }
        
        public System.Threading.Tasks.Task SetAPPStructAsync(GD_STD.Phone.APP_Struct value) {
            return base.Channel.SetAPPStructAsync(value);
        }
        
        public void SetAPPMonitorMec(GD_STD.Phone.MonitorMec value) {
            base.Channel.SetAPPMonitorMec(value);
        }
        
        public System.Threading.Tasks.Task SetAPPMonitorMecAsync(GD_STD.Phone.MonitorMec value) {
            return base.Channel.SetAPPMonitorMecAsync(value);
        }
        
        public void SetMechanicalSetting(GD_STD.Phone.MechanicalSetting value) {
            base.Channel.SetMechanicalSetting(value);
        }
        
        public System.Threading.Tasks.Task SetMechanicalSettingAsync(GD_STD.Phone.MechanicalSetting value) {
            return base.Channel.SetMechanicalSettingAsync(value);
        }
        
        public void SetPhoneOperating(GD_STD.Phone.Operating value) {
            base.Channel.SetPhoneOperating(value);
        }
        
        public System.Threading.Tasks.Task SetPhoneOperatingAsync(GD_STD.Phone.Operating value) {
            return base.Channel.SetPhoneOperatingAsync(value);
        }
        
        public void SetMecOptional(GD_STD.Phone.MecOptional value) {
            base.Channel.SetMecOptional(value);
        }
        
        public System.Threading.Tasks.Task SetMecOptionalAsync(GD_STD.Phone.MecOptional value) {
            return base.Channel.SetMecOptionalAsync(value);
        }
        
        public void SetMonitorWorkOffset(byte[] value, long offset) {
            base.Channel.SetMonitorWorkOffset(value, offset);
        }
        
        public System.Threading.Tasks.Task SetMonitorWorkOffsetAsync(byte[] value, long offset) {
            return base.Channel.SetMonitorWorkOffsetAsync(value, offset);
        }
        
        public void SetWorkMaterial(GD_STD.Phone.WorkMaterial[] value, long offset) {
            base.Channel.SetWorkMaterial(value, offset);
        }
        
        public System.Threading.Tasks.Task SetWorkMaterialAsync(GD_STD.Phone.WorkMaterial[] value, long offset) {
            return base.Channel.SetWorkMaterialAsync(value, offset);
        }
        
        public void SetLogin(GD_STD.Phone.Login value) {
            base.Channel.SetLogin(value);
        }
        
        public System.Threading.Tasks.Task SetLoginAsync(GD_STD.Phone.Login value) {
            return base.Channel.SetLoginAsync(value);
        }
        
        public void SetAndroidTest(GD_STD.AndroidTest value) {
            base.Channel.SetAndroidTest(value);
        }
        
        public System.Threading.Tasks.Task SetAndroidTestAsync(GD_STD.AndroidTest value) {
            return base.Channel.SetAndroidTestAsync(value);
        }
    }
}
