using System.Drawing;

namespace Imagin.Common.Media
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPixel
    {
        /// <summary>
        /// 
        /// </summary>
        Color Color
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int X
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int Y
        {
            get;
        }
    }
}
