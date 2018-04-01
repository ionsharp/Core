using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LChuvViewModel : ColorModel<LChuv>
    {
        /// <summary>
        /// 
        /// </summary>
        public LChuvViewModel() : base(new LComponent(), new CComponent(), new HComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class LChuvComponent : VisualComponent<LChuv>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => LChuv.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => LChuv.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : LChuvComponent, IComponentA
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
            public override double Maximum => LChuv.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChuv.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : LChuvComponent, IComponentB
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
            public override double Maximum => LChuv.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChuv.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : LChuvComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "h";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => LChuv.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChuv.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }
    }
}
