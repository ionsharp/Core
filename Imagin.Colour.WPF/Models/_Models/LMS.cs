using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LMSViewModel : ColorModel<LMS>
    {
        /// <summary>
        /// 
        /// </summary>
        public LMSViewModel() : base(new LComponent(), new MComponent(), new SComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class LMSComponent : VisualComponent<LMS>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => LMS.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => LMS.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : LMSComponent, IComponentA
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
            public override double Maximum => LMS.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LMS.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class MComponent : LMSComponent, IComponentB
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
            public override double Maximum => LMS.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LMS.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : LMSComponent, IComponentC
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
            public override double Maximum => LMS.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LMS.Minimum[2];
        }
    }
}
