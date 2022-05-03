using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class OneWayToSource : LocalBinding
    {
        new public BindingMode Mode
        {
            get => base.Mode;
            private set => base.Mode = value;
        }

        public OneWayToSource() : this(".") { }

        public OneWayToSource(string path) : base(path) => Mode = BindingMode.OneWayToSource;
    }
}