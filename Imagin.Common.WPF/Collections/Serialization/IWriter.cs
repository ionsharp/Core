using Imagin.Common.Analytics;

namespace Imagin.Common.Collections.Serialization
{
    public interface IWriter
    {
        Result Load();

        Result Save();
    }
}