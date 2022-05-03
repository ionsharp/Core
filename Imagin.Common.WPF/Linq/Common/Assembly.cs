using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static class XAssembly
    {
        static readonly Dictionary<string, Assembly> cache = new();

        //...

        static T GetAttribute<T>(string assemblyName) where T : Attribute
        {
            var result = GetAssembly(assemblyName);
            return result.GetCustomAttributes(typeof(T)).OfType<T>().FirstOrDefault();
        }

        //...

        public static Assembly GetAssembly(string assemblyName)
        {
            Assembly result = null;
            if (assemblyName != null)
            {
                if (!cache.ContainsKey(assemblyName))
                    cache.Add(assemblyName, AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyName));

                result = cache[assemblyName];
            }
            return result ?? Assembly.GetEntryAssembly();
        }

        public static IEnumerable<Type> GetDerivedTypes(this Assembly assembly, Type baseType, bool ignoreAbstract = true, bool ignoreHidden = true)
            => assembly.GetDerivedTypes(baseType, null, ignoreAbstract, ignoreHidden);

        public static IEnumerable<Type> GetDerivedTypes(this Assembly assembly, Type baseType, string nameSpace, bool ignoreAbstract = true, bool ignoreHidden = true)
        {
            var result = from type in assembly.GetTypes()
            where
            (
                type.IsClass
                &&
                (nameSpace == null || type.Namespace == nameSpace)
                &&
                type.Inherits(baseType, true)
                &&
                (!ignoreAbstract || !type.IsAbstract)
                &&
                (!ignoreHidden || type.GetAttribute<HiddenAttribute>() == null || !type.GetAttribute<HiddenAttribute>().Hidden || type.GetAttribute<VisibleAttribute>()?.Visible == true)
            )
            select type;
            foreach (var i in result)
                yield return i;
        }

        public static IEnumerable<Type> GetTypes(this Assembly input, string nameSpace, Predicate<Type> predicate = null)
        {
            return from type in input.GetTypes()
            where
            (
                type.Namespace == nameSpace
                &&
                predicate?.Invoke(type) == true
            )
            select type;
        }

        //...

        public static string Company(string assemblyName = null)
        {
            return GetAttribute<AssemblyCompanyAttribute>(assemblyName)?.Company;
        }

        public static string Copyright(string assemblyName = null)
        {
            return GetAttribute<AssemblyCopyrightAttribute>(assemblyName)?.Copyright;
        }

        public static string Description(string assemblyName = null)
        {
            return GetAttribute<AssemblyDescriptionAttribute>(assemblyName)?.Description;
        }

        public static string FileVersion(string assemblyName = null)
        {
            return GetAttribute<AssemblyFileVersionAttribute>(assemblyName)?.Version;
        }

        public static string Product(string assemblyName = null)
        {
            return GetAttribute<AssemblyProductAttribute>(assemblyName)?.Product;
        }

        public static string ShortName(string assemblyName = null)
        {
            return GetAssembly(assemblyName).GetName().Name;
        }

        public static string Title(string assemblyName = null)
        {
            return GetAttribute<AssemblyTitleAttribute>(assemblyName)?.Title;
        }

        /// <summary>
        /// This attribute is not discoverable for some reason. Use <see cref="FileVersion"/> instead.
        /// </summary>
        public static string Version(string assemblyName = null)
        {
            return GetAttribute<AssemblyVersionAttribute>(assemblyName)?.Version;
        }
    }
}