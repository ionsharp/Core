using Imagin.Common;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [TemplatePart(Name = nameof(PART_Content), Type = typeof(ContentPresenter))]
    [TemplatePart(Name = nameof(PART_ScrollViewer), Type = typeof(ScrollViewer))]
    public class Viewer : UserControl
    {
        #region (IMultiValueConverter) LinesVisibilityConverter

        public static readonly IMultiValueConverter LinesVisibilityConverter = new MultiConverter<Visibility>(i =>
        {
            var visibility 
                = (Visibility)i.Values?.GetValue(0);
            var threshold 
                = (double)i.Values?.GetValue(1);
            var zoom 
                = (double)i.Values?.GetValue(2);

            if (zoom > threshold)
                return Visibility.Visible;

            return Visibility.Collapsed;
        });

        #endregion

        #region Properties

        double RelativeX;

        double RelativeY;

        //...

        ContentPresenter PART_Content;

        public ScrollViewer PART_ScrollViewer { get; private set; }

        //...

        public static readonly DependencyProperty CanvasAngleProperty = DependencyProperty.Register(nameof(CanvasAngle), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.0));
        public double CanvasAngle
        {
            get => (double)GetValue(CanvasAngleProperty);
            set => SetValue(CanvasAngleProperty, value);
        }

        public static readonly DependencyProperty CompassVisibilityProperty = DependencyProperty.Register(nameof(CompassVisibility), typeof(Visibility), typeof(Viewer), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility CompassVisibility
        {
            get => (Visibility)GetValue(CompassVisibilityProperty);
            set => SetValue(CompassVisibilityProperty, value);
        }
        
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register(nameof(Resolution), typeof(float), typeof(Viewer), new FrameworkPropertyMetadata(72f));
        public float Resolution
        {
            get => (float)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }

        public static readonly DependencyProperty RulerLengthProperty = DependencyProperty.Register(nameof(RulerLength), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(30.0));
        public double RulerLength
        {
            get => (double)GetValue(RulerLengthProperty);
            set => SetValue(RulerLengthProperty, value);
        }

        public static readonly DependencyProperty RulerVisibilityProperty = DependencyProperty.Register(nameof(RulerVisibility), typeof(Visibility), typeof(Viewer), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility RulerVisibility
        {
            get => (Visibility)GetValue(RulerVisibilityProperty);
            set => SetValue(RulerVisibilityProperty, value);
        }

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(GraphicUnit), typeof(Viewer), new FrameworkPropertyMetadata(GraphicUnit.Pixel));
        public GraphicUnit Unit
        {
            get => (GraphicUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }
        
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(1.0, null, OnZoomCoerced));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }
        static object OnZoomCoerced(DependencyObject d, object Value)
            => d.As<Viewer>().OnZoomCoerced(Value);

        public static readonly DependencyProperty ZoomIncrementProperty = DependencyProperty.Register(nameof(ZoomIncrement), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.01, null, OnZoomIncrementCoerced));
        public double ZoomIncrement
        {
            get => (double)GetValue(ZoomIncrementProperty);
            set => SetValue(ZoomIncrementProperty, value);
        }
        static object OnZoomIncrementCoerced(DependencyObject d, object Value)
            => d.As<Viewer>().OnZoomIncrementCoerced(Value);

        public static readonly DependencyProperty ZoomMaximumProperty = DependencyProperty.Register(nameof(ZoomMaximum), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(50.0, null, OnZoomMaximumCoerced));
        public double ZoomMaximum
        {
            get => (double)GetValue(ZoomMaximumProperty);
            set => SetValue(ZoomMaximumProperty, value);
        }
        static object OnZoomMaximumCoerced(DependencyObject d, object Value)
            => d.As<Viewer>().OnZoomMaximumCoerced(Value);

        public static readonly DependencyProperty ZoomMinimumProperty = DependencyProperty.Register(nameof(ZoomMinimum), typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.05, null, OnZoomMinimumCoerced));
        public double ZoomMinimum
        {
            get => (double)GetValue(ZoomMinimumProperty);
            set => SetValue(ZoomMinimumProperty, value);
        }
        static object OnZoomMinimumCoerced(DependencyObject d, object Value)
            => d.As<Viewer>().OnZoomMinimumCoerced(Value);

        #endregion

        #region Viewer

        public Viewer() : base() { }

        #endregion

        #region Methods

        static double CalculateOffset(double extent, double viewPort, double scrollWidth, double relBefore)
        {
            double Offset = relBefore * extent - 0.5 * viewPort;
            //If negative due to initial values, center content
            if (Offset < 0) Offset = 0.5 * scrollWidth;
            return Offset;
        }

        //...

        void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Check if the content size has changed
            if (e.ExtentWidthChange != 0 || e.ExtentHeightChange != 0)
            {
                //Set accordingly
                PART_ScrollViewer.ScrollToHorizontalOffset
                    (CalculateOffset(e.ExtentWidth, e.ViewportWidth, PART_ScrollViewer.ScrollableWidth, RelativeX));
                PART_ScrollViewer.ScrollToVerticalOffset
                    (CalculateOffset(e.ExtentHeight, e.ViewportHeight, PART_ScrollViewer.ScrollableHeight, RelativeY));
            }
            else
            {
                //Store relative values if normal scroll
                RelativeX = (e.HorizontalOffset + 0.5 * e.ViewportWidth) / e.ExtentWidth;
                RelativeY = (e.VerticalOffset + 0.5 * e.ViewportHeight) / e.ExtentHeight;
            }
        }

        //...

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0)
                {
                    if (Zoom + ZoomIncrement <= ZoomMaximum)
                        Zoom += ZoomIncrement;
                }
                else
                {
                    if (Zoom - ZoomIncrement >= ZoomMinimum)
                        Zoom -= ZoomIncrement;
                }
            }
        }

        //...

        protected virtual object OnZoomCoerced(object Value)
        {
            return ((double)Value).Coerce(ZoomMaximum, ZoomMinimum);
        }

        protected virtual object OnZoomIncrementCoerced(object Value)
        {
            return ((double)Value).Coerce(ZoomMaximum, ZoomMinimum);
        }

        protected virtual object OnZoomMaximumCoerced(object Value)
        {
            return ((double)Value).Coerce(100.0, Zoom);
        }

        protected virtual object OnZoomMinimumCoerced(object Value)
        {
            return ((double)Value).Coerce(Zoom, 0.001);
        }

        //...

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Content = Template.FindName(nameof(PART_Content), this) as ContentPresenter;
            
            PART_ScrollViewer = Template.FindName(nameof(PART_ScrollViewer), this) as ScrollViewer;
            PART_ScrollViewer.If(i => { i.ScrollChanged += OnScrollChanged; });
        }

        public void FitScreen()
        {
            if (PART_Content == null || PART_ScrollViewer == null) 
                return;

            var w = PART_Content.ActualWidth;
            var h = PART_Content.ActualHeight;

            if (w == 0 || h == 0)
                return;

            var resulta = (PART_ScrollViewer.ActualWidth - (0.025 * PART_ScrollViewer.ActualWidth)) / w;
            var resultb = (PART_ScrollViewer.ActualHeight - (0.05 * PART_ScrollViewer.ActualHeight)) / h;

            if (resulta < resultb)
            {
                Zoom = Math.Round(resulta, 2);
            }
            else Zoom = Math.Round(resultb, 2);
        }

        public void FillScreen()
        {
            if (PART_Content == null || PART_ScrollViewer == null)
                return;

            var w = PART_Content.ActualWidth;
            var h = PART_Content.ActualHeight;

            if (w == 0 || h == 0)
                return;

            var resulta = (PART_ScrollViewer.ActualWidth - (0.025 * PART_ScrollViewer.ActualWidth)) / w;
            var resultb = (PART_ScrollViewer.ActualHeight - (0.05 * PART_ScrollViewer.ActualHeight)) / h;

            if (resulta > resultb)
            {
                Zoom = Math.Round(resulta, 2);
            }
            else Zoom = Math.Round(resultb, 2);
        }

        #endregion

        #region Commands

        [field: NonSerialized]
        ICommand backgroundColorCommand;
        [Hidden]
        public ICommand BackgroundColorCommand => backgroundColorCommand ??= new RelayCommand<object>(i =>
        {
            if (i is Color color)
            {
                Background = new SolidColorBrush(color);
                return;
            }

            var window = new Imagin.Common.Controls.ColorWindow
            {
                Value = Background
            };
            window.ShowDialog();
            if (XWindow.GetResult(window) == 0)
                SetCurrentValue(BackgroundProperty, window.Value as SolidColorBrush);
        });

        #endregion
    }
}