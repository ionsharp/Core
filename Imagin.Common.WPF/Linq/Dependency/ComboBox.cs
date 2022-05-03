using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ComboBox))]
    public static class XComboBox
    {
        public static readonly ResourceKey<ToggleButton> ToggleButtonStyleKey = new();

        #region Properties

        #region Flags

        public static readonly DependencyProperty FlagsProperty = DependencyProperty.RegisterAttached("Flags", typeof(object), typeof(XComboBox), new FrameworkPropertyMetadata(null, OnFlagsChanged));
        public static object GetFlags(ComboBox i) => i.GetValue(FlagsProperty);
        public static void SetFlags(ComboBox i, object input) => i.SetValue(FlagsProperty, input);
        
        #endregion

        #region (private) HandleSelectedItems

        static readonly DependencyProperty HandleSelectedItemsProperty = DependencyProperty.RegisterAttached("HandleSelectedItems", typeof(Handle), typeof(XComboBox), new FrameworkPropertyMetadata(null));
        static Handle GetHandleSelectedItems(this ComboBox i) => i.GetValueOrSetDefault<Handle>(HandleSelectedItemsProperty, () => false);

        #endregion

        #region MenuAnimation

        public static readonly DependencyProperty MenuAnimationProperty = DependencyProperty.RegisterAttached("MenuAnimation", typeof(PopupAnimation), typeof(XComboBox), new FrameworkPropertyMetadata(PopupAnimation.Fade));
        public static PopupAnimation GetMenuAnimation(ComboBox i) => (PopupAnimation)i.GetValue(MenuAnimationProperty);
        public static void SetMenuAnimation(ComboBox i, PopupAnimation input) => i.SetValue(MenuAnimationProperty, input);

        #endregion

        #region MenuPlacement

        public static readonly DependencyProperty MenuPlacementProperty = DependencyProperty.RegisterAttached("MenuPlacement", typeof(PlacementMode), typeof(XComboBox), new FrameworkPropertyMetadata(PlacementMode.Bottom));
        public static PlacementMode GetMenuPlacement(ComboBox i) => (PlacementMode)i.GetValue(MenuPlacementProperty);
        public static void SetMenuPlacement(ComboBox i, PlacementMode input) => i.SetValue(MenuPlacementProperty, input);

        #endregion

        #region MinDropDownHeight

        public static readonly DependencyProperty MinDropDownHeightProperty = DependencyProperty.RegisterAttached("MinDropDownHeight", typeof(double), typeof(XComboBox), new FrameworkPropertyMetadata(64.0));
        public static double GetMinDropDownHeight(ComboBox i) => (double)i.GetValue(MinDropDownHeightProperty);
        public static void SetMinDropDownHeight(ComboBox i, double input) => i.SetValue(MinDropDownHeightProperty, input);

        #endregion

        #region (readonly) IsSelectionEmpty

        static readonly DependencyPropertyKey IsSelectionEmptyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionEmpty", typeof(bool), typeof(XComboBox), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsSelectionEmptyProperty = IsSelectionEmptyKey.DependencyProperty;
        public static bool GetIsSelectionEmpty(ComboBox i) => (bool)i.GetValue(IsSelectionEmptyProperty);
        static void SetIsSelectionEmpty(ComboBox i, bool input) => i.SetValue(IsSelectionEmptyKey, input);

        #endregion

        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(XComboBox), new FrameworkPropertyMetadata(string.Empty));
        public static string GetPlaceholder(ComboBox i) => (string)i.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(ComboBox i, string input) => i.SetValue(PlaceholderProperty, input);

        #endregion

        #region PlaceholderTemplate

        public static readonly DependencyProperty PlaceholderTemplateProperty = DependencyProperty.RegisterAttached("PlaceholderTemplate", typeof(DataTemplate), typeof(XComboBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetPlaceholderTemplate(ComboBox i) => (DataTemplate)i.GetValue(PlaceholderTemplateProperty);
        public static void SetPlaceholderTemplate(ComboBox i, DataTemplate input) => i.SetValue(PlaceholderTemplateProperty, input);

        #endregion

        #region PlaceholderTemplateSelector

        public static readonly DependencyProperty PlaceholderTemplateSelectorProperty = DependencyProperty.RegisterAttached("PlaceholderTemplateSelector", typeof(DataTemplateSelector), typeof(XComboBox), new FrameworkPropertyMetadata(null));
        public static DataTemplateSelector GetPlaceholderTemplateSelector(ComboBox i) => (DataTemplateSelector)i.GetValue(PlaceholderTemplateSelectorProperty);
        public static void SetPlaceholderTemplateSelector(ComboBox i, DataTemplateSelector input) => i.SetValue(PlaceholderTemplateSelectorProperty, input);

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(Imagin.Common.Controls.SelectionMode), typeof(XComboBox), new FrameworkPropertyMetadata(Controls.SelectionMode.SingleOrNone, OnSelectionModeChanged));
        public static Controls.SelectionMode GetSelectionMode(ComboBox i) => (Controls.SelectionMode)i.GetValue(SelectionModeProperty);
        public static void SetSelectionMode(ComboBox i,  Controls.SelectionMode input) => i.SetValue(SelectionModeProperty, input);
        static void OnSelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ComboBox box)
            {
                if ((Controls.SelectionMode)e.NewValue == Controls.SelectionMode.Single)
                {
                }
            }
        }

        #endregion

        #region (readonly) SelectedItems

        static readonly DependencyPropertyKey SelectedItemsKey = DependencyProperty.RegisterAttachedReadOnly("SelectedItems", typeof(ICollectionChanged), typeof(XComboBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsKey.DependencyProperty;
        public static ICollectionChanged GetSelectedItems(ComboBox i) => i.GetValueOrSetDefault(SelectedItemsKey, () => new ObservableCollection<object>());

        #endregion

        #region SelectedItemTemplate

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.RegisterAttached("SelectedItemTemplate", typeof(DataTemplate), typeof(XComboBox), new FrameworkPropertyMetadata(default(DataTemplate)));
        public static DataTemplate GetSelectedItemTemplate(ComboBox i) => (DataTemplate)i.GetValue(SelectedItemTemplateProperty);
        public static void SetSelectedItemTemplate(ComboBox i, DataTemplate input) => i.SetValue(SelectedItemTemplateProperty, input);

        #endregion

        #region SelectedItemTemplateSelector

        public static readonly DependencyProperty SelectedItemTemplateSelectorProperty = DependencyProperty.RegisterAttached("SelectedItemTemplateSelector", typeof(DataTemplateSelector), typeof(XComboBox), new FrameworkPropertyMetadata(default(DataTemplateSelector)));
        public static DataTemplateSelector GetSelectedItemTemplateSelector(ComboBox i) => (DataTemplateSelector)i.GetValue(SelectedItemTemplateSelectorProperty);
        public static void SetSelectedItemTemplateSelector(ComboBox i, DataTemplateSelector input) => i.SetValue(SelectedItemTemplateSelectorProperty, input);

        #endregion

        #region SelectionButton

        public static readonly DependencyProperty SelectionButtonProperty = DependencyProperty.RegisterAttached("SelectionButton", typeof(MouseButton), typeof(XComboBox), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetSelectionButton(ComboBox i) => (MouseButton)i.GetValue(SelectionButtonProperty);
        public static void SetSelectionButton(ComboBox i, MouseButton input) => i.SetValue(SelectionButtonProperty, input);

        #endregion

        #region SelectionButtonState

        public static readonly DependencyProperty SelectionButtonStateProperty = DependencyProperty.RegisterAttached("SelectionButtonState", typeof(MouseButtonState), typeof(XComboBox), new FrameworkPropertyMetadata(MouseButtonState.Released));
        public static MouseButtonState GetSelectionButtonState(ComboBox i) => (MouseButtonState)i.GetValue(SelectionButtonStateProperty);
        public static void SetSelectionButtonState(ComboBox i, MouseButtonState input) => i.SetValue(SelectionButtonStateProperty, input);

        #endregion

        #region SelectionModifier

        public static readonly DependencyProperty SelectionModifierProperty = DependencyProperty.RegisterAttached("SelectionModifier", typeof(ModifierKeys), typeof(XComboBox), new FrameworkPropertyMetadata(ModifierKeys.None));
        public static ModifierKeys GetSelectionModifier(ComboBox i) => (ModifierKeys)i.GetValue(SelectionModifierProperty);
        public static void SetSelectionModifier(ComboBox i, ModifierKeys input) => i.SetValue(SelectionModifierProperty, input);

        #endregion

        #region StaysOpen

        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.RegisterAttached("StaysOpen", typeof(bool), typeof(XComboBox), new FrameworkPropertyMetadata(false));
        public static bool GetStaysOpen(ComboBox i) => (bool)i.GetValue(StaysOpenProperty);
        public static void SetStaysOpen(ComboBox i, bool input) => i.SetValue(StaysOpenProperty, input);

        #endregion

        #endregion

        #region XComboBox

        static readonly Dictionary<ICollectionChanged, ComboBox> Selections = new();

        //...

        static XComboBox()
        {
            /*
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.LoadedEvent,
                new RoutedEventHandler(OnLoaded), true);
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.UnloadedEvent,
                new RoutedEventHandler(OnUnloaded), true);
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.SelectionChangedEvent,
                new SelectionChangedEventHandler(OnSelectionChanged), true);
            */
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.PreviewMouseDownEvent,
                new MouseButtonEventHandler(OnPreviewMouseDownUp), true);
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.PreviewMouseUpEvent,
                new MouseButtonEventHandler(OnPreviewMouseDownUp), true);
        }

        //...

        static Enum ConvertFlags(this ComboBox input)
        {
            if (GetSelectionMode(input) == Controls.SelectionMode.Multiple)
            {
                var selection = GetSelectedItems(input);

                Enum result = default;
                if (selection.Count > 0)
                {
                    result = selection.First<Enum>();
                    for (var i = 1; i < selection.Count; i++)
                        result = result.AddFlag((Enum)selection[i]);
                }
                else
                {
                    result = (Enum)GetFlags(input);
                    foreach (Enum i in input.Items)
                        result = result.RemoveFlag(i);
                }
                return result;
            }
            return null;
        }

        //...

        /// <summary>
        /// Occurs when <see cref="FlagsProperty"/> changes.
        /// </summary>
        static void OnFlagsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ComboBox box)
            {
                GetHandleSelectedItems(box).SafeInvoke(() =>
                {
                    if (GetSelectionMode(box) == Controls.SelectionMode.Multiple)
                    {
                        if (e.NewValue is Enum newValue)
                        {
                            var selection = GetSelectedItems(box);
                            foreach (Enum i in box.Items)
                            {
                                if (newValue.HasFlag(i))
                                {
                                    //(1) XComboBox.SelectedItems
                                    if (!selection.Contains(i))
                                        selection.Add(i);

                                    //(2) XComboBoxItem.IsSelected
                                    box.SelectInternal(i, true);
                                }
                                else
                                {
                                    //(1) XComboBox.SelectedItems
                                    selection.Remove(i);
                                    //(2) XComboBoxItem.IsSelected
                                    box.SelectInternal(i, false);
                                }
                            }
                            //(3) ComboBox.SelectedItem
                            if (selection.Count > 0)
                                box.SetCurrentValue(ComboBox.SelectedItemProperty, selection[0]);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Occurs upon loading.
        /// </summary>
        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox box)
            {
                SetIsSelectionEmpty(box, GetSelectedItems(box).Count == 0);

                Selections.Add(GetSelectedItems(box), box);
                GetSelectedItems(box).CollectionChanged += OnSelectedItemsChanged;
            }
        }

        /// <summary>
        /// Occurs when the mouse is pressed and released.
        /// </summary>
        static void OnPreviewMouseDownUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ComboBox box)
            {
                if (box.CanSelect(e))
                {
                    if ((e.OriginalSource as ComboBoxItem ?? e.OriginalSource.FindParent<ComboBoxItem>()) is ComboBoxItem container)
                    {
                        box.GetHandleSelectedItems().Invoke(() =>
                        {
                            var item = box.GetItem(container);

                            var selection = GetSelectedItems(box);
                            switch (GetSelectionMode(box))
                            {
                                case Controls.SelectionMode.Multiple:
                                    if (GetSelectionModifier(box) == ModifierKeys.None || GetSelectionModifier(box).Pressed())
                                    {
                                        //(1) XComboBoxItem.IsSelected
                                        container.SelectInverse();
                                        if (container.isSelected())
                                        {
                                            //(2) ComboBox.SelectedItem
                                            box.SetCurrentValue(ComboBox.SelectedItemProperty, item);

                                            //(3) XComboBox.SelectedItems
                                            if (!selection.Contains(item))
                                                selection.Add(item);
                                        }
                                        else
                                        {
                                            //(2) ComboBox.SelectedItem
                                            if (ReferenceEquals(container, box.ItemContainerGenerator.ContainerFromItem(box.SelectedItem)))
                                                box.SetCurrentValue(ComboBox.SelectedItemProperty, null);

                                            //(3) XComboBox.SelectedItems
                                            selection.Remove(item);
                                        }

                                        //(4) XComboBox.Flags
                                        SetFlags(box, ConvertFlags(box));
                                    }
                                    break;

                                case Controls.SelectionMode.Single:
                                    //(1) XComboBoxItem.IsSelected
                                    container.Select(true);
                                    box.UnselectInternal(container, i => selection.Remove(i));

                                    //(2) XComboBox.SelectedItems
                                    if (!selection.Contains(item))
                                        selection.Add(item);

                                    //(3) ComboBox.SelectedItem
                                    box.SetCurrentValue(ComboBox.SelectedItemProperty, item);
                                    break;

                                case Controls.SelectionMode.SingleOrNone:
                                    //(1) XComboBoxItem.IsSelected
                                    container.SelectInverse();
                                    if (container.isSelected())
                                    {
                                        box.UnselectInternal(container, i => selection.Remove(i));
                                        //(2) XComboBox.SelectedItems
                                        if (!selection.Contains(item))
                                            selection.Add(item);

                                        //(3) ComboBox.SelectedItem
                                        box.SetCurrentValue(ComboBox.SelectedItemProperty, item);
                                    }
                                    else
                                    {
                                        //(2) XComboBox.SelectedItems
                                        selection.Remove(item);
                                        //(3) ComboBox.SelectedItem
                                        if (ReferenceEquals(container, box.ItemContainerGenerator.ContainerFromItem(box.SelectedItem)))
                                            box.SetCurrentValue(ComboBox.SelectedItemProperty, null);
                                    }
                                    break;
                            }
                        });
                        e.Handled = true;
                        if (!GetStaysOpen(box))
                            box.SetCurrentValue(ComboBox.IsDropDownOpenProperty, false);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="SelectedItemsProperty"/> changes.
        /// </summary>
        static void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ICollectionChanged selection)
                SetIsSelectionEmpty(Selections[selection], selection.Count == 0);

            if (Selections[sender as ICollectionChanged] is ComboBox box)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        box.GetHandleSelectedItems().SafeInvoke(() =>
                        {
                            //(1) XComboBoxItem.IsSelected
                            box.SelectInternal(e.NewItems[0], true);
                            //(2) ComboBox.SelectedItem
                            box.SetCurrentValue(ComboBox.SelectedItemProperty, e.NewItems[0]);
                            //(3) XComboBox.Flags
                            SetFlags(box, box.ConvertFlags());
                        });
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        box.GetHandleSelectedItems().SafeInvoke(() =>
                        {
                            //(1) XComboBoxItem.IsSelected
                            box.SelectInternal(e.OldItems[0], false);
                            //(2) ComboBox.SelectedItem
                            if (ReferenceEquals(box.GetContainer(box.SelectedItem), box.GetContainer(e.OldItems[0])))
                                box.SetCurrentValue(ComboBox.SelectedItemProperty, null);

                            //(3) XComboBox.Flags
                            SetFlags(box, box.ConvertFlags());
                        });
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="XComboBoxItem.IsSelectedProperty"/> changes.
        /// </summary>
        internal static void OnSelected(this ComboBox input, ComboBoxItem container) => GetHandleSelectedItems(input).SafeInvoke(() =>
        {
            var selection = GetSelectedItems(input);

            var item = input.GetItem(container);
            if (container.isSelected())
            {
                //(1) XComboBox.SelectedItems
                if (!selection.Contains(item))
                    selection.Add(item);

                //(2) ComboBox.SelectedItem
                input.SetCurrentValue(ComboBox.SelectedItemProperty, item);
            }
            else
            {
                //(1) XComboBox.SelectedItems
                selection.Remove(item);
                //(2) ComboBox.SelectedItem
                if (ReferenceEquals(input.GetContainer(input.SelectedItem), container))
                    input.SetCurrentValue(ComboBox.SelectedItemProperty, null);
            }

            //(3) XComboBox.Flags
            SetFlags(input, input.ConvertFlags());
        });

        /// <summary>
        /// Occurs when <see cref="Selector.SelectedIndex"/> or <see cref="Selector.SelectedItem"/> changes.
        /// </summary>
        static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box)
            {            
                GetHandleSelectedItems(box).SafeInvoke(() =>
                {
                    //(1) XComboBox.SelectedItems
                    var selection = GetSelectedItems(box);
                    if (!selection.Contains(box.SelectedItem))
                        selection.Add(box.SelectedItem);

                    //(2) XComboBoxItem.IsSelected
                    box.SelectInternal(box.SelectedItem, true);
                    box.UnselectInternal(box.GetContainer(box.SelectedItem), i => selection.Remove(i));

                    //(3) XComboBox.Flags
                    SetFlags(box, box.ConvertFlags());
                });
            }
        }

        static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox box)
            {
                GetSelectedItems(box).CollectionChanged -= OnSelectedItemsChanged;
                Selections.Remove(GetSelectedItems(box));
            }
        }

        #endregion

        #region Methods

        static bool CanSelect(this ComboBox input, MouseButtonEventArgs e)
        {
            return GetSelectionButton(input) switch
            {
                MouseButton.Left => e.LeftButton == GetSelectionButtonState(input),
                MouseButton.Middle => e.MiddleButton == GetSelectionButtonState(input),
                MouseButton.Right => e.RightButton == GetSelectionButtonState(input),
                MouseButton.XButton1 => e.XButton1 == GetSelectionButtonState(input),
                MouseButton.XButton2 => e.XButton2 == GetSelectionButtonState(input),
                _ => throw new NotSupportedException(),
            };
        }

        static void SelectInternal(this ComboBox input, object item, bool select)
        {
            if (input.ItemContainerGenerator.ContainerFromItem(item) is ComboBoxItem i)
                XComboBoxItem.Select(i, select);
        }

        static void UnselectInternal(this ComboBox input, ComboBoxItem except, Action<object> action = null)
        {
            for (var i = input.Items.Count - 1; i >= 0; i--)
            {
                if (input.ItemContainerGenerator.ContainerFromItem(input.Items[i]) is ComboBoxItem j)
                {
                    if (!ReferenceEquals(except, j))
                    {
                        j.Select(false);
                        action?.Invoke(input.Items[i]);
                    }
                }
            }
        }

        //...

        public static ComboBoxItem GetContainer(this ComboBox input, object item) => input.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;

        public static object GetItem(this ComboBox input, ComboBoxItem item) => input.ItemContainerGenerator.ItemFromContainer(item);

        public static void ClearSelection(this ComboBox input)
        {
            foreach (var i in input.Items)
            {
                if (input.ItemContainerGenerator.ContainerFromItem(i) is ComboBoxItem item)
                    XComboBoxItem.SetIsSelected(item, false);
            }
        }

        #endregion
    }
}