using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class RGBConverter : ColorConverterBase<RGB>
    {
        static RGB Compand(Vector uncompandedVector, WorkingSpace workingSpace)
        {
            var companding = workingSpace.Companding;
            Vector compandedVector = new[]
            {
                companding.Companding(uncompandedVector[0]).Coerce(1),
                companding.Companding(uncompandedVector[1]).Coerce(1),
                companding.Companding(uncompandedVector[2]).Coerce(1)
            };
            return new RGB(compandedVector, workingSpace);
        }

        /// ------------------------------------------------------------------------------------

        public RGB Convert(CMY input) => new RGB(1.0 - input.C, 1.0 - input.M, 1.0 - input.Y);

        public RGB Convert(CMYK input)
        {
            var r = (1.0 - input.C) * (1.0 - input.K);
            var g = (1.0 - input.M) * (1.0 - input.K);
            var b = (1.0 - input.Y) * (1.0 - input.K);
            return new RGB(r, g, b);
        }

        public RGB Convert(HCG input)
        {
            double h = input.H / 360.0, c = input.C / 100.0, g = input.G / 100.0;

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

        public RGB Convert(HSB input)
        {
            var _color = input / HSB.Maximum;
            double _h = _color[0], _s = _color[1], _b = _color[2];

            double r = 0, g = 0, b = 0;

            if (_s == 0)
            {
                r = g = b = _b;
            }
            else
            {
                _h *= HSB.Maximum[0];

                //The color wheel consists of 6 sectors: Figure out which sector we're in...
                var SectorPosition = _h / 60.0;
                var SectorNumber = Math.Floor(SectorPosition).ToInt32();

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

        public RGB Convert(HSI input)
        {
            var h = input.H.Modulo(0, 360) * Math.PI / 180.0;
            var s = input.S.Coerce(100) / 100.0;
            var i = input.I.Coerce(255) / 255.0;

            var pi3 = Math.PI / 3;

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

            return new RGB(new Vector(r, g, b).Coerce(0, 1));
        }

        public RGB Convert(HSL input)
        {
            double h = input.H / 60.0, s = input.S, l = input.L;

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

        public RGB Convert(HSM input)
        {
            double x = Math.Cos(input.H);
            double w = 41.0.SquareRoot() * input.S * x;

            double a = .0, b = .0, c = .0;

            a = 3.0 / 41.0 * input.S * x;
            b = input.M;
            c = 4.0 / 861.0 * (861.0 * input.S.Power(2) * (1.0 - x.Power(2))).SquareRoot();

            var R = a + b - c;

            a = w;
            b = 23.0 * input.M;
            c = 19.0 * R;

            var G = (a + b - c) / 4.0;

            a = 11 * R;
            b = 9.0 * input.M;
            c = w;

            var B = (a - b - c) / 2.0;

            return new RGB(new Vector(R, G, B).Coerce(0, 1));
        }

        public RGB Convert(HSP input)
        {
            const double Pr = 0.299;
            const double Pg = 0.587;
            const double Pb = 0.114;

            double h = input.H / 360.0, s = input.S / 100.0, p = input.P;

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

        public RGB Convert(HWB input)
        {
            double h = input.H / 360, wh = input.W / 100, bl = input.B / 100;

            var ratio = wh + bl;

            int i;
            double v, f, n;
            double r, g, b;

            //wh + bl cant be > 1
            if (ratio > 1)
            {
                wh /= ratio;
                bl /= ratio;
            }

            i = Math.Floor(6 * h).ToInt32();
            v = 1 - bl;
            f = 6 * h - i;

            //If it is even...
            if ((i & 0x01) != 0)
                f = 1 - f;

            //Linear interpolation
            n = wh + f * (v - wh);  

            switch (i)
            {
                default:
                case 6:
                case 0: r = v; g = n; b = wh; break;
                case 1: r = n; g = v; b = wh; break;
                case 2: r = wh; g = v; b = n; break;
                case 3: r = wh; g = n; b = v; break;
                case 4: r = n; g = wh; b = v; break;
                case 5: r = v; g = wh; b = n; break;
            }

            return new RGB(r, g, b);
        }

        public RGB Convert(LinearRGB input) => Compand(input.Vector, input.WorkingSpace);

        public RGB Convert(RG input)
        {
            var R = input.R * input.g / input.G;
            var G = input.g;
            var B = (1.0 - input.R - input.G) * input.g / input.G;
            return new RGB(R, G, B);
        }

        public RGB Convert(TSL input)
        {
            double T = input.T, S = input.S, L = input.L;

            var x = -(1.0 / Math.Tan(2.0 * Math.PI * T));

            var r_ = .0;
            var g_ = .0;

            if (T > .5)
            {
                g_ = -Math.Sqrt(5.0 / (9.0 * (x.Power(2) + 1.0)));
            }
            else if (T < .5)
            {
                g_ = Math.Sqrt(5.0 / (9.0 * (x.Power(2) + 1.0)));
            }
            else if (T == 0)
            {
                g_ = 0;
            }

            if (T == 0)
            {
                r_ = (5.0.SquareRoot() / 3.0 * S).Absolute();
            }
            else
            {
                r_ = x * g_;
            }

            var r = r_ + 1.0 / 3.0;
            var g = g_ + 1.0 / 3.0;

            var k = L / (.185 * r + .473 * g + .114);

            var R = k * r;
            var G = k * g;
            var B = k * (1.0 - r - g);

            return new RGB(new Vector(R, G, B).Coerce(0, 1));
        }

        public RGB Convert(xvYCC input)
        {
            return default(RGB);
        }

        public RGB Convert(YCbCr input)
        {
            return default(RGB);
        }

        public RGB Convert(YcCbcCrc input)
        {
            return default(RGB);
        }

        public RGB Convert(YCoCg input)
        {
            double ycg = input.Y - input.Cg;
            var r = ycg + input.Co;
            var g = input.Y + input.Cg;
            var b = ycg - input.Co;
            return new RGB(r, g, b);
        }

        public RGB Convert(YDbDr input)
        {
            var r = input.Y + 0.000092303716148 * input.Db - 0.525912630661865 * input.Dr;
            var g = input.Y - 0.129132898890509 * input.Db + 0.267899328207599 * input.Dr;
            var b = input.Y + 0.664679059978955 * input.Db - 0.000079202543533 * input.Dr;
            return new RGB(r, g, b);
        }

        public RGB Convert(YES input)
        {
            var r = input.Y * 1 + input.E *  1.431 + input.S *  0.126;
            var g = input.Y * 1 + input.E * -0.569 + input.S *  0.126;
            var b = input.Y * 1 + input.E *  0.431 + input.S * -1.874;
            return new RGB(new Vector(r, g, b).Coerce(0, 1));
        }

        public RGB Convert(YIQ input)
        {
            double r, g, b;

            r = (input.Y * 1.0) + (input.I *  0.956) + (input.Q * 0.621);
            g = (input.Y * 1.0) + (input.I * -0.272) + (input.Q * -0.647);
            b = (input.Y * 1.0) + (input.I * -1.108) + (input.Q * 1.705);

            r = Math.Min(Math.Max(0, r), 1);
            g = Math.Min(Math.Max(0, g), 1);
            b = Math.Min(Math.Max(0, b), 1);

            return new RGB(new Vector(r, g, b).Coerce(0, 1));
        }

        public RGB Convert(YPbPr input)
        {
            return default(RGB);
        }

        public RGB Convert(YUV input)
        {
            var Y = input.Y;
            var u = input.U;
            var v = input.V;

            var R = Y + 1.140 * v;
            var G = Y - 0.395 * u - 0.581 * v;
            var B = Y + 2.032 * u;

            return new RGB(new Vector(R, G, B).Coerce(0, 1));
        }
}
#pragma warning restore 1591
}