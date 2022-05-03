namespace Imagin.Common.Colors
{
    /// <summary>
    /// Standards produced by the CIE (International Commission on Illumination) are a concise documentation of data defining aspects of light and lighting 
    /// for which international harmony requires a unique definition.
    /// </summary>
    public static class CIE
    {
        /// <remarks>Standard</remarks>
        public const double Epsilon = 0.008856451679035631;

        /// <remarks>Intent</remarks>
        public const double IEpsilon = 216.0 / 24389.0;

        /// <remarks>Standard</remarks>
        public const double Kappa = 903.2962962962961;

        /// <remarks>Intent</remarks>
        public const double IKappa = 24389.0 / 27.0;
    }
}