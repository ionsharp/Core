using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class TwoWay : LocalBinding
    {
        new public BindingMode Mode
        {
            get => base.Mode;
            private set => base.Mode = value;
        }

        public TwoWay() : this(".") { }

        public TwoWay(string path) : base(path) => Mode = BindingMode.TwoWay;
    }
}