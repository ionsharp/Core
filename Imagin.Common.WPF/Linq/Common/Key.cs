using Imagin.Common.Native;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class XKey
    {
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char Character(this Key key)
        {            
            /*
            The old way...
            var shift = ModifierKeys.Shift.Pressed();
            switch (input)
            {
                case Key.OemCloseBrackets:
                    return shift ? '}' : ']';
                case Key.OemComma:
                    return shift ? (char?)null : ','; //<
                case Key.OemMinus:
                    return shift ? '_' : '-';
                case Key.OemOpenBrackets:
                    return shift ? '{' : '[';
                case Key.OemPeriod:
                    return shift ? '.' : (char?)null; //>
                case Key.OemPlus:
                    return shift ? '+' : '=';
                case Key.OemSemicolon:
                    return shift ? (char?)null : ';'; //:

                case Key.Space:
                    return ' ';

                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    return shift ? $"{input}"[0] : $"{input}".ToLower()[0];

                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return $"{input}".Replace("NumPad", string.Empty)[0];

                case Key.Add:
                    return '+';

                case Key.Decimal:
                    return '.';

                case Key.Subtract:
                    return '-';

                case Key.D0:
                    return shift ? ')' : '0';
                case Key.D1:
                    return shift ? '!' : '1';
                case Key.D2:
                    return shift ? '@' : '2';
                case Key.D3:
                    return shift ? '#' : '3';
                case Key.D4:
                    return shift ? '$' : '4';
                case Key.D5:
                    return shift ? '%' : '5';
                case Key.D6:
                    return shift ? '^' : '6';
                case Key.D7:
                    return shift ? '&' : '7';
                case Key.D8:
                    return shift ? (char?)null : '8'; //*
                case Key.D9:
                    return shift ? '(' : '9';
            }
            return null;
            */
            var result = '\0';

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var scan = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            var builder = new StringBuilder(2);

            var unicode = ToUnicode((uint)virtualKey, scan, keyboardState, builder, builder.Capacity, 0);
            switch (unicode)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    result = builder[0];
                    break;
                default:
                    result = builder[0];
                    break;
            }
            return result;
        }

        public static bool ModifyingPopupState(this KeyEventArgs e) => ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up))) || (e.Key == Key.F4));

        public static bool Pressed(this Key key) => Keyboard.IsKeyDown(key);
    }
}