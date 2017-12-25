using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PropertyView : ContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(PropertyModel), typeof(PropertyView), new FrameworkPropertyMetadata(default(PropertyModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public PropertyModel Property
        {
            get
            {
                return (PropertyModel)GetValue(PropertyProperty);
            }
            set
            {
                SetValue(PropertyProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyView()
        {
            InitializeComponent();
        }
    }
}
