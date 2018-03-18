using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentTemplateSelector : DataTemplateSelector
    {
        ObservableCollection<ContentTemplate> _templates;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ContentTemplate> Templates => _templates;

        /// <summary>
        /// 
        /// </summary>
        public object VisibilityParameter
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ContentTemplateSelector() : base() => _templates = new ObservableCollection<ContentTemplate>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            foreach (var i in _templates)
            {
                if (i.Value.Equals(item))
                    return i;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
