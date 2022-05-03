using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Text layer")]
    [Icon(App.ImagePath + "LayerText.png")]
    [Serializable]
    public class TextLayer : RasterizableLayer
    {
        #region Properties

        public override PointCollection Bounds => new PointCollection
        {
            new System.Windows.Point(X, Y),
            new System.Windows.Point(X + width, Y),
            new System.Windows.Point(X + width, Y + height),
            new System.Windows.Point(X, Y + height)
        };

        SolidColorBrush fontColor = System.Windows.Media.Brushes.Black;
        [DisplayName("Font color")]
        public SolidColorBrush FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                this.fontColor = value;
                OnPropertyChanged("FontColor");
            }
        }

        System.Windows.Media.FontFamily fontFamily = new("Arial");
        [DisplayName("Font family")]
        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                fontFamily = value;
                OnPropertyChanged("FontFamily");
            }
        }

        double fontSize = 12.0;
        [DisplayName("Font size")]
        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                this.fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
        [DisplayName("Font style")]
        public System.Drawing.FontStyle FontStyle
        {
            get => fontStyle;
            set => this.Change(ref fontStyle, value);
        }

        protected int height = 0;
        [Range(1, 5000, 1)]
        [Format(Common.RangeFormat.Both)]
        public int Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        TextAlignment horizontalTextAlignment = TextAlignment.Left;
        [DisplayName("Text alignment (horizontal)")]
        public TextAlignment HorizontalTextAlignment
        {
            get => horizontalTextAlignment;
            set => this.Change(ref horizontalTextAlignment, value);
        }

        public Int32Region Region
        {
            set
            {
                handleRender.Invoke(() =>
                {
                    Height = value.Height; Width = value.Width;
                    X = value.X; Y = value.Y;
                });
                Render();
            }
        }

        string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                this.Change(ref text, value);
                Name = value;
            }
        }

        VerticalAlignment verticalTextAlignment = VerticalAlignment.Top;
        [DisplayName("Text alignment (vertical)")]
        public VerticalAlignment VerticalTextAlignment
        {
            get => verticalTextAlignment;
            set => this.Change(ref verticalTextAlignment, value);
        }

        protected int width = 0;
        [Range(1, 5000, 1)]
        [Format(Common.RangeFormat.Both)]
        public int Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        #endregion

        #region TextLayer

        public TextLayer(string name, SolidColorBrush fontColor, System.Windows.Media.FontFamily fontFamily, double fontSize, System.Drawing.FontStyle fontStyle, string text, TextAlignment horizontalTextAlignment, VerticalAlignment verticalTextAlignment, System.Drawing.Size size) : base(LayerType.Text, name)
        {
            Pixels = new(BitmapFactory.New(size));
            handleRender.Invoke(() =>
            {
                FontColor = fontColor;
                FontFamily = fontFamily;
                FontSize = fontSize;
                FontStyle = fontStyle;
                Text = text;
                HorizontalTextAlignment = horizontalTextAlignment;
                VerticalTextAlignment = verticalTextAlignment;
            });
        }

        #endregion

        #region Methods

        [Obsolete]
        protected System.Windows.Media.Geometry GetGeometry()
        {
            if (Text.NullOrEmpty())
                return null;

            var result = new FormattedText(Text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily.Source), FontSize, System.Windows.Media.Brushes.Black);
            return result.BuildGeometry(new System.Windows.Point(X, Y));
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(FontColor):
                case nameof(FontFamily):
                case nameof(FontSize):
                case nameof(FontStretch):
                case nameof(FontStyle):
                case nameof(FontWeight):
                case nameof(Height):
                case nameof(HorizontalTextAlignment):
                case nameof(Text):
                case nameof(VerticalTextAlignment):
                case nameof(Width):
                case nameof(X):
                case nameof(Y):
                    Render();
                    break;
            }
        }

        public sealed override Layer Clone()
        {
            var result = new TextLayer(Name, FontColor, FontFamily, FontSize, FontStyle, Text, HorizontalTextAlignment, VerticalTextAlignment, new(Size.Width, Size.Height))
            {
                Height = Height,
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Name = Name,
                Pixels = XWriteableBitmap.Clone(Pixels),
                Width = Width,
                X = X,
                Y = Y,
            };
            result.Style.Paste(Style);
            return result;
        }

        public override void Render(WriteableBitmap input)
            => input.DrawString(X, Y, new IntRect(new IntPoint(X, Y), new IntSize(Width, Height)), FontColor.Color, new PortableFontDesc(FontFamily.Source, FontSize.Int32()), Text);

        #endregion
    }
}