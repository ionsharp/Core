using Imagin.Common.Input;
using Imagin.Common.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public partial class SelectionCanvas : UserControl
    {
        #region Properties

        public event EventHandler<EventArgs<Selection>> Selected;

        protected bool IsDragging
        {
            get; private set;
        }

        protected Point StartPosition
        {
            get; private set;
        }

        public static DependencyProperty ResetOnDrawnProperty = DependencyProperty.Register("ResetOnDrawn", typeof(bool), typeof(SelectionCanvas), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ResetOnDrawn
        {
            get
            {
                return (bool)GetValue(ResetOnDrawnProperty);
            }
            set
            {
                SetValue(ResetOnDrawnProperty, value);
            }
        }

        public static DependencyProperty SelectionProperty = DependencyProperty.Register("Selection", typeof(Selection), typeof(SelectionCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region SelectionCanvas

        public SelectionCanvas()
        {
            InitializeComponent();

            Selection = new Selection();
        }

        #endregion

        #region Methods

        Rect GetRect(Point CurrentPosition)
        {
            var Result = new Rect();

            double
                x = (StartPosition.X < CurrentPosition.X ? StartPosition.X : CurrentPosition.X),
                y = (StartPosition.Y < CurrentPosition.Y ? StartPosition.Y : CurrentPosition.Y);

            var SelectionSize = new Size(Selection.Width, Selection.Height);

            Result.Size = new Size(Math.Abs(CurrentPosition.X - StartPosition.X), Math.Abs(CurrentPosition.Y - StartPosition.Y));
            Result.X = x;
            Result.Y = y;

            return Result;
        }

        private void OnDrawDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                PART_Grid.CaptureMouse();
                IsDragging = true;
                StartPosition = e.GetPosition(PART_Grid);
                Selection.Set(StartPosition.X, StartPosition.Y, 0, 0);
            }
        }

        private void OnDrawMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                var Rect = GetRect(e.GetPosition(PART_Grid));
                Selection.Set(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            }
        }

        private void OnDrawUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;

            if (PART_Grid.IsMouseCaptured)
                PART_Grid.ReleaseMouseCapture();

            StartPosition = default(Point);

            OnSelected(Selection);

            if (ResetOnDrawn)
                Selection.Set(0, 0, 0, 0);
        }

        protected virtual void OnSelected(Selection Selection)
        {
            if (Selected != null)
                Selected(this, new EventArgs<Selection>(Selection));
        }

        #endregion
    }
}
