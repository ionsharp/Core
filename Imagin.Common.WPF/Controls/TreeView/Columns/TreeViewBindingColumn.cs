using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public abstract class TreeViewBindingColumn : TreeViewColumn
    {
        public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(TreeViewBindingColumn), new FrameworkPropertyMetadata(null));
        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static readonly DependencyProperty ConverterParameterProperty = DependencyProperty.Register(nameof(ConverterParameter), typeof(object), typeof(TreeViewBindingColumn), new FrameworkPropertyMetadata(null));
        public object ConverterParameter
        {
            get => GetValue(ConverterParameterProperty);
            set => SetValue(ConverterParameterProperty, value);
        }

        public static readonly DependencyProperty MemberProperty = DependencyProperty.Register(nameof(Member), typeof(string), typeof(TreeViewBindingColumn), new FrameworkPropertyMetadata(null));
        public string Member
        {
            get => (string)GetValue(MemberProperty);
            set => SetValue(MemberProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BindingMode), typeof(TreeViewBindingColumn), new FrameworkPropertyMetadata(BindingMode.OneWay));
        public BindingMode Mode
        {
            get => (BindingMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
    }
}