using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint.Effects
{
    public class PolygonEffect : BaseEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(PolygonEffect), 0);
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Color), typeof(PolygonEffect), new FrameworkPropertyMetadata(Colors.Black, PixelShaderConstantCallback(0)));
        public Color Fill
        {
            get => (Color)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty SidesProperty = DependencyProperty.Register(nameof(Sides), typeof(double), typeof(PolygonEffect), new FrameworkPropertyMetadata(6d, PixelShaderConstantCallback(1)));
        public double Sides
        {
            get => (double)GetValue(SidesProperty);
            set => SetValue(SidesProperty, value);
        }

        public static readonly DependencyProperty StarProperty = DependencyProperty.Register(nameof(Star), typeof(bool), typeof(PolygonEffect), new FrameworkPropertyMetadata(false, PixelShaderConstantCallback(2)));
        public bool Star
        {
            get => (bool)GetValue(StarProperty);
            set => SetValue(StarProperty, value);
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Color), typeof(PolygonEffect), new FrameworkPropertyMetadata(Colors.Black, PixelShaderConstantCallback(3)));
        public Color Stroke
        {
            get => (Color)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(PolygonEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(4)));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public PolygonEffect()
        {
            var pixelShader = new PixelShader { UriSource = new Uri($"{BasePath}Shapes/Polygon.ps", UriKind.Relative) };
            PixelShader = pixelShader;

            UpdateShaderValue(FillProperty);
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(SidesProperty);
            UpdateShaderValue(StarProperty);
            UpdateShaderValue(StrokeProperty);
            UpdateShaderValue(StrokeThicknessProperty);
        }
    }
}