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
}