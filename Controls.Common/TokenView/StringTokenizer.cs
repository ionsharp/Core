using Imagin.Common;
using Imagin.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Tokenizes a <see cref="string"/> into multiple <see cref="string"/>s.
    /// </summary>
    public sealed class StringTokenizer : Tokenizer<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenString"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        public override IEnumerable<string> GenerateFrom(string TokenString, char Delimiter)
        {
            var source = TokenString.Split(Arr.New(Delimiter), StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in source)
                yield return i;
        }

        /// <summary>
        /// Gets a token if the given <see cref="string"/> can be parsed.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public override string ParseToken(string Text)
        {
            var Result = Text?.Trim();
            return !Result.IsEmpty() ? Result : null;
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to a list of tokens.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public override IEnumerable<string> Tokenize(string Text)
        {
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public override string ToString(string Token)
        {
            return Token;
        }
    }
}
