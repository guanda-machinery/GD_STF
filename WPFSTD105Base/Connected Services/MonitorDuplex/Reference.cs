﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFSTD105.MonitorDuplex {
    
    
    /// 從WSDL文檔：
    /// 
    ///  
    /// 
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Codesys.MonitorDuplex", ConfigurationName="MonitorDuplex.IMonitorDuplex", CallbackContract=typeof(WPFSTD105.MonitorDuplex.IMonitorDuplexCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IMonitorDuplex {
        
        /// 從WSDL文檔：
        /// 
        ///  
        /// 
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.MonitorDuplex/IMonitorDuplex/SetMonitorWorkOffset", ReplyAction="http://Codesys.MonitorDuplex/IMonitorDuplex/SetMonitorWorkOffsetResponse")]
        void SetMonitorWorkOffset(byte[] value, long offset);
        
        /// 從WSDL文檔：
        /// 
        ///  
        /// 
        [System.ServiceModel.OperationContractAttribute(Action="http://Codesys.MonitorDuplex/IMonitorDuplex/GetWorkMaterial", ReplyAction="http://Codesys.MonitorDuplex/IMonitorDuplex/GetWorkMaterialResponse")]
        GD_STD.Phone.WorkMaterial GetWorkMaterial(int index);
        
        /// 從WSDL文檔：
        /// 
        ///  
        /// 
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Codesys.MonitorDuplex/IMonitorDuplex/GetIndex")]
        void GetIndex();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMonitorDuplexCallback {
        
        /// 從WSDL文檔：
        /// 
        ///  
        /// 
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Codesys.MonitorDuplex/IMonitorDuplex/SendIndex")]
        void SendIndex(short[] index);
        
        /// 從WSDL文檔：
        /// 
        ///  
        /// 
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://Codesys.MonitorDuplex/IMonitorDuplex/SendOther")]
        void SendOther(short current, double enOccupy, double exOccupy1, double exOccupy2);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMonitorDuplexChannel : WPFSTD105.MonitorDuplex.IMonitorDuplex, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MonitorDuplexClient : System.ServiceModel.DuplexClientBase<WPFSTD105.MonitorDuplex.IMonitorDuplex>, WPFSTD105.MonitorDuplex.IMonitorDuplex {
        
        public MonitorDuplexClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public MonitorDuplexClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public MonitorDuplexClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MonitorDuplexClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MonitorDuplexClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void SetMonitorWorkOffset(byte[] value, long offset) {
            base.Channel.SetMonitorWorkOffset(value, offset);
        }
        
        public GD_STD.Phone.WorkMaterial GetWorkMaterial(int index) {
            return base.Channel.GetWorkMaterial(index);
        }
        
        public void GetIndex() {
            base.Channel.GetIndex();
        }
    }
}
