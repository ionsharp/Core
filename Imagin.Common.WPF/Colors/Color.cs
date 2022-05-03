using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Linq;

namespace Imagin.Common.Colors
{
    #region (abstract) ColorVector

    [Serializable]
    public abstract class ColorVector
    {
        public Vector Value { get; protected set; }

        public double this[int i]
        {
            get => Value[i];
            set => Value = new(i == 0 ? value : Value[0], i == 1 ? value : Value[1], i == 2 ? value : Value[2]);
        }

        //...

        protected ColorVector(params double[] input) : base() => Value = new Vector(input);

        public static implicit operator Vector(ColorVector input) => input.Value;

        //...

        public override string ToString() => Value.ToString();

        //...

        public abstract RGB Convert();

        public abstract void Convert(RGB input);

        //...

        /// <summary>
        /// Prints all <see cref="RGB"/> values based on the specified <see cref="IModel"/> (for debugging purposes).
        /// </summary>
        public static void Print(ColorModels model)
        {
            double minA = double.MaxValue, minB = double.MaxValue, minC = double.MaxValue, maxA = double.MinValue, maxB = double.MinValue, maxC = double.MinValue;
            for (var r = 0.0; r < 256; r++)
            {
                for (var g = 0.0; g < 256; g++)
                {
                    for (var b = 0.0; b < 256; b++)
                    {
                        var rgb = new RGB(r, g, b);
                        var xyz = Create(model, rgb);

                        if (xyz[0] < minA)
                            minA = xyz[0];
                        if (xyz[1] < minB)
                            minB = xyz[1];
                        if (xyz[2] < minC)
                            minC = xyz[2];

                        if (xyz[0] > maxA)
                            maxA = xyz[0];
                        if (xyz[1] > maxB)
                            maxB = xyz[1];
                        if (xyz[2] > maxC)
                            maxC = xyz[2];
                    }
                }
            }
            Log.Write<ColorVector>($"{model}: minimum [{minA}, {minB}, {minC}], maximum [{maxA}, {maxB}, {maxC}]");
        }

        public static VisualColor Create(ColorModels model)
        {
            foreach (var i in XAssembly.GetAssembly(InternalAssembly.Name).GetDerivedTypes(typeof(VisualColor)))
            {
                var j = i.Create<VisualColor>();
                if (j.Model == model)
                    return j;
            }
            return null;
        }

        public static VisualColor Create(ColorModels model, Vector3<double> values)
        {
            var result = Create(model);
            result.Value = new(values);
            return result;
        }

        public static VisualColor Create(ColorModels model, RGB rgb)
        {
            var result = Create(model);
            result.Convert(rgb);
            return result;
        }
    }

    #endregion

    #region (abstract) LogicalColor

    /// <summary>
    /// A color that does not support a visual representation.
    /// </summary>
    [Serializable]
    public abstract class LogicalColor : ColorVector
    {
        protected LogicalColor(params double[] input) : base() => Value = new Vector(input);
    }

    #endregion

    #region (abstract) VisualColor

    /// <summary>
    /// A color that supports a visual representation.
    /// </summary>
    [Serializable]
    public abstract class VisualColor : ColorVector
    {
        public abstract ColorModels Model { get; }

        public Range<Vector> Range
            => new(new(Model.GetComponents()[0].Minimum, Model.GetComponents()[1].Minimum, Model.GetComponents()[2].Minimum), new(Model.GetComponents()[0].Maximum, Model.GetComponents()[1].Maximum, Model.GetComponents()[2].Maximum));

        protected VisualColor(params double[] input) : base() => Value = new Vector(input);
    }

    #endregion

    ///[Logical]

    #region CMYK

    [Serializable]
    public sealed class CMYK : LogicalColor
    {
        public CMYK(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
        public override RGB Convert()
        {
            var r = (1.0 - Value[0]) * (1.0 - Value[3]);
            var g = (1.0 - Value[1]) * (1.0 - Value[3]);
            var b = (1.0 - Value[2]) * (1.0 - Value[3]);
            return new RGB(r, g, b);
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
        public override void Convert(RGB input)
        {
            var k0 = 1.0 - Math.Max(input[0], Math.Max(input[1], input[2]));
            var k1 = 1.0 - k0;

            var c = (1.0 - input[0] - k0) / k1;
            var m = (1.0 - input[1] - k0) / k1;
            var y = (1.0 - input[2] - k0) / k1;

            c = double.IsNaN(c) ? 0 : c;
            m = double.IsNaN(m) ? 0 : m;
            y = double.IsNaN(y) ? 0 : y;

            Value = new(c, m, y, k0);
        }
    }

    #endregion

    #region LinearRGB [Hidden]

    [Hidden]
    [Serializable]
    public sealed class LinearRGB : LogicalColor
    {
        public LinearRGB(params double[] input) : base(input) { }

        /// <remarks>https://github.com/tompazourek/Colourful</remarks>
        public override RGB Convert() => Convert(DefaultColorProfiles.sRGB);

        /// <remarks>https://github.com/tompazourek/Colourful</remarks>
        public override void Convert(RGB input) => Value = input.Convert(DefaultColorProfiles.sRGB).Value;

        public RGB Convert(ColorProfile profile)
        {
            var oldValue = Value;
            var newValue = oldValue.Transform(i => profile.Companding.ConvertToNonLinear(i));
            return new(newValue);
        }
    }

    #endregion

    ///[Visual]

    #region RGB

    [Serializable]
    public sealed class RGB : VisualColor
    {
        public override ColorModels Model => ColorModels.RGB;

        public RGB(params double[] input) : base(input) { }

        public RGB(System.Windows.Media.Color input) : base(input.R / 255, input.G / 255, input.B / 255) { }

        public static implicit operator System.Windows.Media.Color(RGB input)
        {
            var result = input.Value.Transform(i => i.Multiply(255));
            return System.Windows.Media.Color.FromArgb(byte.MaxValue, result[0], result[1], result[2]);
        }

        public override RGB Convert() => new(Value);

        public override void Convert(RGB input) => Value = new(input.Value);

        /// <remarks>https://github.com/tompazourek/Colourful</remarks>
        public LinearRGB Convert(ColorProfile profile)
        {
            var oldValue = Value;
            var newValue = oldValue.Transform(i => profile.Companding.ConvertToLinear(i));
            return new(newValue);
        }
    }

    #endregion

    ///H**

    #region HCV

    public sealed class HCV : VisualColor
    {
        public override ColorModels Model => ColorModels.HCV;

        public HCV(params double[] input) : base(input) { }

        /// <remarks>https://github.com/helixd2s/hcv-color</remarks>
        public override RGB Convert()
        {
            double h = Value[0] / 360.0, c = Value[1] / 100.0, g = Value[2] / 100.0;

            if (c == 0)
                return new RGB(g, g, g);

            var hi = (h % 1.0) * 6.0;
            var v = hi % 1.0;
            var pure = new double[3];
            var w = 1.0 - v;

            switch (Math.Floor(hi))
            {
                case 0:
                    pure[0] = 1; pure[1] = v; pure[2] = 0; break;
                case 1:
                    pure[0] = w; pure[1] = 1; pure[2] = 0; break;
                case 2:
                    pure[0] = 0; pure[1] = 1; pure[2] = v; break;
                case 3:
                    pure[0] = 0; pure[1] = w; pure[2] = 1; break;
                case 4:
                    pure[0] = v; pure[1] = 0; pure[2] = 1; break;
                default:
                    pure[0] = 1; pure[1] = 0; pure[2] = w; break;
            }

            var mg = (1.0 - c) * g;

            return new RGB
            (
                c * pure[0] + mg,
                c * pure[1] + mg,
                c * pure[2] + mg
            );
        }

        /// <remarks>https://github.com/helixd2s/hcv-color</remarks>
        public override void Convert(RGB input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];

            var maximum = Math.Max(Math.Max(r, g), b);
            var minimum = Math.Min(Math.Min(r, g), b);

            var chroma = maximum - minimum;
            double grayscale = 0;
            double hue;

            if (chroma < 1)
                grayscale = minimum / (1.0 - chroma);

            if (chroma > 0)
            {
                if (maximum == r)
                {
                    hue = ((g - b) / chroma) % 6;
                }
                else if (maximum == g)
                {
                    hue = 2 + (b - r) / chroma;
                }
                else hue = 4 + (r - g) / chroma;

                hue /= 6;
                hue %= 1;
            }
            else hue = 0;

            Value = new(hue * 360.0, chroma * 100.0, grayscale * 100.0);
        }
    }

