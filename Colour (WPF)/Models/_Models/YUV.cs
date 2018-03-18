using Imagin.Colour.Primitives;
using Imagin.Common;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class YUVViewModel : ColorModel<YUV>
    {
        /// <summary>
        /// 
        /// </summary>
        public YUVViewModel() : base(new YComponent(), new UComponent(), new VComponent()) { }

        /// <summary>
        /// 
        /// </summary>
        public abstract class YUVComponent : VisualComponent<YUV>
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override Vector GetMaximum() => YUV.Maximum;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override Vector GetMinimum() => YUV.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : YUVComponent, IComponentA
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
            public override double Maximum => YUV.Maximum[0];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YUV.Minimum[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class UComponent : YUVComponent, IComponentB
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "U";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YUV.Maximum[1];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YUV.Minimum[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class VComponent : YUVComponent, IComponentC
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string Label => "V";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Maximum => YUV.Maximum[2];

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override double Minimum => YUV.Minimum[2];
        }
    }
}
