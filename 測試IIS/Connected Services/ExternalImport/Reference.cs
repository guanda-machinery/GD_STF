﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 測試IIS.ExternalImport {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SteelPart", Namespace="http://schemas.datacontract.org/2004/07/GD_STD.Data")]
    [System.SerializableAttribute()]
    public partial class SteelPart : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int Countk__BackingFieldField;
        
        private System.DateTime Creationk__BackingFieldField;
        
        private string DrawingNamek__BackingFieldField;
        
        private int[] Fatherk__BackingFieldField;
        
        private System.Nullable<System.Guid> GUIDk__BackingFieldField;
        
        private float Hk__BackingFieldField;
        
        private int[] IDk__BackingFieldField;
        
        private bool IsTeklak__BackingFieldField;
        
        private double Lengthk__BackingFieldField;
        
        private bool Lockk__BackingFieldField;
        
        private bool[] Matchk__BackingFieldField;
        
        private string Materialk__BackingFieldField;
        
        private bool Nck__BackingFieldField;
        
        private string Numberk__BackingFieldField;
        
        private string Profilek__BackingFieldField;
        
        private System.DateTime Revisek__BackingFieldField;
        
        private int SortCountk__BackingFieldField;
        
        private 測試IIS.ExternalImport.DRAWING_STATE Statek__BackingFieldField;
        
        private 測試IIS.ExternalImport.OBJECT_TYPE Typek__BackingFieldField;
        
        private double UnitAreak__BackingFieldField;
        
        private double UnitWeightk__BackingFieldField;
        
        private float Wk__BackingFieldField;
        
        private float t1k__BackingFieldField;
        
        private float t2k__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Count>k__BackingField", IsRequired=true)]
        public int Countk__BackingField {
            get {
                return this.Countk__BackingFieldField;
            }
            set {
                if ((this.Countk__BackingFieldField.Equals(value) != true)) {
                    this.Countk__BackingFieldField = value;
                    this.RaisePropertyChanged("Countk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Creation>k__BackingField", IsRequired=true)]
        public System.DateTime Creationk__BackingField {
            get {
                return this.Creationk__BackingFieldField;
            }
            set {
                if ((this.Creationk__BackingFieldField.Equals(value) != true)) {
                    this.Creationk__BackingFieldField = value;
                    this.RaisePropertyChanged("Creationk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<DrawingName>k__BackingField", IsRequired=true)]
        public string DrawingNamek__BackingField {
            get {
                return this.DrawingNamek__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.DrawingNamek__BackingFieldField, value) != true)) {
                    this.DrawingNamek__BackingFieldField = value;
                    this.RaisePropertyChanged("DrawingNamek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Father>k__BackingField", IsRequired=true)]
        public int[] Fatherk__BackingField {
            get {
                return this.Fatherk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Fatherk__BackingFieldField, value) != true)) {
                    this.Fatherk__BackingFieldField = value;
                    this.RaisePropertyChanged("Fatherk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<GUID>k__BackingField", IsRequired=true)]
        public System.Nullable<System.Guid> GUIDk__BackingField {
            get {
                return this.GUIDk__BackingFieldField;
            }
            set {
                if ((this.GUIDk__BackingFieldField.Equals(value) != true)) {
                    this.GUIDk__BackingFieldField = value;
                    this.RaisePropertyChanged("GUIDk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<H>k__BackingField", IsRequired=true)]
        public float Hk__BackingField {
            get {
                return this.Hk__BackingFieldField;
            }
            set {
                if ((this.Hk__BackingFieldField.Equals(value) != true)) {
                    this.Hk__BackingFieldField = value;
                    this.RaisePropertyChanged("Hk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<ID>k__BackingField", IsRequired=true)]
        public int[] IDk__BackingField {
            get {
                return this.IDk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.IDk__BackingFieldField, value) != true)) {
                    this.IDk__BackingFieldField = value;
                    this.RaisePropertyChanged("IDk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IsTekla>k__BackingField", IsRequired=true)]
        public bool IsTeklak__BackingField {
            get {
                return this.IsTeklak__BackingFieldField;
            }
            set {
                if ((this.IsTeklak__BackingFieldField.Equals(value) != true)) {
                    this.IsTeklak__BackingFieldField = value;
                    this.RaisePropertyChanged("IsTeklak__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Length>k__BackingField", IsRequired=true)]
        public double Lengthk__BackingField {
            get {
                return this.Lengthk__BackingFieldField;
            }
            set {
                if ((this.Lengthk__BackingFieldField.Equals(value) != true)) {
                    this.Lengthk__BackingFieldField = value;
                    this.RaisePropertyChanged("Lengthk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Lock>k__BackingField", IsRequired=true)]
        public bool Lockk__BackingField {
            get {
                return this.Lockk__BackingFieldField;
            }
            set {
                if ((this.Lockk__BackingFieldField.Equals(value) != true)) {
                    this.Lockk__BackingFieldField = value;
                    this.RaisePropertyChanged("Lockk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Match>k__BackingField", IsRequired=true)]
        public bool[] Matchk__BackingField {
            get {
                return this.Matchk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Matchk__BackingFieldField, value) != true)) {
                    this.Matchk__BackingFieldField = value;
                    this.RaisePropertyChanged("Matchk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Material>k__BackingField", IsRequired=true)]
        public string Materialk__BackingField {
            get {
                return this.Materialk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Materialk__BackingFieldField, value) != true)) {
                    this.Materialk__BackingFieldField = value;
                    this.RaisePropertyChanged("Materialk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Nc>k__BackingField", IsRequired=true)]
        public bool Nck__BackingField {
            get {
                return this.Nck__BackingFieldField;
            }
            set {
                if ((this.Nck__BackingFieldField.Equals(value) != true)) {
                    this.Nck__BackingFieldField = value;
                    this.RaisePropertyChanged("Nck__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Number>k__BackingField", IsRequired=true)]
        public string Numberk__BackingField {
            get {
                return this.Numberk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Numberk__BackingFieldField, value) != true)) {
                    this.Numberk__BackingFieldField = value;
                    this.RaisePropertyChanged("Numberk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Profile>k__BackingField", IsRequired=true)]
        public string Profilek__BackingField {
            get {
                return this.Profilek__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Profilek__BackingFieldField, value) != true)) {
                    this.Profilek__BackingFieldField = value;
                    this.RaisePropertyChanged("Profilek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Revise>k__BackingField", IsRequired=true)]
        public System.DateTime Revisek__BackingField {
            get {
                return this.Revisek__BackingFieldField;
            }
            set {
                if ((this.Revisek__BackingFieldField.Equals(value) != true)) {
                    this.Revisek__BackingFieldField = value;
                    this.RaisePropertyChanged("Revisek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<SortCount>k__BackingField", IsRequired=true)]
        public int SortCountk__BackingField {
            get {
                return this.SortCountk__BackingFieldField;
            }
            set {
                if ((this.SortCountk__BackingFieldField.Equals(value) != true)) {
                    this.SortCountk__BackingFieldField = value;
                    this.RaisePropertyChanged("SortCountk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<State>k__BackingField", IsRequired=true)]
        public 測試IIS.ExternalImport.DRAWING_STATE Statek__BackingField {
            get {
                return this.Statek__BackingFieldField;
            }
            set {
                if ((this.Statek__BackingFieldField.Equals(value) != true)) {
                    this.Statek__BackingFieldField = value;
                    this.RaisePropertyChanged("Statek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Type>k__BackingField", IsRequired=true)]
        public 測試IIS.ExternalImport.OBJECT_TYPE Typek__BackingField {
            get {
                return this.Typek__BackingFieldField;
            }
            set {
                if ((this.Typek__BackingFieldField.Equals(value) != true)) {
                    this.Typek__BackingFieldField = value;
                    this.RaisePropertyChanged("Typek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<UnitArea>k__BackingField", IsRequired=true)]
        public double UnitAreak__BackingField {
            get {
                return this.UnitAreak__BackingFieldField;
            }
            set {
                if ((this.UnitAreak__BackingFieldField.Equals(value) != true)) {
                    this.UnitAreak__BackingFieldField = value;
                    this.RaisePropertyChanged("UnitAreak__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<UnitWeight>k__BackingField", IsRequired=true)]
        public double UnitWeightk__BackingField {
            get {
                return this.UnitWeightk__BackingFieldField;
            }
            set {
                if ((this.UnitWeightk__BackingFieldField.Equals(value) != true)) {
                    this.UnitWeightk__BackingFieldField = value;
                    this.RaisePropertyChanged("UnitWeightk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<W>k__BackingField", IsRequired=true)]
        public float Wk__BackingField {
            get {
                return this.Wk__BackingFieldField;
            }
            set {
                if ((this.Wk__BackingFieldField.Equals(value) != true)) {
                    this.Wk__BackingFieldField = value;
                    this.RaisePropertyChanged("Wk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<t1>k__BackingField", IsRequired=true)]
        public float t1k__BackingField {
            get {
                return this.t1k__BackingFieldField;
            }
            set {
                if ((this.t1k__BackingFieldField.Equals(value) != true)) {
                    this.t1k__BackingFieldField = value;
                    this.RaisePropertyChanged("t1k__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<t2>k__BackingField", IsRequired=true)]
        public float t2k__BackingField {
            get {
                return this.t2k__BackingFieldField;
            }
            set {
                if ((this.t2k__BackingFieldField.Equals(value) != true)) {
                    this.t2k__BackingFieldField = value;
                    this.RaisePropertyChanged("t2k__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DRAWING_STATE", Namespace="http://schemas.datacontract.org/2004/07/GD_STD.Enum")]
    public enum DRAWING_STATE : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NULL = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        INCREASE_COUNT = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REDUCE_COUNT = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CHANGE = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NEW = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DELETE = 5,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OBJECT_TYPE", Namespace="http://schemas.datacontract.org/2004/07/GD_STD.Enum")]
    public enum OBJECT_TYPE : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RH = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CH = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        L = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BOX = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BH = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PLATE = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        C = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RB = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FB = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PIPE = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TUBE = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        H = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        I = 13,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LB = 14,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BT = 15,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CT = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        T = 17,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TURN_BUCKLE = 18,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WELD = 19,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SA = 20,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GRATING = 21,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        HNUT = 22,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NUT = 23,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://HMI.ExternalImportDuplex", ConfigurationName="ExternalImport.IExternalImport", CallbackContract=typeof(測試IIS.ExternalImport.IExternalImportCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IExternalImport {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/RootDirectory")]
        void RootDirectory();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/RootDirectory")]
        System.Threading.Tasks.Task RootDirectoryAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/ReadDirectory")]
        void ReadDirectory(string path);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/ReadDirectory")]
        System.Threading.Tasks.Task ReadDirectoryAsync(string path);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/CreateFile")]
        void CreateFile(string path, string projectName, int length);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/CreateFile")]
        System.Threading.Tasks.Task CreateFileAsync(string path, string projectName, int length);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/WriteFile")]
        void WriteFile(byte[] data);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/WriteFile")]
        System.Threading.Tasks.Task WriteFileAsync(byte[] data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExternalImportCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/ResponseDirectory")]
        void ResponseDirectory(string[] path);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/WriteStream")]
        void WriteStream(long position, long schedule);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/Conflict")]
        void Conflict(測試IIS.ExternalImport.SteelPart[] list);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://HMI.ExternalImportDuplex/IExternalImport/EndFile")]
        void EndFile();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExternalImportChannel : 測試IIS.ExternalImport.IExternalImport, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExternalImportClient : System.ServiceModel.DuplexClientBase<測試IIS.ExternalImport.IExternalImport>, 測試IIS.ExternalImport.IExternalImport {
        
        public ExternalImportClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ExternalImportClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ExternalImportClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ExternalImportClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ExternalImportClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RootDirectory() {
            base.Channel.RootDirectory();
        }
        
        public System.Threading.Tasks.Task RootDirectoryAsync() {
            return base.Channel.RootDirectoryAsync();
        }
        
        public void ReadDirectory(string path) {
            base.Channel.ReadDirectory(path);
        }
        
        public System.Threading.Tasks.Task ReadDirectoryAsync(string path) {
            return base.Channel.ReadDirectoryAsync(path);
        }
        
        public void CreateFile(string path, string projectName, int length) {
            base.Channel.CreateFile(path, projectName, length);
        }
        
        public System.Threading.Tasks.Task CreateFileAsync(string path, string projectName, int length) {
            return base.Channel.CreateFileAsync(path, projectName, length);
        }
        
        public void WriteFile(byte[] data) {
            base.Channel.WriteFile(data);
        }
        
        public System.Threading.Tasks.Task WriteFileAsync(byte[] data) {
            return base.Channel.WriteFileAsync(data);
        }
    }
}
