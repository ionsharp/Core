using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Invoke the given action if predicate.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Predicate"></param>
        public static void InvokeIf(this Action Value, Func<Action, bool> Predicate)
        {
            if (Predicate(Value))
                Value.Invoke();
        }
    }
}
