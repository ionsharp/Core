using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Imagin.Common.Behavior
{
    public static class WindowBehavior
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

        static Window Window
        {
            get; set;
        }

        #endregion

        #region Behavior

        #region Always On Bottom

        public static bool GetAlwaysOnBottom(DependencyObject obj)
                {
                    return (bool)obj.GetValue(AlwaysOnBottom);
                }
        public static void SetAlwaysOnBottom(DependencyObject obj, bool value)
        {
                    obj.SetValue(AlwaysOnBottom, value);
        }
        public static readonly DependencyProperty AlwaysOnBottom = DependencyProperty.RegisterAttached("IsWatermarkEnabled", typeof(bool), typeof(WindowBehavior), new UIPropertyMetadata(false, OnAlwaysOnBottomChanged));
        static void OnAlwaysOnBottomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Window = sender as Window;
            if (Window != null)
            {
                Window.Loaded += Window_Loaded;
                Window.Closing += Window_Closing;
            }
        }

        #endregion

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

        static void Window_Loaded(object sender, RoutedEventArgs e)
                {
                    IntPtr hWnd = new WindowInteropHelper(Window).Handle;
                    SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

                    IntPtr windowHandle = (new WindowInteropHelper(Window)).Handle;
                    HwndSource src = HwndSource.FromHwnd(windowHandle);
                    src.AddHook(new HwndSourceHook(WndProc));
                }

        static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IntPtr windowHandle = (new WindowInteropHelper(Window)).Handle;
            HwndSource src = HwndSource.FromHwnd(windowHandle);
            src.RemoveHook(new HwndSourceHook(WndProc));
        }

        static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETFOCUS)
            {
                hWnd = new WindowInteropHelper(Window).Handle;
                SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                handled = true;
            }
            return IntPtr.Zero;
        }

        #endregion
    }
}
