using Imagin.Common.Events;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class PasswordBox : AdvancedTextBox
    {
        #region Properties

        bool IgnorePasswordChange = false;

        bool IgnoreTextChange = false;

        System.Windows.Controls.PasswordBox PART_PasswordBox
        {
            get; set;
        }

        public event EventHandler<ObjectEventArgs> Entered;

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
            this.PART_PasswordBox.PreviewKeyUp += OnPreviewKeyUp;

            this.IgnorePasswordChange = true;
            this.PART_PasswordBox.Password = this.Text;

            this.Template.FindName("PART_EnterButton", this).As<MaskedButton>().Click += (s, e) => this.OnEntered();
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Enter)
                this.OnEntered();
        }

        #endregion

        #region Virtual

        protected virtual void OnEntered()
        {
            if (this.Entered != null)
                this.Entered(this, new ObjectEventArgs(this.Text));
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

        void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.OnEntered();
        }

        #endregion

        #endregion
    }
}
