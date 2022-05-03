using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Storage
{
    /// <summary>
    /// Specifies a semi-colon (;) separated list of file or folder extensions.
    /// </summary>
    public struct Extensions
    {
        public static Extensions Empty = new Extensions(string.Empty);

        public int Count => values.Length;

        readonly string value;

        readonly string[] values;
        public string[] Values => values;

        /// <summary>
        /// Initializes an instance of the <see cref="Extensions"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public Extensions(string input)
        {
            value
                = input;
            values
                = input.Split(Array<char>.New(';'), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Extensions"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public Extensions(string[] input) : this(input.ToString<string>(";")) { }

        public static implicit operator string(Extensions i) => i.value;

        public static implicit operator Extensions(string i) => new Extensions(i);

        public static bool operator <(Extensions a, Extensions b) => a.Count < b.Count;

        public static bool operator >(Extensions a, Extensions b) => a.Count > b.Count;

        public static bool operator ==(Extensions a, Extensions b) => a.value == b.value;

        public static bool operator !=(Extensions a, Extensions b) => a.value != b.value;

        public static bool operator <=(Extensions a, Extensions b) => a.Count <= b.Count;

        public static bool operator >=(Extensions a, Extensions b) => a.Count >= b.Count;

        public static Extensions operator +(Extensions a, Extensions b) => $"{a.value};{b.value}";

        public static Extensions operator +(Extensions a, string b) => $"{a.value};{b}";

        public override bool Equals(object a) => a is Extensions b ? this == b : false;

        public override int GetHashCode() => value.GetHashCode();

        public override string ToString() => value;
    }
}