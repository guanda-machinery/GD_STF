﻿using System.Reflection;
using System.Runtime.InteropServices;

// 組件的一般資訊是由下列的屬性集控制。
// 變更這些屬性的值即可修改組件的相關
// 資訊。
[assembly: AssemblyTitle("GD_STD")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("GD_STD.Properties")]
[assembly: AssemblyCopyright("Copyright ©  2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 將 ComVisible 設定為 false 會使得這個組件中的類型 
// 對 COM 元件而言為不可見。如果您需要從 COM 存取這個組件中 
// 的類型，請在該類型上將 ComVisible 屬性設定為 true。
[assembly: ComVisible(false)]

// 下列 GUID 為專案公開 (Expose) 至 COM 時所要使用的 typelib ID
[assembly: Guid("03d7354c-2261-487c-8d82-f209b9dbac11")]

// 組件的版本資訊由下列四個值所組成: 
//
//      主要版本
//      次要版本 
//      組建編號
//      修訂編號
//
[assembly: AssemblyVersion("1.0.0.14")]
[assembly: AssemblyFileVersion("1.0.0.14")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log.config", ConfigFileExtension = "config", Watch = true)]