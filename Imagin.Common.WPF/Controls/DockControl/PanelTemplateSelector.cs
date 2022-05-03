using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    public class PanelTemplateSelector : TypeTemplateSelector, IDockSelector
    {
        public sealed override bool Strict => true;

        public sealed override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var result = base.SelectTemplate(item, container);
            if (ReferenceEquals(result, Default))
            {
                var control = container.FindParent<Popup>() is Popup popup
                    ? popup.PlacementTarget.FindVisualParent<DockRootControl>()?.DockControl
                    : container.FindParent<DockRootControl>()?.DockControl;

                if (control?.DefaultTemplates is KeyTemplateCollection templates)
                {
                    foreach (var i in templates)
                    {
                        if (Check(item, i))
                            return i;
                    }
                }
                return Default;
            }
            return result;
        }
    }
}