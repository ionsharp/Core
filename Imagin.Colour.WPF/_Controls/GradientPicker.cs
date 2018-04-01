using Imagin.Common.Geometry;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class GradientPicker : Control, IBrushPicker<Brush>
    {
        #region Properties

        bool _AngleChangeHandled = false;

        bool _GradientChangeHandled = false;

        GradientStopCollection _GradientStops
        {
            get
            {
                switch (GradientType)
                {
                    case GradientType.Linear:
                        return Gradient.As<LinearGradientBrush>().GradientStops;
                    case GradientType.Radial:
                        return Gradient.As<RadialGradientBrush>().GradientStops;
                }

                return default(GradientStopCollection);
            }
        }

        Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Brush>> GradientChanged;

        /// <summary>
        /// 
        /// </summary>
        public static GradientStopCollection DefaultGradientStops
        {
            get
            {
                var GradientStops = new GradientStopCollection();
                GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                GradientStops.Add(new GradientStop(Colors.Black, 1.0));
                return GradientStops;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static LinearGradientBrush DefaultLinearGradient
        {
            get
            {
                return new LinearGradientBrush(DefaultGradientStops, new Point(0.0, 0.5), new Point(1.0, 0.5));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static RadialGradientBrush DefaultRadialGradient
        {
            get
            {
                return new RadialGradientBrush(DefaultGradientStops);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Type[] Supported = new Type[]
        {
            typeof(LinearGradientBrush),
            typeof(RadialGradientBrush)
        };

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(GradientPicker), new PropertyMetadata(0d, OnAngleChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        static void OnAngleChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnAngleChanged((double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AngularUnitProperty = DependencyProperty.Register(nameof(AngularUnit), typeof(AngularUnit), typeof(GradientPicker), new PropertyMetadata(AngularUnit.Degree));
        /// <summary>
        /// 
        /// </summary>
        public AngularUnit AngularUnit
        {
            get => (AngularUnit)GetValue(AngularUnitProperty);
            set => SetValue(AngularUnitProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BandsProperty = DependencyProperty.Register(nameof(Bands), typeof(int), typeof(GradientPicker), new PropertyMetadata(2, new PropertyChangedCallback(OnBandsChanged)));
        /// <summary>
        /// 
        /// </summary>
        public int Bands
        {
            get => (int)GetValue(BandsProperty);
            set => SetValue(BandsProperty, value);
        }

        static void OnBandsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnBandsChanged((int)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(Brush), typeof(GradientPicker), new PropertyMetadata(default(Brush), OnGradientChanged, new CoerceValueCallback(OnGradientCoerced)));
        /// <summary>
        /// 
        /// </summary>
        public Brush Gradient
        {
            get => (Brush)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        static void OnGradientChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnGradientChanged((Brush)e.NewValue);
        }

        static object OnGradientCoerced(DependencyObject element, object value)
        {
            return ((GradientPicker)element).OnGradientCoerced(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GradientPositionProperty = DependencyProperty.Register(nameof(GradientPosition), typeof(GradientPosition), typeof(GradientPicker), new PropertyMetadata(GradientPosition.Start, OnGradientPositionChanged));
        /// <summary>
        /// 
        /// </summary>
        public GradientPosition GradientPosition
        {
            get => (GradientPosition)GetValue(GradientPositionProperty);
            set => SetValue(GradientPositionProperty, value);
        }
        static void OnGradientPositionChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnGradientPositionChanged((GradientPosition)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GradientTypeProperty = DependencyProperty.Register(nameof(GradientType), typeof(GradientType), typeof(GradientPicker), new PropertyMetadata(GradientType.Linear, OnGradientTypeChanged));
        /// <summary>
        /// 
        /// </summary>
        public GradientType GradientType
        {
            get => (GradientType)GetValue(GradientTypeProperty);
            set => SetValue(GradientTypeProperty, value);
        }
        static void OnGradientTypeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnGradientTypeChanged((GradientType)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(double), typeof(GradientPicker), new PropertyMetadata(0.0, OnOffsetChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            internal set => SetValue(OffsetProperty, value);
        }
        static void OnOffsetChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnOffsetChanged((double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PreviewBorderBrushProperty = DependencyProperty.Register(nameof(PreviewBorderBrush), typeof(Brush), typeof(GradientPicker), new PropertyMetadata(Brushes.Black));
        /// <summary>
        /// 
        /// </summary>
        public Brush PreviewBorderBrush
        {
            get => (Brush)GetValue(PreviewBorderBrushProperty);
            set => SetValue(PreviewBorderBrushProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PreviewBorderThicknessProperty = DependencyProperty.Register(nameof(PreviewBorderThickness), typeof(Thickness), typeof(GradientPicker), new PropertyMetadata(default(Thickness)));
        /// <summary>
        /// 
        /// </summary>
        public Thickness PreviewBorderThickness
        {
            get => (Thickness)GetValue(PreviewBorderThicknessProperty);
            set => SetValue(PreviewBorderThicknessProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty RadiansProperty = DependencyProperty.Register(nameof(Radians), typeof(double), typeof(GradientPicker), new PropertyMetadata(0d));
        /// <summary>
        /// 
        /// </summary>
        public double Radians
        {
            get => (double)GetValue(RadiansProperty);
            set => SetValue(RadiansProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedBandProperty = DependencyProperty.Register(nameof(SelectedBand), typeof(int), typeof(GradientPicker), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedBandChanged)));
        /// <summary>
        /// 
        /// </summary>
        public int SelectedBand
        {
            get => (int)GetValue(SelectedBandProperty);
            internal set => SetValue(SelectedBandProperty, value);
        }
        static void OnSelectedBandChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnSelectedBandChanged((int)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(GradientPicker), new PropertyMetadata(Colors.Transparent, OnSelectedColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            internal set => SetValue(SelectedColorProperty, value);
        }
        static void OnSelectedColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)element).OnSelectedColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register(nameof(ShowPreview), typeof(bool), typeof(GradientPicker), new PropertyMetadata(true));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowPreview
        {
            get => (bool)GetValue(ShowPreviewProperty);
            internal set => SetValue(ShowPreviewProperty, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public GradientPicker()
        {
            DefaultStyleKey = typeof(GradientPicker);
            SetCurrentValue(GradientProperty, DefaultLinearGradient);
            SetCurrentValue(PreviewBorderThicknessProperty, new Thickness(1.0));
            SetCurrentValue(SelectedBandProperty, 1);
        }

        #endregion

        #region Methods

        void Shift(bool ShiftAll)
        {
            var GradientStops = this._GradientStops;

            var Count = GradientStops.Count;

            for (var k = 0; k < (ShiftAll ? Count : Count - 1); k++)
                GradientStops[k].Offset = k == 0 ? 0 : GradientStops[k - 1].Offset + 1d.Divide(GradientStops.Count.ToDouble() - 1d).Round(2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnAngleChanged(double value)
        {
            if (!_AngleChangeHandled)
            {
                var Angle = value;

                var x = Angle * (1d / 180d) * Math.PI;
                var y = new Point(Math.Cos(x), Math.Sin(x));

                switch (GradientType)
                {
                    case GradientType.Linear:
                        var Linear = Gradient.As<LinearGradientBrush>();
                        switch (GradientPosition)
                        {
                            case GradientPosition.End:
                                Linear.EndPoint = y;
                                break;
                            case GradientPosition.Start:
                                Linear.StartPoint = y;
                                break;
                        }
                        break;
                    case GradientType.Radial:
                        var Radial = Gradient.As<RadialGradientBrush>();
                        switch (GradientPosition)
                        {
                            case GradientPosition.End:
                                Radial.Center = y;
                                break;
                            case GradientPosition.Start:
                                Radial.GradientOrigin = y;
                                break;
                        }
                        break;
                }
            }

            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnBandsChanged(int value)
        {
            var GradientStops = this._GradientStops;

            if (value > GradientStops.Count)
            {
                //Number of bands to add
                value -= GradientStops.Count;

                for (var i = 0; i < value; i++)
                {
                    var RandomColor = Color.FromRgb(_random.Next(0, 255).ToByte(), _random.Next(0, 255).ToByte(), _random.Next(0, 255).ToByte());

                    //Add new band with random color
                    GradientStops.Add(new GradientStop(RandomColor, 1d));

                    //If next to last band index is valid
                    if (GradientStops.Count > 1)
                        Shift(false);
                }
            }
            else if (Bands < GradientStops.Count)
            {
                for (var i = GradientStops.Count - 1; i >= Bands; i--)
                    GradientStops.Remove(GradientStops[i]);

                if (SelectedBand > GradientStops.Count)
                    SelectedBand = GradientStops.Count;

                Shift(true);
            }

            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnGradientChanged(Brush value)
        {
            if (!_GradientChangeHandled)
            {
                Bands = _GradientStops.Count;

                SelectedBand = -1;
                SelectedBand = 1;

                OnGradientChanged(new EventArgs<Brush>(Gradient));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGradientChanged(EventArgs<Brush> e)
            => GradientChanged?.Invoke(this, e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual object OnGradientCoerced(object value)
        {
            if
            (
                value == null ||
                !value.IsAny(Supported) ||
                (value is LinearGradientBrush && GradientType != GradientType.Linear) ||
                (value is RadialGradientBrush && GradientType != GradientType.Radial)
            )
            {
                var Result = default(Brush);

                switch (GradientType)
                {
                    case GradientType.Linear:
                        Result = DefaultLinearGradient;
                        break;
                    case GradientType.Radial:
                        Result = DefaultRadialGradient;
                        break;
                }

                return Result;
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnGradientPositionChanged(GradientPosition value)
        {
            var Point = default(Point);

            switch (GradientType)
            {
                case GradientType.Linear:
                    switch (value)
                    {
                        case GradientPosition.End:
                            Point = Gradient.As<LinearGradientBrush>().EndPoint;
                            break;
                        case GradientPosition.Start:
                            Point = Gradient.As<LinearGradientBrush>().StartPoint;
                            break;
                    }
                    break;
                case GradientType.Radial:
                    switch (value)
                    {
                        case GradientPosition.End:
                            Point = Gradient.As<RadialGradientBrush>().Center;
                            break;
                        case GradientPosition.Start:
                            Point = Gradient.As<RadialGradientBrush>().GradientOrigin;
                            break;
                    }
                    break;
            }

            _AngleChangeHandled = true;
            var x = Math.Acos(Point.X);
            Angle = x / Math.PI / (1d / 180d);
            _AngleChangeHandled = false;

            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnGradientTypeChanged(GradientType value)
        {
            Gradient = null;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        protected virtual void OnOffsetChanged(double offset)
        {
            _GradientStops[SelectedBand - 1].Offset = offset;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnSelectedBandChanged(int value)
        {
            if (value > -1)
            {
                var GradientStop = _GradientStops[value - 1];

                SelectedColor = GradientStop.Color;
                Offset = GradientStop.Offset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectedBandChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            => SelectedBand = e.NewValue.As<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnSelectedColorChanged(Color value)
        {
            _GradientStops[SelectedBand - 1].Color = value;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        #endregion
    }
}