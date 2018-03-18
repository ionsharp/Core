using System.Text.RegularExpressions;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class HexBox : AlphaNumericBox
    {
        /// <summary>
        /// 
        /// </summary>
        protected override Regex regex
        {
            get
            {
                return new Regex("^[a-fA-F0-9]*$");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HexBox() : base()
        {
        }
    }
}
