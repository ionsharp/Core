using Imagin.Common.Input;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class KeyboardExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public static bool IsAnyKeyDown(params Key[] Keys)
        {
            if (Keys?.Length > 0)
            {
                foreach (var i in Keys)
                {
                    if (Keyboard.IsKeyDown(i))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsKeyModifyingPopupState(KeyEventArgs e)
        {
            return ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up))) || (e.Key == Key.F4));
        }

        #region public static char ToChar(this Key key)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wVirtKey"></param>
        /// <param name="wScanCode"></param>
        /// <param name="lpKeyState"></param>
        /// <param name="pwszBuff"></param>
        /// <param name="cchBuff"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpKeyState"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uCode"></param>
        /// <param name="uMapType"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static char ToChar(this Key key)
        {
            var Result = '\0';

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);

            var stringBuilder = new StringBuilder(2);

            var r = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (r)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    Result = stringBuilder[0];
                    break;
                default:
                    Result = stringBuilder[0];
                    break;
            }
            return Result;
        }

        #endregion
    }
}
