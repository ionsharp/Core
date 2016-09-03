using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;

namespace Imagin.Controls.Common
{
    public class FileBox : AdvancedTextBox
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

        #endregion

        #region FileBox

        public FileBox()
        {
            this.DefaultStyleKey = typeof(FileBox);
            this.IsReadOnly = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Template.FindName("PART_BrowseButton", this).As<ImageButton>().Click += this.OnClick;
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
