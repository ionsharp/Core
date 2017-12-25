using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ThicknessBox : UserControl
    {
        bool ThicknessPartChangeHandled = false;

        bool ThicknessChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FieldSpacingProperty = DependencyProperty.Register("FieldSpacing", typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Thickness FieldSpacing
        {
            get
            {
                return (Thickness)GetValue(FieldSpacingProperty);
            }
            set
            {
                SetValue(FieldSpacingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnThicknessChanged));
        /// <summary>
        /// 
        /// </summary>
        public Thickness Thickness
        {
            get
            {
                return (Thickness)GetValue(ThicknessProperty);
            }
            set
            {
                SetValue(ThicknessProperty, value);
            }
        }
        static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ThicknessBox).OnThicknessChanged((Thickness)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public ThicknessBox()
        {
            InitializeComponent();
            SetCurrentValue(FieldSpacingProperty, new Thickness(0, 0, 5, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnThicknessPartChanged(object sender, TextChangedEventArgs e)
        {
            if (!ThicknessPartChangeHandled)
            {
                ThicknessChangeHandled = true;
                SetCurrentValue(ThicknessProperty, new Thickness(PART_Left.Value, PART_Top.Value, PART_Right.Value, PART_Bottom.Value));
                ThicknessChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnThicknessChanged(Thickness Value)
        {
            if (!ThicknessChangeHandled)
            {
                ThicknessPartChangeHandled = true;
                PART_Left.Value = Value.Left;
                PART_Top.Value = Value.Top;
                PART_Right.Value = Value.Right;
                PART_Bottom.Value = Value.Bottom;
                ThicknessPartChangeHandled = false;
            }
        }
    }
}
