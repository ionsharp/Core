using System;

namespace Imagin.Core.Collections.ObjectModel;

[Categorize(false), Serializable]
public class GroupItem<T> : Namable<T>, IDescription, IGeneric
{
    [Hide]
    public DateTime Created { get => Get(DateTime.Now); private set => Set(value); }

    [Index(1), Modify, Pin(Pin.AboveOrLeft), StringStyle(StringStyle.MultiLine), Vertical]
    public string Description { get => Get(""); set => Set(value); }

    [Hide]
    public bool IsSelected { get => Get(false); set => Set(value); }

    [Editable, HideName, Modify]
    public override T Value { get => base.Value; set => base.Value = value; }

    ///

    public GroupItem() : this("") { }

    public GroupItem(T value) : this("", value) { }

    public GroupItem(string name) : base(name) { }

    public GroupItem(string name, T value) : this(name) => Value = value;

    public GroupItem(string name, string description, T value) : this(name, value) => Description = description;

    ///

    Type IGeneric.GetGenericType() => Value is T i ? i.GetType() : typeof(T);
}