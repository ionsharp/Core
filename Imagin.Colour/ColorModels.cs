using System;

namespace Imagin.Colour
{
    /// <summary>
    /// Specifies a color model.
    /// </summary>
    [Flags]
    public enum ColorModels
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        CMY = 1,
        /// <summary>
        /// 
        /// </summary>
        CMYK = 2,
        /// <summary>
        /// 
        /// </summary>
        HCG = 4,
        /// <summary>
        /// 
        /// </summary>
        HSB = 8,
        /// <summary>
        /// 
        /// </summary>
        HSI = 16,
        /// <summary>
        /// 
        /// </summary>
        HSL = 32,
        /// <summary>
        /// 
        /// </summary>
        HSM = 64,
        /// <summary>
        /// 
        /// </summary>
        HSP = 128,
        /// <summary>
        /// 
        /// </summary>
        HunterLab = 256,
        /// <summary>
        /// 
        /// </summary>
        HWB = 512,
        /// <summary>
        /// 
        /// </summary>
        Lab = 1024,
        /// <summary>
        /// 
        /// </summary>
        LChab = 2048,
        /// <summary>
        /// 
        /// </summary>
        LChuv = 4096,
        /// <summary>
        /// 
        /// </summary>
        LMS = 8192,
        /// <summary>
        /// 
        /// </summary>
        Luv = 16384,
        /// <summary>
        /// 
        /// </summary>
        RG = 32768,
        /// <summary>
        /// 
        /// </summary>
        RGB = 65536,
        /// <summary>
        /// 
        /// </summary>
        TSL = 131072,
        /// <summary>
        /// 
        /// </summary>
        xvYCC = 262144,
        /// <summary>
        /// 
        /// </summary>
        xyY = 524288,
        /// <summary>
        /// 
        /// </summary>
        XYZ = 1048576,
        /// <summary>
        /// 
        /// </summary>
        YCbCr = 2097152,
        /// <summary>
        /// 
        /// </summary>
        YCbcCrc = 4194304,
        /// <summary>
        /// 
        /// </summary>
        YCoCg = 8388608,
        /// <summary>
        /// 
        /// </summary>
        YDbDr = 16777216,
        /// <summary>
        /// 
        /// </summary>
        YES = 33554432,
        /// <summary>
        /// 
        /// </summary>
        YIQ = 67108864,
        /// <summary>
        /// 
        /// </summary>
        YPbPr = 134217728,
        /// <summary>
        /// 
        /// </summary>
        YUV = 268435456,
        /// <summary>
        /// 
        /// </summary>
        All = CMY | CMYK | HCG | HSB | HSI | HSL | HSM | HSP | HunterLab | HWB | Lab | LChab | LChuv | LMS | Luv | RG | RGB | TSL | xvYCC | xyY | XYZ | YCbCr | YCbcCrc | YCoCg | YDbDr | YES | YIQ | YPbPr | YUV 
    }
}
