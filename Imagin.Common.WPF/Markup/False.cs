using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class False : MarkupExtension
    {
        public False() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => false;
    }
}