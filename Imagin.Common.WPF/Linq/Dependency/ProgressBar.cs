using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ProgressBar))]
    public static class XProgressBar
    {
        #region Content

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(object), typeof(XProgressBar), new FrameworkPropertyMetadata(null));
        public static object GetContent(ProgressBar i) => i.GetValue(ContentProperty);
        public static void SetContent(ProgressBar i, object input) => i.SetValue(ContentProperty, input);

        #endregion

        #region ContentTemplate

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached("ContentTemplate", typeof(DataTemplate), typeof(XProgressBar), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetContentTemplate(ProgressBar i) => (DataTemplate)i.GetValue(ContentTemplateProperty);
        public static void SetContentTemplate(ProgressBar i, DataTemplate input) => i.SetValue(ContentTemplateProperty, input);

        #endregion

        #region ContentTemplateSelector

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.RegisterAttached("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(XProgressBar), new FrameworkPropertyMetadata(null));
        public static DataTemplateSelector GetContentTemplateSelector(ProgressBar i) => (DataTemplateSelector)i.GetValue(ContentTemplateSelectorProperty);
        public static void SetContentTemplateSelector(ProgressBar i, DataTemplateSelector input) => i.SetValue(ContentTemplateSelectorProperty, input);

        #endregion
    }
}