using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ComboBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedItemTemplate { get; set; }

        public DataTemplateSelector SelectedItemTemplateSelector { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        public DataTemplateSelector ItemTemplateSelector { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //Search up visual tree, stopping at either a ComboBox or a ComboBoxItem (or null). This will determine which template to use
            while (container != null && !(container is ComboBox) && !(container is ComboBoxItem))
                container = VisualTreeHelper.GetParent(container);

            //If you stopped at a ComboBoxItem, you're in the dropdown
            var inDropDown = container is ComboBoxItem;

            return inDropDown
                ? ItemTemplate
                    ?? ItemTemplateSelector?.SelectTemplate(item, container)
                : SelectedItemTemplate 
                    ?? SelectedItemTemplateSelector?.SelectTemplate(item, container);
        }
    }

    public class ComboBoxTemplateSelectorExtension : MarkupExtension
    {
        public DataTemplate SelectedItemTemplate { get; set; }

        public DataTemplateSelector SelectedItemTemplateSelector { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        public DataTemplateSelector ItemTemplateSelector { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider) => new ComboBoxTemplateSelector()
        {
            SelectedItemTemplate 
                = SelectedItemTemplate,
            SelectedItemTemplateSelector 
                = SelectedItemTemplateSelector,
            ItemTemplate
                = ItemTemplate,
            ItemTemplateSelector
                = ItemTemplateSelector
        };
    }
}