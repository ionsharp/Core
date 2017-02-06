using Imagin.Common;
using Imagin.Common.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Windows;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyAttributes : List<Span<Type, string, object>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public void Add(Type a, string b, object c)
        {
            Add(new Span<Type, string, object>(a, b, c));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Property"></param>
        public void ExtractFrom(PropertyInfo Property)
        {
            foreach (var i in Property.GetCustomAttributes(true))
            {
                var j = this.WhereFirst(k => k.First == i.GetType());

                if (j != null)
                    j.Third = j.Second == null ? i : i.GetValue(j.Second);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public TValue Get<TAttribute, TValue>() where TAttribute : Attribute
        {
            return (TValue)this.WhereFirst(i => i.First == typeof(TAttribute)).Third;
        }
    }
}
