using System;

namespace Imagin.Common
{
    /// <summary>
    /// An alternative for <see langword="System.ComponentModel.DescriptionAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        public readonly string Description;

        public readonly string Name;

        public readonly bool Localize;

        public DescriptionAttribute(string description = "", bool localize = true) : base()
        {
            Description
                = description;
            Localize
                = localize;
        }

        public DescriptionAttribute(string name, string description, bool localize = true) : this(description, localize)
        {
            Name = name;
        }
    }
}