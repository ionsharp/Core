using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class xyYViewModel : ColorModel<xyY>
    {
        /// <summary>
        /// 
        /// </summary>
        public xyYViewModel() : base(new xComponent(), new yComponent(), new YComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class xyYComponent : VisualComponent<xyY>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => xyY.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => xyY.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class xComponent : xyYComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "x";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => xyY.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => xyY.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class yComponent : xyYComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "y";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => xyY.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => xyY.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : xyYComponent, IComponentC
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
            public override double Maximum => xyY.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => xyY.Minimum[2];
        }
    }
}
