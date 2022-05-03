using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Button))]
    public static class XButton
    {
        #region Result

        public static readonly DependencyProperty ResultProperty = DependencyProperty.RegisterAttached("Result", typeof(int), typeof(XButton), new FrameworkPropertyMetadata(-1));
        public static int GetResult(Button i) => (int)i.GetValue(ResultProperty);
        public static void SetResult(Button i, int value) => i.SetValue(ResultProperty, value);

        #endregion
    }
}