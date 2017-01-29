using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TOwner"></typeparam>
    public class DependencyProperty<TProperty, TOwner> where TOwner : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="metadata"></param>
        public DependencyProperty(string name, PropertyMetadata metadata = null)
        {
            property = DependencyProperty.Register(name, typeof(TProperty), typeof(TOwner), metadata);
        }

        DependencyProperty property;
        /// <summary>
        /// 
        /// </summary>
        public DependencyProperty Property
        {
            get
            {
                return property;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public TProperty Get(TOwner owner)
        {
            return (TProperty)owner.GetValue(property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        public void Set(TOwner owner, TProperty value)
        {
            owner.SetValue(property, value);
        }
    }
}
