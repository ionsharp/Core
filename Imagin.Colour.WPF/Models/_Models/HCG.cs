using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HCGViewModel : ColorModel<HCG>
    {
        /// <summary>
        /// 
        /// </summary>
        public HCGViewModel() : base(new HComponent(), new CComponent(), new GComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HCGComponent : VisualComponent<HCG>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HCG.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HCG.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HCGComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "H";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HCG.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HCG.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : HCGComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "C";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HCG.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HCG.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class GComponent : HCGComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "G";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HCG.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HCG.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}