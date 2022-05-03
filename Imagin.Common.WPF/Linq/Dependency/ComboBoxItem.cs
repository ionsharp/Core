using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ComboBoxItem))]
    public static class XComboBoxItem
    {
        public static readonly ResourceKey<Separator> SeparatorStyleKey = new();

        #region Properties

        #region IsSelected

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(XComboBoxItem), new FrameworkPropertyMetadata(false, OnIsSelectedChanged));
        public static bool GetIsSelected(ComboBoxItem i) => (bool)i.GetValue(IsSelectedProperty);
        public static void SetIsSelected(ComboBoxItem i, bool input) => i.SetValue(IsSelectedProperty, input);
        static void OnIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ComboBoxItem item)
                item.GetParent().OnSelected(item);
        }

        #endregion

        #region (private) Parent

        static readonly DependencyProperty ParentProperty = DependencyProperty.RegisterAttached("Parent", typeof(ComboBox), typeof(XComboBoxItem), new FrameworkPropertyMetadata(null));
        static ComboBox GetParent(this ComboBoxItem i) => i.GetValueOrSetDefault(ParentProperty, () => i.FindParent<ComboBox>());

        #endregion

        #endregion

        #region Methods

        internal static bool isSelected(this ComboBoxItem input) => GetIsSelected(input);

        public static void Select(this ComboBoxItem i, bool input) => SetIsSelected(i, input);

        public static void SelectInverse(this ComboBoxItem i) => SetIsSelected(i, !GetIsSelected(i));

        #endregion
    }
}