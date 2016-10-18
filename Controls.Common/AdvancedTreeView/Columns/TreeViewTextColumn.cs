using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    public class TreeViewTextColumn : TreeViewColumn
    {
        public static DependencyProperty ConverterProperty = DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(IValueConverter), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public IValueConverter Converter
        {
            get
            {
                return (IValueConverter)GetValue(ConverterProperty);
            }
            set
            {
                SetValue(ConverterProperty, value);
            }
        }

        public static DependencyProperty MemberPathProperty = DependencyProperty.Register("MemberPath", typeof(string), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));
        public string MemberPath
        {
            get
            {
                return (string)GetValue(MemberPathProperty);
            }
            set
            {
                SetValue(MemberPathProperty, value);
            }
        }

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TextTrimming TextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(TextTrimmingProperty);
            }
            set
            {
                SetValue(TextTrimmingProperty, value);
            }
        }
        
        public TreeViewTextColumn() : base()
        {
        }
    }
}
