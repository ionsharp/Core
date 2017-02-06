using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public static IEnumerable<object> Where(this Array Value, Func<object, bool> Predicate)
        {
            foreach (var i in Value)
            {
                if (Predicate(i))
                    yield return i;
            }
            yield break;
        }
    }
}
