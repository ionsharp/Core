using Imagin.Core.Analytics;
using Imagin.Core.Collections.Serialization;
using Imagin.Core.Linq;
using Imagin.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Imagin.Core;

[Serializable]
public abstract class Savable : Base
{
    [field: NonSerialized]
    public event EventHandler<EventArgs> Saved;

    [field: NonSerialized]
    public event EventHandler<EventArgs> Saving;

    ///

    protected abstract string FileExtension { get; }

    protected abstract string FileName { get; }

    protected string FilePath => $@"{FolderPath}\{FileName}.{FileExtension}";

    protected abstract string FolderPath { get; }

    ///

    protected virtual void OnSaving() => Saving?.Invoke(this, EventArgs.Empty);

    protected virtual void OnSaved() => Saved?.Invoke(this, EventArgs.Empty);

    ///

    public Result Save()
    {
        OnSaving();
        var result = BinarySerializer.Serialize(FilePath, this);
        OnSaved();

        return result;
    }

    public virtual Result Load<T>(out T output) where T : Savable
    {
        var result = BinarySerializer.Deserialize(FilePath, out object options);
        output = (T)options ?? (T)this;
        return result;
    }
}

[Serializable]
public abstract class DataSavable : Savable
{
    public DataSavable() : base() => OnLoaded();

    [OnDeserialized]
    void OnDeserialized(StreamingContext input) => OnLoaded();

    protected virtual IEnumerable<IWriter> GetData() => default;

    protected virtual void OnLoaded()
    {
        GetData().If(i => i.Count() > 0, i => i.ForEach(j => j.Load()));
    }

    protected override void OnSaved()
    {
        base.OnSaved();
        GetData().If(i => i.Count() > 0, i => i.ForEach(j => j.Save()));
    }
}