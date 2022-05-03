using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Models
{
    [Serializable]
    public class PanelOptions
    {
        Dictionary<string, object> source = new();

        public PanelOptions() : base() { }

        public void Load(object input)
        {
            if (input.GetType().GetAttribute<SerializableAttribute>() != null)
            {
                foreach (var i in input.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public, MemberTypes.Field | MemberTypes.Property, null, true, true))
                {
                    if (source.ContainsKey(i.Name))
                    {
                        var result = source[i.Name];
                        //Can't trust anything these days ;'(
                        Try.Invoke(() =>
                        {
                            if (i is FieldInfo field)
                                field.SetValue(input, result);

                            if (i is PropertyInfo property)
                                property.SetValue(input, result);
                        },
                        e => Log.Write<PanelOptions>(e));
                    }
                }
            }
            else Log.Write<PanelOptions>(new NotSerializableWarning(input));
        }

        public void Save(object input)
        {
            if (input.GetType().GetAttribute<SerializableAttribute>() != null)
            {
                foreach (var j in input.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public, MemberTypes.Field | MemberTypes.Property, null, true, true))
                {
                    Try.Invoke(() =>
                    {
                        if (j is FieldInfo field)
                            source.Add(j.Name, field.GetValue(input));

                        if (j is PropertyInfo property)
                            source.Add(j.Name, property.GetValue(input));
                    },
                    e => Log.Write<PanelOptions>(e));
                }
            }
            else Log.Write<PanelOptions>(new NotSerializableWarning(input));
        }
    }
}