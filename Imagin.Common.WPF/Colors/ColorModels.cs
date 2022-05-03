using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Colors
{
    public static class XColorModels
    {
        static readonly Dictionary<ColorModels, Vector<Component>> components = new();

        static XColorModels()
        {
            typeof(ColorModels).GetEnumValues<ColorModels>(Appearance.All).ForEach(i =>
            {
                var j = i.GetAttributes<ComponentAttribute>();
                var a = j[0]; var b = j[1]; var c = j[2];
                components.Add(i, new(a.Info, b.Info, c.Info));
            });
        }

        public static Vector<Component> GetComponents(this ColorModels model) => components[model];

        public static Component GetComponent(this ColorModels model, int index) => components[model][index];

        public static Component GetComponent(this ColorModels model, Components component) => components[model][(int)component];
    }

    /// <summary>
    /// Specifies an abstract mathematical model that describes the way colors can be represented as tuples of numbers (https://en.wikipedia.org/wiki/Color_model).
    /// </summary>
    [Flags, Serializable]
    public enum ColorModels
    {
        [Index(0)]
        [Component(0, 1, "R", "Red"), Component(0, 1, "G", "Green"), Component(0, 1, "B", "Blue")]
        RGB = 0,

        //[Component(0, 100, '%', "C", "Cyan"), Component(0, 100, '%', "M", "Magenta"), Component(0, 100, '%', "Y", "Yellow"), Component(0, 100, '%', "K", "Black")]
        //CMYK,

        [Index(1)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 100, '%', "V", "Gray")]
        HCV = 1,

        [Index(2)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 255, ' ', "Y", "Luminance")]
        HCY = 2,

        [Index(3)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "P", "Saturation")]
        [Component(0, 100, '%', "L", "Lightness")]
        HPLuv = 4,

        [Index(4)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "S", "Saturation")]
        [Component(0, 100, '%', "B", "Brightness")]
        HSB = 8,

        [Index(5)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "S", "Saturation")]
        [Component(0, 100, '%', "L", "Lightness")]
        HSL = 16,

        [Index(6)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "S", "Saturation")]
        [Component(0, 100, '%', "L", "Lightness")]
        HSLuv = 32,

        [Index(7)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "S", "Saturation")]
        [Component(0, 255, ' ', "P", "Percieved brightness")]
        HSP = 64,

        [Index(8)]
        [Component(0, 359, '°', "H", "Hue")]
        [Component(0, 100, '%', "W", "Whiteness")]
        [Component(0, 100, '%', "B", "Blackness")]
        HWB = 128,

        [Index(9)]
        [Component(   0, 100, '%', "L", "Lightness")]
        [Component(-100, 100, ' ', "A")]
        [Component(-100, 100, ' ', "B")]
        LAB = 256,

        [Index(10)]
        [Component(   0, 100, '%', "L", "Lightness")]
        [Component(-128, 128, ' ', "A")]
        [Component(-128, 128, ' ', "B")]
        LABh = 512,

        [Index(11)]
        [Component(0, 100, '%', "L", "Luminance")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 359, '°', "H", "Hue")]
        LCHab = 1024,

        [Index(12)]
        [Component(0, 100, '%', "L", "Luminance")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 359, '°', "H", "Hue")]
        LCHuv = 2048,

        [Index(13)]
        [Component(0, 100, '%', "L", "Long")]
        [Component(0, 100, '%', "M", "Medium")]
        [Component(0, 100, '%', "S", "Short")]
        LMS = 4096,

        [Index(14)]
        [Component(   0, 100, '%', "L", "Lightness")]
        [Component(-134, 224, ' ', "U")]
        [Component(-140, 122, ' ', "V")]
        LUV = 8192,

        [Index(15)]
        [Component(0, 1, '%', "T", "Tint")]
        [Component(0, 1, '%', "S", "Saturation")]
        [Component(0, 1, '%', "L", "Lightness")]
        TSL = 16384,

        [Index(16)]
        [Component(0, 100, '%', "L", "Luminance")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 359, '°', "T", "Tint")]
        HUVcv = 32768,

        [Index(17)]
        [Component(0, 100, '%', "L", "Luminance")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 359, '°', "T", "Tint")]
        HUVcy = 65536,

        [Index(18)]
        [Component(0, 100, '%', "L", "Luminance")]
        [Component(0, 100, '%', "C", "Chroma")]
        [Component(0, 359, '°', "T", "Tint")]
        HUVsp = 131072,

        [Index(19)]
        [Component(0, 100, '%', "U", "U")]
        [Component(0, 100, '%', "C", "V")]
        [Component(0, 100, '%', "S", "W")]
        UCS = 262144,

        [Index(20)]
        [Component(-134, 224, ' ', "U")]
        [Component(-140, 122, ' ', "V")]
        [Component(   0, 100, '%', "W")]
        UVW = 524288,

        [Index(21)]
        [Component(0, 1, "x"), Component(0, 1, "y"), Component(0, 100, '%', "Y")]
        xyY = 1048576,

        [Index(22)]
        [Component(0, 95.0455927051670, "X"), Component(0, 100, "Y"), Component(0, 108.9057750759878, "Z")]
        XYZ = 2097152,
    }
}