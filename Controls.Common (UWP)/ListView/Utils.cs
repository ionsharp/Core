using System;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace Imagin.Controls.Common
{
    public class Utils
    {
        public static DoubleAnimation CreateDoubleAnimation(DependencyObject target, string propertyPath, EasingFunctionBase easingFunc, double to, double duration)
        {
            var anim = new DoubleAnimation
            {
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = easingFunc
            };
            Storyboard.SetTarget(anim, target);
            Storyboard.SetTargetProperty(anim, propertyPath);
            return anim;
        }

        public static T FindVisualChild<T>(DependencyObject obj, string childName) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var visualChild = child as T;
                if (visualChild != null && visualChild.Name == childName) return visualChild;

                var childOfChild = FindVisualChild<T>(child, childName);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }

        public static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(obj);
            var visualParent = parent as T;
            if (visualParent != null) return visualParent;

            var parentOfParent = FindVisualParent<T>(parent);
            return parentOfParent;
        }

        public static bool IsOnMobile => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

        public static async void SetBase64ToImage(BitmapSource imageSource, string base64Str)
        {
            var imgBytes = Convert.FromBase64String(base64Str);
            var ms = new InMemoryRandomAccessStream();
            var dw = new DataWriter(ms);
            dw.WriteBytes(imgBytes);
            await dw.StoreAsync();
            ms.Seek(0);
            imageSource.SetSource(ms);
        }
    }
}
