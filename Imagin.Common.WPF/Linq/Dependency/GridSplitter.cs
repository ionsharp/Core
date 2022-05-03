using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(GridSplitter))]
    public static class XGridSplitter
    {
        #region Content

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(object), typeof(XGridSplitter), new FrameworkPropertyMetadata(null));
        public static object GetContent(GridSplitter i) => (object)i.GetValue(ContentProperty);
        public static void SetContent(GridSplitter i, object input) => i.SetValue(ContentProperty, input);

        #endregion

        #region ContentTemplate

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached("ContentTemplate", typeof(DataTemplate), typeof(XGridSplitter), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetContentTemplate(GridSplitter i) => (DataTemplate)i.GetValue(ContentTemplateProperty);
        public static void SetContentTemplate(GridSplitter i, DataTemplate input) => i.SetValue(ContentTemplateProperty, input);

        #endregion
    }
}