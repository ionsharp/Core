using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeViewTextColumn : TreeViewColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ConverterProperty = DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(IValueConverter), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MemberPathProperty = DependencyProperty.Register("MemberPath", typeof(string), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        public TreeViewTextColumn() : base()
        {
        }
    }
}
