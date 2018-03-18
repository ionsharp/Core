using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AlphaGlider : ComponentGliderBase
    {
        Grid _Content
        {
            get => Content as Grid;
        }

        /// <summary>
        /// 
        /// </summary>
        public AlphaGlider() : base() => InitializeComponent();

        LinearGradientBrush GetBackground(Color color, Orientation orientation)
        {
            var epoint = default(Point);
            var spoint = new Point(0, 0);

            switch (orientation)
            {
                case Orientation.Horizontal:
                    epoint = new Point(1.0, 0);
                    break;
                case Orientation.Vertical:
                    epoint = new Point(0, 1.0);
                    break;
            }

            var result = new LinearGradientBrush()
            {
                EndPoint = epoint,
                StartPoint = spoint
            };
            result.GradientStops.Add(new GradientStop(Color.FromArgb(255, color.R, color.G, color.B), 0.0));
            result.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1.0));

            return result;
        }

        void UpdateColor(Point position)
        {
            var alpha = 0.0;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    alpha = position.X / ActualWidth;
                    break;
                case Orientation.Vertical:
                    alpha = position.Y / ActualHeight;
                    break;
            }

            alpha = (255.0 - (255.0 * alpha).Round()).Coerce(255.0);
            var color = Color.FromArgb(alpha.ToByte(), Color.R, Color.G, Color.B);

            ColorChangeHandled = true;
            SetCurrentValue(ColorProperty, color);
            ColorChangeHandled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Draw() => _Content.Background = GetBackground(Color, Orientation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected override void Update(Point position)
        {
            base.Update(position);
            UpdateColor(position);
        }
    }
}
