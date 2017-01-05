using System.Windows.Input;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModifierKeysExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPressed(this ModifierKeys Value)
        {
            return (Keyboard.Modifiers & Value) != 0;
        }
    }
}
