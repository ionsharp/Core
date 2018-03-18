using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HSBViewModel : ColorModel<HSB>
    {
        /// <summary>
        /// 
        /// </summary>
        public HSBViewModel() : base(new HComponent(), new SComponent(), new BComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HSBComponent : VisualComponent<HSB>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HSB.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HSB.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HSBComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            public override ComponentKind Kind => ComponentKind.Uniform;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "H";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSB.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSB.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : HSBComponent, IComponentB
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
            public override double Maximum => HSB.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSB.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : HSBComponent, IComponentC
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
            public override double Maximum => HSB.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSB.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}