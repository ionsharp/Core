using Imagin.Common.Analytics;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    [ValueConversion(typeof(TreeViewItem), typeof(Thickness))]
    public class TreeViewItemMarginConverter : Converter<TreeViewItem, Thickness>
    {
        public static TreeViewItemMarginConverter Default { get; private set; } = new TreeViewItemMarginConverter();
        TreeViewItemMarginConverter() { }

        protected override ConverterValue<Thickness> ConvertTo(ConverterData<TreeViewItem> input)
        {
            double depth = input.Value.GetDepth();
            return new Thickness(depth * input.ActualParameter.Double(), 0, 0, 0);
        }

        protected override ConverterValue<TreeViewItem> ConvertBack(ConverterData<Thickness> input) => Nothing.Do;
    }
}