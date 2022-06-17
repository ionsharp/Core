using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtendsAttribute : Attribute
    {
        public readonly Type Type;

        public ExtendsAttribute(Type type) => Type = type;
    }
}