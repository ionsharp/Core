using System;
using System.ComponentModel;

namespace Imagin.Common.Text
{
    [Flags]
    [Serializable]
    public enum Encoding
    {
        [Description("")]
        Default = 0,
        [Description("A protocol to encode 128 specified characters into seven-bit integers.")]
        ASCII = 1,
        [Description("A computing industry standard for the consistent encoding, representation, and handling of text.")]
        Unicode = 2,
        [Description("A variable-length character encoding that was proposed for representing Unicode text using a stream of ASCII characters.")]
        UTF7 = 4,
        [Description("A character encoding capable of encoding all possible characters, or code points, defined by Unicode.")]
        UTF8 = 8,
        [Description("A fixed-length protocol to encode Unicode code points that uses exactly 32 bits per Unicode code point.")]
        UTF32 = 16
    }
}
