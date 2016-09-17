using Imagin.Common;
using System.Windows;
using System.Windows.Controls;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    public class ThicknessBox : ContentControl
    {
        bool PartChangedHandled = false;

        bool ThicknessChangedHandled = false;

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)GetValue(ContentMarginProperty);
            }
            set
            {
                SetValue(ContentMarginProperty, value);
            }
        }

        public static DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness Spacing
        {
            get
            {
                return (Thickness)GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }

        public static DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPartChanged));
        public double Left
        {
            get
            {
                return (double)GetValue(LeftProperty);
            }
            set
            {
                SetValue(LeftProperty, value);
            }
        }

        public static DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPartChanged));
        public double Top
        {
            get
            {
                return (double)GetValue(TopProperty);
            }
            set
            {
                SetValue(TopProperty, value);
            }
        }

        public static DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPartChanged));
        public double Right
        {
            get
            {
                return (double)GetValue(RightProperty);
            }
            set
            {
                SetValue(RightProperty, value);
            }
        }

        public static DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPartChanged));
        public double Bottom
        {
            get
            {
                return (double)GetValue(BottomProperty);
            }
            set
            {
                SetValue(BottomProperty, value);
            }
        }

        static void OnPartChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<ThicknessBox>().OnPartChanged();
        }

        public static DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnThicknessChanged));
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
        static void OnThicknessChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<ThicknessBox>().OnThicknessChanged();
        }

        void Get(out double Left, out double Top, out double Right, out double Bottom)
        {
            Left = 0.0;
            Top = 0.0;
            Right = 0.0;
            Bottom = 0.0;
            if (this.Thickness != null)
            {
                Left = this.Thickness.Left;
                Top = this.Thickness.Top;
                Right = this.Thickness.Right;
                Bottom = this.Thickness.Bottom;
            }
        }

        void Set(double Left, double Top, double Right, double Bottom)
        {
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }

        protected virtual void OnPartChanged()
        {
            if (this.PartChangedHandled)
            {
                this.PartChangedHandled = false;
                return;
            }
            this.ThicknessChangedHandled = true;
            this.Thickness = new Thickness(this.Left, this.Top, this.Right, this.Bottom);
        }

        protected virtual void OnThicknessChanged()
        {
            if (this.ThicknessChangedHandled)
            {
                this.ThicknessChangedHandled = false;
                return;
            }
            this.PartChangedHandled = true;

            double Left = 0.0, Top = 0.0, Right = 0.0, Bottom = 0.0;
            this.Get(out Left, out Top, out Right, out Bottom);
            this.Set(Left, Top, Right, Bottom);

            this.PartChangedHandled = false;
        }

        public ThicknessBox() : base()
        {
            this.DefaultStyleKey = typeof(ThicknessBox);
            this.ContentMargin = new Thickness(0, 0, 5, 0);
            this.Spacing = new Thickness(0, 0, 5, 0);
        }
    }
}
