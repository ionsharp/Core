using System.Windows;

namespace Imagin.Common
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
            => _property = DependencyProperty.Register(name, typeof(TProperty), typeof(TOwner), metadata);

        DependencyProperty _property;
        /// <summary>
        /// 
        /// </summary>
        public DependencyProperty Property => _property;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public TProperty Get(TOwner owner) => (TProperty)owner.GetValue(_property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        public void Set(TOwner owner, TProperty value) => owner.SetValue(_property, value);
    }
}
