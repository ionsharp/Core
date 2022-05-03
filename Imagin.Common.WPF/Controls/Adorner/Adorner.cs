using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class Adorner<T> : Adorner where T : UIElement
    {
        protected readonly VisualCollection Children;

        protected override int VisualChildrenCount => Children.Count;

        protected T Element => (T)AdornedElement;

        public Adorner(T element) : base(element)
        {
            Children = new(this);
        }

        protected override Visual GetVisualChild(int index) => Children[index];
    }
}