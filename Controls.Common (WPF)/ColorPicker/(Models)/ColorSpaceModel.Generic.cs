using Imagin.Common.Drawing;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ColorSpaceModel<T> : ColorSpaceModel where T : IColor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override Color GetColor()
        {
            return GetValue().Color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract T GetValue();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return typeof(T).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceModel() : base()
        {
        }
    }
}
