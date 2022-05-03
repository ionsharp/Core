using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FontWeightBox : ComboBox
    {
        public FontWeightBox() : base() => SetCurrentValue(ItemsSourceProperty, new ObservableCollection<FontWeight>
        {
            FontWeights.Black,
            FontWeights.Bold,
            FontWeights.DemiBold,
            FontWeights.ExtraBlack,
            FontWeights.ExtraBold,
            FontWeights.ExtraLight,
            FontWeights.Heavy,
            FontWeights.Light,
            FontWeights.Medium,
            FontWeights.Normal,
            FontWeights.Regular,
            FontWeights.SemiBold,
            FontWeights.Thin,
            FontWeights.UltraBlack,
            FontWeights.UltraBold,
            FontWeights.UltraLight
        });
    }
}