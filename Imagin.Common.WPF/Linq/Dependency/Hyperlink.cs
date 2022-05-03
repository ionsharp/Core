using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Hyperlink))]
    public static class XHyperlink
    {
        public static readonly DependencyProperty IsExternalProperty = DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(XHyperlink), new FrameworkPropertyMetadata(false));
        public static bool GetIsExternal(Hyperlink i)
            => (bool)i.GetValue(IsExternalProperty);
        public static void SetIsExternal(Hyperlink i, bool input)
            => i.SetValue(IsExternalProperty, input);

        static XHyperlink()
        {
            EventManager.RegisterClassHandler(typeof(Hyperlink), Hyperlink.RequestNavigateEvent,
                new RequestNavigateEventHandler(OnRequestNavigate), true);
        }

        static void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                if (GetIsExternal(link))
                {
                    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                    e.Handled = true;
                }
            }
        }
    }
}