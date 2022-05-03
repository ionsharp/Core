using System;

namespace Imagin.Common
{
    public class ExternalChangeException<Object> : Exception
    {
        public ExternalChangeException(string propertyName) : base($"External changes to '{typeof(Object).FullName}.{propertyName}' are not allowed.") { }
    }
}