    #endregion

    #region HCY

    public sealed class HCY : VisualColor
    {
        public override ColorModels Model => ColorModels.HCY;

        public HCY(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hcy.js</remarks>
        public override RGB Convert()
        {
            double h = (this[0] < 0 ? (this[0] % 360) + 360 : (this[0] % 360)) * Math.PI / 180;
            double s = Math.Max(0, Math.Min(this[1], 100)) / 100;
            double i = Math.Max(0, Math.Min(this[2], 255)) / 255;

            double pi3 = Math.PI / 3;

            double r, g, b;
            if (h < (2 * pi3))
            {
                b = i * (1 - s);
                r = i * (1 + (s * Math.Cos(h) / Math.Cos(pi3 - h)));
                g = i * (1 + (s * (1 - Math.Cos(h) / Math.Cos(pi3 - h))));
            }
            else if (h < (4 * pi3))
            {
                h = h - 2 * pi3;
                r = i * (1 - s);
                g = i * (1 + (s * Math.Cos(h) / Math.Cos(pi3 - h)));
                b = i * (1 + (s * (1 - Math.Cos(h) / Math.Cos(pi3 - h))));
            }
            else
            {
                h = h - 4 * pi3;
                g = i * (1 - s);
                b = i * (1 + (s * Math.Cos(h) / Math.Cos(pi3 - h)));
                r = i * (1 + (s * (1 - Math.Cos(h) / Math.Cos(pi3 - h))));
            }

            return new(r, g, b);
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hcy.js</remarks>
        public override void Convert(RGB input) 
        {
            var sum = input[0] + input[1] + input[2];
            double r = input[0] / sum, g = input[1] / sum, b = input[2] / sum;

            var h = Math.Acos((0.5 * ((r - g) + (r - b))) / Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b)));
            if (b > g)
            {
                h = 2 * Math.PI - h;
            }

            var s = 1 - 3 * Math.Min(r, Math.Min(g, b));
            var i = sum / 3;

            Value = new(h * 180 / Math.PI, s * 100, i);
        }
    }

    #endregion

    #region HSB

    [Serializable]
    public sealed class HSB : VisualColor
    {
        public override ColorModels Model => ColorModels.HSB;

        public HSB(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsb.js</remarks>
        public override RGB Convert() => Convert(new HSB(Value));

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsb.js</remarks>
        public override void Convert(RGB input) => Value = From(input);

        public static RGB Convert(HSB input)
        {
            double _h = input[0], _s = input[1], _b = input[2];

            double r = 0, g = 0, b = 0;

            if (_s == 0)
            {
                r = g = b = _b;
            }
            else
            {
                _h *= new HSB().Range.Maximum[0];

                //The color wheel consists of 6 sectors: Figure out which sector we're in...
                var SectorPosition = _h / 60.0;
                var SectorNumber = Math.Floor(SectorPosition).Int32();

                //Get the fractional part of the sector
                var FractionalSector = SectorPosition - SectorNumber;

                //Calculate values for the three axes of the color. 
                var p = _b * (1.0 - _s);
                var q = _b * (1.0 - (_s * FractionalSector));
                var t = _b * (1.0 - (_s * (1.0 - FractionalSector)));

                //Assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (SectorNumber)
                {
                    case 0:
                        r = _b;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = _b;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = _b;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = _b;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = _b;
                        break;
                    case 5:
                        r = _b;
                        g = p;
                        b = q;
                        break;
                }
            }
            return new RGB(r, g, b);
        }

        public static HSB From(RGB input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];

            var minimum = Math.Min(input.Value[0], Math.Min(input.Value[1], input.Value[2]));
            var maximum = Math.Max(input.Value[0], Math.Max(input.Value[1], input.Value[2]));

            var chroma = maximum - minimum;

            var _h = 0.0;
            var _s = 0.0;
            var _b = maximum;

            if (chroma == 0)
            {
                _h = 0;
                _s = 0;
            }
            else
            {
                _s = chroma / maximum;

                if (input.Value[0] == maximum)
                {
                    _h = (input.Value[1] - input.Value[2]) / chroma;
                    _h = input.Value[1] < input.Value[2] ? _h + 6 : _h;
                }
                else if (input.Value[1] == maximum)
                {
                    _h = 2.0 + ((input.Value[2] - input.Value[0]) / chroma);
                }
                else if (input.Value[2] == maximum)
                    _h = 4.0 + ((input.Value[0] - input.Value[1]) / chroma);

                _h *= 60;
            }

            return new HSB(_h, _s.Shift(2), _b.Shift(2));
        }
    }

    #endregion

    #region HSL

    [Serializable]
    public sealed class HSL : VisualColor
    {
        public override ColorModels Model => ColorModels.HSL;

