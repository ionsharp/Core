using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Blur), DisplayName("Blur")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class BlurEffect : ImageEffect
    {
        #region Properties

        public static readonly DependencyProperty BiasProperty = DependencyProperty.Register(nameof(Bias), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Bias
        {
            get => (double)GetValue(BiasProperty);
            set => SetValue(BiasProperty, value);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0.0078125d, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register(nameof(Weight), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(2)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Weight
        {
            get => (double)GetValue(WeightProperty);
            set => SetValue(WeightProperty, value);
        }

        public static readonly DependencyProperty X0Property = DependencyProperty.Register(nameof(X0), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(3)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double X0
        {
            get => (double)GetValue(X0Property);
            set => SetValue(X0Property, value);
        }

        public static readonly DependencyProperty Y0Property = DependencyProperty.Register(nameof(Y0), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(4)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Y0
        {
            get => (double)GetValue(Y0Property);
            set => SetValue(Y0Property, value);
        }

        public static readonly DependencyProperty Z0Property = DependencyProperty.Register(nameof(Z0), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(5)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Z0
        {
            get => (double)GetValue(Z0Property);
            set => SetValue(Z0Property, value);
        }

        public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof(X1), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(6)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double X1
        {
            get => (double)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof(Y1), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(7)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public static readonly DependencyProperty Z1Property = DependencyProperty.Register(nameof(Z1), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(8)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Z1
        {
            get => (double)GetValue(Z1Property);
            set => SetValue(Z1Property, value);
        }

        public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof(X2), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(9)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof(Y2), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(10)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        public static readonly DependencyProperty Z2Property = DependencyProperty.Register(nameof(Z2), typeof(double), typeof(BlurEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(11)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 16.0, 0.01)]
        [Visible]
        public double Z2
        {
            get => (double)GetValue(Z2Property);
            set => SetValue(Z2Property, value);
        }

        #endregion

        #region BlurEffect

        public BlurEffect() : base()
        {
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(WeightProperty);

            UpdateShaderValue(X0Property);
            UpdateShaderValue(Y0Property);
            UpdateShaderValue(Z0Property);

            UpdateShaderValue(X1Property);
            UpdateShaderValue(Y1Property);
            UpdateShaderValue(Z1Property);

            UpdateShaderValue(X2Property);
            UpdateShaderValue(Y2Property);
            UpdateShaderValue(Z2Property);
        }

        #endregion

        #region Methods

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new BlurEffect();

        #endregion

        #region Legacy

        #region (enum) Types

        public enum Types
        {
            Mean3x3,
            Mean5x5,
            Mean7x7,
            Mean9x9,
            GaussianBlur3x3,
            GaussianBlur5x5,
            MotionBlur5x5,
            MotionBlur5x5At45Degrees,
            MotionBlur5x5At135Degrees,
            MotionBlur7x7,
            MotionBlur7x7At45Degrees,
            MotionBlur7x7At135Degrees,
            MotionBlur9x9,
            MotionBlur9x9At45Degrees,
            MotionBlur9x9At135Degrees,
            Median3x3,
            Median5x5,
            Median7x7,
            Median9x9,
            Median11x11
        }

        #endregion

        /*
        public static Bitmap Render(Bitmap input, Types blurType)
        {
            Bitmap result = null;

            switch (blurType)
            {
                case Types.Mean3x3:
                    RenderConvolution(result, Matrix.Mean3x3, 1.0 / 9.0, 0);
                    break;
                case Types.Mean5x5:
                    RenderConvolution(result, Matrix.Mean5x5, 1.0 / 25.0, 0);
                    break;
                case Types.Mean7x7:
                    RenderConvolution(result, Matrix.Mean7x7, 1.0 / 49.0, 0);
                    break;
                case Types.Mean9x9:
                    RenderConvolution(result, Matrix.Mean9x9, 1.0 / 81.0, 0);
                    break;
                case Types.GaussianBlur3x3:
                    RenderConvolution(result, Matrix.GaussianBlur3x3, 1.0 / 16.0, 0);
                    break;
                case Types.GaussianBlur5x5:
                    RenderConvolution(result, Matrix.GaussianBlur5x5, 1.0 / 159.0, 0);
                    break;
                case Types.MotionBlur5x5:
                    RenderConvolution(result, Matrix.MotionBlur5x5, 1.0 / 10.0, 0);
                    break;
                case Types.MotionBlur5x5At45Degrees:
                    RenderConvolution(result, Matrix.MotionBlur5x5At45Degrees, 1.0 / 5.0, 0);
                    break;
                case Types.MotionBlur5x5At135Degrees:
                    RenderConvolution(result, Matrix.MotionBlur5x5At135Degrees, 1.0 / 5.0, 0);
                    break;
                case Types.MotionBlur7x7:
                    RenderConvolution(result, Matrix.MotionBlur7x7, 1.0 / 14.0, 0);
                    break;
                case Types.MotionBlur7x7At45Degrees:
                    RenderConvolution(result, Matrix.MotionBlur7x7At45Degrees, 1.0 / 7.0, 0);
                    break;
                case Types.MotionBlur7x7At135Degrees:
                    RenderConvolution(result, Matrix.MotionBlur7x7At135Degrees, 1.0 / 7.0, 0);
                    break;
                case Types.MotionBlur9x9:
                    RenderConvolution(result, Matrix.MotionBlur9x9, 1.0 / 18.0, 0);
                    break;
                case Types.MotionBlur9x9At45Degrees:
                    RenderConvolution(result, Matrix.MotionBlur9x9At45Degrees, 1.0 / 9.0, 0);
                    break;
                case Types.MotionBlur9x9At135Degrees:
                    RenderConvolution(result, Matrix.MotionBlur9x9At135Degrees, 1.0 / 9.0, 0);
                    break;
                case Types.Median3x3:
                    RenderMedian(result, 3);
                    break;
                case Types.Median5x5:
                    RenderMedian(result, 5);
                    break;
                case Types.Median7x7:
                    RenderMedian(result, 7);
                    break;
                case Types.Median9x9:
                    RenderMedian(result, 9);
                    break;
                case Types.Median11x11:
                    RenderMedian(result, 11);
                    break;
            }
            return result;
        }

        public static void RenderConvolution(Bitmap input, double[,] filterMatrix, double factor = 1, int bias = 0)
        {
            double blue = 0.0, green = 0.0, red = 0.0;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;

            for (int offsetY = filterOffset; offsetY < input.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < input.Width - filterOffset; offsetX++)
                {
                    blue = 0; green = 0; red = 0;
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            blue 
                                += (double)(pixelBuffer[calcOffset]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            green 
                                += (double)(pixelBuffer[calcOffset + 1]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            red 
                                += (double)(pixelBuffer[calcOffset + 2]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blue 
                        = (factor * blue + bias).Coerce(255);
                    green 
                        = (factor * green + bias).Coerce(255);
                    red 
                        = (factor * red + bias).Coerce(255);
                }
            }
        }

        public static void RenderMedian(Bitmap input, int matrixSize)
        {
            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY < input.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < input.Width - filterOffset; offsetX++)
                {
                    neighbourPixels.Clear();
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            neighbourPixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();
                    middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);

                    var b = middlePixel[0]; var g = middlePixel[1]; var r = middlePixel[2]; var a = middlePixel[3];
                }
            }
        }
        */

        #endregion
    }
}