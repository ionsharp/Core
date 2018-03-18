using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LuvViewModel : ColorModel<Luv>
    {
        /// <summary>
        /// 
        /// </summary>
        public LuvViewModel() : base(new LComponent(), new UComponent(), new VComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class LuvComponent : VisualComponent<Luv>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => Luv.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => Luv.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : LuvComponent, IComponentA
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
            public override double Maximum => Luv.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Luv.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class UComponent : LuvComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "u";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => Luv.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Luv.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class VComponent : LuvComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "v";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => Luv.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Luv.Minimum[2];
        }
    }
}
