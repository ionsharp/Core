using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// A bindable variation of <see cref="System.Windows.Size"/>.
    /// </summary>
    [Serializable]
    public class Proportions : ObjectBase, ICloneable, IEquatable<Proportions>, IVariation<Size>
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Size>> Changed;

        double _height = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                SetValue(ref _height, value);
                OnChanged(this);
            }
        }

        double _width = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Width
        {
            get => _width;
            set
            {
                SetValue(ref _height, value);
                OnChanged(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Proportions() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Proportions(Size input) : base() => Set(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Proportions(double width, double height) : this(new Size(width, height)) { }

#pragma warning disable 1591
        public static bool operator ==(Proportions left, Proportions right) => left.Equals_(right);

        public static bool operator !=(Proportions left, Proportions right) => !(left == right);

        public static implicit operator Proportions(Size right) => new Proportions(right);

        public static implicit operator Size(Proportions right) => new Size(right.Width, right.Height);
#pragma warning restore 1591

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Proportions Clone() => new Proportions(_width, _height);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Proportions o) => this.Equals<Proportions>(o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Size Get() => this;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Size Value) => Changed?.Invoke(this, new EventArgs<Size>(Value));

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
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Proportions)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => ((Size)this).GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "Width => {0}, Height => {1}".F(_width, _height);
    }
}
