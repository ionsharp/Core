using System.Windows.Input;

namespace Imagin.Common.Extensions
{
    public static class KeyboardExtensions
    {
        public static bool IsKeyModifyingPopupState(KeyEventArgs e)
        {
            return ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up))) || (e.Key == Key.F4));
        }
    }
}
