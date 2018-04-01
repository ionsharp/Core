using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HSMViewModel : ColorModel<HSM>
    {
        /// <summary>
        /// 
        /// </summary>
        public HSMViewModel() : base(new HComponent(), new SComponent(), new MComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HSMComponent : VisualComponent<HSM>
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
        public sealed class HComponent : HSMComponent, IComponentA
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
            public override double Maximum => HSM.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSM.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : HSMComponent, IComponentB
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
            public override double Maximum => HSM.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSM.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class MComponent : HSMComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "M";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSM.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSM.Minimum[2];
        }
    }
}