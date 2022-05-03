using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class EnumMenuItem : MenuItem
    {
        protected readonly Handle handle = false;

        protected readonly ObservableCollection<BaseCheckable<Enum>> items = new();

        //...

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Type), typeof(EnumMenuItem), new FrameworkPropertyMetadata(null, OnTypeChanged));
        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<EnumMenuItem>().OnTypeChanged(new(e));

        public static readonly DependencyProperty TypeVisibilityProperty = DependencyProperty.Register(nameof(TypeVisibility), typeof(Appearance), typeof(EnumMenuItem), new FrameworkPropertyMetadata(Appearance.Visible, OnTypeVisibilityChanged));
        public Appearance TypeVisibility
        {
            get => (Appearance)GetValue(TypeVisibilityProperty);
            set => SetValue(TypeVisibilityProperty, value);
        }
        static void OnTypeVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<EnumMenuItem>().OnTypeVisibilityChanged(new(e));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Enum), typeof(EnumMenuItem), new FrameworkPropertyMetadata(default(Enum), OnValueChanged));
        public Enum Value
        {
            get => (Enum)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<EnumMenuItem>().OnValueChanged(new(e));

        //...

        public EnumMenuItem() : base()
        {
            SetCurrentValue(ItemsSourceProperty, items);
            this.RegisterHandler(OnLoaded, OnUnloaded);
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
                    IsChecked = Compare(Value, i)
                };
                items.Add(result);
            });

            Subscribe();
        }

        //...

        void Subscribe()
            => items.ForEach(i => Subscribe(i));

        void Unsubscribe()
            => items.ForEach(i => Unsubscribe(i));

        //...

        void OnChecked(object sender, EventArgs e) => OnChecked(sender as BaseCheckable<Enum>);

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue?.Equals(items) != true)
                throw new NotSupportedException();

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        //...

        protected virtual bool Compare(Enum a, Enum b) => a?.Equals(b) == true;

        protected virtual void OnChecked(BaseCheckable<Enum> input)
        {
            handle.SafeInvoke(() =>
            {
                SetCurrentValue(ValueProperty, input.Value);
                foreach (var j in items)
                {
                    if (j != input)
                        j.IsChecked = false;
                }
            });
        }

        protected virtual void OnLoaded()
            => Subscribe();

        protected virtual void Subscribe(BaseCheckable<Enum> input)
            => input.Checked += OnChecked;

        protected virtual void OnTypeChanged(Value<Type> input)
            => Update();

        protected virtual void OnTypeVisibilityChanged(Value<Appearance> input)
            => Update();

        protected virtual void OnUnloaded()
            => Unsubscribe();

        protected virtual void Unsubscribe(BaseCheckable<Enum> input)
            => input.Checked -= OnChecked;

        protected virtual void OnValueChanged(Value<Enum> input)
            => handle.SafeInvoke(() => items.ForEach(i => i.IsChecked = Compare(input.New, i.Value)));
    }
}