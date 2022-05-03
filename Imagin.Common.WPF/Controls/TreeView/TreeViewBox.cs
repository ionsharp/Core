using GongSolutions.Wpf.DragDrop;
using Imagin.Common.Converters;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    public class TreeViewBox : TreeView
    {
        public static readonly DependencyProperty DropHandlerProperty = DependencyProperty.Register(nameof(DropHandler), typeof(IDropTarget), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public IDropTarget DropHandler
        {
            get => (IDropTarget)GetValue(DropHandlerProperty);
            set => SetValue(DropHandlerProperty, value);
        }

        public static readonly DependencyProperty IsMenuOpenProperty = DependencyProperty.Register(nameof(IsMenuOpen), typeof(bool), typeof(TreeViewBox), new FrameworkPropertyMetadata(false));
        public bool IsMenuOpen
        {
            get => (bool)GetValue(IsMenuOpenProperty);
            set => SetValue(IsMenuOpenProperty, value);
        }
        
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TreeViewBox), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty MenuAnimationProperty = DependencyProperty.Register(nameof(MenuAnimation), typeof(PopupAnimation), typeof(TreeViewBox), new FrameworkPropertyMetadata(PopupAnimation.Fade));
        public PopupAnimation MenuAnimation
        {
            get => (PopupAnimation)GetValue(MenuAnimationProperty);
            set => SetValue(MenuAnimationProperty, value);
        }

        public static readonly DependencyProperty MenuHeightProperty = DependencyProperty.Register(nameof(MenuHeight), typeof(DoubleRange), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleRangeTypeConverter))]
        public DoubleRange MenuHeight
        {
            get => (DoubleRange)GetValue(MenuHeightProperty);
            set => SetValue(MenuHeightProperty, value);
        }

        public static readonly DependencyProperty MenuPlacementProperty = DependencyProperty.Register(nameof(MenuPlacement), typeof(PlacementMode), typeof(TreeViewBox), new FrameworkPropertyMetadata(PlacementMode.Bottom));
        public PlacementMode MenuPlacement
        {
            get => (PlacementMode)GetValue(MenuPlacementProperty);
            set => SetValue(MenuPlacementProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(object), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public object Placeholder
        {
            get => (object)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty PlaceholderTemplateProperty = DependencyProperty.Register(nameof(PlaceholderTemplate), typeof(DataTemplate), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplate PlaceholderTemplate
        {
            get => (DataTemplate)GetValue(PlaceholderTemplateProperty);
            set => SetValue(PlaceholderTemplateProperty, value);
        }

        public static readonly DependencyProperty PlaceholderTemplateSelectorProperty = DependencyProperty.Register(nameof(PlaceholderTemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector PlaceholderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(PlaceholderTemplateSelectorProperty);
            set => SetValue(PlaceholderTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register(nameof(StaysOpen), typeof(bool), typeof(TreeViewBox), new FrameworkPropertyMetadata(true));
        public bool StaysOpen
        {
            get => (bool)GetValue(StaysOpenProperty);
            set => SetValue(StaysOpenProperty, value);
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.Register(nameof(SelectedItemTemplate), typeof(DataTemplate), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplate SelectedItemTemplate
        {
            get => (DataTemplate)GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectedItemTemplateSelectorProperty = DependencyProperty.Register(nameof(SelectedItemTemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector SelectedItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(SelectedItemTemplateSelectorProperty);
            set => SetValue(SelectedItemTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty SelectionTemplateProperty = DependencyProperty.Register(nameof(SelectionTemplate), typeof(DataTemplate), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplate SelectionTemplate
        {
            get => (DataTemplate)GetValue(SelectionTemplateProperty);
            set => SetValue(SelectionTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectionTemplateSelectorProperty = DependencyProperty.Register(nameof(SelectionTemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewBox), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector SelectionTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(SelectionTemplateSelectorProperty);
            set => SetValue(SelectionTemplateSelectorProperty, value);
        }

        public TreeViewBox() : base() => SetCurrentValue(DropHandlerProperty, new DefaultDropHandler(this));
    }
}