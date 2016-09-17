using Imagin.Common.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System;

namespace Imagin.Controls.Extended
{
    public partial class GradientEditor : UserControl
    {
        #region Properties

        Random Random = new Random();

        bool GradientChangeHandled = false;

        public LinearGradientBrush DefaultGradient
        {
            get
            {
                var GradientStops = new GradientStopCollection();
                GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                GradientStops.Add(new GradientStop(Colors.Black, 1.0));
                return new LinearGradientBrush(GradientStops, new Point(0.0, 0.5), new Point(1.0, 0.5));
            }
        }

        public static DependencyProperty PreviewBorderThicknessProperty = DependencyProperty.Register("PreviewBorderThickness", typeof(Thickness), typeof(GradientEditor), new PropertyMetadata(default(Thickness)));
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

        public static DependencyProperty PreviewBorderBrushProperty = DependencyProperty.Register("PreviewBorderBrush", typeof(Brush), typeof(GradientEditor), new PropertyMetadata(Brushes.Black));
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

        public static DependencyProperty GradientProperty = DependencyProperty.Register("Gradient", typeof(LinearGradientBrush), typeof(GradientEditor), new PropertyMetadata(default(LinearGradientBrush), OnGradientChanged));
        public LinearGradientBrush Gradient
        {
            get
            {
                return (LinearGradientBrush)GetValue(GradientProperty);
            }
            set
            {
                SetValue(GradientProperty, value);
            }
        }
        static void OnGradientChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientEditor)Object).OnGradientChanged((LinearGradientBrush)e.NewValue);
        }

        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(GradientEditor), new PropertyMetadata(Colors.Transparent, OnSelectedColorChanged));
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
            ((GradientEditor)Object).OnSelectedColorChanged((Color)e.NewValue);
        }

        public static DependencyProperty SelectedBandProperty = DependencyProperty.Register("SelectedBand", typeof(int), typeof(GradientEditor), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedBandChanged)));
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
            ((GradientEditor)Object).OnSelectedBandChanged((int)e.NewValue);
        }

        public static DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(GradientEditor), new PropertyMetadata(default(Point)));
        public Point StartPoint
        {
            get
            {
                return (Point)GetValue(StartPointProperty);
            }
            set
            {
                SetValue(StartPointProperty, value);
            }
        }

        public static DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(GradientEditor), new PropertyMetadata(default(Point)));
        public Point EndPoint
        {
            get
            {
                return (Point)GetValue(EndPointProperty);
            }
            set
            {
                SetValue(EndPointProperty, value);
            }
        }

        public static DependencyProperty BandsProperty = DependencyProperty.Register("Bands", typeof(int), typeof(GradientEditor), new PropertyMetadata(2, new PropertyChangedCallback(OnBandsChanged)));
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
            ((GradientEditor)Object).OnBandsChanged((int)e.NewValue);
        }

        public static DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(GradientEditor), new PropertyMetadata(0.0, OnOffsetChanged));
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
            ((GradientEditor)Object).OnOffsetChanged((double)e.NewValue);
        }

        #endregion

        #region GradientEditor

        public GradientEditor()
        {
            this.Gradient = this.DefaultGradient;

            this.StartPoint = new Point(0.0, 0.5);
            this.EndPoint = new Point(1.0, 0.5);

            this.PreviewBorderThickness = new Thickness(1.0);

            InitializeComponent();

            this.SelectedBand = 1;
        }

        #endregion

        #region Methods

        void OnBandsChanged(int Bands)
        {
            this.GradientChangeHandled = true;
            if (this.Bands > this.Gradient.GradientStops.Count)
            {
                //Number of bands to add
                var ToAdd = this.Bands - this.Gradient.GradientStops.Count;
                for (int i = 0; i < ToAdd; i++)
                {
                    this.Gradient.GradientStops.Add(new GradientStop(Color.FromRgb(Random.Next(0, 255).ToByte(), Random.Next(0, 255).ToByte(), Random.Next(0, 255).ToByte()), 1.0));
                    //First figure out i, then figure out 
                    var j = this.Gradient.GradientStops.Count - 2;
                    if (j < 0) continue;
                    var LastOffset = this.Gradient.GradientStops[j].Offset;
                    this.Gradient.GradientStops[j].Offset -= LastOffset;
                }
            }
            else if (this.Bands < this.Gradient.GradientStops.Count)
            {
                for (int i = this.Gradient.GradientStops.Count - 1; i >= this.Bands; i--)
                    this.Gradient.GradientStops.Remove(this.Gradient.GradientStops[i]);
                if (this.SelectedBand > this.Gradient.GradientStops.Count)
                    this.SelectedBand = this.Gradient.GradientStops.Count;
            }
            this.GradientChangeHandled = true;
        }

        void OnGradientChanged(LinearGradientBrush LinearGradientBrush)
        {
            if (LinearGradientBrush == null || this.GradientChangeHandled) return;
            this.Bands = LinearGradientBrush.GradientStops.Count;
            this.StartPoint = new Point(LinearGradientBrush.StartPoint.X, LinearGradientBrush.StartPoint.Y);
            this.EndPoint = new Point(LinearGradientBrush.EndPoint.X, LinearGradientBrush.EndPoint.Y);
            this.SelectedBand = -1;
            this.SelectedBand = 1;
            this.GradientChangeHandled = true;
        }

        void OnOffsetChanged(double Offset)
        {
            this.GradientChangeHandled = true;
            this.Gradient.GradientStops[this.SelectedBand - 1].Offset = Offset;
            this.GradientChangeHandled = false;
        }

        void OnSelectedBandChanged(int SelectedBand)
        {
            if (SelectedBand == -1) return;
            if (this.Gradient == null) return;
            var g = this.Gradient.GradientStops[SelectedBand - 1];
            this.SelectedColor = g.Color;
            this.Offset = g.Offset;
        }

        void OnSelectedBandChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SelectedBand = e.NewValue.As<int>();
        }

        void OnSelectedColorChanged(Color SelectedColor)
        {
            if (this.Gradient == null) return;
            this.GradientChangeHandled = true;
            this.Gradient.GradientStops[SelectedBand - 1].Color = SelectedColor;
            this.GradientChangeHandled = false;
        }

        void OnPointChanged(object sender, TextChangedEventArgs e)
        {
            this.GradientChangeHandled = true;
            this.Gradient.StartPoint = this.StartPoint;
            this.Gradient.EndPoint = this.EndPoint;
            this.GradientChangeHandled = false;
        }

        #endregion
    }
}
