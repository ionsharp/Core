using Imagin.Common.Scheduling;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public partial class RepeatBox : UserControl
    {
        public static DependencyProperty RepeatOptionsProperty = DependencyProperty.Register("RepeatOptions", typeof(RepeatOptions), typeof(RepeatBox), new FrameworkPropertyMetadata(default(RepeatOptions), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public RepeatOptions RepeatOptions
        {
            get
            {
                return (RepeatOptions)GetValue(RepeatOptionsProperty);
            }
            set
            {
                SetValue(RepeatOptionsProperty, value);
            }
        }

        public RepeatBox()
        {
            InitializeComponent();
        }
    }
}
