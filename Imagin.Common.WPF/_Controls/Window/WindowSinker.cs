using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowSinker
    {
        #region Properties

        const UInt32 SWP_NOSIZE = 0x0001;

        const UInt32 SWP_NOMOVE = 0x0002;

        const UInt32 SWP_NOACTIVATE = 0x0010;

        const UInt32 SWP_NOZORDER = 0x0004;

        const int WM_ACTIVATEAPP = 0x001C;

        const int WM_ACTIVATE = 0x0006;

        const int WM_SETFOCUS = 0x0007;

        const int WM_WINDOWPOSCHANGING = 0x0046;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        Window Window = null;

        #endregion

        #region WindowSinker

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Window"></param>
        public WindowSinker(Window Window)
        {
            this.Window = Window;
        }

        #endregion

        #region Methods

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll")]
        static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var Handle = (new WindowInteropHelper(Window)).Handle;

            var Source = HwndSource.FromHwnd(Handle);
            Source.RemoveHook(new HwndSourceHook(WndProc));
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            var Hwnd = new WindowInteropHelper(Window).Handle;
            SetWindowPos(Hwnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

            var Handle = (new WindowInteropHelper(Window)).Handle;

            var Source = HwndSource.FromHwnd(Handle);
            Source.AddHook(new HwndSourceHook(WndProc));
        }

        IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETFOCUS)
            {
                hWnd = new WindowInteropHelper(Window).Handle;
                SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                handled = true;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sink()
        {
            Window.Loaded += OnLoaded;
            Window.Closing += OnClosing;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unsink()
        {
            Window.Loaded -= OnLoaded;
            Window.Closing -= OnClosing;
        }

        #endregion
    }
}
