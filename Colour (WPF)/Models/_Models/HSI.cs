using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HSIViewModel : ColorModel<HSI>
    {
        /// <summary>
        /// 
        /// </summary>
        public HSIViewModel() : base(new HComponent(), new SComponent(), new IComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HSIComponent : VisualComponent<HSI>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HSI.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HSI.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : HSIComponent, IComponentA
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
            public override double Maximum => HSI.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSI.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : HSIComponent, IComponentB
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
            public override double Maximum => HSI.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSI.Minimum[1];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class IComponent : HSIComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "I";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HSI.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HSI.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}