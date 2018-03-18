using Imagin.Colour.Primitives;
using Imagin.Common.Media;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CMYKViewModel : ColorModel<CMYK>
    {
        /// <summary>
        /// 
        /// </summary>
        public override Orientation Orientation => Orientation.Horizontal;

        /// <summary>
        /// 
        /// </summary>
        public CMYKViewModel() : base(new CComponent(), new YComponent(), new MComponent(), new KComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class CMYKComponent : Component
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override double Maximum => CMYK.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override double Minimum => CMYK.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public sealed override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : CMYKComponent
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "C";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color) => 0;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => "Cyan";
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class MComponent : CMYKComponent
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "M";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color) => 0;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => "Magenta";
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : CMYKComponent
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Y";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color) => 0;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => "Yellow";
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class KComponent : CMYKComponent
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "K";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color) => 0;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => "Black";
        }
    }
}
