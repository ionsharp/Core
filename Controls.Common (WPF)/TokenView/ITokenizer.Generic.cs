using System.Collections.Generic;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TToken"></typeparam>
    public interface ITokenizer<TToken>
    {
        /// <summary>
        /// 
        /// </summary>
        object Source
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenString"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        IEnumerable<TToken> Tokenize(string TokenString, char Delimiter);

        /// <summary>
        /// Gets a token if the given <see cref="string"/> can be parsed.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        TToken ParseToken(string Text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        string ToString(TToken Token);
    }
}
