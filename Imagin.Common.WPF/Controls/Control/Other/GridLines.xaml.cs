using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class GridLines : Control
    {
        public static readonly DependencyProperty PenProperty = DependencyProperty.Register(nameof(Pen), typeof(Pen), typeof(GridLines), new FrameworkPropertyMetadata(null));
        public Pen Pen
        {
            get => (Pen)GetValue(PenProperty);
            set => SetValue(PenProperty, value);
        }

        public GridLines() => InitializeComponent();
    }
}