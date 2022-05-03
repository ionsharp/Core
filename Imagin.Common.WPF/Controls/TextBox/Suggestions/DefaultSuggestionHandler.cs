namespace Imagin.Common.Controls
{
    public class DefaultSuggestionHandler : ISuggest
    {
        public string Convert(object input) => $"{input}";

        public bool Handle(object input, string text) => Convert(input).ToLower().StartsWith(text.ToLower());
    }
}