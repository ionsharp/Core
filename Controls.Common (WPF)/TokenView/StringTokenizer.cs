using Imagin.Common;
using Imagin.Common.Linq;
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
        public override IEnumerable<string> Tokenize(string TokenString, char Delimiter)
        {
            var source = TokenString.Split(Batch.New(Delimiter), StringSplitOptions.RemoveEmptyEntries);
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
            var Result = Text.Trim();
            return !Result.IsEmpty() ? Result : null;
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

        /// <summary>
        /// 
        /// </summary>
        public StringTokenizer() : base(null)
        {
        }
    }
}
