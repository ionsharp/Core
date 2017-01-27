using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <remarks>
    /// When a component gets value, 
    /// casting the result to int throws
    /// exception for a small, but weird 
    /// range of colors (black is one of them). 
    /// Exceptions must be caught until 
    /// a solution is found.
    /// 
    /// This issue only occurs for this model.
    /// </summary>
    public class CmykModel : ColorSpaceModel<Cmyk>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Cmyk GetValue()
        {
            return new Cmyk(Components[typeof(CComponent)].Value, Components[typeof(MComponent)].Value, Components[typeof(YComponent)].Value, Components[typeof(KComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public CmykModel() : base()
        {
            Components.Add(new CComponent(this));
            Components.Add(new YComponent(this));
            Components.Add(new MComponent(this));
            Components.Add(new KComponent(this));

            Orientation = Orientation.Horizontal;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class CmykComponent : ComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public sealed override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Cmyk.MaxValue;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Cmyk.MinValue;
            }

            /// <summary>
            /// 
            /// </summary>
            public sealed override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public CmykComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : CmykComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "C";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Cmyk(Color).C;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public CComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class MComponent : CmykComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "M";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Cmyk(Color).M;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public MComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : CmykComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "Y";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Cmyk(Color).Y;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public YComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class KComponent : CmykComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "K";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Cmyk(Color).K;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public KComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }
    }
}
