using Imagin.Common.Linq;
using Imagin.Common.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Controls
{
    public class MemberSource : IEnumerable
    {
        public class Ancestor
        {
            public readonly string Name;

            public readonly object Value;

            public Ancestor(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        //...

        readonly Array Items;

        public readonly Ancestor Parent;

        /// <summary>
        /// Gets the shared type of all items.
        /// </summary>
        public readonly Type SharedType;

        //...

        public int Count => Items.Length;

        public Types DataType => SharedType.IsValueType ? Reflection.Types.Value : Reflection.Types.Reference;

        public object First => this[0];

        /// <summary>
        /// Gets if indeterminate: An array of something where at least one object does not share a base type with the others.
        /// </summary>
        public bool Indeterminate => SharedType == null;

        /// <summary>
        /// Gets the type of each item.
        /// </summary>
        public IEnumerable<Type> Types => Items.Select(i => i.GetType());

        //...

        public MemberSource(MemberRouteElement input, Ancestor ancestor)
        {
            Parent 
                = ancestor;
            Items 
                = input.Values;
            SharedType 
                = Count > 1 ? GetSharedType(Types) : First.GetType();
        }

        public object this[int index]
        {
            get
            {
                var i = 0;
                foreach (var j in Items)
                {
                    if (i == index)
                        return j;

                    i++;
                }
                return null;
            }
        }

        //...

        Type GetSharedType(IEnumerable<Type> types)
        {
            if (types == null)
                return null;

            Type a = null;
            foreach (var i in types)
            {
                if (a == null)
                {
                    a = i;
                    continue;
                }

                var b = i;

                //Compare a and b
                while (a != typeof(object))
                {
                    while (b != typeof(object))
                    {
                        if (a == b)
                            goto next;

                        b = b.BaseType;
                    }
                    a = a.BaseType;
                    b = i;
                }

                return null;
                next: continue;
            }
            return a;
        }

        //...

        public bool Contains(Predicate<object> predicate) => Items.Contains(predicate);

        public IEnumerator GetEnumerator() => Items.GetEnumerator();
    }
}