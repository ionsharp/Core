using Imagin.Common;
using System.Windows.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ColorModel<TColor> : ColorModel where TColor : IColor
    {
        /// <summary>
        /// 
        /// </summary>
        public ColorModel(params Component[] components) : base(components) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TColor GetValue()
        {
            var result = new double[Components.Count];

            for (int i = 0, count = Components.Count; i < count; i++)
                result[i] = Components[i].Value;

            return (TColor)(dynamic)(Vector)result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override Color GetColor() => default(Color); //GetValue().Color;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => typeof(TColor).ToString();
    }
}
