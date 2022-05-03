using System.Windows;
using System.Windows.Controls;

namespace Imagin.Apps.Paint
{
    public enum GroupView
    {
        Grid,
        List,
        One
    }

    public class GroupControl : Control
    {
        public static readonly DependencyProperty PanelProperty = DependencyProperty.Register(nameof(Panel), typeof(object), typeof(GroupControl), new FrameworkPropertyMetadata(null));
        public object Panel
        {
            get => (object)GetValue(PanelProperty);
            set => SetValue(PanelProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(GroupControl), new FrameworkPropertyMetadata(null));
        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(GroupControl), new FrameworkPropertyMetadata(null));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectedGroupProperty = DependencyProperty.Register(nameof(SelectedGroup), typeof(object), typeof(GroupControl), new FrameworkPropertyMetadata(null));
        public object SelectedGroup
        {
            get => (object)GetValue(SelectedGroupProperty);
            set => SetValue(SelectedGroupProperty, value);
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(object), typeof(GroupControl), new FrameworkPropertyMetadata(null));
        public object SelectedIndex
        {
            get => (object)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(nameof(View), typeof(GroupView), typeof(GroupControl), new FrameworkPropertyMetadata(GroupView.Grid));
        public GroupView View
        {
            get => (GroupView)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }
    }
}