using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Design-time is not supported!
    /// </summary>
    public sealed class TemplatedParent : Binding
    {
        public TemplatedParent() : this(".") { }

        public TemplatedParent(string path) : base(path)
        {
            Mode = BindingMode.OneWay;
            RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
        }
    }
}