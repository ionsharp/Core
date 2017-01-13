using Imagin.Common.Input;
using System;
using System.Windows;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents a rect with binding support.
    /// </summary>
    [Serializable]
    public class Selection : AbstractObject, IVariant<Rect>
    {
        #region Properties

        [field: NonSerialized]
        public event EventHandler<EventArgs<Rect>> Changed;

        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> PositionChanged;

        [field: NonSerialized]
        public event EventHandler<EventArgs<Size>> SizeChanged;

        double width = 0;
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
                OnChanged(Rect);
                OnSizeChanged(Size);
            }
        }

        double height = 0;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
                OnChanged(Rect);
                OnSizeChanged(Size);
            }
        }

        double x = 0;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
                OnPositionChanged(Position);
                OnChanged(Rect);
            }
        }

        double y = 0;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
                OnPositionChanged(Position);
                OnChanged(Rect);
            }
        }

        public Point Position
        {
            get
            {
                return Rect.Location;
            }
        }

        public Point TopLeft
        {
            get
            {
                return Rect.TopLeft;
            }
        }

        public Point TopRight
        {
            get
            {
                return Rect.TopRight;
            }
        }

        public Point BottomLeft
        {
            get
            {
                return Rect.BottomLeft;
            }
        }

        public Point BottomRight
        {
            get
            {
                return Rect.BottomRight;
            }
        }

        public Rect Rect
        {
            get
            {
                return new Rect(x, y, width, height);
            }
        }

        public Size Size
        {
            get
            {
                return Rect.Size;
            }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Initializes new instance of Selection.
        /// </summary>
        /// <param name="X">X-position of selection.</param>
        /// <param name="Y">Y-position of selection.</param>
        /// <param name="Width">Width of selection.</param>
        /// <param name="Height">Height of selection.</param>
        public Selection()
        {
        }

        public Selection(Point Point, Size Size)
        {
            Set(Point, Size);
        }

        public Selection(Rect Rect)
        {
            Set(Rect);
        }

        public Selection(double[] Values)
        {
            Set(Values);
        }

        public Selection(double x, double y, double width, double height)
        {
            Set(X, Y, Width, Height);
        }

        public static implicit operator Selection(double[] Values)
        {
            return new Selection(Values);
        }

        public static implicit operator Selection(Rect Value)
        {
            return new Selection(Value);
        }

        #endregion

        #region Methods

        protected virtual void OnPositionChanged(Point Point)
        {
            if (PositionChanged != null)
                PositionChanged(this, new EventArgs<Point>(Point));
        }

        protected virtual void OnSizeChanged(Size Size)
        {
            if (SizeChanged != null)
                SizeChanged(this, new EventArgs<Size>(Size));
        }

        public Rect Get()
        {
            return Rect;
        }

        public void OnChanged(Rect Value)
        {
            if (Changed != null)
                Changed(this, new EventArgs<Rect>(Value));
        }

        /// <summary>
        /// Set selection from given values.
        /// </summary>
        /// <param name="X">X-position of selection.</param>
        /// <param name="Y">Y-position of selection.</param>
        /// <param name="Width">Width of selection.</param>
        /// <param name="Height">Height of selection.</param>
        public void Set(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Set selection from given Rect.
        /// </summary>
        public void Set(Rect Rect)
        {
            Set(Rect.X, Rect.Y, Rect.Width, Rect.Height);
        }

        public void Set(double[] Values)
        {
            if (Values.Length == 4)
                Set(new Rect(Values[0], Values[1], Values[2], Values[3]));
        }

        public void Set(Point Point, Size Size)
        {
            Set(new Rect(Point, Size));
        }

        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}, Width = {2}, Height = {3}", X, Y, Width, Height);
        }

        #endregion
    }
}
