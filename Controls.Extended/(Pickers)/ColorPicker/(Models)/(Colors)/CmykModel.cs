using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
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
    public class CmykModel : ColorSpaceModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return Cmyk.ToColor(Components[typeof(CComponent)].Value, Components[typeof(MComponent)].Value, Components[typeof(YComponent)].Value, Components[typeof(KComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public CmykModel() : base()
        {
            Components.Add(new CComponent());
            Components.Add(new YComponent());
            Components.Add(new MComponent());
            Components.Add(new KComponent());

            Orientation = Orientation.Horizontal;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : ComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

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
            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override int GetValue(Color Color)
            {
                return (Cmyk.FromColor(Color).C * Maximum).Round().ToInt32();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class MComponent : ComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

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
            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override int GetValue(Color Color)
            {
                return (Cmyk.FromColor(Color).M * Maximum).Round().ToInt32();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : ComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

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
            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override int GetValue(Color Color)
            {
                return (Cmyk.FromColor(Color).Y * Maximum).Round().ToInt32();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class KComponent : ComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

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
            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override int GetValue(Color Color)
            {
                return (Cmyk.FromColor(Color).K * Maximum).Round().ToInt32();
            }
        }
    }
}
