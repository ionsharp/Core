using System.Windows;

namespace Imagin.Common.Controls
{
    public class TreeViewCheckBoxColumn : TreeViewBindingColumn
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TreeViewCheckBoxColumn), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty IsThreeStateProperty = DependencyProperty.Register(nameof(IsThreeState), typeof(bool), typeof(TreeViewCheckBoxColumn), new FrameworkPropertyMetadata(false));
        public bool IsThreeState
        {
            get => (bool)GetValue(IsThreeStateProperty);
            set => SetValue(IsThreeStateProperty, value);
        }

        public TreeViewCheckBoxColumn() : base() { }
    }
}