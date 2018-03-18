using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonChrome : ContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius), new PropertyChangedCallback(OnCornerRadiusChanged)));
        /// <summary>
        /// 
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        static void OnCornerRadiusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ButtonChrome buttonChrome = o as ButtonChrome;
            if (buttonChrome != null)
                buttonChrome.OnCornerRadiusChanged((CornerRadius)e.OldValue, (CornerRadius)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty InnerCornerRadiusProperty = DependencyProperty.Register("InnerCornerRadius", typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius)));
        /// <summary>
        /// 
        /// </summary>
        public CornerRadius InnerCornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(InnerCornerRadiusProperty);
            }
            set
            {
                SetValue(InnerCornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderCheckedProperty = DependencyProperty.Register("RenderChecked", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderChecked
        {
            get
            {
                return (bool)GetValue(RenderCheckedProperty);
            }
            set
            {
                SetValue(RenderCheckedProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderEnabledProperty = DependencyProperty.Register("RenderEnabled", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderEnabled
        {
            get
            {
                return (bool)GetValue(RenderEnabledProperty);
            }
            set
            {
                SetValue(RenderEnabledProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderFocusedProperty = DependencyProperty.Register("RenderFocused", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderFocused
        {
            get
            {
                return (bool)GetValue(RenderFocusedProperty);
            }
            set
            {
                SetValue(RenderFocusedProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderMouseOverProperty = DependencyProperty.Register("RenderMouseOver", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderMouseOver
        {
            get
            {
                return (bool)GetValue(RenderMouseOverProperty);
            }
            set
            {
                SetValue(RenderMouseOverProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderNormalProperty = DependencyProperty.Register("RenderNormal", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderNormal
        {
            get
            {
                return (bool)GetValue(RenderNormalProperty);
            }
            set
            {
                SetValue(RenderNormalProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RenderPressedProperty = DependencyProperty.Register("RenderPressed", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        /// <summary>
        /// 
        /// </summary>
        public bool RenderPressed
        {
            get
            {
                return (bool)GetValue(RenderPressedProperty);
            }
            set
            {
                SetValue(RenderPressedProperty, value);
            }
        }

        static ButtonChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonChrome), new FrameworkPropertyMetadata(typeof(ButtonChrome)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The <see cref="InnerCornerRadius"/> should be one less than the <see cref="CornerRadius"/>.
        /// </remarks>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnCornerRadiusChanged(CornerRadius oldValue, CornerRadius newValue)
        {
            var cornerRadius = new CornerRadius
            (
                Math.Max(0, newValue.TopLeft - 1), 
                Math.Max(0, newValue.TopRight - 1), 
                Math.Max(0, newValue.BottomRight - 1), 
                Math.Max(0, newValue.BottomLeft - 1)
            );
            InnerCornerRadius = cornerRadius;
        }
    }
}
