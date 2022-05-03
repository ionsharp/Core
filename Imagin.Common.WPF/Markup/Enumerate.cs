using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class Enumerate : MarkupExtension
    {
        public bool Sort { get; set; } = false;

        public bool String { get; set; } = false;

        public Type Type { get; set; }

        public Appearance Visibility { get; set; } = Appearance.Visible;

        public Enumerate(Type type) : base() => Type = type;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type != null)
            {
                if (String)
                    return Type.GetEnumCollection(Visibility, i => i.ToString(), Sort);

                return Type.GetEnumCollection(Visibility, Sort);
            }
            return DependencyProperty.UnsetValue;
        }
    }
}