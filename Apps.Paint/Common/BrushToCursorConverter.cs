using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    public class BrushToBitmapConverter : MultiConverter<Cursor>
    {
        public static readonly BrushToBitmapConverter Default = new();
        public BrushToBitmapConverter() : base() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 3)
            {
                if (values[0] is Brush brush)
                {
                    if (values[1] is int oldSize)
                    {
                        if (values[2] is double zoom)
                        {
                            var fill = Colors.Transparent;
                            if (values.Length >= 4)
                            {
                                if (values[3] is Color newFill)
                                    fill = newFill;
                            }

                            oldSize = (oldSize * zoom).Int32();
                            var xy1 = oldSize - 1; var xy2 = oldSize - 2;

                            var newSize = oldSize;

                            WriteableBitmap icon = null;
                            var iconOffset = 1;

                            if (values.Length == 4)
                            {
                                if (values[3] is Uri iconUri)
                                {
                                    icon = iconUri.GetImage().As<BitmapSource>().Bitmap(ImageExtensions.Png).WriteableBitmap();
                                    icon = icon.PixelHeight != icon.PixelWidth ? null : icon;

                                    newSize += icon != null ? iconOffset + icon.PixelHeight : 0;
                                }
                            }

                            double oldOpacity = 1;
                            if (values.Length >= 5)
                            {
                                if (values[4] is double newOpacity)
                                    oldOpacity = newOpacity;

                                fill = fill.A((oldOpacity * 255).Byte());
                            }

                            var result = BitmapFactory.New(newSize, newSize);
                            if (brush is CircleBrush)
                            {
                                result.FillEllipse(0, 0, xy1, xy1, fill, BlendModes.Normal);
                                result.DrawEllipse(0, 0, xy1, xy1, Colors.White);
                                result.DrawEllipse(1, 1, xy2, xy2, Colors.Black);
                            }
                            else if (brush is SquareBrush)
                            {
                                result.FillRectangle(0, 0, xy1, xy1, fill, BlendModes.Normal);
                                result.DrawRectangle(0, 0, xy1, xy1, Colors.White);
                                result.DrawRectangle(1, 1, xy2, xy2, Colors.Black);
                            }
                            else if (brush is ShapeBrush shapeBrush)
                            {
                                var shape = new Shape(shapeBrush.Path);
                                shape.Scale(new(oldSize, oldSize));

                                var points = Shape.From(shape.Points);
                                result.FillPolygon(points, fill, BlendModes.Normal);
                                result.DrawPolyline(points, Colors.White, 1);
                                result.DrawPolyline(points, Colors.Black, 1);
                            }

                            if (icon != null)
                            {
                                var iconPoint = oldSize + iconOffset;
                                result.Blend(BlendModes.Normal, icon, new Point(iconPoint, iconPoint), 1);
                            }

                            return result.Bitmap(ImageExtensions.Png).Cursor(oldSize / 2, oldSize / 2).Convert();
                        }
                    }
                }
            }
            return Cursors.Arrow;
        }
    }
}