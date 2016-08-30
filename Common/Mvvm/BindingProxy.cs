using System.Windows;

namespace Imagin.Common.Mvvm
{
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
        public object Data
        {
            get
            {
                return (object)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
