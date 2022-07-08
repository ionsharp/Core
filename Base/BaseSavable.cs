using Imagin.Core.Analytics;
using Imagin.Core.Serialization;

namespace Imagin.Core;

public class BaseSavable : Base
{
    protected virtual string FilePath { get; set; }

    protected virtual void OnSaving() { }

    protected virtual void OnSaved() { }

    public void Save()
    {
        OnSaving();
        BinarySerializer.Serialize(FilePath, this);
        OnSaved();
    }

    public virtual Result Load(out BaseSavable output)
    {
        var result = BinarySerializer.Deserialize(FilePath, out object options);
        output = options as BaseSavable ?? this;
        return result;
    }
}