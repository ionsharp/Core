using System;

namespace Imagin.Common.Colors
{
    [Serializable]
    public enum ColorProfiles
    {
        /// <summary>
        /// Adobe RGB (1998).
        /// </summary>
        AdobeRGB1998,
        /// <summary>
        /// Apple sRGB.
        /// </summary>
        AppleRGB,
        /// <summary>
        /// Best RGB.
        /// </summary>
        BestRGB,
        /// <summary>
        /// Beta RGB.
        /// </summary>
        BetaRGB,
        /// <summary>
        /// Bruce RGB.
        /// </summary>
        BruceRGB,
        /// <summary>
        /// CIE RGB.
        /// </summary>
        CIERGB,
        /// <summary>
        /// ColorMatch RGB.
        /// </summary>
        ColorMatchRGB,
        /// <summary>
        /// Don RGB 4.
        /// </summary>
        DonRGB4,
        /// <summary>
        /// ECI RGB v2.
        /// </summary>
        ECIRGBv2,
        /// <summary>
        /// Ekta Space PS5.
        /// </summary>
        EktaSpacePS5,
        /// <summary>
        /// NTSC RGB.
        /// </summary>
        NTSCRGB,
        /// <summary>
        /// PAL/SECAM RGB.
        /// </summary>
        PALSECAMRGB,
        /// <summary>
        /// ProPhoto RGB.
        /// </summary>
        ProPhotoRGB,
        /// <summary>
        /// Rec. 709 (ITU-R Recommendation BT.709).
        /// </summary>
        Rec709,
        /// <summary>
        /// Rec. 2020 (ITU-R Recommendation BT.2020).
        /// </summary>
        Rec2020,
        /// <summary>
        /// SMPTE-C RGB.
        /// </summary>
        SMPTECRGB,
        /// <summary>
        /// <para>sRGB is a standard RGB (red, green, blue) color space that HP and Microsoft created cooperatively in 1996 to use on monitors, printers, and the World Wide Web. It was subsequently standardized by the International Electrotechnical Commission (IEC) as IEC 61966-2-1:1999. sRGB is the current defined standard colorspace for the web, and it is usually the assumed colorspace for images that are neither tagged for a colorspace nor have an embedded color profile.</para>
        /// <para>sRGB uses the same color primaries and white point as ITU-R BT.709, the standard for HDTV. However sRGB does not use the BT.709 nonlinear transfer function (sometimes informally referred to as gamma). Instead the sRGB transfer function was created for computer processing convenience, as well as being compatible with the era's CRT displays. An associated viewing environment is designed to match typical home and office viewing conditions. sRGB essentially codifies the display specifications for the computer monitors in use at that time.</para>
        /// <para>An amendment of the IEC 61966-2-1 standard document that defines sRGB includes the definition of a number of variants including sYCC, which is a Y′Cb′Cr′ luma-chroma-chroma color representation of sRGB colors with an extended range of values in the RGB domain (supporting negative values in the RGB domain).</para>
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/SRGB</remarks>
        sRGB,
        /// <summary>
        /// <para>scRGB is a wide color gamut RGB color space created by Microsoft and HP that uses the same color primaries and white/black points as the sRGB color space but allows coordinates below zero and greater than one. The full range is −0.5 through just less than +7.5.</para>
        /// <para>Negative numbers enables scRGB to encompass most of the CIE 1931 color space while maintaining simplicity and backward compatibility with sRGB without the complexity of color management. The cost of maintaining compatibility with sRGB is that approximately 80% of the scRGB color space consists of imaginary colors.</para>
        /// <para>Large positive numbers allow high dynamic range images to be represented, though the range is inferior to that of some other high dynamic range formats such as OpenEXR.</para>
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/ScRGB</remarks>
        scRGB,
        /// <summary>
        /// Simplified sRGB (uses <see cref="GammaCompanding"/> instead of <see cref="sRGBCompanding"/>).
        /// </summary>
        sRGBSimplified,
        /// <summary>
        /// Wide Gamut RGB.
        /// </summary>
        WideGamutRGB,
    }

