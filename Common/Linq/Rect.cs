using System;
using System.Windows;

namespace Imagin.Common.Extensions
{
    public static class RectExtensions
    {
        /// <summary>
        /// Bound rect to given size.
        /// </summary>
        /// <param name="Rect">The rect to bind.</param>
        /// <param name="Size">The size of the binding.</param>
        /// <returns>A rect bound to given size.</returns>
        public static Rect BoundSize(this Rect Rect, Size Bounds)
        {
            Rect.Width =
                Rect.Width + Rect.X > Bounds.Width
                    ? Bounds.Width - Rect.X
                    : Rect.Width;
            Rect.Width = Rect.Width.Coerce(0.0, true);

            Rect.Height =
                Rect.Height + Rect.Y > Bounds.Height
                    ? Bounds.Height - Rect.Y
                    : Rect.Height;
            Rect.Height = Rect.Height.Coerce(0.0, true);

            return Rect;
        }

        /// <summary>
        /// Bound rect based on given sizes.
        /// </summary>
        public static Rect BoundPoint(this Rect Rect, Size Bounds)
        {
            Rect.X = Rect.X.Coerce(0d, true);
            Rect.Y = Rect.Y.Coerce(0d, true);

            Rect.X = Rect.X.Coerce(Bounds.Width - Rect.Width, 0d);
            Rect.Y = Rect.Y.Coerce(Bounds.Height - Rect.Height, 0d);

            return Rect;
        }
    }
}
