using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Common.Effects
{
    public abstract class BaseEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ColorModelEffect), 0);
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public abstract string FilePath { get; }

        public BaseEffect() : base()
        {
            PixelShader = new() { UriSource = new Uri($"/{InternalAssembly.Name};component/Effects/{FilePath}", UriKind.Relative) };
            UpdateShaderValue(InputProperty);
        }
    }
}