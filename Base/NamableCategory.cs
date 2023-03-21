using System;

namespace Imagin.Core;

///<inheritdoc/>
[Serializable]
public class NamableCategory<T> : Namable<T>
{
    public string Category { get => Get(""); set => Set(value); }

    public NamableCategory() : base() { }

    public NamableCategory(string name, string category = default) : base(name) => Category = category;

    public NamableCategory(string name, string category, T value) : base(name, value) => Category = category;
}