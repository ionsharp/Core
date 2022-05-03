using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class TreeViewComboBoxColumn : TreeViewBindingColumn
    {
        public static readonly DependencyProperty DataTypeProperty = DependencyProperty.Register(nameof(DataType), typeof(Type), typeof(TreeViewComboBoxColumn), new FrameworkPropertyMetadata(null));
        public Type DataType
        {
            get => (Type)GetValue(DataTypeProperty);
            set => SetValue(DataTypeProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TreeViewComboBoxColumn), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public TreeViewComboBoxColumn() : base() { }
    }
}