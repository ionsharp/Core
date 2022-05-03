namespace Imagin.Common.Colors
{
    /// <summary>
    /// Represents a theoretical source of visible light with a published profile.
    /// </summary>
    /// <remarks>
    /// <para>https://github.com/tompazourek/Colourful</para>
    /// <para>https://en.wikipedia.org/wiki/Standard_illuminant</para>
    /// </remarks>
    public static class Illuminants
    {
        /// <summary>
        /// An arbitrary illuminant when none is specified (<see cref="Illuminants.D65"/>).
        /// </summary>
        public static readonly Vector Default = D65;

        //...

        /// <summary>
        /// Incandescent / tungsten
        /// </summary>
        /// <remarks>[1.09850, 1, 0.35585]</remarks>
        public static readonly Vector A     = new(1.09850, 1, 0.35585);
        /// <summary>
        /// Direct sunlight at noon 
        /// </summary>
        /// <remarks>[0.99072, 1, 0.85223]</remarks>
        public static readonly Vector B     = new(0.99072, 1, 0.85223);
        /// <summary>
        /// Average / North sky daylight 
        /// </summary>
        /// <remarks>[0.98074, 1, 1.18232]</remarks>
        public static readonly Vector C     = new(0.98074, 1, 1.18232);
        /// <summary> Horizon light</summary>
        /// <remarks>
        /// [0.96422, 1, 0.82521]
        /// </remarks>
        public static readonly Vector D50   = new(0.96422, 1, 0.82521);
        /// <summary> Mid-morning / Mid-afternoon daylight</summary>
        /// <remarks>
        /// [0.95682, 1, 0.92149]
        /// </remarks>
        public static readonly Vector D55   = new(0.95682, 1, 0.92149);
        /// <summary> Noon daylight (television/sRGB color space)</summary>
        /// <remarks>
        /// [0.95047, 1, 1.08883]
        /// </remarks>
        public static readonly Vector D65   = new(0.95047, 1, 1.08883);
        /// <summary> North sky daylight</summary>
        /// <remarks>
        /// [0.94972, 1, 1.22638]
        /// </remarks>
        public static readonly Vector D75   = new(0.94972, 1, 1.22638);
        /// <summary> High-efficiency blue phosphor monitors (BT.2035)</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector D93   = new(1.00000, 1, 1.00000);
        /// <summary> Equal energy</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector E     = new(1.00000, 1, 1.00000);
        /// <summary> Daylight fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F1    = new(1.00000, 1, 1.00000);
        /// <summary> Cool white fluorescent</summary>
        /// <remarks>[0.99186, 1, 0.67393]</remarks>
        public static readonly Vector F2    = new(0.99186, 1, 0.67393);
        /// <summary> White fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F3    = new(1.00000, 1, 1.00000);
        /// <summary> Warm white fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F4    = new(1.00000, 1, 1.00000);
        /// <summary> Daylight fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F5    = new(1.00000, 1, 1.00000);
        /// <summary> Light white fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F6    = new(1.00000, 1, 1.00000);
        /// <summary>D65 simulator (daylight simulator)</summary>
        /// <remarks>[0.95041, 1, 1.08747]</remarks>
        public static readonly Vector F7    = new(0.95041, 1, 1.08747);
        /// <summary>D50 simulator (Sylvania F40 Design 50)</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F8    = new(1.00000, 1, 1.00000);
        /// <summary>Cool white deluxe fluorescent</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F9    = new(1.00000, 1, 1.00000);
        /// <summary>Philips TL85, Ultralume 50</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F10   = new(1.00000, 1, 1.00000);
        /// <summary>Philips TL84, Ultralume 40</summary>
        /// <remarks>[1.00962, 1, 0.64350]</remarks>
        public static readonly Vector F11   = new(1.00962, 1, 0.64350);
        /// <summary>Philips TL83, Ultralume 30</summary>
        /// <remarks>[1, 1, 1]</remarks>
        public static readonly Vector F12   = new(1.00000, 1, 1.00000);
    }
}