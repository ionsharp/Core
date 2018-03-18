using Imagin.Colour.Primitives;
using Imagin.Common;
using Imagin.Common.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LabViewModel : ColorModel<Lab>
    {
        /// <summary>
        /// 
        /// </summary>
        public LabViewModel() : base(new LComponent(), new AComponent(), new BComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class LabComponent : VisualComponent<Lab>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => Lab.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => Lab.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : LabComponent, IComponentA
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
            public override double Maximum => Lab.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Lab.Minimum[0];

            /// <summary>
            /// 
            /// </summary>
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AComponent : LabComponent, IComponentB
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
            public override double Maximum => Lab.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Lab.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : LabComponent, IComponentC
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
            public override double Maximum => Lab.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => Lab.Minimum[2];
        }
    }
}
