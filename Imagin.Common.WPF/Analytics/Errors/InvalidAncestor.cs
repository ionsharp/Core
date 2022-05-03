using System;

namespace Imagin.Common
{
    public class InvalidAncestor<Target> : Exception
    {
        public InvalidAncestor() : base($"'{typeof(Target).FullName}' must be an ancestor.") { }
    }
}