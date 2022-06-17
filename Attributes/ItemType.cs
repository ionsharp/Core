using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ItemTypeAttribute : Attribute
    {
        public readonly Type ItemType;

        public ItemTypeAttribute(Type type) : base() => ItemType = type;
    }
}