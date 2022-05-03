using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public sealed class MemberAttributes : Dictionary<Type, Attribute>
    {
        public bool Hidden => Get<HiddenAttribute>()?.Hidden == true || Get<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || Get<VisibleAttribute>()?.Visible == false;

        public MemberAttributes(MemberInfo member) : base()
        {
            if (member != null)
            {
                foreach (Attribute attribute in member.GetCustomAttributes(true))
                {
                    var type = attribute.GetType();
                    if (ContainsKey(type))
                        this[type] = attribute;

                    else Add(type, attribute);
                }
            }
        }

        public Attribute Get<Attribute>() where Attribute : System.Attribute => ContainsKey(typeof(Attribute)) ? (Attribute)this[typeof(Attribute)] : default;

        public void Set<Attribute>(Attribute input) where Attribute : System.Attribute => this[typeof(Attribute)] = input;
    }
}