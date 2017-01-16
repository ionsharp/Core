using Imagin.Common.Input;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowBase : Window, INotifyPropertyChanged
    {
        #region Properties

        bool isLoaded = false;

        /// <summary>
        /// The result of the window subsequent to closing.
        /// </summary>
        public WindowResult Result = WindowResult.Unknown;

        /// <summary>
        /// Occurs when the window is closed.
        /// </summary>
        public new event EventHandler<EventArgs<WindowResult>> Closed;

        /// <summary>
        /// Occurs when the window is loaded for the first time.
        /// </summary>
        public event EventHandler<EventArgs> FirstLoad;

        /// <summary>
        /// Occurs when the window is hidden.
        /// </summary>
        public event EventHandler<EventArgs> Hidden;

        /// <summary>
        /// Occurs when a window is shown.
        /// </summary>
        public event EventHandler<EventArgs> Shown;

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowBase), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            private set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.Register("IsHidden", typeof(bool), typeof(WindowBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Whether or not the window is currently hidden.
        /// </summary>
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

        #endregion

        #region Methods

        #region Commands

        ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                closeCommand = closeCommand ?? new RelayCommand(x => Close(), true);
                return closeCommand;
            }
        }

        ICommand hideCommand;
        public ICommand HideCommand
        {
            get
            {
                hideCommand = hideCommand ?? new RelayCommand(x => Hide(), true);
                return hideCommand;
            }
        }

        ICommand maximizeCommand;
        public ICommand MaximizeCommand
        {
            get
            {
                maximizeCommand = maximizeCommand ?? new RelayCommand(x => SystemCommands.MaximizeWindow(this), true);
                return maximizeCommand;
            }
        }

        ICommand minimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                minimizeCommand = minimizeCommand ?? new RelayCommand(x => SystemCommands.MinimizeWindow(this), true);
                return minimizeCommand;
            }
        }

        ICommand restoreCommand;
        public ICommand RestoreCommand
        {
            get
            {
                restoreCommand = restoreCommand ?? new RelayCommand(x => SystemCommands.RestoreWindow(this), true);
                return restoreCommand;
            }
        }

        ICommand showCommand;
        public ICommand ShowCommand
        {
            get
            {
                showCommand = showCommand ?? new RelayCommand(x => Show(), true);
                return showCommand;
            }
        }

        #endregion

        #region New

        public new void Show()
        {
            base.Show();
            IsHidden = false;
            OnShown();
        }

        public new void Hide()
        {
            base.Hide();
            IsHidden = true;
            OnHidden();
        }

        public new bool? ShowDialog()
        {
            OnShown();
            return base.ShowDialog();
        }

        public new void Close()
        {
            base.Close();
            OnClosed();
        }

        #endregion

        #region Virtual

        protected virtual void OnClosed()
        {
            if (Closed != null)
                Closed(this, new EventArgs<WindowResult>(Result));
        }

        protected virtual void OnFirstLoad()
        {
            if (FirstLoad != null)
                FirstLoad(this, new EventArgs());
        }

        protected virtual void OnHidden()
        {
            if (Hidden != null)
                Hidden(this, new EventArgs());
        }

        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                OnFirstLoad();
            }
        }

        protected virtual void OnShown()
        {
            if (Shown != null)
                Shown(this, new EventArgs());
        }

        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
        }

        #endregion

        #endregion

        #region WindowBase

        /// <summary>
        /// 
        /// </summary>
        public WindowBase() : base()
        {
            Loaded += (s, e) => OnLoaded(e);
            Unloaded += (s, e) => OnUnloaded(e);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
