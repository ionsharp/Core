using Imagin.Colour.Primitives;
using Imagin.Common.Linq;
using System.Windows.Media;

namespace Imagin.Colour.Controls.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class RGBExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color ToColor(this RGB input, byte alpha = 255)
        {
            var _input = input.Vector.Multiply(255).Round().Transform((index, value) => value.ToByte());
            return Color.FromArgb(alpha, _input[0], _input[1], _input[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workingSpace"></param>
        /// <returns></returns>
        public static RGB ToRGB(this Color input, WorkingSpace workingSpace = default(WorkingSpace))
        {
            return new RGB(input.R.ToDouble() / 255, input.G.ToDouble() / 255.0, input.B.ToDouble() / 255.0, workingSpace);
        }
    }
}
