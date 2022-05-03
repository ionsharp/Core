using System;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class Ancestor : Binding
    {
        public Type Type { set => RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = value }; }

        public Ancestor() : this(".", null) { }

        public Ancestor(Type type) : this(".", type) { }

        public Ancestor(string path, Type type) : base(path)
        {
            Mode = BindingMode.OneWay;
            Type = type;
        }
    }
}