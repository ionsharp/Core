using Imagin.Common;
using Imagin.Common.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Controls.Extended
{
    public class PropertyAttributes : List<Span<string, string, object>>
    {
        public void Add(string a, string b, object c)
        {
            Add(New(a, b, c));
        }

        public void ExtractFrom(PropertyInfo Property)
        {
            foreach (var Attribute in Property.GetCustomAttributes(true))
            {
                var AttributeType = Attribute.GetType().Name.ToString();

                for (var j = 0; j < this.Count; j++)
                {
                    if (AttributeType == base[j].First + "Attribute")
                    {
                        var PropertyName = base[j].Second;
                        base[j].Third = PropertyName.IsNullOrEmpty() ? Attribute : Attribute.GetValue(PropertyName);
                        break;
                    }
                }
            }
        }

        public Span<string, string, object> New(string a, string b, object c)
        {
            return new Span<string, string, object>(a, b, c);
        }

        public object this[string Key, bool MemberOrValue]
        {
            get
            {
                var Item = this.WhereFirst(x => x.First == Key);
                return MemberOrValue ? Item.Second : Item.Third;
            }
            set
            {
                var i = IndexOf(this.WhereFirst(x => x.First == Key));

                if (MemberOrValue)
                {
                    base[i] = New(base[i].First, value.ToString(), base[i].Third);
                }
                else
                {
                    base[i] = New(base[i].First, base[i].Second, value);
                }
            }
        }
    }
}
