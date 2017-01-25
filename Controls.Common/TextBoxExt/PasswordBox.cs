using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Dots", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_EnterButton", Type = typeof(Button))]
    public class PasswordBox : TextBoxExt
    {
        Button PART_EnterButton
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanEnterProperty = DependencyProperty.Register("CanEnter", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool CanEnter
        {
            get
            {
                return (bool)GetValue(CanEnterProperty);
            }
            set
            {
                SetValue(CanEnterProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DotForegroundProperty = DependencyProperty.Register("DotForeground", typeof(Brush), typeof(PasswordBox), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush DotForeground
        {
            get
            {
                return (Brush)GetValue(DotForegroundProperty);
            }
            set
            {
                SetValue(DotForegroundProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DotSizeProperty = DependencyProperty.Register("DotSize", typeof(double), typeof(PasswordBox), new FrameworkPropertyMetadata(6d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double DotSize
        {
            get
            {
                return (double)GetValue(DotSizeProperty);
            }
            set
            {
                SetValue(DotSizeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DotSpacingProperty = DependencyProperty.Register("DotSpacing", typeof(Thickness), typeof(PasswordBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Thickness DotSpacing
        {
            get
            {
                return (Thickness)GetValue(DotSpacingProperty);
            }
            set
            {
                SetValue(DotSpacingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string Hint
        {
            get
            {
                return (string)GetValue(HintProperty);
            }
            set
            {
                SetValue(HintProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowEnterButtonProperty = DependencyProperty.Register("ShowEnterButton", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowEnterButton
        {
            get
            {
                return (bool)GetValue(ShowEnterButtonProperty);
            }
            set
            {
                SetValue(ShowEnterButtonProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowPasswordProperty = DependencyProperty.Register("ShowPassword", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowPassword
        {
            get
            {
                return (bool)GetValue(ShowPasswordProperty);
            }
            set
            {
                SetValue(ShowPasswordProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public PasswordBox()
        {
            DefaultStyleKey = typeof(PasswordBox);
        }

        /// <summary>
        /// When this.Text changes, set System.Windows.Controls.PasswordBox.Password.
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_EnterButton = Template.FindName("PART_EnterButton", this) as Button;
            if (PART_EnterButton != null)
                PART_EnterButton.Click += (s, e) => OnEntered(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="HandledTypes"></param>
        /// <returns></returns>
        protected override bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e, Type[] HandledTypes = null)
        {
            return base.OnPreviewMouseLeftButtonDownHandled(e, new Type[] 
            {
                typeof(Button),
                typeof(ToggleButton),
            });
        }
    }
}
