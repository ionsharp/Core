using Imagin.Common;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System.Runtime.CompilerServices;

namespace Imagin.Apps.Paint
{
    public enum ImageResizeMode
    {
        Stretch, Trim
    }

    public class ImageResizeOptions : Base
    {
        enum Category { Current, New, Stretch, Trim }

        readonly Handle handle = false;

        CardinalDirection anchor = CardinalDirection.Origin;
        [Category(Category.Trim)]
        [DisplayName("Direction")]
        public CardinalDirection Anchor
        {
            get => anchor;
            set => this.Change(ref anchor, value);
        }

        int bitsPerPixel;
        [Hidden]
        public int BitsPerPixel
        {
            get => bitsPerPixel;
            private set => this.Change(ref bitsPerPixel, value);
        }

        double currentHeight;
        [Category(Category.Current)]
        [Index(1)]
        [ReadOnly]
        [Style(DoubleStyle.Unit)]
        public double CurrentHeight
        {
            get => currentHeight;
            private set => this.Change(ref currentHeight, value);
        }

        long currentSize;
        [Category(Category.Current)]
        [Convert(typeof(FileSizeConverter))]
        [Index(0)]
        [ReadOnly]
        public long CurrentSize
        {
            get => currentSize;
            private set => this.Change(ref currentSize, value);
        }

        double currentWidth;
        [Category(Category.Current)]
        [Index(2)]
        [ReadOnly]
        [Style(DoubleStyle.Unit)]
        public double CurrentWidth
        {
            get => currentWidth;
            private set => this.Change(ref currentWidth, value);
        }

        Interpolations interpolation = Interpolations.Bilinear;
        [Category(Category.Stretch)]
        public Interpolations Interpolation
        {
            get => interpolation;
            set => this.Change(ref interpolation, value);
        }

        bool link = false;
        [Category(Category.New)]
        [Index(3)]
        [Style(BooleanStyle.Switch)]
        public bool Link
        {
            get => link;
            set => this.Change(ref link, value);
        }

        ImageResizeMode mode = ImageResizeMode.Trim;
        [Featured]
        public ImageResizeMode Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        double newHeight;
        [Category(Category.New)]
        [Index(1)]
        [Style(DoubleStyle.Unit)]
        public double NewHeight
        {
            get => newHeight;
            set => this.Change(ref newHeight, value);
        }

        long newSize;
        [Category(Category.New)]
        [Convert(typeof(FileSizeConverter))]
        [Index(0)]
        [ReadOnly]
        public long NewSize
        {
            get => newSize;
            private set => this.Change(ref newSize, value);
        }

        double newWidth;
        [Category(Category.New)]
        [Index(2)]
        [Style(DoubleStyle.Unit)]
        public double NewWidth
        {
            get => newWidth;
            set => this.Change(ref newWidth, value);
        }

        float resolution;
        [Hidden]
        public float Resolution
        {
            get => resolution;
            private set => this.Change(ref resolution, value);
        }

        GraphicUnit unit = GraphicUnit.Pixel;
        [Hidden]
        public GraphicUnit Unit
        {
            get => unit;
            set => this.Change(ref unit, value);
        }

        public ImageResizeOptions(ImageDocument input) : base()
        {
            CurrentHeight
                = input.Height;
            CurrentWidth
                = input.Width;

            NewHeight
                = input.Height;
            NewWidth
                = input.Width;

            Resolution
                = input.Resolution;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(CurrentHeight):
                case nameof(CurrentWidth):
                    CurrentSize = ((CurrentHeight * CurrentWidth) * (BitsPerPixel / 8.0)).Int64();
                    break;

                case nameof(NewHeight):
                    if (Link)
                    {
                        handle.SafeInvoke(() =>
                        {
                            var result = new DoubleSize(NewHeight, NewWidth).Resize(SizeField.Height, NewHeight);
                            NewWidth = result.Width;
                        });
                    }
                    
                    NewSize = ((NewHeight * NewWidth) * (BitsPerPixel / 8.0)).Int64();
                    break;

                case nameof(NewWidth):
                    if (Link)
                    {
                        handle.SafeInvoke(() =>
                        {
                            var result = new DoubleSize(NewHeight, NewWidth).Resize(SizeField.Width, NewWidth);
                            NewHeight = result.Height;
                        });

                    }
                    
                    NewSize = ((NewHeight * NewWidth) * (BitsPerPixel / 8.0)).Int64();
                    break;
            }
        }
    }
}