using Imagin.Common;
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("No rights reserved.")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyDescription("Common elements for WPF projects.")]
[assembly: AssemblyProduct("Imagin.Common")]
[assembly: AssemblyTrademark("Imagin")]

[assembly: AssemblyCompany("Imagin")]
[assembly: AssemblyTitle("Imagin.Common.Wpf")]

//[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("6.6.1.0")]
[assembly: AssemblyVersion("6.6.1.0")]

//Setting ComVisible to false makes the types in this assembly not visible to COM components.  If you need to access a type in this assembly from COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: Guid("6bd41976-f6f8-461f-a071-fabc002761c9")]

//In order to begin building localizable applications, set <UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file inside a <PropertyGroup>.  For example, if you are using US english in your source files, set the <UICulture> to en-US.  Then uncomment the NeutralResourceLanguage attribute below.  Update the "en-US" in the line below to match the UICulture setting in the project file.
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.None)]

[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Analytics)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Behavior)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Collections)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.CollectionsConcurrent)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.CollectionsObjectModel)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.CollectionsSerialization)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Colors)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Common)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Configuration)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Controls)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Converters)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Data)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Effects)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Local)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.LocalEngine)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.LocalExtensions)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.LocalProviders)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Input)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Linq)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Markup)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Media)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.MediaAnimation)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Models)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Numbers)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Storage)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Text)]
[assembly: XmlnsDefinition(InternalAssembly.Xml, InternalAssembly.Space.Time)]

//[assembly: XmlnsPrefix(Xmlns.Root, "i")]