using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    public partial class NewWindow : Window
    {
        static readonly DependencyPropertyKey DocumentPresetKey = DependencyProperty.RegisterReadOnly(nameof(DocumentPreset), typeof(DocumentPreset), typeof(NewWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty DocumentPresetProperty = DocumentPresetKey.DependencyProperty;
        public DocumentPreset DocumentPreset
        {
            get => (DocumentPreset)GetValue(DocumentPresetProperty);
            private set => SetValue(DocumentPresetKey, value);
        }

        public static readonly DependencyProperty SelectedPresetProperty = DependencyProperty.Register(nameof(SelectedPreset), typeof(DocumentPreset), typeof(NewWindow), new FrameworkPropertyMetadata(null));
        public DocumentPreset SelectedPreset
        {
            get => (DocumentPreset)GetValue(SelectedPresetProperty);
            set => SetValue(SelectedPresetProperty, value);
        }

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(GraphicUnit), typeof(NewWindow), new FrameworkPropertyMetadata(null));
        public GraphicUnit Unit
        {
            get => (GraphicUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public NewWindow() : base()
        {
            DocumentPreset = new("Untitled".Translate());
            InitializeComponent();
        }

        void OnPresetChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectedPreset != null)
                DocumentPreset = SelectedPreset;
        }

        ICommand deletePresetCommand;
        public ICommand DeletePresetCommand => deletePresetCommand ??= new RelayCommand(() => Get.Current<Options>().DocumentPresets.Remove(DocumentPreset), () => DocumentPreset != null);

        ICommand savePresetCommand;
        public ICommand SavePresetCommand => savePresetCommand ??= new RelayCommand(() => Get.Current<Options>().DocumentPresets.Add(DocumentPreset.Clone() as DocumentPreset), () => DocumentPreset != null);
    }
}