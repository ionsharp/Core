using Imagin.Common.Input.WinApi;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imagin.Common.Input
{
    /// <summary>
    /// This component monitors Application or Global input, depending on 
	/// <see cref="MouseKeyEventProvider.Enabled"/> and provides appropriate
	/// events.
	/// </summary>
    public class MouseKeyEventProvider : Component
    {
        #region Properties

        readonly KeyboardHookListener keyboardHookManager;

        readonly MouseHookListener mouseHookManager;

        bool DesignTimeEnabled
        {
            get; set;
        }

        bool RunTimeEnabled
        {
            get
            {
                return mouseHookManager.Enabled && keyboardHookManager.Enabled;
            }
            set
            {
                mouseHookManager.Enabled = value;
                keyboardHookManager.Enabled = value;
            }
        }

        /// <summary>
        /// This component raises events. The value is always true.
        /// </summary>
        protected override bool CanRaiseEvents
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or Sets the enabled status of the component.
        /// </summary>
        /// <value>
        /// True - The component is presently activated and will fire events.
        /// <para>
        /// False - The component is not active and will not fire events.
        /// </para>
        /// </value>
        public bool Enabled
        {
            get
            {
                return DesignMode ? DesignTimeEnabled : RunTimeEnabled;
            }
            set
            {
                if (DesignMode)
                {
                    DesignTimeEnabled = value;
                }
                else RunTimeEnabled = value;
            }
        }

        ///<summary>
        /// Indicates which hooks to listen to application or global.
        ///</summary>
        public HookType HookType
        {
            get
            {
                return mouseHookManager.IsGlobal ? HookType.Global : HookType.Application;
            }
            set
            {
                Hooker hooker;
                switch (value)
                {
                    case HookType.Global:
                        hooker = new GlobalHooker();
                        break;

                    case HookType.Application:
                        hooker = new AppHooker();
                        break;

                    default:
                        return;
                }

                mouseHookManager.Replace(hooker);
                keyboardHookManager.Replace(hooker);
            }
        }

        event KeyEventHandler keyDown;
        /// <summary>
        /// Activated when a key is pushed.
        /// </summary>
        public event KeyEventHandler KeyDown
        {
            add
            {
                if (keyDown == null)
                    keyboardHookManager.KeyDown += HookManager_KeyDown;
                keyDown += value;
            }
            remove
            {
                keyDown -= value;
                if (keyDown == null)
                    keyboardHookManager.KeyDown -= HookManager_KeyDown;
            }
        }

        event KeyPressEventHandler keyPress;
        /// <summary>
        /// Activated when the user presses a key.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public event KeyPressEventHandler KeyPress
        {
            add
            {
                if (keyPress == null)
                    keyboardHookManager.KeyPress += HookManager_KeyPress;
                keyPress += value;
            }
            remove
            {
                keyPress -= value;
                if (keyPress == null)
                    keyboardHookManager.KeyPress -= HookManager_KeyPress;
            }
        }

        event KeyEventHandler keyUp;
        /// <summary>
        /// Activated upon the release of a key.
        /// </summary>
        public event KeyEventHandler KeyUp
        {
            add
            {
                if (keyUp == null)
                    keyboardHookManager.KeyUp += HookManager_KeyUp;
                keyUp += value;
            }
            remove
            {
                keyUp -= value;
                if (keyUp == null)
                    keyboardHookManager.KeyUp -= HookManager_KeyUp;
            }
        }

        event MouseEventHandler mouseClick;
        /// <summary>
        /// Activated upon a single click of the mouse.
        /// </summary>
        public event MouseEventHandler MouseClick
        {
            add
            {
                if (mouseClick == null)
                {
                    mouseHookManager.MouseClick += HookManager_MouseClick;
                }
                mouseClick += value;
            }

            remove
            {
                mouseClick -= value;
                if (mouseClick == null)
                {
                    mouseHookManager.MouseClick -= HookManager_MouseClick;
                }
            }
        }

        event EventHandler<MouseEventExtArgs> mouseClickExt;
        /// <summary>
        /// Activated upon a single click of the mouse.
        /// </summary>
        /// <remarks>
        /// This event provides extended arguments of type <see cref="MouseEventArgs"/> enabling you to 
        /// supress further processing of mouse click in other applications.
        /// </remarks>
        public event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                // Disable warning that MouseClickExt is obsolete
#pragma warning disable 618
                if (mouseClickExt == null)
                {
                    mouseHookManager.MouseClickExt += HookManager_MouseClickExt;
                }
                mouseClickExt += value;
            }
            remove
            {
                mouseClickExt -= value;
                if (mouseClickExt == null)
                {
                    mouseHookManager.MouseClickExt -= HookManager_MouseClickExt;
                }
#pragma warning restore 618
            }
        }

        event MouseEventHandler mouseDoubleClick;
        /// <summary>
        /// Activated when the user double-clicks with the mouse.
        /// </summary>
        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                if (mouseDoubleClick == null)
                {
                    mouseHookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
                }
                mouseDoubleClick += value;
            }

            remove
            {
                mouseDoubleClick -= value;
                if (mouseDoubleClick == null)
                {
                    mouseHookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
                }
            }
        }

        event MouseEventHandler mouseDown;
        /// <summary>
        /// Activated when the user presses a mouse button.
        /// </summary>
        public event MouseEventHandler MouseDown
        {
            add
            {
                if (mouseDown == null)
                    mouseHookManager.MouseDown += HookManager_MouseDown;
                mouseDown += value;
            }

            remove
            {
                mouseDown -= value;
                if (mouseDown == null)
                    mouseHookManager.MouseDown -= HookManager_MouseDown;
            }
        }

        event EventHandler<MouseEventExtArgs> mouseDownExt;
        /// <summary>
        /// Activated when the user presses a mouse button.
        /// </summary>
        /// <remarks>
        /// This event provides extended arguments of type <see cref="MouseEventArgs"/> enabling you to 
        /// supress further processing of mouse down in other applications.
        /// </remarks>
        public event EventHandler<MouseEventExtArgs> MouseDownExt
        {
            add
            {
                if (mouseDownExt == null)
                    mouseHookManager.MouseDownExt += HookManager_MouseDownExt;
                mouseDownExt += value;
            }
            remove
            {
                mouseDownExt -= value;
                if (mouseDownExt == null)
                    mouseHookManager.MouseDownExt -= HookManager_MouseDownExt;
            }
        }

        event MouseEventHandler mouseMove;
        /// <summary>
        /// Activated when the user moves the mouse. 
        /// </summary>
        public event MouseEventHandler MouseMove
        {
            add
            {
                if (mouseMove == null)
                {
                    mouseHookManager.MouseMove += HookManager_MouseMove;
                }
                mouseMove += value;
            }

            remove
            {
                mouseMove -= value;
                if (mouseMove == null)
                {
                    mouseHookManager.MouseMove -= HookManager_MouseMove;
                }
            }
        }

        event EventHandler<MouseEventExtArgs> mouseMoveExt;
        /// <summary>
        /// Activated when the user moves the mouse. 
        /// </summary>
        /// <remarks>
        /// This event provides extended arguments of type <see cref="MouseEventArgs"/> enabling you to 
        /// supress further processing of mouse movement in other applications.
        /// </remarks>
        public event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                if (mouseMoveExt == null)
                    mouseHookManager.MouseMoveExt += HookManager_MouseMoveExt;
                mouseMoveExt += value;
            }

            remove
            {
                mouseMoveExt -= value;
                if (mouseMoveExt == null)
                    mouseHookManager.MouseMoveExt -= HookManager_MouseMoveExt;
            }
        }

        event EventHandler<MouseEventArgs> mouseWheel;
        /// <summary>
        /// Activated upon mouse scrolling.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseWheel
        {
            add
            {
                if (mouseWheel == null)
                    mouseHookManager.MouseWheel += HookManager_MouseWheel;
                mouseWheel += value;
            }

            remove
            {
                mouseWheel -= value;
                if (mouseWheel == null)
                {
                    mouseHookManager.MouseWheel -= HookManager_MouseWheel;
                }
            }
        }

        event MouseEventHandler mouseUp;
        /// <summary>
        /// Activated when the user releases a mouse button.
        /// </summary>
        public event MouseEventHandler MouseUp
        {
            add
            {
                if (mouseUp == null)
                    mouseHookManager.MouseUp += HookManager_MouseUp;
                mouseUp += value;
            }

            remove
            {
                mouseUp -= value;
                if (mouseUp == null)
                {
                    mouseHookManager.MouseUp -= HookManager_MouseUp;
                }
            }
        }

        #endregion

        #region MouseKeyEventProvider

        /// <summary>
        /// Initializes a new instance of <see cref="MouseKeyEventProvider"/>
        /// </summary>
        public MouseKeyEventProvider()
        {
            keyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            mouseHookManager = new MouseHookListener(new GlobalHooker());
        }

        #endregion

        #region Methods

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            RaiseEventOnUIThread(keyDown, e);
        }

        void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (keyPress != null)
                RaiseEventOnUIThread(keyPress, e);
        }

        void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyUp != null)
                RaiseEventOnUIThread(keyUp, e);
        }

        void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseMove != null)
                RaiseEventOnUIThread(mouseMove, e);
        }

        void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseClick != null)
                RaiseEventOnUIThread(mouseClick, e);
        }

        void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            if (mouseDown != null)
                RaiseEventOnUIThread(mouseDown, e);
        }

        void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseUp != null)
                RaiseEventOnUIThread(mouseUp, e);
        }

        void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (mouseDoubleClick != null)
                RaiseEventOnUIThread(mouseDoubleClick, e);
        }

        void HookManager_MouseMoveExt(object sender, MouseEventExtArgs e)
        {
            if (mouseMoveExt != null)
            {
                RaiseEventOnUIThread(mouseMoveExt, e);
            }
        }

        void HookManager_MouseClickExt(object sender, MouseEventExtArgs e)
        {
            if (mouseClickExt != null)
                RaiseEventOnUIThread(mouseClickExt, e);
        }

        void HookManager_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (mouseDownExt != null)
                RaiseEventOnUIThread(mouseDownExt, e);
        }

        void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            if (mouseWheel != null)
                RaiseEventOnUIThread(mouseWheel, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputEvent"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Modified from http://stackoverflow.com/questions/1698889/raise-events-in-net-on-the-main-ui-thread
        /// </remarks>
        void RaiseEventOnUIThread(Delegate inputEvent, EventArgs e)
        {
            object sender = this;
            foreach (Delegate d in inputEvent.GetInvocationList())
            {
                ISynchronizeInvoke syncer = d.Target as ISynchronizeInvoke;

                if (syncer == null)
                {
                    d.DynamicInvoke(new[] { sender, e });
                }
                else
                {
                    // I don't know if ASyncronous is really the way to go.
                    //  If the programmer wants to suppress input,
                    //  will asyncronous make that happen consistently?

                    //syncer.EndInvoke(syncer.BeginInvoke(inputEvent, new[] { sender, e }));
                    syncer.Invoke(inputEvent, new[] { sender, e });
                }
            }
        }

        #endregion
    }
}
