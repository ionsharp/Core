using System.Threading.Tasks;

namespace Imagin.Common
{
    /// <summary>
    /// Defines a grid that displays properties.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IPropertyGrid<TSource>
    {
        /// <summary>
        /// Loads a collection of properties served by the given <see langword="TSource"/>.
        /// </summary>
        /// <param name="source">The source in which properties are served.</param>
        Task LoadPropertiesAsync(TSource source);
    }
}
