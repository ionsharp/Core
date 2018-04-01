using Imagin.Colour.Conversion;

namespace Imagin.Colour.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class IColorExtensions
    {
        /*
        public static Vector Format(this IColor source, RangeFormat format)
        {
            var _vector = source.Vector;
            double _a = _vector[0], _b = _vector[1], _c = _vector[2];

            var maximum = source.Maximum;
            var minimum = source.Minimum;

            var ma = minimum[0].Abs();
            var mb = minimum[1].Abs();
            var mc = minimum[2].Abs();

            switch (format)
            {
                case RangeFormat.Nominal:
                    var ra = maximum[0] + ma;
                    var rb = maximum[1] + mb;
                    var rc = maximum[2] + mc;

                    _a = (_a + ma) / ra;
                    _b = (_b + mb) / rb;
                    _c = (_c + mc) / rc;
                    break;
                case RangeFormat.ZeroBased:
                    _a = _a + ma;
                    _b = _b + mb;
                    _c = _c + mc;
                    break;
            }

            return new[] { _a, _b, _c };
        }
        */

        /// <summary>
        /// Converts the <see cref="IColor"/> to a color of type <see langword="TOutput"/> : <see cref="IColor"/> using the given <see cref="ColorConverter"/> (supports chaining).
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="converter"></param>
        /// <example>
        /// var converter = new ColorConverter();
        /// var result = new RGB(1, 1, 1).Convert[LinearRGB](converter).Convert[XYZ](converter);
        /// </example>
        /// <returns></returns>
        public static TOutput To<TOutput>(this IColor input, ColorConverter converter) where TOutput : IColor => converter.Convert<TOutput>(input);
    }
}