        public HSL(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsl.js</remarks>
        public override RGB Convert() => From(new HSL(Value));

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsl.js</remarks>
        public override void Convert(RGB input) => Value = From(input);

        public static HSL From(RGB input)
        {
            var maximum = Math.Max(Math.Max(input.Value[0], input.Value[1]), input.Value[2]);
            var minimum = Math.Min(Math.Min(input.Value[0], input.Value[1]), input.Value[2]);

            var chroma = maximum - minimum;

            double h = 0, s = 0, l = (maximum + minimum) / 2.0;

            if (chroma != 0)
            {
                s
                    = l < 0.5
                    ? chroma / (2.0 * l)
                    : chroma / (2.0 - 2.0 * l);

                if (input.Value[0] == maximum)
                {
                    h = (input.Value[1] - input.Value[2]) / chroma;
                    h = input.Value[1] < input.Value[2]
                    ? h + 6.0
                    : h;
                }
                else if (input.Value[2] == maximum)
                {
                    h = 4.0 + ((input.Value[0] - input.Value[1]) / chroma);
                }
                else if (input.Value[1] == maximum)
                    h = 2.0 + ((input.Value[2] - input.Value[0]) / chroma);

                h *= 60;
            }

            return new HSL(h, s * 100, l * 100);
        }

        public static RGB From(HSL input)
        {
            double h = input[0] * ColorModels.HSL.GetComponent(0).Maximum / 60.0, s = input[1] / 100, l = input[2] / 100;

            double r = l, g = l, b = l;

            if (s > 0)
            {
                var chroma = (1.0 - (2.0 * l - 1.0).Absolute()) * s;
                var x = chroma * (1.0 - ((h % 2.0) - 1).Absolute());

                var result = new Vector(0.0, 0, 0);

                if (0 <= h && h <= 1)
                {
                    result = new Vector(chroma, x, 0);
                }
                else if (1 <= h && h <= 2)
                {
                    result = new Vector(x, chroma, 0);
                }
                else if (2 <= h && h <= 3)
                {
                    result = new Vector(0.0, chroma, x);
                }
                else if (3 <= h && h <= 4)
                {
                    result = new Vector(0.0, x, chroma);
                }
                else if (4 <= h && h <= 5)
                {
                    result = new Vector(x, 0, chroma);
                }
                else if (5 <= h && h <= 6)
                    result = new Vector(chroma, 0, x);

                var m = l - (0.5 * chroma);

                r = result[0] + m;
                g = result[1] + m;
                b = result[2] + m;
            }

            return new RGB(r, g, b);
        }
    }

    #endregion

    #region HSP

    [Serializable]
    public sealed class HSP : VisualColor
    {
        public override ColorModels Model => ColorModels.HSP;

