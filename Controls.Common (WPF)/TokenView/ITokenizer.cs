using System.Collections.Generic;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Specifies an object capable of tokenizing a <see cref="string"/>.
    /// </summary>
    public interface ITokenizer
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
        IEnumerable<object> Tokenize(string TokenString, char Delimiter);

        /// <summary>
        /// Gets a token if the given <see cref="string"/> can be parsed.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        object ParseToken(string Text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        string ToString(object Token);
    }
}
