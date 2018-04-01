using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class YIQViewModel : ColorModel<YIQ>
    {
        /// <summary>
        /// 
        /// </summary>
        public YIQViewModel() : base(new YComponent(), new IComponent(), new QComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class YIQComponent : VisualComponent<YIQ>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => YIQ.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => YIQ.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : YIQComponent, IComponentA
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
            public override double Maximum => YIQ.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YIQ.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class IComponent : YIQComponent, IComponentB
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
            public override double Maximum => YIQ.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YIQ.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class QComponent : YIQComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Q";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YIQ.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YIQ.Minimum[2];
        }
    }
}
