using System.Text.RegularExpressions;

namespace Imagin.Controls.Common
{
    public abstract class RationalUpDown<T> : NumericUpDown<T>
    {
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        public RationalUpDown() : base()
        {
        }
    }
}