    /// <remarks>https://github.com/tompazourek/Colourful</remarks>
    public static class DefaultColorProfiles
    {
        public static ColorProfile AdobeRGB1998 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile AppleRGB 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(1.8), 
            new ChromacityPosition(new ChromacityPoint(0.6250, 0.3400), new ChromacityPoint(0.2800, 0.5950), new ChromacityPoint(0.1550, 0.0700)));

        public static ColorProfile BestRGB 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.7347, 0.2653), new ChromacityPoint(0.2150, 0.7750), new ChromacityPoint(0.1300, 0.0350)));

        public static ColorProfile BetaRGB 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6888, 0.3112), new ChromacityPoint(0.1986, 0.7551), new ChromacityPoint(0.1265, 0.0352)));

        public static ColorProfile BruceRGB 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2800, 0.6500), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile CIERGB 
            => new ColorProfile(Illuminants.E,   
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.7350, 0.2650), new ChromacityPoint(0.2740, 0.7170), new ChromacityPoint(0.1670, 0.0090)));

        public static ColorProfile ColorMatchRGB 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(1.8), 
            new ChromacityPosition(new ChromacityPoint(0.6300, 0.3400), new ChromacityPoint(0.2950, 0.6050), new ChromacityPoint(0.1500, 0.0750)));

        public static ColorProfile DonRGB4 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6960, 0.3000), new ChromacityPoint(0.2150, 0.7650), new ChromacityPoint(0.1300, 0.0350)));

        public static ColorProfile ECIRGBv2 
            => new ColorProfile(Illuminants.D50, 
            new LCompanding(), 
            new ChromacityPosition(new ChromacityPoint(0.6700, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1400, 0.0800)));

        public static ColorProfile EktaSpacePS5 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6950, 0.3050), new ChromacityPoint(0.2600, 0.7000), new ChromacityPoint(0.1100, 0.0050)));

        public static ColorProfile NTSCRGB 
            => new ColorProfile(Illuminants.C,   
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6700, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1400, 0.0800)));

        public static ColorProfile PALSECAMRGB 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2900, 0.6000), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile ProPhotoRGB 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(1.8), 
            new ChromacityPosition(new ChromacityPoint(0.7347, 0.2653), new ChromacityPoint(0.1596, 0.8404), new ChromacityPoint(0.0366, 0.0001)));

        public static ColorProfile Rec709 
            => new ColorProfile(Illuminants.D65, 
            new Rec709Companding(), 
            new ChromacityPosition(new ChromacityPoint(0.64, 0.33), new ChromacityPoint(0.30, 0.60), new ChromacityPoint(0.15, 0.06)));

        public static ColorProfile Rec2020 
            => new ColorProfile(Illuminants.D65, 
            new Rec2020Companding(), 
            new ChromacityPosition(new ChromacityPoint(0.708, 0.292), new ChromacityPoint(0.170, 0.797), new ChromacityPoint(0.131, 0.046)));

        public static ColorProfile SMPTECRGB 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6300, 0.3400), new ChromacityPoint(0.3100, 0.5950), new ChromacityPoint(0.1550, 0.0700)));

        public static ColorProfile sRGB 
            => new ColorProfile(Illuminants.D65, 
            new sRGBCompanding(), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile scRGB
            => new ColorProfile(Illuminants.D65, 
            new sRGBCompanding(), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile sRGBSimplified 
            => new ColorProfile(Illuminants.D65, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));

        public static ColorProfile WideGamutRGB 
            => new ColorProfile(Illuminants.D50, 
            new GammaCompanding(2.2), 
            new ChromacityPosition(new ChromacityPoint(0.7350, 0.2650), new ChromacityPoint(0.1150, 0.8260), new ChromacityPoint(0.1570, 0.0180)));
    }
}