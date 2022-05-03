using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class XVector
    {
        public static Point BoundSize(this System.Windows.Vector Vector, Point? Origin, Point Offset, Size MaxSize, Size Size, double Snap)
        {
            var x = Math.Round(Offset.X + Vector.X);
            var y = Math.Round(Offset.Y + Vector.Y);

            var delta_x = x;
            var delta_y = y;

            if (Origin != null)
            {
                x = Origin.Value.X + x;
                y = Origin.Value.Y + y;
            }

            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            x = (x + Size.Width) > MaxSize.Width ? MaxSize.Width - Size.Width : x;
            y = (y + Size.Height) > MaxSize.Height ? MaxSize.Height - Size.Height : y;

            x = x.NearestFactor(Snap);
            y = y.NearestFactor(Snap);

            if (Origin != null)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    y = Origin.Value.Y;
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    x = Origin.Value.X;
            }

            return new Point(x, y);
        }
    }
}