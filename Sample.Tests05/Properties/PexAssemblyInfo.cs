// <copyright file="PexAssemblyInfo.cs">Copyright ©  2013</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Sample")]
[assembly: PexInstrumentAssembly("GalaSoft.MvvmLight.WPF4")]
[assembly: PexInstrumentAssembly("System.Drawing")]
[assembly: PexInstrumentAssembly("Graphviz4Net.WPF")]
[assembly: PexInstrumentAssembly("Graphviz4Net")]
[assembly: PexInstrumentAssembly("Microsoft.CSharp")]
[assembly: PexInstrumentAssembly("GalaSoft.MvvmLight.Extras.WPF4")]
[assembly: PexInstrumentAssembly("System.Windows.Forms")]
[assembly: PexInstrumentAssembly("Xceed.Wpf.Toolkit")]
[assembly: PexInstrumentAssembly("PresentationCore")]
[assembly: PexInstrumentAssembly("PresentationFramework")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("WindowsBase")]
[assembly: PexInstrumentAssembly("Microsoft.Practices.ServiceLocation")]
[assembly: PexInstrumentAssembly("System.Xaml")]
[assembly: PexInstrumentAssembly("System.Windows.Interactivity")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GalaSoft.MvvmLight.WPF4")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Drawing")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Graphviz4Net.WPF")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Graphviz4Net")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.CSharp")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GalaSoft.MvvmLight.Extras.WPF4")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Windows.Forms")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Xceed.Wpf.Toolkit")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "PresentationCore")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "PresentationFramework")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "WindowsBase")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Practices.ServiceLocation")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Xaml")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Windows.Interactivity")]

