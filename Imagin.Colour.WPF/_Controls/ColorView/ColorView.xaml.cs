using Imagin.Colour.Controls.Models;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ColorView : Common.UserControl
    {
        byte? LastAlpha;

        ColorModel _colorModel;

        /// <summary>
        /// 
        /// </summary>
        public Imagin.Colour.Controls.Collections.ComponentCollection Components => _colorModel.Components;

        Imagin.Colour.Conversion.ColorConverter _converter = new Imagin.Colour.Conversion.ColorConverter();
        /// <summary>
        /// 
        /// </summary>
        public Imagin.Colour.Conversion.ColorConverter Converter => _converter;

        int columns;
        /// <summary>
        /// 
        /// </summary>
        public int Columns
        {
            get => columns;
            private set => Property.Set(this, ref columns, value, () => Columns);
        }

        int rows;
        /// <summary>
        /// 
        /// </summary>
        public int Rows
        {
            get => rows;
            private set => Property.Set(this, ref rows, value, () => Rows);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register(nameof(CheckerBackground), typeof(Color), typeof(ColorView), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Color CheckerBackground
        {
            get => (Color)GetValue(CheckerBackgroundProperty);
            set => SetValue(CheckerBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register(nameof(CheckerForeground), typeof(Color), typeof(ColorView), new FrameworkPropertyMetadata(Colors.LightGray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Color CheckerForeground
        {
            get => (Color)GetValue(CheckerForegroundProperty);
            set => SetValue(CheckerForegroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorView), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.As<ColorView>().OnColorChanged((Color)e.OldValue, (Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorModelProperty = DependencyProperty.Register(nameof(ColorModel), typeof(ColorModels), typeof(ColorView), new FrameworkPropertyMetadata(ColorModels.HSB, OnColorModelChanged, OnColorModelCoerced));
        /// <summary>
        /// 
        /// </summary>
        public ColorModels ColorModel
        {
            get => (ColorModels)GetValue(ColorModelProperty);
            set => SetValue(ColorModelProperty, value);
        }

        static void OnColorModelChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) 
            => element.As<ColorView>().OnColorModelChanged((ColorModels)e.NewValue);

        static object OnColorModelCoerced(DependencyObject element, object value) 
            => element.As<ColorView>().OnColorModelCoerced(value);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ColorView), new FrameworkPropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.As<ColorView>().OnOrientationChanged((Orientation)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TransparencyProperty = DependencyProperty.Register(nameof(Transparency), typeof(Transparency), typeof(ColorView), new FrameworkPropertyMetadata(Transparency.Opaque, OnTransparencyChanged));
        /// <summary>
        /// 
        /// </summary>
        public Transparency Transparency
        {
            get => (Transparency)GetValue(TransparencyProperty);
            set => SetValue(TransparencyProperty, value);
        }
        static void OnTransparencyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.As<ColorView>().OnTransparencyChanged((Transparency)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ColorView() => InitializeComponent();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnColorChanged(Color OldValue, Color NewValue) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnColorModelChanged(ColorModels value)
        {
            SetColumnsRows();

            _colorModel = Models.ColorModel.New(value);
            foreach (var i in _colorModel.Components)
                i.Converter = Converter;

            Property.Raise(this, () => Components);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ColorModels OnColorModelCoerced(object value)
        {
            if (value is ColorModels)
            {
                switch ((ColorModels)value)
                {
                    case ColorModels.CMY:
                    case ColorModels.CMYK:
                    case ColorModels.RG:
                        throw new NotSupportedException();
                }
                return (ColorModels)value;
            }
            throw new InvalidEnumArgumentException(nameof(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnOrientationChanged(Orientation value) => SetColumnsRows();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnTransparencyChanged(Transparency value)
        {
            SetColumnsRows();

            switch (value)
            {
                case Transparency.Opaque:
                    LastAlpha = Color.A;
                    SetCurrentValue(ColorProperty, Color.FromArgb(255, Color.R, Color.G, Color.B));
                    break;
                case Transparency.Transparent:
                    SetCurrentValue(ColorProperty, Color.FromArgb(LastAlpha ?? 255, Color.R, Color.G, Color.B));
                    break;
            }
        }

        void SetColumnsRows()
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    Columns = 0;
                    Rows = 3;
                    break;
                case Orientation.Vertical:
                    Columns = 3;
                    Rows = 0;
                    break;
            }
        }

        ICommand setColorSpaceCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand SetColorSpaceCommand
        {
            get
            {
                setColorSpaceCommand = setColorSpaceCommand ?? new RelayCommand<object>(p => SetCurrentValue(ColorModelProperty, p), p => p is ColorModels);
                return setColorSpaceCommand;
            }
        }

        ICommand setOrientationCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand SetOrientationCommand
        {
            get
            {
                setOrientationCommand = setOrientationCommand ?? new RelayCommand<object>(p => SetCurrentValue(OrientationProperty, p), p => p is Orientation);
                return setOrientationCommand;
            }
        }
    }
}
