using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Imagin.Common.Linq
{
    public static partial class XType
    {
        public static List<FieldInfo> GetAttachedProperties(this Type input)
        {
            List<FieldInfo> result = new();
            if (input?.Inherits<DependencyObject>(true) == true)
            {
                var types = XAssembly.GetAssembly(InternalAssembly.Name).GetTypes(InternalAssembly.Space.Linq, i => i.GetAttribute<ExtendsAttribute>()?.Type != null);
                foreach (var i in types)
                {
                    var extends = i.GetAttribute<ExtendsAttribute>().Type;
                    if (input.GetType().Inherits(extends, true) || (extends.IsInterface && input.GetType().Implements(extends)))
                    {
                        var fields = i.GetFields(BindingFlags.Public | BindingFlags.Static).Where(j => j.FieldType == typeof(DependencyProperty));
                        foreach (var j in fields)
                            result.Add(j);
                    }
                }
            }
            return result;
        }

        public static List<FieldInfo> GetDependencyProperties(this Type input)
        {
            List<FieldInfo> result = new();
            if (input?.Inherits<DependencyObject>(true) == true)
            {
                var fields = input.GetFields(BindingFlags.Public | BindingFlags.Static).Where(i => i.FieldType == typeof(DependencyProperty));
                fields.ForEach(i => result.Add(i));
            }
            return result;
        }
    }
}