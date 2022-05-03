using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows.Media;
using System.Collections.Generic;

namespace Imagin.Apps.Paint
{
    public enum PresetCategory { Definition, Inches, Ratio, Square }

    [Serializable]
    public class DocumentPreset : Preset<Document>
    {
        public static IEnumerable<DocumentPreset> Default()
        {
            yield return new DocumentPreset(PresetCategory.Definition, "SD (Standard Definition)", 640, 480);

            yield return new DocumentPreset(PresetCategory.Definition, "HD (Full)", 1920, 1080);
            yield return new DocumentPreset(PresetCategory.Definition, "HD (High Definition)", 1280, 720);
            yield return new DocumentPreset(PresetCategory.Definition, "HD (High Definition)", 1080, 1080);
            yield return new DocumentPreset(PresetCategory.Definition, "HD (Ultra)", 3840, 2160);

            yield return new DocumentPreset(PresetCategory.Definition, "2K", 2048, 1152);
            yield return new DocumentPreset(PresetCategory.Definition, "4K", 4096, 2160);
            yield return new DocumentPreset(PresetCategory.Definition, "8K", 7680, 4320);

            yield return new DocumentPreset(PresetCategory.Square, 8, 8);
            yield return new DocumentPreset(PresetCategory.Square, 16, 16);
            yield return new DocumentPreset(PresetCategory.Square, 32, 32);
            yield return new DocumentPreset(PresetCategory.Square, 48, 48);
            yield return new DocumentPreset(PresetCategory.Square, 64, 64);
            yield return new DocumentPreset(PresetCategory.Square, 96, 96);
            yield return new DocumentPreset(PresetCategory.Square, 128, 128);
            yield return new DocumentPreset(PresetCategory.Square, 256, 256);
            yield return new DocumentPreset(PresetCategory.Square, 512, 512);

            yield return new DocumentPreset(PresetCategory.Ratio, 1, 1);
            yield return new DocumentPreset(PresetCategory.Ratio, 3, 2);
            yield return new DocumentPreset(PresetCategory.Ratio, 4, 3);
            yield return new DocumentPreset(PresetCategory.Ratio, 16, 9);

            yield return new DocumentPreset(PresetCategory.Inches, 3, 5);
            yield return new DocumentPreset(PresetCategory.Inches, 4, 6);
            yield return new DocumentPreset(PresetCategory.Inches, 5, 7);
            yield return new DocumentPreset(PresetCategory.Inches, 8, 8);
            yield return new DocumentPreset(PresetCategory.Inches, 8, 10);
            yield return new DocumentPreset(PresetCategory.Inches, 8.5, 11);
            yield return new DocumentPreset(PresetCategory.Inches, 9, 16);
            yield return new DocumentPreset(PresetCategory.Inches, 11, 14);
            yield return new DocumentPreset(PresetCategory.Inches, 11, 16);
            yield return new DocumentPreset(PresetCategory.Inches, 12, 18);
            yield return new DocumentPreset(PresetCategory.Inches, 18, 24);
            yield return new DocumentPreset(PresetCategory.Inches, 24, 36);
        }

        string background = $"255,255,255,255";
        public Color Background
        {
            get
            {
                var result = background.Split(',');
                return Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref background, $"{value.A},{value.R},{value.G},{value.B}");
        }

        int bitsPerPixel = 32;
        public int BitsPerPixel
        {
            get => bitsPerPixel;
            set
            {
                this.Change(ref bitsPerPixel, value);
                this.Changed(() => Size);
            }
        }

        string category = "Custom";
        public string Category
        {
            get => category;
            set => this.Change(ref category, value);
        }

        int height = 256;
        public int Height
        {
            get => height;
            set
            {
                if (link)
                {
                    var newSize = new Int32Size(height, width).Resize(SizeField.Height, value);
                    this.Change(ref width, newSize.Width, () => Width);
                }

                this.Change(ref height, value);
                this.Changed(() => Size);
            }
        }

        bool link = false;
        public bool Link
        {
            get => link;
            set => this.Change(ref link, value);
        }

        ImageMode mode = ImageMode.RGB;
        public ImageMode Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        ImageModeDepth modeDepth = ImageModeDepth.Bit32;
        public ImageModeDepth ModeDepth
        {
            get => modeDepth;
            set => this.Change(ref modeDepth, value);
        }

        float resolution = 72f;
        public float Resolution
        {
            get => resolution;
            set => this.Change(ref resolution, value);
        }

        public long Size
        {
            get
            {
                var result = (height * width).Double();
                result *= bitsPerPixel.Double() / 8.0;
                return result.Int64();
            }
        }

        int width = 256;
        public int Width
        {
            get => width;
            set
            {
                if (link)
                {
                    var newSize = new Int32Size(height, width).Resize(SizeField.Width, value);
                    this.Change(ref height, newSize.Height, () => Height);
                }

                this.Change(ref width, value);
                this.Changed(() => Size);
            }
        }

        public DocumentPreset() : this(string.Empty) { }

        public DocumentPreset(string name) : base(name) { }

        public DocumentPreset(PresetCategory category, string name, double width, double height) : this(name)
        {
            Category = $"{category}";
            Width = width.Int32();
            Height = height.Int32();
        }

        public DocumentPreset(PresetCategory category, double width, double height) : this(category, $"{category}", width, height) { }

        public override Preset<Document> Clone() => new DocumentPreset()
        {
            Background 
                = Background,
            Height 
                = height,
            Mode 
                = Mode,
            ModeDepth 
                = ModeDepth,
            Name 
                = Name,
            Resolution 
                = resolution,
            Width 
                = width
        };
    }
}