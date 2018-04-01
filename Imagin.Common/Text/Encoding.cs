using System;

namespace Imagin.Common.Text
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Encoding
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        Default = 0,
        /// <summary>
        /// A protocol to encode 128 specified characters into seven-bit integers.
        /// </summary>
        [Description("A protocol to encode 128 specified characters into seven-bit integers.")]
        ASCII = 1,
        /// <summary>
        /// A computing industry standard for the consistent encoding, representation, and handling of text.
        /// </summary>
        [Description("A computing industry standard for the consistent encoding, representation, and handling of text.")]
        Unicode = 2,
        /// <summary>
        /// A variable-length character encoding that was proposed for representing Unicode text using a stream of ASCII characters.
        /// </summary>
        [Description("A variable-length character encoding that was proposed for representing Unicode text using a stream of ASCII characters.")]
        UTF7 = 4,
        /// <summary>
        /// A character encoding capable of encoding all possible characters, or code points, defined by Unicode.
        /// </summary>
        [Description("A character encoding capable of encoding all possible characters, or code points, defined by Unicode.")]
        UTF8 = 8,
        /// <summary>
        /// A fixed-length protocol to encode Unicode code points that uses exactly 32 bits per Unicode code point.
        /// </summary>
        [Description("A fixed-length protocol to encode Unicode code points that uses exactly 32 bits per Unicode code point.")]
        UTF32 = 16
    }
}
