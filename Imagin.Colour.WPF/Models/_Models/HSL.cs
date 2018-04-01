using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HSLViewModel : ColorModel<HSL>
    {
        /// <summary>
        /// 
        /// </summary>
        public HSLViewModel() : base(new HComponent(), new SComponent(), new LComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HSLComponent : VisualComponent<HSL>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HSL.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HSL.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HSLComponent, IComponentA
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
            public override double Maximum => HSL.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSL.Minimum[0];
            
            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : HSLComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            public override double Increment => 0.01;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "S";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSL.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSL.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : HSLComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            public override double Increment => 0.01;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "L";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSL.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSL.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}
