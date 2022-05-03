using System.IO;

namespace Imagin.Common.Configuration
{
    public class ManifestNotFoundException : FileNotFoundException
    {
        public readonly bool IsRemote;

        public ManifestNotFoundException(bool isRemote, string message) : base(message)
        {
            IsRemote = isRemote;
        }
    }
}