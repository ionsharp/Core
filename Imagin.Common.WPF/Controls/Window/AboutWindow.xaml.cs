using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class AboutWindow : Window
    {
        public AboutWindow() : base()
        {
            XWindow.SetFooterButtons(this, new Buttons(this, Buttons.Done));
            InitializeComponent();
        }
    }
}