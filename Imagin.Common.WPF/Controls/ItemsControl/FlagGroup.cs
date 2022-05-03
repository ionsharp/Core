using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FlagGroup : ItemsControl
    {
        readonly Handle handle = false;

        readonly ObservableCollection<BaseCheckable<Enum>> items = new();

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Type), typeof(FlagGroup), new FrameworkPropertyMetadata(null, Update));
        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeVisibilityProperty = DependencyProperty.Register(nameof(TypeVisibility), typeof(Appearance), typeof(FlagGroup), new FrameworkPropertyMetadata(Appearance.Visible, Update));
        public Appearance TypeVisibility
        {
            get => (Appearance)GetValue(TypeVisibilityProperty);
            set => SetValue(TypeVisibilityProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Enum), typeof(FlagGroup), new FrameworkPropertyMetadata(default(Enum), OnValueChanged));
        public Enum Value
        {
            get => (Enum)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<FlagGroup>().OnValueChanged(new(e));

        public FlagGroup() : base()
        {
            SetCurrentValue(ItemsSourceProperty, items);
            this.RegisterHandler(i => Subscribe(), i => Unsubscribe());
        }

        //...

        void Update()
        {
            Unsubscribe();

            items.Clear();
            Type?.GetEnumValues(TypeVisibility).ForEach(i =>
            {
                var result = new BaseCheckable<Enum>(i)
                {
                    IsChecked = Value?.HasFlag(i) ?? false
                };
                items.Add(result);
            });

            Subscribe();
        }

        static void Update(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<FlagGroup>().Update();

        //...

        void Subscribe()
        {
            items.ForEach(i =>
            {
                i.Checked
                    += OnChecked;
                i.Unchecked
                    += OnUnchecked;
            });
        }

        void Unsubscribe()
        {
            items.ForEach(i =>
            {
                i.Checked
                    -= OnChecked;
                i.Unchecked
                    -= OnUnchecked;
            });
        }

        //...

        void OnChecked(object sender, EventArgs e)
        {
            handle.SafeInvoke(() =>
            {
                if (sender is BaseCheckable<Enum> i)
                {
                    if (!Value.HasFlag(i.Value))
                        SetCurrentValue(ValueProperty, Value.AddFlag(i.Value));
                }
            });
        }

        void OnUnchecked(object sender, EventArgs e)
        {
            handle.SafeInvoke(() =>
            {
                if (sender is BaseCheckable<Enum> i)
                {
                    if (Value.HasFlag(i.Value))
                        SetCurrentValue(ValueProperty, Value.RemoveFlag(i.Value));
                }
            });
        }

        //...

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue?.Equals(items) != true)
                throw new NotSupportedException();

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected virtual void OnValueChanged(Value<Enum> input)
            => handle.SafeInvoke(() => items.ForEach(i => i.IsChecked = input.New?.HasFlag(i.Value) == true));
    }
}