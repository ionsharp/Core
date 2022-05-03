using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public enum ImageModeDepth
    {
        [DisplayName("1")]
        Bit1,
        [DisplayName("8")]
        Bit8,
        [DisplayName("16")]
        Bit16,
        [DisplayName("32")]
        Bit32,
    }
}