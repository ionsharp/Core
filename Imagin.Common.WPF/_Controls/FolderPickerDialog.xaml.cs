using System.Collections.Generic;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StoragePickerDialog : BasicWindow
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedPathsProperty = DependencyProperty.Register("CheckedPaths", typeof(IList<string>), typeof(StoragePickerDialog), new FrameworkPropertyMetadata(default(IList<string>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public IList<string> CheckedPaths
        {
            get
            {
                return (IList<string>)GetValue(CheckedPathsProperty);
            }
            private set
            {
                SetValue(CheckedPathsProperty, value);
            }
        }

        /// <summary>
        /// CheckedPaths
        /// </summary>
        public StoragePickerDialog()
        {
            InitializeComponent();
            SetCurrentValue(CheckedPathsProperty, PART_Picker.CheckedPaths);
        }
    }
}
