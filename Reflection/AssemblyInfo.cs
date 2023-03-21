using Imagin.Core.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace Imagin.Core.Reflection;

public class AssemblyInfo
{
    public readonly Assembly Assembly;

    ///

    public readonly string Company;

    public readonly string Copyright;

    public readonly string Description;

    ///

    public readonly string FileVersion;

    public readonly string Version;

    ///

    public readonly string Name;

    public readonly string Product;

    public readonly string Title;

    ///

    public AssemblyInfo(string assemblyName) : base()
    {
        Assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);

        ///

        Company
            = GetAttribute<AssemblyCompanyAttribute>()?.Company;
        Copyright
            = GetAttribute<AssemblyCopyrightAttribute>()?.Copyright;
        Description
            = GetAttribute<AssemblyDescriptionAttribute>()?.Description;
        FileVersion
            = GetAttribute<AssemblyFileVersionAttribute>()?.Version;
        Name
            = Assembly.GetName().Name;
        Product
            = GetAttribute<AssemblyProductAttribute>()?.Product;
        Title
            = GetAttribute<AssemblyTitleAttribute>()?.Title;
        Version
            = GetAttribute<AssemblyVersionAttribute>()?.Version;
    }

    ///

    T GetAttribute<T>() where T : Attribute => Assembly.GetCustomAttributes(typeof(T)).OfType<T>().FirstOrDefault();
}