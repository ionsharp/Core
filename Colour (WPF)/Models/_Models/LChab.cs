using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LChabViewModel : ColorModel<LChab>
    {
        /// <summary>
        /// 
        /// </summary>
        public LChabViewModel() : base(new LComponent(), new CComponent(), new HComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class LChabComponent : VisualComponent<LChab>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => LChab.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => LChab.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : LChabComponent, IComponentA
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
            public override double Maximum => LChab.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChab.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : LChabComponent, IComponentB
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
            public override double Maximum => LChab.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChab.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : LChabComponent, IComponentC
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
            public override double Maximum => LChab.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => LChab.Minimum[2];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }
    }
}
