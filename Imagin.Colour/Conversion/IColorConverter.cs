namespace Imagin.Colour.Conversion
{
    /// <summary>
    /// Converts a color between <see langword="TInput"/> : <see cref="IColor"/> and <see langword="TOutput"/> : <see cref="IColor"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public interface IColorConverter<in TInput, out TOutput> where TInput : IColor where TOutput : IColor
    {
        /// <summary>
        /// Converts the given <see langword="TInput"/> : <see cref="IColor"/> to the specified <see langword="TOutput"/> : <see cref="IColor"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TOutput Convert(TInput input);
    }
}
