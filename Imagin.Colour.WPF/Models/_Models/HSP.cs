using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HSPViewModel : ColorModel<HSP>
    {
        /// <summary>
        /// 
        /// </summary>
        public HSPViewModel() : base(new HComponent(), new SComponent(), new PComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HSPComponent : VisualComponent<HSP>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HSP.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HSP.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HSPComponent, IComponentA
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
            public override double Maximum => HSP.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSP.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : HSPComponent, IComponentB
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
            public override double Maximum => HSP.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSP.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class PComponent : HSPComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "P";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSP.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSP.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}