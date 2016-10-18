using System.Windows;

namespace Imagin.Controls.Common
{
    public class TreeViewTemplateColumn : TreeViewColumn
    {
        public static DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(DataTemplate), typeof(TreeViewTemplateColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate Template
        {
            get
            {
                return (DataTemplate)GetValue(TemplateProperty);
            }
            set
            {
                SetValue(TemplateProperty, value);
            }
        }

        public TreeViewTemplateColumn() : base()
        {
        }
    }
}
