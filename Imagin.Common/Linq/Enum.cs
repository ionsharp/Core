using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    public static class XEnum
    {
        public static Enum AddFlag<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() | value.To<int>());

        public static Enum AddFlag(this Enum input, Enum value) => Enum.ToObject(input.GetType(), input.To<int>() | value.To<int>()) as Enum;

        public static Attribute GetAttribute<Attribute>(this Enum input) where Attribute : System.Attribute
        {
            var info = input.GetType().GetMember(input.ToString());
            if (info?.Length == 1)
            {
                foreach (var i in info[0].GetCustomAttributes(true))
                {
                    if (i is Attribute)
                        return (Attribute)i;
                }
            }
            return default;
        }

        public static Attribute[] GetAttributes<Attribute>(this Enum input) where Attribute : System.Attribute
        {
            var info = input.GetType().GetMember(input.ToString());
            if (info?.Length == 1)
            {
                var result = info[0].GetCustomAttributes(true).Where(i => i is Attribute);
                return result.Count() > 0 ? result.Cast<Attribute>().ToArray() : null;
            }
            return default;
        }

        public static IEnumerable<Attribute> GetAttributes(this Enum input)
        {
            var info = input.GetType().GetMember(input.ToString());
            return info[0].GetCustomAttributes(true).Cast<Attribute>() ?? Enumerable.Empty<Attribute>();
        }

        public static bool HasAllFlags(this Enum input, params Enum[] values) 
        {
            foreach (var i in values)
            {
                if (!input.HasFlag(i))
                    return false;
            }
            return true;
        }

        public static bool HasAnyFlags(this Enum input, params Enum[] values) 
        {
            foreach (var i in values)
            {
                if (input.HasFlag(i))
                    return true;
            }
            return false;
        }

        public static bool HasAttribute<Attribute>(this Enum input) where Attribute : System.Attribute
        {
            var info = input.GetType().GetMember(input.ToString());

            foreach (var i in info[0].GetCustomAttributes(true))
            {
                if (i is Attribute)
                    return true;
            }

            return false;
        }

        public static bool HasNoFlags(this Enum input, params Enum[] values) 
        {
            return !input.HasAnyFlags(values);
        }

        public static Enum RemoveFlag<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible 
            => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() & ~value.To<int>());

        public static Enum RemoveFlag(this Enum input, Enum value) 
            => Enum.ToObject(input.GetType(), input.To<int>() & ~value.To<int>()) as Enum;
    }
}