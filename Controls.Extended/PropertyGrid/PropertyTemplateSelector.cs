using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Extended
{
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources
        {
            get; set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                foreach (DictionaryEntry i in Resources)
                {
                    if (i.Key.As<Type>() == item.As<PropertyModel>().Primitive)
                        return i.Value as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
