using Imagin.Common.Data;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [DisplayName(nameof(ConsoleOptions))]
    [Serializable]
    public class ConsoleOptions : ControlOptions<Console>
    {
        StringColor background = System.Windows.Media.Colors.Black;
        public SolidColorBrush Background
        {
            get => background.Brush;
            set => this.Change(ref background, value.Color);
        }

        string backgroundImage = string.Empty;
        [Style(StringStyle.FilePath)]
        public string BackgroundImage
        {
            get => backgroundImage;
            set => this.Change(ref backgroundImage, value);
        }

        string backgroundStretch = $"{Stretch.Fill}";
        public Stretch BackgroundStretch
        {
            get => (Stretch)Enum.Parse(typeof(Stretch), backgroundStretch);
            set => this.Change(ref backgroundStretch, $"{value}");
        }

        string fontFamily = "Consolas";
        public FontFamily FontFamily
        {
            get
            {
                if (fontFamily == null)
                    return default;

                FontFamily result = null;
                Try.Invoke(() => result = new FontFamily(fontFamily));
                return result;
            }
            set => this.Change(ref fontFamily, value.Source);
        }

        double fontSize = 16.0;
        [Range(12.0, 48.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        StringColor foreground = System.Windows.Media.Colors.White;
        public SolidColorBrush Foreground
        {
            get => foreground.Brush;
            set => this.Change(ref foreground, value.Color);
        }

        string output = string.Empty;
        [Hidden]
        public string Output
        {
            get => output;
            set => this.Change(ref output, value);
        }

        string textWrap = $"{TextWrapping.NoWrap}";
        public TextWrapping TextWrap
        {
            get => (TextWrapping)Enum.Parse(typeof(TextWrapping), textWrap);
            set => this.Change(ref textWrap, value.ToString());
        }

        public ConsoleOptions() : base() { }
    }
}