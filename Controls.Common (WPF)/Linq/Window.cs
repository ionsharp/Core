using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WindowExtensions
    {
        #region Classes

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }

        #endregion

        #region Properties

        #region Always On Bottom

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SinkerProperty = DependencyProperty.RegisterAttached("Sinker", typeof(WindowSinker), typeof(WindowExtensions), new UIPropertyMetadata(null));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static WindowSinker GetSinker(DependencyObject obj)
        {
            return (WindowSinker)obj.GetValue(SinkerProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetSinker(DependencyObject obj, WindowSinker value)
        {
            obj.SetValue(SinkerProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AlwaysOnBottomProperty = DependencyProperty.RegisterAttached("AlwaysOnBottom", typeof(bool), typeof(WindowExtensions), new UIPropertyMetadata(false, OnAlwaysOnBottomChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetAlwaysOnBottom(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlwaysOnBottomProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetAlwaysOnBottom(DependencyObject obj, bool value)
        {
            obj.SetValue(AlwaysOnBottomProperty, value);
        }
        static void OnAlwaysOnBottomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Window = sender as Window;
            if (Window != null)
            {
                if ((bool)e.NewValue)
                {
                    var Sinker = new WindowSinker(Window);
                    Sinker.Sink();
                    SetSinker(Window, Sinker);
                }
                else
                {
                    var Sinker = GetSinker(Window);
                    Sinker.Unsink();
                    SetSinker(Window, null);
                }
            }
        }

        #endregion

        #region Icon

        /// <summary>
        /// Gets or sets a value indicating whether window should implement default behavior; this is only applicable when window
        /// a) overrides default style,
        /// b) allows transparency, and
        /// c) style is set to 'None'.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof(ImageSource), typeof(WindowExtensions), new PropertyMetadata(null));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ImageSource GetIcon(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIcon(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconProperty, value);
        }

        #endregion

        #region OverridesDefaultBehavior

        /// <summary>
        /// Gets or sets a value indicating whether window should implement default behavior; this is only applicable when window
        /// a) overrides default style,
        /// b) allows transparency, and
        /// c) style is set to 'None'.
        /// </summary>
        public static readonly DependencyProperty OverridesDefaultBehaviorProperty = DependencyProperty.RegisterAttached("OverridesDefaultBehavior", typeof(bool), typeof(WindowExtensions), new PropertyMetadata(false, OnOverridesDefaultBehaviorChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetOverridesDefaultBehavior(DependencyObject obj)
        {
            return (bool)obj.GetValue(OverridesDefaultBehaviorProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetOverridesDefaultBehavior(DependencyObject obj, bool value)
        {
            obj.SetValue(OverridesDefaultBehaviorProperty, value);
        }
        static void OnOverridesDefaultBehaviorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Window = sender as Window;
            if (Window != null && (bool)e.NewValue)
            {
                Window.SourceInitialized += (a, b) =>
                {
                    var Handle = (new WindowInteropHelper(Window)).Handle;
                    HwndSource.FromHwnd(Handle).AddHook(new HwndSourceHook(WindowProc));

                    Window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
                    Window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
                    Window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
                    Window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
                };
            }
        }

        #endregion

        #endregion

        #region Methods

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        static void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            var Window = sender as Window;
            e.CanExecute = Window.ResizeMode == ResizeMode.CanResize || Window.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        static void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(sender as Window);
        }

        static void OnMaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as Window).WindowState = WindowState.Maximized;
        }

        static void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(sender as Window);
        }

        static void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as Window).ResizeMode != ResizeMode.NoResize;
        }

        static void OnRestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(sender as Window);
        }

        static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            var MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {

                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);

                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        #endregion

        #region Structures

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            /// <summary>
            /// 
            /// </summary>
            public POINT ptReserved;
            /// <summary>
            /// 
            /// </summary>
            public POINT ptMaxSize;
            /// <summary>
            /// 
            /// </summary>
            public POINT ptMaxPosition;
            /// <summary>
            /// 
            /// </summary>
            public POINT ptMinTrackSize;
            /// <summary>
            /// 
            /// </summary>
            public POINT ptMaxTrackSize;
        };

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;

            /// <summary> Win32 </summary>
            public int top;

            /// <summary> Win32 </summary>
            public int right;

            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }

            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }

            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }

            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

        #endregion
    }
}
