using System.Windows.Controls;

namespace Imagin.Controls.Common
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
        /// <param name="contentTemplate"></param>
        /// <param name="contentTemplateSelector"></param>
        internal TokenButton(object content) : base()
        {
            Content = content;
        }
    }
}
