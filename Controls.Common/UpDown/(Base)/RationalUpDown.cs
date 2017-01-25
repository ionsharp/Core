using System.Text.RegularExpressions;

namespace Imagin.Controls.Common
{
    public abstract class RationalUpDown<T> : NumericUpDown<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public override Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RationalUpDown() : base()
        {
        }
    }
}
