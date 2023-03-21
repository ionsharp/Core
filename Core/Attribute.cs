using System;

namespace Imagin.Core;

public abstract class Attribute<T> : Attribute
{
    public readonly T Value;

    public Attribute(T input) : base() => Value = input;
}

public abstract class BindingAttribute : Attribute
{
    public Type Converter { get; set; }

    public object ConverterParameter { get; set; }

    public string Path { get; set; }

    public string StringFormat { get; set; }

    public BindingAttribute() : base() { }
}

public abstract class LocalizableAttribute : Attribute
{
    public bool Localize { get; set; }

    public LocalizableAttribute(bool localize = true) : base() => Localize = localize;
}

public abstract class TemplateAttribute : Attribute
{
    public string Key { get; set; }

    public string Path { get; set; }

    public Type Source { get; set; }

    public TemplateAttribute(string path, Type source, string key) : base()
    {
        Path = path; Source = source; Key = key;
    }
}