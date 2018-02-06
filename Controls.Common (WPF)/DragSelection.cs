using Imagin.Common.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DragSelection : System.Windows.Controls.UserControl
    {
        Border PART_Rectangle
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionProperty = DependencyProperty.Register("Selection", typeof(Selection), typeof(DragSelection), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectionChanged));
        /// <summary>
        /// 
        /// </summary>
        public Selection Selection
        {
            get
            {
                return (Selection)GetValue(SelectionProperty);
            }
            set
            {
                SetValue(SelectionProperty, value);
            }
        }
        static void OnSelectionChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((DragSelection)Object).OnSelectionChanged((Selection)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public DragSelection() : base()
        {
            this.DefaultStyleKey = typeof(DragSelection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectionChanged(Selection Value)
        {
            if (Value != null)
                Selection.PositionChanged += OnPositionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Rectangle = Template.FindName("PART_Rectangle", this) as Border;
        }

        void OnPositionChanged(object sender, Imagin.Common.Input.EventArgs<Point> e)
        {
            if (PART_Rectangle != null)
                PART_Rectangle.RenderTransform = new TranslateTransform(e.Value.X, e.Value.Y);
        }
    }
}
