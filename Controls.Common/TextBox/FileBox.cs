using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public class FileBox : AdvancedTextBox
    {
        #region DependencyProperties

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
            FileBox FileBox = (FileBox)Object;
            if (FileBox.ValidatePath)
                FileBox.Validate();
        }

        public static DependencyProperty ButtonPositionProperty = DependencyProperty.Register("ButtonPosition", typeof(HorizontalAlignment), typeof(FileBox), new FrameworkPropertyMetadata(HorizontalAlignment.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public HorizontalAlignment ButtonPosition
        {
            get
            {
                return (HorizontalAlignment)GetValue(ButtonPositionProperty);
            }
            set
            {
                SetValue(ButtonPositionProperty, value);
            }
        }

        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(FileSystemEntry), typeof(FileBox), new FrameworkPropertyMetadata(FileSystemEntry.Folder, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FileSystemEntry Type
        {
            get
            {
                return (FileSystemEntry)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        public static DependencyProperty CanBrowseProperty = DependencyProperty.Register("CanBrowse", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region FileBox

        public FileBox()
        {
            this.DefaultStyleKey = typeof(FileBox);
            this.IsReadOnly = true;
        }

        public void Validate()
        {
            this.PathExists = System.IO.Directory.Exists(this.Text);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Template.FindName("PART_BrowseButton", this).As<ImageButton>().Click += this.OnClick;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (this.ValidatePath)
                this.Validate();
        }

        void OnClick(object sender, RoutedEventArgs e)
        {
            string[] Paths;
            if (DialogProvider.Show(out Paths, "Browse...", this.Type == FileSystemEntry.Folder ? DialogProvider.Type.Folder : DialogProvider.Type.Open, DialogProvider.Mode.Single, null, this.Text))
                this.Text = Paths[0];
        }

        #endregion
    }
}
