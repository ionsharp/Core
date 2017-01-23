using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_BrowseButton", Type = typeof(MaskedButton))]
    [TemplatePart(Name = "PART_Checked", Type = typeof(UIElement))]
    public class FileBox : TextBoxExt
    {
        #region DependencyProperties

        UIElement PART_Checked { get; set; } = null;

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

        #endregion

        #region FileBox

        public FileBox()
        {
            DefaultStyleKey = typeof(FileBox);
        }

        #endregion

        #region Methods

        void OnClick(object sender, RoutedEventArgs e)
        {
            string[] Paths;
            if (DialogProvider.Show(out Paths, DialogProviderTitle, DialogProviderMode, DialogProviderSelectionMode.Single, null, Text))
                Text = Paths[0];
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            OnValidatePathChanged(ValidatePath);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Template.FindName("PART_Button", this).As<MaskedButton>().Click += this.OnClick;

            PART_Checked = Template.FindName("PART_Checked", this) as UIElement;
        }

        protected virtual void OnValidatePathChanged(bool Value)
        {
            if (PART_Checked != null)
            {
                var Result = false;

                if (Value)
                {
                    switch (DialogProviderMode)
                    {
                        case DialogProviderMode.OpenFile:
                            Result = Text.FileExists();
                            break;
                        case DialogProviderMode.OpenFolder:
                            Result = Text.DirectoryExists();
                            break;
                    }
                }

                PART_Checked.Visibility = Result.ToVisibility();
            }
        }

        public void Browse()
        {
            OnClick(this, new RoutedEventArgs());
        }

        #endregion
    }
}
