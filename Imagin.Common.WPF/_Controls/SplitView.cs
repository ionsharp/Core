using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// A <see cref="Control"/> that arranges two <see cref="UIElement"/>s horizontally or vertically.
    /// </summary>
    public class SplitView : Control
    {
        ContentPresenter PART_Content1;

        ContentPresenter PART_Content2;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Content1Property = DependencyProperty.Register("Content1", typeof(UIElement), typeof(SplitView), new FrameworkPropertyMetadata(default(UIElement)));
        /// <summary>
        /// 
        /// </summary>
        public UIElement Content1
        {
            get => (UIElement)GetValue(Content1Property);
            set => SetValue(Content1Property, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Content2Property = DependencyProperty.Register("Content2", typeof(UIElement), typeof(SplitView), new FrameworkPropertyMetadata(default(UIElement)));
        /// <summary>
        /// 
        /// </summary>
        public UIElement Content2
        {
            get => (UIElement)GetValue(Content2Property);
            set => SetValue(Content2Property, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SplitView), new FrameworkPropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => (element as SplitView).OnOrientationChanged((Orientation)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public SplitView() : base() => DefaultStyleKey = typeof(SplitView);

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Content1 = Template.FindName("PART_Content1", this) as ContentPresenter;
            PART_Content2 = Template.FindName("PART_Content2", this) as ContentPresenter;
            OnOrientationChanged(Orientation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewValue"></param>
        protected virtual void OnOrientationChanged(Orientation NewValue)
        {
            if (PART_Content1 == null || PART_Content2 == null)
                return;

            int cspan = 1, rspan = 1;

            int c1 = 0, r1 = 0;
            int c2 = 0, r2 = 0;

            switch (NewValue)
            {
                case Orientation.Horizontal:
                    rspan = 2;
                    c2 = 1;
                    break;
                case Orientation.Vertical:
                    cspan = 2;
                    r2 = 1;
                    break;
            }

            Grid.SetColumn(PART_Content1, c1);
            Grid.SetColumn(PART_Content2, c2);

            Grid.SetColumnSpan(PART_Content1, cspan);
            Grid.SetColumnSpan(PART_Content2, cspan);

            Grid.SetRow(PART_Content1, r1);
            Grid.SetRow(PART_Content2, r2);

            Grid.SetRowSpan(PART_Content1, rspan);
            Grid.SetRowSpan(PART_Content2, rspan);
        }
    }
}
