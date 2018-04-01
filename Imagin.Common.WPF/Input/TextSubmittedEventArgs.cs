using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class TextSubmittedEventArgs : EventArgs
    {
        readonly string _text;
        /// <summary>
        /// 
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public TextSubmittedEventArgs(string Text) : base() => _text = Text;
    }
}
