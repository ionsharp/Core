using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    #region Curve

    [DisplayName("Curve")]
    [Icon(App.ImagePath + "Curve.png")]
    [Serializable]
    public class Curve : BaseNamable, ICloneable
    {
        const double NATURAL_LOG_OF_2 = 0.693147181d;

        PointCollection Preview
        {
            get
            {
                var result = new PointCollection();
                for (var x = 0d; x < 1; x += 0.02)
                    result.Add(new(x, 1 - Execute(x)));

                result.Add(new(1, 1 - Execute(1)));
                return result;
            }
        }

        [Hidden]
        public PointCollection PreviewX
        {
            get
            {
                var result = Preview;
                result.Add(new(1, 0));
                result.Add(new(0, 0));
                result.Add(result[0]);
                return result;
            }
        }

        [Hidden]
        public PointCollection PreviewY
        {
            get
            {
                var result = Preview;
                result.Add(new(1, 1));
                result.Add(new(0, 1));
                result.Add(result[0]);
                return result;
            }
        }

        //...

        public virtual double Execute(double x) => x;

        public object Clone() => GetType().Create<Curve>();
    }

    #endregion

    #region Default curves

    [Serializable]
    public abstract class DefaultCurve : Curve
    {
        [Featured, ReadOnly]
        public override string Name => GetType().Name;

        public override double Execute(double x) => Execute(0, 1, x);

        public abstract double Execute(double start, double end, double value);
    }

    [Serializable]
    public class Linear : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
            => start + value * (end - start);
    }

    [Serializable]
    public class Spring : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            //value = Math.Clamp01(value);
            value = (Math.Sin(value * Math.PI * (0.2d + 2.5d * value * value * value)) * Math.Pow(1d - value, 2.2d) + value) * (1d + (1.2d * (1d - value)));
            return start + (end - start) * value;
        }
    }

    [Serializable]
    public class EaseInQuad : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * value * value + start;
        }
    }

    [Serializable]
    public class EaseOutQuad : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }
    }

    [Serializable]
    public class EaseInOutQuad : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return end * 0.5d * value * value + start;
            value--;
            return -end * 0.5d * (value * (value - 2) - 1) + start;
        }
    }

    [Serializable]
    public class EaseInCubic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value + start;
        }
    }

    [Serializable]
    public class EaseOutCubic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }
    }

    [Serializable]
    public class EaseInOutCubic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return end * 0.5d * value * value * value + start;
            value -= 2;
            return end * 0.5d * (value * value * value + 2) + start;
        }
    }

    [Serializable]
    public class EaseInQuart : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value * value + start;
        }
    }

    [Serializable]
    public class EaseOutQuart : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }
    }

    [Serializable]
    public class EaseInOutQuart : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return end * 0.5d * value * value * value * value + start;
            value -= 2;
            return -end * 0.5d * (value * value * value * value - 2) + start;
        }
    }

    [Serializable]
    public class EaseInQuint : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value * value * value + start;
        }
    }

    [Serializable]
    public class EaseOutQuint : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }
    }

    [Serializable]
    public class EaseInOutQuint : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return end * 0.5d * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5d * (value * value * value * value * value + 2) + start;
        }
    }

    [Serializable]
    public class EaseInSine : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return -end * Math.Cos(value * (Math.PI * 0.5d)) + end + start;
        }
    }

    [Serializable]
    public class EaseOutSine : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * Math.Sin(value * (Math.PI * 0.5d)) + start;
        }
    }

    [Serializable]
    public class EaseInOutSine : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return -end * 0.5d * (Math.Cos(Math.PI * value) - 1) + start;
        }
    }

    [Serializable]
    public class EaseInExpo : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * Math.Pow(2, 10 * (value - 1)) + start;
        }
    }

    [Serializable]
    public class EaseOutExpo : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return end * (-Math.Pow(2, -10 * value) + 1) + start;
        }
    }

    [Serializable]
    public class EaseInOutExpo : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return end * 0.5d * Math.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5d * (-Math.Pow(2, -10 * value) + 2) + start;
        }
    }

    [Serializable]
    public class EaseInCirc : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            return -end * (Math.Sqrt(1 - value * value) - 1) + start;
        }
    }

    [Serializable]
    public class EaseOutCirc : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * Math.Sqrt(1 - value * value) + start;
        }
    }

    [Serializable]
    public class EaseInOutCirc : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            value /= .5d;
            end -= start;
            if (value < 1) return -end * 0.5d * (Math.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5d * (Math.Sqrt(1 - value * value) + 1) + start;
        }
    }

    [Serializable]
    public class EaseInBounce : DefaultCurve
    {
        public static readonly EaseInBounce Default = new();

        public override double Execute(double start, double end, double value)
        {
            end -= start;
            double d = 1d;
            return end - EaseOutBounce.Default.Execute(0, end, d - value) + start;
        }
    }

    [Serializable]
    public class EaseOutBounce : DefaultCurve
    {
        public static readonly EaseOutBounce Default = new();

        public override double Execute(double start, double end, double value)
        {
            value /= 1d;
            end -= start;
            if (value < (1 / 2.75d))
            {
                return end * (7.5625d * value * value) + start;
            }
            else if (value < (2 / 2.75d))
            {
                value -= (1.5d / 2.75d);
                return end * (7.5625d * (value) * value + .75d) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25d / 2.75d);
                return end * (7.5625d * (value) * value + .9375d) + start;
            }
            else
            {
                value -= (2.625d / 2.75d);
                return end * (7.5625d * (value) * value + .984375d) + start;
            }
        }
    }

    [Serializable]
    public class EaseInOutBounce : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            double d = 1d;
            if (value < d * 0.5d) return EaseInBounce.Default.Execute(0, end, value * 2) * 0.5d + start;
            else return EaseOutBounce.Default.Execute(0, end, value * 2 - d) * 0.5d + end * 0.5d + start;
        }
    }

    [Serializable]
    public class EaseInBack : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;
            value /= 1;
            double s = 1.70158d;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }
    }

    [Serializable]
    public class EaseOutBack : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            double s = 1.70158d;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }
    }

    [Serializable]
    public class EaseInOutBack : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            double s = 1.70158d;
            end -= start;
            value /= .5d;
            if ((value) < 1)
            {
                s *= (1.525d);
                return end * 0.5d * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525d);
            return end * 0.5d * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }
    }

    [Serializable]
    public class EaseInElastic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;

            double d = 1d;
            double p = d * .3d;
            double s;
            double a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0d || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            return -(a * Math.Pow(2, 10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p)) + start;
        }
    }

    [Serializable]
    public class EaseOutElastic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;

            double d = 1d;
            double p = d * .3d;
            double s;
            double a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0d || a < Math.Abs(end))
            {
                a = end;
                s = p * 0.25d;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            return (a * Math.Pow(2, -10 * value) * Math.Sin((value * d - s) * (2 * Math.PI) / p) + end + start);
        }
    }

    [Serializable]
    public class EaseInOutElastic : DefaultCurve
    {
        public override double Execute(double start, double end, double value)
        {
            end -= start;

            double d = 1d;
            double p = d * .3d;
            double s;
            double a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5d) == 2) return start + end;

            if (a == 0d || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            if (value < 1) return -0.5d * (a * Math.Pow(2, 10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p)) + start;
            return a * Math.Pow(2, -10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p) * 0.5d + end + start;
        }
    }

    #endregion
}