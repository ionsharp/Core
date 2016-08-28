using Imagin.Common;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public partial class FileBox : UserControl
    {
        #region DependencyProperties
        
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

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static DependencyProperty ShowPasswordProperty = DependencyProperty.Register("ShowPassword", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #region FileBox

        public FileBox()
        {
            InitializeComponent();
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            string[] Paths;
            if (this.Type == FileSystemEntry.Folder)
            {
                if (DialogProvider.Show(out Paths, "Browse...", DialogProvider.Type.Folder, DialogProvider.Mode.None, null, this.Text))
                    this.Text = Paths[0];
            } else if (this.Type == FileSystemEntry.File)
            {
                if (DialogProvider.Show(out Paths, "Browse...", DialogProvider.Type.Open, DialogProvider.Mode.Single, null, this.Text))
                    this.Text = Paths[0];
            }
        }

        #endregion
    }
}
