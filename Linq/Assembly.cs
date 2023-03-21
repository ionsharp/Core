using Imagin.Core.Linq;
using Imagin.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imagin.Core.Linq;

public static class XAssembly
{
    static readonly Dictionary<string, AssemblyInfo> Assemblies = new();

    ///

    public const string BaseName = "Imagin.Core";

    public const string Name = BaseName + ".WPF";

    ///

    public static Assembly GetAssembly(string assemblyName) => GetProperties(assemblyName).Assembly;

    public static Assembly GetAssembly(AssemblyType input) => GetAssembly(GetAssemblyName(input));

    public static string GetAssemblyName(AssemblyType input)
    {
        return input switch
        {
            AssemblyType.Color => BaseName + ".Color",
            AssemblyType.Core => Name,
            AssemblyType.Current => GetProperties(null).Name,
            AssemblyType.Shared => BaseName,
            _ => throw new NotSupportedException()
        };
    }

    ///

    public static AssemblyInfo GetProperties(AssemblyType input) => GetProperties(GetAssemblyName(input));

    public static AssemblyInfo GetProperties(string assemblyName)
    {
        assemblyName ??= Assembly.GetEntryAssembly().GetName().Name;
        if (!Assemblies.ContainsKey(assemblyName))
            Assemblies.Add(assemblyName, new(assemblyName));

        return Assemblies[assemblyName];
    }

    ///

    public static IEnumerable<Type> GetDerivedTypes<T>(AssemblyType assembly, bool ignoreAbstract = true, bool ignoreHidden = true)
        => GetAssembly(assembly).GetDerivedTypes<T>(ignoreAbstract, ignoreHidden);

    public static IEnumerable<Type> GetDerivedTypes<T>(this Assembly assembly, bool ignoreAbstract = true, bool ignoreHidden = true)
        => assembly.GetDerivedTypes<T>(null, ignoreAbstract, ignoreHidden);

    public static IEnumerable<Type> GetDerivedTypes<T>(this Assembly assembly, string namePath, bool ignoreAbstract = true, bool ignoreHidden = true)
    {
        var result = from type in assembly.GetTypes()
        where
        (
            ((type.IsClass && type.Inherits<T>(true)) || (typeof(T).IsInterface && type.Implements<T>()))
            &&
            (namePath == null || type.Namespace == namePath)
            &&
            (!ignoreAbstract || !type.IsAbstract)
            &&
            (!ignoreHidden || !type.HasAttribute<HideAttribute>())
        )
        select type;
        foreach (var i in result)
            yield return i;
    }

    ///

    public static IEnumerable<Type> GetTypes(this Assembly input, string namePath, Predicate<Type> where = null)
        => from type in input.GetTypes() where type.Namespace == namePath && @where?.Invoke(type) == true select type;

    public static IEnumerable<Type> GetTypes(AssemblyType assembly, string namePath, Predicate<Type> where = null) => GetAssembly(assembly).GetTypes(namePath, where);
}