using System.Threading.Tasks;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IPropertyGrid<TSource>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        Task GetPropertiesAsync(TSource source);
    }
}
