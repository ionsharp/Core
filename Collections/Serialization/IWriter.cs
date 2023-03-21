using Imagin.Core.Analytics;

namespace Imagin.Core.Collections.Serialization;

public interface IWriter
{
    Result Load();

    Result Save();
}