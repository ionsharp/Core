using System;

namespace Imagin.Core;

[Serializable]
public class NamableCategory<T> : Namable<T>
{
    string category = default;
    public string Category
    {
        get => category;
        set => this.Change(ref category, value);
    }

    public NamableCategory() : base() { }

    public NamableCategory(string name, string category = default) : base(name) => Category = category;

    public NamableCategory(string name, string category, T value) : base(name, value) => Category = category;
}