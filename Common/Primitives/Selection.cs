using Imagin.Common.Input;
using System;
using System.Windows;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents a rect with binding support.
    /// </summary>
    [Serializable]
    public class Selection : AbstractObject, ICloneable, IVariant<Rect>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Rect>> Changed;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> PositionChanged;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Size>> SizeChanged;

        double width = 0;
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public Point Position
        {
            get
            {
                return Rect.Location;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point TopLeft
        {
            get
            {
                return Rect.TopLeft;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point TopRight
        {
            get
            {
                return Rect.TopRight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point BottomLeft
        {
            get
            {
                return Rect.BottomLeft;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point BottomRight
        {
            get
            {
                return Rect.BottomRight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Rect Rect
        {
            get
            {
                return new Rect(x, y, width, height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Size"></param>
        public Selection(Point Point, Size Size)
        {
            Set(Point, Size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rect"></param>
        public Selection(Rect Rect)
        {
            Set(Rect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values"></param>
        public Selection(double[] Values)
        {
            Set(Values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Selection(double x, double y, double width, double height)
        {
            Set(X, Y, Width, Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values"></param>
        public static implicit operator Selection(double[] Values)
        {
            return new Selection(Values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Selection(Rect Value)
        {
            return new Selection(Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        protected virtual void OnPositionChanged(Point Point)
        {
            if (PositionChanged != null)
                PositionChanged(this, new EventArgs<Point>(Point));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Size"></param>
        protected virtual void OnSizeChanged(Size Size)
        {
            if (SizeChanged != null)
                SizeChanged(this, new EventArgs<Size>(Size));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Selection Clone()
        {
            return new Selection(Rect);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rect Get()
        {
            return Rect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Rect Value)
        {
            if (Changed != null)
                Changed(this, new EventArgs<Rect>(Value));
        }

        /// <summary>
        /// Set selection from given values.
        /// </summary>
        /// <param name="x">X-position of selection.</param>
        /// <param name="y">Y-position of selection.</param>
        /// <param name="width">Width of selection.</param>
        /// <param name="height">Height of selection.</param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values"></param>
        public void Set(double[] Values)
        {
            if (Values.Length == 4)
                Set(new Rect(Values[0], Values[1], Values[2], Values[3]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Size"></param>
        public void Set(Point Point, Size Size)
        {
            Set(new Rect(Point, Size));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}, Width = {2}, Height = {3}", X, Y, Width, Height);
        }

        #endregion
    }
}
