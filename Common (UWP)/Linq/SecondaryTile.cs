using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class SecondaryTileExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        public static async Task<SecondaryTile> FindAsync(Predicate<SecondaryTile> Where)
        {
            foreach (var i in await SecondaryTile.FindAllAsync())
            {
                if (Where(i))
                    return i;
            }

            return default(SecondaryTile);
        }
    }
}
