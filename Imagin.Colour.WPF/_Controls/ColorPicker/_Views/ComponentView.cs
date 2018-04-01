using Imagin.Colour.Controls.Models;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentView : ContentControl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ColorChanged;

        /// <summary>
        /// 
        /// </summary>
        internal bool _ColorChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        internal bool _ValueChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorSpaceModelProperty = DependencyProperty.Register(nameof(ColorSpaceModel), typeof(ColorModel), typeof(ComponentView), new FrameworkPropertyMetadata(default(ColorModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColorModel ColorSpaceModel
        {
            get => (ColorModel)GetValue(ColorSpaceModelProperty);
            set => SetValue(ColorSpaceModelProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentModelProperty = DependencyProperty.Register(nameof(ComponentModel), typeof(Component), typeof(ComponentView), new FrameworkPropertyMetadata(default(Component), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Component ComponentModel
        {
            get => (Component)GetValue(ComponentModelProperty);
            set => SetValue(ComponentModelProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(Imagin.Colour.Conversion.ColorConverter), typeof(ComponentView), new FrameworkPropertyMetadata(default(Imagin.Colour.Conversion.ColorConverter), OnConverterChanged));
        /// <summary>
        /// 
        /// </summary>
        public Imagin.Colour.Conversion.ColorConverter Converter
        {
            get => (Imagin.Colour.Conversion.ColorConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }
        static void OnConverterChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.As<ComponentView>().OnConverterChanged((Imagin.Colour.Conversion.ColorConverter)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(ComponentView), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ComponentView>().OnValueChanged((double)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ComponentView), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static async void OnColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => await element.To<ComponentView>().OnColorChanged(e.NewValue.As<Color>());

        #endregion

        #region ComponentView

        /// <summary>
        /// 
        /// </summary>
        public ComponentView() : base() { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void OnConverterChanged(Imagin.Colour.Conversion.ColorConverter value) => ComponentModel.Converter = value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public async Task OnColorChanged(Color Value)
        {
            if (!_ValueChangedHandled)
            {
                _ColorChangedHandled = true;
                await ComponentModel.SetAsync(Value);
                _ColorChangedHandled = false;

                ColorChanged?.Invoke(this, new EventArgs<Color>(Value));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnValueChanged(double Value)
        {
            if (!_ColorChangedHandled)
            {
                _ValueChangedHandled = true;
                Color = ColorSpaceModel.GetColor();
                _ValueChangedHandled = false;
            }
        }

        #endregion
    }
}