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
        public override Color GetColor()
        {
            return Cmyk.ToColor(this.Components[typeof(CComponent)].CurrentValue, this.Components[typeof(MComponent)].CurrentValue, this.Components[typeof(YComponent)].CurrentValue, this.Components[typeof(KComponent)].CurrentValue);
        }

        public CmykModel() : base()
        {
            this.Components.Add(new CComponent());
            this.Components.Add(new YComponent());
            this.Components.Add(new MComponent());
            this.Components.Add(new KComponent());

            this.Orientation = Orientation.Horizontal;
        }

        public sealed class CComponent : ComponentModel
        {
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

            public override string ComponentLabel
            {
                get
                {
                    return "C";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override int GetValue(Color Color)
            {
                try
                {
                    return (Cmyk.FromColor(Color).C * this.MaxValue).Round().ToInt();
                }
                catch
                {
                    return 0;
                }
            }
        }

        public sealed class MComponent : ComponentModel
        {
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

            public override string ComponentLabel
            {
                get
                {
                    return "M";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override int GetValue(Color Color)
            {
                try
                {
                    return (Cmyk.FromColor(Color).M * this.MaxValue).Round().ToInt();
                }
                catch
                {
                    return 0;
                }
            }
        }

        public sealed class YComponent : ComponentModel
        {
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

            public override string ComponentLabel
            {
                get
                {
                    return "Y";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override int GetValue(Color Color)
            {
                try
                {
                    return (Cmyk.FromColor(Color).Y * this.MaxValue).Round().ToInt();
                }
                catch
                {
                    return 0;
                }
            }
        }

        public sealed class KComponent : ComponentModel
        {
            public override bool CanSelect
            {
                get
                {
                    return false;
                }
            }

            public override string ComponentLabel
            {
                get
                {
                    return "K";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override int GetValue(Color Color)
            {
                try
                {
                    return (Cmyk.FromColor(Color).K * this.MaxValue).Round().ToInt();
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
