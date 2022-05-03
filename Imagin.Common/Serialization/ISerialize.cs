using Imagin.Common.Analytics;

namespace Imagin.Common.Serialization
{
    public interface ISerialize
    {
        string FilePath { get; }

        Result Deserialize(string filePath, out object data);

        Result Serialize(object data);

        Result Serialize(string filePath, object data);
    }
}