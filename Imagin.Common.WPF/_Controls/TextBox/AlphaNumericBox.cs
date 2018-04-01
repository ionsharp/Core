using System.Text.RegularExpressions;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class AlphaNumericBox : RegexBoxBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected override Regex regex
        {
            get
            {
                return new Regex("^[a-zA-Z0-9]*$");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AlphaNumericBox() : base()
        {
        }
    }
}
