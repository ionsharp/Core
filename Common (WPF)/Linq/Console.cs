using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void ClearLine()
        {
            for (var i = Console.CursorLeft; i >= 0; i--)
                Console.Write("\b \b");
        }
    }
}
