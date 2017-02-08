using System.Collections.Generic;
using System.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Tokenizer<TToken> : ITokenizer, ITokenizer<TToken>
    {
        /// <summary>
        /// 
        /// </summary>
        public object TokensSource
        {
            get; set;
        }
        object ITokenizer.TokensSource
        {
            get
            {
                return TokensSource;
            }
            set
            {
                TokensSource = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenString"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        public abstract IEnumerable<TToken> GenerateFrom(string TokenString, char Delimiter);
        IEnumerable<object> ITokenizer.GenerateFrom(string TokenString, char Delimiter)
        {
            return GenerateFrom(TokenString, Delimiter).Cast<object>();
        }

        /// <summary>
        /// Gets a token if the given <see cref="string"/> can be parsed.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public abstract TToken ParseToken(string Text);
        object ITokenizer.ParseToken(string Text)
        {
            return ParseToken(Text);
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to a list of tokens.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public abstract IEnumerable<TToken> Tokenize(string Text);
        IEnumerable<object> ITokenizer.Tokenize(string Text)
        {
            return Tokenize(Text).Cast<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public abstract string ToString(TToken Token);
        string ITokenizer.ToString(object Token)
        {
            return ToString((TToken)Token);
        }
    }
}
