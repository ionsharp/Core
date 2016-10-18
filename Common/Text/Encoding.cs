using System;

namespace Imagin.Common.Text
{
    [Flags]
    [Serializable]
    public enum Encoding
    {
        Default = 0,
        ASCII = 1,
        Unicode = 2,
        UTF7 = 4,
        UTF8 = 8,
        UTF32 = 16
    }
}
