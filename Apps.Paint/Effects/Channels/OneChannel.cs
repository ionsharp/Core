using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint.Effects
{
    public class OneChannelEffect : BaseEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(OneChannelEffect), 0);
        public System.Windows.Media.Brush Input
        {
            get => (System.Windows.Media.Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty GreyProperty = DependencyProperty.Register("Grey", typeof(double), typeof(OneChannelEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public double Grey
        {
            get => (double)GetValue(GreyProperty);
            set => SetValue(GreyProperty, value);
        }

        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register("Channel", typeof(double), typeof(OneChannelEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public double Channel
        {
            get => (double)GetValue(ChannelProperty);
            set => SetValue(ChannelProperty, value);
        }

        public OneChannelEffect()
        {
            var pixelShader = new PixelShader
            {
                UriSource = new Uri($"{BasePath}Channels/OneChannel.ps", UriKind.Relative)
            };
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(GreyProperty);
            UpdateShaderValue(ChannelProperty);
        }
    }
}