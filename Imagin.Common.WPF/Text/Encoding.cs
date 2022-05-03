using System;

namespace Imagin.Common.Text
{
    [Serializable]
    public enum Encoding
    {
        ASCII,
        BigEndianUnicode,
        Default,
        Unicode,
        UTF32,
        UTF7,
        UTF8
    }
}