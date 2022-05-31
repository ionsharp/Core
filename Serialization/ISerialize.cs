using Imagin.Core.Analytics;

namespace Imagin.Core.Serialization
{
    public interface ISerialize
    {
        string FilePath { get; }

        Result Deserialize(string filePath, out object data);

        Result Serialize(object data);

        Result Serialize(string filePath, object data);
    }
}