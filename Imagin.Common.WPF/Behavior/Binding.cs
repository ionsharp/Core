using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class BindingBehavior : Behavior<DependencyObject>
    {
        public DependencyProperty DataProperty { get; set; }

        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(nameof(Binding), typeof(Binding), typeof(BindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public Binding Binding
        {
            get => (Binding)GetValue(BindingProperty);
            set => SetValue(BindingProperty, value);
        }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(DependencyProperty), typeof(BindingBehavior), new FrameworkPropertyMetadata(null, OnPropertyChanged));
        public DependencyProperty Property
        {
            get => (DependencyProperty)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        static void OnPropertyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<BindingBehavior>().OnPropertyChanged();

        bool updated = false;

        void Update()
        {
            if (!updated)
            {
                if (Binding is not null && Property is not null)
                {
                    AssociatedObject.Bind(Property, Binding);
                    updated = true;
                }
            }
        }

        protected virtual void OnPropertyChanged()
        {
            if (AssociatedObject is not null)
                Update();
        }

        protected override void OnAttached()
        {
            Update();
        }
    }
}