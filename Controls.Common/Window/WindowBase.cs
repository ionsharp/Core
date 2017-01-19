using Imagin.Common.Input;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using Imagin.Common.Extensions;

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
        /// Occurs when the window is about to close.
        /// </summary>
        public new event EventHandler<EventArgs<WindowResult>> Closed;

        /// <summary>
        /// Occurs directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        public new event CancelEventHandler Closing;

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

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowBase), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// Identifies the <see cref="IsHidden"/> dependency property.
        /// </summary>
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

        #region Methods

        #region Commands
        
        ICommand beginCloseCommand;
        public ICommand BeginCloseCommand
        {
            get
            {
                beginCloseCommand = beginCloseCommand ?? new RelayCommand(async x => await BeginClose(x != null ? x.To<bool>() : true), x => x == null || x is bool);
                return beginCloseCommand;
            }
        }

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

        #region Public

        public async Task BeginClose(bool SupportsCancellation = true)
        {
            var e = new CancelEventArgs(false);

            await OnBeginClosing(e);

            if (!e.Cancel)
            {
                await OnBeginClosed(Result);
                base.Close();
            }
        }

        public new void Close()
        {
            var e = new CancelEventArgs(false);

            OnClosing(e);

            if (!e.Cancel)
            {
                OnClosed(Result);
                base.Close();
            }
        }

        /// <summary>
        /// Manually closes a window.
        /// </summary>
        /// <param name="SupportsCancellation">Whether or not cancellation is supported.</param>
        public void Close(bool SupportsCancellation)
        {
            if (!SupportsCancellation)
            {
                base.Close();
            }
            else Close();
        }

        public new void Hide()
        {
            base.Hide();
            IsHidden = true;
            OnHidden();
        }

        public new void Show()
        {
            base.Show();
            IsHidden = false;
            OnShown();
        }

        public new bool? ShowDialog()
        {
            OnShown();
            return base.ShowDialog();
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Occurs when the window is about to close (async).
        /// </summary>
        protected virtual async Task OnBeginClosed(WindowResult Result)
        {
            await Task.Run(() => Closed?.Invoke(this, new EventArgs<WindowResult>(Result)));
        }

        /// <summary>
        /// Occurs directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure (async).
        /// </summary>
        protected virtual async Task OnBeginClosing(CancelEventArgs e)
        {
            await Task.Run(() => Closing?.Invoke(this, e));
        }

        [Obsolete("Do not use.", true)]
        protected sealed override void OnClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        protected virtual void OnClosed(WindowResult Result)
        {
            Closed?.Invoke(this, new EventArgs<WindowResult>(Result));
        }

        /// <summary>
        /// Occurs directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        protected virtual void OnFirstLoad()
        {
            FirstLoad?.Invoke(this, new EventArgs());
        }

        protected virtual void OnHidden()
        {
            Hidden?.Invoke(this, new EventArgs());
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
            Shown?.Invoke(this, new EventArgs());
        }

        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
        }

        #endregion

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
