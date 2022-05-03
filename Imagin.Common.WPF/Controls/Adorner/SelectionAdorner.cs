using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class SelectionAdorner : Adorner<FrameworkElement>
    {
        readonly SelectionBorder Border;

        readonly Canvas Canvas;

        public SelectionAdorner(FrameworkElement element) : base(element)
        {
            Canvas = new()
            {
                IsHitTestVisible = false
            };

            Border = new();
            Border.Bind
                (Canvas.LeftProperty, 
                new PropertyPath($"(0).{nameof(DoubleRegion.X)}", 
                XElement.SelectionProperty), element);
            Border.Bind
                (Canvas.TopProperty,
                new PropertyPath($"(0).{nameof(DoubleRegion.Y)}", 
                XElement.SelectionProperty), element);
            Border.Bind
                (SelectionBorder.HeightProperty,
                new PropertyPath($"(0).{nameof(DoubleRegion.Height)}", 
                XElement.SelectionProperty), element);
            Border.Bind
                (SelectionBorder.WidthProperty,
                new PropertyPath($"(0).{nameof(DoubleRegion.Width)}", 
                XElement.SelectionProperty), element);

            Canvas
                .Children
                .Add(Border);
            Children
                .Add(Canvas);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Canvas?.Arrange(new Rect(new Point(0, 0), Element.ActualSize()));
            return finalSize;
        }
    }
}