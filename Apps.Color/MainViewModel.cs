using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Color
{
    public class MainViewModel : MainViewModel<MainWindow>, IFrameworkReference
    {
        public static readonly ReferenceKey<ColorControl> ColorControlReferenceKey = new();

        Document activeDocument = null;
        public Document ActiveDocument
        {
            get => activeDocument;
            set => this.Change(ref activeDocument, value);
        }

        public DocumentCollection Documents => Get.Current<Options>().Documents;

        PanelCollection panels = null;
        public PanelCollection Panels
        {
            get => panels;
            private set => this.Change(ref panels, value);
        }

        //...

        public MainViewModel() : base() 
        {
            NewCommand.Execute(ColorModels.RGB);
        }

        //...

        void IFrameworkReference.SetReference(IFrameworkKey key, FrameworkElement element)
        {
            if (key == ColorControlReferenceKey)
            {
                Panels = (element as ColorControl).Panels;
                Panels.Add(new LogPanel(Get.Current<App>().Log));
                Panels.Add(new NotificationsPanel(Get.Current<App>().Notifications));
                Panels.Add(new ThemePanel());
            }
        }

        ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ??= new RelayCommand(() => Documents.Remove(ActiveDocument), () => ActiveDocument != null);

        ICommand closeAllCommand;
        public ICommand CloseAllCommand => closeAllCommand ??= new RelayCommand(() => Documents.Clear(), () => Documents.Count > 0);

        ICommand colorCommand;
        public ICommand ColorCommand => colorCommand ??= new RelayCommand<System.Windows.Media.Color>(i => (ActiveDocument as ColorDocument).Color.ActualColor = i, i => (ActiveDocument as ColorDocument)?.Color != null);

        ICommand newCommand;
        public ICommand NewCommand => newCommand ??= new RelayCommand<ColorModels>(i => Documents.Add(new ColorDocument(Colors.White, i)));
    }
}