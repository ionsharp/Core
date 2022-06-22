using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ViewAttribute : Attribute
    {
        public readonly string Name;

        public ViewAttribute(string name) : base() => Name = name;

        public ViewAttribute(object name) : this($"{name}") { }
    }
}