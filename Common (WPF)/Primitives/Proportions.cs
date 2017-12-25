using Imagin.Common.Linq;
using Imagin.Common.Input;
using System;
using System.Windows;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents <see cref="System.Windows.Size"/> with binding support; variant of <see cref="System.Windows.Size"/>.
    /// </summary>
    [Serializable]
    public class Proportions : BindableObject, ICloneable, IVariant<Size>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Size>> Changed;

        double width = 0d;
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
                width = value.Coerce(double.MaxValue, 0d);
                OnChanged(Size);
                OnPropertyChanged("Width");
            }
        }

        double height = 0d;
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
                height = value.Coerce(double.MaxValue, 0d);
                OnChanged(Size);
                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(width, height);
            }
        }

        #endregion

        #region Proportions

        /// <summary>
        /// 
        /// </summary>
        public Proportions() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Size"></param>
        public Proportions(Size Size) : base()
        {
            Set(Size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Proportions(double Width, double Height) : base()
        {
            Set(Width, Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Proportions(Size Value)
        {
            return new Proportions(Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Proportions Clone()
        {
            return new Proportions(Size);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Size Get()
        {
            return Size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Size Value)
        {
            if (Changed != null)
                Changed(this, new EventArgs<Size>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Size"></param>
        public void Set(Size Size)
        {
            Set(Size.Width, Size.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Set(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Size.ToString();
        }

        #endregion
    }
}
