using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class YCoCgViewModel : ColorModel<YCoCg>
    {
        /// <summary>
        /// 
        /// </summary>
        public YCoCgViewModel() : base(new YComponent(), new CoComponent(), new CgComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class YCoCgComponent : VisualComponent<YUV>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => YCoCg.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => YCoCg.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : YCoCgComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Y";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YCoCg.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YCoCg.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CoComponent : YCoCgComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Co";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YCoCg.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YCoCg.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CgComponent : YCoCgComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Cg";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YCoCg.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YCoCg.Minimum[2];
        }
    }
}
