using System.Drawing;

namespace Imagin.Common.Media
{
    /// <summary>
    /// 
    /// </summary>
    public struct Pixel : IPixel
    {
        readonly Color color;
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => color;
        }

        readonly int x;
        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get => x;
        }

        readonly int y;
        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get => y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Pixel(Color color, int x, int y)
        {
            this.color = color;
            this.x = x;
            this.y = y;
        }
    }
}
