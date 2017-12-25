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
            get
            {
                return instance;
            }
            set
            {
                SetValue(ref instance, value, () => Instance);
            }
        }

        public override string Name
        {
            get
            {
                return base.Name ?? Type.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        DataTemplate options;
        public DataTemplate Options
        {
            get
            {
                return options;
            }
            set
            {
                SetValue(ref options, value, () => Options);
            }
        }

        Type type;
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                SetValue(ref type, value, () => Type);
            }
        }

        public Control() : this(null)
        {
        }

        public Control(string Name) : base(Name)
        {
        }
    }
}
