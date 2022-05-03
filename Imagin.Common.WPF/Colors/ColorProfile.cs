using System;

namespace Imagin.Common.Colors
{
    [Serializable]
    public struct ColorProfile
    {
        public ChromacityPosition Chromaticity 
            { get; private set; }

        public ICompanding Companding 
            { get; private set; }

        public Vector Illuminant 
            { get; private set; }

        public ColorProfile(Vector illuminant, ICompanding companding, ChromacityPosition chromaticity)
        { Illuminant = illuminant; Companding = companding; Chromaticity = chromaticity; }
    }
}