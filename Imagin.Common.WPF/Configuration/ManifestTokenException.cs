using System;

namespace Imagin.Common.Configuration
{
    public class ManifestTokenException : Exception
    {
        public ManifestTokenException() : base() { }

        public ManifestTokenException(string message) : base(message) { }
    }
}