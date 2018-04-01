using Imagin.Colour.Conversion;
using Imagin.Colour.Linq;
using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Imagin.Colour.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorTests : TestSeries
    {
        #region ColorTests

        public ColorTests(string name) : base(name) { }

        #endregion

        #region Tests

        protected override IEnumerable<Test> GetTests()
        {
            var converter = new ColorConverter();
            yield return new ConversionAccuracyTest("Conversion > Accuracy", converter);
            //yield return new RangeTest("Range", converter);
        }

        public abstract class ColorTest : Test
        {
            protected readonly ColorConverter _converter;

            protected ColorTest(string name, ColorConverter converter) : base(name) => _converter = converter;
        }

        public class ConversionAccuracyTest : ColorTest
        {
            public ConversionAccuracyTest(string name, ColorConverter converter) : base(name, converter) { }

            /// <summary>
            /// Compute accuracy by comparing <see langword="a"/> and <see langword="b"/>.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            double GetAccuracy(RGB a, RGB b)
            {
                var accuracy = 100.0;
                if (a != b)
                {
                    var ar = a.R;
                    var ag = a.G;
                    var ab = a.B;

                    var br = b.R;
                    var bg = b.G;
                    var bb = b.B;

                    var zero = 0.0000000001;
                    var fzero = new Func<double, double>((value) => value == 0 ? zero : value);

                    var pa = ar > br ? fzero(br) / fzero(ar) : fzero(ar) / fzero(br);
                    var pb = ag > bg ? fzero(bg) / fzero(ag) : fzero(ag) / fzero(bg);
                    var pc = ab > bb ? fzero(bb) / fzero(ab) : fzero(ab) / fzero(bb);

                    var tp = (pa + pb + pc) / 3.0;
                    accuracy = tp.Shift(2);
                }

                return accuracy;
            }

            /// <summary>
            /// Tests accuracy of conversion for a given color between <see cref="RGB"/> and every <see cref="ColorModels"/>.
            /// </summary>
            void Perform(RGB _color)
            {
                var colors = new IColor[]
                {
                    _color.To<CMY>(_converter),
                    _color.To<CMYK>(_converter),
                    _color.To<HCG>(_converter),
                    _color.To<HSB>(_converter),
                    _color.To<HSI>(_converter),
                    _color.To<HSL>(_converter),
                    _color.To<HSP>(_converter),
                    _color.To<HunterLab>(_converter),
                    _color.To<HWB>(_converter),
                    _color.To<Lab>(_converter),
                    _color.To<LChab>(_converter),
                    _color.To<LChuv>(_converter),
                    _color.To<LinearRGB>(_converter),
                    _color.To<LMS>(_converter),
                    _color.To<Luv>(_converter),
                    _color.To<TSL>(_converter),
                    _color.To<xvYCC>(_converter),
                    _color.To<xyY>(_converter),
                    _color.To<XYZ>(_converter),
                    _color.To<YCbCr>(_converter),
                    _color.To<YcCbcCrc>(_converter),
                    _color.To<YCoCg>(_converter),
                    _color.To<YDbDr>(_converter),
                    _color.To<YES>(_converter),
                    _color.To<YIQ>(_converter),
                    _color.To<YPbPr>(_converter),
                    _color.To<YUV>(_converter),
                };

                Console.WriteLine("Input");
                Console.WriteLine(string.Empty);

                Console.WriteLine(_color);
                Console.WriteLine(string.Empty);

                Console.WriteLine("Output");
                Console.WriteLine(string.Empty);

                foreach (var color in colors)
                {
                    Console.WriteLine(color);

                    var result = color.To<RGB>(_converter);
                    Console.WriteLine(result);

                    var accuracy = GetAccuracy(_color, result);
                    Console.WriteLine("Accuracy: {0}%".F(accuracy));
                    Console.WriteLine(string.Empty);
                }

                Console.WriteLine(Separator);
                Console.WriteLine(string.Empty);
            }

            void Perform<TColor>() where TColor : IColor
            {
                Console.WriteLine(typeof(TColor).FullName);

                double accuracy = 0;
                double length = 0;

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                for (double r = 0; r < 255; r++)
                {
                    ConsoleExtensions.ClearLine();
                    Console.Write("Step {0}/255 ({1})".F(r.ToString().PadLeft(3, '0'), stopwatch.Elapsed.GetRemaining(255, r.ToInt64()).ToShortTime()));
                    for (double g = 0; g < 255; g++)
                    {
                        for (double b = 0; b < 255; b++)
                        {
                            //The first color
                            var color_a = new RGB(r / 255.0, g / 255.0, b / 255.0);
                            //The first color, converted
                            var color_b = color_a.To<TColor>(_converter);
                            //The converted color, converted back
                            var color_c = color_b.To<RGB>(_converter);

                            //The accuracy of the conversion
                            var _accuracy = GetAccuracy(color_a, color_c);
                            _accuracy = double.IsNaN(_accuracy) ? 100 : _accuracy;

                            accuracy += _accuracy;
                            length++;
                        }
                    }
                }
                stopwatch.Stop();
                accuracy /= length;

                ConsoleExtensions.ClearLine();
                Console.WriteLine("Accuracy: {0}%".F(accuracy));
                Console.WriteLine(string.Empty);
            }

            public override object Perform()
            {
                Console.WriteLine(Name);
                Console.WriteLine(string.Empty);

                Console.WriteLine(Separator);
                Console.WriteLine(string.Empty);

                /*
                Perform<CMYK>();
                Perform<HSB>();
                Perform<HSL>();
                Perform<HunterLab>();
                Perform<Lab>();
                Perform<LChab>();
                Perform<LChuv>();
                Perform<LinearRGB>();
                Perform<LMS>();
                Perform<Luv>();
                Perform<xyY>();
                Perform<XYZ>();
                Perform<YUV>();
                */

                new[]
                {
                    new RGB(1, 1, 1),
                    new RGB(0.25, 0.5, 0.75),
                    new RGB(0.75, 0.5, 0.25),
                    new RGB(0.33, 0.99, 0.66),
                    new RGB(0.16, 0.48, 0.32),
                    new RGB(0.72, 0.48, 0.84),
                    new RGB(0, 0, 0)
                }
                .ForEach(i => Perform(i));

                return default(object);
            }
        }

        public class RangeTest : ColorTest
        {
            public RangeTest(string name, ColorConverter converter) : base(name, converter) { }

            void Perform<TColor>() where TColor : IColor
            {
                var amax = double.MinValue;
                var bmax = double.MinValue;
                var cmax = double.MinValue;

                var amin = double.MaxValue;
                var bmin = double.MaxValue;
                var cmin = double.MaxValue;

                Console.WriteLine(typeof(TColor).FullName);
                Console.WriteLine(string.Empty);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                for (var r = 0.0; r < 256; r++)
                {
                    ConsoleExtensions.ClearLine();
                    Console.Write("Step {0}/255 ({1})".F(r.ToString().PadLeft(3, '0'), stopwatch.Elapsed.GetRemaining(255, r.ToInt64()).ToShortTime()));

                    for (var g = 0.0; g < 256; g++)
                    {
                        for (var b = 0.0; b < 256; b++)
                        {
                            var result = new RGB(r / 255.0, g / 255.0, b / 255.0).To<TColor>(_converter).As<IColor>().Vector;

                            if (result[0] > amax) amax = result[0];
                            if (result[1] > bmax) bmax = result[1];
                            if (result[2] > cmax) cmax = result[2];

                            if (result[0] < amin) amin = result[0];
                            if (result[1] < bmin) bmin = result[1];
                            if (result[2] < cmin) cmin = result[2];
                        }
                    }
                }
                stopwatch.Stop();
                ConsoleExtensions.ClearLine();

                Console.WriteLine("Maximum: [{0}]".F(new Vector(VectorType.Row, amax, bmax, cmax)));

                Console.WriteLine(string.Empty);
                Console.WriteLine("Minimum: [{0}]".F(new Vector(VectorType.Row, amin, bmin, cmin)));

                Console.WriteLine(string.Empty);
                Console.WriteLine(Separator);

                Console.WriteLine(string.Empty);
            }

            public override object Perform()
            {
                Console.WriteLine(Name);
                Console.WriteLine(string.Empty);

                Console.WriteLine(Separator);
                Console.WriteLine(string.Empty);

                //Perform<HSB>();
                Perform<HSL>();
                //Perform<HunterLab>();
                //Perform<Lab>();
                /*
                Perform<LChab>();
                Perform<LChuv>();
                Perform<LinearRGB>();
                Perform<LMS>();
                Perform<Luv>();
                Perform<xyY>();
                Perform<XYZ>();
                Perform<YUV>();
                */

                return default(object);
            }
        }

        #endregion
    }
}
