using System.Collections.Generic;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Specifies an object capable of tokenizing a <see cref="string"/>.
    /// </summary>
    public interface ITokenize
    {
        object Source { get; }

        IEnumerable<object> Tokenize(string input, char delimiter);

        object ToToken(string input);

        string ToString(object input);
    }
}