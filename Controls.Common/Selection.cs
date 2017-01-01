using Imagin.Common;
using System;
using System.Diagnostics;
using System.Windows;

namespace Imagin.Controls.Common
{
    [Serializable]
    public class Selection : AbstractObject
    {
        #region Properties

        [field: NonSerialized]
        public event EventHandler<EventArgs> SelectionChanged;

        private double width = 0;
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                this.width = value;
                OnPropertyChanged("Width");
                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
        }

        private double height = 0;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                this.height = value;
                OnPropertyChanged("Height");
                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
        }

        private double x = 0;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
                OnPropertyChanged("X");
                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
        }

        private double y = 0;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
                OnPropertyChanged("Y");
                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
        }

        public Point TopLeft
        {
            get
            {
                return new Point(this.X, this.Y);
            }
        }

        public Point TopRight
        {
            get
            {
                return new Point(this.X + this.Width, this.Y);
            }
        }

        public Point BottomLeft
        {
            get
            {
                return new Point(this.X, this.Y + this.Height);
            }
        }

        public Point BottomRight
        {
            get
            {
                return new Point(this.X + this.Width, this.Y + this.Height);
            }
        }

        #endregion

        #region Methods

        internal void Dump()
        {
            Debug.WriteLine(string.Format("X: {0}, Y: {1}, Width: {2}, Height: {3}", this.X, this.Y, this.Width, this.Height));
        }

        public Rect GetRect()
        {
            return new Rect(x, y, width, height);
        }

        /// <summary>
        /// Set selection from given values.
        /// </summary>
        /// <param name="X">X-position of selection.</param>
        /// <param name="Y">Y-position of selection.</param>
        /// <param name="Width">Width of selection.</param>
        /// <param name="Height">Height of selection.</param>
        public void Set(double X, double Y, double Width, double Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Set selection from given Rect.
        /// </summary>
        public void Set(Rect Rect)
        {
            this.X = Rect.X;
            this.Y = Rect.Y;
            this.Width = Rect.Width;
            this.Height = Rect.Height;
        }

        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}, Width = {2}, Height = {3}", X, Y, Width, Height);
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

        public Selection(double X, double Y, double Width, double Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public Selection(Rect Rect)
        {
            this.X = Rect.X;
            this.Y = Rect.Y;
            this.Width = Rect.Width;
            this.Height = Rect.Height;
        }

        #endregion
    }
}
