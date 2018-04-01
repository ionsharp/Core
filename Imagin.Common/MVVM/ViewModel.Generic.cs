namespace Imagin.Common.Mvvm
{
    /// <summary>
    /// Base implementatation for defining view models.
    /// </summary>
    /// <typeparam name="T">The type of an object to model.</typeparam>
    public class ViewModel<T> : ViewModel
    {
        T view = default(T);
        /// <summary>
        /// The view to model.
        /// </summary>
        public T View
        {
            get => view;
            set => Property.Set(this, ref view, value, () => View);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{T}"/> class.
        /// </summary>
        protected ViewModel() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{T}"/> class.
        /// </summary>
        /// <param name="view">The view to model.</param>
        public ViewModel(T view) : this() => View = view;
    }
}
