namespace Imagin.Common.Text
{
    public class Characters
    {
        public static string All = $"{Numbers}{Lower}{Special}{Upper}";

        public static string LettersAndNumbers = $"{Numbers}{Lower}{Upper}";

        public const string Numbers 
            = "0123456789";

        public const string Lower 
            = "abcdefghijklmnopqrstuvwxyz";

        public const string Special 
            = "!@#$%^&*()-=_+[]{};':\",./<>?`~\\|";

        public const string Upper 
            = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}