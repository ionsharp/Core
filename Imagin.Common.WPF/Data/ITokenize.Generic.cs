using System.Collections.Generic;

namespace Imagin.Common.Data
{
    public interface ITokenize<Token>
    {
        object Source { get; }

        IEnumerable<Token> Tokenize(string input, char delimiter);

        Token ToToken(string input);

        string ToString(Token input);
    }
}