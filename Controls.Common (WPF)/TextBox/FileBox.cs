using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FileBox : TextBox
    {
        #region Classes

        /// <summary>
        /// 
        /// </summary>
        public sealed class FileBoxValidateHandler : LocalPathValidateHandler
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Arguments"></param>
            /// <returns></returns>
            public override bool Validate(params object[] Arguments)
            {
                var Path = Arguments[0].ToString();
                var Mode = Arguments[1].To<WindowsDialogMode>();

                switch (Mode)
                {
                    case WindowsDialogMode.OpenFile:
                    case WindowsDialogMode.OpenFolder:
                        FileOrFolder = Mode == WindowsDialogMode.OpenFile;
                        return Validate(Path);
                }

                return false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler DialogOpened;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseButtonTemplateProperty = DependencyProperty.Register("BrowseButtonTemplate", typeof(DataTemplate), typeof(FileBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate BrowseButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(BrowseButtonTemplateProperty);
            }
            set
            {
                SetValue(BrowseButtonTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseButtonToolTipProperty = DependencyProperty.Register("BrowseButtonToolTip", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string BrowseButtonToolTip
        {
            get
            {
                return (string)GetValue(BrowseButtonToolTipProperty);
            }
            set
            {
                SetValue(BrowseButtonToolTipProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseModeProperty = DependencyProperty.Register("BrowseMode", typeof(WindowsDialogMode), typeof(FileBox), new FrameworkPropertyMetadata(WindowsDialogMode.OpenFolder, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBrowseModeChanged));
        /// <summary>
        /// Gets or sets the type of objects to browse.
        /// </summary>
        public WindowsDialogMode BrowseMode
        {
            get
            {
                return (WindowsDialogMode)GetValue(BrowseModeProperty);
            }
            set
            {
                SetValue(BrowseModeProperty, value);
            }
        }
        static void OnBrowseModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<FileBox>().OnBrowseModeChanged((WindowsDialogMode)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty BrowseTitleProperty = DependencyProperty.Register("BrowseTitle", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the title of the dialog used to browse objects.
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
        public static DependencyProperty CanBrowseProperty = DependencyProperty.Register("CanBrowse", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not browsing objects is enabled.
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
        /// Gets or sets whether or not the input (or file or folder path) should be validated.
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
        /// If validation is enabled, gets whether or not the input (or file or folder path) is valid.
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
        public static DependencyProperty ShowBrowseButtonProperty = DependencyProperty.Register("ShowBrowseButton", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not to show the button used to browse.
        /// </summary>
        public bool ShowBrowseButton
        {
            get
            {
                return (bool)GetValue(ShowBrowseButtonProperty);
            }
            set
            {
                SetValue(ShowBrowseButtonProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValidateHandlerProperty = DependencyProperty.Register("ValidateHandler", typeof(IValidate<object>), typeof(FileBox), new FrameworkPropertyMetadata(default(IValidate<object[]>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// If validation is enabled, gets or sets an object that implements <see cref="IValidate{T}"/>, which is used to validate the input (or file or folder path).
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValidityIndicatorTemplateProperty = DependencyProperty.Register("ValidityIndicatorTemplate", typeof(DataTemplate), typeof(FileBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate ValidityIndicatorTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ValidityIndicatorTemplateProperty);
            }
            set
            {
                SetValue(ValidityIndicatorTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ValidityIndicatorToolTipProperty = DependencyProperty.Register("ValidityIndicatorToolTip", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string ValidityIndicatorToolTip
        {
            get
            {
                return (string)GetValue(ValidityIndicatorToolTipProperty);
            }
            set
            {
                SetValue(ValidityIndicatorToolTipProperty, value);
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
            SetCurrentValue(ValidateHandlerProperty, new FileBoxValidateHandler());
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnBrowseModeChanged(WindowsDialogMode Value)
        {
            OnCanValidateChanged(CanValidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnCanValidateChanged(bool Value)
        {
            if (Value && ValidateHandler != null)
            {
                IsValid = ValidateHandler.Validate(Text, BrowseMode);
            }
            else IsValid = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDialogOpened()
        {
            DialogOpened?.Invoke(this, new EventArgs());

            var Path = string.Empty;
            if (Dialog.Show(out Path, BrowseTitle, BrowseMode, null, Text))
                Text = Path;
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
        public void Browse()
        {
            OnDialogOpened();
        }

        ICommand browseCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowseCommand
        {
            get
            {
                browseCommand = browseCommand ?? new RelayCommand(Browse, () => CanBrowse);
                return browseCommand;
            }
        }

        #endregion
    }
}
