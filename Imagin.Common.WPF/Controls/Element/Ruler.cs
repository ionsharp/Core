using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <remarks>
    /// Rewritten by: 
    /// Sebestyen Murancsik
    /// 
    /// Contributions from:
    /// <see cref="http://www.orbifold.net/default/?p=2295&amp;cpage=1#comment-61500">Raf Lenfers</see>
    /// <seealso>http://visualizationtools.net/default/wpf-ruler/</seealso>
    /// </remarks>
    public class Ruler : FrameworkElement
    {
        #region TickPlacements

        public enum TickPlacements
        {
            Top,
            Bottom
        }

        #endregion

        #region Fields

        readonly double segmentHeight;

        readonly Pen pen = new(Brushes.Black, 1.0);

        readonly Pen redPen = new(Brushes.Red, 1.0);

        #endregion

        #region Properties

        #region AutoSize

        public static readonly DependencyProperty AutoSizeProperty = DependencyProperty.Register("AutoSize", typeof(bool), typeof(Ruler), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Gets or sets <see cref="AutoSize"/>.
        /// 
        /// <see langword="false"/> (default): The length of the ruler results from the <see cref="Length"/> property. 
        /// If the window size has changed (e.g., it is wider than the ruler's length), free space is shown at 
        /// the end of the ruler; no rescaling is done. 
        /// 
        /// <see langword="true"/>: The length of the ruler is always adjusted to its actual width. This ensures that 
        /// the ruler is shown for the actual width of the window.
        /// </summary>
        public bool AutoSize
        {
            get => (bool)GetValue(AutoSizeProperty);
            set
            {
                SetValue(AutoSizeProperty, value);
                InvalidateVisual();
            }
        }

        #endregion

        #region Background

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        #endregion

        #region BorderBrush

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        #endregion

        #region BorderThickness

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(int), typeof(Ruler), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));
        public int BorderThickness
        {
            get => (int)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        #endregion

        #region Chip

        public static readonly DependencyProperty ChipProperty = DependencyProperty.Register("Chip", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata((double)-1000, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Sets the location of the chip in the units of the ruler.
        /// So, to set the chip to 100px units the chip needs to be set to 100.
        /// Use the <see cref="DipHelper"/> class for conversions.
        /// </summary>
        public double Chip
        {
            get => (double)GetValue(ChipProperty);
            set => SetValue(ChipProperty, value);
        }

        #endregion

        #region ChipColor

        public static readonly DependencyProperty ChipColorProperty = DependencyProperty.Register("ChipColor", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Sets color of chip.
        /// </summary>
        public Brush ChipColor
        {
            get => (Brush)GetValue(ChipColorProperty);
            set => SetValue(ChipColorProperty, value);
        }

        #endregion

        #region CountShift

        public static readonly DependencyProperty CountShiftProperty = DependencyProperty.Register("CountShift", typeof(int), typeof(Ruler), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// By default the counting of numbers starts at zero, this property allows you to shift
        /// the counting.
        /// </summary>
        public int CountShift
        {
            get => (int)GetValue(CountShiftProperty);
            set => SetValue(CountShiftProperty, value);
        }

        #endregion

        #region Foreground

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        #endregion

        #region Length

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata(20D, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Gets or sets the length of the ruler. If the <see cref="AutoSize"/> property is set to false (default) this
        /// is a fixed length. Otherwise the length is calculated based on the actual width of the ruler.
        /// </summary>
        public double Length
        {
            get
            {
                if (AutoSize)
                    return ActualWidth / Zoom;

                return (double)GetValue(LengthProperty);
            }
            set => SetValue(LengthProperty, value);
        }

        #endregion

        #region Resolution

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(float), typeof(Ruler), new FrameworkPropertyMetadata(72f, FrameworkPropertyMetadataOptions.AffectsRender));
        public float Resolution
        {
            get => (float)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }

        #endregion

        #region SmallStep

        public static readonly DependencyProperty SmallStepProperty = DependencyProperty.Register("SmallStep", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata((double)25.0, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Gets or sets the small step for the ruler. The default value is 25.0. 
        /// </summary>
        public double SmallStep
        {
            get => (double)GetValue(SmallStepProperty);
            set
            {
                SetValue(SmallStepProperty, value);
                InvalidateVisual();
            }
        }

        #endregion

        #region Step

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata((double)100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Gets or sets the step for the ruler. The default value is 100.0. 
        /// </summary>
        public double Step
        {
            get => (double)GetValue(StepProperty);
            set 
            {
                SetValue(StepProperty, value);
                InvalidateVisual();
            }
        }

        #endregion

        #region StringFormat

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(Ruler), new FrameworkPropertyMetadata("N2", FrameworkPropertyMetadataOptions.AffectsRender));
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        #endregion

        #region TickPlacement

        public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register("TickPlacement", typeof(TickPlacements), typeof(Ruler), new FrameworkPropertyMetadata(TickPlacements.Top, FrameworkPropertyMetadataOptions.AffectsRender));
        public TickPlacements TickPlacement
        {
            get => (TickPlacements)GetValue(TickPlacementProperty);
            set => SetValue(TickPlacementProperty, value);
        }

        #endregion

        #region TickStroke

        public static readonly DependencyProperty TickStrokeProperty = DependencyProperty.Register("TickStroke", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush TickStroke
        {
            get => (Brush)GetValue(TickStrokeProperty);
            set => SetValue(TickStrokeProperty, value);
        }

        #endregion

        #region Unit

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(GraphicUnit), typeof(Ruler), new FrameworkPropertyMetadata(GraphicUnit.Pixel, FrameworkPropertyMetadataOptions.AffectsRender));
        public GraphicUnit Unit
        {
            get => (GraphicUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        #endregion

        #region Zoom

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata((double)1.0, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// Gets or sets the zoom factor for the ruler. The default value is 1.0. 
        /// </summary>
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set
            {
                SetValue(ZoomProperty, value);
                InvalidateVisual();
            }
        }

        #endregion

        #endregion

        #region Ruler

        static Ruler()
            => HeightProperty.OverrideMetadata(typeof(Ruler), new FrameworkPropertyMetadata(25.0));

        public Ruler()
        {
            segmentHeight = Height - 10;
            SetCurrentValue(SnapsToDevicePixelsProperty, true);
        }

        #endregion

        #region Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double xEnd = Length * Zoom;

            var pen = new Pen(BorderBrush, BorderThickness);
            drawingContext.DrawRectangle(Background, pen, new Rect(new Point(0.0, 0.0), new Point(xEnd, Height)));

            pen = new Pen(ChipColor, 1.0);
            drawingContext.DrawLine(pen, new Point(Chip, 0), new Point(Chip, Height));

            for (double i = 0; i < Length; i += SmallStep)
            {
                double d = i * Zoom;

                double startHeight;
                double endHeight;

                if (TickPlacement == TickPlacements.Top)
                {
                    startHeight = 0;
                    // Main step or small step?
                    endHeight = ((i % Step == 0) ? segmentHeight : segmentHeight / 2);
                }
                else
                {
                    startHeight = Height;
                    // Main step or small step?
                    endHeight = ((i % Step == 0) ? segmentHeight : segmentHeight * 1.5);
                }

                pen = new Pen(TickStroke, 1);
                drawingContext.DrawLine(pen, new Point(d, startHeight), new Point(d, endHeight));

                if ((i != 0.0) && (i % Step == 0) && (i < Length))
                {
                    var formattedText = new FormattedText((i + CountShift).Convert(GraphicUnit.Pixel, Unit, Resolution.Double()).ToString(StringFormat, CultureInfo.CurrentCulture) + $" {Unit.GetAttribute<AbbreviationAttribute>().Abbreviation}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 6, Foreground, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                    formattedText.SetFontWeight(FontWeights.Regular);
                    formattedText.TextAlignment = TextAlignment.Center;

                    if (TickPlacement == TickPlacements.Top)
                    {
                        drawingContext.DrawText(formattedText, new Point(d, Height - formattedText.Height));
                    }
                    else drawingContext.DrawText(formattedText, new Point(d, Height - segmentHeight - formattedText.Height));
                }
            }
        }

        #endregion
    }
}