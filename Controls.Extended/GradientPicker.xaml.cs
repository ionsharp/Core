using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class GradientPicker : UserControl, IBrushPicker<Brush>
    {
        #region Properties

        bool AngleChangeHandled = false;

        bool GradientChangeHandled = false;

        GradientStopCollection GradientStops
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

        Random Random = new Random();

        public event EventHandler<EventArgs<Brush>> GradientChanged;

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

        public static LinearGradientBrush DefaultLinearGradient
        {
            get
            {
                return new LinearGradientBrush(DefaultGradientStops, new Point(0.0, 0.5), new Point(1.0, 0.5));
            }
        }

        public static RadialGradientBrush DefaultRadialGradient
        {
            get
            {
                return new RadialGradientBrush(DefaultGradientStops);
            }
        }

        public static Type[] Supported = new Type[]
        {
            typeof(LinearGradientBrush),
            typeof(RadialGradientBrush)
        };

        public static DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(GradientPicker), new PropertyMetadata(0d, OnAngleChanged));
        public double Angle
        {
            get
            {
                return (double)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }
        static void OnAngleChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnAngleChanged((double)e.NewValue);
        }

        public static DependencyProperty AngularUnitProperty = DependencyProperty.Register("AngularUnit", typeof(AngularUnit), typeof(GradientPicker), new PropertyMetadata(AngularUnit.Degree));
        public AngularUnit AngularUnit
        {
            get
            {
                return (AngularUnit)GetValue(AngularUnitProperty);
            }
            set
            {
                SetValue(AngularUnitProperty, value);
            }
        }
        
        public static DependencyProperty BandsProperty = DependencyProperty.Register("Bands", typeof(int), typeof(GradientPicker), new PropertyMetadata(2, new PropertyChangedCallback(OnBandsChanged)));
        public int Bands
        {
            get
            {
                return (int)GetValue(BandsProperty);
            }
            set
            {
                SetValue(BandsProperty, value);
            }
        }
        static void OnBandsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnBandsChanged((int)e.NewValue);
        }

        public static DependencyProperty GradientProperty = DependencyProperty.Register("Gradient", typeof(Brush), typeof(GradientPicker), new PropertyMetadata(default(Brush), OnGradientChanged, new CoerceValueCallback(OnGradientCoerced)));
        public Brush Gradient
        {
            get
            {
                return (Brush)GetValue(GradientProperty);
            }
            set
            {
                SetValue(GradientProperty, value);
            }
        }
        static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)d).OnGradientChanged((Brush)e.NewValue);
        }
        static object OnGradientCoerced(DependencyObject d, object value)
        {
            return ((GradientPicker)d).OnGradientCoerced(value);
        }
        
        public static DependencyProperty GradientPositionProperty = DependencyProperty.Register("GradientPosition", typeof(GradientPosition), typeof(GradientPicker), new PropertyMetadata(GradientPosition.Start, OnGradientPositionChanged));
        public GradientPosition GradientPosition
        {
            get
            {
                return (GradientPosition)GetValue(GradientPositionProperty);
            }
            set
            {
                SetValue(GradientPositionProperty, value);
            }
        }
        static void OnGradientPositionChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnGradientPositionChanged((GradientPosition)e.NewValue);
        }
        
        public static DependencyProperty GradientTypeProperty = DependencyProperty.Register("GradientType", typeof(GradientType), typeof(GradientPicker), new PropertyMetadata(GradientType.Linear, OnGradientTypeChanged));
        public GradientType GradientType
        {
            get
            {
                return (GradientType)GetValue(GradientTypeProperty);
            }
            set
            {
                SetValue(GradientTypeProperty, value);
            }
        }
        static void OnGradientTypeChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnGradientTypeChanged((GradientType)e.NewValue);
        }

        public static DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(GradientPicker), new PropertyMetadata(0.0, OnOffsetChanged));
        public double Offset
        {
            get
            {
                return (double)GetValue(OffsetProperty);
            }
            internal set
            {
                SetValue(OffsetProperty, value);
            }
        }
        static void OnOffsetChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnOffsetChanged((double)e.NewValue);
        }

        public static DependencyProperty PreviewBorderBrushProperty = DependencyProperty.Register("PreviewBorderBrush", typeof(Brush), typeof(GradientPicker), new PropertyMetadata(Brushes.Black));
        public Brush PreviewBorderBrush
        {
            get
            {
                return (Brush)GetValue(PreviewBorderBrushProperty);
            }
            set
            {
                SetValue(PreviewBorderBrushProperty, value);
            }
        }

        public static DependencyProperty PreviewBorderThicknessProperty = DependencyProperty.Register("PreviewBorderThickness", typeof(Thickness), typeof(GradientPicker), new PropertyMetadata(default(Thickness)));
        public Thickness PreviewBorderThickness
        {
            get
            {
                return (Thickness)GetValue(PreviewBorderThicknessProperty);
            }
            set
            {
                SetValue(PreviewBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty RadiansProperty = DependencyProperty.Register("Radians", typeof(double), typeof(GradientPicker), new PropertyMetadata(0d));
        public double Radians
        {
            get
            {
                return (double)GetValue(RadiansProperty);
            }
            set
            {
                SetValue(RadiansProperty, value);
            }
        }

        public static DependencyProperty SelectedBandProperty = DependencyProperty.Register("SelectedBand", typeof(int), typeof(GradientPicker), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedBandChanged)));
        public int SelectedBand
        {
            get
            {
                return (int)GetValue(SelectedBandProperty);
            }
            internal set
            {
                SetValue(SelectedBandProperty, value);
            }
        }
        static void OnSelectedBandChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnSelectedBandChanged((int)e.NewValue);
        }

        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(GradientPicker), new PropertyMetadata(Colors.Transparent, OnSelectedColorChanged));
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            internal set
            {
                SetValue(SelectedColorProperty, value);
            }
        }
        static void OnSelectedColorChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientPicker)Object).OnSelectedColorChanged((Color)e.NewValue);
        }

        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register("ShowPreview", typeof(bool), typeof(GradientPicker), new PropertyMetadata(true));
        public bool ShowPreview
        {
            get
            {
                return (bool)GetValue(ShowPreviewProperty);
            }
            internal set
            {
                SetValue(ShowPreviewProperty, value);
            }
        }

        #endregion

        #region GradientPicker

        public GradientPicker()
        {
            InitializeComponent();

            Gradient = DefaultLinearGradient;
            PreviewBorderThickness = new Thickness(1.0);
            SelectedBand = 1;
        }

        #endregion

        #region Methods

        void Shift(bool ShiftAll)
        {
            var GradientStops = this.GradientStops;

            var Count = GradientStops.Count;

            for (var k = 0; k < (ShiftAll ? Count : Count - 1); k++)
                GradientStops[k].Offset = k == 0 ? 0 : GradientStops[k - 1].Offset + 1d.Divide(GradientStops.Count.ToDouble() - 1d).Round(2);
        }

        protected virtual void OnAngleChanged(double Value)
        {
            if (!AngleChangeHandled)
            {
                var Angle = Value;

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

        protected virtual void OnBandsChanged(int Value)
        {
            var GradientStops = this.GradientStops;

            if (Value > GradientStops.Count)
            {
                //Number of bands to add
                Value -= GradientStops.Count;

                for (var i = 0; i < Value; i++)
                {
                    var RandomColor = Color.FromRgb(Random.Next(0, 255).ToByte(), Random.Next(0, 255).ToByte(), Random.Next(0, 255).ToByte());

                    //Add new band with random color
                    GradientStops.Add(new GradientStop(RandomColor, 1d));

                    //If next to last band index is valid
                    if (GradientStops.Count  > 1)
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

        protected virtual void OnGradientChanged(Brush Value)
        {
            if (!GradientChangeHandled)
            {
                Bands = GradientStops.Count;

                SelectedBand = -1;
                SelectedBand = 1;

                OnGradientChanged(new EventArgs<Brush>(Gradient));
            }
        }

        protected virtual void OnGradientChanged(EventArgs<Brush> e)
        {
            if (GradientChanged != null)
                GradientChanged(this, e);
        }

        protected virtual object OnGradientCoerced(object Value)
        {
            if
            (
                Value == null ||
                !Value.IsAny(Supported) ||
                (Value is LinearGradientBrush && GradientType != GradientType.Linear) ||
                (Value is RadialGradientBrush && GradientType != GradientType.Radial)
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

            return Value;
        }

        protected virtual void OnGradientPositionChanged(GradientPosition Value)
        {
            var Point = default(Point);

            switch (GradientType)
            {
                case GradientType.Linear:
                    switch (Value)
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
                    switch (Value)
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

            AngleChangeHandled = true;
            var x = Math.Acos(Point.X);
            Angle = x / Math.PI / (1d / 180d);
            AngleChangeHandled = false;

            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        protected virtual void OnGradientTypeChanged(GradientType Value)
        {
            Gradient = null;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        protected virtual void OnOffsetChanged(double Offset)
        {
            GradientStops[SelectedBand - 1].Offset = Offset;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        protected virtual void OnSelectedBandChanged(int Value)
        {
            if (Value > -1)
            {
                var GradientStop = GradientStops[Value - 1];

                SelectedColor = GradientStop.Color;
                Offset = GradientStop.Offset;
            }
        }

        protected virtual void OnSelectedBandChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SelectedBand = e.NewValue.As<int>();
        }

        protected virtual void OnSelectedColorChanged(Color Value)
        {
            GradientStops[SelectedBand - 1].Color = Value;
            OnGradientChanged(new EventArgs<Brush>(Gradient));
        }

        #endregion
    }
}
