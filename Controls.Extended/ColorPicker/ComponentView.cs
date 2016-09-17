using Imagin.Common.Events;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public class ComponentView : ContentControl
    {
        #region Properties

        public event EventHandler<EventArgs<Color>> ColorChanged;

        protected internal bool ColorChangedHandled = false;

        protected internal bool ValueChangedHandled = false;

        public static DependencyProperty ColorSpaceModelProperty = DependencyProperty.Register("ColorSpaceModel", typeof(ColorSpaceModel), typeof(ComponentView), new FrameworkPropertyMetadata(default(ColorSpaceModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ColorSpaceModel ColorSpaceModel
        {
            get
            {
                return (ColorSpaceModel)GetValue(ColorSpaceModelProperty);
            }
            set
            {
                SetValue(ColorSpaceModelProperty, value);
            }
        }

        public static DependencyProperty ComponentModelProperty = DependencyProperty.Register("ComponentModel", typeof(ComponentModel), typeof(ComponentView), new FrameworkPropertyMetadata(default(ComponentModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ComponentModel ComponentModel
        {
            get
            {
                return (ComponentModel)GetValue(ComponentModelProperty);
            }
            set
            {
                SetValue(ComponentModelProperty, value);
            }
        }

        public static DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue", typeof(double), typeof(ComponentView), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentValueChanged));
        public double CurrentValue
        {
            get
            {
                return (double)GetValue(CurrentValueProperty);
            }
            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }
        static void OnCurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentView>().OnValueChanged(e.NewValue.As<double>());
        }

        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ComponentView), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }
        static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentView>().OnColorChanged(e.NewValue.As<Color>());
        }

        #endregion

        #region Methods

        public virtual void OnColorChanged(Color Color)
        {
            if (ValueChangedHandled) return;
            ColorChangedHandled = true;
            this.ComponentModel.SetValue(Color);
            ColorChangedHandled = false;

            if (this.ColorChanged != null)
                this.ColorChanged(this, new EventArgs<Color>(Color));
        }

        public virtual void OnValueChanged(double Value)
        {
            if (this.ColorChangedHandled) return;
            this.ValueChangedHandled = true;
            this.Color = this.ColorSpaceModel.GetColor();
            this.ValueChangedHandled = false;
        }

        #endregion

        #region ComponentView

        public ComponentView() : base()
        {
        }

        #endregion
    }
}
