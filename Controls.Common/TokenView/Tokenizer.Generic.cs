using System.Collections.Generic;
using System.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Tokenizer<TToken> : ITokenizer, ITokenizer<TToken>
    {
        readonly object source;
        /// <summary>
        /// 
        /// </summary>
        public object Source
        {
            get
            {
                return source;
            }
        }
        object ITokenizer.Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenString"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        public abstract IEnumerable<TToken> Tokenize(string TokenString, char Delimiter);
        IEnumerable<object> ITokenizer.Tokenize(string TokenString, char Delimiter)
        {
            return Tokenize(TokenString, Delimiter).Cast<object>();
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
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public abstract string ToString(TToken Token);
        string ITokenizer.ToString(object Token)
        {
            return ToString((TToken)Token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        public Tokenizer(object Source = null)
        {
            source = Source;
        }
    }
}
