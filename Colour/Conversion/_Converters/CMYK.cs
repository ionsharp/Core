using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
    /// <summary>
    /// 
    /// </summary>
    public class CMYKConverter : ColorConverterBase<CMYK>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CMYK Convert(RGB input)
        {
            var k0 = 1.0 - Math.Max(input.R, Math.Max(input.G, input.B));
            var k1 = 1.0 - k0;

            var c = (1.0 - input.R - k0) / k1;
            var m = (1.0 - input.G - k0) / k1;
            var y = (1.0 - input.B - k0) / k1;

            c = double.IsNaN(c) ? 0 : c;
            m = double.IsNaN(m) ? 0 : m;
            y = double.IsNaN(y) ? 0 : y;

            return new CMYK(c, m, y, k0);
        }
    }
}