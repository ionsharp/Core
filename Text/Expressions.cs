namespace Imagin.Core.Text
{
    public static class Expressions
    {
        public const string Guid
            = "^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$";

        public const string Letters
            = "^[a-zA-Z]*$";

        public const string LettersAndNumbers
            = "^[a-zA-Z0-9]*$";

        public const string Numbers
            = "^[0-9]*$";
    }
}