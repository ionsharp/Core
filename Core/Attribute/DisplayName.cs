using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public readonly string DisplayName;

        public readonly bool Localize;

        public DisplayNameAttribute(string displayName, bool localize = true) : base()
        {
            DisplayName 
                = displayName;
            Localize 
                = localize;
        }
    }
}