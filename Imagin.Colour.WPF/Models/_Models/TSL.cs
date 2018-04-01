using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TSLViewModel : ColorModel<TSL>
    {
        /// <summary>
        /// 
        /// </summary>
        public TSLViewModel() : base(new TComponent(), new SComponent(), new LComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class TSLComponent : VisualComponent<TSL>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => TSL.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => TSL.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class TComponent : TSLComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "T";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => TSL.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => TSL.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : TSLComponent, IComponentB
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
            public override double Maximum => TSL.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => TSL.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : TSLComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "L";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => TSL.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => TSL.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}