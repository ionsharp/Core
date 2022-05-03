using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>Pixel shader: Edge detection using a parametric, symetric, directional convolution kernel</summary>
    [Category(ImageEffectCategory.Stroke), DisplayName("Stroke")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class StrokeEffect : ImageEffect
    {
        public static readonly DependencyProperty ThreshholdProperty = DependencyProperty.Register(nameof(Threshhold), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
        /// <summary>The threshold of the edge detection.</summary>
        [Format(RangeFormat.Both)]
        [Range(0.0, 2.0, 0.01)]
        [Visible]
        public double Threshhold
        {
            get => (double)GetValue(ThreshholdProperty);
            set => SetValue(ThreshholdProperty, value);
        }

        public static readonly DependencyProperty K00Property = DependencyProperty.Register(nameof(K00), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));
        /// <summary>Kernel first column top. Default is the Sobel operator.</summary>
        [Format(RangeFormat.Both)]
        [Range(-100.0, 100.0, 0.01)]
        [Visible]
        public double K00
        {
            get => (double)GetValue(K00Property);
            set => SetValue(K00Property, value);
        }

        public static readonly DependencyProperty K01Property = DependencyProperty.Register(nameof(K01), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(2)));
        /// <summary>Kernel first column middle. Default is the Sobel operator.</summary>
        [Format(RangeFormat.Both)]
        [Range(-100.0, 100.0, 0.01)]
        [Visible]
        public double K01
        {
            get => (double)GetValue(K01Property);
            set => SetValue(K01Property, value);
        }

        public static readonly DependencyProperty K02Property = DependencyProperty.Register(nameof(K02), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
        /// <summary>Kernel first column bottom. Default is the Sobel operator.</summary>
        [Format(RangeFormat.Both)]
        [Range(-100.0, 100.0, 0.01)]
        [Visible]
        public double K02
        {
            get => (double)GetValue(K02Property);
            set => SetValue(K02Property, value);
        }

        public static readonly DependencyProperty TextureSizeProperty = DependencyProperty.Register(nameof(TextureSize), typeof(Point), typeof(StrokeEffect), new FrameworkPropertyMetadata(new Point(512D, 512D), PixelShaderConstantCallback(4)));
        /// <summary>The size of the texture.</summary>
        [Hidden]
        public Point TextureSize
        {
            get => (Point)GetValue(TextureSizeProperty);
            set => SetValue(TextureSizeProperty, value);
        }

        public static readonly DependencyProperty SizeXProperty = DependencyProperty.Register(nameof(SizeX), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(512d, OnSizeChanged));
        /// <summary>The size of the texture (X).</summary>
        [Range(1.0, 2048.0, 1.0)]
        [Visible]
        public double SizeX
        {
            get => (double)GetValue(SizeXProperty);
            set => SetValue(SizeXProperty, value);
        }

        public static readonly DependencyProperty SizeYProperty = DependencyProperty.Register(nameof(SizeY), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(512d, OnSizeChanged));
        /// <summary>The size of the texture (Y).</summary>
        [Range(1.0, 2048.0, 1.0)]
        [Visible]
        public double SizeY
        {
            get => (double)GetValue(SizeYProperty);
            set => SetValue(SizeYProperty, value);
        }

        static void OnSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.If<StrokeEffect>(i => i.TextureSize = new(i.SizeX, i.SizeY));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Color), typeof(StrokeEffect), new FrameworkPropertyMetadata(Colors.Black, PixelShaderConstantCallback(5)));
        [Visible]
        public Color Stroke
        {
            get => (Color)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(3d, PixelShaderConstantCallback(6)));
        [Visible]
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public StrokeEffect() : base()
        {
            UpdateShaderValue(K00Property);
            UpdateShaderValue(K01Property);
            UpdateShaderValue(K02Property);
            UpdateShaderValue(TextureSizeProperty);
            UpdateShaderValue(ThreshholdProperty);
            UpdateShaderValue(StrokeProperty);
            UpdateShaderValue(StrokeThicknessProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new StrokeEffect()
            {
                K00 = K00,
                K01 = K01,
                K02 = K02,
                TextureSize = TextureSize,
                Threshhold = Threshhold,
            };
        }
    }
}
/*
    [Category(ImageEffectCategory.Stroke), DisplayName("Stroke")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class StrokeEffect : ImageEffect
    {
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(StrokeEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        [Format(Common.RangeFormat.Both)]
        [Range(-8048.0, 2048.0, 1.0)]
        [Visible]
        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(StrokeEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(1)));
        [Format(Common.RangeFormat.Both)]
        [Range(-8048.0, 2048.0, 1.0)]
        [Visible]
        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(nameof(Threshold), typeof(double), typeof(StrokeEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(2)));
        [Format(Common.RangeFormat.Both)]
        [Range(-256.0, 256.0, 0.01)]
        [Visible]
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Color), typeof(StrokeEffect), new FrameworkPropertyMetadata(Colors.Red, PixelShaderConstantCallback(3)));
        [Visible]
        public Color Stroke
        {
            get => (Color)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(StrokeEffect), new FrameworkPropertyMetadata(3d, PixelShaderConstantCallback(4)));
        [Format(Common.RangeFormat.Both)]
        [Range(-500.0, 500.0, 1.0)]
        [Visible]
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public StrokeEffect() : base()
        {
            UpdateShaderValue(ImageWidthProperty);
            UpdateShaderValue(ImageHeightProperty);
            UpdateShaderValue(ThresholdProperty);
            UpdateShaderValue(StrokeProperty);
            UpdateShaderValue(StrokeThicknessProperty);
        }

        /// <remarks>
        /// https://stackoverflow.com/questions/54325000/how-to-draw-a-outline-edge-of-irregular-shape-using-c-sharp
        /// </remarks>
        public Matrix<Color> Render(WriteableBitmap input, Color color, double thickness = 1)
        {
            return default;
            var colors
                = new ColorMatrix(input);
            var points
                = new List<Point>();

            //From top
            for (int x = 0; x < input.PixelWidth; x++)
            {
                for (int y = 0; y < input.PixelHeight; y++)
                {
                    var alpha = colors[(uint)y, (uint)x].A;
                    if (alpha != 0)
                    {
                        var p = new Point(x, y);
                        if (!points.Contains(p))
                            points.Add(p);

                        break;
                    }
                }
            }

            //Helper variable for storing position of the last pixel visible from both sides or last inserted pixel
            int? lastInsertedPosition = null;

            //From right
            for (int y = 0; y < input.PixelHeight; y++)
            {
                for (int x = input.PixelWidth - 1; x >= 0; x--)
                {
                    var alpha = colors[(uint)y, (uint)x].A;
                    if (alpha != 0)
                    {
                        var p = new Point(x, y);
                        if (!points.Contains(p))
                        {
                            if (lastInsertedPosition.HasValue)
                            {
                                points.Insert(lastInsertedPosition.Value + 1, p);
                                lastInsertedPosition += 1;
                            }
                            else
                            {
                                points.Add(p);
                            }
                        }
                        else
                        {
                            //save last common pixel from visible from more than one sides
                            lastInsertedPosition = points.IndexOf(p);
                        }
                        break;
                    }
                }
            }

            lastInsertedPosition = null;
            //From bottom
            for (int x = input.PixelWidth - 1; x >= 0; x--)
            {
                for (int y = input.PixelHeight - 1; y >= 0; y--)
                {
                    var alpha = colors[(uint)y, (uint)x].A;
                    if (alpha != 0)
                    {
                        var p = new Point(x, y);
                        if (!points.Contains(p))
                        {
                            if (lastInsertedPosition.HasValue)
                            {
                                points.Insert(lastInsertedPosition.Value + 1, p);
                                lastInsertedPosition += 1;
                            }
                            else
                            {
                                points.Add(p);
                            }
                        }
                        else
                        {
                            //save last common pixel from visible from more than one sides
                            lastInsertedPosition = points.IndexOf(p);
                        }
                        break;
                    }
                }
            }

            lastInsertedPosition = null;
            //From left
            for (int y = input.PixelHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < input.PixelWidth; x++)
                {
                    var alpha = colors[(uint)y, (uint)x].A;
                    if (alpha != 0)
                    {
                        var p = new Point(x, y);
                        if (!points.Contains(p))
                        {
                            if (lastInsertedPosition.HasValue)
                            {
                                points.Insert(lastInsertedPosition.Value + 1, p);
                                lastInsertedPosition += 1;
                            }
                            else
                            {
                                points.Add(p);
                            }
                        }
                        else
                        {
                            //save last common pixel from visible from more than one sides
                            lastInsertedPosition = points.IndexOf(p);
                        }
                        break;
                    }
                }
            }

            //Added to close the loop
            points.Add(points[0]);

            input.DrawPolyline(Path.From(points), color, thickness.Int32());
        }

        public override ImageEffect Copy() => new StrokeEffect();
    }

Old
sampler2D imageSampler : register(s0);

float ImageWidth : register(c0);
float ImageHeight : register(c1);
float Threshold : register(c2);
float4 Stroke : register(c3);
float StrokeThickness : register(c4);

static const float3x3 laplace =
{
    -1.0f, -1.0f, -1.0f,
    -1.0f, 8.0f, -1.0f,
    -1.0f, -1.0f, -1.0f,
};

float grayScaleByLumino(float3 color)
{
    return (0.299 * color.r + 0.587 * color.g + 0.114 * color.b);
}

// ALSO WORKING!
//float4 OutlinesFunction3x3(float2 input, float2 pixelSize)
//{
//    float4 lum = float4(0.30, 0.59, 0.11, 1);

//    // TOP ROW
//    float s11 = dot(tex2D(imageSampler, input + float2(-pixelSize.x, -pixelSize.y)), lum); // LEFT
//    float s12 = dot(tex2D(imageSampler, input + float2(0, -pixelSize.y)), lum); // MIDDLE
//    float s13 = dot(tex2D(imageSampler, input + float2(pixelSize.x, -pixelSize.y)), lum); // RIGHT

//    // MIDDLE ROW
//    float s21 = dot(tex2D(imageSampler, input + float2(-pixelSize.x, 0)), lum); // LEFT
//    // Omit center
//    float s23 = dot(tex2D(imageSampler, input + float2(pixelSize.x, 0)), lum); // RIGHT

//    // LAST ROW
//    float s31 = dot(tex2D(imageSampler, input + float2(-pixelSize.x, pixelSize.y)), lum); // LEFT
//    float s32 = dot(tex2D(imageSampler, input + float2(0, pixelSize.y)), lum); // MIDDLE
//    float s33 = dot(tex2D(imageSampler, input + float2(pixelSize.x, pixelSize.y)), lum); // RIGHT

//    // Filter ... thanks internet
//    float t1 = s13 + s33 + (2 * s23) - s11 - (2 * s21) - s31;
//    float t2 = s31 + (2 * s32) + s33 - s11 - (2 * s12) - s13;

//    float4 col;

//    if (((t1 * t1) + (t2 * t2)) > threshold)
//    {
//        col = float4(1, 0, 0, 1);
//    }
//    else
//    {
//        col = tex2D(imageSampler, input);
//    }

//    return col;
//}

float4 GetEdgeGeorge(float2 coord, float2 pixelSize)
{
    float2 current = coord;
    float avrg = 0;

    float kernelValue;
    float4 currentColor;
    float grayScale;

    float4 result;

    current.x = coord.x - pixelSize.x;
    for (int x = 0; x < 3; x++)
    {
        current.y = coord.y - pixelSize.y;
        for (int y = 0; y < 3; y++)
        {
            kernelValue = laplace[x][y];
            grayScale = grayScaleByLumino(tex2D(imageSampler, current).rgb);
            avrg += grayScale * kernelValue;

            current.y += pixelSize.y;
        }
        current.x += pixelSize.x;
    }

    avrg = abs(avrg / 8);

    if (avrg > Threshold)
    {
        result = Stroke;
    }
    else
    {
        result = tex2D(imageSampler, coord);
    }

    return result;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 pixelSize = (1 / ImageWidth, 1 / ImageHeight);

    float2 pixel = ceil((uv / (StrokeThickness * 0.01) * 100)) / 100 * StrokeThickness * 0.01;
    return GetEdgeGeorge(uv, pixelSize);
}

*/