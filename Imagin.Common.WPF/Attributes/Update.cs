using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UpdateAttribute : Attribute
    {
        public readonly double Seconds;

        public UpdateAttribute(double seconds) : base() => Seconds = seconds;
    }
}