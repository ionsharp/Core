namespace Imagin.Common.Mvvm
{
    /// <summary>
    /// Base implementatation for defining view models.
    /// </summary>
    /// <typeparam name="T">The type of an object to model.</typeparam>
    public class ViewModel<T> : BindableObject
    {
        T _object = default(T);
        /// <summary>
        /// An object to model.
        /// </summary>
        public T Object
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
                OnPropertyChanged("Object");
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        public ViewModel() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        /// <param name="_object">The object to model.</param>
        public ViewModel(T _object) : base()
        {
            Object = _object;
        }
    }
}
