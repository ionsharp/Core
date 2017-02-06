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
    public static class StackExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public static bool Any<TObject>(this Stack<TObject> Value)
        {
            return Value.Count > 0;
        }
    }
}
