using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class TreeViewHyperlinkColumn : TreeViewBindingColumn
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TreeViewHyperlinkColumn), new FrameworkPropertyMetadata(null));
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(TreeViewHyperlinkColumn), new FrameworkPropertyMetadata(null));
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TreeViewHyperlinkColumn), new FrameworkPropertyMetadata(null));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TreeViewHyperlinkColumn() : base() { }
    }
}