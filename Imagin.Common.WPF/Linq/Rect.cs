using System.Windows;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Bound rect to given size.
        /// </summary>
        /// <param name="Rect">The rect to bind.</param>
        /// <param name="Bounds">The size of the binding.</param>
        /// <returns>A rect bound to given size.</returns>
        public static Rect CoerceSize(this Rect Rect, Size Bounds)
        {
            Rect.Width =
                Rect.Width + Rect.X > Bounds.Width
                    ? Bounds.Width - Rect.X
                    : Rect.Width;
            Rect.Width = Rect.Width.Coerce(double.MaxValue, 0);

            Rect.Height =
                Rect.Height + Rect.Y > Bounds.Height
                    ? Bounds.Height - Rect.Y
                    : Rect.Height;
            Rect.Height = Rect.Height.Coerce(double.MaxValue, 0);

            return Rect;
        }

        /// <summary>
        /// Bound rect based on given sizes.
        /// </summary>
        public static Rect CoercePoint(this Rect Rect, Size Bounds)
        {
            Rect.X = Rect.X.Coerce(double.MaxValue, 0);
            Rect.Y = Rect.Y.Coerce(double.MaxValue, 0);

            Rect.X = Rect.X.Coerce(Bounds.Width - Rect.Width, 0);
            Rect.Y = Rect.Y.Coerce(Bounds.Height - Rect.Height, 0);

            return Rect;
        }
    }
}
