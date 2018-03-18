using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.NET.Demo
{
    [ContentProperty("Controls")]
    public partial class ControlView : UserControl
    {
        public static DependencyProperty ControlsProperty = DependencyProperty.Register(nameof(Controls), typeof(ObservableCollection<Control>), typeof(ControlView), new FrameworkPropertyMetadata(null));
        public ObservableCollection<Control> Controls
        {
            get => (ObservableCollection<Control>)GetValue(ControlsProperty);
            set => SetValue(ControlsProperty, value);
        }

        public static DependencyProperty SelectedControlProperty = DependencyProperty.Register(nameof(SelectedControl), typeof(Control), typeof(ControlView), new FrameworkPropertyMetadata(default(Control)));
        public Control SelectedControl
        {
            get => (Control)GetValue(SelectedControlProperty);
            set => SetValue(SelectedControlProperty, value);
        }
        
        public ControlView() : base()
        {
            SetCurrentValue(ControlsProperty, new ObservableCollection<Control>());
            InitializeComponent();
        }
    }
}