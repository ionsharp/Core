namespace Imagin.Common.Converters
{
    public sealed class ConverterValue<T>
    {
        public readonly object ActualValue;

        public ConverterValue(T input) => ActualValue = input;

        public ConverterValue(Nothing input) => ActualValue = input;

        public static implicit operator ConverterValue<T>(T input) => new(input);

        public static implicit operator ConverterValue<T>(Nothing input) => new(input);
    }
}