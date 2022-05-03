using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(MenuItem))]
    public static class XMenuItem
    {
        #region Equals

        public static readonly DependencyProperty EqualsProperty = DependencyProperty.RegisterAttached("Equals", typeof(object), typeof(XMenuItem), new FrameworkPropertyMetadata(null, OnEqualsChanged));
        public static object GetEquals(MenuItem i) => (object)i.GetValue(EqualsProperty);
        public static void SetEquals(MenuItem i, object input) => i.SetValue(EqualsProperty, input);
        static void OnEqualsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MenuItem item)
                Equals_Update(item);
        }

        static void Equals_Update(MenuItem item)
            => GetEqualsHandle(item).SafeInvoke(() => item.IsChecked = GetEquals(item) == GetEqualsParameter(item));

        #endregion

        #region (private) EqualsHandle

        static readonly DependencyProperty EqualsHandleProperty = DependencyProperty.RegisterAttached("EqualsHandle", typeof(Handle), typeof(XMenuItem), new FrameworkPropertyMetadata(null));
        static Handle GetEqualsHandle(MenuItem i) => i.GetValueOrSetDefault(EqualsHandleProperty, () => new Handle());

        #endregion

        #region EqualsParameter

        public static readonly DependencyProperty EqualsParameterProperty = DependencyProperty.RegisterAttached("EqualsParameter", typeof(object), typeof(XMenuItem), new FrameworkPropertyMetadata(null, OnEqualsParameterChanged));
        public static object GetEqualsParameter(MenuItem i) => (object)i.GetValue(EqualsParameterProperty);
        public static void SetEqualsParameter(MenuItem i, object input) => i.SetValue(EqualsParameterProperty, input);
        static void OnEqualsParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                item.RegisterHandlerAttached(e.NewValue != null, EqualsParameterProperty, i =>
                {
                    i.Checked += EqualsParameter_Checked;
                    Equals_Update(i);
                }, 
                i => i.Checked -= EqualsParameter_Checked);
                Equals_Update(item);
            }
        }

        static void EqualsParameter_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
                GetEqualsHandle(item).SafeInvoke(() => SetEquals(item, GetEqualsParameter(item)));
        }

        #endregion

        #region IconTemplate

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.RegisterAttached("IconTemplate", typeof(DataTemplate), typeof(XMenuItem), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetIconTemplate(FrameworkElement i) => (DataTemplate)i.GetValue(IconTemplateProperty);
        public static void SetIconTemplate(FrameworkElement i, DataTemplate input) => i.SetValue(IconTemplateProperty, input);

        #endregion

        #region IconTemplateSelector

        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.RegisterAttached("IconTemplateSelector", typeof(DataTemplateSelector), typeof(XMenuItem), new FrameworkPropertyMetadata(null));
        public static DataTemplateSelector GetIconTemplateSelector(FrameworkElement i) => (DataTemplateSelector)i.GetValue(IconTemplateSelectorProperty);
        public static void SetIconTemplateSelector(FrameworkElement i, DataTemplateSelector input) => i.SetValue(IconTemplateSelectorProperty, input);

        #endregion

        #region IconVisibility

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.RegisterAttached("IconVisibility", typeof(Visibility), typeof(XMenuItem), new FrameworkPropertyMetadata(Visibility.Visible));
        public static Visibility GetIconVisibility(MenuItem i) => (Visibility)i.GetValue(IconVisibilityProperty);
        public static void SetIconVisibility(MenuItem i, Visibility input) => i.SetValue(IconVisibilityProperty, input);

        #endregion
        
        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(XMenuItem), new FrameworkPropertyMetadata(string.Empty, OnGroupNameChanged));
        public static string GetGroupName(MenuItem i) => (string)i.GetValue(GroupNameProperty);
        public static void SetGroupName(MenuItem i, string input) => i.SetValue(GroupNameProperty, input);
        static void OnGroupNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                item.RegisterHandlerAttached(e.NewValue?.ToString().Empty() == false, GroupNameProperty, i =>
                {
                    i.Checked
                       += GroupName_Checked;
                    i.Click
                        += GroupName_Click;
                    i.PreviewMouseDown
                        += GroupName_PreviewMouseDown;
                }, i =>
                {
                    i.Checked
                       -= GroupName_Checked;
                    i.Click
                        -= GroupName_Click;
                    i.PreviewMouseDown
                        -= GroupName_PreviewMouseDown;
                });
            }
        }

        static void GroupName_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var selectionMode = GetSelectionMode(item);
                if (selectionMode != SelectionMode.Multiple)
                {
                    var parent = item.Parent as ItemsControl;
                    if (parent != null)
                    {
                        foreach (var i in parent.Items)
                        {
                            var j = i as MenuItem ?? parent.GetContainer(i) as MenuItem;
                            if (j != null)
                            {
                                if (!ReferenceEquals(j, item) && GetGroupName(j) == GetGroupName(item))
                                    j.IsChecked = false;
                            }
                        }
                    }
                }
            }
        }

        static void GroupName_Click(object sender, RoutedEventArgs e)
        {
            //Make sure it's still checkable!
            if (sender is MenuItem item)
            {
                if (item.IsChecked && GetSelectionMode(item) == SelectionMode.Single)
                    item.IsCheckable = true;
            }
        }

        static void GroupName_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //If it's already checked, we're about to uncheck it; if one element must always be checked, disable unchecking
            if (sender is MenuItem item)
            {
                if (item.IsChecked && GetSelectionMode(item) == SelectionMode.Single)
                    item.IsCheckable = false;
            }
        }

        #endregion

        #region InputGestureTextTemplate

        public static readonly DependencyProperty InputGestureTextTemplateProperty = DependencyProperty.RegisterAttached("InputGestureTextTemplate", typeof(DataTemplate), typeof(XMenuItem), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetInputGestureTextTemplate(MenuItem i) => (DataTemplate)i.GetValue(InputGestureTextTemplateProperty);
        public static void SetInputGestureTextTemplate(MenuItem i, DataTemplate input) => i.SetValue(InputGestureTextTemplateProperty, input);

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(SelectionMode), typeof(XMenuItem), new FrameworkPropertyMetadata(SelectionMode.Single, OnSelectionModeChanged));
        public static void SetSelectionMode(MenuItem i, SelectionMode input) => i.SetValue(SelectionModeProperty, input);
        public static SelectionMode GetSelectionMode(MenuItem i) => (SelectionMode)i.GetValue(SelectionModeProperty);
        static void OnSelectionModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MenuItem item)
            {            
                //Multiple to single
                if (e.OldValue?.To<SelectionMode>() == SelectionMode.Multiple)
                {
                    var k = false;

                    var parent = item.Parent as ItemsControl;
                    if (parent != null)
                    {
                        foreach (var i in parent.Items)
                        {
                            var j = i as MenuItem ?? parent.GetContainer(i) as MenuItem;
                            if (j != null)
                            {
                                if (GetGroupName(j) == GetGroupName(item))
                                {
                                    if (!k)
                                    {
                                        if (j.IsChecked)
                                        {
                                            k = true;
                                            j.SetCurrentValue(MenuItem.IsCheckedProperty, true);
                                            continue;
                                        }
                                    }
                                    j.SetCurrentValue(MenuItem.IsCheckedProperty, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}