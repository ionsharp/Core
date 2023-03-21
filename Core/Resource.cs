using Imagin.Core.Linq;
using Imagin.Core.Reflection;
using System;

namespace Imagin.Core;

public static class Resource
{
    public static Uri GetImageUri(object image) => GetUri($"Images/{image}.png", AssemblyType.Core);

    public static Uri GetImageUri(string fileName, AssemblyType assembly = AssemblyType.Current) => GetUri($"Images/{fileName}", assembly);

    public static Uri GetUri(string relativePath, AssemblyType assembly = AssemblyType.Current) => GetUri(XAssembly.GetAssemblyName(assembly), relativePath);

    public static Uri GetUri(string assemblyName, string relativePath) => new($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);
}