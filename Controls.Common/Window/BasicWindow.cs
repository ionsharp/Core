using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class BasicWindow : WindowBase
    {
        #region Properties

        bool isInitialLoaded = false;

        protected virtual CommandBinding[] Commands
        {
            get
            {
                return new CommandBinding[]
                {
                    new CommandBinding(CloseCommand, CloseCommand_Executed, CloseCommand_CanExecute),
                    new CommandBinding(HideCommand, HideCommand_Executed, HideCommand_CanExecute),
                    new CommandBinding(MaximizeCommand, MaximizeCommand_Executed, MaximizeCommand_CanExecute),
                    new CommandBinding(MinimizeCommand, MinimizeCommand_Executed, MinimizeCommand_CanExecute),
                    new CommandBinding(RestoreCommand, RestoreCommand_Executed, RestoreCommand_CanExecute),
                    new CommandBinding(ShowCommand, ShowCommand_Executed, ShowCommand_CanExecute),
                };
            }
        }

        public WindowResult Result = WindowResult.Unknown;

        #region Dependency

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FrameworkElementCollection Buttons
        {
            get
            {
                return (FrameworkElementCollection)GetValue(ButtonsProperty);
            }
            set
            {
                SetValue(ButtonsProperty, value);
            }
        }

        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.Register("IsHidden", typeof(bool), typeof(BasicWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsHidden
        {
            get
            {
                return (bool)GetValue(IsHiddenProperty);
            }
            private set
            {
                SetValue(IsHiddenProperty, value);
            }
        }

        public static DependencyProperty StartupLocationProperty = DependencyProperty.Register("StartupLocation", typeof(WindowLocation), typeof(BasicWindow), new FrameworkPropertyMetadata(WindowLocation.CenterScreen, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStartupLocationChanged));
        public WindowLocation StartupLocation
        {
            get
            {
                return (WindowLocation)GetValue(StartupLocationProperty);
            }
            set
            {
                SetValue(StartupLocationProperty, value);
            }
        }
        static void OnStartupLocationChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<BasicWindow>().WindowStartupLocation = (WindowStartupLocation)Enum.Parse(typeof(WindowStartupLocation), e.NewValue.As<WindowLocation>().ToString());
        }

        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(WindowType), typeof(BasicWindow), new FrameworkPropertyMetadata(WindowType.Window, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public WindowType Type
        {
            get
            {
                return (WindowType)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        #endregion

        #region Events

        public new event EventHandler<EventArgs<WindowResult>> Closed;

        public event EventHandler<EventArgs> Hidden;

        public event EventHandler<EventArgs> InitialLoaded;

        public event EventHandler<EventArgs> Shown;

        #endregion

        #endregion

        #region Methods

        #region Commands

        public static readonly RoutedUICommand CloseCommand = new RoutedUICommand("CloseCommand", "CloseCommand", typeof(BasicWindow));
        void CloseCommand_Executed(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand HideCommand = new RoutedUICommand("HideCommand", "HideCommand", typeof(BasicWindow));
        void HideCommand_Executed(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        void HideCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand MaximizeCommand = new RoutedUICommand("MaximizeCommand", "MaximizeCommand", typeof(BasicWindow));
        void MaximizeCommand_Executed(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }
        void MaximizeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand MinimizeCommand = new RoutedUICommand("MinimizeCommand", "MinimizeCommand", typeof(BasicWindow));
        void MinimizeCommand_Executed(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        void MinimizeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand RestoreCommand = new RoutedUICommand("RestoreCommand", "RestoreCommand", typeof(BasicWindow));
        void RestoreCommand_Executed(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
        void RestoreCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand ShowCommand = new RoutedUICommand("ShowCommand", "ShowCommand", typeof(BasicWindow));
        void ShowCommand_Executed(object sender, RoutedEventArgs e)
        {
            Show();
        }
        void ShowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region New

        public new void Show()
        {
            base.Show();
            this.OnShown();
        }

        public new void Hide()
        {
            base.Hide();
            this.OnHidden();
        }

        public new bool? ShowDialog()
        {
            this.OnShown();
            return base.ShowDialog();
        }

        public new void Close()
        {
            base.Close();
            this.OnClosed();
        }

        #endregion

        #region Virtual

        protected virtual void OnClosed()
        {
            if (this.Closed != null)
                this.Closed(this, new EventArgs<WindowResult>(Result));
        }

        protected virtual void OnHidden()
        {
            this.IsHidden = true;
            if (this.Hidden != null)
                this.Hidden(this, new EventArgs());
        }

        protected virtual void OnInitialized()
        {
            Buttons = new FrameworkElementCollection();

            if (Commands != null)
            {
                foreach (var c in Commands)
                    CommandBindings.Add(c);
            }

            Loaded += (s, e) => OnLoaded(e);
            Unloaded += (s, e) => OnUnloaded(e);
        }

        protected virtual void OnInitialLoaded()
        {
        }

        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            Button MinimizeButton = this.Template.FindName("PART_MinimizeButton", this) as Button;
            if (MinimizeButton != null)
                MinimizeButton.Click += (a, b) => this.WindowState = WindowState.Minimized;
            Button RestoreButton = this.Template.FindName("PART_RestoreButton", this) as Button;
            if (RestoreButton != null)
                RestoreButton.Click += (a, b) => this.WindowState = WindowState.Normal;
            Button MaximizeButton = this.Template.FindName("PART_MaximizeButton", this) as Button;
            if (MaximizeButton != null)
                MaximizeButton.Click += (a, b) => this.WindowState = WindowState.Maximized;
            Button CloseButton = this.Template.FindName("PART_CloseButton", this) as Button;
            if (CloseButton != null)
                CloseButton.Click += (a, b) => this.Close();

            if (!this.isInitialLoaded)
            {
                this.isInitialLoaded = true;
                if (this.InitialLoaded != null)
                    this.InitialLoaded(this, new EventArgs());
                this.OnInitialLoaded();
            }
        }

        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
        }

        protected virtual void OnShown()
        {
            this.IsHidden = false;
            if (this.Shown != null)
                this.Shown(this, new EventArgs());
        }

        #endregion

        #endregion

        #region BasicWindow

        public BasicWindow() : base()
        {
            DefaultStyleKey = typeof(BasicWindow);
            OnInitialized();
        }

        public BasicWindow(Action OnClosed) : base()
        {
            DefaultStyleKey = typeof(BasicWindow);
            OnInitialized();

            if (OnClosed != null)
                this.Closed += (s, e) => OnClosed.Invoke();
        }

        #endregion
    }
}
