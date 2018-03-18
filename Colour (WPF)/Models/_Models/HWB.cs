using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HWBViewModel : ColorModel<HWB>
    {
        /// <summary>
        /// 
        /// </summary>
        public HWBViewModel() : base(new HComponent(), new WComponent(), new BComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HWBComponent : VisualComponent<HWB>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HWB.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HWB.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HWBComponent, IComponentA
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
            public override double Maximum => HWB.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HWB.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class WComponent : HWBComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "W";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HWB.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HWB.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : HWBComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "B";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HWB.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HWB.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}