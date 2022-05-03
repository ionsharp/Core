using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class TokenButton : Button
    {
        internal TokenButton(object content) : base() => Content = content;

        public TokenButton() : base() { }
    }
}