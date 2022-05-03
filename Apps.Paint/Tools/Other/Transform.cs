using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [DisplayName("Transform")]
    [Icon(App.ImagePath + "Transform.png")]
    public class TransformTool : Tool
    {
        [Hidden]
        public override Cursor Cursor => Cursors.Arrow;

        [Hidden]
        public override bool Hidden => true;

        TransformModes mode = TransformModes.Scale;
        public TransformModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Transform.png");

        static Point ShearXY(Point source, double shearX, double shearY, int offsetX, int offsetY)
        {
            Point result = new()
            {
                X = (int)(Math.Round(source.X + shearX * source.Y))
            };
            result.X -= offsetX;


            result.Y = (int)(Math.Round(source.Y + shearY * source.X));
            result.Y -= offsetY;

            return result;
        }

        public static Bitmap Shear(Bitmap sourceBitmap, double shearX, double shearY)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            int xOffset = (int)Math.Round(sourceBitmap.Width * shearX / 2.0);
            int yOffset = (int)Math.Round(sourceBitmap.Height * shearY / 2.0);

            int sourceXY = 0;
            int resultXY = 0;

            Point sourcePoint = new();
            Point resultPoint = new();

            Rectangle imageBounds = new(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            for (int row = 0; row < sourceBitmap.Height; row++)
            {
                for (int col = 0; col < sourceBitmap.Width; col++)
                {
                    sourceXY = row * sourceData.Stride + col * 4;

                    sourcePoint.X = col;
                    sourcePoint.Y = row;

                    if (sourceXY >= 0 && sourceXY + 3 < pixelBuffer.Length)
                    {
                        resultPoint = ShearXY(sourcePoint, shearX, shearY, xOffset, yOffset);

                        resultXY = resultPoint.Y * sourceData.Stride + resultPoint.X * 4;
                        if (imageBounds.Contains(resultPoint) && resultXY >= 0)
                        {
                            if (resultXY + 6 <= resultBuffer.Length)
                            {
                                resultBuffer[resultXY + 4] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY + 5] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY + 6] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY + 7] =
                                    pixelBuffer[sourceXY + 3];
                            }
                            if (resultXY - 3 >= 0)
                            {
                                resultBuffer[resultXY - 4] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY - 3] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY - 2] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY - 1] =
                                    pixelBuffer[sourceXY + 3];
                            }
                            if (resultXY + 3 < resultBuffer.Length)
                            {
                                resultBuffer[resultXY] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY + 1] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY + 2] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY + 3] =
                                    pixelBuffer[sourceXY + 3];
                            }
                        }
                    }
                }
            }

            Bitmap resultBitmap = new(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}