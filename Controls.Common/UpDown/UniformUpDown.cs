using System.Windows;

namespace Imagin.Controls.Common
{
    public abstract class UniformUpDown<T> : UpDown<T>
    {
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(UpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        #region UniformUpDown

        public UniformUpDown() : base()
        {
        }

        #endregion
    }
}
