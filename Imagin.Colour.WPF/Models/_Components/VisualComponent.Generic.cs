using Imagin.Colour.Controls.Linq;
using Imagin.Colour.Linq;
using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TColor"></typeparam>
    public abstract class VisualComponent<TColor> : VisualComponent where TColor : IColor
    {
        TColor From(double x, double y, double z)
        {
            return (TColor)(dynamic)new Vector(x, y, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Vector GetMaximum();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Vector GetMinimum();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public sealed override Color ColorFrom(Color color, double value)
        {
            var _color = color.ToRGB();
            var vector = _color.To<TColor>(Converter).Vector;

            var result = default(Vector);

            if (this is IComponentA)
            {
                result = new Vector(value, vector[1], vector[2]);
            }
            else if (this is IComponentB)
            {
                result = new Vector(vector[0], value, vector[2]);
            }
            else if (this is IComponentC)
            {
                result = new Vector(vector[0], vector[1], value);
            }

            return ((TColor)(dynamic)result).To<RGB>(Converter).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public sealed override Color ColorFromPoint(System.Windows.Point point)
        {
            double a = 0, b = 0, c = 0;

            if (this is IComponentA)
            {
                a = Value;
                b = point.X / 255.0;
                c = 1.0 - point.Y / 255d;
            }
            else if (this is IComponentB)
            {
                a = point.X / 255.0;
                b = Value;
                c = 1.0 - point.Y / 255d;
            }
            else if (this is IComponentC)
            {
                a = point.X / 255.0;
                b = 1.0 - point.Y / 255d;
                c = Value;
            }

            return ((TColor)(dynamic)new Vector(a, b, c)).To<RGB>(Converter).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="value"></param>
        public sealed override void DrawXY(WriteableBitmap bitmap, int value)
        {
            var maximum = GetMaximum();
            var minimum = GetMinimum();

            var _maximum = maximum + minimum.Absolute();

            var opponent = default(System.Windows.Point);

                 if (this is IComponentA)
                opponent = new System.Windows.Point(_maximum[1], _maximum[2]);

            else if (this is IComponentB)
                opponent = new System.Windows.Point(_maximum[0], _maximum[2]);

            else if (this is IComponentC)
                opponent = new System.Windows.Point(_maximum[0], _maximum[1]);

            DrawXY(bitmap, value, i =>
            {
                var result = default(TColor);

                     if (this is IComponentA)
                    result = From(value, i.X + minimum[1], i.Y + minimum[2]);

                else if (this is IComponentB)
                    result = From(i.X + minimum[0], value, i.Y + minimum[2]);

                else if (this is IComponentC)
                    result = From(i.X + minimum[0], i.Y + minimum[1], value);

                return result.To<RGB>(Converter);
            }, opponent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="color"></param>
        /// <param name="orientation"></param>
        public sealed override void DrawZ(WriteableBitmap bitmap, Color color, Orientation orientation = Orientation.Vertical)
            => DrawZ(bitmap, value => ColorFrom(color, value).ToRGB(), orientation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public sealed override double GetValue(Color color)
        {
            var _color = color.ToRGB().To<TColor>(Converter);

            var index = default(int);

            if (this is IComponentA)
            {
                index = 0;
            }
            else if (this is IComponentB)
            {
                index = 1;
            }
            else if(this is IComponentC)
            {
                index = 2;
            }

            return _color.Vector[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public sealed override System.Windows.Point PointFromColor(Color color)
        {
            var _color = color.ToRGB().To<TColor>(Converter).Vector;
            var maximum = GetMaximum();

            var result = default(System.Windows.Point);

            if (this is IComponentA)
            {
                result = PointFromColor(_color[1] / maximum[1], _color[2] / maximum[2]);
            }
            else if (this is IComponentB)
            {
                result = PointFromColor(_color[0] / maximum[0], _color[2] / maximum[2]);
            }
            else if (this is IComponentC)
            {
                result = PointFromColor(_color[0] / maximum[0], _color[1] / maximum[1]);
            }

            return result;
        }
    }
}