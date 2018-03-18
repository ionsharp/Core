using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenButton : Button
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        internal TokenButton(object content) : base()
        {
            Content = content;
        }

        /// <summary>
        /// 
        /// </summary>
        public TokenButton() : base()
        {
        }
    }
}
