using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class OneWay : LocalBinding
    {
        new public BindingMode Mode
        {
            get => base.Mode;
            private set => base.Mode = value;
        }

        public OneWay() : this(".") { }

        public OneWay(string path) : base(path) => Mode = BindingMode.OneWay;
    }
}