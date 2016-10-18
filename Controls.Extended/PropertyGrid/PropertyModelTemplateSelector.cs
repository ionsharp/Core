using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Extended
{
    public class PropertyModelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ShortTemplate
        {
            get; set;
        }

        public DataTemplate ByteTemplate
        {
            get; set;
        }

        public DataTemplate DecimalTemplate
        {
            get; set;
        }

        public DataTemplate LongTemplate
        {
            get; set;
        }

        public DataTemplate DateTimeTemplate
        {
            get; set;
        }

        public DataTemplate LinearGradientBrushTemplate
        {
            get; set;
        }

        public DataTemplate SolidColorBrushTemplate
        {
            get; set;
        }

        public DataTemplate StringTemplate
        {
            get; set;
        }

        public DataTemplate IntTemplate
        {
            get; set;
        }

        public DataTemplate DoubleTemplate
        {
            get; set;
        }

        public DataTemplate EnumTemplate
        {
            get; set;
        }

        public DataTemplate BoolTemplate
        {
            get; set;
        }

        public DataTemplate GuidTemplate
        {
            get; set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var Model = item as PropertyModel;
            if (Model == null)
                return base.SelectTemplate(item, container);

            switch (Model.Type)
            {
                case PropertyType.LinearGradientBrush:
                    return LinearGradientBrushTemplate;
                case PropertyType.SolidColorBrush:
                    return SolidColorBrushTemplate;
                case PropertyType.String:
                    return StringTemplate;
                case PropertyType.Int:
                    return IntTemplate;
                case PropertyType.Double:
                    return DoubleTemplate;
                case PropertyType.Enum:
                    return EnumTemplate;
                case PropertyType.Bool:
                    return BoolTemplate;
                case PropertyType.Guid:
                    return GuidTemplate;
                case PropertyType.DateTime:
                    return DateTimeTemplate;
                case PropertyType.Long:
                    return LongTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}
