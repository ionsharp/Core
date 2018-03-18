using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RGBViewModel : ColorModel<RGB>
    {
        /// <summary>
        /// 
        /// </summary>
        public RGBViewModel() : base(new RComponent(), new GComponent(), new BComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class RGBComponent : VisualComponent<RGB>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override double Increment => 0.01;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override double Maximum => RGB.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override double Minimum => RGB.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => RGB.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => RGB.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class RComponent : RGBComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "R";
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class GComponent : RGBComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "G";
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : RGBComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "B";
        }
    }
}
