using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class YESViewModel : ColorModel<YES>
    {
        /// <summary>
        /// 
        /// </summary>
        public YESViewModel() : base(new YComponent(), new EComponent(), new SComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class YESComponent : VisualComponent<YES>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => YES.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => YES.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : YESComponent, IComponentA
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
            public override double Maximum => YES.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YES.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EComponent : YESComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "E";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YES.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YES.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : YESComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "S";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YES.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YES.Minimum[2];
        }
    }
}
