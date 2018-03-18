using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HunterLabViewModel : ColorModel<HunterLab>
    {
        /// <summary>
        /// 
        /// </summary>
        public HunterLabViewModel() : base(new LComponent(), new AComponent(), new BComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class HunterLabComponent : VisualComponent<HunterLab>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => HunterLab.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => HunterLab.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : HunterLabComponent, IComponentA
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
            public override double Maximum => HunterLab.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HunterLab.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AComponent : HunterLabComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "a";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HunterLab.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HunterLab.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : HunterLabComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "b";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => HunterLab.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => HunterLab.Minimum[2];
        }
    }
}
