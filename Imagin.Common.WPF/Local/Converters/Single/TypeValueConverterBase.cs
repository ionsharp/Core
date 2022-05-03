using System;
using System.Windows.Markup;

namespace Imagin.Common.Local.Converters
{
    /// <summary>
    /// Baseclass for ValueTypeConvertes which implements easy usage as MarkupExtension
    /// </summary>
    public abstract class TypeValueConverterBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}