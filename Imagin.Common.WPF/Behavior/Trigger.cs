using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Imagin.Common.Behavior
{
    public class Setter
    {
        public DependencyProperty Property { get; set; }

        public object Value { get; set; }
    }

    public class SetterCollection : List<Setter>
    {
        public SetterCollection() : base() { }
    }

    [ContentProperty(nameof(Setters))]
    public class TriggerBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(nameof(Binding), typeof(object), typeof(TriggerBehavior), new FrameworkPropertyMetadata(null, OnBindingChanged));
        public object Binding
        {
            get => (object)GetValue(BindingProperty);
            set => SetValue(BindingProperty, value);
        }
        static void OnBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TriggerBehavior behavior)
                behavior.Evaluate();
        }

        public static readonly DependencyProperty SettersProperty = DependencyProperty.Register(nameof(Setters), typeof(SetterCollection), typeof(TriggerBehavior), new FrameworkPropertyMetadata(null, OnSettersChanged));
        public SetterCollection Setters
        {
            get => (SetterCollection)GetValue(SettersProperty);
            set => SetValue(SettersProperty, value);
        }
        static void OnSettersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TriggerBehavior behavior)
                behavior.Evaluate();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(TriggerBehavior), new FrameworkPropertyMetadata(null));
        public object Value
        {
            get => (object)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public TriggerBehavior() : base()
        {
            SetCurrentValue(SettersProperty, new SetterCollection());
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            Evaluate();
        }

        void Evaluate()
        {
            if (AssociatedObject != null)
            {
                if (Binding?.Equals(Value) == true)
                {
                    if (Setters?.Count > 0)
                    {
                        foreach (var i in Setters)
                            AssociatedObject.SetCurrentValue(i.Property, i.Value);
                    }
                }
            }
        }
    }
}