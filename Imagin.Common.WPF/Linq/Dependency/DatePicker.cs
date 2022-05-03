using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(DatePicker))]
    public static class XDatePicker
    {
        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(object), typeof(XDatePicker), new FrameworkPropertyMetadata(null));
        public static object GetPlaceholder(DatePicker i) => (object)i.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(DatePicker i, object input) => i.SetValue(PlaceholderProperty, input);

        #endregion

        #region PlaceholderTemplate

        public static readonly DependencyProperty PlaceholderTemplateProperty = DependencyProperty.RegisterAttached("PlaceholderTemplate", typeof(DataTemplate), typeof(XDatePicker), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetPlaceholderTemplate(DatePicker i) => (DataTemplate)i.GetValue(PlaceholderTemplateProperty);
        public static void SetPlaceholderTemplate(DatePicker i, DataTemplate input) => i.SetValue(PlaceholderTemplateProperty, input);

        #endregion
    }
}