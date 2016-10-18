using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_EnterButton", Type = typeof(MaskedButton))]
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(PasswordBox))]
    public class PasswordBox : AdvancedTextBox
    {
        #region Properties

        bool IgnorePasswordChange = false;

        bool IgnoreTextChange = false;

        System.Windows.Controls.PasswordBox PART_PasswordBox
        {
            get; set;
        }

        #region Dependency 

        public static DependencyProperty CanEnterProperty = DependencyProperty.Register("CanEnter", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanEnter
        {
            get
            {
                return (bool)GetValue(CanEnterProperty);
            }
            set
            {
                SetValue(CanEnterProperty, value);
            }
        }

        public static DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Hint
        {
            get
            {
                return (string)GetValue(HintProperty);
            }
            set
            {
                SetValue(HintProperty, value);
            }
        }

        public static DependencyProperty ShowEnterButtonProperty = DependencyProperty.Register("ShowEnterButton", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowEnterButton
        {
            get
            {
                return (bool)GetValue(ShowEnterButtonProperty);
            }
            set
            {
                SetValue(ShowEnterButtonProperty, value);
            }
        }

        public static DependencyProperty ShowPasswordProperty = DependencyProperty.Register("ShowPassword", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPassword
        {
            get
            {
                return (bool)GetValue(ShowPasswordProperty);
            }
            set
            {
                SetValue(ShowPasswordProperty, value);
            }
        }
        
        #endregion

        #endregion

        #region PasswordBox

        public PasswordBox()
        {
            this.DefaultStyleKey = typeof(PasswordBox);
        }

        #endregion

        #region Methods

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_PasswordBox = this.Template.FindName("PART_PasswordBox", this).As<System.Windows.Controls.PasswordBox>();
            this.PART_PasswordBox.PasswordChanged += OnPasswordChanged;

            this.IgnorePasswordChange = true;
            this.PART_PasswordBox.Password = this.Text;

            this.Template.FindName("PART_EnterButton", this).As<MaskedButton>().Click += (s, e) => this.OnEntered(null);
        }

        /// <summary>
        /// When this.Text changes, set System.Windows.Controls.PasswordBox.Password.
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (this.PART_PasswordBox == null)
                return;
            if (this.IgnoreTextChange)
            {
                this.IgnoreTextChange = false;
                return;
            }
            this.IgnorePasswordChange = true;
            this.PART_PasswordBox.Password = this.Text;
        }

        #endregion

        #region Events

        /// <summary>
        /// When System.Windows.Controls.PasswordBox.Password changes, set this.Text.
        /// </summary>
        void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.IgnorePasswordChange)
            {
                this.IgnorePasswordChange = false;
                return;
            }
            this.IgnoreTextChange = true;
            this.Text = sender.As<System.Windows.Controls.PasswordBox>().Password;
        }

        #endregion

        #endregion
    }
}
