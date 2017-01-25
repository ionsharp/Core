using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Button", Type = typeof(Button))]
    public class FileBox : TextBoxExt
    {
        #region Classes

        /// <summary>
        /// 
        /// </summary>
        public class ValidatePathHandler : IValidate<object>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Arguments"></param>
            /// <returns></returns>
            public bool Validate(params object[] Arguments)
            {
                var Mode = Arguments[0].To<DialogProviderMode>();
                var Path = Arguments[1].ToString();

                var Result = false;

                switch (Mode)
                {
                    case DialogProviderMode.OpenFile:
                        Result = Path.FileExists();
                        break;
                    case DialogProviderMode.OpenFolder:
                        Result = Path.DirectoryExists();
                        break;
                }

                return Result;
            }
        }

        #endregion

        #region Properties

        Button PART_Button { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler DialogOpened;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseModeProperty = DependencyProperty.Register("BrowseMode", typeof(DialogProviderMode), typeof(FileBox), new FrameworkPropertyMetadata(DialogProviderMode.OpenFolder, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBrowseModeChanged));
        /// <summary>
        /// Gets or sets the type of mode to browse in.
        /// </summary>
        public DialogProviderMode BrowseMode
        {
            get
            {
                return (DialogProviderMode)GetValue(BrowseModeProperty);
            }
            set
            {
                SetValue(BrowseModeProperty, value);
            }
        }
        static void OnBrowseModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<FileBox>().OnBrowseModeChanged((DialogProviderMode)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseTitleProperty = DependencyProperty.Register("BrowseTitle", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata("Browse...", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the title of the dialog used to select a path.
        /// </summary>
        public string BrowseTitle
        {
            get
            {
                return (string)GetValue(BrowseTitleProperty);
            }
            set
            {
                SetValue(BrowseTitleProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ButtonPositionProperty = DependencyProperty.Register("ButtonPosition", typeof(LeftRight), typeof(FileBox), new FrameworkPropertyMetadata(LeftRight.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the position of the button used to show a dialog.
        /// </summary>
        public LeftRight ButtonPosition
        {
            get
            {
                return (LeftRight)GetValue(ButtonPositionProperty);
            }
            set
            {
                SetValue(ButtonPositionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ButtonToolTipProperty = DependencyProperty.Register("ButtonToolTip", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata("Click to browse...", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the tool tip of the button used to show a dialog.
        /// </summary>
        public string ButtonToolTip
        {
            get
            {
                return (string)GetValue(ButtonToolTipProperty);
            }
            set
            {
                SetValue(ButtonToolTipProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(FileBox), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the visibility of the button used to show a dialog.
        /// </summary>
        public Visibility ButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(ButtonVisibilityProperty);
            }
            set
            {
                SetValue(ButtonVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanBrowseProperty = DependencyProperty.Register("CanBrowse", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not the button used to show a dialog is enabled.
        /// </summary>
        public bool CanBrowse
        {
            get
            {
                return (bool)GetValue(CanBrowseProperty);
            }
            set
            {
                SetValue(CanBrowseProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanValidateProperty = DependencyProperty.Register("CanValidate", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCanValidateChanged));
        /// <summary>
        /// Gets or sets whether or not the path should be validated.
        /// </summary>
        public bool CanValidate
        {
            get
            {
                return (bool)GetValue(CanValidateProperty);
            }
            set
            {
                SetValue(CanValidateProperty, value);
            }
        }
        static void OnCanValidateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<FileBox>().OnCanValidateChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// Gets whether or not the path is valid, if validation is enabled.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            private set
            {
                SetValue(IsValidProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValidateHandlerProperty = DependencyProperty.Register("ValidateHandler", typeof(IValidate<object>), typeof(FileBox), new FrameworkPropertyMetadata(default(IValidate<object[]>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets an object that implements <see cref="IValidate{T}"/> and handles validation when enabled.
        /// </summary>
        public IValidate<object> ValidateHandler
        {
            get
            {
                return (IValidate<object>)GetValue(ValidateHandlerProperty);
            }
            set
            {
                SetValue(ValidateHandlerProperty, value);
            }
        }

        #endregion

        #region FileBox

        /// <summary>
        /// 
        /// </summary>
        public FileBox()
        {
            DefaultStyleKey = typeof(FileBox);
            ValidateHandler = new ValidatePathHandler();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnBrowseModeChanged(DialogProviderMode Value)
        {
            OnCanValidateChanged(CanValidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDialogOpened(RoutedEventArgs e)
        {
            DialogOpened?.Invoke(this, e);

            string[] Paths;
            if (DialogProvider.Show(out Paths, BrowseTitle, BrowseMode, DialogProviderSelectionMode.Single, null, Text))
                Text = Paths[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            OnCanValidateChanged(CanValidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnCanValidateChanged(bool Value)
        {
            IsValid = Value && ValidateHandler != null ? ValidateHandler.Validate(BrowseMode, Text) : false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Button = Template.FindName("PART_Button", this) as Button;
            PART_Button.Click += (s, e) => OnDialogOpened(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Browse()
        {
            OnDialogOpened(new RoutedEventArgs());
        }

        #endregion
    }
}
