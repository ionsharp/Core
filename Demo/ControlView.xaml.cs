using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.NET.Demo
{
    [ContentProperty("Controls")]
    public partial class ControlView : UserControl
    {
        public static DependencyProperty ControlsProperty = DependencyProperty.Register("Controls", typeof(ObservableCollection<Control>), typeof(ControlView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ObservableCollection<Control> Controls
        {
            get
            {
                return (ObservableCollection<Control>)GetValue(ControlsProperty);
            }
            set
            {
                SetValue(ControlsProperty, value);
            }
        }

        public static DependencyProperty SelectedControlProperty = DependencyProperty.Register("SelectedControl", typeof(Control), typeof(ControlView), new FrameworkPropertyMetadata(default(Control), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Control SelectedControl
        {
            get
            {
                return (Control)GetValue(SelectedControlProperty);
            }
            set
            {
                SetValue(SelectedControlProperty, value);
            }
        }
        
        public ControlView() : base()
        {
            SetCurrentValue(ControlsProperty, new ObservableCollection<Control>());
            InitializeComponent();
        }
    }
}
