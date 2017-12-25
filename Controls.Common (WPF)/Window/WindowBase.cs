using Imagin.Common.Input;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
        public int Result = -1;

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        public new event EventHandler<EventArgs<int>> Closed;

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
        /// <summary>
        /// 
        /// </summary>
        public ICommand BeginCloseCommand
        {
            get
            {
                beginCloseCommand = beginCloseCommand ?? new RelayCommand(async () => await BeginClose(), () => true);
                return beginCloseCommand;
            }
        }

        ICommand closeCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                closeCommand = closeCommand ?? new RelayCommand(() => Close(), () => true);
                return closeCommand;
            }
        }

        ICommand hideCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand HideCommand
        {
            get
            {
                hideCommand = hideCommand ?? new RelayCommand(() => Hide(), () => !IsHidden);
                return hideCommand;
            }
        }

        ICommand maximizeCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand MaximizeCommand
        {
            get
            {
                maximizeCommand = maximizeCommand ?? new RelayCommand(() => SystemCommands.MaximizeWindow(this), () => WindowState == WindowState.Normal);
                return maximizeCommand;
            }
        }

        ICommand minimizeCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand MinimizeCommand
        {
            get
            {
                minimizeCommand = minimizeCommand ?? new RelayCommand(() => SystemCommands.MinimizeWindow(this), () => true);
                return minimizeCommand;
            }
        }

        ICommand restoreCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand RestoreCommand
        {
            get
            {
                restoreCommand = restoreCommand ?? new RelayCommand(() => SystemCommands.RestoreWindow(this), () => WindowState == WindowState.Maximized);
                return restoreCommand;
            }
        }

        ICommand showCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ShowCommand
        {
            get
            {
                showCommand = showCommand ?? new RelayCommand(() => Show(), () => IsHidden);
                return showCommand;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupportsCancellation"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public new void Hide()
        {
            base.Hide();
            IsHidden = true;
            OnHidden();
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Show()
        {
            base.Show();
            IsHidden = false;
            OnShown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        protected virtual async Task OnBeginClosed(int Result)
        {
            await Task.Run(() => Closed?.Invoke(this, new EventArgs<int>(Result)));
        }

        /// <summary>
        /// Occurs directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure (async).
        /// </summary>
        protected virtual async Task OnBeginClosing(CancelEventArgs e)
        {
            await Task.Run(() => Closing?.Invoke(this, e));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        [Obsolete("Do not use.", true)]
        protected sealed override void OnClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        protected virtual void OnClosed(int Result)
        {
            Closed?.Invoke(this, new EventArgs<int>(Result));
        }

        /// <summary>
        /// Occurs directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnFirstLoad()
        {
            FirstLoad?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnHidden()
        {
            Hidden?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                OnFirstLoad();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnShown()
        {
            Shown?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
