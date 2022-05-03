using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.Common.Controls
{
    [ContentProperty(nameof(Content))]
    public abstract class ContentControl<T> : Control
    {
        public static readonly DependencyProperty<T, ContentControl<T>> ContentProperty = new(nameof(Content), new FrameworkPropertyMetadata(default(T), OnContentChanged));
        public T Content
        {
            get => ContentProperty.Get(this);
            set => ContentProperty.Set(this, value);
        }
        static void OnContentChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ContentControl<T>>().OnContentChanged(e);

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl<T>), new FrameworkPropertyMetadata(null, OnContentTemplateChanged));
        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }
        static void OnContentTemplateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<ContentControl<T>>().OnContentTemplateChanged(e);

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(ContentControl<T>), new FrameworkPropertyMetadata(null, OnContentTemplateSelectorChanged));
        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }
        static void OnContentTemplateSelectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<ContentControl<T>>().OnContentTemplateSelectorChanged(e);

        public ContentControl() : base() { }

        protected virtual void OnContentChanged(Value<T> input) { }

        protected virtual void OnContentTemplateChanged(Value<DataTemplate> input) { }

        protected virtual void OnContentTemplateSelectorChanged(Value<DataTemplateSelector> input) { }
    }
}