using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Data
{
    public abstract class Tokenizer<Token> : ITokenize, ITokenize<Token>
    {
        readonly object source;
        public object Source => source;
        object ITokenize.Source => source;

        public abstract IEnumerable<Token> Tokenize(string input, char delimiter);
        IEnumerable<object> ITokenize.Tokenize(string input, char delimiter) => Tokenize(input, delimiter).Cast<object>();

        public abstract Token ToToken(string input);
        object ITokenize.ToToken(string input) => ToToken(input);

        public abstract string ToString(Token input);
        string ITokenize.ToString(object input) => ToString((Token)input);

        public Tokenizer(object input = null) => source = input;
    }
}
