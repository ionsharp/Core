using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Tokenizes a <see cref="string"/> into multiple <see cref="string"/>s.
    /// </summary>
    public sealed class StringTokenizer : Tokenizer<string>
    {
        public override IEnumerable<string> Tokenize(string input, char delimiter)
        {
            var result = input.Split(Array<char>.New(delimiter), StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in result)
                yield return i;
        }

        public override string ToToken(string input)
        {
            var result = input.Trim();
            return !result.Empty() ? result : null;
        }

        public override string ToString(string input) => input;

        public StringTokenizer() : base(null) { }
    }
}