        public HSP(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsp.js</remarks>
        public override RGB Convert()
        {
            const double Pr = 0.299;
            const double Pg = 0.587;
            const double Pb = 0.114;

            double h = Value[0] / 360.0, s = Value[1] / 100.0, p = Value[2];

            double r, g, b, part, minOverMax = 1.0 - s;

            if (minOverMax > 0.0)
            {
                // R > G > B
                if (h < 1.0 / 6.0)
                {
                    h = 6.0 * (h - 0.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    b = p / Math.Sqrt(Pr / minOverMax / minOverMax + Pg * part * part + Pb);
                    r = (b) / minOverMax;
                    g = (b) + h * ((r) - (b));
                }
                // G > R > B
                else if (h < 2.0 / 6.0)
                {
                    h = 6.0 * (-h + 2.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    b = p / Math.Sqrt(Pg / minOverMax / minOverMax + Pr * part * part + Pb);
                    g = (b) / minOverMax;
                    r = (b) + h * ((g) - (b));
                }
                // G > B > R
                else if (h < 3.0 / 6.0)
                {
                    h = 6.0 * (h - 2.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    r = p / Math.Sqrt(Pg / minOverMax / minOverMax + Pb * part * part + Pr);
                    g = (r) / minOverMax;
                    b = (r) + h * ((g) - (r));
                }
                // B > G > R
                else if (h < 4.0 / 6.0)
                {
                    h = 6.0 * (-h + 4.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    r = p / Math.Sqrt(Pb / minOverMax / minOverMax + Pg * part * part + Pr);
                    b = (r) / minOverMax;
                    g = (r) + h * ((b) - (r));
                }
                // B > R > G
                else if (h < 5.0 / 6.0)
                {
                    h = 6.0 * (h - 4.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    g = p / Math.Sqrt(Pb / minOverMax / minOverMax + Pr * part * part + Pg);
                    b = (g) / minOverMax;
                    r = (g) + h * ((b) - (g));
                }
                // R > B > G
                else
                {
                    h = 6.0 * (-h + 6.0 / 6.0);
                    part = 1.0 + h * (1.0 / minOverMax - 1.0);
                    g = p / Math.Sqrt(Pr / minOverMax / minOverMax + Pb * part * part + Pg);
                    r = (g) / minOverMax;
                    b = (g) + h * ((r) - (g));
                }
            }
            else
            {
                // R > G > B
                if (h < 1.0 / 6.0)
                {
                    h = 6.0 * (h - 0.0 / 6.0);
                    r = Math.Sqrt(p * p / (Pr + Pg * h * h));
                    g = (r) * h;
                    b = 0.0;
                }
                // G > R > B
                else if (h < 2.0 / 6.0)
                {
                    h = 6.0 * (-h + 2.0 / 6.0);
                    g = Math.Sqrt(p * p / (Pg + Pr * h * h));
                    r = (g) * h;
                    b = 0.0;
                }
                // G > B > R
                else if (h < 3.0 / 6.0)
                {
                    h = 6.0 * (h - 2.0 / 6.0);
                    g = Math.Sqrt(p * p / (Pg + Pb * h * h));
                    b = (g) * h;
                    r = 0.0;
                }
                // B > G > R
                else if (h < 4.0 / 6.0)
                {
                    h = 6.0 * (-h + 4.0 / 6.0);
                    b = Math.Sqrt(p * p / (Pb + Pg * h * h));
                    g = (b) * h;
                    r = 0.0;
                }
                // B > R > G
                else if (h < 5.0 / 6.0)
                {
                    h = 6.0 * (h - 4.0 / 6.0);
                    b = Math.Sqrt(p * p / (Pb + Pr * h * h));
                    r = (b) * h;
                    g = 0.0;
                }
                // R > B > G
                else
                {
                    h = 6.0 * (-h + 6.0 / 6.0);
                    r = Math.Sqrt(p * p / (Pr + Pb * h * h));
                    b = (r) * h;
                    g = 0.0;
                }
            }
            return new RGB(new Vector(r.Round() / 255.0, g.Round() / 255.0, b.Round() / 255.0).Coerce(0, 1));
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsp.js</remarks>
        public override void Convert(RGB input)
        {
            const double Pr = 0.299;
            const double Pg = 0.587;
            const double Pb = 0.114;

            var _input = input.Value * 255;
            double r = _input[0], g = _input[1], b = _input[2];
            double h = 0, s = 0, p = 0;

            p = Math.Sqrt(r * r * Pr + g * g * Pg + b * b * Pb);

            if (r == g && r == b)
            {
                h = 0.0;
                s = 0.0;
            }
            else
            {
                //R is largest
                if (r >= g && r >= b)
                {
                    if (b >= g)
                    {
                        h = 6.0 / 6.0 - 1.0 / 6.0 * (b - g) / (r - g);
                        s = 1.0 - g / r;
                    }
                    else
                    {
                        h = 0.0 / 6.0 + 1.0 / 6.0 * (g - b) / (r - b);
                        s = 1.0 - b / r;
                    }
                }

                //G is largest
                if (g >= r && g >= b)
                {
                    if (r >= b)
                    {
                        h = 2.0 / 6.0 - 1.0 / 6.0 * (r - b) / (g - b);
                        s = 1 - b / g;
                    }
                    else
                    {
                        h = 2.0 / 6.0 + 1.0 / 6.0 * (b - r) / (g - r);
                        s = 1.0 - r / g;
                    }
                }

                //B is largest
                if (b >= r && b >= g)
                {
                    if (g >= r)
                    {
                        h = 4.0 / 6.0 - 1.0 / 6.0 * (g - r) / (b - r);
                        s = 1.0 - r / b;
                    }
                    else
                    {
                        h = 4.0 / 6.0 + 1.0 / 6.0 * (r - g) / (b - g);
                        s = 1.0 - g / b;
                    }
                }
            }
            Value = new((h * 360.0).Round(), s * 100.0, p.Round());
        }
    }

    #endregion

    #region HWB

    [Serializable]
    public sealed class HWB : VisualColor
    {
        public override ColorModels Model => ColorModels.HWB;

        public HWB(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hwb.js</remarks>
        public override RGB Convert()
        {
            var white = Value[1] / 100;
            var black = Value[2] / 100;

            if (white + black >= 1)
            {
                var gray = white / (white + black);
                return new RGB(gray, gray, gray);
            }

            double[] rgb = HSL.From(new HSL(Value[0], 100, 50)).Value;
            for (var i = 0; i < 3; i++)
            {
                rgb[i] *= (1 - white - black);
                rgb[i] += white;
            }
            return new(rgb);
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hwb.js</remarks>
        public override void Convert(RGB input)
        {
            var hsl = HSL.From(input);
            var white = Math.Min(input[0], Math.Min(input[1], input[2]));
            var black = 1 - Math.Max(input[0], Math.Max(input[1], input[2]));
            Value = new(hsl[0], white * 100, black * 100);
        }
    }

    #endregion

    ///HUV

    #region HUV

    /// <summary>
    /// An intermediate color space used to convert to the relevant color spaces and back (<see cref="HUVcv"/>, <see cref="HUVcy"/>, <see cref="HUVsp"/> | <see cref="HUV"/> | <see cref="HCV"/>, <see cref="HCY"/>, <see cref="HSP"/>).
    /// </summary>
    /// <author>Imagin</author>
    [Hidden]
    [Serializable]
    public abstract class HUV : VisualColor
    {
        public HUV(params double[] input) : base(input) { }

        protected Vector FromHUV(Vector input)
        {
            double l = input[0], c = input[1], h = input[2];
            double s; double b;

            //(1) Decrease range to a desired subrange
            l = l / 100 * 50 + 50;
            c = c / 100 * 25 + 75;

            double cr = c / 100 * 2 * Math.PI;
            double lr = l / 100 * 2 * Math.PI;

            //(2) Solve for {h} using {c} and {l}
            h = 359 - (((l / 100) * (c / 100) * h) % 359);

            //(3) Solve for {s}
            s = ((c * Math.Sin(cr)) + 140) / (122 + 140) * 100 * 1.5;

            //(4) Solve for {b}
            b = ((l * Math.Cos(lr)) + 134) / (224 + 134) * 100 * 1.5;

            return new Vector(h, s, b / 100 * 255);
        }

        protected Vector ToHUV(Vector input)
        {
            double l = 0, c = 0, h = 0;

            //(1) Solve for {c}
            //s = ((c * Math.Sin(cr)) + 140) / (122 + 140) * 100 * 1.5;
            //How do we solve for {c}?

            //(2) Solve for {l}
            //b = (((l * Math.Cos(lr)) + 134) / (224 + 134) * 100 * 1.5) / 100 * 255;
            //How do we solve for {l}?
            //var l = ((b / 255 * 100 / 1.5 / 100) * (224 + 134)) - 134 = l * Math.Cos(l / 100 * 2 * Math.PI)

            //(3) Solve for {h} using {c} and {l}
            //h = 359 - (((l / 100) * (c / 100) * h) % 359);
            //How do we solve for {h}?

            //(4) Convert {l} and {c} to original range
            l = (l - 50) / 50 * 100;
            c = (c - 75) / 25 * 100;
            return new Vector(l, c, h);
        }
    }

    #endregion

    #region HUVcv

    /// <summary>
    /// An experimental derivative of <see cref="HCV"/>.
    /// </summary>
    /// <author>Imagin</author>
    [Serializable]
    public sealed class HUVcv : HUV
    {
        public override ColorModels Model => ColorModels.HUVcv;

        public HUVcv(params double[] input) : base(input) { }

        public override RGB Convert()
        {
            Vector tlc = ToHUV(this);
            return new HCV(tlc[0], tlc[1], tlc[2] / 255 * 100).Convert();
        }

        public override void Convert(RGB input)
        {
            var hcv = new HCV();
            hcv.Convert(input);
            hcv[2] = hcv[2] / 100 * 255;
            Value = FromHUV(hcv);
        }
    }

    #endregion

    #region HUVcy

    /// <summary>
    /// An experimental derivative of <see cref="HCY"/>.
    /// </summary>
    /// <author>Imagin</author>
    [Serializable]
    public sealed class HUVcy : HUV
    {
        public override ColorModels Model => ColorModels.HUVcy;

        public HUVcy(params double[] input) : base(input) { }

        public override RGB Convert()
        {
            Vector tlc = ToHUV(this);
            return new HCY(tlc).Convert();
        }

        public override void Convert(RGB input)
        {
            var hcy = new HCY();
            hcy.Convert(input);
            Value = FromHUV(hcy);
        }
    }

    #endregion

    #region HUVsp

    /// <summary>
    /// An experimental derivative of <see cref="HSP"/>.
    /// </summary>
    /// <author>Imagin</author>
    [Serializable]
    public sealed class HUVsp : HUV
    {
        public override ColorModels Model => ColorModels.HUVsp;

        public HUVsp(params double[] input) : base(input) { }

        public override RGB Convert()
        {
            Vector tlc = ToHUV(this);
            return new HSP(tlc).Convert();
        }

        public override void Convert(RGB input)
        {
            var hsp = new HSP();
            hsp.Convert(input);
            Value = FromHUV(hsp);
        }
    }

    #endregion

    ///LAB

    #region LAB (1976)

    /// <summary>
    /// <para><see cref="CIE"/> / LAB (1976)</para>
    /// A color space defined by <see cref="CIE"/> in 1976. (Referring to CIELAB as "Lab" without asterisks should be avoided to prevent confusion with Hunter Lab.) It expresses color as three values: L* for perceptual lightness, and a* and b* for the four unique colors of human vision: red, green, blue, and yellow. CIELAB was intended as a perceptually uniform space, where a given numerical change corresponds to a similar perceived change in color. While the LAB space is not truly perceptually uniform, it nevertheless is useful in industry for detecting small differences in color.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/CIELAB_color_space</remarks>
    [Serializable]
    public sealed class LAB : VisualColor
    {
        public override ColorModels Model => ColorModels.LAB;

        public LAB(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lab.js</remarks>
        public override RGB Convert()
        {
            var xyz = new XYZ();

            xyz[1] = (this[0] + 16.0f) / 116.0f;
            xyz[0] = (this[1] / 500.0f) + xyz[1];
            xyz[2] = xyz[1] - (this[2] / 200.0f);

            for (int i = 0; i < 3; i++)
            {
                double pow = xyz[i] * xyz[i] * xyz[i];
                double ratio = (6.0f / 29.0f);
                if (xyz[i] > ratio)
                {
                    xyz[i] = pow;
                }
                else
                {
                    xyz[i] = (3.0f * (6.0f / 29.0f) * (6.0f / 29.0f) * (xyz[i] - (4.0f / 29.0f)));
                }
            }

            xyz[0] = xyz[0] * Illuminants.D65[0];
            xyz[1] = xyz[1] * Illuminants.D65[1];
            xyz[2] = xyz[2] * Illuminants.D65[2];
            return xyz.Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lab.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double x = xyz[0], y = xyz[1], z = xyz[2];
            double l, a, b;

            x /= 95.047;
            y /= 100;
            z /= 108.883;

            x = x > 0.008856 ? Math.Pow(x, 1 / 3) : (7.787 * x) + (16 / 116);
            y = y > 0.008856 ? Math.Pow(y, 1 / 3) : (7.787 * y) + (16 / 116);
            z = z > 0.008856 ? Math.Pow(z, 1 / 3) : (7.787 * z) + (16 / 116);

            l = (116 * y) - 16;
            a = 500 * (x - y);
            b = 200 * (y - z);

            Value = new(l, a, b);
        }
    }

    #endregion

    #region LABh

    [Serializable]
    public sealed class LABh : VisualColor
    {
        public override ColorModels Model => ColorModels.LABh;

        public LABh(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/labh.js</remarks>
        public override RGB Convert()
        {
            double l = this[0], a = this[1], b = this[2];

            double _y = l / 10;
            double _x = a / 17.5 * l / 10;
            double _z = b / 7 * l / 10;

            double y = _y * _y;
            double x = (_x + y) / 1.02;
            double z = -(_z - y) / 0.847;
            return new XYZ(x, y, z).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/labh.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double x = xyz[0], y = xyz[1], z = xyz[2];

            var _y12 = Math.Sqrt(y);
            var l = 10 * _y12;
            var a = y == 0 ? 0 : 17.5 * (((1.02 * x) - y) / _y12);
            var b = y == 0 ? 0 : 7 * ((y - (0.847 * z)) / _y12);

            Value = new(l, a, b);
        }
    }

    #endregion

    #region LCHab

    [Serializable]
    public sealed class LCHab : VisualColor
    {
        public override ColorModels Model => ColorModels.LCHab;

        public LCHab(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lchab.js</remarks>
        public override RGB Convert()
        {
            double l = this[0], c = this[1], h = this[2];
            double a, b, hr;

            hr = h / 360 * 2 * Math.PI;
            a = c * Math.Cos(hr);
            b = c * Math.Sin(hr);

            return new LABh(l, a, b).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lchab.js</remarks>
        public override void Convert(RGB input)
        {
            var lab = new LABh();
            lab.Convert(input);

            double l = lab[0], a = lab[1], b = lab[2], hr, h, c;

            hr = Math.Atan2(b, a);
            h = hr * 360 / 2 / Math.PI;
            if (h < 0)
                h += 360;

            c = Math.Sqrt(a * a + b * b);
            Value = new(l, c, h);
        }
    }

    #endregion

    ///LUV

    #region HPLuv [*]

    [Serializable]
    public sealed class HPLuv : VisualColor
    {
        public override ColorModels Model => default; //ColorModels.HPLuv;

        public HPLuv(params double[] input) : base(input) { }

        /// <remarks>Needs checked!</remarks>
        public static double M(double a0)
        {
            /*
            a = f(a); 
            for (var c = Infinity, b = 0; b < a.length;) 
            { 
                var d = a[b]; 
                ++b; 

                c = Math.min(c, Math.abs(d.a) / Math.sqrt(Math.pow(d.b, 2) + 1))
            }
            */
            return default; //c;
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hpluv.js</remarks>
        public override RGB Convert()
        {
            //HPLuv -> LCHuv
            LCHuv lchuv = null;

            double c = this[0], b = this[1], a = this[2];
            if (99.9999999 < a)
            {
                lchuv = new(100, 0, c);
            }
            else if (1E-8 > a)
            {
                lchuv = new(0, 0, c);
            }
            else
            {
                b = M(a) / 100 * b;
                lchuv = new LCHuv(a, b, c);
            }

            //LCHuv -> RGB
            return lchuv.Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hpluv.js</remarks>
        public override void Convert(RGB input)
        {
            //RGB -> (XYZ) -> LCHuv
            var lchuv = new LCHuv();
            lchuv.Convert(input);

            //LCHuv -> HPLuv
            double c = lchuv[0], b = lchuv[1], a = lchuv[2];
            if (99.9999999 < c)
            {
                Value = new(a, 0, 100);
            }
            else if (1E-8 > c)
            {
                Value = new(a, 0, 0);
            }
            else
            {
                var d = M(c);
                Value = new(a, b / d * 100, c);
            }
        }
    }

    #endregion

    #region HSLuv [*]

    [Serializable]
    public sealed class HSLuv : VisualColor
    {
        public override ColorModels Model => default; //ColorModels.HSLuv;

        public HSLuv(params double[] input) : base(input) { }

        /// <remarks>Needs checked!</remarks>
        public static double N(double a0, double c0)
        {
            /*
            c0 = c0 / 360 * Math.PI * 2; a0 = f(a0); 
            for (var b = Infinity, d = 0; d < a0.length;) 
            { 
                var e = a0[d]; 
                ++d; 

                e = e.a / (Math.Sin(c0) - e.b * Math.Cos(c0));
                0 <= e && (b = Math.Min(b, e));
            }
            */
            return default; //b;
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsluv.js</remarks>
        public override RGB Convert()
        {
            //HSLuv -> LCHuv
            LCHuv lchuv = null;

            double c = this[0], b = this[1], a = this[2];
            if (99.9999999 < a)
            {
                lchuv = new(100, 0, c);
            }
            else if (1E-8 > a)
            {
                lchuv = new(0, 0, c);
            }
            else
            {
                b = N(a, c) / 100 * b;
                lchuv = new(a, b, c);
            }

            //LCH -> RGB
            return lchuv.Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/hsluv.js</remarks>
        public override void Convert(RGB input)
        {
            //RGB -> (XYZ) -> LCHuv
            var lchuv = new LCHuv();
            lchuv.Convert(input);

            //LCHuv -> HSLuv
            double c = lchuv[0], b = lchuv[1], a = lchuv[2];
            if (99.9999999 < c)
            {
                Value = new(a, 0, 100);
            }
            else if (1E-8 > c)
            {
                Value = new(a, 0, 0);
            }
            else
            {
                var d = N(c, a);
                Value = new(a, b / d * 100, c);
            }
        }
    }

    #endregion

    #region LCHuv [*]

    [Serializable]
    public sealed class LCHuv : VisualColor
    {
        public override ColorModels Model => ColorModels.LCHuv;

        public LCHuv(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lchuv.js</remarks>
        public override RGB Convert()
        {
            double l = this[0], c = this[1], h = this[2];
            double u; double v; double hr;

            hr = h / 360 * 2 * Math.PI;
            u = c * Math.Cos(hr);
            v = c * Math.Sin(hr);

            return new LUV(l, u, v).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lchuv.js</remarks>
        public override void Convert(RGB input)
        {
            var luv = new LUV();
            luv.Convert(input);

            double l = luv[0], u = luv[1], v = luv[2];

            var c = Math.Sqrt(u * u + v * v);
            var hr = Math.Atan2(v, u);
            var h = hr * 360 / 2 / Math.PI;
            if (h < 0)
                h += 360;

            Value = new(l, c, h);
        }
    }

    #endregion

    #region LUV (1976) [*]

    /// <summary>
    /// <para><see cref="CIE"/> / LUV (1976)</para>
    /// A color space adopted by <see cref="CIE"/> in 1976, as a simple-to-compute transformation of <see cref="XYZ"/>, but which attempted perceptual uniformity. It is extensively used for applications such as computer graphics which deal with colored lights. Although additive mixtures of different colored lights will fall on a line in the uniform chromaticity diagram, such additive mixtures will not, contrary to popular belief, fall along a line in this color space unless the mixtures are constant in lightness.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/CIELUV</remarks>
    [Serializable]
    public sealed class LUV : VisualColor
    {
        public override ColorModels Model => ColorModels.LUV;

        public LUV(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/luv.js</remarks>
        public override RGB Convert()
        {
            #region Source (DO NOT MODIFY)
            /*
            export default {
	            //NOTE: luv has no rigidly defined limits
	            //easyrgb fails to get proper coords
	            //boronine states no rigid limits
	            //colorMine refers this ones:
	            min: [0,-134,-140],
	            max: [100,224,122],
	            alias: ['LUV', 'cieluv', 'cie1976'],

	            xyz: function(arg, i, o){
		            var _u, _v, l, u, v, x, y, z, xn, yn, zn, un, vn;
		            l = arg[0], u = arg[1], v = arg[2];

		            if (l === 0) return [0,0,0];

		            //get constants
		            //var e = 0.008856451679035631; //(6/29)^3
		            var k = 0.0011070564598794539; //(3/29)^3

		            //get illuminant/observer
		            i = i || 'D65';
		            o = o || 2;

		            xn = xyz.whitepoint[o][i][0];
		            yn = xyz.whitepoint[o][i][1];
		            zn = xyz.whitepoint[o][i][2];

		            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
		            vn = (9 * yn) / (xn + (15 * yn) + (3 * zn));
		            // un = 0.19783000664283;
		            // vn = 0.46831999493879;

		            _u = u / (13 * l) + un || 0;
		            _v = v / (13 * l) + vn || 0;

		            y = l > 8 ? yn * Math.pow( (l + 16) / 116 , 3) : yn * l * k;

		            //wikipedia method
		            x = y * 9 * _u / (4 * _v) || 0;
		            z = y * (12 - 3 * _u - 20 * _v) / (4 * _v) || 0;

		            //boronine method
		            //https://github.com/boronine/husl/blob/master/husl.coffee#L201
		            // x = 0 - (9 * y * _u) / ((_u - 4) * _v - _u * _v);
		            // z = (9 * y - (15 * _v * y) - (_v * x)) / (3 * _v);

		            return [x, y, z];
	            }
            };

            // http://www.brucelindbloom.com/index.html?Equations.html
            // https://github.com/boronine/husl/blob/master/husl.coffee
            xyz.luv = function(arg, i, o) {
	            var _u, _v, l, u, v, x, y, z, xn, yn, zn, un, vn;

	            //get constants
	            var e = 0.008856451679035631; //(6/29)^3
	            var k = 903.2962962962961; //(29/3)^3

	            //get illuminant/observer coords
	            i = i || 'D65';
	            o = o || 2;

	            xn = xyz.whitepoint[o][i][0];
	            yn = xyz.whitepoint[o][i][1];
	            zn = xyz.whitepoint[o][i][2];

	            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
	            vn = (9 * yn) / (xn + (15 * yn) + (3 * zn));

	            x = arg[0], y = arg[1], z = arg[2];

	            _u = (4 * x) / (x + (15 * y) + (3 * z)) || 0;
	            _v = (9 * y) / (x + (15 * y) + (3 * z)) || 0;

	            var yr = y/yn;

	            l = yr <= e ? k * yr : 116 * Math.pow(yr, 1/3) - 16;

	            u = 13 * l * (_u - un);
	            v = 13 * l * (_v - vn);

	            return [l, u, v];
            };
            */
            #endregion

            #region Old code
            var xyz = new XYZ();

            //Standard
            double e = 0.008856;
            //Intent
            //e = 216 / 24389;

            //Standard
            double k = 903.3;
            //Intent
            //k = 24389 / 27;

            if (this[0] > k * e)
            {
                xyz[1] = Math.Pow((this[0] + 16) / 116, 3);
            }
            else
            {
                xyz[1] = this[0] / k;
            }

            double u0 = (4 * Illuminants.D65[0]) / (Illuminants.D65[0] + (15 * Illuminants.D65[1]) + (3 * Illuminants.D65[2]));
            double v0 = (9 * Illuminants.D65[1]) / (Illuminants.D65[0] + (15 * Illuminants.D65[1]) + (3 * Illuminants.D65[2]));

            double a, b, c, d;
            a = 1 / 3 * (((52 * this[0]) / (this[1] + (13 * this[0] * u0))) - 1);
            b = -5 * xyz[1];
            c = -1 / 3;
            d = xyz[1] * (((39 * this[0]) / (this[2] + (13 * this[0] * v0))) - 5);

            xyz[0] = a - c == 0 ? 0 : (d - b) / (a - c);
            xyz[2] = (xyz[0] * a) + b;

            xyz[0] = xyz[0] * Illuminants.D65[0];
            xyz[1] = xyz[1] * Illuminants.D65[1];
            xyz[2] = xyz[2] * Illuminants.D65[2];
            return xyz.Convert();
            #endregion
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/luv.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double _u, _v, l, u, v, x, y, z, xn, yn, zn, un, vn;

            //get illuminant/observer coords
            var i = Illuminants.D65;
            xn = Illuminants.D65[0];
            yn = Illuminants.D65[1];
            zn = Illuminants.D65[2];

            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
            vn = (9 * yn) / (xn + (15 * yn) + (3 * zn));

            x = xyz[0]; y = xyz[1]; z = xyz[2];

            var uv = x + (15 * y) + (3 * z);
            _u = uv == 0 ? 0 : (4 * x) / uv;
            _v = uv == 0 ? 0 : (9 * y) / uv;

            var yr = y / yn;

            l = yr <= CIE.IEpsilon ? CIE.IKappa * yr : 116 * Math.Pow(yr, 1 / 3) - 16;
            u = 13 * l * (_u - un);
            v = 13 * l * (_v - vn);

            Value = new(l, u, v);
        }
    }

    #endregion

    ///TSL 

    #region TSL

    [Serializable]
    public sealed class TSL : VisualColor
    {
        public override ColorModels Model => ColorModels.TSL;

        public TSL(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/tsl.js</remarks>
        public override RGB Convert()
        {
            double T = this[0], S = this[1], L = this[2];

            //wikipedia solution
            /*
            // var x = - 1 / Math.tan(Math.PI * 2 * T);
            var x = -Math.sin(2*Math.PI*T);
            if ( x != 0 ) x = Math.cos(2*Math.PI*T)/x;
            var g = T > .5 ? -S * Math.sqrt( 5 / (9 * (x*x + 1)) ) :
                    T < .5 ? S * Math.sqrt( 5 / (9 * (x*x + 1)) ) : 0;
            var r = T === 0 ? 0.7453559 * S : (x * g + 1/3);
            var R = k * r, G = k * g, B = k * (1 - r - g);
            */

            double x = Math.Tan(2 * Math.PI * (T - 1 / 4));
            x = x * x;

            double r = Math.Sqrt(5 * S * S / (9 * (1 / x + 1))) + 1 / 3;
            double g = Math.Sqrt(5 * S * S / (9 * (x + 1))) + 1 / 3;

            double k = L / (.185 * r + .473 * g + .114);

            double B = k * (1 - r - g);
            double G = k * g;
            double R = k * r;

            return new(R, G, B);
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/tsl.js</remarks>
        public override void Convert(RGB input)
        {
            var r = input[0] / 255;
            var g = input[1] / 255;
            var b = input[2] / 255;

            var sum = r + g + b;
            double r_ = (sum == 0 ? 0 : r / sum) - 1 / 3, g_ = (sum == 0 ? 0 : g / sum) - 1 / 3;

            var T = g_ > 0 ? .5 * Math.Atan(r_ / g_) / Math.PI + .25 : g_ < 0 ? .5 * Math.Atan(r_ / g_) / Math.PI + .75 : 0;
            var S = Math.Sqrt(9 / 5 * (r_ * r_ + g_ * g_));
            var L = (r * 0.299) + (g * 0.587) + (b * 0.114);
            Value = new(T, S, L);
        }
    }

    #endregion

    ///XYZ

    #region LMS

    [Serializable]
    public sealed class LMS : VisualColor
    {
        public override ColorModels Model => ColorModels.LMS;

        public LMS(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lms.js</remarks>
        public override RGB Convert()
        {
            double l = this[0], m = this[1], s = this[2];

            var a = new double[3] { +1.096123820835514, -0.278869000218287, +0.182745179382773 };
            var b = new double[3] { +0.454369041975359, +0.473533154307412, +0.072097803717229 };
            var c = new double[3] { -0.009627608738429, -0.005698031216113, +1.015325639954543 };

            double x = l * a[0] + m * a[1] + s * a[2];
            double y = l * b[0] + m * b[1] + s * b[2];
            double z = l * c[0] + m * c[1] + s * c[2];
            return new XYZ(x, y, z).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/lms.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double x = xyz[0], y = xyz[1], z = xyz[2];

            var matrix = LMSTransformMatrix.VonKriesHPEAdjusted;
            Value = new
            (
                x * matrix[0][0] + y * matrix[0][1] + z * matrix[0][2],
                x * matrix[1][0] + y * matrix[1][1] + z * matrix[1][2],
                x * matrix[2][0] + y * matrix[2][1] + z * matrix[2][2]
            );
        }
    }

    #endregion

    #region UCS (1960)

    /// <summary>
    /// <para><see cref="CIE"/> / UCS (1960)</para>
    /// Mostly used to calculate correlated color temperature, where the isothermal lines are perpendicular to the Planckian locus. As a uniform chromaticity space, it has been superseded by the <see cref="LUV"/>.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/CIE_1960_color_space</remarks>
    [Serializable]
    public sealed class UCS : VisualColor
    {
        public override ColorModels Model => ColorModels.UCS;

        public UCS(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/ucs.js</remarks>
        public override RGB Convert()
        {
            double u = this[0], v = this[1], w = this[2];
            return new XYZ(1.5 * u, v, 1.5 * u - 3 * v + 2 * w).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/ucs.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double x = xyz[0], y = xyz[1], z = xyz[2];

            Value = new
            (
                x * 2 / 3,
                y,
                0.5 * (-x + 3 * y + z)
            );
        }
    }

    #endregion

    #region UVW (1964)

    /// <summary>
    /// <para><see cref="CIE"/> / UVW (1964)</para>
    /// Wyszecki invented the UVW color space in order to be able to calculate color differences without having to hold the luminance constant. He defined a lightness index W* by simplifying expressions suggested earlier by Ladd and Pinney, and Glasser et al.. The chromaticity components U* and V* are defined such that the white point maps to the origin, as in Adams chromatic valence color spaces. This arrangement has the benefit of being able to express the loci of chromaticities with constant saturation simply as (U*)2 + (V*)2 = C for a constant C. Furthermore, the chromaticity axes are scaled by the lightness "so as to account for the apparent increase or decrease in saturation when the lightness index is increased or decreased, respectively, and the chromaticity (u, v) is kept constant".
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/CIE_1964_color_space</remarks>
    [Serializable]
    public sealed class UVW : VisualColor
    {
        public override ColorModels Model => ColorModels.UVW;

        public UVW(params double[] input) : base(input) { }

        /// <remarks>
        /// <para>(1) https://github.com/colorjs/color-space/blob/master/uvw.js</para>
        /// <para>(2) http://cs.haifa.ac.il/hagit/courses/ist/Lectures/IST05_ColorLABx4.pdf</para>
        /// </remarks>
        public override RGB Convert()
        {
            #region Source (DO NOT MODIFY)
            /*
            var uvw = {
	            min: [-134, -140, 0],
	            max: [224, 122, 100],
	            alias: ['UVW', 'cieuvw', 'cie1964']
            };

            uvw.xyz = function (arg, i, o) {
	            var _u, _v, w, u, v, x, y, z, xn, yn, zn, un, vn;
	            u = arg[0], v = arg[1], w = arg[2];

	            if (w === 0) return [0,0,0];

	            //get illuminant/observer
	            i = i || 'D65';
	            o = o || 2;

	            xn = xyz.whitepoint[o][i][0];
	            yn = xyz.whitepoint[o][i][1];
	            zn = xyz.whitepoint[o][i][2];

	            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
	            vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

	            y = Math.pow((w + 17) / 25, 3);

	            _u = u / (13 * w) + un || 0;
	            _v = v / (13 * w) + vn || 0;

	            x = (6 / 4) * y * _u / _v;
	            z = y * (2 / _v - 0.5 * _u / _v - 5);

	            return [x, y, z];
            };

            xyz.uvw = function (arr, i, o) {
	            var x = arr[0], y = arr[1], z = arr[2], xn, yn, zn, un, vn;

	            //find out normal source u v
	            i = i || 'D65';
	            o = o || 2;

	            xn = xyz.whitepoint[o][i][0];
	            yn = xyz.whitepoint[o][i][1];
	            zn = xyz.whitepoint[o][i][2];

	            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
	            vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

	            var _u = 4 * x / (x + 15 * y + 3 * z) || 0;
	            var _v = 6 * y / (x + 15 * y + 3 * z) || 0;

	            //calc values
	            var w = 25 * Math.pow(y, 1/3) - 17;
	            var u = 13 * w * (_u - un);
	            var v = 13 * w * (_v - vn);

	            return [u, v, w];
            };

            uvw.ucs = function(uvw) {
	            //find chromacity variables
            };

            ucs.uvw = function(ucs) {
	            // //find chromacity variables
	            // var u = U / (U + V + W);
	            // var v = V / (U + V + W);

	            // //find 1964 UVW
	            // w = 25 * Math.pow(y, 1/3) - 17;
	            // u = 13 * w * (u - un);
	            // v = 13 * w * (v - vn);
            };
            */
            #endregion

            #region Old code
            double _u, _v, w, u, v, x, y, z, xn, yn, zn, un, vn;
            u = this[0]; v = this[1]; w = this[2];

            if (w == 0)
            {
                return new((double)0, 0, 0);
            }

            //get illuminant/observer
            xn = Illuminants.D65[0].Single();
            yn = Illuminants.D65[1].Single();
            zn = Illuminants.D65[2].Single();

            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
            vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

            y = Math.Pow((w + 17f) / 25f, 3f).Single();

            _u = 13 * w == 0 ? 0 : u / (13 * w) + un;
            _v = 13 * w == 0 ? 0 : v / (13 * w) + vn;

            x = (6 / 4) * y * _u / _v;
            z = y * (2 / _v - 0.5 * _u / _v - 5).Single();

            return new XYZ(x, y, z).Convert();
            #endregion
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/uvw.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double x = xyz[0], y = xyz[1], z = xyz[2], xn, yn, zn, un, vn;

            var i = Illuminants.D65;
            xn = i[0];
            yn = i[1];
            zn = i[2];

            un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
            vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

            var uv = x + 15 * y + 3 * z;

            var _u = uv == 0 ? 0 : 4 * x / uv;
            var _v = uv == 0 ? 0 : 6 * y / uv;

            var w = 25 * Math.Pow(y, 1 / 3) - 17;
            var u = 13 * w * (_u - un);
            var v = 13 * w * (_v - vn);
            Value = new(u, v, w);
        }
    }

    #endregion

    #region xyY

    [Serializable]
    public sealed class xyY : VisualColor
    {
        public override ColorModels Model => ColorModels.xyY;

        public xyY(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/xyy.js</remarks>
        public override RGB Convert()
        {
            double x = this[0], y = this[1], Y = this[2];
            if (y == 0)
                return new((double)0, 0, 0);

            return new XYZ(x * Y / y, Y, (1 - x - y) * Y / y).Convert();
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/xyy.js</remarks>
        public override void Convert(RGB input)
        {
            var xyz = new XYZ();
            xyz.Convert(input);

            double sum, X, Y, Z;
            X = xyz[0]; Y = xyz[1]; Z = xyz[2];

            sum = X + Y + Z;
            if (sum == 0)
            {
                Value = new(.0, 0, Y);
                return;
            }
            Value = new(X / sum, Y / sum, Y);
        }
    }

    #endregion

    #region XYZ (1931)

    /// <summary>
    /// <para><see cref="CIE"/> / XYZ (1931)</para>
    /// Created by the International Commission on Illumination (CIE) in 1931. Resulted from a series of experiments done in the late 1920s by William David Wright using ten observers and John Guild using seven observers. The experimental results were combined into the specification of the CIE RGB color space, from which the CIE XYZ color space was derived.
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/CIE_1931_color_space</remarks>
    [Serializable]
    public sealed class XYZ : VisualColor
    {
        public override ColorModels Model => ColorModels.XYZ;

        public XYZ(params double[] input) : base(input) { }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/xyz.js</remarks>
        public override RGB Convert()
        {
            double x = this[0] / Illuminants.D65[0];
            double y = this[1] / Illuminants.D65[1];
            double z = this[2] / Illuminants.D65[2];

            //Linear transformation
            double r = 0; double g = 0; double b = 0;
            r = (x * 3.2409699419045210) + (y * -1.537383177570093) + (z * -0.49861076029300);
            g = (x * -0.969243636280870) + (y * 1.8759675015077200) + (z * 0.041555057407175);
            b = (x * 0.0556300796969930) + (y * -0.203976958888970) + (z * 1.056971514242878);

            //Gamma correction
            if (r > 0.0031308)
            {
                r = ((1.055 * Math.Pow(r, 1.0 / 2.4)) - 0.055);
            }
            else
            {
                r = (r * 12.92);
            }
            if (g > 0.0031308)
            {
                g = ((1.055 * Math.Pow(g, 1.0 / 2.4)) - 0.055);
            }
            else
            {
                g = (g * 12.92);
            }
            if (b > 0.0031308)
            {
                b = ((1.055 * Math.Pow(b, 1.0 / 2.4)) - 0.055);
            }
            else
            {
                b = (b * 12.92);
            }

            r = Math.Min(Math.Max(0, r), 1);
            g = Math.Min(Math.Max(0, g), 1);
            b = Math.Min(Math.Max(0, b), 1);

            return new(r, g, b);
        }

        /// <remarks>https://github.com/colorjs/color-space/blob/master/xyz.js</remarks>
        public override void Convert(RGB input)
        {
            var r = input[0] / 255;
            var g = input[1] / 255;
            var b = input[2] / 255;

            r = r > 0.04045 ? Math.Pow(((r + 0.055) / 1.055), 2.4) : (r / 12.92);
            g = g > 0.04045 ? Math.Pow(((g + 0.055) / 1.055), 2.4) : (g / 12.92);
            b = b > 0.04045 ? Math.Pow(((b + 0.055) / 1.055), 2.4) : (b / 12.92);

            var x = (r * 0.412390799265950) + (g * 0.35758433938387) + (b * 0.18048078840183);
            var y = (r * 0.212639005871510) + (g * 0.71516867876775) + (b * 0.072192315360733);
            var z = (r * 0.019330818715591) + (g * 0.11919477979462) + (b * 0.95053215224966);

            Value = new(x * Illuminants.D65[0], y * Illuminants.D65[1], z * Illuminants.D65[2]);
        }
    }

    #endregion
}