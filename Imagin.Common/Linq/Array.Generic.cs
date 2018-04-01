namespace Imagin.Common.Linq
{
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static TElement[] Add<TElement>(this TElement[] source, params TElement[] elements)
        {
            var length = source.Length;
            System.Array.Resize(ref source, length + elements.Length);

            for (int j = 0, count = elements.Length; j < count; j++)
                source[length + j] = elements[j];

            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static int IndexOf<TElement>(this TElement[] source, TElement element)
        {
            for (int i = 0, length = source.Length; i < length; i++)
            {
                if (source[i].Equals(element))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TElement[][] Project<TElement>(this TElement[,] source)
        {
            int rows = source.GetLength(0), columns = source.GetLength(1);

            var result = new TElement[rows][];
            for (int row = 0; row < rows; row++)
            {
                result[row] = new TElement[columns];
                for (int column = 0; column < columns; column++)
                    result[row][column] = source[row, column];
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TElement[,] Project<TElement>(this TElement[][] source)
        {
            int rows = source.Length, columns = source[0].Length;

            var result = new TElement[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                    result[row, column] = source[row][column];
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="elements"></param>
        public static void Remove<TElement>(this TElement[] source, params TElement[] elements)
        {
            foreach (var i in elements)
                source.RemoveAt(source.IndexOf(i));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        public static void RemoveAt<TElement>(this TElement[] source, int index)
        {
            var result = new TElement[source.Length - 1];

            if (index > 0)
                System.Array.Copy(source, 0, result, 0, index);

            if (index < source.Length - 1)
                System.Array.Copy(source, index + 1, result, index, source.Length - index - 1);

            source = result;
        }
    }
}
