using System.Windows;

namespace Imagin.Common
{
    public class GenericDependencyProperty
    {
        protected DependencyProperty property;
        public DependencyProperty Property => property;
    }

    public class DependencyProperty<Value, Owner> : GenericDependencyProperty where Owner : DependencyObject
    {
        public DependencyProperty(string name, PropertyMetadata metadata = null) => property = DependencyProperty.Register(name, typeof(Value), typeof(Owner), metadata);

        public Value Get(Owner owner) => (Value)owner.GetValue(property);

        public void Set(Owner owner, Value value) => owner.SetValue(property, value);
    }
}