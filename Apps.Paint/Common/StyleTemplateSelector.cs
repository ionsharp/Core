using Imagin.Common.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Apps.Paint
{
    public class StyleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; }

        public DataTemplate Group { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IGroup)
                return Group;

            return Default;
        }

    }
}