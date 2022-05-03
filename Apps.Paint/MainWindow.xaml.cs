using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Windows.Controls;

namespace Imagin.Apps.Paint
{
    public partial class MainWindow : Common.Controls.MainWindow
    {
        public MainWindow() : base() => InitializeComponent();

        void OnDoubleClick(object sender, EventArgs<object> e)
            => _ = Get.Current<MainViewModel>().OpenAsync((e.Value as Item)?.Path);

        void OnBrushSelected(object sender, SelectionChangedEventArgs e)
        {
            var brushTool = Get.Current<MainViewModel>().SelectedTool as BrushTool;
            if (sender is ComboBox box)
                brushTool.Brush = box.SelectedItem.As<Brush>().Clone();
        }
    }
}