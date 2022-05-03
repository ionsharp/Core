namespace Imagin.Common.Converters
{
    public sealed class Nothing
    {
        public static readonly Nothing Do = new();

        Nothing() { }
    }
}