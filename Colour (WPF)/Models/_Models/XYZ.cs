using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class XYZViewModel : ColorModel<XYZ>
    {
        /// <summary>
        /// 
        /// </summary>
        public XYZViewModel() : base(new XComponent(), new YComponent(), new ZComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class XYZComponent : VisualComponent<XYZ>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => XYZ.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => XYZ.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class XComponent : XYZComponent, IComponentA
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "X";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => XYZ.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => XYZ.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : XYZComponent, IComponentB
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
            public override double Maximum => XYZ.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => XYZ.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class ZComponent : XYZComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "Z";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => XYZ.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => XYZ.Minimum[2];
        }
    }
}
