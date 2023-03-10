// <copyright file="PexAssemblyInfo.cs">Copyright ©  2021</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "MSTestv2")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("GD_STD")]
[assembly: PexInstrumentAssembly("GD_STD.Phone")]
[assembly: PexInstrumentAssembly("GD_STD.Enum")]
[assembly: PexInstrumentAssembly("System.Web")]
[assembly: PexInstrumentAssembly("Newtonsoft.Json")]
[assembly: PexInstrumentAssembly("PresentationFramework")]
[assembly: PexInstrumentAssembly("System.Runtime.Serialization")]
[assembly: PexInstrumentAssembly("log4net")]
[assembly: PexInstrumentAssembly("GD_STD.Base")]
[assembly: PexInstrumentAssembly("WPFWindowsBase")]
[assembly: PexInstrumentAssembly("System.Core")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GD_STD.Phone")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GD_STD.Enum")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Newtonsoft.Json")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "PresentationFramework")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Runtime.Serialization")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "log4net")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GD_STD.Base")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "WPFWindowsBase")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]

