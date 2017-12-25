using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentView : ContentControl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ColorChanged;

        /// <summary>
        /// 
        /// </summary>
        protected internal bool ColorChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        protected internal bool ValueChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorSpaceModelProperty = DependencyProperty.Register("ColorSpaceModel", typeof(ColorSpaceModel), typeof(ComponentView), new FrameworkPropertyMetadata(default(ColorSpaceModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentModelProperty = DependencyProperty.Register("ComponentModel", typeof(ComponentModel), typeof(ComponentView), new FrameworkPropertyMetadata(default(ComponentModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ComponentView), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentView>().OnValueChanged((double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ComponentView), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static async void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await d.As<ComponentView>().OnColorChanged(e.NewValue.As<Color>());
        }

        #endregion

        #region ComponentView

        /// <summary>
        /// 
        /// </summary>
        public ComponentView() : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public virtual async Task OnColorChanged(Color Value)
        {
            if (!ValueChangedHandled)
            {
                ColorChangedHandled = true;
                await ComponentModel.BeginSet(Value);
                ColorChangedHandled = false;

                ColorChanged?.Invoke(this, new EventArgs<Color>(Value));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public virtual void OnValueChanged(double Value)
        {
            if (!ColorChangedHandled)
            {
                ValueChangedHandled = true;
                Color = ColorSpaceModel.GetColor();
                ValueChangedHandled = false;
            }
        }

        #endregion
    }
}
