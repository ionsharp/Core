using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FontStyleBox : ComboBox
    {
        public FontStyleBox() : base() => SetCurrentValue(ItemsSourceProperty, new ObservableCollection<FontStyle>
        {
            FontStyles.Italic,
            FontStyles.Normal,
            FontStyles.Oblique
        });
    }
}