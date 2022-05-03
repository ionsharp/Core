using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.Common.Controls
{
    [ContentProperty(nameof(Templates))]
    public class MemberTemplateSelector : DataTemplateSelector
    {
        public KeyTemplateCollection Templates { get; set; } = new();
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MemberModel model)
            {
                var propertyGrid = container?.FindParent<PropertyGrid>();
                if (propertyGrid != null)
                {
                    foreach (var i in propertyGrid.OverrideTemplates)
                    {
                        if (i.DataKey?.Equals(model.Name) == true || i.DataKey?.Equals(model.Type) == true)
                            return i;
                    }
                }
                foreach (var i in Templates)
                {
                    var checkType = model.TemplateType ?? MemberModel.GetTemplateType(model.Type);
                    if (i.DataKey?.Equals(checkType) == true)
                        return i;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}