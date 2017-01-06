using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_BrowseButton", Type = typeof(MaskedButton))]
    public class FileBox : AdvancedTextBox
    {
        #region DependencyProperties

        public static DependencyProperty ButtonPositionProperty = DependencyProperty.Register("ButtonPosition", typeof(LeftRight), typeof(FileBox), new FrameworkPropertyMetadata(LeftRight.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        
        public static DependencyProperty ButtonToolTipProperty = DependencyProperty.Register("ButtonToolTip", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata("Click to browse...", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(FileBox), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DialogProviderModeProperty = DependencyProperty.Register("DialogProviderMode", typeof(DialogProviderMode), typeof(FileBox), new FrameworkPropertyMetadata(DialogProviderMode.OpenFolder, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DialogProviderMode DialogProviderMode
        {
            get
            {
                return (DialogProviderMode)GetValue(DialogProviderModeProperty);
            }
            set
            {
                SetValue(DialogProviderModeProperty, value);
            }
        }
        
        public static DependencyProperty DialogProviderTitleProperty = DependencyProperty.Register("DialogProviderTitle", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata("Browse...", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string DialogProviderTitle
        {
            get
            {
                return (string)GetValue(DialogProviderTitleProperty);
            }
            set
            {
                SetValue(DialogProviderTitleProperty, value);
            }
        }

        public static DependencyProperty IsDialogEnabledProperty = DependencyProperty.Register("IsDialogEnabled", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsDialogEnabled
        {
            get
            {
                return (bool)GetValue(IsDialogEnabledProperty);
            }
            set
            {
                SetValue(IsDialogEnabledProperty, value);
            }
        }

        public static DependencyProperty PathExistsProperty = DependencyProperty.Register("PathExists", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool PathExists
        {
            get
            {
                return (bool)GetValue(PathExistsProperty);
            }
            private set
            {
                SetValue(PathExistsProperty, value);
            }
        }
        
        public static DependencyProperty ValidatePathProperty = DependencyProperty.Register("ValidatePath", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValidatePathChanged));
        public bool ValidatePath
        {
            get
            {
                return (bool)GetValue(ValidatePathProperty);
            }
            set
            {
                SetValue(ValidatePathProperty, value);
            }
        }
        static void OnValidatePathChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((FileBox)Object).OnValidatePathChanged((bool)e.NewValue);
        }

        public static DependencyProperty ValidatePathToolTipProperty = DependencyProperty.Register("ValidatePathToolTip", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string ValidatePathToolTip
        {
            get
            {
                return (string)GetValue(ValidatePathToolTipProperty);
            }
            set
            {
                SetValue(ValidatePathToolTipProperty, value);
            }
        }

        #endregion

        #region FileBox

        public FileBox()
        {
            this.DefaultStyleKey = typeof(FileBox);
        }

        #endregion

        #region Methods

        void OnClick(object sender, RoutedEventArgs e)
        {
            string[] Paths;
            if (DialogProvider.Show(out Paths, this.DialogProviderTitle, this.DialogProviderMode, DialogProviderSelectionMode.Single, null, this.Text))
                this.Text = Paths[0];
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.OnValidatePathChanged(this.ValidatePath);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Template.FindName("PART_Button", this).As<MaskedButton>().Click += this.OnClick;
        }

        protected virtual void OnValidatePathChanged(bool Value)
        {
            if (Value)
                this.PathExists = this.Text.DirectoryExists();
        }

        public void Browse()
        {
            this.OnClick(this, new RoutedEventArgs());
        }

        #endregion
    }
}
