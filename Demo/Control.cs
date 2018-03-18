using Imagin.Common;
using System;
using System.Windows;

namespace Imagin.NET.Demo
{
    public class Control : NamedObject
    {
        object instance;
        public object Instance
        {
            get => instance;
            set => Property.Set(this, ref instance, value, () => Instance);
        }

        public override string Name
        {
            get => base.Name ?? Type.Name;
            set => base.Name = value;
        }

        DataTemplate options;
        public DataTemplate Options
        {
            get => options;
            set => Property.Set(this, ref options, value, () => Options);
        }

        Type type;
        public Type Type
        {
            get => type;
            set => Property.Set(this, ref type, value, () => Type);
        }

        public Control() : this(null) { }

        public Control(string Name) : base(Name) { }
    }
}
