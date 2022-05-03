namespace Imagin.Common.Native
{
    public enum WindowMessages
    {
        /// <summary>Sent after a window has been moved.</summary>
        WM_MOVE = 0x0003,
        /// <summary>
        /// Sent to a window when the size or position of the window is about to change.
        /// An application can use this message to override the window's default maximized size and position,
        /// or its default minimum or maximum tracking size.
        /// </summary>
        WM_GETMINMAXINFO = 0x0024,
        /// <summary>
        /// Sent to a window whose size, position, or place in the Z order is about to change as a result
        /// of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x0046,
        /// <summary>
        /// Sent to a window whose size, position, or place in the Z order has changed as a result of a
        /// call to the SetWindowPos function or another window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x0047,
        /// <summary>
        /// Sent to a window that the user is moving. By processing this message, an application can monitor
        /// the position of the drag rectangle and, if needed, change its position.
        /// </summary>
        WM_MOVING = 0x0216,
        /// <summary>
        /// Sent once to a window after it enters the moving or sizing modal loop. The window enters the
        /// moving or sizing modal loop when the user clicks the window's title bar or sizing border, or
        /// when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam
        /// parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete
        /// when DefWindowProc returns.
        /// <para />
        /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows
        /// is enabled.
        /// </summary>
        WM_ENTERSIZEMOVE = 0x0231,
        /// <summary>
        /// Sent once to a window once it has exited moving or sizing modal loop. The window enters the
        /// moving or sizing modal loop when the user clicks the window's title bar or sizing border, or
        /// when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the
        /// wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is
        /// complete when DefWindowProc returns.
        /// </summary>
        WM_EXITSIZEMOVE = 0x0232,
        WM_SIZING = 0x0214,
    }
}