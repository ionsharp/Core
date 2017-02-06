using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class ClippedBorder : Border
    {
        public ClippedBorder() : base()
        {
            var e = new Border()
            {
                Background = Brushes.Black,
                SnapsToDevicePixels = true,
            };
            e.SetBinding(Border.CornerRadiusProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("CornerRadius"),
                Source = this
            });
            e.SetBinding(Border.HeightProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("ActualHeight"),
                Source = this
            });
            e.SetBinding(Border.WidthProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("ActualWidth"),
                Source = this
            });

            OpacityMask = new VisualBrush(e);
        }
    }
}
