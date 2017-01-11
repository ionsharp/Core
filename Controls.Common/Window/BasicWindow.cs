using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicWindow : WindowBase
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

        public static readonly DependencyProperty ButtonsPanelProperty = DependencyProperty.Register("ButtonsPanel", typeof(ItemsPanelTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ItemsPanelTemplate ButtonsPanel
        {
            get
            {
                return (ItemsPanelTemplate)GetValue(ButtonsPanelProperty);
            }
            set
            {
                SetValue(ButtonsPanelProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonsStyleProperty = DependencyProperty.Register("ButtonsStyle", typeof(Style), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style ButtonsStyle
        {
            get
            {
                return (Style)GetValue(ButtonsStyleProperty);
            }
            set
            {
                SetValue(ButtonsStyleProperty, value);
            }
        }
        
        public static DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register("ContentBorderBrush", typeof(Brush), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the border brush of the content.
        /// </summary>
        public Brush ContentBorderBrush
        {
            get
            {
                return (Brush)GetValue(ContentBorderBrushProperty);
            }
            set
            {
                SetValue(ContentBorderBrushProperty, value);
            }
        }

        public static DependencyProperty ContentBorderThicknessProperty = DependencyProperty.Register("ContentBorderThickness", typeof(Thickness), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the border thickness of the content.
        /// </summary>
        public Thickness ContentBorderThickness
        {
            get
            {
                return (Thickness)GetValue(ContentBorderThicknessProperty);
            }
            set
            {
                SetValue(ContentBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the outer margin of the content.
        /// </summary>
        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)GetValue(ContentMarginProperty);
            }
            set
            {
                SetValue(ContentMarginProperty, value);
            }
        }

        public static DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Footer
        {
            get
            {
                return GetValue(FooterProperty);
            }
            set
            {
                SetValue(FooterProperty, value);
            }
        }

        public static DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate FooterTemplate
        {
            get
            {
                return (DataTemplate)GetValue(FooterTemplateProperty);
            }
            set
            {
                SetValue(FooterTemplateProperty, value);
            }
        }

        public static DependencyProperty FooterTemplateSelectorProperty = DependencyProperty.Register("FooterTemplateSelector", typeof(DataTemplateSelector), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplateSelector FooterTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(FooterTemplateSelectorProperty);
            }
            set
            {
                SetValue(FooterTemplateSelectorProperty, value);
            }
        }

        public static DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate IconTemplate
        {
            get
            {
                return (DataTemplate)GetValue(IconTemplateProperty);
            }
            set
            {
                SetValue(IconTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.Register("IsHidden", typeof(bool), typeof(BasicWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(object), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Element to place on top of everything else; element covers entire window.
        /// </summary>
        public object Overlay
        {
            get
            {
                return GetValue(OverlayProperty);
            }
            set
            {
                SetValue(OverlayProperty, value);
            }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(BasicWindow), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The visibility of the overlay element.
        /// </summary>
        public Visibility OverlayVisibility
        {
            get
            {
                return (Visibility)GetValue(OverlayVisibilityProperty);
            }
            set
            {
                SetValue(OverlayVisibilityProperty, value);
            }
        }
        
        public static DependencyProperty ResizeGripTemplateProperty = DependencyProperty.Register("ResizeGripTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ResizeGripTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ResizeGripTemplateProperty);
            }
            set
            {
                SetValue(ResizeGripTemplateProperty, value);
            }
        }

        public static DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate TitleTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TitleTemplateProperty);
            }
            set
            {
                SetValue(TitleTemplateProperty, value);
            }
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

        protected virtual void OnHidden()
        {
            if (Hidden != null)
                Hidden(this, new EventArgs());
        }

        protected virtual void OnInitialized()
        {
            Buttons = new FrameworkElementCollection();

            Loaded += (s, e) => OnLoaded(e);
            Unloaded += (s, e) => OnUnloaded(e);
        }

        protected virtual void OnFirstLoad()
        {
            if (FirstLoad != null)
                FirstLoad(this, new EventArgs());
        }

        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                OnFirstLoad();
            }
        }

        protected virtual void OnUnloaded(RoutedEventArgs e)
        {
        }

        protected virtual void OnShown()
        {
            if (Shown != null)
                Shown(this, new EventArgs());
        }

        #endregion

        #endregion

        #region BasicWindow

        public BasicWindow() : base()
        {
            DefaultStyleKey = typeof(BasicWindow);
            OnInitialized();
        }

        public BasicWindow(Action OnClosed) : this()
        {
            if (OnClosed != null)
                Closed += (s, e) => OnClosed.Invoke();
        }

        #endregion
    }
}
