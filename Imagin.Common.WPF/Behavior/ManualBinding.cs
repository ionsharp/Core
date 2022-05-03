using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class ManualBindingBehavior : Behavior<DependencyObject>
    {
        public DependencyProperty DataProperty { get; set; }

        public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static readonly DependencyProperty ConverterParameterProperty = DependencyProperty.Register(nameof(ConverterParameter), typeof(object), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public object ConverterParameter
        {
            get => GetValue(ConverterParameterProperty);
            set => SetValue(ConverterParameterProperty, value);
        }

        public static readonly DependencyProperty ConverterSelectorProperty = DependencyProperty.Register(nameof(ConverterSelector), typeof(ConverterSelector), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public ConverterSelector ConverterSelector
        {
            get => (ConverterSelector)GetValue(ConverterSelectorProperty);
            set => SetValue(ConverterSelectorProperty, value);
        }

        public static readonly DependencyProperty ConverterSelectorKeyProperty = DependencyProperty.Register(nameof(ConverterSelectorKey), typeof(object), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnConverterSelectorKeyChanged));
        public object ConverterSelectorKey
        {
            get => (object)GetValue(ConverterSelectorKeyProperty);
            set => SetValue(ConverterSelectorKeyProperty, value);
        }
        static void OnConverterSelectorKeyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ManualBindingBehavior>().OnConverterSelectorKeyChanged(e);

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BindingMode), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(BindingMode.OneWay, OnPropertyChanged));
        public BindingMode Mode
        {
            get => (BindingMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(PropertyPath), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public PropertyPath Path
        {
            get => (PropertyPath)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(DependencyProperty), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public DependencyProperty Property
        {
            get => (DependencyProperty)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        public static readonly DependencyProperty UpdateSourceTriggerProperty = DependencyProperty.Register(nameof(UpdateSourceTrigger), typeof(UpdateSourceTrigger), typeof(ManualBindingBehavior), new FrameworkPropertyMetadata(UpdateSourceTrigger.Default, OnPropertyChanged));
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => (UpdateSourceTrigger)GetValue(UpdateSourceTriggerProperty);
            set => SetValue(UpdateSourceTriggerProperty, value);
        }

        static void OnPropertyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ManualBindingBehavior>().OnPropertyChanged();

        protected override void OnAttached() => OnPropertyChanged();
        
        protected virtual void OnConverterSelectorKeyChanged(Value<object> input)
        {
            SetCurrentValue(ConverterProperty, ConverterSelector?.Select(input.New));
        }

        protected virtual void OnPropertyChanged()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Unbind(Property);
                AssociatedObject.Bind(Property, new Binding()
                {
                    Converter
                        = Converter,
                    ConverterParameter
                        = ConverterParameter,
                    Mode
                        = Mode,
                    Path
                        = Path,
                    Source
                        = Source,
                    StringFormat
                        = StringFormat,
                    UpdateSourceTrigger
                        = UpdateSourceTrigger
                });
            }
        }
    }
}