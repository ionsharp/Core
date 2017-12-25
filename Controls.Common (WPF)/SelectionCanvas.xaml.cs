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

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Selection>> Selected;

        /// <summary>
        /// 
        /// </summary>
        protected bool IsDragging
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Point StartPosition
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ResetOnDrawnProperty = DependencyProperty.Register("ResetOnDrawn", typeof(bool), typeof(SelectionCanvas), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionProperty = DependencyProperty.Register("Selection", typeof(Selection), typeof(SelectionCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region SelectionCanvas

        /// <summary>
        /// 
        /// </summary>
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

        void OnDrawDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                PART_Grid.CaptureMouse();
                IsDragging = true;
                StartPosition = e.GetPosition(PART_Grid);
                Selection.Set(StartPosition.X, StartPosition.Y, 0, 0);
            }
        }

        void OnDrawMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                var Rect = GetRect(e.GetPosition(PART_Grid));
                Selection.Set(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            }
        }

        void OnDrawUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;

            if (PART_Grid.IsMouseCaptured)
                PART_Grid.ReleaseMouseCapture();

            StartPosition = default(Point);

            OnSelected(Selection);

            if (ResetOnDrawn)
                Selection.Set(0, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Selection"></param>
        protected virtual void OnSelected(Selection Selection)
        {
            if (Selected != null)
                Selected(this, new EventArgs<Selection>(Selection));
        }

        #endregion
    }
}
