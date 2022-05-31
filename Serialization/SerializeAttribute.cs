using System;

namespace Imagin.Core
{
    /// <summary>
    /// Specifies something can or can't be serialized. 
    /// Alternative to <see cref="SerializableAttribute"/>, which doesn't support properties!
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeAttribute : Attribute
    {
        public readonly bool Serializable;

        public SerializeAttribute() : this(true) { }

        public SerializeAttribute(bool serializable) : base() => Serializable = serializable;
    }
}