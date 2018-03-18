using Imagin.Colour.Compression;

namespace Imagin.Colour
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkingSpaces
    {
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace AdobeRGB1998    => new WorkingSpace(Illuminants.D65, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1500, 0.0600)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace AppleRGB        => new WorkingSpace(Illuminants.D65, new GammaCompanding(1.8),   new ChromaticityCoordinates(new ChromacityPoint(0.6250, 0.3400), new ChromacityPoint(0.2800, 0.5950), new ChromacityPoint(0.1550, 0.0700)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace BestRGB         => new WorkingSpace(Illuminants.D50, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.7347, 0.2653), new ChromacityPoint(0.2150, 0.7750), new ChromacityPoint(0.1300, 0.0350)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace BetaRGB         => new WorkingSpace(Illuminants.D50, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6888, 0.3112), new ChromacityPoint(0.1986, 0.7551), new ChromacityPoint(0.1265, 0.0352)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace BruceRGB        => new WorkingSpace(Illuminants.D65, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2800, 0.6500), new ChromacityPoint(0.1500, 0.0600)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace CIERGB          => new WorkingSpace(Illuminants.E,   new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.7350, 0.2650), new ChromacityPoint(0.2740, 0.7170), new ChromacityPoint(0.1670, 0.0090)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace ColorMatchRGB   => new WorkingSpace(Illuminants.D50, new GammaCompanding(1.8),   new ChromaticityCoordinates(new ChromacityPoint(0.6300, 0.3400), new ChromacityPoint(0.2950, 0.6050), new ChromacityPoint(0.1500, 0.0750)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace Default         => sRGB;
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace DonRGB4         => new WorkingSpace(Illuminants.D50, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6960, 0.3000), new ChromacityPoint(0.2150, 0.7650), new ChromacityPoint(0.1300, 0.0350)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace ECIRGBv2        => new WorkingSpace(Illuminants.D50, new LCompanding(),          new ChromaticityCoordinates(new ChromacityPoint(0.6700, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1400, 0.0800)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace EktaSpacePS5    => new WorkingSpace(Illuminants.D50, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6950, 0.3050), new ChromacityPoint(0.2600, 0.7000), new ChromacityPoint(0.1100, 0.0050)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace NTSCRGB         => new WorkingSpace(Illuminants.C,   new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6700, 0.3300), new ChromacityPoint(0.2100, 0.7100), new ChromacityPoint(0.1400, 0.0800)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace PALSECAMRGB     => new WorkingSpace(Illuminants.D65, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.2900, 0.6000), new ChromacityPoint(0.1500, 0.0600)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace ProPhotoRGB     => new WorkingSpace(Illuminants.D50, new GammaCompanding(1.8),   new ChromaticityCoordinates(new ChromacityPoint(0.7347, 0.2653), new ChromacityPoint(0.1596, 0.8404), new ChromacityPoint(0.0366, 0.0001)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace Rec709          => new WorkingSpace(Illuminants.D65, new Rec709Companding(),     new ChromaticityCoordinates(new ChromacityPoint(0.64, 0.33),     new ChromacityPoint(0.30, 0.60),     new ChromacityPoint(0.15, 0.06)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace Rec2020         => new WorkingSpace(Illuminants.D65, new Rec2020Companding(),    new ChromaticityCoordinates(new ChromacityPoint(0.708, 0.292),   new ChromacityPoint(0.170, 0.797),   new ChromacityPoint(0.131, 0.046)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace SMPTECRGB       => new WorkingSpace(Illuminants.D65, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6300, 0.3400), new ChromacityPoint(0.3100, 0.5950), new ChromacityPoint(0.1550, 0.0700)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace sRGB            => new WorkingSpace(Illuminants.D65, new sRGBCompanding(),       new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace sRGBSimplified  => new WorkingSpace(Illuminants.D65, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace WideGamutRGB    => new WorkingSpace(Illuminants.D50, new GammaCompanding(2.2),   new ChromaticityCoordinates(new ChromacityPoint(0.7350, 0.2650), new ChromacityPoint(0.1150, 0.8260), new ChromacityPoint(0.1570, 0.0180)));
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static WorkingSpace scRGB           => new WorkingSpace(Illuminants.D65, new sRGBCompanding(),       new ChromaticityCoordinates(new ChromacityPoint(0.6400, 0.3300), new ChromacityPoint(0.3000, 0.6000), new ChromacityPoint(0.1500, 0.0600)));
    }
}
