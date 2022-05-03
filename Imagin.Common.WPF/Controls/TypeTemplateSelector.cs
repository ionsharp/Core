using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.Common.Controls
{
    [ContentProperty(nameof(Templates))]
    public class TypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; } = new();

        public virtual bool Strict { get; set; } = true;

        public List<DataTemplate> Templates { get; set; } = new();

        //...

        public TypeTemplateSelector() : base() { }

        //...

        protected bool Check(object item, DataTemplate template)
        {
            var a = item?.GetType();
            var b = template.DataType as Type;
            if (a != null && b != null)
            {
                if (Strict)
                    return a == b;

                else if (item != null)
                    return a.Inherits(b) || a.Implements(b);
            }
            return false;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) => Templates.FirstOrDefault(i => Check(item, i)) ?? Default;
    }
}