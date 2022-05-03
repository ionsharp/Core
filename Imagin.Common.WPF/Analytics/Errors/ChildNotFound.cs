using System;

namespace Imagin.Common
{
    public class ChildNotFoundException<Child, Parent> : Exception
    {
        public ChildNotFoundException() : base($"'{typeof(Parent).FullName}' must have logical or visual child of type '{typeof(Child).FullName}'.") { }
    }
}