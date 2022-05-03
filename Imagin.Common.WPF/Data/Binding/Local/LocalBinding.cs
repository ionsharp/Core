using System;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class LocalBinding : Binding
    {
        public RelativeSourceMode Relative { set => RelativeSource = new(value); }

        public Type RelativeType { set => RelativeSource = new(RelativeSourceMode.FindAncestor) { AncestorType = value }; }

        public UpdateSourceTrigger Trigger { set => UpdateSourceTrigger = value; }

        public LocalBinding() : this(".") { }

        public LocalBinding(string path) : base(path) { }
    }
}