using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeViewTemplateColumn : TreeViewColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(DataTemplate), typeof(TreeViewTemplateColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public TreeViewTemplateColumn() : base()
        {
        }
    }
}
