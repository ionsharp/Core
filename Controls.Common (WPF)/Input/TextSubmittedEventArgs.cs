using System;

namespace Imagin.Controls.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class TextSubmittedEventArgs : EventArgs
    {
        readonly string text;
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public TextSubmittedEventArgs(string Text) : base()
        {
            text = Text;
        }
    }
}
