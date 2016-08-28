using Imagin.Common.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public partial class PasswordBox : UserControl
    {
        #region Properties

        bool IgnoreChange = false;

        public event EventHandler<ObjectEventArgs> Entered;

        #region Dependency 

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));
        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }
        private static void OnPasswordChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox PasswordBox = (PasswordBox)Object;
            if (PasswordBox.IgnoreChange)
            {
                PasswordBox.IgnoreChange = false;
                return;
            }
            PasswordBox.PART_Password.Password = PasswordBox.Password;
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

        public static DependencyProperty ShowHintProperty = DependencyProperty.Register("ShowHint", typeof(bool), typeof(PasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowHint
        {
            get
            {
                return (bool)GetValue(ShowHintProperty);
            }
            set
            {
                SetValue(ShowHintProperty, value);
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

        #endregion

        #endregion

        #region PasswordBox

        public PasswordBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        void OnEntered()
        {
            if (this.Entered != null)
                this.Entered(this, new ObjectEventArgs(this.Password));
        }

        #region Events

        void OnEntered(object sender, RoutedEventArgs e)
        {
            this.OnEntered();
        }

        void Password_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.OnEntered();
                return;
            }
            this.IgnoreChange = true;
            this.Password = (sender as System.Windows.Controls.PasswordBox).Password;
        }

        #endregion

        #endregion
    }
}
